using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_TAX_TAY_TXA_TYA_TSX_TXS
{
    [Fact]
    public void Test1_Tax_TransferAToX()
    {
        // Test 1: TAX — transfer A to X
        byte[] bytes = [
            0xA9, 0x42, // LDA #$42
            0xAA        // TAX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$42
        nes.Step(); // TAX
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0x42);
    }

    [Fact]
    public void Test2_Tax_ZeroFlag()
    {
        // Test 2: TAX — zero flag
        byte[] bytes = [
            0xA9, 0x00, // LDA #$00
            0xAA        // TAX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$00
        nes.Step(); // TAX
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0x00);
        Assert.True(nes.Cpu.Register.Status.Z);
    }

    [Fact]
    public void Test3_Tax_NegativeFlag()
    {
        // Test 3: TAX — negative flag
        byte[] bytes = [
            0xA9, 0x80, // LDA #$80
            0xAA        // TAX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$80
        nes.Step(); // TAX
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0x80);
        Assert.True(nes.Cpu.Register.Status.N);
    }

    [Fact]
    public void Test4_Tay_TransferAToY()
    {
        // Test 4: TAY — transfer A to Y
        byte[] bytes = [
            0xA9, 0x42, // LDA #$42
            0xA8        // TAY
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$42
        nes.Step(); // TAY
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.Y, 0x42);
    }

    [Fact]
    public void Test5_Txa_TransferXToA()
    {
        // Test 5: TXA — transfer X to A
        byte[] bytes = [
            0xA2, 0x42, // LDX #$42
            0x8A        // TXA
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDX #$42
        nes.Step(); // TXA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test6_Tya_TransferYToA()
    {
        // Test 6: TYA — transfer Y to A
        byte[] bytes = [
            0xA0, 0x42, // LDY #$42
            0x98        // TYA
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDY #$42
        nes.Step(); // TYA
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.A, 0x42);
    }

    [Fact]
    public void Test7_Tsx_TransferSpToX()
    {
        // Test 7: TSX — transfer SP to X
        byte[] bytes = [
            0xBA // TSX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // TSX
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.X, 0xFD);
    }

    [Fact]
    public void Test8_Tsx_NegativeFlag()
    {
        // Test 8: TSX — negative flag (SP=$FD has bit 7 set)
        byte[] bytes = [
            0xBA // TSX
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // TSX
        nes.Debug();

        Assert.True(nes.Cpu.Register.Status.N);
    }

    [Fact]
    public void Test9_Txs_TransferXToSp()
    {
        // Test 9: TXS — transfer X to SP
        byte[] bytes = [
            0xA2, 0xFF, // LDX #$FF
            0x9A        // TXS
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDX #$FF
        nes.Step(); // TXS
        nes.Debug();

        Assert.Equal<u8>(nes.Cpu.Register.SP, 0xFF);
    }

    [Fact]
    public void Test10_Txs_DoesNotModifyFlags()
    {
        // Test 10: TXS — doesn't modify flags
        byte[] bytes = [
            0xA2, 0x00, // LDX #$00 (sets Z=1)
            0x9A        // TXS
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDX #$00
        u8 statusBefore = nes.Cpu.Register.Status.Value;

        nes.Step(); // TXS
        nes.Debug();

        Assert.Equal(nes.Cpu.Register.Status.Value, statusBefore);
    }
}
