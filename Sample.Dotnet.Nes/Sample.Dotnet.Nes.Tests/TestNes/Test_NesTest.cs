using Sample.Dotnet.Nes.Types;
using System.Reflection;

namespace Sample.Dotnet.Nes.Tests.TestNes;


public sealed class Test_NesTest
{
    static class RomPath
    {
        public const string NES_TEST = ".roms/test/nestest.nes";
        public const string NES_TEST_LOG = ".roms/test/nestest.log";
    }
    static readonly string SOLUTION_DIR = GetSolutionDirectory();

    public static string GetSolutionDirectory()
    {
        DirectoryInfo? currentDir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);

        while (currentDir != null && currentDir.GetFiles("*.slnx").Length == 0)
        {
            currentDir = currentDir.Parent;
        }

        return currentDir?.FullName ?? throw new DirectoryNotFoundException("Solution directory not found.");
    }

    [Fact]
    public void Test1()
    {
        string romPath = Path.Combine(SOLUTION_DIR, RomPath.NES_TEST);
        Nes nes = Nes.CreateFromPath(romPath);
        nes.Cpu.Register.PC = 0xC000;

        string logPath = Path.Combine(SOLUTION_DIR, RomPath.NES_TEST_LOG);

        string[] lines = File.ReadAllLines(logPath);
        foreach ((int i, string line) in lines.Index())
        {
            u16 expected_pc = Convert.ToUInt16(line[0..4], 16);
            u8 expected_a = Convert.ToByte(line[50..52], 16);
            u8 expected_x = Convert.ToByte(line[55..57], 16);
            u8 expected_y = Convert.ToByte(line[60..62], 16);
            u8 expected_status = Convert.ToByte(line[65..67], 16);
            u8 expected_sp = Convert.ToByte(line[71..73], 16);

            Assert.Equal(expected_pc, nes.Cpu.Register.PC);
            Assert.Equal(expected_a, nes.Cpu.Register.A);
            Assert.Equal(expected_x, nes.Cpu.Register.X);
            Assert.Equal(expected_y, nes.Cpu.Register.Y);
            Assert.Equal(expected_status, nes.Cpu.Register.Status.Value);
            Assert.Equal(expected_sp, nes.Cpu.Register.SP);

            try
            {
                nes.Step();
            }
            catch (NesException ex)
            {
                Assert.Equal(5003, i);

                if (ex.Message == "UnknownOpcode")
                {
                    Console.WriteLine($"{i} // {line}");
                    break;
                }
            }
        }
    }
}
