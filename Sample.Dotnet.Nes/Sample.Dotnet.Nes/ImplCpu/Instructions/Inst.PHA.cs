namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void PHA(Cpu cpu, out int outCpuCycle)
    {
        cpu.Stack.PushByte(cpu.Register.A);

        outCpuCycle = 3;
    }
}