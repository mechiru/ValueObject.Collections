using System.Diagnostics.CodeAnalysis;

namespace ValueObject.Collections;

public static class SortedDictionaryValue
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SortedDictionaryValue<TKey, TValue> Create<TKey, TValue>(params (TKey, TValue)[] items)
        where TKey : notnull
        => new(new(items.ToDictionary(x => x.Item1, x => x.Item2)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SortedDictionaryValue<TKey, TValue> ToSortedDictionaryValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> self)
        where TKey : notnull
        => new(new SortedDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(self)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SortedDictionaryValue<TKey, TValue> ToSortedDictionaryValue<TKey, TValue>(this IEnumerable<TValue> self,
        Func<TValue, TKey> selector)
        where TKey : notnull
        => Helper.ToDictionary<TKey, TValue, SortedDictionaryValue<TKey, TValue>>(self, selector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SortedDictionaryValue<TKey, TValue> ToSortedDictionaryValue<TKey, TValue>(this IEnumerable<(TKey, TValue)> self)
        where TKey : notnull
        => self.Select(pair => KeyValuePair.Create(pair.Item1, pair.Item2)).ToSortedDictionaryValue();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<SortedDictionaryValue<TKey, TValue>> ToSortedDictionaryValueAsync<TKey, TValue>(this IAsyncEnumerable<KeyValuePair<TKey, TValue>> self, CancellationToken cancellationToken = default)
        where TKey : notnull
        => Helper.CollectAsync<KeyValuePair<TKey, TValue>, SortedDictionaryValue<TKey, TValue>>(self, cancellationToken);
}

public sealed class SortedDictionaryValue<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly SortedDictionary<TKey, TValue> _dict;

    private ICollection<KeyValuePair<TKey, TValue>> Collection
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _dict;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SortedDictionaryValue() => _dict = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal SortedDictionaryValue(in SortedDictionary<TKey, TValue> dict) => _dict = dict;

    // Equality operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(SortedDictionaryValue<TKey, TValue>? a, SortedDictionaryValue<TKey, TValue>? b) => a?.Equals((object?)b) ?? b is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(SortedDictionaryValue<TKey, TValue>? a, SortedDictionaryValue<TKey, TValue>? b) => !(a == b);

    // Equality methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Equals(SortedDictionaryValue<TKey, TValue> other) => Enumerable.SequenceEqual(_dict, other._dict);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is SortedDictionaryValue<TKey, TValue> other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Helper.GetHashCode(this);

    // ToString method

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"[{string.Join(",", _dict.Select(x => $"({x.Key}, {x.Value})"))}]";

    // ISortedDictionary<TKey, TValue> methods 

    /// <inheritdoc />
    public TValue this[TKey key]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _dict[key];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _dict[key] = value;
    }

    /// <inheritdoc />
    public ICollection<TKey> Keys
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _dict.Keys;
    }

    /// <inheritdoc />
    public ICollection<TValue> Values
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _dict.Values;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ContainsKey(TKey key) => _dict.ContainsKey(key);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(TKey key, TValue value) => _dict.Add(key, value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(TKey key) => _dict.Remove(key);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _dict.TryGetValue(key, out value);

    // ICollection<KeyValuePair<TKey, TValue>>

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dict.GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Collection.Add(item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => Collection.Clear();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => Collection.Contains(item);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Collection.Remove(item);

    /// <inheritdoc />
    int ICollection<KeyValuePair<TKey, TValue>>.Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Collection.Count;
    }

    /// <inheritdoc />
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Collection.IsReadOnly;
    }
}
