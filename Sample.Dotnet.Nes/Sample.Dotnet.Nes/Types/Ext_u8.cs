namespace Sample.Dotnet.Nes.Types;

public static class Ext_u8
{
    public static bool IsFlagOn(this u8 x, E_STATUS_FLAG flag)
    {
        return (x & (byte)flag) != 0;
    }
}