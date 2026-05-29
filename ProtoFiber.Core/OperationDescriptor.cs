namespace ProtoFiber.Core;

public readonly struct OperationDescriptor : IConnectorDescriptor
{

    private readonly byte[]? _connections;

    byte[]? IConnectorDescriptor.Connections => _connections;

    public readonly bool IsAsync { get; }

    internal OperationDescriptor(
        byte[]? linkedOperations,
        bool isAsync
    )
    {
        _connections = linkedOperations;
        IsAsync = isAsync;
    }

}
