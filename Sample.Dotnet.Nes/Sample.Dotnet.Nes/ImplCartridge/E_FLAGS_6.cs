using System;

namespace Sample.Dotnet.Nes.ImplCartridge;

[Flags]
public enum E_FLAGS_6 : byte
{
    MIRROR_MASK = 0b0000_0001, // 0 = horizontal, 1 = vertical
    BATTERY_MASK = 0b0000_0010, // 1 = battery-backed RAM
    TRAINER_MASK = 0b0000_0100, // 1 = 512-byte trainer before PRG
    FOUR_SCREEN_MASK = 0b0000_1000, // 1 = four-screen VRAM
}