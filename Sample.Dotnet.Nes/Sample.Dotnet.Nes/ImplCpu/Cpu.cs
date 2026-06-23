using Sample.Dotnet.Nes.Types;
using System.Diagnostics;

namespace Sample.Dotnet.Nes.ImplCpu;

[DebuggerDisplay("{ToString(),nq}")]
public sealed class Cpu
{
    readonly static u16 RESET_VECTOR = 0xFFFC;

    public CpuRegister Register = new CpuRegister();
    private readonly CpuBus _bus;

    public Stack Stack { get; private set; }

    public Cpu(CpuBus bus)
    {
        _bus = bus;
        Stack = new Stack(this);
    }

    public u8 ReadByte(u16 address)
    {
        return _bus.Read(address);
    }

    public void Step(out int outCpuCycle)
    {
        u8 opCode = FetchByte();
        OpCodeContainer.FuncExecutor? funcOrNull = OpCodeContainer.GetFuncOrNull(opCode);

        if (funcOrNull is null)
        {
            throw new NesException("UnknownOpcode");
            //return 0;
        }

        funcOrNull(this, out outCpuCycle);
    }

    public u8 FetchByte()
    {
        u8 data = _bus.Read(Register.PC);
        Register.PC++;
        return data;
    }

    public void WriteByte(u16 addr, u8 value)
    {
        _bus.Write(addr, value);
    }

    public override string ToString()
    {
        return Register.ToString()!;
    }

    public void Reset()
    {
        u16 lo = (u16)ReadByte(RESET_VECTOR);
        u16 hi = (u16)ReadByte(RESET_VECTOR + 1);
        u16 pc = (hi << 8) | lo;
        Register.PC = pc;
    }

    public void Nmi()
    {
        Stack.PushWord(Register.PC);

        u8 s = Register.Status.Value;
        s |= (u8)(byte)E_STATUS_FLAG.FLAG_U;
        s &= ~(u8)(byte)E_STATUS_FLAG.FLAG_B;
        Stack.PushByte(s);

        Register.Status.I = true;

        const int NMI_VECTOR = 0xFFFA;
        u16 lo = (u16)ReadByte(NMI_VECTOR);
        u16 hi = (u16)ReadByte(NMI_VECTOR + 1);
        Register.PC = hi << 8 | lo;
    }
}
