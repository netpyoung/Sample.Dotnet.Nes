using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_LDA
{
    [Fact]
    public void Test1_Immediate()
    {
        byte[] bytes = [
            0xA9, // LDA immediate
            0x42, // 66 // 0b0000_0000
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);
        nes.Step(); // LDA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
        Assert.False(nes.Cpu.Register.Status.Z);
        Assert.False(nes.Cpu.Register.Status.N);
    }

    [Fact]
    public void Test2_zero()
    {
        byte[] bytes = [
            0xA9, // LDA immediate
            0x00, // 0 // 0b0000_0000
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);
        nes.Step(); // LDA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x00);
        Assert.True(nes.Cpu.Register.Status.Z);
        Assert.False(nes.Cpu.Register.Status.N);
    }


    [Fact]
    public void Test3_neg()
    {
        byte[] bytes = [
            0xA9, // LDA immediate
            0x80, // -128 // 0b1000_0000
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);
        nes.Step(); // LDA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x80);
        Assert.False(nes.Cpu.Register.Status.Z);
        Assert.True(nes.Cpu.Register.Status.N);
    }

    [Fact]
    public void Test4_ZeroPage()
    {
        byte[] bytes = [
            0xA5, // LDA zero page
            0x10,
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.CpuBus.Write(0x10, 0x42);
        nes.LoadBytes(bytes);
        nes.Step(); // LDA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test5_ZeroPage_X()
    {
        byte[] bytes = [
            0xA2, // LDX immediate
            0x05, // X = $05
            0xB5, // LDA zero page,X
            0x10  // base address $10
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.CpuBus.Write(0x15, 0x42);
        nes.LoadBytes(bytes);
        nes.Step(); // LDX immediate
        nes.Step(); // LDA zero page,X
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test6_Absolute()
    {
        byte[] bytes = [
            0xAD, // LDA absolute
            0x00, // low byte
            0x02  // high byte ($0200)
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.CpuBus.Write(0x0200, 0x42);
        nes.LoadBytes(bytes);
        nes.Step(); // LDA absolute
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test7_Absolute_X()
    {
        byte[] bytes = [
            0xA2, // LDX immediate
            0x05, // X = $05
            0xBD, // LDA absolute,X
            0x00, // low byte
            0x02  // high byte ($0200)
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.CpuBus.Write(0x0205, 0x42);
        nes.LoadBytes(bytes);
        nes.Step(); // LDX immediate
        nes.Step(); // LDA absolute,X
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test8_AbsoluteY()
    {
        // Test 8: Absolute,Y
        byte[] bytes = [
            0xA0, // LDY immediate
            0x05, // Y = $05
            0xB9, // LDA absolute,Y
            0x00, // low byte
            0x02  // high byte ($0200)
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.CpuBus.Write(0x0205, 0x42); // $0200 + $05 = $0205
        nes.LoadBytes(bytes);

        nes.Step(); // LDY #$05
        nes.Step(); // LDA $0200,Y
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test9_IndirectX()
    {
        // Test 9: Indirect,X (Indexed Indirect)
        byte[] bytes = [
            0xA2, // LDX immediate
            0x05, // X = $05
            0xA1, // LDA ($nn,X)
            0x20  // base address $20
        ];

        Nes nes = Nes.CreateDummyCartridge();
        // Pointer at $25/$26 (base $20 + X $05) points to $0200
        nes.CpuBus.Write(0x25, 0x00); // low byte of pointer
        nes.CpuBus.Write(0x26, 0x02); // high byte of pointer
        nes.CpuBus.Write(0x0200, 0x42);
        nes.LoadBytes(bytes);

        nes.Step(); // LDX #$05
        nes.Step(); // LDA ($20,X)
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test10_IndirectY()
    {
        // Test 10: Indirect,Y (Indirect Indexed)
        byte[] bytes = [
            0xA0, // LDY immediate
            0x05, // Y = $05
            0xB1, // LDA ($nn),Y
            0x20  // base address $20
        ];

        Nes nes = Nes.CreateDummyCartridge();
        // Pointer at $20/$21 points to $0200, then add Y=$05 => $0205
        nes.CpuBus.Write(0x20, 0x00); // low byte of pointer
        nes.CpuBus.Write(0x21, 0x02); // high byte of pointer
        nes.CpuBus.Write(0x0205, 0x42);
        nes.LoadBytes(bytes);

        nes.Step(); // LDY #$05
        nes.Step(); // LDA ($20),Y
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }
}
