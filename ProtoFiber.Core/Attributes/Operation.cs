namespace ProtoFiber.Core.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OperationAttribute : Attribute
{

    public string? Continuation { get; set; }

    public bool HasContinuation
    {
        get => Continuation is not null;
        set => Continuation = value ? "" : null;
    }

}
