namespace WhichDistroSharp;

public static class WhichDistroSharp
{
    public static Distro Detect()
    {
        string? osReleasePath = GetOsReleasePath();
        if (osReleasePath != null && TryParseId(osReleasePath, out Distro distro)) {
            return distro;
        }
        return Distro.Unknown;
    }

    private static PlatformData GetDefaultPlatformData()
    {
        #if NET6_0
            return new PlatformData(Distro.Unknown, new());
        #elif NET8_0_OR_GREATER
            return new PlatformData(Distro.Unknown, []); 
        #endif
    }

    public static IPlatform DetectPlatform()
    {
        string? osReleasePath = GetOsReleasePath();

        if (osReleasePath == null) {
            return GetDefaultPlatformData();
        }
        
        var fields = ParseOsRelease(osReleasePath);
        string id = fields.GetValueOrDefault("ID") ?? "";

        if (!DistroMap.Map.TryGetValue(id, out Distro distro)) { 
            return GetDefaultPlatformData();
        }
        return new PlatformData(distro, fields); 
    }

    private static string? GetOsReleasePath()
    {
        #if NET6_0
            string[] candidates = { "/etc/os-release", "/usr/lib/os-release" };
        

        #elif NET8_0_OR_GREATER
            string[] candidates = [ "/etc/os-release", "/usr/lib/os-release" ];
        
        #endif
        

        foreach (string path in candidates)
        {
            if (File.Exists(path)) {
                return path;
            }
        }
        return null;
    }

    private static bool TryParseId(string path, out Distro distro)
    {
        var fields = ParseOsRelease(path);
        string id = fields.GetValueOrDefault("ID") ?? "";
        return DistroMap.Map.TryGetValue(id, out distro);
    }

    private static Dictionary<string, string> ParseOsRelease(string path)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string line in File.ReadLines(path))
        {
            int eqIndex = line.IndexOf('=');
            
            if (eqIndex <= 0) { continue; }

            string key = line[..eqIndex].Trim();
            string value = line[(eqIndex + 1)..].Trim();
            
            if (value.StartsWith('"') && value.EndsWith('"') && value.Length >= 2) {
                value = value[1..^1];
            }
            
            result[key] = value;
        }
        return result;
    }
}
