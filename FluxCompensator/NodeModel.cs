namespace FluxCompensator;

[AttributeUsageAttribute(AttributeTargets.Parameter)]
public sealed class InputAttribute : System.Attribute;

[AttributeUsageAttribute(AttributeTargets.Parameter)]
public sealed class InputRefAttribute : System.Attribute;

[AttributeUsageAttribute(AttributeTargets.Parameter)]
public sealed class OutputAttribute : System.Attribute;

[AttributeUsageAttribute(AttributeTargets.Parameter)]
public sealed class ContextAttribute : System.Attribute;

[AttributeUsageAttribute(AttributeTargets.Parameter)]
public sealed class BindingAttribute(string name, bool constant = false) : System.Attribute
{
    public string Name { get; } = name;

    /// <summary>
    /// When constant, the value is inlined. If the binding-provided value
    /// changes, the impulse must be recompiled.
    /// </summary>
    public bool Constant { get; } = constant;
}

/// <summary>
/// Implicitly make this an action node with a "next" continuation.
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Method)]
public sealed class ActionAttribute(bool implicitNext = true) : System.Attribute
{
    public bool ImplicitNext { get; } = implicitNext;
}

public abstract class CompilerHint
{
    internal protected CompilerHint()
        => throw new InvalidOperationException(
            $"{GetType().Name} is a compiler hint and cannot be instantiated"
        );
}

public delegate void Call();

public delegate T Expression<T>();

public sealed class Variable<T> : CompilerHint
{
    private Variable() : base() { }

    public T? Read() => throw new NotImplementedException();
    public bool Write(T? value) => throw new NotImplementedException();
}
