namespace ValueObject.Collections;

public static class HashSetValue
{
    public static HashSetValue<T> Create<T>(params T[] items) => new(new HashSet<T>(items));

    public static HashSetValue<T> ToHashSetValue<T>(this IEnumerable<T> self) => new(new HashSet<T>(self));

    public static ValueTask<HashSetValue<T>> ToHashSetValueAsync<T>(this IAsyncEnumerable<T> self, CancellationToken cancellationToken = default)
        => Helper.CollectAsync<T, HashSetValue<T>>(self, cancellationToken);
}

public sealed class HashSetValue<T> : ISet<T>, IReadOnlySet<T>
{
    private readonly HashSet<T> _set;

    public HashSetValue() => _set = new();

    public HashSetValue(int capacity) => _set = new(capacity);

    internal HashSetValue(in HashSet<T> set) => _set = set;

    // Equality operators

    public static bool operator ==(HashSetValue<T>? a, HashSetValue<T>? b) => a?.Equals((object?)b) ?? b is null;

    public static bool operator !=(HashSetValue<T>? a, HashSetValue<T>? b) => !(a == b);

    // Equality methods

    private bool Equals(HashSetValue<T> other) => _set.SetEquals(other._set);

    /// <inheritdoc />
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is HashSetValue<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Helper.GetHashCode(this);

    // ToString method

    /// <inheritdoc />
    public override string ToString() => $"[{string.Join(",", _set)}]";

    // ICollection<T> methods

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Add(T item) => _set.Add(item);

    /// <inheritdoc />
    public void Clear() => _set.Clear();

    /// <inheritdoc />
    public bool Contains(T item) => _set.Contains(item);

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(T item) => _set.Remove(item);

    /// <inheritdoc />
    public int Count => _set.Count;

    /// <inheritdoc />
    public bool IsReadOnly => (_set as ICollection<T>).IsReadOnly;

    // ISet<T> methods

    /// <inheritdoc />
    public void ExceptWith(IEnumerable<T> other) => _set.ExceptWith(other);

    /// <inheritdoc />
    public void IntersectWith(IEnumerable<T> other) => _set.IntersectWith(other);

    /// <inheritdoc />
    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

    /// <inheritdoc />
    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

    /// <inheritdoc />
    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

    /// <inheritdoc />
    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

    /// <inheritdoc />
    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

    /// <inheritdoc />
    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

    /// <inheritdoc />
    public void SymmetricExceptWith(IEnumerable<T> other) => _set.SymmetricExceptWith(other);

    /// <inheritdoc />
    public void UnionWith(IEnumerable<T> other) => _set.UnionWith(other);

    /// <inheritdoc />
    bool ISet<T>.Add(T item) => _set.Add(item);
}
