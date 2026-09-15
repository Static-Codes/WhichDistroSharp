namespace WhichDistroSharp;

/// <summary> A generic platform interface that will be inherited by each Distro member following source generation. </summary>
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
