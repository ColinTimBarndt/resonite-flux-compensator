using ProtoFiber.Core.Attributes;

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
    public static bool Condition;

}

[ProtoFiberNode]
public abstract class ForLoop<T> : LoopNode
{

    [Input]
    public static T Count;

    [Input]
    public static bool Reverse;

    [Output]
    public static T Iteration;

}

[ProtoFiberNode]
public abstract class RangeLoop<T> : LoopNode
{

    [Input]
    public static T Start;

    [Input]
    public static T End;

    [Input(Default = 1)]
    public static T StepSize;

    [Output]
    public static T Current;

}
