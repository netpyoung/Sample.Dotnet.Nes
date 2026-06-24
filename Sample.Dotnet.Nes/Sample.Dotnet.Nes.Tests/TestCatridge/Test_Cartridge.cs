using Sample.Dotnet.Nes.ImplCartridge;

namespace Sample.Dotnet.Nes.Tests.TestCatridge;

public sealed class Test_Cartridge
{
    [Fact]
    public void Test2_Cartridge_RejectInvalidFile()
    {
        // Test 2: Invalid file
        NesException exception = Assert.Throws<NesException>(() =>
        {
            byte[] data = new byte[256];
            _ = new Cartridge(data);
        });

        Assert.Contains("magic number", exception.Message.ToLower());
    }
}
