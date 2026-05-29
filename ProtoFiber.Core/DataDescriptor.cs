namespace ProtoFiber.Core;

/// <summary>
/// Represents either a data input or output.
/// </summary>
public readonly struct DataDescriptor : IConnectorDescriptor
{

    private readonly byte[]? _connections;

    byte[]? IConnectorDescriptor.Connections => _connections;

    public Type Type { get; }

    internal DataDescriptor(byte[]? linkedOpposites, Type type)
    {
        _connections = linkedOpposites;
        Type = type;
    }

}
