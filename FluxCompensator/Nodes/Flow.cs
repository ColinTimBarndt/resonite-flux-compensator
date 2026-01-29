namespace FluxCompensator.Nodes;

public static class Flow
{
    private const int LOOP_CHECK_MASK = 1023;

    [Action(implicitNext: false)]
    public static void If(
        [Input] bool condition,
        Call onTrue,
        Call onFalse
    )
    {
        if (condition)
            onTrue();
        else
            onFalse();
    }

    [Action]
    public static void For(
        [Context] CancellationToken token,
        [Input] int count,
        [Output] ref int index,
        Call onBefore,
        Call onStep
    )
    {
        index = 0;
        onBefore();
        for (; index < count; index++)
        {
            // For performance, don't check every iteration.
            // For bounded loops, this might get optimized away by the JIT.
            if ((index & LOOP_CHECK_MASK) == LOOP_CHECK_MASK)
                token.ThrowIfCancellationRequested();
            onStep();
        }
        index = default;
    }

    [Action]
    public static void While(
        [Context] CancellationToken token,
        [Input] Expression<bool> condition,
        [Output] ref int index, // Expose index since we use it anyways
        Call onBefore,
        Call onStep
    )
    {
        index = 0;
        onBefore();
        while (condition())
        {
            // For performance, don't check every iteration.
            // For bounded loops, this might get optimized away by the JIT.
            if ((index & LOOP_CHECK_MASK) == LOOP_CHECK_MASK)
                token.ThrowIfCancellationRequested();
            onStep();
        }
        index = default;
    }

    [Action(implicitNext: false)]
    public static void Sequence(
        Call[] calls
    )
    {
        for (int i = 0; i < calls.Length; i++)
        {
            calls[i]();
        }
    }
}