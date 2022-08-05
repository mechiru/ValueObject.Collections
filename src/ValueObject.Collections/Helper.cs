using System.Runtime.CompilerServices;

namespace ValueObject.Collections;

internal static class Helper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<U> CollectAsync<T, U>(IAsyncEnumerable<T> self, CancellationToken cancellationToken)
        where U : ICollection<T>, new()
    {
        var collection = new U();

        await foreach (var item in self.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            collection.Add(item);
        }

        return collection;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TDictionary ToDictionary<TKey, TValue, TDictionary>(in IEnumerable<TValue> self, Func<TValue, TKey> selector)
        where TKey : notnull
        where TDictionary : IDictionary<TKey, TValue>, new()
    {
        var dict = new TDictionary();

        foreach (var elem in self)
        {
            dict.Add(selector(elem), elem);
        }

        return dict;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(ICollection<T> collection)
    {
        var hash = new HashCode();
        foreach (var item in collection)
        {
            hash.Add(item);
        }
        return hash.ToHashCode();
    }
}
