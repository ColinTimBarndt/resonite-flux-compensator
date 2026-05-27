using System.Buffers;
using System.Collections;

namespace ProtoFiber.Core.Collections;

internal struct InlineCollection<T>() : ICollection<T>
{

    private static readonly ArrayPool<T> _pool = ArrayPool<T>.Create(16, 64);

    public static readonly InlineCollection<T> Empty = [];

    public int Count { readonly get; private set; }
    public readonly int Capacity => _items.Length;

    public readonly bool IsReadOnly => false;

    private T[] _items = [];

    public InlineCollection(int minimumCapacity) : this()
    {
        _items = _pool.Rent(minimumCapacity);
    }

    public readonly Span<T> AsSpan() => _items.AsSpan()[..Count];

    public readonly int IndexOf(T value) => _items.IndexOf(value);

    public void Add(T value)
    {
        if (Count == Capacity)
        {
            var newItems = _pool.Rent(Capacity * 2);
            _items.CopyTo(newItems.AsSpan()[..Count]);
            _pool.Return(_items);
            _items = newItems;
        }
        _items[Count++] = value;
    }

    public bool Remove(T value)
    {
        int i = IndexOf(value);
        if (i < 0)
            return false;

        // Swap-remove if it's not the last item
        if (i != --Count)
            _items[i] = _items[Count];

        return true;
    }

    public readonly bool Contains(T value) => IndexOf(value) >= 0;

    public void Clear()
    {
        Count = 0;
        _pool.Return(_items);
        _items = [];
    }

    public readonly void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public readonly Span<T>.Enumerator GetEnumerator() => AsSpan().GetEnumerator();

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _items.Take(Count).GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<T>).GetEnumerator();

}
