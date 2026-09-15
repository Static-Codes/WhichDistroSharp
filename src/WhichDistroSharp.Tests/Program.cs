
namespace WhichDistroSharp.Tests;

using static global::WhichDistroSharp.WhichDistroSharp;
public class Program
{
    public static void Main(string[] args) {
        global::WhichDistroSharp.Distro distro = Detect();
        Console.WriteLine($"Distro Found: {distro.WasFound()}");
        Console.WriteLine($"Distro Name: {distro}");
    }
}