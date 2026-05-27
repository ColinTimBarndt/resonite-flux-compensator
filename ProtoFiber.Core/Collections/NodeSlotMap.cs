using System.Collections;
using ProtoFiber.Core.Graph;

namespace ProtoFiber.Core.Collections;

internal struct NodeSlotMap() : IEnumerable<Node>
{

    private Node[] _items = [];

    /// <summary>
    /// How many slots are occupied.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// The next free element
    /// </summary>
    private int _next;

    public readonly ref Node this[NodeId id] => ref _items[(uint)id];

    public NodeId Add(Node node)
    {
        if (node.Type == NodeTypeId.Invalid)
            throw new ArgumentException("node type is invalid", nameof(node));

        if (_next == _items.Length)
        {
            Array.Resize(ref _items, _next * 2);

            // Insert node
            _items[_next] = node;

            // Make each new vacant node point to the next one
            // Note: The last entry points to the new `_items.length`
            for (int i = _next; i < _items.Length;)
                _items[i] = new(++i);

            Count++;
            return new((uint)_next++);
        }
        else
        {
            // Pop next free entry
            int slot = _next;
            _next = _items[slot]._vacantData;

            // Insert node
            _items[slot] = node;

            Count++;
            return new((uint)slot);
        }
    }

    public void RemoveAt(NodeId id)
    {
        ref var value = ref this[id];
        if (value.Type == NodeTypeId.Invalid)
            return;

        Count--;

        // Push next free entry
        value = new(_next);
        _next = (int)id;
    }

    public readonly Enumerator GetEnumerator() => new(_items);

    readonly IEnumerator<Node> IEnumerable<Node>.GetEnumerator() => GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<Node>
    {

        internal Enumerator(Node[] items)
        {
            _items = items;
        }

        private readonly Node[] _items;

        private int _index;

        public Node Current { get; private set; }

        readonly object IEnumerator.Current => Current;

        public readonly void Dispose() { }

        public bool MoveNext()
        {
            while (_index < _items.Length)
            {
                ref var item = ref _items[_index++];
                if (item.Type == NodeTypeId.Invalid)
                    continue;

                Current = item;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _index = 0;
        }

    }

}
