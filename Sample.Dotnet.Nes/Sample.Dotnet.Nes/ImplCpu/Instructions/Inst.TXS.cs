namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void TXS(Cpu cpu, out int outCycle)
    {
        cpu.Register.SP = cpu.Register.X;

        outCycle = 2;
    }
}
