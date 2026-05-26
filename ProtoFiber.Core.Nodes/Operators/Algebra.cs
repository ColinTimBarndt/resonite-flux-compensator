using ProtoFiber.Core.Attributes;

namespace ProtoFiber.Core.Nodes.Operators;

[ProtoFiberNode]
public abstract class Increment<T> : UnaryOperationNode<T, T>;

[ProtoFiberNode]
public abstract class Decrement<T> : UnaryOperationNode<T, T>;

[ProtoFiberNode]
public abstract class Negate<T> : UnaryOperationNode<T, T>;

[ProtoFiberNode]
public abstract class Reciprocal<T> : UnaryOperationNode<T, T>;

[ProtoFiberNode]
public abstract class Add<TLeft, TRight, TResult> : BinaryOperationNode<TLeft, TRight, TResult>;

[ProtoFiberNode]
public abstract class Subtract<TLeft, TRight, TResult> : BinaryOperationNode<TLeft, TRight, TResult>;

[ProtoFiberNode]
public abstract class Multiply<TLeft, TRight, TResult> : BinaryOperationNode<TLeft, TRight, TResult>;

[ProtoFiberNode]
public abstract class Divide<TLeft, TRight, TResult> : BinaryOperationNode<TLeft, TRight, TResult>;
