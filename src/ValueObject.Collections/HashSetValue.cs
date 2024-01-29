namespace ValueObject.Collections;

public static class HashSetValue
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSetValue<T> Create<T>(params T[] items) => new(new HashSet<T>(items));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSetValue<T> ToHashSetValue<T>(this IEnumerable<T> self) => new(new HashSet<T>(self));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<HashSetValue<T>> ToHashSetValueAsync<T>(this IAsyncEnumerable<T> self, CancellationToken cancellationToken = default)
        => Helper.CollectAsync<T, HashSetValue<T>>(self, cancellationToken);
}

public sealed class HashSetValue<T> : ISet<T>, IReadOnlySet<T>
{
    private readonly HashSet<T> _set;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HashSetValue() => _set = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HashSetValue(int capacity) => _set = new(capacity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal HashSetValue(in HashSet<T> set) => _set = set;

    // Equality operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(HashSetValue<T>? a, HashSetValue<T>? b) => a?.Equals((object?)b) ?? b is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(HashSetValue<T>? a, HashSetValue<T>? b) => !(a == b);

    // Equality methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Equals(HashSetValue<T> other) => _set.SetEquals(other._set);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is HashSetValue<T> other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Helper.GetHashCode(this);

    // ToString method

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"[{string.Join(",", _set)}]";

    // ICollection<T> methods

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item) => _set.Add(item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => _set.Clear();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item) => _set.Contains(item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(T item) => _set.Remove(item);

    /// <inheritdoc />
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _set.Count;
    }

    /// <inheritdoc />
    public bool IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (_set as ICollection<T>).IsReadOnly;
    }

    // ISet<T> methods

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ExceptWith(IEnumerable<T> other) => _set.ExceptWith(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void IntersectWith(IEnumerable<T> other) => _set.IntersectWith(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SymmetricExceptWith(IEnumerable<T> other) => _set.SymmetricExceptWith(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnionWith(IEnumerable<T> other) => _set.UnionWith(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool ISet<T>.Add(T item) => _set.Add(item);
}
