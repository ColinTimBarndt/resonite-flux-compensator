using ProtoFiber.Core.Model;

namespace ProtoFiber.Core.Nodes.Flow;

public abstract class ImpulseDemultiplexer
{
    
    [OperationListIndex]
    static protected int Operation { get; }

    [Output]
    static protected int Index { set; get; }

    [Operation(HasContinuation = true)]
    public abstract void Execute();
    
}