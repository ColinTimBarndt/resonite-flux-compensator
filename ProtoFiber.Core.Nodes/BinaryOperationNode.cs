using ProtoFiber.Core.Attributes;

namespace ProtoFiber.Core.Nodes;

public abstract class BinaryOperationNode<TLeft, TRight, TResult> : EvaluatedNode
{

    [Input]
    public static TLeft Left;

    [Input]
    public static TRight Right;

    [Output]
    public static TResult Result;

}
