namespace ProtoFiber.Core.Model;

[AttributeUsage(AttributeTargets.Property)]
public sealed class InputAttribute : Attribute
{

    public object? Default { get; set; }

}
