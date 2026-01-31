using FluxCompensator.Compiler;

MethodReader.ReadMethod(() =>
{
    for (int i = 0; i < TestClass.MyInt; i++)
        Console.WriteLine("Hello, World!");
});
