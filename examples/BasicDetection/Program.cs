using WhichDistroSharp;
using static WhichDistroSharp.WhichDistroSharp;

Distro distro = Detect();

Console.WriteLine($"Detected Distro: {distro}");
Console.WriteLine($"Was Found: {distro.WasFound()}");
