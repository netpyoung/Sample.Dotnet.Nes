using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed partial class Ppu
{
    void _Write_PPUADDR(u8 data)
    {
        if (LoopyRegister.W)
        {
            LoopyRegister.T.WriteAddress_lo(data);
            LoopyRegister.V = LoopyRegister.T;
        }
        else
        {
            LoopyRegister.T.WriteAddress_hi(data);
        }

        LoopyRegister.W = !LoopyRegister.W;
    }
}
