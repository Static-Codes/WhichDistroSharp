namespace WhichDistroSharp;

public static class WhichDistroSharp
{
    private static readonly Dictionary<string, Distro> DistroMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "almalinux", Distro.Almalinux },
        { "alpine", Distro.Alpine },
        { "altlinux", Distro.Altlinux },
        { "amzn", Distro.Amzn },
        { "arch", Distro.Arch },
        { "arch32", Distro.Arch32 },
        { "archcraft", Distro.Archcraft },
        { "arkane", Distro.Arkane },
        { "artix", Distro.Artix },
        { "aurora", Distro.Aurora },
        { "azurelinux", Distro.Azurelinux },
        { "bazzite", Distro.Bazzite },
        { "blackarch", Distro.Blackarch },
        { "blendos", Distro.Blendos },
        { "bluefin", Distro.Bluefin },
        { "bodhi", Distro.Bodhi },
        { "buildroot", Distro.Buildroot },
        { "cachyos", Distro.Cachyos },
        { "centos", Distro.Centos },
        { "chimera", Distro.Chimera },
        { "chimeraos", Distro.Chimeraos },
        { "cirros", Distro.Cirros },
        { "clear-linux-os", Distro.ClearLinuxOs },
        { "clearos", Distro.Clearos },
        { "coreos", Distro.Coreos },
        { "cos", Distro.Cos },
        { "cumulus-linux", Distro.CumulusLinux },
        { "debian", Distro.Debian },
        { "deepin", Distro.Deepin },
        { "devuan", Distro.Devuan },
        { "discontinued", Distro.Discontinued },
        { "dragonfly", Distro.Dragonfly },
        { "elementary", Distro.Elementary },
        { "endeavouros", Distro.Endeavouros },
        { "endless", Distro.Endless },
        { "eurolinux", Distro.Eurolinux },
        { "exherbo", Distro.Exherbo },
        { "fedora", Distro.Fedora },
        { "fedoraremixforwsl", Distro.Fedoraremixforwsl },
        { "flatcar", Distro.Flatcar },
        { "freebsd", Distro.Freebsd },
        { "funtoo", Distro.Funtoo },
        { "garuda", Distro.Garuda },
        { "gentoo", Distro.Gentoo },
        { "ghostbsd", Distro.Ghostbsd },
        { "gnoppix", Distro.Gnoppix },
        { "guix", Distro.Guix },
        { "hyperbola", Distro.Hyperbola },
        { "ios_xr", Distro.IosXr },
        { "kali", Distro.Kali },
        { "kaos", Distro.Kaos },
        { "linuxmint", Distro.Linuxmint },
        { "mageia", Distro.Mageia },
        { "manjaro", Distro.Manjaro },
        { "manjaro-arm", Distro.ManjaroArm },
        { "mariner", Distro.Mariner },
        { "miraclelinux", Distro.Miraclelinux },
        { "neon", Distro.Neon },
        { "nexus", Distro.Nexus },
        { "nilrt", Distro.Nilrt },
        { "nixos", Distro.Nixos },
        { "nobara", Distro.Nobara },
        { "novariaos", Distro.Novariaos },
        { "nuros", Distro.Nuros },
        { "ol", Distro.Ol },
        { "omnios", Distro.Omnios },
        // { "openeuler", Distro.OpenEuler },
        { "openmandriva", Distro.Openmandriva },
        { "opensuse", Distro.Opensuse },
        { "opensuse-leap", Distro.OpensuseLeap },
        { "opensuse-tumbleweed", Distro.OpensuseTumbleweed },
        { "openwrt", Distro.Openwrt },
        { "parrot", Distro.Parrot },
        { "pclinuxos", Distro.Pclinuxos },
        { "pengwin", Distro.Pengwin },
        { "photon", Distro.Photon },
        { "pika", Distro.Pika },
        { "pisilinux", Distro.Pisilinux },
        { "pop", Distro.Pop },
        { "postmarketos", Distro.Postmarketos },
        { "puppy", Distro.Puppy },
        { "puppy_fossapup64", Distro.PuppyFossapup64 },
        { "puppy_s15pup32", Distro.PuppyS15pup32 },
        { "puppy_s15pup64", Distro.PuppyS15pup64 },
        { "pureos", Distro.Pureos },
        { "rancheros", Distro.Rancheros },
        { "raspbian", Distro.Raspbian },
        { "rebornos", Distro.Rebornos },
        { "redox-os", Distro.RedoxOs },
        { "rhel", Distro.Rhel },
        { "rocky", Distro.Rocky },
        { "scientific", Distro.Scientific },
        { "slackware", Distro.Slackware },
        { "sled", Distro.Sled },
        { "sles", Distro.Sles },
        { "sles_sap", Distro.SlesSap },
        { "solaris", Distro.Solaris },
        { "solus", Distro.Solus },
        { "steamos", Distro.Steamos },
        { "suse-microos", Distro.SuseMicroos },
        { "sysrescue", Distro.Sysrescue },
        { "tails", Distro.Tails },
        { "tencentos", Distro.Tencentos },
        { "tinycore", Distro.Tinycore },
        { "trisquel", Distro.Trisquel },
        { "ubios", Distro.Ubios },
        { "ubuntu", Distro.Ubuntu },
        { "ubuntu_kylin", Distro.UbuntuKylin },
        { "ultramarine", Distro.Ultramarine },
        { "vanillaos", Distro.Vanillaos },
        { "void", Distro.Void },
        { "wolfi", Distro.Wolfi },
        { "wrlinux", Distro.Wrlinux },
        { "XCP-ng", Distro.XCPNg },
        { "xenenterprise", Distro.Xenenterprise },
        { "zorin", Distro.Zorin },
    };

    public static Distro Detect()
    {
        string? osReleasePath = GetOsReleasePath();
        if (osReleasePath != null && TryParseId(osReleasePath, out Distro distro))
        {
            return distro;
        }
        return Distro.Ubuntu;
    }

    public static IPlatform DetectPlatform()
    {
        string? osReleasePath = GetOsReleasePath();
        if (osReleasePath != null)
        {
            var fields = ParseOsRelease(osReleasePath);
            string id = fields.GetValueOrDefault("ID") ?? "";
            if (DistroMap.TryGetValue(id, out Distro distro)) {
                return new PlatformData(distro, fields);
            }
        }
        #if NET8_0_OR_GREATER
            return new PlatformData(Distro.Unknown, []);

        #elif NET6_0
            return new PlatformData(Distro.Unknown, new());
        
        #endif
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
        return DistroMap.TryGetValue(id, out distro);
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
