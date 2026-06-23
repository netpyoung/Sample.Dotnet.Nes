using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed partial class Ppu
{
    u8[] _OAM = new u8[256];

    private u8 _Read_OAMDATA()
    {
        return _OAM[PpuRegister.OAMADDR];
    }

    void _Write_OAMDATA(u8 v)
    {
        _OAM[PpuRegister.OAMADDR] = v;
        PpuRegister.OAMADDR += 1;
    }
}
