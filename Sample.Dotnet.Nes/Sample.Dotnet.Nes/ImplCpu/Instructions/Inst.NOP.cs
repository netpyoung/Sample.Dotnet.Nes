namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void NOP(Cpu cpu, out int outCycle)
    {
        outCycle = 2;
    }
}
