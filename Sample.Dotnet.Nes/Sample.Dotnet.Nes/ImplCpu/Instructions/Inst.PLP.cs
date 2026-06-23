using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void PLP(Cpu cpu, out int outCpuCycle)
    {
        u8 status = cpu.Stack.PopByte();
        status &= ~(byte)E_STATUS_FLAG.FLAG_B;
        status |= (byte)E_STATUS_FLAG.FLAG_U;
        cpu.Register.Status.Reset(status);

        outCpuCycle = 4;
    }
}