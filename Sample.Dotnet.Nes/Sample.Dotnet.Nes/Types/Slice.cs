using System.Diagnostics;

namespace Sample.Dotnet.Nes.Types;

public readonly struct Slice<T>
{
    private readonly T[] _arr;
    private readonly int _offset;

    public int Length { get; }

    public T this[int index]
    {
        readonly get
        {
            Debug.Assert(0 <= index && index < Length);
            return _arr[_offset + index];
        }
        set
        {
            Debug.Assert(0 <= index && index < Length);
            _arr[_offset + index] = value;
        }
    }

    public Slice(T[] arr, int offset, int length)
    {
        _arr = arr;
        _offset = offset;
        Length = length;
    }
}

#pragma warning disable MA0048 // File name must match type name
public readonly struct ReadonlySlice<T>
#pragma warning restore MA0048 // File name must match type name
{
    private readonly T[] _arr;
    private readonly int _offset;

    public int Length { get; }
    public T this[int index]
    {
        get
        {
            Debug.Assert(0 <= index && index < Length);
            return _arr[_offset + index];
        }
    }

    public ReadonlySlice(T[] arr, int offset, int length)
    {
        _arr = arr;
        _offset = offset;
        Length = length;
    }
}

#pragma warning disable MA0048 // File name must match type name
public static class Ex_Slice
#pragma warning restore MA0048 // File name must match type name
{
    public static Slice<T> ToSlice<T>(this T[] arr)
    {
        return new Slice<T>(arr, 0, arr.Length);
    }

    public static Slice<T> ToSlice<T>(this T[] arr, int offset, int length)
    {
        return new Slice<T>(arr, offset, length);
    }

    public static ReadonlySlice<T> ToReadonlySlice<T>(this T[] arr, int offset, int length)
    {
        return new ReadonlySlice<T>(arr, offset, length);
    }

    public static ReadonlySlice<T> ToReadonlySlice<T>(this T[] arr)
    {
        return new ReadonlySlice<T>(arr, 0, arr.Length);
    }
}
