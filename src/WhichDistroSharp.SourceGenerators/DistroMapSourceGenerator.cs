namespace WhichDistroSharp.SourceGenerators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

#pragma warning disable RS1035
[Generator]
public class DistroMapSourceGenerator : ISourceGenerator
#pragma warning restore RS1035
{
    private static readonly string[] OsReleaseSpecFields =
    [
        "ID", "NAME", "VERSION_ID", "PRETTY_NAME", "HOME_URL", "BUG_REPORT_URL",
        "VERSION", "CPE_NAME", "BUILD_ID",
        "ID_LIKE", "VENDOR_NAME", "VENDOR_URL", "SUPPORT_URL",
        "PRIVACY_POLICY_URL", "VERSION_CODENAME", "ANSI_COLOR"
    ];

    private static readonly HashSet<string> OsReleaseRequiredFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "ID", "NAME", "VERSION_ID", "PRETTY_NAME", "HOME_URL", "BUG_REPORT_URL"
    };

    public void Initialize(GeneratorInitializationContext context) {}

    public void Execute(GeneratorExecutionContext context)
    {
        var projectPath = GetProjectPath(context);
        if (projectPath == null) {
            ReportDiagnostic(
                context, 
                "DSG002", 
                "DistroMapSourceGenerator",
                "Could not determine project file path.", 
                DiagnosticSeverity.Warning
            );
            return;
        }

        var distrosPath = Path.Combine(projectPath, "Distros");

        if (!Directory.Exists(distrosPath)) {
            ReportDiagnostic(
                context, 
                "DSG003", 
                "DistroMapSourceGenerator",
                $"Distros directory not found at {distrosPath}. Distro generation skipped.",
                DiagnosticSeverity.Warning
            );
            return;
        }

        var distros = ParseDistros(distrosPath, context);

        if (distros.Count == 0) {
            ReportDiagnostic(
                context, 
                "DSG004", 
                "DistroMapSourceGenerator",
                "No distro entries found in Distros directory.", 
                DiagnosticSeverity.Warning
            );
            return;
        }

        var code = GenerateCode(distros);
        context.AddSource("DistroMap.g.cs", SourceText.From(code, Encoding.UTF8));
    }

    private static string? GetProjectPath(GeneratorExecutionContext context)
    {
        var syntaxTree = context.Compilation.SyntaxTrees.FirstOrDefault();
        
        if (syntaxTree?.FilePath == null) { return null; }

        return Path.GetDirectoryName(syntaxTree.FilePath);
    }

    private List<DistroEntry> ParseDistros(string distrosPath, GeneratorExecutionContext context)
    {
        var entries = new List<DistroEntry>();
        var seenEnumNames = new HashSet<string>();
        var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var dir in Directory.GetDirectories(distrosPath))
        {
            var dirName = Path.GetFileName(dir);
            var files = Directory.GetFiles(dir)
                .Where(f => Path.GetFileName(f) != "README.md" && Path.GetFileName(f) != "LICENSE")
                .ToArray();

            if (files.Length == 0) { continue; }

            var osReleaseFile = files.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == dirName)
                                ?? files[0];

            Dictionary<string, string> fields;

            try { fields = ParseOsRelease(osReleaseFile); }

            catch (Exception ex)
            {
                ReportDiagnostic(
                    context, 
                    "DSG005", 
                    "DistroMapSourceGenerator",
                    $"Failed to parse {osReleaseFile}: {ex.Message}", 
                    DiagnosticSeverity.Warning
                );
                continue;
            }

            if (!fields.TryGetValue("ID", out var id)) { continue; }

            if (!seenIds.Add(id)) { continue; }

            var enumName = ToPascalCase(dirName);

            if (!seenEnumNames.Add(enumName)) { continue; }

            entries.Add(
                new DistroEntry(dirName, id, enumName, fields)
            );
        }

        entries.Sort((a, b) => string.Compare(a.EnumName, b.EnumName, StringComparison.Ordinal));
        
        entries.Add(
            new DistroEntry("", "", "Unknown", [])
        );

        return entries;
    }

    private static Dictionary<string, string> ParseOsRelease(string path)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in File.ReadLines(path))
        {
            var eqIndex = line.IndexOf('=');
            if (eqIndex <= 0) { continue; }

            // Handling missing System.Range and System.Index (applies to Net Standard 2.0) 
            #if NET6_0_OR_GREATER
                var key = line[..eqIndex].Trim();
                var value = line[(eqIndex + 1)..].Trim();
            #else 
                var key = line.Substring(0, eqIndex).Trim();
                var value = line.Substring(eqIndex + 1).Trim();
            #endif

            // Handling error CS1503: Argument 1: cannot convert from 'char' to 'string' in Net Standard 2.0
            # if NET6_0_OR_GREATER
                var quoteValue = '"';
            # else
                var quoteValue = "\"";
            #endif
            
            if (value.StartsWith(quoteValue) && value.EndsWith(quoteValue) && value.Length >= 2) {
                #if NET6_0_OR_GREATER
                    value = value[1..^1];
                #else 
                    value = value.Substring(1, value.Length - 2);
                #endif
            }

            result[key] = value;
        }
        return result;
    }

    private static string ToPascalCase(string input)
    {
        var parts = input.Split(['-', '_', ' ', '.', '/', '\\'], StringSplitOptions.RemoveEmptyEntries);
        var result = new StringBuilder();
        foreach (var part in parts)
        {
            if (part.Length == 0) continue;
            result.Append(char.ToUpper(part[0]));
            if (part.Length > 1) result.Append(part.Substring(1));
        }
        return result.ToString();
    }

    private static string GenerateCode(List<DistroEntry> distros)
    {
        var sb = new StringBuilder();

        // Suppressing code generation warnings related to warning CS8669:
        // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        // Auto-generated code requires an explicit '#nullable' directive in source.
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine("namespace WhichDistroSharp;");
        sb.AppendLine();


        var generators = new Action<StringBuilder>[]
        {
            s => GenerateEnum(s, distros),
            s => GenerateInterface(s),
            
            // Previously this was a static/hardcoded DistroMap Dictionary.
            // The current implementation works dynamically using the upstream data from which-distro/os-release
            s => GenerateDistroMap(s, distros),

            // Generating the class info for every mapped distro.
            s => { 
                foreach (var entry in distros) { 
                    if (entry.EnumName == "Unknown") { continue; } 
                    
                    GenerateDistroInfoClass(s, entry); 
                } 
            },
            s => GenerateOsReleaseInfo(s, distros),
            s => GenerateIsExtensions(s, distros),
        };

        for (int i = 0; i < generators.Length; i++)
        {
            generators[i](sb);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static void GenerateEnum(StringBuilder sb, List<DistroEntry> distros)
    {
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Represents a Linux distribution.");
        sb.AppendLine("/// Generated from os-release directory names.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public enum Distro");
        sb.AppendLine("{");
        foreach (var entry in distros)
        {
            sb.AppendLine($"    {entry.EnumName},");
        }
        sb.AppendLine("}");
    }

    private static void GenerateInterface(StringBuilder sb)
    {
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Provides OS-release specification fields.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public interface IOsRelease");
        sb.AppendLine("{");

        foreach (var field in OsReleaseSpecFields)
        {
            var isRequired = OsReleaseRequiredFields.Contains(field);
            var type = isRequired ? "string" : "string?";
            sb.AppendLine($"    {type} {field} {{ get; }}");
        }

        sb.AppendLine("}");
    }

    private static void GenerateDistroMap(StringBuilder sb, List<DistroEntry> distros)
    {
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Maps distro ID strings to <see cref=\"Distro\"/> values.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class DistroMap");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Maps distro ID strings to <see cref=\"Distro\"/> values.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static readonly Dictionary<string, Distro> Map = new(StringComparer.OrdinalIgnoreCase)");
        sb.AppendLine("    {");
        foreach (var entry in distros)
        {
            if (entry.EnumName == "Unknown") continue;
            sb.AppendLine($"        {{ \"{entry.Id}\", Distro.{entry.EnumName} }},");
        }
        sb.AppendLine("    };");
        sb.AppendLine("}");
    }

    private static void GenerateDistroInfoClass(StringBuilder sb, DistroEntry entry)
    {
        sb.AppendLine($"/// <summary>");
        sb.AppendLine($"/// OS-release information for {entry.EnumName}.");
        sb.AppendLine($"/// </summary>");
        sb.AppendLine($"public sealed class {entry.EnumName}Info : IOsRelease");
        sb.AppendLine("{");

        foreach (var field in OsReleaseSpecFields)
        {
            var value = entry.Fields.TryGetValue(field, out var v) ? EscapeString(v) : "";
            sb.AppendLine($"    public string {field} => \"{value}\";");
        }

        sb.AppendLine("}");
    }

    private static void GenerateOsReleaseInfo(StringBuilder sb, List<DistroEntry> distros)
    {
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Provides OS-release information for <see cref=\"Distro\"/> values.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class OsReleaseInfo");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Gets the <see cref=\"IOsRelease\"/> data for the specified distro.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static IOsRelease? GetInfo(Distro distro) => distro switch");
        sb.AppendLine("    {");
        foreach (var entry in distros)
        {
            if (entry.EnumName == "Unknown") continue;
            sb.AppendLine($"        Distro.{entry.EnumName} => new {entry.EnumName}Info(),");
        }
        sb.AppendLine("        _ => null");
        sb.AppendLine("    };");
        sb.AppendLine("}");
    }

    private static void GenerateIsExtensions(StringBuilder sb, List<DistroEntry> distros)
    {
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Provides <c>Is*</c> extension methods for each <see cref=\"Distro\"/> value.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class DistroIsExtensions");
        sb.AppendLine("{");

        foreach (var entry in distros)
        {
            if (entry.EnumName == "Unknown") continue;
            sb.AppendLine($"    public static bool Is{entry.EnumName}(this Distro distro) => distro == Distro.{entry.EnumName};");
        }

        sb.AppendLine("    public static bool WasFound(this Distro distro) => distro != Distro.Unknown;");

        sb.AppendLine("}");
    }

    private static string EscapeString(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    private static void ReportDiagnostic(GeneratorExecutionContext context, string id, string title, string message, DiagnosticSeverity severity)
    {
        var descriptor = new DiagnosticDescriptor(id, title, message, "WhichDistroSharp", severity, true);
        var diagnostic = Diagnostic.Create(descriptor, Location.None);
        context.ReportDiagnostic(diagnostic);
        Debug.Print(message);
    }

    private class DistroEntry
    {
        public string DirName { get; }
        public string Id { get; }
        public string EnumName { get; }
        public Dictionary<string, string> Fields { get; }

        public DistroEntry(string dirName, string id, string enumName, Dictionary<string, string> fields)
        {
            DirName = dirName;
            Id = id;
            EnumName = enumName;
            Fields = fields;
        }
    }
}