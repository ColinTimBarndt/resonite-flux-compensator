using ProtoFiber.Core.Attributes;

namespace ProtoFiber.Core.Nodes;

public abstract class EvaluatedNode
{

    [Evaluate]
    public abstract void Evaluate();

}
