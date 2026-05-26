namespace ProtoFiber.Core.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public sealed class InputAttribute : Attribute
{

    public object? Default { get; set; }

}
