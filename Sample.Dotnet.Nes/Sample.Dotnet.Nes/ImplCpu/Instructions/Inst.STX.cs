using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu.Instructions;

public static partial class Inst
{
    public static void STX_ZERO_PAGE(Cpu cpu, out int outCycle)
    {
        u16 addr = Addressing.ZeroPage(cpu);
        cpu.WriteByte(addr, cpu.Register.X);

        outCycle = 3;
    }

    public static void STX_ZERO_PAGE_Y(Cpu cpu, out int outCycle)
    {
        u16 addr = Addressing.ZeroPage_Y(cpu);
        cpu.WriteByte(addr, cpu.Register.X);

        outCycle = 4;
    }

    public static void STX_ABSOLUTE(Cpu cpu, out int outCycle)
    {
        u16 addr = Addressing.Absolute(cpu);
        cpu.WriteByte(addr, cpu.Register.X);

        outCycle = 4;
    }
}
