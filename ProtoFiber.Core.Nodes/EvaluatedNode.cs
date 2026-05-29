using ProtoFiber.Core.Model;

namespace ProtoFiber.Core.Nodes;

public abstract class EvaluatedNode
{

    [Evaluate]
    public abstract void Evaluate();

}
