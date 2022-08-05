namespace ValueObject.Collections.Tests;

public sealed class SortedDictionaryValueTest
{
    public static object?[][] TestDataEquality => new[]
    {
        new object?[] { null, null, true },
        new object?[] { SortedDictionaryValue.Create((1, 1), (2, 2)), null, false },
        new object?[] { null, SortedDictionaryValue.Create((1, 1), (2, 2)), false },
        new object?[] { SortedDictionaryValue.Create((1, 1), (2, 2)), SortedDictionaryValue.Create((1, 1), (2, 2)), true },
    };

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(SortedDictionaryValue<int, int>? a, SortedDictionaryValue<int, int> b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<SortedDictionaryValue<int, int>?>.Default.Equals(a, b));
        Assert.Equal(expected, object.Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString => new[]
    {
        new object?[]{ null, null },
        new object?[]{SortedDictionaryValue.Create<string, int>(), "[]" },
        new object?[]{SortedDictionaryValue.Create(("a", 1), ("b", 2)), "[(a, 1),(b, 2)]" }
    };

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(SortedDictionaryValue<string, int>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson => new[]
    {
        new object?[] { "null", null },
        new object?[] { "{}", SortedDictionaryValue.Create<string, int>() },
        new object?[] { "{\"a\":1,\"b\":2}", SortedDictionaryValue.Create(("a", 1), ("b", 2)) },
    };

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, SortedDictionaryValue<string, int> expected)
    {
        var actual = JsonSerializer.Deserialize<SortedDictionaryValue<string, int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));
    }
}
