using Sample.Dotnet.Nes.Types;

namespace Sample.Dotnet.Nes.ImplController;

public sealed class Controller
{
    u8 _buttons;
    u8 _shiftRegister;
    bool _isStrobe;

    public void Press(E_BUTTON button)
    {
        _buttons |= (u8)(byte)button;
    }
    public void Release(E_BUTTON button)
    {
        _buttons &= ~(u8)(byte)button;
    }

    public void Write(u8 v)
    {
        _isStrobe = (v & 1) != 0;
        if (_isStrobe)
        {
            _shiftRegister = _buttons;
        }
    }

    public bool Read()
    {
        if (_isStrobe)
        {
            return (_buttons & (u8)(byte)E_BUTTON.A) != 0;
        }

        bool isOn = (_shiftRegister & 1) != 0;
        _shiftRegister >>= 1;
        return isOn;
    }
}
