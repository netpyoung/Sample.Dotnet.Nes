using System;
using System.Diagnostics;
using System.Numerics;

namespace Sample.Dotnet.Nes.Types;

[DebuggerDisplay("{Value.ToString(),nq}")]
public readonly struct i8 :
    IComparisonOperators<i8, i8, bool>,
    IEqualityOperators<i8, i8, bool>,
    IEquatable<i8>,
    IBitwiseOperators<i8, i8, i8>,
    IFormattable
{
    public sbyte Value { get; } = 0;

    public i8(sbyte value)
    {
        Value = value;
    }

    public static implicit operator i8(int value) => new((sbyte)value);
    public static implicit operator int(i8 v) => v.Value;

    public static explicit operator sbyte(i8 v) => v.Value;
    public static explicit operator i8(sbyte v) => new(v);
    public static explicit operator u16(i8 v) => v.Value;
    public static explicit operator i8(u8 v) => v.Value;

    public static i8 operator +(i8 a, i8 b) => new((sbyte)(a.Value + b.Value));
    public static bool operator >(i8 left, i8 right) => left.Value > right.Value;
    public static bool operator >=(i8 left, i8 right) => left.Value >= right.Value;
    public static bool operator <(i8 left, i8 right) => left.Value < right.Value;
    public static bool operator <=(i8 left, i8 right) => left.Value <= right.Value;
    public static bool operator ==(i8 left, i8 right) => left.Value == right.Value;
    public static bool operator !=(i8 left, i8 right) => left.Value != right.Value;
    public static i8 operator &(i8 left, i8 right) => left.Value & right.Value;
    public static i8 operator |(i8 left, i8 right) => left.Value | right.Value;
    public static i8 operator ^(i8 left, i8 right) => left.Value ^ right.Value;
    public static i8 operator ~(i8 x) => ~x.Value;
    public bool Equals(i8 other) => Value == other.Value;

    public override bool Equals(object? obj)
    {
        return obj is i8 other && Value == other.Value;
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