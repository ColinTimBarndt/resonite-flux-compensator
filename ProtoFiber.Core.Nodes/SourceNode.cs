using ProtoFiber.Core.Model;

namespace ProtoFiber.Core.Nodes;

public abstract class SourceNode<T> : EvaluatedNode
{

    [Output]
    protected static T? Value { get; }

}
