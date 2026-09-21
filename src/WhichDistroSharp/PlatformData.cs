namespace WhichDistroSharp;

#if NET6_0
    internal sealed class PlatformData : IPlatform
    {
        private readonly Dictionary<string, string> _fields;

        public PlatformData(Distro distro, Dictionary<string, string> fields)
        {
            Distro = distro;
            _fields = fields ?? new Dictionary<string, string>();
        }

        public Distro Distro { get; }
        public string Name => _fields.GetValueOrDefault("NAME") ?? "";
        public string Id => _fields.GetValueOrDefault("ID") ?? "";
        public string VersionId => _fields.GetValueOrDefault("VERSION_ID") ?? "";
        public string PrettyName => _fields.GetValueOrDefault("PRETTY_NAME") ?? "";
        public string HomeUrl => _fields.GetValueOrDefault("HOME_URL") ?? "";
        public string? IdLike => _fields.GetValueOrDefault("ID_LIKE");
        public string? VendorName => _fields.GetValueOrDefault("VENDOR_NAME");
        public string? VendorUrl => _fields.GetValueOrDefault("VENDOR_URL");
        public string? BugReportUrl => _fields.GetValueOrDefault("BUG_REPORT_URL");
        public string? SupportUrl => _fields.GetValueOrDefault("SUPPORT_URL");
        public string? PrivacyPolicyUrl => _fields.GetValueOrDefault("PRIVACY_POLICY_URL");
        public string? VersionCodename => _fields.GetValueOrDefault("VERSION_CODENAME");
        public string? CpeName => _fields.GetValueOrDefault("CPE_NAME");
    }

#elif NET8_0_OR_GREATER
    internal sealed class PlatformData(Distro distro, Dictionary<string, string> fields) : IPlatform
    {
        private readonly Dictionary<string, string> _fields = fields ?? [];

        public Distro Distro { get; } = distro;
        public string Name => _fields.GetValueOrDefault("NAME") ?? "";
        public string Id => _fields.GetValueOrDefault("ID") ?? "";
        public string VersionId => _fields.GetValueOrDefault("VERSION_ID") ?? "";
        public string PrettyName => _fields.GetValueOrDefault("PRETTY_NAME") ?? "";
        public string HomeUrl => _fields.GetValueOrDefault("HOME_URL") ?? "";
        public string? IdLike => _fields.GetValueOrDefault("ID_LIKE");
        public string? VendorName => _fields.GetValueOrDefault("VENDOR_NAME");
        public string? VendorUrl => _fields.GetValueOrDefault("VENDOR_URL");
        public string? BugReportUrl => _fields.GetValueOrDefault("BUG_REPORT_URL");
        public string? SupportUrl => _fields.GetValueOrDefault("SUPPORT_URL");
        public string? PrivacyPolicyUrl => _fields.GetValueOrDefault("PRIVACY_POLICY_URL");
        public string? VersionCodename => _fields.GetValueOrDefault("VERSION_CODENAME");
        public string? CpeName => _fields.GetValueOrDefault("CPE_NAME");
    }

#endif