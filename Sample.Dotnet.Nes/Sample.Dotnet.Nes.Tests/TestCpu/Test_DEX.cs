using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_DEX
{

    [Fact]
    public void Test1_Dex_SimpleDecrement()
    {
        // Test 1: Simple decrement
        byte[] bytes = [
            0xA2, 0x05, // LDX #$05
            0xCA        // DEX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDX #$05
        nes.Step(); // DEX
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0x04);
    }

    [Fact]
    public void Test2_Dex_ZeroFlag()
    {
        // Test 2: Zero flag
        byte[] bytes = [
            0xA2, 0x01, // LDX #$01
            0xCA        // DEX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDX #$01
        nes.Step(); // DEX
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0x00);
        Assert.True(nes.Cpu.Register.Status.Z);
    }

    [Fact]
    public void Test3_Dex_WrapAndNegativeFlag()
    {
        // Test 3: Wrap + Negative flag
        byte[] bytes = [
            0xCA // DEX (X starts at $00, wraps to $FF)
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // DEX (X=$00 wraps to $FF)
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0xFF);
        Assert.True(nes.Cpu.Register.Status.N);
    }
}
