using Sample.Dotnet.Nes.ImplController;

namespace Sample.Dotnet.Nes.Tests.TestController;

public sealed class Test_Controller
{
    [Fact]
    public void Test_01()
    {
        // # Test 1: Press Start and read it back
        Controller ctrl = new Controller();
        ctrl.Press(E_BUTTON.START);
        ctrl.Write(1);
        ctrl.Write(0);

        string[] names = ["A", "B", "Select", "Start", "Up", "Down", "Left", "Right"];
        bool[] expected = [false, false, false, true, false, false, false, false];

        bool[] actual = new bool[8];
        foreach ((int i, string v) in names.Index())
        {
            bool bit = ctrl.Read();
            actual[i] = bit;

            Console.WriteLine($"   Read #{i} ({v}): {bit}");
        }

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_02()
    {
        // # Test 2: Press multiple buttons
        Controller ctrl = new Controller();
        ctrl.Press(E_BUTTON.A);
        ctrl.Press(E_BUTTON.START);
        ctrl.Press(E_BUTTON.RIGHT);
        ctrl.Write(1);
        ctrl.Write(0);

        string[] names = ["A", "B", "Select", "Start", "Up", "Down", "Left", "Right"];
        bool[] expected = [true, false, false, true, false, false, false, true];

        bool[] actual = new bool[8];
        foreach ((int i, string v) in names.Index())
        {
            bool bit = ctrl.Read();
            actual[i] = bit;

            Console.WriteLine($"   Read #{i} ({v}): {bit}");
        }

        Assert.Equal(expected, actual);
    }
}
