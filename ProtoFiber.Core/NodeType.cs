namespace ProtoFiber.Core;

file struct SpecialPackerNode;
file struct SpecialPackerNode<T>;

public sealed partial class NodeType
{

    private NodeType(
        NodeTypeId id,
        Type decl,
        Flags flags,
        OperationDescriptor[] operations,
        ImpulseDescriptor[] impulses,
        DataDescriptor[] data,
        int inputCount
    )
    {
        TypeId = id;
        DeclaringType = decl;
        _flags = flags;
        _operations = operations;
        _impulses = impulses;
        _data = data;
        InputCount = inputCount;
    }

    internal static NodeType CreateSpecialPacker(NodeTypeId id, ConnectorType type, ConnectorDirection direction)
    {
        var flags = Flags.IsSpecialPacker;

        Type decl;
        OperationDescriptor[] operations = [];
        ImpulseDescriptor[] impulses = [];
        DataDescriptor[] data = [];
        int inputCount = 0;

        Type tArg;

        switch ((type, direction))
        {
            case (ConnectorType.Data, ConnectorDirection.Upstream):
                // Input
                flags |= Flags.IsSpecialData | Flags.IsSpecialUpstream;
                decl = typeof(SpecialPackerNode<>);
                tArg = decl.GetGenericArguments()[0];
                data = [new(null, tArg), new(null, typeof(IReadOnlyList<>).MakeGenericType(tArg))];
                inputCount = 1;
                break;

            case (ConnectorType.Data, ConnectorDirection.Downstream):
                // Output
                flags |= Flags.IsSpecialData;
                decl = typeof(SpecialPackerNode<>);
                tArg = decl.GetGenericArguments()[0];
                data = [new(null, tArg), new(null, typeof(IReadOnlyList<>).MakeGenericType(tArg))];
                inputCount = 1;
                break;

            case (ConnectorType.Flow, ConnectorDirection.Upstream | ConnectorDirection.Downstream):
                // Operation, Impulse
                if (direction == ConnectorDirection.Upstream)
                    flags |= Flags.IsSpecialUpstream;
                decl = typeof(SpecialPackerNode);
                impulses = [ImpulseDescriptor.Continuation(null)];
                operations = [new(null, false)];
                break;

            case (_, ConnectorDirection.Upstream | ConnectorDirection.Downstream):
                throw new ArgumentException("Invalid connector type", nameof(type));

            default:
                throw new ArgumentException("Invalid connector direction", nameof(type));
        }

        return new(id, decl, flags, operations, impulses, data, inputCount);
    }

    public NodeTypeId TypeId { get; }

    public Type DeclaringType { get; }

    private readonly Flags _flags;

    [Flags]
    private enum Flags
    {
        /// <summary>Is this a special packer node?</summary>
        IsSpecialPacker = 1,
        /// <summary>Is this a special data node? Otherwise flow.</summary>
        IsSpecialData = 2,
        /// <summary>Is this a special upstream node? Otherwise downstream.</summary>
        IsSpecialUpstream = 4,
        /// <summary>Is this a custom node?</summary>
        IsCustom = 8,
    }

    private readonly OperationDescriptor[] _operations;
    private readonly ImpulseDescriptor[] _impulses;
    private readonly DataDescriptor[] _data;

    /// <summary>Is this a special packer node?</summary>
    internal bool IsSpecialPacker => _flags.HasFlag(Flags.IsSpecialPacker);

    /// <summary>If special, is this a special data node? Otherwise flow.</summary>
    internal bool IsSpecialData => _flags.HasFlag(Flags.IsSpecialData);

    /// <summary>If special, is this a special input node? Otherwise output.</summary>
    internal bool IsSpecialInput => _flags.HasFlag(Flags.IsSpecialUpstream);

    public bool IsCustom => _flags.HasFlag(Flags.IsCustom);

    public int OperationCount => _operations.Length;
    public int ImpulseCount => _impulses.Length;
    public int InputCount { get; }
    public int OutputCount => _data.Length - InputCount;

    public ReadOnlySpan<OperationDescriptor> Operations => _operations;
    public ReadOnlySpan<ImpulseDescriptor> Impulses => _impulses;
    public ReadOnlySpan<DataDescriptor> Inputs => _data.AsSpan()[..InputCount];
    public ReadOnlySpan<DataDescriptor> Outputs => _data.AsSpan()[InputCount..];

}
