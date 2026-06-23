namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void CLV(Cpu cpu, out int outCpuCycle)
    {
        cpu.Register.Status.V = false;

        outCpuCycle = 2;
    }
}