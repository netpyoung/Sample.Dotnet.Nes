using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_INY
{
    [Fact]
    public void Test1_Iny_SimpleIncrement()
    {
        // Test 1: Simple increment
        byte[] bytes = [
            0xC8 // INY
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // INY
        nes.Debug();

        bool isZero = (nes.Cpu.Register.Status.Value & 0x02) != 0;
        bool isNegative = (nes.Cpu.Register.Status.Value & 0x80) != 0;

        Assert.Equal<u8>(nes.Cpu.Register.Y, 1);
        Assert.False(isZero);
        Assert.False(isNegative);
    }

    [Fact]
    public void Test2_Iny_WrapAndZeroFlag()
    {
        // Test 2: Wrap + Zero flag
        byte[] bytes = new byte[256];
        Array.Fill<byte>(bytes, 0xC8);

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        // INY x255
        for (int i = 0; i < 255; i++)
        {
            nes.Step();
        }
        nes.Debug();

        // INY (wrap)
        nes.Step();
        nes.Debug();

        bool isZero = (nes.Cpu.Register.Status.Value & 0x02) != 0;

        Assert.Equal<u8>(nes.Cpu.Register.Y, 0);
        Assert.True(isZero);
    }

    [Fact]
    public void Test3_Iny_NegativeFlag()
    {
        // Test 3: Negative flag
        byte[] bytes = new byte[128];
        Array.Fill<byte>(bytes, 0xC8);

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        // INY x128
        for (int i = 0; i < 128; i++)
        {
            nes.Step();
        }
        nes.Debug();

        bool isNegative = (nes.Cpu.Register.Status.Value & 0x80) != 0;

        Assert.Equal<u8>(nes.Cpu.Register.Y, 0x80);
        Assert.True(isNegative);
    }
}