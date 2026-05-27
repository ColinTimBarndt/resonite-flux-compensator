namespace ProtoFiber.Core.Graph;

public readonly record struct NodeConnection(
    ConnectionType Type,
    byte Slot,
    NodeId Node
) : IEquatable<NodeConnection>;
