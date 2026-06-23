using System;
using System.Diagnostics;
using System.Numerics;

namespace Sample.Dotnet.Nes.Types;

[DebuggerDisplay("0x{Value.ToString(\"X4\"),nq}")]
public readonly struct u16 :
    IComparisonOperators<u16, u16, bool>,
    IEqualityOperators<u16, u16, bool>,
    IEquatable<u16>,
    IBitwiseOperators<u16, u16, u16>,
    IFormattable
{
    public ushort Value { get; }

    public u16(ushort value)
    {
        Value = value;
    }

    public static implicit operator u16(int value) => new((ushort)value);
    public static implicit operator int(u16 v) => v.Value;

    public static explicit operator ushort(u16 v) => v.Value;
    public static explicit operator u16(ushort v) => new(v);
    public static explicit operator u8(u16 v) => v.Value;

    public static u16 operator +(u16 a, u16 b) => new((ushort)(a.Value + b.Value));
    public static bool operator >(u16 left, u16 right) => left.Value > right.Value;
    public static bool operator >=(u16 left, u16 right) => left.Value >= right.Value;
    public static bool operator <(u16 left, u16 right) => left.Value < right.Value;
    public static bool operator <=(u16 left, u16 right) => left.Value <= right.Value;
    public static bool operator ==(u16 left, u16 right) => left.Value == right.Value;
    public static bool operator !=(u16 left, u16 right) => left.Value != right.Value;
    public static u16 operator &(u16 left, u16 right) => new u16((ushort)(left.Value & right.Value));
    public static u16 operator |(u16 left, u16 right) => new u16((ushort)(left.Value | right.Value));
    public static u16 operator ^(u16 left, u16 right) => new u16((ushort)(left.Value ^ right.Value));
    public static u16 operator ~(u16 value) => new u16((ushort)~value.Value);

    public bool Equals(u16 other) => Value == other.Value;

    public override bool Equals(object? obj)
    {
        return obj is u16 other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
    public string ToString(string? format, IFormatProvider? provider)
    {
        return Value.ToString(format, provider);
    }
}
