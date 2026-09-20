WhichDistroSharp is a unofficial .NET wrapper for [Which-Distro](https://github.com/which-distro)'s [os-release](https://github.com/which-distro/os-release) archive, allowing for easy platform detection in just one LoC!


## Basic Usage (Detects distro name only)

```csharp
using WhichDistroSharp;
using static WhichDistroSharp.WhichDistroSharp;

// Distro detection
Distro distro = Detect();

Console.WriteLine($"Detected Distro: {distro}");
Console.WriteLine($"Was Found: {distro.WasFound()}");
```

## Advanced Usage (Detects distro and os-release info)

```csharp
using WhichDistroSharp;
using static WhichDistroSharp.WhichDistroSharp;

// Distro detection
Distro distro = Detect();

Console.WriteLine($"Detected Distro: {distro}");
Console.WriteLine($"Was Found: {distro.WasFound()}");
Console.WriteLine();

if (!distro.WasFound()) {
    Console.Error.WriteLine("Failed to detect the linux distro running on the current machine");
    Environment.Exit(1);
}

// Parsing /etc/os-release
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

if (info == null) {
    Console.Error.WriteLine("Failed to detect the os-release information for the distro running on the current machine");
    Environment.Exit(1);
}

Console.WriteLine($"OS Release Info:");
Console.WriteLine($"  Name: {info.NAME}");
Console.WriteLine($"  Version: {info.VERSION_ID}");
Console.WriteLine($"  Pretty Name: {info.PRETTY_NAME}");
```