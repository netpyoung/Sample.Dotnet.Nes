using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void JSR(Cpu cpu, out int outCpuCycle)
    {
        u16 addr = Addressing.Absolute(cpu);
        cpu.Stack.PushWord(cpu.Register.PC - (u16)1);
        cpu.Register.PC = addr;

        outCpuCycle = 6;
    }
}
