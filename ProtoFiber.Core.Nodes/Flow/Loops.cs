using ProtoFiber.Core.Model;

namespace ProtoFiber.Core.Nodes.Flow;

public abstract class LoopNode
{

    [Operation(Continuation = "LoopEnd")]
    public abstract void Execute<TLoopStart, TLoopIteration>()
    where TLoopStart : ICall
    where TLoopIteration : ICall;

}

[ProtoFiberNode]
public abstract class WhileLoop : LoopNode
{

    [Input]
    protected static bool Condition { get; }

}

[ProtoFiberNode]
public abstract class ForLoop<T> : LoopNode
{

    [Input]
    protected static T? Count { get; }

    [Input]
    protected static bool Reverse { get; }

    [Output]
    protected static T? Iteration { set; get; }

}

[ProtoFiberNode]
public abstract class RangeLoop<T> : LoopNode
{

    [Input]
    protected static T? Start { get; }

    [Input]
    protected static T? End { get; }

    [Input(Default = 1)]
    protected static T? StepSize { get; }

    [Output]
    protected static T? Current { set; get; }

}
