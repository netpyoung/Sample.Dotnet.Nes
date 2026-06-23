using System;

namespace Sample.Dotnet.Nes;

public sealed class NesException : Exception
{
    public NesException(string message) : base(message) { }
}
