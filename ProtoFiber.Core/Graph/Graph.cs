using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ProtoFiber.Core.Collections;

namespace ProtoFiber.Core.Graph;

using NodeConnectionSet = InlineCollection<NodeConnection>;

public sealed class Graph : IEnumerable<KeyValuePair<NodeId, Node>>
{

    private NodeSlotMap _nodes = [];

    private Dictionary<NodeConnection, NodeConnection> _edges = [];

    private Dictionary<NodeConnection, NodeConnectionSet> _backEdges = [];

    // Nodes

    public int NodeCount => _nodes.Count;

    public NodeId Add(NodeType type) => _nodes.Add(new(type));

    public bool RemoveAt(NodeId id)
    {
        var node = _nodes.RemoveAt(id);
        if (!node.HasValue)
            return false;

        byte slot;

        for (slot = 0; slot < node.Value._impulses; slot++)
            DisconnectDownstream(new(ConnectorType.Flow, id, slot));

        for (slot = 0; slot < node.Value._inputs; slot++)
            DisconnectDownstream(new(ConnectorType.Data, id, slot));

        for (slot = 0; slot < node.Value._operations; slot++)
            DisconnectAllUpstream(new(ConnectorType.Flow, id, slot));

        for (slot = 0; slot < node.Value._outputs; slot++)
            DisconnectAllUpstream(new(ConnectorType.Data, id, slot));

        return true;
    }

    public Enumerator GetEnumerator() => new(_nodes.GetEnumerator());

    IEnumerator<KeyValuePair<NodeId, Node>> IEnumerable<KeyValuePair<NodeId, Node>>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<KeyValuePair<NodeId, Node>>
    {

        private NodeSlotMap.Enumerator _inner;

        internal Enumerator(NodeSlotMap.Enumerator inner)
        {
            _inner = inner;
        }

        public readonly KeyValuePair<NodeId, Node> Current => _inner.Current;

        readonly object IEnumerator.Current => Current;

        public readonly void Dispose() => _inner.Dispose();

        public bool MoveNext() => _inner.MoveNext();

        public void Reset() => _inner.Reset();

    }

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

    public bool DisconnectDownstream(NodeConnection down)
    {
        if (!_edges.Remove(down, out var up))
            return false;

        ref var downs = ref GetDownstreamsRefOrNull(up);
        // Connections are always bidirectional => downs is not null
        downs.Remove(down);

        return true;
    }

    public bool DisconnectAllUpstream(NodeConnection up)
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
