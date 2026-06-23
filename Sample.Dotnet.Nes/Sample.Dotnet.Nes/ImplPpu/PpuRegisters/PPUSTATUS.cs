using Sample.Dotnet.Nes.Types;
using System;
using System.Diagnostics;

namespace Sample.Dotnet.Nes.ImplPpu.PpuRegisters;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct PPUSTATUS
{
    [Flags]
    public enum E_FLAG : byte
    {
        // https://www.nesdev.org/wiki/PPU_registers#PPUSTATUS_-_Rendering_events_($2002_read)
        //7  bit  0
        //---- ----
        //VSOx xxxx
        //|||| ||||
        //|||+-++++- (PPU open bus or 2C05 PPU identifier)
        //||+------- Sprite overflow flag
        //| +--------Sprite 0 hit flag
        //+---------Vblank flag, cleared on read.Unreliable; see below.


        SPRITE_0_HIT = 0b0100_0000, // 0x40
        VBLANK = 0b1000_0000, // 0x80
    }

    u8 _value;

    public u8 Value => _value;
    private string DebuggerDisplay => $"0x{_value:X2}";

    public PPUSTATUS(u8 value)
    {
        _value = value;
    }

    public static explicit operator PPUSTATUS(u8 v) => new(v);

    public bool FLAG_VBLANK
    {
        get
        {
            return (_value & (u8)(byte)E_FLAG.VBLANK) != 0;
        }
        set
        {
            if (value)
            {
                _value |= (u8)(byte)E_FLAG.VBLANK;
            }
            else
            {
                _value &= ~(u8)(byte)E_FLAG.VBLANK;
            }
        }
    }

    public bool FLAG_SPRITE_0_HIT
    {
        get
        {
            return (_value & (u8)(byte)E_FLAG.SPRITE_0_HIT) != 0;
        }
        set
        {
            if (value)
            {
                _value |= (u8)(byte)E_FLAG.SPRITE_0_HIT;
            }
            else
            {
                _value &= ~(u8)(byte)E_FLAG.SPRITE_0_HIT;
            }
        }
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public string ToString(string? format, IFormatProvider? provider)
    {
        return _value.ToString(format, provider);
    }
}
