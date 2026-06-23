using System;

namespace Sample.Dotnet.Nes.ImplApu;

[Flags]
public enum E_CHANNEL_STATUS : byte
{
    NONE = 0b0000_0000, // 0x00
    STATUS_PULSE1 = 0b0000_0001, // 0x01
    STATUS_PULSE2 = 0b0000_0010, // 0x02
    STATUS_TRIANGLE = 0b0000_0100, // 0x04
    STATUS_NOISE = 0b0000_1000, // 0x08
}