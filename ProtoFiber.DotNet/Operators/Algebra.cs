using System.Numerics;
using ProtoFiber.Core.Nodes.Operators;

namespace ProtoFiber.DotNet.Operators;

public sealed class IncrementNumber<T> : Increment<T>
where T : IIncrementOperators<T>
{
    public override void Evaluate()
    {
        T value = Argument;
        Result = ++value;
    }
}

public sealed class DecrementNumber<T> : Decrement<T>
where T : IDecrementOperators<T>
{
    public override void Evaluate()
    {
        T value = Argument;
        Result = --value;
    }
}

public sealed class NegateNumber<T> : Negate<T>
where T : IUnaryNegationOperators<T, T>
{
    public override void Evaluate()
        => Result = -Argument;
}

public sealed class ReciprocalNumber<T> : Reciprocal<T>
where T : IDivisionOperators<T, T, T>, IMultiplicativeIdentity<T, T>
{
    public override void Evaluate()
        => Result = T.MultiplicativeIdentity / Argument;
}

public sealed class AddNumbers<TLeft, TRight, TResult> : Add<TLeft, TRight, TResult>
where TLeft : IAdditionOperators<TLeft, TRight, TResult>
{
    public override void Evaluate()
        => Result = Left + Right;
}

public sealed class SubtractNumbers<TLeft, TRight, TResult> : Subtract<TLeft, TRight, TResult>
where TLeft : ISubtractionOperators<TLeft, TRight, TResult>
{
    public override void Evaluate()
        => Result = Left - Right;
}

public sealed class MultiplyNumbers<TLeft, TRight, TResult> : Multiply<TLeft, TRight, TResult>
where TLeft : IMultiplyOperators<TLeft, TRight, TResult>
{
    public override void Evaluate()
        => Result = Left * Right;
}

public sealed class DivideNumbers<TLeft, TRight, TResult> : Divide<TLeft, TRight, TResult>
where TLeft : IDivisionOperators<TLeft, TRight, TResult>
{
    public override void Evaluate()
        => Result = Left / Right;
}
