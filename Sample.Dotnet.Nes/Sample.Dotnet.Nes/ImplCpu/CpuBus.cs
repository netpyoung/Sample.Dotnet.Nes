using Sample.Dotnet.Nes.ImplApu;
using Sample.Dotnet.Nes.ImplCartridge;
using Sample.Dotnet.Nes.ImplController;
using Sample.Dotnet.Nes.ImplPpu;
using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplCpu;

public sealed class CpuBus
{
    readonly static u16 RAM_MASK = 0x07FF; // (0 ~ 2047)

    readonly Cartridge _cartridge;
    readonly Ppu _ppu;
    readonly Apu _apu;
    readonly u8[] _cpuRam;
    readonly u8[] _oamDmaBuffer = new u8[256];

    public Controller Controller1 { get; } = new Controller();
    public Controller Controller2 { get; } = new Controller();

    public CpuBus(Cartridge cartridge, Ppu ppu, Apu apu)
    {
        _cpuRam = new u8[2048]; // 2kb - 0x0000 ~ 0x07FF
        _cartridge = cartridge;
        _ppu = ppu;
        _apu = apu;
    }

    public u8 Read(u16 addr)
    {
        if (CpuAddress.RegionRam.IsIn(addr))
        {
            return _CpuRam_Read(addr);
        }

        if (CpuAddress.RegionPpu.IsIn(addr))
        {
            return _ppu.Read(addr);
        }

        if (CpuAddress.RegionPrgRom.IsIn(addr))
        {
            return _cartridge.Mapper.Prg_Read(addr);
        }

        if (addr == CpuAddress.ADDRESS_APU_STATUS)
        {
            return _apu.ReadStatus();
        }

        if (addr == CpuAddress.CONTROLLER_1)
        {
            return (u8)Controller1.Read();
        }

        if (addr == CpuAddress.CONTROLLER_2)
        {
            return (u8)Controller2.Read();
        }

        return 0;
    }

    public void Write(u16 addr, u8 value)
    {
        if (CpuAddress.RegionRam.IsIn(addr))
        {
            _CpuRam_Write(addr, value);
            return;
        }

        if (CpuAddress.RegionPpu.IsIn(addr))
        {
            _ppu.Write(addr, value);
            return;
        }

        if (CpuAddress.RegionApu.IsIn(addr))
        {
            _apu.Write(addr, value);
            return;
        }

        if (addr == CpuAddress.ADDRESS_APU_STATUS)
        {
            _apu.WriteStatus(value);
            return;
        }
        if (addr == CpuAddress.ADDRESS_APU_FRAME_COUNTER)
        {
            _apu.WriteFrameCounter(value);
            return;
        }
        if (addr == CpuAddress.CONTROLLER_1)
        {
            Controller1.Write(value);
            Controller2.Write(value);
            return;
        }

        if (addr == CpuAddress.OAM_DMA)
        {
            _CpuOam_Dma_Write(value);
            return;
        }
    }

    u8 _CpuRam_Read(u16 addr)
    {
        u16 maskedAddr = addr & RAM_MASK;
        return _cpuRam[(int)maskedAddr];
    }

    void _CpuRam_Write(u16 addr, u8 v)
    {
        u16 maskedAddr = addr & RAM_MASK;
        _cpuRam[(int)maskedAddr] = v;
    }


    void _CpuOam_Dma_Write(u8 page)
    {
        u16 baseAddr = (u16)page << 8;  // ex. 0x02 => 0x02_00
        for (int i = 0; i < 256; ++i)
        {
            _oamDmaBuffer[i] = _CpuRam_Read(baseAddr + i);
        }

        _ppu.Oam_Dma(_oamDmaBuffer);
    }
}
