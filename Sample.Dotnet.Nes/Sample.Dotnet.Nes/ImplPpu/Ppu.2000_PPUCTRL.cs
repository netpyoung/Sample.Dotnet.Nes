using Sample.Dotnet.Nes.ImplPpu.PpuRegisters;
using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed partial class Ppu
{
    void _Write_PPUCTRL(u8 data)
    {
        PpuRegister.PPUCTRL = (PPUCTRL)data;
        u8 nameTable = PpuRegister.PPUCTRL.BASE_NAMETABLE_ADDRESS;
        LoopyRegister.T.SetNameTable(nameTable);
    }
}