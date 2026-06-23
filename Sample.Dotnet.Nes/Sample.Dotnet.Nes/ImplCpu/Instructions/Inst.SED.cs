namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void SED(Cpu cpu, out int outCpuCycle)
    {
        cpu.Register.Status.D = true;

        outCpuCycle = 2;
    }
}
