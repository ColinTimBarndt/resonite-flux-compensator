using ProtoFiber.Core.Nodes.Flow;

namespace ProtoFiber.DotNet.Flow;

public sealed class WhileLoopCore : WhileLoop
{
    public override void Execute<TLoopStart, TLoopIteration>()
    {
        TLoopStart.Run();
        while (Condition)
            TLoopIteration.Run();
    }
}

public sealed class ForLoopInt : ForLoop<int>
{
    public override void Execute<TLoopStart, TLoopIteration>()
    {
        int count = Count;
        bool reverse = Reverse;
        TLoopStart.Run();
        if (reverse)
        {
            for (Iteration = count; --Iteration >= 0;)
            {
                TLoopIteration.Run();
            }
        }
        else
        {
            for (Iteration = 0; Iteration < count; Iteration++)
            {
                TLoopIteration.Run();
            }
        }
    }
}

public sealed class RangeLoopInt : RangeLoop<int>
{
    public override void Execute<TLoopStart, TLoopIteration>()
    {
        int start = Start, end = End, stepSize = StepSize;
        TLoopStart.Run();
        if (stepSize <= 0)
            return;

        Current = start;
        if (start > end)
        {
            while (Current >= end)
            {
                TLoopIteration.Run();
                Current -= stepSize;
            }
        }
        else
        {
            for (; Current <= end; Current += stepSize)
            {
                TLoopIteration.Run();
            }
        }
    }
}
