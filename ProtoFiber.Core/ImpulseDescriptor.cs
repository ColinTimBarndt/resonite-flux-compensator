using System.Collections.Immutable;

namespace ProtoFiber.Core;

public readonly struct ImpulseDescriptor
{

    private readonly ushort[]? _linkedOperations;

    private readonly Flags _flags;

    private ImpulseDescriptor(ushort[]? linkedOperations, Flags flags) : this()
    {
        _linkedOperations = linkedOperations;
        _flags = flags;
    }

    public bool IsConnectedTo(int operation) => IsConnectedTo((ushort)operation);
    public bool IsConnectedTo(ushort operation) => _linkedOperations is null || _linkedOperations.Contains(operation);

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

    internal static ImpulseDescriptor Continuation(ushort[]? linkedOperations)
        => new(linkedOperations, default);

    internal static ImpulseDescriptor Call(ushort[]? linkedOperations)
        => new(linkedOperations, Flags.Call);

    internal static ImpulseDescriptor AsyncCall(ushort[]? linkedOperations)
        => new(linkedOperations, Flags.Call | Flags.Async);

    internal static ImpulseDescriptor Resumption(ushort[]? linkedOperations)
        => new(linkedOperations, Flags.Call | Flags.Resumption);

    internal static ImpulseDescriptor AsyncResumption(ushort[]? linkedOperations)
        => new(linkedOperations, Flags.Call | Flags.Async | Flags.Resumption);


}
