using Sample.Dotnet.Nes.Types;
using System.Runtime.CompilerServices;

namespace Sample.Dotnet.Nes.ImplCpu;


// ref: https://www.nesdev.org/obelisk-6502-guide/addressing.html

public static class Addressing
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static u16 ZeroPage(Cpu cpu)
    {
        u8 addr = cpu.FetchByte();
        return (u16)addr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static u16 ZeroPage_X(Cpu cpu)
    {
        u16 addr = ZeroPage(cpu);
        return (u16)((u8)addr + cpu.Register.X);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static u16 ZeroPage_Y(Cpu cpu)
    {
        u16 addr = ZeroPage(cpu);
        return (u16)((u8)addr + cpu.Register.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static u16 Relative(Cpu cpu)
    {
        i8 offset = (i8)cpu.FetchByte();
        return cpu.Register.PC + offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static u16 Absolute(Cpu cpu)
    {
        u16 lo = (u16)cpu.FetchByte();
        u16 hi = (u16)cpu.FetchByte();

        return (hi << 8) | lo;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static u16 Absolute_X(Cpu cpu, out bool outIsPageCrossed)
    {
        u16 baseAddr = Absolute(cpu);
        u16 nextAddr = baseAddr + cpu.Register.X;

        outIsPageCrossed = (baseAddr & 0xFF00) != (nextAddr & 0xFF00);

        return nextAddr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static u16 Absolute_Y(Cpu cpu, out bool outIsPageCrossed)
    {
        u16 baseAddr = Absolute(cpu);
        u16 nextAddr = baseAddr + cpu.Register.Y;

        outIsPageCrossed = (baseAddr & 0xFF00) != (nextAddr & 0xFF00);

        return nextAddr;
    }

    public static u16 Indirect(Cpu cpu)
    {
        u16 loAddr = Absolute(cpu);

        u16 lo = (u16)cpu.ReadByte(loAddr);
        u16 hiAddr;
        if ((loAddr & 0xFF) == 0xFF)
        {
            hiAddr = loAddr & 0xFF00;
        }
        else
        {
            hiAddr = loAddr + 1;
        }
        u16 hi = (u16)cpu.ReadByte(hiAddr);
        return (hi << 8) | lo;
    }

    /// <summary>
    /// Indexed Indirect
    /// </summary>
    public static u16 Indirect_X(Cpu cpu) => _Indexed_Indirect(cpu);

    /// <summary>
    /// Indirect Indexed
    /// </summary>
    public static u16 Indirect_Y(Cpu cpu, out bool outIsPageCrossed) => _Indirect_Indexed(cpu, out outIsPageCrossed);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static u16 _Indexed_Indirect(Cpu cpu)
    {
        u8 data = cpu.FetchByte();
        u8 ptr = data + cpu.Register.X;

        u16 lo = (u16)cpu.ReadByte((u16)ptr);
        u16 hi = (u16)cpu.ReadByte((u16)(ptr + 1));
        return (hi << 8) | lo;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static u16 _Indirect_Indexed(Cpu cpu, out bool outIsPageCrossed)
    {
        u8 data = cpu.FetchByte();

        u16 lo = (u16)cpu.ReadByte((u16)data);
        u16 hi = (u16)cpu.ReadByte((u16)(data + 1));
        u16 baseAddr = (hi << 8) | lo;
        u16 nextAddr = baseAddr + cpu.Register.Y;

        outIsPageCrossed = ((lo & 0xFF) + cpu.Register.Y > 0xFF);

        return nextAddr;
    }
}
