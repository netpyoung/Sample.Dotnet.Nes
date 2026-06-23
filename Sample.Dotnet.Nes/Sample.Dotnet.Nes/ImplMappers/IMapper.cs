using Sample.Dotnet.Nes.ImplCartridge;
using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplMappers;

public interface IMapper
{
    E_MIRROR_MODE? MirrorModeOrNull { get; }

    u8 Prg_Read(u16 addr);
    u8 Chr_Read(u16 addr);
    u8 Chr_Write(u16 addr, u8 v);
}
