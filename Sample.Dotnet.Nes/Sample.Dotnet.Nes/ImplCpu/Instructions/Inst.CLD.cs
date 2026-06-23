namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void CLD(Cpu cpu, out int outCpuCycle)
    {
        cpu.Register.Status.D = false;

        outCpuCycle = 2;
    }
}