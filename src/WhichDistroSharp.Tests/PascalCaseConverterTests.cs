namespace WhichDistroSharp.Tests;

using static global::WhichDistroSharp.PascalCaseConverter;
using Xunit;

public class PascalCaseConverterTests
{
    [Theory]
    [InlineData("linuxmint", "LinuxMint")]
    [InlineData("LinuxMint", "LinuxMint")]
    public void ToPascalCase_KnownTerm_ReturnsCorrectPascalCase(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Theory]
    [InlineData("ubuntu", "Ubuntu")]
    [InlineData("debian", "Debian")]
    [InlineData("arch", "Arch")]
    [InlineData("fedora", "Fedora")]
    [InlineData("gentoo", "Gentoo")]
    [InlineData("kali", "Kali")]
    [InlineData("nixos", "Nixos")]
    [InlineData("rocky", "Rocky")]
    [InlineData("slackware", "Slackware")]
    public void ToPascalCase_SimpleLowercase_ReturnsCapitalized(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Theory]
    [InlineData("ubuntu_kylin", "UbuntuKylin")]
    [InlineData("ios_xr", "IosXr")]
    [InlineData("puppy_s15pup32", "PuppyS15pup32")]
    [InlineData("puppy_fossapup64", "PuppyFossapup64")]
    [InlineData("puppy_s15pup64", "PuppyS15pup64")]
    [InlineData("sles_sap", "SlesSap")]
    public void ToPascalCase_UnderscoreSeparated_ReturnsPascalCase(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Theory]
    [InlineData("opensuse-leap", "OpensuseLeap")]
    [InlineData("opensuse-tumbleweed", "OpensuseTumbleweed")]
    [InlineData("suse-microos", "SuseMicroos")]
    [InlineData("clear-linux-os", "ClearLinuxOs")]
    [InlineData("cumulus-linux", "CumulusLinux")]
    [InlineData("redox-os", "RedoxOs")]
    public void ToPascalCase_HyphenSeparated_ReturnsPascalCase(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Theory]
    [InlineData("opensuse", "Opensuse")]
    [InlineData("centos", "Centos")]
    [InlineData("coreos", "Coreos")]
    [InlineData("deepin", "Deepin")]
    [InlineData("solus", "Solus")]
    public void ToPascalCase_LowercaseSingleWord_ReturnsCapitalized(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Theory]
    [InlineData("Deepin", "Deepin")]
    [InlineData("openEuler", "OpenEuler")]
    [InlineData("XCP-ng", "XCPNg")]
    public void ToPascalCase_MixedCaseInput_PreservesInternalCasing(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Theory]
    [InlineData("hello world", "HelloWorld")]
    [InlineData("hello.world", "HelloWorld")]
    [InlineData("hello-world_test", "HelloWorldTest")]
    [InlineData("hello-world", "HelloWorld")]
    public void ToPascalCase_MixedSeparators_ReturnsPascalCase(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Fact]
    public void ToPascalCase_SingleCharacter_ReturnsCapitalized()
    {
        Assert.Equal("A", ToPascalCase("a"));
    }

    [Fact]
    public void ToPascalCase_EmptyString_ReturnsEmpty()
    {
        Assert.Equal("", ToPascalCase(""));
    }

