using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_INX
{
    [Fact]
    public void Test1()
    {
        byte[] bytes = [
            0xE8, // INX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);
        nes.Step(); // INX
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0x01);
    }

    [Fact]
    public void Test2()
    {
        byte[] bytes = [
            0xE8, // INX
            0xE8, // INX
            0xE8, // INX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);
        for (int i = 0; i < 3; ++i)
        {
            nes.Step(); // INX
            nes.Debug();
        }


        Assert.Equal<u8>(nes.Cpu.Register.X, 0x03);
    }

    [Fact]
    public void Test3()
    {
        byte[] bytes = new byte[256];
        Array.Fill<byte>(bytes, 0xE8); // INX

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);
        for (int i = 0; i < 256; ++i)
        {
            nes.Step(); // INX
            nes.Debug();
        }


        Assert.Equal<u8>(nes.Cpu.Register.X, 0); // 0 ~ 127, -128 ~ 0
        Assert.True(nes.Cpu.Register.Status.Z);
    }

    [Fact]
    public void Test4()
    {
        byte[] bytes = new byte[128];
        Array.Fill<byte>(bytes, 0xE8); // INX

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);
        for (int i = 0; i < 128; ++i)
        {
            nes.Step(); // INX
            nes.Debug();
        }


        Assert.Equal<u8>(nes.Cpu.Register.X, 0x80); // -128
        Assert.True(nes.Cpu.Register.Status.N);
    }
}