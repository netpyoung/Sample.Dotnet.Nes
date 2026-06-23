using Sample.Dotnet.Nes.ImplApu;
using Sample.Dotnet.Nes.ImplCartridge;
using Sample.Dotnet.Nes.ImplCpu;
using Sample.Dotnet.Nes.ImplPpu;
using Sample.Dotnet.Nes.Types;
using System;
using System.Linq;

namespace Sample.Dotnet.Nes;

#pragma warning disable MA0049 // Type name should not match containing namespace
public sealed class Nes
#pragma warning restore MA0049 // Type name should not match containing namespace
{
    public Cpu Cpu { get; private init; }
    public CpuBus CpuBus { get; private init; }

    public Apu Apu { get; private init; }
    public Ppu Ppu { get; private init; }
    public PpuBus PpuBus { get; private init; }
    public Cartridge Cartridge { get; private init; }

    public unsafe u8* FrameBufferPtr => Ppu.FrameBufferPtr;

    public static Nes CreateFromPath(string romPath)
    {
        Cartridge cartridge = Cartridge.CreateFromPath(romPath);
        return new Nes(cartridge);
    }

    public static Nes CreateDummyCartridge()
    {
        Cartridge cartridge = Cartridge.CreateDummyCartridge();
        return new Nes(cartridge);
    }

    public Nes(Cartridge cartridge)
    {
        Cartridge = cartridge;

        PpuBus = new PpuBus(Cartridge);
        Ppu = new Ppu(PpuBus);

        Apu = new Apu();

        CpuBus = new CpuBus(Cartridge, Ppu, Apu);
        Cpu = new Cpu(CpuBus);

        Cpu.Reset();
    }

    public void RunFrame()
    {
        if (Ppu.IsFrameReady)
        {
            Ppu.ClearFrameReady();
        }

        while (!Ppu.IsFrameReady)
        {
            Step();
        }
    }

    internal void LoadBytes(byte[] bytes)
    {
        foreach ((int i, byte v) in bytes.Index())
        {
            CpuBus.Write(i, v);
        }
    }

    internal int Step()
    {
        Cpu.Step(out int cpuCycle);

        for (int i = 0; i < cpuCycle * 3; ++i)
        {
            Ppu.Step(out bool isNmiTriggerRequired);
            if (isNmiTriggerRequired)
            {
                Cpu.Nmi();
            }
        }

        for (int i = 0; i < cpuCycle; ++i)
        {
            Apu.Step();
        }

        return cpuCycle;
    }

    internal void Debug()
    {
        Console.WriteLine(Cpu);
    }
}
