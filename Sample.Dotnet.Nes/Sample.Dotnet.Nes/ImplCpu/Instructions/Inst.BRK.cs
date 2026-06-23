using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    readonly static u16 IRQ_VECTOR = (u16)0xFFFE;

    public static void BRK(Cpu cpu, out int outCpuCycle)
    {
        u16 returnAddr = cpu.Register.PC + 1;
        cpu.Stack.PushWord(returnAddr);

        byte flag = (byte)cpu.Register.Status.Value;
        flag |= (byte)E_STATUS_FLAG.FLAG_B;
        flag |= (byte)E_STATUS_FLAG.FLAG_U;
        cpu.Stack.PushByte(flag);

        cpu.Register.Status.I = true;

        u16 lo = (u16)cpu.ReadByte(IRQ_VECTOR);
        u16 hi = (u16)cpu.ReadByte(IRQ_VECTOR + 1);
        cpu.Register.PC = (hi << 8) | lo;

        outCpuCycle = 7;
    }
}
