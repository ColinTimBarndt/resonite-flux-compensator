using ProtoFiber.Core.Model;

namespace ProtoFiber.Core.Nodes;

public abstract class BinaryOperationNode<TLeft, TRight, TResult> : EvaluatedNode
{

    [Input]
    protected static TLeft? Left { get; }

    [Input]
    protected static TRight? Right { get; }

    [Output]
    protected static TResult? Result { set; get; }

}
