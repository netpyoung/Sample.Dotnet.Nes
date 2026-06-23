using Sample.Dotnet.Nes.ImplPpu.PpuRegisters;
using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed partial class Ppu
{
    void _Write_PPUMASK(u8 data)
    {
        PpuRegister.PPUMASK = (PPUMASK)data;
    }
}