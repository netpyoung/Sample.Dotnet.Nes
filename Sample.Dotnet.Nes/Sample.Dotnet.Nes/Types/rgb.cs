using System.Runtime.InteropServices;

namespace Sample.Dotnet.Nes.Types;

#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
[StructLayout(LayoutKind.Auto)]
public struct rgb
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
{
    public u8 r;
    public u8 g;
    public u8 b;

    public rgb(u8 r, u8 g, u8 b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
}
