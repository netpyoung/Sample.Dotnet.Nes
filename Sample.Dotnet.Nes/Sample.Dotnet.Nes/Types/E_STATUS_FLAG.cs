using System;

namespace Sample.Dotnet.Nes.Types;

[Flags]
public enum E_STATUS_FLAG : byte
{
    // Flag Masks (N V - B D I Z C)

    /// <summary>
    /// 7 - Negative
    /// </summary>
    FLAG_N = 0b1000_0000,

    /// <summary>
    /// 6 - Overflow
    /// </summary>
    FLAG_V = 0b0100_0000,

    /// <summary>
    /// 5 - Unused - always 1
    /// </summary>
    FLAG_U = 0b0010_0000,

    /// <summary>
    /// 4 - Break
    /// </summary>
    FLAG_B = 0b0001_0000,

    /// <summary>
    /// 3 - Decimal
    /// </summary>
    FLAG_D = 0b0000_1000,

    /// <summary>
    /// 2 - Interrupt Disable
    /// </summary>
    FLAG_I = 0b0000_0100,

    /// <summary>
    /// 1 - Zero
    /// </summary>
    FLAG_Z = 0b0000_0010,

    /// <summary>
    /// 0 - Carry
    /// </summary>
    FLAG_C = 0b0000_0001,
}
