using ProtoFiber.Core.Nodes.Flow;

namespace ProtoFiber.DotNet.Flow;

public sealed class SequenceCore : Sequence
{
    public override void Execute<TCalls>()
    {
        // i & 0xffff_ffff makes the JIT unroll the loop up to i=4
        for (uint i = 0; (i & 0xffff_ffff) < TCalls.Count; i++)
            TCalls.Run((int)i);
    }
}
