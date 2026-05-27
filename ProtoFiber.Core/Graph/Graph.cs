using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ProtoFiber.Core.Collections;

namespace ProtoFiber.Core.Graph;

using NodeConnectionSet = InlineCollection<NodeConnection>;

public sealed class Graph
{

    private NodeSlotMap _nodes = new();

    private Dictionary<NodeConnection, NodeConnection> _edges = [];

    private Dictionary<NodeConnection, NodeConnectionSet> _backEdges = [];

    // Nodes

    // TODO

    // Connections

    public NodeConnection GetUpstream(NodeConnection down) => _edges.GetValueOrDefault(down);

    public ReadOnlySpan<NodeConnection> GetDownstreams(NodeConnection up)
    {
        ref var downs = ref GetDownstreamsRefOrNull(up);
        return Unsafe.IsNullRef(ref downs) ? [] : downs.AsSpan();
    }

    public void Connect(NodeConnection down, NodeConnection up)
    {
        ref var currentUp = ref GetUpstreamRefOrCreate(down, out bool exists);
        if (exists)
        {
            ref var currentDowns = ref GetDownstreamsRefOrNull(currentUp);
            // Connections are always bidirectional => downs is not null
            currentDowns.Remove(down);
        }
        currentUp = up;
        ref var downs = ref GetDownstreamsRefOrCreate(up, out _);
        downs.Add(down);
    }

    public bool Disconnect(NodeConnection down)
    {
        if (!_edges.Remove(down, out var up))
            return false;

        ref var downs = ref GetDownstreamsRefOrNull(up);
        // Connections are always bidirectional => downs is not null
        downs.Remove(down);

        return true;
    }

    public bool DisconnectAll(NodeConnection up)
    {
        if (!_backEdges.Remove(up, out var downs))
            return false;

        foreach (var down in downs)
            _edges.Remove(down);
        downs.Clear();

        return true;
    }

    private ref NodeConnection GetUpstreamRefOrCreate(NodeConnection down, out bool exists)
        => ref CollectionsMarshal.GetValueRefOrAddDefault(_edges, down, out exists);

    private ref NodeConnectionSet GetDownstreamsRefOrNull(NodeConnection up)
        => ref CollectionsMarshal.GetValueRefOrNullRef(_backEdges, up);

    private ref NodeConnectionSet GetDownstreamsRefOrCreate(NodeConnection up, out bool exists)
    {
        ref var result = ref CollectionsMarshal.GetValueRefOrAddDefault(_backEdges, up, out exists);
        if (!exists)
            result = [];
        return ref result;
    }

}
