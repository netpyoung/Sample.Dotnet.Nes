namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void INX(Cpu cpu, out int outCycle)
    {
        cpu.Register.X++;
        cpu.Register.Status.SetZfromValue(cpu.Register.X);
        cpu.Register.Status.SetNfromValue(cpu.Register.X);

        outCycle = 2;
    }
}
