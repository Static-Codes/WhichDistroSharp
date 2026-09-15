namespace WhichDistroSharp;

public static class DistroHelpers
{
    public static bool WasFound(this Distro distro) => distro != Distro.Unknown;
}