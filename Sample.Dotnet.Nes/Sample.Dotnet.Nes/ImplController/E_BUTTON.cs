using System;

namespace Sample.Dotnet.Nes.ImplController;

[Flags]
public enum E_BUTTON : byte
{
    A = 0b0000_0001,
    B = 0b0000_0010,
    SELECT = 0b0000_0100,
    START = 0b0000_1000,
    UP = 0b0001_0000,
    DOWN = 0b0010_0000,
    LEFT = 0b0100_0000,
    RIGHT = 0b1000_0000,
}
