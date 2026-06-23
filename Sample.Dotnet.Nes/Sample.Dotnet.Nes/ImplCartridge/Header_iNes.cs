using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sample.Dotnet.Nes.ImplCartridge;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct Header_iNes
{
    //| Byte |                                              |
    //| ---- | -------------------------------------------- |
    //| 0-3  | "NES" followed by byte 0x1A                  |
    //| 4    | Number of PRG - ROM blocks(each block = 16KB)|
    //| 5    | Number of CHR - ROM blocks(each block = 8KB) |
    //| 6    | Flags 6                                      |
    //| 7    | Flags 7                                      |
    //| 8-15 | Padding                                      |

    public const int HEADER_SIZE = 16;
    public const int PRG_BLOCK_SIZE = 0x4000; // 16kb
    public const int CHR_BLOCK_SIZE = 0x2000; // 8kb
    public const int TRAINER_SIZE = 512;

    const int PRG_SIZE_BYTE = 4;
    const int CHR_SIZE_BYTE = 5;
    const int FLAGS_6_BYTE = 6;
    const int FLAGS_7_BYTE = 7;

    static readonly byte[] MAGIC = [0x4E, 0x45, 0x53, 0x1A]; // [(byte)'N', (byte)'E', (byte)'S', 0x1A]

    public fixed byte Magic[4];
    public byte NumberOfPRG;
    public byte NumberOfCHR;
    public byte Flags6;
    public byte Flags7;
    public fixed byte Padding[8];

    public static void Validate(byte[] bytes)
    {
        Debug.Assert(sizeof(Header_iNes) == HEADER_SIZE);

        if (bytes.Length < HEADER_SIZE)
        {
            throw new NesException($"bytes.Length < HEADER_SIZE({HEADER_SIZE}) - {bytes.Length}");
        }

        if (bytes[0..4].SequenceCompareTo(MAGIC) != 0)
        {
            string actual = Convert.ToHexString(bytes[0..4]);
            string expected = Convert.ToHexString(MAGIC);
            throw new NesException($"magic number: bytes[0..4].SequenceCompareTo(magic) != 0 - expected:{expected} / actual:{actual}");
        }
    }

    public int GetMapperId()
    {
        byte lo = (byte)((byte)(Flags6 & 0b1111_0000) >> 4);
        byte hi = (byte)(Flags7 & 0b1111_0000);
        byte mapperId = (byte)(hi | lo);
        return mapperId;
    }

    public E_MIRROR_MODE GetMirrorMode()
    {
        if ((Flags6 & (byte)E_FLAGS_6.FOUR_SCREEN_MASK) != 0)
        {
            return E_MIRROR_MODE.FOUR_SCREEN;
        }

        if ((Flags6 & (byte)E_FLAGS_6.MIRROR_MASK) != 0)
        {
            return E_MIRROR_MODE.VERTICAL;
        }

        return E_MIRROR_MODE.HORIZONTAL;
    }

    public int GetPrgSize()
    {
        int prgSize = NumberOfPRG * PRG_BLOCK_SIZE;
        return prgSize;
    }

    public int GetChrSize()
    {
        int chrSize = NumberOfCHR * CHR_BLOCK_SIZE;
        return chrSize;
    }

    public bool IsBattery()
    {
        return (Flags6 & (byte)E_FLAGS_6.BATTERY_MASK) != 0;
    }

    public bool IsTrainer()
    {
        return (Flags6 & (byte)E_FLAGS_6.TRAINER_MASK) != 0;
    }
}
