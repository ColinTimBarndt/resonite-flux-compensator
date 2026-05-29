using ProtoFiber.Core.Model;

namespace ProtoFiber.Core.Nodes;

public abstract class UnaryOperationNode<TArgument, TResult> : EvaluatedNode
{

    [Input]
    protected static TArgument? Argument { get; }

    [Output]
    protected static TResult? Result { set; get; }

}
