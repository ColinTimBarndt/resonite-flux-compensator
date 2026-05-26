namespace ProtoFiber.Core;

public readonly struct OperationDescriptor
{

    private readonly ushort[]? _linkedOperations;

    public readonly bool IsAsync { get; }

    internal OperationDescriptor(ushort[]? linkedOperations, bool isAsync)
    {
        _linkedOperations = linkedOperations;
        IsAsync = isAsync;
    }

    public bool IsReachableFrom(int impulse) => IsReachableFrom((ushort)impulse);
    public bool IsReachableFrom(ushort impulse) => _linkedOperations is null || _linkedOperations.Contains(impulse);

}
