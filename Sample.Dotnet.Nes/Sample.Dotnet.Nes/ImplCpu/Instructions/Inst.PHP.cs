using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void PHP(Cpu cpu, out int outCpuCycle)
    {
        u8 x = cpu.Register.Status.Value;
        x |= (byte)E_STATUS_FLAG.FLAG_B;
        x |= (byte)E_STATUS_FLAG.FLAG_U;

        cpu.Stack.PushByte(x);

        outCpuCycle = 3;
    }
}