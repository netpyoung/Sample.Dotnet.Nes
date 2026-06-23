using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.Tests.TestCpu;

public sealed class Test_BMI
{
    [Fact]
    public void Test7_Bmi_BranchTaken()
    {
        // Test 7: BMI — branch taken (N=1)
        byte[] bytes = [
            0xA9, // LDA immediate
            0x80, // A = $80 (negative, N=1)
            0x30, // BMI
            0x02  // offset +2
        ];

        Nes nes = Nes.CreateDummyCartridge();
        nes.LoadBytes(bytes);

        nes.Step(); // LDA #$80
        nes.Step(); // BMI +2
        nes.Debug();

        Assert.Equal<u16>(nes.Cpu.Register.PC, 0x0006);
    }
}