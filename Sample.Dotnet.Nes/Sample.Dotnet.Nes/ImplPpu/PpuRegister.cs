using Sample.Dotnet.Nes.ImplPpu.PpuRegisters;
using Sample.Dotnet.Nes.Types;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sample.Dotnet.Nes.ImplPpu;

[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
public struct PpuRegister
{
    public PPUCTRL PPUCTRL; //$2000 PPUCTRL
    public PPUMASK PPUMASK; //$2001 PPUMASK
    public PPUSTATUS PPUSTATUS; //$2002 PPUSTATUS
    public u8 OAMADDR; //$2003 OAMADDR

    //$2004 OAMDATA
    //$2005 PPUSCROLL - x/y
    //$2006 PPUADDR - addr
    //$2007 PPUDATA

    public override readonly string ToString()
    {
        return $"PPUCTRL:{PPUCTRL} PPUMASK:{PPUMASK} PPUSTATUS:{PPUSTATUS} OAMADDR:{OAMADDR}";
    }
}
