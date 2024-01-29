namespace ValueObject.Collections;

public static class SortedSetValue
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SortedSetValue<T> Create<T>(params T[] items) => new(new(items));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SortedSetValue<T> ToSortedSetValue<T>(this IEnumerable<T> self) => new(new(self));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<SortedSetValue<T>> ToSortedSetValueAsync<T>(this IAsyncEnumerable<T> self, CancellationToken cancellationToken = default)
        => Helper.CollectAsync<T, SortedSetValue<T>>(self, cancellationToken);
}

public sealed class SortedSetValue<T> : ISet<T>, IReadOnlySet<T>
{
    private readonly SortedSet<T> _set;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SortedSetValue() => _set = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal SortedSetValue(in SortedSet<T> set) => _set = set;

    // Equality operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(SortedSetValue<T>? a, SortedSetValue<T>? b) => a?.Equals((object?)b) ?? b is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(SortedSetValue<T>? a, SortedSetValue<T>? b) => !(a == b);

    // Equality methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Equals(SortedSetValue<T> other) => _set.SetEquals(other._set);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is SortedSetValue<T> other && Equals(other);

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
