namespace WhichDistroSharp.SourceGenerators;


using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;


/// <summary> A very rudamentary source generator to dynamically create Is* extension methods for distro instance(s). </summary>
[Generator]
public class IsDistroSourceGenerator : ISourceGenerator
{
    private const string DistroTypeSymbol = "WhichDistroSharp.Distro";
    public void Initialize(GeneratorInitializationContext context) {}

    /// <summary> Creates a StringBuilder using the provided List of IFieldSymbols. </summary>
    /// <param name="enumValues">A List of IFieldSymbols that were resolved before this function was called.</param>
    /// <returns>The created StringBuilder object.</returns>
    private static StringBuilder CreateBuilder(List<IFieldSymbol> enumValues)
    {
        var sb = new StringBuilder();
        sb.AppendLine("namespace WhichDistroSharp;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Provides <c>Is*</c> extension methods for each <see cref=\"Distro\"/> value.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class DistroIsExtensions");
        sb.AppendLine("{");

        foreach (var field in enumValues) {
            sb.AppendLine($"    public static bool Is{field.Name}(this Distro distro) => distro == Distro.{field.Name};");
        }

        sb.AppendLine("}");
        return sb;
    }

    /// <summary>
    /// If the provided distroType is null, a diagnostic is reported to the provided execution context.
    /// </summary>
    /// <returns>true if distroType was null and execution should stop; otherwise false.</returns>
    /// <param name="distroType">The resolved INamedTypeSymbol object.</param>
    /// <param name="context">The ExecutionContext created and managed by the InitializationContext at compile-time. </param>
    public static bool ManageEmptyDistroType(INamedTypeSymbol? distroType, GeneratorExecutionContext context)
    {
        if (distroType is null)
        {
            var descriptor = new DiagnosticDescriptor(
                "DSG001",
                "IsDistroSourceGenerator",
                "Distro type was not found during in compilation",
                "WhichDistroSharp",
                DiagnosticSeverity.Warning,
                true
            );

            var diagnostic = Diagnostic.Create(descriptor, Location.None);

            context.ReportDiagnostic(diagnostic);
            Debug.Print("Please reference the above diagnostic for IsDistroSourceGenerator.Execute().");
            return true;
        }

        return false;
    }

    public void Execute(GeneratorExecutionContext context)
    {
        INamedTypeSymbol? distroType = context.Compilation.GetTypeByMetadataName(DistroTypeSymbol);

        if (ManageEmptyDistroType(distroType, context)) { return; }

        var enumValues = distroType!.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f => f.IsConst)
            .OrderBy(f => f.Name)
            .ToList();

        if (enumValues.Count == 0) { return; }

        StringBuilder sb = CreateBuilder(enumValues);

        context.AddSource("DistroIsExtensions.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }
}
