namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void SEC(Cpu cpu, out int outCpuCycle)
    {
        cpu.Register.Status.C = true;

        outCpuCycle = 2;
    }
}
