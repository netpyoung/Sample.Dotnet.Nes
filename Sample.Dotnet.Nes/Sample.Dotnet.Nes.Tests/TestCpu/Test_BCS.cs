using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_BCS
{
    [Fact]
    public void Test5_Bcs_BranchTaken()
    {
        // Test 5: BCS — branch taken (C=1)
        byte[] bytes = [
            0xA9, // LDA immediate
            0x50, // A = $50
            0xC9, // CMP immediate (sets C=1 when A >= value)
            0x30, // compare with $30
            0xB0, // BCS
            0x02  // offset +2
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$50
        nes.Step(); // CMP #$30  ; C=1
        nes.Step(); // BCS +2
        nes.Debug();

        Assert.Equal<u16>(nes.Cpu.Register.PC, 0x0008);
    }
}