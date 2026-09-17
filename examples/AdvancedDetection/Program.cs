using WhichDistroSharp;
using static WhichDistroSharp.WhichDistroSharp;

Distro distro = Detect();

Console.WriteLine($"Detected Distro: {distro}");
Console.WriteLine($"Was Found: {distro.WasFound()}");
Console.WriteLine();

if (distro.WasFound())
{
    IPlatform platform = DetectPlatform();
    Console.WriteLine($"Name: {platform.Name}");
    Console.WriteLine($"ID: {platform.Id}");
    Console.WriteLine($"Version: {platform.VersionId}");
    Console.WriteLine($"Pretty Name: {platform.PrettyName}");
    Console.WriteLine();

    Console.WriteLine($"Is Linux Mint: {distro.IsLinuxmint()}");
    Console.WriteLine($"Is Ubuntu: {distro.IsUbuntu()}");
    Console.WriteLine($"Is Fedora: {distro.IsFedora()}");
    Console.WriteLine();

    IOsRelease? info = OsReleaseInfo.GetInfo(distro);
    if (info != null)
    {
        Console.WriteLine($"OS Release Info:");
        Console.WriteLine($"  Name: {info.NAME}");
        Console.WriteLine($"  Version: {info.VERSION_ID}");
        Console.WriteLine($"  Pretty Name: {info.PRETTY_NAME}");
    }
}
