using System.Diagnostics;

namespace ProtoFiber.Core;

[DebuggerDisplay("0x{_value:X8}")]
public readonly struct NodeTypeId(uint value) : IEquatable<NodeTypeId>
{

    public static readonly NodeTypeId Invalid = new(uint.MaxValue);

    private readonly uint _value = value;

    public override string ToString() => $"T{_value:X8}";

    public static explicit operator uint(NodeTypeId id) => id._value;

    public override bool Equals(object? obj) => obj is NodeTypeId type && Equals(type);

    public bool Equals(NodeTypeId other) => this == other;

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(NodeTypeId left, NodeTypeId right) => left._value == right._value;

    public static bool operator !=(NodeTypeId left, NodeTypeId right) => left._value != right._value;

}
