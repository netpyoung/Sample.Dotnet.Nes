using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_PHA_PHP_PLA_PLP
{
    [Fact]
    public void Test1_Pha_PushAToStack()
    {
        // Test 1: PHA — push A to stack
        byte[] bytes = [
            0xA9, 0x42, // LDA #$42
            0x48        // PHA
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$42
        nes.Step(); // PHA
        nes.Debug();

        u8 valueOnStack = nes.CpuBus.Read(0x01FD);

        Assert.Equal<u8>(valueOnStack, 0x42);
        Assert.Equal<u8>(nes.Cpu.Register.SP, 0xFC);
    }

    [Fact]
    public void Test2_Pla_PhaPlaRoundTrip()
    {
        // Test 2: PHA + PLA round-trip
        byte[] bytes = [
            0xA9, 0x42, // LDA #$42
            0x48,       // PHA
            0xA9, 0x00, // LDA #$00 (clear A)
            0x68        // PLA
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$42
        nes.Step(); // PHA
        nes.Step(); // LDA #$00
        nes.Step(); // PLA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
        Assert.Equal<u8>(nes.Cpu.Register.SP, 0x1FD);
    }

    [Fact]
    public void Test3_Pla_ZeroFlag()
    {
        // Test 3: PLA — zero flag
        byte[] bytes = [
            0xA9, 0x00, // LDA #$00
            0x48,       // PHA
            0xA9, 0x42, // LDA #$42 (clear Z)
            0x68        // PLA (pulls $00, sets Z)
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$00
        nes.Step(); // PHA
        nes.Step(); // LDA #$42
        nes.Step(); // PLA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x00);
        Assert.True(nes.Cpu.Register.Status.Z);
    }

    [Fact]
    public void Test4_Pla_NegativeFlag()
    {
        // Test 4: PLA — negative flag
        byte[] bytes = [
            0xA9, 0x80, // LDA #$80
            0x48,       // PHA
            0xA9, 0x00, // LDA #$00
            0x68        // PLA (pulls $80, sets N)
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$80
        nes.Step(); // PHA
        nes.Step(); // LDA #$00
        nes.Step(); // PLA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x80);
        Assert.True(nes.Cpu.Register.Status.N);
    }

    [Fact]
    public void Test5_Php_PushesStatusWithBandUSet()
    {
        // Test 5: PHP — pushes status with B and U set
        byte[] bytes = [
            0x08 // PHP
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // PHP
        nes.Debug();

        u8 pushed = nes.CpuBus.Read(0x01FD);

        bool bSet = (pushed & 0x10) != 0;
        bool uSet = (pushed & 0x20) != 0;

        Assert.True(bSet);
        Assert.True(uSet);
    }

    [Fact]
    public void Test6_Plp_PhpPlpRoundTrip()
    {
        // Test 6: PHP + PLP round-trip
        byte[] bytes = [
            0x38, // SEC (set C)
            0x08, // PHP
            0x18, // CLC (clear C)
            0x28  // PLP (restore C=1)
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // SEC
        nes.Step(); // PHP
        nes.Step(); // CLC
        nes.Step(); // PLP
        nes.Debug();

        Assert.True(nes.Cpu.Register.Status.C);
        Assert.False(nes.Cpu.Register.Status.B);
        Assert.True(nes.Cpu.Register.Status.U);
    }
}