    [Theory]
    [InlineData("nobara/gnome", "NobaraGnome")]
    public void ToPascalCase_ForwardSlashSeparated_ReturnsPascalCase(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Theory]
    [InlineData("alpine", "Alpine")]
    [InlineData("altlinux", "Altlinux")]
    [InlineData("amzn", "Amzn")]
    [InlineData("arch32", "Arch32")]
    [InlineData("arkane", "Arkane")]
    [InlineData("aurora", "Aurora")]
    [InlineData("azurelinux", "Azurelinux")]
    [InlineData("bazzite", "Bazzite")]
    [InlineData("blackarch", "Blackarch")]
    [InlineData("blendos", "Blendos")]
    [InlineData("bluefin", "Bluefin")]
    [InlineData("bodhi", "Bodhi")]
    [InlineData("buildroot", "Buildroot")]
    [InlineData("cachyos", "Cachyos")]
    [InlineData("chimera", "Chimera")]
    [InlineData("chimeraos", "Chimeraos")]
    [InlineData("cirros", "Cirros")]
    [InlineData("cos", "Cos")]
    [InlineData("debian", "Debian")]
    [InlineData("devuan", "Devuan")]
    [InlineData("discontinued", "Discontinued")]
    [InlineData("dragonfly", "Dragonfly")]
    [InlineData("elementary", "Elementary")]
    [InlineData("endeavouros", "Endeavouros")]
    [InlineData("endless", "Endless")]
    [InlineData("eurolinux", "Eurolinux")]
    [InlineData("exherbo", "Exherbo")]
    [InlineData("flatcar", "Flatcar")]
    [InlineData("freebsd", "Freebsd")]
    [InlineData("funtoo", "Funtoo")]
    [InlineData("garuda", "Garuda")]
    [InlineData("ghostbsd", "Ghostbsd")]
    [InlineData("gnoppix", "Gnoppix")]
    [InlineData("guix", "Guix")]
    [InlineData("hyperbola", "Hyperbola")]
    [InlineData("kaos", "Kaos")]
    [InlineData("mageia", "Mageia")]
    [InlineData("manjaro", "Manjaro")]
    [InlineData("mariner", "Mariner")]
    [InlineData("miraclelinux", "Miraclelinux")]
    [InlineData("nilrt", "Nilrt")]
    [InlineData("nobara", "Nobara")]
    [InlineData("novariaos", "Novariaos")]
    [InlineData("nuros", "Nuros")]
    [InlineData("omnios", "Omnios")]
    [InlineData("openmandriva", "Openmandriva")]
    [InlineData("openwrt", "Openwrt")]
    [InlineData("parrot", "Parrot")]
    [InlineData("pclinuxos", "Pclinuxos")]
    [InlineData("pengwin", "Pengwin")]
    [InlineData("photon", "Photon")]
    [InlineData("pika", "Pika")]
    [InlineData("pisilinux", "Pisilinux")]
    [InlineData("pop", "Pop")]
    [InlineData("postmarketos", "Postmarketos")]
    [InlineData("puppy", "Puppy")]
    [InlineData("pureos", "Pureos")]
    [InlineData("rancheros", "Rancheros")]
    [InlineData("raspbian", "Raspbian")]
    [InlineData("rebornos", "Rebornos")]
    [InlineData("rhel", "Rhel")]
    [InlineData("scientific", "Scientific")]
    [InlineData("sled", "Sled")]
    [InlineData("sles", "Sles")]
    [InlineData("solus", "Solus")]
    [InlineData("steamos", "Steamos")]
    [InlineData("sysrescue", "Sysrescue")]
    [InlineData("tails", "Tails")]
    [InlineData("tencentos", "Tencentos")]
    [InlineData("tinycore", "Tinycore")]
    [InlineData("trisquel", "Trisquel")]
    [InlineData("ubios", "Ubios")]
    [InlineData("ultramarine", "Ultramarine")]
    [InlineData("vanillaos", "Vanillaos")]
    [InlineData("void", "Void")]
    [InlineData("wolfi", "Wolfi")]
    [InlineData("wrlinux", "Wrlinux")]
    [InlineData("xenenterprise", "Xenenterprise")]
    [InlineData("zorin", "Zorin")]
    public void ToPascalCase_VariousDistroNames_ReturnsPascalCase(string input, string expected)
    {
        Assert.Equal(expected, ToPascalCase(input));
    }

    [Fact]
    public void ToPascalCase_NullInput_ReturnsNull()
    {
        Assert.Null(ToPascalCase((string?)null));
    }

    [Fact]
    public void ToPascalCase_WhitespaceOnly_ReturnsEmpty()
    {
        Assert.Equal("", ToPascalCase("   "));
    }

    [Fact]
    public void ToPascalCase_OnlySeparators_ReturnsEmpty()
    {
        Assert.Equal("", ToPascalCase("---___"));
    }

    [Fact]
    public void ToPascalCase_LeadingSeparators_Ignored()
    {
        Assert.Equal("Hello", ToPascalCase("-hello"));
    }

    [Fact]
    public void ToPascalCase_TrailingSeparators_Ignored()
    {
        Assert.Equal("Hello", ToPascalCase("hello-"));
    }

    [Fact]
    public void ToPascalCase_MultipleConsecutiveSeparators_ReturnsPascalCase()
    {
        Assert.Equal("HelloWorld", ToPascalCase("hello---world"));
    }

    [Fact]
    public void ToPascalCase_MixedSeparatorsEdgeCase_ReturnsPascalCase()
    {
        Assert.Equal("HelloWorldTest", ToPascalCase("hello-world_test"));
    }

    [Fact]
    public void ToPascalCase_AllCapsWord_ReturnsCapitalized()
    {
        Assert.Equal("XCPNg", ToPascalCase("XCP-ng"));
    }

    [Fact]
    public void ToPascalCase_KnownTermCaseInsensitive_ReturnsKnownValue()
    {
        Assert.Equal("LinuxMint", ToPascalCase("LINUXMINT"));
    }
}