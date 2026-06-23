using Sample.Dotnet.Nes.Types;
using System;
using System.Diagnostics;

namespace Sample.Dotnet.Nes.ImplPpu.PpuRegisters;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct PPUCTRL
{
    //[Flags]
    public enum E_FLAG : byte
    {
        // https://www.nesdev.org/wiki/PPU_registers#PPUCTRL_-_Miscellaneous_settings_($2000_write)
        //7  bit  0
        //---- ----
        //VPHB SINN
        //|||| ||||
        //|||| ||++- Base nametable address
        //|||| ||    (0 = $2000; 1 = $2400; 2 = $2800; 3 = $2C00)
        //|||| |+--- VRAM address increment per CPU read/write of PPUDATA
        //|||| |     (0: add 1, going across; 1: add 32, going down)
        //|||| +---- Sprite pattern table address for 8x8 sprites
        //||||       (0: $0000; 1: $1000; ignored in 8x16 mode)
        //|||+------ Background pattern table address(0: $0000; 1: $1000)
        //||+------- Sprite size(0: 8x8 pixels; 1: 8x16 pixels – see PPU OAM#Byte 1)
        //| +--------PPU master/slave select
        //|          (0: read backdrop from EXT pins; 1: output color on EXT pins)
        //+--------- Vblank NMI enable(0: off, 1: on)
        BASE_NAMETABLE_ADDRESS = 0b0000_0011,
        VRAM_INCREMENT = 0b0000_0100,
        PATTERN_TABLE_SPRITE_ADDRESS = 0b0000_1000,
        PATTERN_TABLE_BACKGROUND_ADDRESS = 0b0001_0000,
        NMI_ENABLED = 0b1000_0000,
    }

    u8 _value;
    public readonly u8 Value => _value;
    private readonly string DebuggerDisplay => $"0x{_value:X2}";

    public PPUCTRL(u8 value)
    {
        _value = value;
    }

    public static explicit operator PPUCTRL(u8 v) => new(v);


    public readonly u8 BASE_NAMETABLE_ADDRESS
    {
        get
        {
            u8 addr = _value & (u8)(byte)E_FLAG.BASE_NAMETABLE_ADDRESS;
            return addr;
        }
    }

    public bool FLAG_VRAM_INCREMENT
    {
        readonly get
        {
            return (_value & (u8)(byte)E_FLAG.VRAM_INCREMENT) != 0;
        }
        set
        {
            if (value)
            {
                _value |= (u8)(byte)E_FLAG.VRAM_INCREMENT;
            }
            else
            {
                _value &= ~(u8)(byte)E_FLAG.VRAM_INCREMENT;
            }
        }
    }

    public bool FLAG_NMI_ENABLED
    {
        readonly get
        {
            return (_value & (u8)(byte)E_FLAG.NMI_ENABLED) != 0;
        }
        set
        {
            if (value)
            {
                _value |= (u8)(byte)E_FLAG.NMI_ENABLED;
            }
            else
            {
                _value &= ~(u8)(byte)E_FLAG.NMI_ENABLED;
            }
        }
    }

    public bool FLAG_PATTERN_TABLE_SPRITE_ADDRESS
    {
        readonly get
        {
            return (_value & (u8)(byte)E_FLAG.PATTERN_TABLE_SPRITE_ADDRESS) != 0;
        }
        set
        {
            if (value)
            {
                _value |= (u8)(byte)E_FLAG.PATTERN_TABLE_SPRITE_ADDRESS;
            }
            else
            {
                _value &= ~(u8)(byte)E_FLAG.PATTERN_TABLE_SPRITE_ADDRESS;
            }
        }
    }

    public bool FLAG_PATTERN_TABLE_BACKGROUND_ADDRESS
    {
        readonly get
        {
            return (_value & (u8)(byte)E_FLAG.PATTERN_TABLE_BACKGROUND_ADDRESS) != 0;
        }
        set
        {
            if (value)
            {
                _value |= (u8)(byte)E_FLAG.PATTERN_TABLE_BACKGROUND_ADDRESS;
            }
            else
            {
                _value &= ~(u8)(byte)E_FLAG.PATTERN_TABLE_BACKGROUND_ADDRESS;
            }
        }
    }

    public readonly u16 GetBgPatternTableAddress()
    {
        if (FLAG_PATTERN_TABLE_BACKGROUND_ADDRESS)
        {
            return 0x1000;
        }
        return 0x0000;
    }

    public readonly u16 GetSpritePatternTableAddress()
    {
        if (FLAG_PATTERN_TABLE_SPRITE_ADDRESS)
        {
            return 0x1000;
        }
        return 0x0000;
    }

    public readonly int GetVramIncrementAmount()
    {
        if (FLAG_VRAM_INCREMENT)
        {
            return 32;
        }
        return 1;
    }

    public override readonly string ToString()
    {
        return _value.ToString();
    }

    public readonly string ToString(string? format, IFormatProvider? provider)
    {
        return _value.ToString(format, provider);
    }
}
