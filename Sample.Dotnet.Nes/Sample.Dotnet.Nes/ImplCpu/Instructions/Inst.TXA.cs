namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void TXA(Cpu cpu, out int outCycle)
    {
        cpu.Register.A = cpu.Register.X;
        cpu.Register.Status.SetZfromValue(cpu.Register.A);
        cpu.Register.Status.SetNfromValue(cpu.Register.A);

        outCycle = 2;
    }
}
