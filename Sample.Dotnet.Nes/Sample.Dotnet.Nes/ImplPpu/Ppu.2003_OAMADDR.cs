using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed partial class Ppu
{
    void _Write_OAMADDR(u8 data)
    {
        PpuRegister.OAMADDR = data;
    }
}
