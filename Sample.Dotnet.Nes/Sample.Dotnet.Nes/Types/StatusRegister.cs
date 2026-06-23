using System.Text;

namespace Sample.Dotnet.Nes.Types;

public sealed class StatusRegister
{
    u8 _flags;

    public u8 Value => _flags;

    public StatusRegister(E_STATUS_FLAG flag)
    {
        _flags = (u8)(byte)flag;
    }

    public void Reset(u8 v)
    {
        _flags = v;
    }

    /// <summary>
    /// Negative
    /// </summary>
    public bool N
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_N);
        private set => _Set(E_STATUS_FLAG.FLAG_N, value);
    }

    /// <summary>
    /// Overflow
    /// </summary>
    public bool V
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_V);
        set => _Set(E_STATUS_FLAG.FLAG_V, value);
    }

    /// <summary>
    /// Unused
    /// </summary>
    public bool U
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_U);
        private set => _Set(E_STATUS_FLAG.FLAG_U, value);
    }

    /// <summary>
    /// Break
    /// </summary>
    public bool B
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_B);
        set => _Set(E_STATUS_FLAG.FLAG_B, value);
    }

    /// <summary>
    /// Decimal
    /// </summary>
    public bool D
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_D);
        set => _Set(E_STATUS_FLAG.FLAG_D, value);
    }

    /// <summary>
    /// Interrupt Disable
    /// </summary>
    public bool I
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_I);
        set => _Set(E_STATUS_FLAG.FLAG_I, value);
    }

    /// <summary>
    /// Zero
    /// </summary>
    public bool Z
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_Z);
        private set => _Set(E_STATUS_FLAG.FLAG_Z, value);
    }

    /// <summary>
    /// Carry
    /// </summary>
    public bool C
    {
        get => _IsOn(E_STATUS_FLAG.FLAG_C);
        set => _Set(E_STATUS_FLAG.FLAG_C, value);
    }

    public static implicit operator StatusRegister(int value)
    {
        return new StatusRegister((E_STATUS_FLAG)value);
    }

    private bool _IsOn(E_STATUS_FLAG flag)
    {
        return (_flags & (byte)flag) != 0;
    }

    private void _Set(E_STATUS_FLAG flag, bool isOn)
    {
        if (isOn)
        {
            _flags |= (u8)(byte)flag;
        }
        else
        {
            _flags &= ~(u8)(byte)flag;
        }
    }

    public void SetZfromValue(u8 v)
    {
        Z = v == 0;
    }

    public void SetNfromValue(u8 v)
    {
        N = (v & (byte)E_STATUS_FLAG.FLAG_N) != 0;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(N ? 'N' : '.');
        sb.Append(V ? 'V' : '.');
        sb.Append(U ? '.' : '@');
        sb.Append(B ? 'B' : '.');
        sb.Append(D ? 'D' : '.');
        sb.Append(I ? 'I' : '.');
        sb.Append(Z ? 'Z' : '.');
        sb.Append(C ? 'C' : '.');
        return sb.ToString();
    }
}
