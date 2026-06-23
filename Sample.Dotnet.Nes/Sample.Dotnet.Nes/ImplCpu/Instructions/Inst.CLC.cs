namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void CLC(Cpu cpu, out int outCpuCycle)
    {
        cpu.Register.Status.C = false;

        outCpuCycle = 2;
    }
}