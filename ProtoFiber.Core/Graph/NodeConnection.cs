namespace ProtoFiber.Core.Graph;

/// <summary>
/// Represents one side of a connection between two connectors on a ´<see cref="Node"/>.
/// </summary>
/// <param name="Type">
/// Either <see cref="ConnectorType.Invalid"/> (no connection),
/// <see cref="ConnectorType.Data"/>, or
/// <see cref="ConnectorType.Flow"/>.
/// </param>
/// <param name="Node">
/// The node this side is connected to.
/// </param>
/// <param name="Slot">
/// The index of the connector this side is connected to.
/// </param>
public readonly record struct NodeConnection(
    ConnectorType Type,
    NodeId Node,
    byte Slot
) : IEquatable<NodeConnection>;
