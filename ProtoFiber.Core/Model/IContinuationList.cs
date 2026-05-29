using System.Diagnostics.CodeAnalysis;

namespace ProtoFiber.Core.Model;

public interface IContinuationList
{
    /// <summary>
    /// Hands control over to the continuation,
    /// returning from the current node execution.
    /// </summary>
    [DoesNotReturn]
    public static abstract void RunContinue(int index);
}
