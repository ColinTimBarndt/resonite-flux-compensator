using ProtoFiber.Core.Attributes;

namespace ProtoFiber.Core.Nodes;

public abstract class SourceNode<T> : EvaluatedNode
{

    [Output]
    public T Value;

}
