using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu;

public sealed class CpuRegister
{
    public u8 A;
    public u8 X;
    public u8 Y;
    public u8 SP;
    public u16 PC;

    public StatusRegister Status { get; private set; }

    public CpuRegister()
    {
        SP = 0xFD; // 0xFF - 3
        Status = 0b0010_0100; // ..U._.I..
    }

    public override string ToString()
    {
        return $"A=0x{A:X2} X=0x{X:X2} Y=0x{Y:X2} SP=0x{SP:X2} PC=0x{PC:X4}  [{Status}]";
    }
}
