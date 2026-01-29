using System.Numerics;

namespace FluxCompensator.Nodes;

public static class Variables
{
    public static void Read<T>(
        [InputRef] ref T? variable,
        [Output] ref T? value
    )
    {
        value = variable;
    }

    public static void IndirectRead<T>(
        [Input] Variable<T> variable,
        [Output] ref T? value
    )
    {
        value = variable.Read();
    }

    [Action]
    public static void Write<T>(
        [Input] T value,
        [InputRef] ref T variable
    )
    {
        variable = value;
    }

    [Action]
    public static void Write<T>(
        [Input] T value,
        [InputRef] Variable<T> variable,
        Call onSuccess,
        Call onError
    )
    {
        if (variable.Write(value))
            onSuccess();
        else
            onError();
    }

    [Action]
    public static void IndirectWrite<T>(
        [Input] T value,
        [Input] Variable<T> variable,
        Call onSuccess,
        Call onError
    )
    {
        if (variable.Write(value))
            onSuccess();
        else
            onError();
    }

    [Action]
    public static void Increment<T>(
        [InputRef] ref T? variable
    ) where T : INumber<T>
    {
        variable ??= T.AdditiveIdentity;
        variable++;
    }

    [Action]
    public static void Increment<T>(
        [InputRef] Variable<T> variable,
        Call onSuccess,
        Call onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next++;
        if (variable.Write(next))
            onSuccess();
        else
            onError();
    }

    [Action]
    public static void IndirectIncrement<T>(
        [Input] Variable<T> variable,
        Call onSuccess,
        Call onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next++;
        if (variable.Write(next))
            onSuccess();
        else
            onError();
    }

    [Action]
    public static void Decrement<T>(
        [InputRef] ref T? variable
    ) where T : INumber<T>
    {
        variable ??= T.AdditiveIdentity;
        variable--;
    }

    [Action]
    public static void Decrement<T>(
        [InputRef] Variable<T> variable,
        Call onSuccess,
        Call onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next--;
        if (variable.Write(next))
            onSuccess();
        else
            onError();
    }

    [Action]
    public static void IndirectDecrement<T>(
        [Input] Variable<T> variable,
        Call onSuccess,
        Call onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next--;
        if (variable.Write(next))
            onSuccess();
        else
            onError();
    }
}