namespace ValueObject.Collections;

public static class ListValue
{
    public static ListValue<T> Create<T>(params T[] items) => new(Enumerable.ToList(items));

    public static ListValue<T> ToListValue<T>(this ICollection<T> self) => new(new List<T>(self));

    public static ListValue<T> ToListValue<T>(this IEnumerable<T> self) => self is ICollection<T> collection ? ToListValue(collection) : new(Enumerable.ToList(self));

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

    public ListValue() => _list = new();

    public ListValue(int capacity) => _list = new(capacity);

    internal ListValue(List<T> list) => _list = list;

    // Equality operators

    public static bool operator ==(ListValue<T>? a, ListValue<T>? b) => a?.Equals((object?)b) ?? b is null;

    public static bool operator !=(ListValue<T>? a, ListValue<T>? b) => !(a == b);

    // Equality methods

    private bool Equals(ListValue<T> other) => ReferenceEquals(_list, other._list) || _list.SequenceEqual(other._list);

    /// <inheritdoc />
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ListValue<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Helper.GetHashCode(this);

    // ToString methods

    /// <inheritdoc />
    public override string ToString() => $"[{string.Join(",", _list)}]";

    // ICollection<T> methods

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Add(T item) => _list.Add(item);

    /// <inheritdoc />
    public void Clear() => _list.Clear();

    /// <inheritdoc />
    public bool Contains(T item) => _list.Contains(item);

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(T item) => _list.Remove(item);

    /// <inheritdoc />
    public int Count => _list.Count;

    /// <inheritdoc />
    bool ICollection<T>.IsReadOnly => (_list as ICollection<T>).IsReadOnly;

    // IList<T> methods

    /// <inheritdoc />
    public int IndexOf(T item) => _list.IndexOf(item);

    /// <inheritdoc />
    public void Insert(int index, T item) => _list.Insert(index, item);

    /// <inheritdoc />
    public void RemoveAt(int index) => _list.RemoveAt(index);

    /// <inheritdoc />
    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }
}
