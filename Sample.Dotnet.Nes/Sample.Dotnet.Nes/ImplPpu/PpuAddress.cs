using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplPpu;

public sealed class PpuAddress
{
    public readonly static u16 ADDRESS_PATTERN_TABLE_START = 0x0000;
    public readonly static u16 ADDRESS_PATTERN_TABLE_END = 0x1FFF;
    public readonly static u16 ADDRESS_NAMETABLE_START = 0x2000;
    public readonly static u16 ADDRESS_NAMETABLE_MIRROR_END = 0x3EFF;
    public readonly static u16 ADDRESS_PALETTE_START = 0x3F00;
    public readonly static u16 ADDRESS_PALETTE_MIRROR_END = 0x3FFF;

    public readonly static Region RegionPatternTable = new Region(ADDRESS_PATTERN_TABLE_START, ADDRESS_PATTERN_TABLE_END);
    public readonly static Region RegionNametable = new Region(ADDRESS_NAMETABLE_START, ADDRESS_NAMETABLE_MIRROR_END);
    public readonly static Region RegionPalette = new Region(ADDRESS_PALETTE_START, ADDRESS_PALETTE_MIRROR_END);

    public sealed class Region
    {
        public u16 Start { get; private set; }
        public u16 End { get; private set; }

        public Region(u16 start, u16 end)
        {
            Start = start;
            End = end;
        }

        public bool IsIn(u16 addr)
        {
            return Start <= addr && addr <= End;
        }
    }
}