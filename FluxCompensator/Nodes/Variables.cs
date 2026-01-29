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
    public static Continuation Write<T>(
        [Input] T value,
        [InputRef] Variable<T> variable,
        Continuation onSuccess,
        Continuation onError
    )
    {
        return variable.Write(value) ? onSuccess : onError;
    }

    [Action]
    public static Continuation IndirectWrite<T>(
        [Input] T value,
        [Input] Variable<T> variable,
        Continuation onSuccess,
        Continuation onError
    )
    {
        return variable.Write(value) ? onSuccess : onError;
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
    public static Continuation Increment<T>(
        [InputRef] Variable<T> variable,
        Continuation onSuccess,
        Continuation onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next++;
        return variable.Write(next) ? onSuccess : onError;
    }

    [Action]
    public static Continuation IndirectIncrement<T>(
        [Input] Variable<T> variable,
        Continuation onSuccess,
        Continuation onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next++;
        return variable.Write(next) ? onSuccess : onError;
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
    public static Continuation Decrement<T>(
        [InputRef] Variable<T> variable,
        Continuation onSuccess,
        Continuation onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next--;
        return variable.Write(next) ? onSuccess : onError;
    }

    [Action]
    public static Continuation IndirectDecrement<T>(
        [Input] Variable<T> variable,
        Continuation onSuccess,
        Continuation onError
    ) where T : INumber<T>
    {
        var next = variable.Read() ?? T.AdditiveIdentity;
        next--;
        return variable.Write(next) ? onSuccess : onError;
    }
}