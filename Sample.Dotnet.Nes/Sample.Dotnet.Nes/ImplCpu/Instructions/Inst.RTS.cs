using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void RTS(Cpu cpu, out int outCpuCycle)
    {
        u16 addr = cpu.Stack.PopWord();
        cpu.Register.PC = addr + 1;

        outCpuCycle = 6;
    }
}
