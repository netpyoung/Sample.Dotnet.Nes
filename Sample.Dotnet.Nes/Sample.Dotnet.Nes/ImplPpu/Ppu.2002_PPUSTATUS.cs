using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed partial class Ppu
{
    private u8 _Read_PPUSTATUS()
    {
        u8 status = PpuRegister.PPUSTATUS.Value;
        PpuRegister.PPUSTATUS.FLAG_VBLANK = false;
        LoopyRegister.W = false;
        return status;
    }
}
