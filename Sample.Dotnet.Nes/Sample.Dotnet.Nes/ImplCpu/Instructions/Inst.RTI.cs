using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void RTI(Cpu cpu, out int outCpuCycle)
    {
        u8 status = cpu.Stack.PopByte();
        status &= ~(byte)E_STATUS_FLAG.FLAG_B;
        status |= (byte)E_STATUS_FLAG.FLAG_U;

        cpu.Register.Status.Reset(status);
        cpu.Register.PC = cpu.Stack.PopWord();

        outCpuCycle = 6;
    }
}
