namespace ValueObject.Collections;

public static class ListValue
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ListValue<T> Create<T>(params T[] items) => new(Enumerable.ToList(items));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ListValue<T> ToListValue<T>(this ICollection<T> self) => new(new List<T>(self));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ListValue<T> ToListValue<T>(this IEnumerable<T> self) => self is ICollection<T> collection ? ToListValue(collection) : new(Enumerable.ToList(self));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<ListValue<T>> ToListValueAsync<T>(this IAsyncEnumerable<T> self, CancellationToken cancellationToken = default)
        => Helper.CollectAsync<T, ListValue<T>>(self, cancellationToken);
}

/// <summary>
/// ListValue is a mutable list objects.
/// </summary>
/// <typeparam name="T">T is a element type of ListValue.</typeparam>
public sealed class ListValue<T> : IList<T>, IReadOnlyList<T>
{
    private readonly List<T> _list;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ListValue() => _list = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ListValue(int capacity) => _list = new(capacity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ListValue(List<T> list) => _list = list;

    // Equality operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ListValue<T>? a, ListValue<T>? b) => a?.Equals((object?)b) ?? b is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ListValue<T>? a, ListValue<T>? b) => !(a == b);

    // Equality methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Equals(ListValue<T> other) => ReferenceEquals(_list, other._list) || _list.SequenceEqual(other._list);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ListValue<T> other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Helper.GetHashCode(this);

    // ToString methods

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"[{string.Join(",", _list)}]";

    // ICollection<T> methods

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item) => _list.Add(item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => _list.Clear();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item) => _list.Contains(item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(T item) => _list.Remove(item);

    /// <inheritdoc />
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _list.Count;
    }

    /// <inheritdoc />
    bool ICollection<T>.IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_list as ICollection<T>).IsReadOnly;
    }

    // IList<T> methods

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(T item) => _list.IndexOf(item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Insert(int index, T item) => _list.Insert(index, item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveAt(int index) => _list.RemoveAt(index);

    /// <inheritdoc />
    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _list[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _list[index] = value;
    }
}
