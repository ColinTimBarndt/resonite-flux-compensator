namespace FluxCompensator.Nodes;

public static class Core
{
    public static void Input<T>(
        [Binding("Value")] T? bindingValue,
        [Output] ref T? value
    )
    {
        value = bindingValue;
    }

    public static void Constant<T>(
        [Binding("Value", constant: true)] T? bindingValue,
        [Output] ref T? value
    )
    {
        value = bindingValue;
    }
}