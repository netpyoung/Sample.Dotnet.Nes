using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed class Display
{
    public const int WIDTH = 256;
    public const int HEIGHT = 240;

    public static readonly rgb[] PALETTE = [
        // $00-$0F: Dark
        new(84, 84, 84),    new(0, 30, 116),    new(8, 16, 144),    new(48, 0, 136),
        new(68, 0, 100),    new(92, 0, 48),     new(84, 4, 0),      new(60, 24, 0),
        new(32, 42, 0),     new(8, 58, 0),      new(0, 64, 0),      new(0, 60, 0),
        new(0, 50, 60),     new(0, 0, 0),       new(0, 0, 0),       new(0, 0, 0),

        // $10-$1F: Medium
        new(152, 150, 152), new(8, 76, 196),    new(48, 50, 236),   new(92, 30, 228),
        new(136, 20, 176),  new(160, 20, 100),  new(152, 34, 32),   new(120, 60, 0),
        new(84, 90, 0),     new(40, 114, 0),    new(8, 124, 0),     new(0, 118, 40),
        new(0, 102, 120),   new(0, 0, 0),       new(0, 0, 0),       new(0, 0, 0),

        // $20-$2F: Bright
        new(236, 238, 236), new(76, 154, 236),  new(120, 124, 236), new(176, 98, 236),
        new(228, 84, 236),  new(236, 88, 180),  new(236, 106, 100), new(212, 136, 32),
        new(160, 170, 0),   new(116, 196, 0),   new(76, 208, 32),   new(56, 204, 108),
        new(56, 180, 204),  new(60, 60, 60),    new(0, 0, 0),       new(0, 0, 0),

        // $30-$3F: Pastel
        new(236, 238, 236), new(168, 204, 236), new(188, 188, 236), new(212, 178, 236),
        new(236, 174, 236), new(236, 174, 212), new(236, 180, 176), new(228, 196, 144),
        new(204, 210, 120), new(180, 222, 120), new(168, 226, 144), new(152, 226, 180),
        new(160, 214, 228), new(160, 162, 160), new(0, 0, 0),       new(0, 0, 0),
    ];
}
