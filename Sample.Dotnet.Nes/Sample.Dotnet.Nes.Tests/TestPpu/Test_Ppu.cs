using Sample.Dotnet.Nes.ImplCartridge;
using Sample.Dotnet.Nes.ImplPpu;
using Sample.Dotnet.Nes.Types;
using System.Reflection;

namespace Sample.Dotnet.Nes.Tests.TestPpu;

public sealed class Test_Ppu
{
    static class RomPath
    {
        public const string NES_TEST = ".roms/test/nestest.nes";
        public const string NES_TEST_LOG = ".roms/test/nestest.log";
        public const string MARIO = ".roms/mario.nes";
    }
    static readonly string SOLUTION_DIR = GetSolutionDirectory();

    public static string GetSolutionDirectory()
    {
        DirectoryInfo? currentDirOrNull = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);

        while (currentDirOrNull != null && currentDirOrNull.GetFiles("*.slnx").Length == 0)
        {
            currentDirOrNull = currentDirOrNull.Parent;
        }

        if (currentDirOrNull == null)
        {
            throw new DirectoryNotFoundException("Solution directory not found.");
        }

        if (currentDirOrNull.FullName == null)
        {
            throw new DirectoryNotFoundException("Solution directory not found.");
        }

        return currentDirOrNull.FullName;
    }

    private static Ppu PreparePpu()
    {
        string romPath = Path.Combine(SOLUTION_DIR, RomPath.MARIO);
        Cartridge cartridge = Cartridge.CreateFromPath(romPath);
        PpuBus bus = new PpuBus(cartridge);
        Ppu ppu = new Ppu(bus);
        return ppu;
    }

    readonly static u8 FLAG_VBLANK = 0b1000_0000; // 0x80

    [Fact]
    public void Test01()
    {
        string romPath = Path.Combine(SOLUTION_DIR, RomPath.MARIO);
        Cartridge cartridge = Cartridge.CreateFromPath(romPath);
        PpuBus bus = new PpuBus(cartridge);
        Ppu ppu = new Ppu(bus);

        // # Test 1: VBlank fires at the right scanline
        for (int i = 0; i < 241 * 341 - 1; ++i)
        {
            ppu.Step(out bool _);
        }

        Assert.False(ppu.IsFrameReady);
        Assert.False(ppu.PpuRegister.PPUSTATUS.FLAG_VBLANK);

        ppu.Step(out bool _);
        Assert.True(ppu.IsFrameReady);
        Assert.True(ppu.PpuRegister.PPUSTATUS.FLAG_VBLANK);

        // # Test 2: read_status clears VBlank and resets latch
        u8 result;

        ppu.Write(0x2005, 0x00);
        Assert.True(ppu.LoopyRegister.W);

        result = ppu.Read(0x2002);

        Assert.NotEqual<u8>(result & FLAG_VBLANK, 0);
        Assert.False(ppu.PpuRegister.PPUSTATUS.FLAG_VBLANK);
        Assert.False(ppu.LoopyRegister.W);


    }

    [Fact]
    public void Test03()
    {
        // # Test 3: NMI callback fires
        Ppu ppu = PreparePpu();
        ppu.Write(0x2000, 0x80);

        int nmiCount = 0;
        for (int i = 0; i < 341 * 262; ++i)
        {
            ppu.Step(out bool isNmiTriggerRequired);
            if (isNmiTriggerRequired)
            {
                nmiCount++;
            }
        }

        Assert.Equal(1, nmiCount);
    }

    [Fact]
    public void Test04()
    {
        // # Test 4: NMI does NOT fire when disabled
        Ppu ppu = PreparePpu();

        int nmiCount = 0;
        for (int i = 0; i < 341 * 262; ++i)
        {
            ppu.Step(out bool isNmiTriggerRequired);
            if (isNmiTriggerRequired)
            {
                nmiCount++;
            }
        }

        Assert.Equal(0, nmiCount);
    }

    [Fact]
    public void Test05()
    {
        // # Test 5: PPUSCROLL sets loopy registers
        Ppu ppu = PreparePpu();
        ppu.Write(0x2005, 43);
        Assert.Equal<u8>(3, ppu.LoopyRegister.FineX);
        Assert.Equal<u16>(5, ppu.LoopyRegister.T.Addr & 0x001F);

        ppu.Write(0x2005, 43);
        Assert.Equal(5, ppu.LoopyRegister.T.CoarseY);
        Assert.Equal(3, ppu.LoopyRegister.T.FineY);
    }

    [Fact]
    public void Test06()
    {
        // # Test 6: PPUADDR sets v on second write
        Ppu ppu = PreparePpu();
        ppu.Write(0x2006, 0x21);
        ppu.Write(0x2006, 0x08);

        Assert.Equal<u16>(0x2108, ppu.LoopyRegister.V.Addr);
    }

    [Fact]
    public void Test07()
    {
        // # Test 7: PPUDATA write/read through VRAM
        Ppu ppu = PreparePpu();
        ppu.Write(0x2006, 0x20);
        ppu.Write(0x2006, 0x00);
        ppu.Write(0x2007, 0x42);

        ppu.Write(0x2006, 0x20);
        ppu.Write(0x2006, 0x00);
        ppu.Read(0x2007);
        u8 result = ppu.Read(0x2007);
        Assert.Equal<u8>(0x42, result);
    }

    [Fact]
    public void Test08()
    {
        // # Test 8: Palette read is immediate (no dummy read)
        Ppu ppu = PreparePpu();
        ppu.Write(0x2006, 0x3F);
        ppu.Write(0x2006, 0x00);
        ppu.Write(0x2007, 0x15);

        ppu.Write(0x2006, 0x3F);
        ppu.Write(0x2006, 0x00);
        u8 result = ppu.Read(0x2007);
        Assert.Equal<u8>(0x15, result);
    }

    [Fact]
    public void Test09()
    {
        // # Test 9: OAM write/read
        Ppu ppu = PreparePpu();
        ppu.Write(0x2003, 0x00);
        ppu.Write(0x2004, 0xAA);
        ppu.Write(0x2003, 0x00);

        u8 result = ppu.Read(0x2004);
        Assert.Equal<u8>(0xAA, result);

        ppu.Write(0x2003, 0x00);
        ppu.Write(0x2004, 0x11);
        ppu.Write(0x2004, 0x22);
        ppu.Write(0x2003, 0x01);
        result = ppu.Read(0x2004);
        Assert.Equal<u8>(0x22, result);
    }

    [Fact]
    public void Test10()
    {
        // # Test 10: PPUCTRL nametable bits go to t
        Ppu ppu = PreparePpu();
        ppu.Write(0x2000, 0x03);

        Assert.Equal<u16>(0x0C00, ppu.LoopyRegister.T.Addr & 0x0C00);
        Assert.Equal(3, ppu.LoopyRegister.T.Nametable);
    }
}
