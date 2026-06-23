namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void CLI(Cpu cpu, out int outCpuCycle)
    {
        cpu.Register.Status.I = false;

        outCpuCycle = 2;
    }
}
