namespace ProtoFiber.Core.Model;

public interface ICallList
{
    public static abstract int Count { get; }

    public static abstract void Run(int index);
}
