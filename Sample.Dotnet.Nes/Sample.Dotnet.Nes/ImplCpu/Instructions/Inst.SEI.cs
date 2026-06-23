namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void SEI(Cpu cpu, out int outCpuCycle)
    {
        cpu.Register.Status.I = true;

        outCpuCycle = 2;
    }
}