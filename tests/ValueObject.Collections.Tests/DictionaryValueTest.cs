namespace ValueObject.Collections.Tests;

public sealed class DictionaryValueTest
{
    public static object?[][] TestDataEquality => new[]
    {
        new object?[] { null, null, true },
        new object?[] { DictionaryValue.Create((1, 1), (2, 2)), null, false },
        new object?[] { null, DictionaryValue.Create((1, 1), (2, 2)), false },
        new object?[] { DictionaryValue.Create((1, 1), (2, 2)), DictionaryValue.Create((1, 1), (2, 2)), true },
    };

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(DictionaryValue<int, int>? a, DictionaryValue<int, int> b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<DictionaryValue<int, int>?>.Default.Equals(a, b));
        Assert.Equal(expected, object.Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString => new[]
    {
        new object?[]{ null, null },
        new object?[]{DictionaryValue.Create<string, int>(), "[]" },
        new object?[]{DictionaryValue.Create(("a", 1), ("b", 2)), "[(a, 1),(b, 2)]" }
    };

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(DictionaryValue<string, int>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson => new[]
    {
        new object?[] { "null", null },
        new object?[] { "{}", DictionaryValue.Create<string, int>() },
        new object?[] { "{\"a\":1,\"b\":2}", DictionaryValue.Create(("a", 1), ("b", 2)) },
    };

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, DictionaryValue<string, int> expected)
    {
        var actual = JsonSerializer.Deserialize<DictionaryValue<string, int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));
    }
}
