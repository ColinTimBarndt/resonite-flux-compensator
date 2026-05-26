using System.Runtime.InteropServices;

namespace ProtoFiber.Core.Graph;

[StructLayout(LayoutKind.Explicit)]
public struct Node
{

    public Node(NodeType type)
    {
        Type = type.TypeId;
        _operations = (byte)type.OperationCount;
        _impulses = (byte)type.ImpulseCount;
        _inputs = (byte)type.InputCount;
        _outputs = (byte)type.OutputCount;
    }

    internal Node(int vacantData)
    {
        Type = NodeTypeId.Invalid;
        _vacantData = vacantData;
    }

    [field: FieldOffset(0)]
    public NodeTypeId Type { get; }

    [FieldOffset(4)]
    internal int _vacantData;

    [FieldOffset(4)]
    internal byte _operations;

    [FieldOffset(5)]
    internal byte _impulses;

    [FieldOffset(6)]
    internal byte _inputs;

    [FieldOffset(7)]
    internal byte _outputs;

}
