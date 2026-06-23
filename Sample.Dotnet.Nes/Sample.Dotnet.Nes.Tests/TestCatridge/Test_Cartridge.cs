using Sample.Dotnet.Nes.ImplCartridge;
using System.Reflection;

namespace Sample.Dotnet.Nes.Tests.TestCatridge;

public sealed class Test_Cartridge
{
    static class RomPath
    {
        public const string MARIO = ".roms/mario.nes";
    }
    static readonly string SOLUTION_DIR = GetSolutionDirectory();

    public static string GetSolutionDirectory()
    {
        DirectoryInfo? currentDir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);

        while (currentDir != null && currentDir.GetFiles("*.slnx").Length == 0)
        {
            currentDir = currentDir.Parent;
        }

        return currentDir?.FullName ?? throw new DirectoryNotFoundException("Solution directory not found.");
    }

    [Fact]
    public void Test1_Cartridge_ParseRomHeader()
    {
        // Test 1: Parse a real ROM
        string romPath = Path.Combine(SOLUTION_DIR, RomPath.MARIO);
        if (!File.Exists(romPath))
        {
            return;
        }

        Cartridge cart = Cartridge.CreateFromPath(romPath);
        Console.WriteLine(cart);

        // Super Mario Bros: 32KB PRG, 8KB CHR, mapper 0, vertical mirroring, no battery
        Assert.Equal(32768, cart.PrgRom.Length);
        Assert.Equal(8192, cart.ChrRom.Length);
        Assert.Equal(0, cart.MapperId);
        Assert.Equal(E_MIRROR_MODE.VERTICAL, cart.MirrorMode);
        Assert.False(cart.IsBattery);
    }

    [Fact]
    public void Test2_Cartridge_RejectInvalidFile()
    {
        // Test 2: Invalid file
        NesException exception = Assert.Throws<NesException>(() =>
        {
            byte[] data = new byte[256];
            _ = new Cartridge(data);
        });

        Assert.Contains("magic number", exception.Message.ToLower());
    }
}
