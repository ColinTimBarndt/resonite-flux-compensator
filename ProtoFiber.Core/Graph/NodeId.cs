using System.Diagnostics;

namespace ProtoFiber.Core.Graph;

[DebuggerDisplay("0x{_value:X8}")]
public readonly struct NodeId(uint value) : IEquatable<NodeId>
{

    public static readonly NodeId Invalid = new(uint.MaxValue);

    private readonly uint _value = value;

    public override string ToString() => $"N{_value:X8}";

    public static explicit operator int(NodeId id) => (int)id._value;

    public static explicit operator uint(NodeId id) => id._value;

    public override bool Equals(object? obj) => obj is NodeId type && Equals(type);

    public bool Equals(NodeId other) => this == other;

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(NodeId left, NodeId right) => left._value == right._value;

    public static bool operator !=(NodeId left, NodeId right) => left._value != right._value;

}
