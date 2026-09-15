namespace WhichDistroSharp;

public interface IPlatform
{
    string Name { get; }
    string Id { get; }
    string VersionId { get; }
    string PrettyName { get; }
    string HomeUrl { get; }
    string? IdLike { get; }
    string? VendorName { get; }
    string? VendorUrl { get; }
    string? BugReportUrl { get; }
    string? SupportUrl { get; }
    string? PrivacyPolicyUrl { get; }
    string? VersionCodename { get; }
    string? CpeName { get; }
}
