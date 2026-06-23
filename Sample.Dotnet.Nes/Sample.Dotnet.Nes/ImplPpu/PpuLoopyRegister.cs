using Sample.Dotnet.Nes.Types;
using System.Diagnostics;

namespace Sample.Dotnet.Nes.ImplPpu;

[DebuggerDisplay("{ToString(),nq}")]
public struct PpuLoopyRegister
{
    /// <summary>
    /// Current VRAM Address
    /// </summary>
    public PpuVramAddress V;

    /// <summary>
    /// Temporary VRAM Address
    /// </summary>
    public PpuVramAddress T;

    /// <summary>
    /// Fine X Scroll
    /// </summary>
    public u8 FineX;

    /// <summary>
    /// Write Toggle Latch
    /// </summary>
    public bool W;

    public override string ToString()
    {
        return $"V:{V} T:{T} FineX:{FineX} W:{W}";
    }
}
