using ProtoFiber.Core.Attributes;

namespace ProtoFiber.Core.Nodes;

public abstract class UnaryOperationNode<TArgument, TResult> : EvaluatedNode
{

    [Input]
    public static TArgument Argument;

    [Output]
    public static TResult Result;

}
