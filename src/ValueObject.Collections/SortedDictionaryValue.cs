using System.Diagnostics.CodeAnalysis;

namespace ValueObject.Collections;

public static class SortedDictionaryValue
{
    public static SortedDictionaryValue<TKey, TValue> Create<TKey, TValue>(params (TKey, TValue)[] items)
        where TKey : notnull
        => new(new(items.ToDictionary(x => x.Item1, x => x.Item2)));

    public static SortedDictionaryValue<TKey, TValue> ToSortedDictionaryValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> self)
        where TKey : notnull
        => new(new SortedDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(self)));

    public static SortedDictionaryValue<TKey, TValue> ToSortedDictionaryValue<TKey, TValue>(this IEnumerable<TValue> self,
        Func<TValue, TKey> selector)
        where TKey : notnull
        => Helper.ToDictionary<TKey, TValue, SortedDictionaryValue<TKey, TValue>>(self, selector);

    public static SortedDictionaryValue<TKey, TValue> ToSortedDictionaryValue<TKey, TValue>(this IEnumerable<(TKey, TValue)> self)
        where TKey : notnull
        => self.Select(pair => KeyValuePair.Create(pair.Item1, pair.Item2)).ToSortedDictionaryValue();

    public static ValueTask<SortedDictionaryValue<TKey, TValue>> ToSortedDictionaryValueAsync<TKey, TValue>(this IAsyncEnumerable<KeyValuePair<TKey, TValue>> self, CancellationToken cancellationToken = default)
        where TKey : notnull
        => Helper.CollectAsync<KeyValuePair<TKey, TValue>, SortedDictionaryValue<TKey, TValue>>(self, cancellationToken);
}

public sealed class SortedDictionaryValue<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly SortedDictionary<TKey, TValue> _dict;

    private ICollection<KeyValuePair<TKey, TValue>> Collection => _dict;

    public SortedDictionaryValue() => _dict = new();

    internal SortedDictionaryValue(in SortedDictionary<TKey, TValue> dict) => _dict = dict;

    // Equality operators

    public static bool operator ==(SortedDictionaryValue<TKey, TValue>? a, SortedDictionaryValue<TKey, TValue>? b) => a?.Equals((object?)b) ?? b is null;

    public static bool operator !=(SortedDictionaryValue<TKey, TValue>? a, SortedDictionaryValue<TKey, TValue>? b) => !(a == b);

    // Equality methods

    private bool Equals(SortedDictionaryValue<TKey, TValue> other) => Enumerable.SequenceEqual(_dict, other._dict);

    /// <inheritdoc />
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is SortedDictionaryValue<TKey, TValue> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Helper.GetHashCode(this);

    // ToString method

    /// <inheritdoc />
    public override string ToString() => $"[{string.Join(",", _dict.Select(x => $"({x.Key}, {x.Value})"))}]";

    // ISortedDictionary<TKey, TValue> methods 

    /// <inheritdoc />
    public TValue this[TKey key]
    {
        get => _dict[key];
        set => _dict[key] = value;
    }

    /// <inheritdoc />
    public ICollection<TKey> Keys => _dict.Keys;

    /// <inheritdoc />
    public ICollection<TValue> Values => _dict.Values;

    /// <inheritdoc />
    public bool ContainsKey(TKey key) => _dict.ContainsKey(key);

    /// <inheritdoc />
    public void Add(TKey key, TValue value) => _dict.Add(key, value);

    /// <inheritdoc />
    public bool Remove(TKey key) => _dict.Remove(key);

    /// <inheritdoc />
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _dict.TryGetValue(key, out value);

    // ICollection<KeyValuePair<TKey, TValue>>

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dict.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Collection.Add(item);

    /// <inheritdoc />
    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => Collection.Clear();

    /// <inheritdoc />
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => Collection.Contains(item);

    /// <inheritdoc />
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Collection.Remove(item);

    /// <inheritdoc />
    int ICollection<KeyValuePair<TKey, TValue>>.Count => Collection.Count;

    /// <inheritdoc />
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => Collection.IsReadOnly;
}
