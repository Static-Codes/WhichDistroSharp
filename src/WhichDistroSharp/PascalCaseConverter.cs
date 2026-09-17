namespace WhichDistroSharp;

using System.Collections.Generic;
using System.Text;

public static class PascalCaseConverter
{   
    // Handling of non-canonical distro names.
    // This is the only static logic in WhichDistroSharp; the remainder is handled through source-gen.
    private static readonly Dictionary<string, string> KnownTerms = new(StringComparer.OrdinalIgnoreCase)
    {
        { "linuxmint", "LinuxMint" },
    };

    public static string? ToPascalCase(string? input)
    {
        if (input is null) { return null; }

        if (KnownTerms.TryGetValue(input, out var known)) {
            return known;
        }

        var separators = new[] { '-', '_', ' ', '.', '/', '\\' };
        var parts = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var result = new StringBuilder();

        foreach (var part in parts)
        {
            if (part.Length == 0) { continue; }

            result.Append(char.ToUpper(part[0]));

            if (part.Length > 1) { result.Append(part.AsSpan(1)); }
        }

        return result.ToString();
    }
}