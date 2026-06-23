using Sample.Dotnet.Nes.ImplMappers;
using Sample.Dotnet.Nes.Types;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Sample.Dotnet.Nes.ImplCartridge;

public sealed class Cartridge
{
    public Header_iNes _header;

    private readonly byte[] _data;
    private readonly byte[] _chrRamData;

    public ReadonlySlice<byte> PrgRom;
    public ReadonlySlice<byte> ChrRom;
    public Slice<byte> ChrRam;

    public int MapperId { get; }
    public bool IsBattery { get; }
    public E_MIRROR_MODE MirrorMode { get; }
    public bool IsUseChrRam { get; }
    public IMapper Mapper { get; }

    public static Cartridge CreateFromPath(string path)
    {
        if (!File.Exists(path))
        {
            throw new NesException($"!File.Exists(path) - {path}");
        }

        byte[] bytes = File.ReadAllBytes(path);
        return new Cartridge(bytes);
    }

    public static Cartridge CreateDummyCartridge()
    {
        byte[] bytes = new byte[16 + 16 * 1024];
        bytes[0] = (byte)'N';
        bytes[1] = (byte)'E';
        bytes[2] = (byte)'S';
        bytes[3] = 0x1A;
        bytes[4] = 1; // 16KB PRG
        return new Cartridge(bytes);
    }

    public Cartridge(byte[] bytes)
    {
        Header_iNes.Validate(bytes);
        Header_iNes header = MemoryMarshal.Read<Header_iNes>(bytes[0..Header_iNes.HEADER_SIZE]);

        int prgStart;
        if (header.IsTrainer())
        {
            prgStart = Header_iNes.HEADER_SIZE + Header_iNes.TRAINER_SIZE;
        }
        else
        {
            prgStart = Header_iNes.HEADER_SIZE;
        }

        int prgSize = header.GetPrgSize();
        PrgRom = bytes.ToReadonlySlice(prgStart, prgSize);

        int chrSize = header.GetChrSize();
        if (chrSize > 0)
        {
            _chrRamData = [];

            ChrRam = _chrRamData.ToSlice();
            ChrRom = bytes.ToReadonlySlice(prgStart + prgSize, chrSize);
            IsUseChrRam = false;
        }
        else
        {
            _chrRamData = new byte[Header_iNes.CHR_BLOCK_SIZE];

            ChrRam = _chrRamData.ToSlice();
            ChrRom = new byte[8192].ToReadonlySlice(); // 8kb
            IsUseChrRam = true;
        }

        _data = bytes;
        _header = header;
        Mapper = new Mapper000(this);
        MapperId = header.GetMapperId();
        MirrorMode = header.GetMirrorMode();
        IsBattery = header.IsBattery();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"PRG - ROM: {PrgRom.Length / 1024:00}KB ({PrgRom.Length} bytes)");
        if (!IsUseChrRam)
        {
            sb.AppendLine($"CHR - ROM: {ChrRom.Length / 1024:00}KB ({ChrRom.Length} bytes)");
        }
        else
        {
            sb.AppendLine($"CHR - RAM: {ChrRam.Length / 1024:00}KB ({ChrRam.Length} bytes)");
        }
        sb.AppendLine($"Mapper: {MapperId}");
        sb.AppendLine($"Mirror: {MirrorMode}");
        sb.AppendLine($"Battery: {IsBattery}");
        return sb.ToString();
    }
}
