using System;
using System.Diagnostics;
using System.Numerics;

namespace Sample.Dotnet.Nes.Types;

[DebuggerDisplay("0x{Value.ToString(\"X2\"),nq}")]
public readonly struct u8 :
    IComparisonOperators<u8, u8, bool>,
    IEqualityOperators<u8, u8, bool>,
    IEquatable<u8>,
    IBitwiseOperators<u8, u8, u8>,
    IFormattable
{
    public byte Value { get; } = 0;

    public u8(byte value)
    {
        Value = value;
    }

    public static implicit operator u8(int value) => new((byte)value);
    public static implicit operator int(u8 v) => v.Value;

    public static explicit operator byte(u8 v) => v.Value;
    public static explicit operator u8(byte v) => new(v);
    public static explicit operator u16(u8 v) => (u16)v.Value;
    public static explicit operator u8(bool v)
    {
        if (v)
        {
            return 1;
        }
        return 0;
    }

    public static u8 operator +(u8 a, u8 b) => new((byte)(a.Value + b.Value));
    public static bool operator >(u8 left, u8 right) => left.Value > right.Value;
    public static bool operator >=(u8 left, u8 right) => left.Value >= right.Value;
    public static bool operator <(u8 left, u8 right) => left.Value < right.Value;
    public static bool operator <=(u8 left, u8 right) => left.Value <= right.Value;
    public static bool operator ==(u8 left, u8 right) => left.Value == right.Value;
    public static bool operator !=(u8 left, u8 right) => left.Value != right.Value;
    public static u8 operator &(u8 left, u8 right) => left.Value & right.Value;
    public static u8 operator |(u8 left, u8 right) => left.Value | right.Value;
    public static u8 operator ^(u8 left, u8 right) => left.Value ^ right.Value;
    public static u8 operator ~(u8 x) => ~x.Value;

    public bool Equals(u8 other) => Value == other.Value;

    public override bool Equals(object? obj)
    {
        return obj is u8 other && Value == other.Value;
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