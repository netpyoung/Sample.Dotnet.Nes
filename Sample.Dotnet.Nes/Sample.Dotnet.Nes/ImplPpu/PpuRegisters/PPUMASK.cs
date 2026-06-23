using Sample.Dotnet.Nes.Types;
using System;
using System.Diagnostics;

namespace Sample.Dotnet.Nes.ImplPpu.PpuRegisters;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct PPUMASK
{
    [Flags]
    public enum E_FLAG : byte
    {
        // https://www.nesdev.org/wiki/PPU_registers#PPUMASK_-_Rendering_settings_($2001_write)
        //7  bit  0
        //---- ----
        //BGRs bMmG
        //|||| ||||
        //|||| |||+- Greyscale (0: normal color, 1: greyscale)
        //|||| ||+-- 1: Show background in leftmost 8 pixels of screen, 0: Hide
        //|||| |+--- 1: Show sprites in leftmost 8 pixels of screen, 0: Hide
        //|||| +---- 1: Enable background rendering
        //|||+------ 1: Enable sprite rendering
        //|| +-------Emphasize red(green on PAL/Dendy)
        //|+-------- Emphasize green(red on PAL/Dendy)
        //+--------- Emphasize blue
        ENABLE_BACKGROUND = 0b0000_1000,
        ENABLE_SPRITE = 0b0001_0000,
    }

    readonly u8 _value;
    public readonly u8 Value => _value;
    private readonly string DebuggerDisplay => $"0x{_value:X2}";

    public static explicit operator PPUMASK(u8 v) => new(v);

    public PPUMASK(u8 value)
    {
        _value = value;
    }


    public readonly bool FLAG_ENABLE_BACKGROUND
    {
        get
        {
            return (_value & (u8)(byte)E_FLAG.ENABLE_BACKGROUND) != 0;
        }
    }

    public readonly bool FLAG_ENABLE_SPRITE
    {
        get
        {
            return (_value & (u8)(byte)E_FLAG.ENABLE_SPRITE) != 0;
        }
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