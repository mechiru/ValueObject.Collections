using System.Text.Json.Serialization;

namespace ValueObject.Collections.Tests;

public sealed class SortedDictionaryValueTest
{
    public static object?[][] TestDataEquality =>
    [
        [null, null, true],
        [SortedDictionaryValue.Create((1, 1), (2, 2)), null, false],
        [null, SortedDictionaryValue.Create((1, 1), (2, 2)), false],
        [SortedDictionaryValue.Create((1, 1), (2, 2)), SortedDictionaryValue.Create((1, 1), (2, 2)), true]
    ];

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(SortedDictionaryValue<int, int>? a, SortedDictionaryValue<int, int> b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<SortedDictionaryValue<int, int>?>.Default.Equals(a, b));
        Assert.Equal(expected, Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString =>
    [
        [null, null],
        [SortedDictionaryValue.Create<string, int>(), "[]"],
        [SortedDictionaryValue.Create(("a", 1), ("b", 2)), "[(a, 1),(b, 2)]"]
    ];

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(SortedDictionaryValue<string, int>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson =>
    [
        ["null", null],
        ["{}", SortedDictionaryValue.Create<string, int>()],
        ["""{"a":1,"b":2}""", SortedDictionaryValue.Create(("a", 1), ("b", 2))]
    ];

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, SortedDictionaryValue<string, int> expected)
    {
        var actual = JsonSerializer.Deserialize<SortedDictionaryValue<string, int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));

        actual = JsonSerializer.Deserialize<SortedDictionaryValue<string, int>>(json, SortedDictionaryValueJsonContext.Default.Options);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual, SortedDictionaryValueJsonContext.Default.Options));
    }
}

[JsonSerializable(typeof(SortedDictionaryValue<string, int>))]
internal sealed partial class SortedDictionaryValueJsonContext : JsonSerializerContext;
