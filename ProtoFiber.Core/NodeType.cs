namespace ProtoFiber.Core;

public sealed class NodeType
{

    public NodeTypeId TypeId { get; }

    private readonly OperationDescriptor[] _operations;
    private readonly ImpulseDescriptor[] _impulses;

    public bool HasOperationList { get; }
    public bool HasImpulseList { get; }

    public int OperationCount => _operations.Length;
    public int ImpulseCount => _impulses.Length;

    private readonly Type[] _types;

    private ushort _inputCount;
    public int InputCount => _inputCount;
    public int OutputCount => _types.Length - InputCount;

    [Flags]
    private enum Flags
    {
        HasOperationList = 1,
        HasImpulseList = 2,
        Custom = 4,
    }

}
