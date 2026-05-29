using ProtoFiber.Core.Model;

namespace ProtoFiber.Core.Nodes.Flow;

public abstract class Sequence
{

    [Operation]
    public abstract void Execute<TCalls>()
    where TCalls : ICallList;

}
