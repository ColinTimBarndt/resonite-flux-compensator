using System.Collections.Immutable;

namespace ProtoFiber.Core;

public readonly struct ImpulseDescriptor : IConnectorDescriptor
{

    private readonly byte[]? _connections;

    byte[]? IConnectorDescriptor.Connections => _connections;

    private readonly Flags _flags;

    private ImpulseDescriptor(
        byte[]? linkedOperations,
        Flags flags
    ) : this()
    {
        _connections = linkedOperations;
        _flags = flags;
    }

    public bool IsContinuation => _flags == default;

    public bool IsCall => _flags.HasFlag(Flags.Call);

    public bool IsAsync => _flags.HasFlag(Flags.Async);

    public bool IsResumption => _flags.HasFlag(Flags.Resumption);

    [Flags]
    private enum Flags
    {
        Call = 1,
        Async = 2,
        Resumption = 4,
    }

    internal static ImpulseDescriptor Continuation(byte[]? linkedOperations)
        => new(linkedOperations, default);

    internal static ImpulseDescriptor Call(byte[]? linkedOperations)
        => new(linkedOperations, Flags.Call);

    internal static ImpulseDescriptor AsyncCall(byte[]? linkedOperations)
        => new(linkedOperations, Flags.Call | Flags.Async);

    internal static ImpulseDescriptor Resumption(byte[]? linkedOperations)
        => new(linkedOperations, Flags.Call | Flags.Resumption);

    internal static ImpulseDescriptor AsyncResumption(byte[]? linkedOperations)
        => new(linkedOperations, Flags.Call | Flags.Async | Flags.Resumption);


}
