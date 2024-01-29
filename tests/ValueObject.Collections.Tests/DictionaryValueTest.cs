using System.Text.Json.Serialization;

namespace ValueObject.Collections.Tests;

public sealed class DictionaryValueTest
{
    public static object?[][] TestDataEquality =>
    [
        [null, null, true],
        [DictionaryValue.Create((1, 1), (2, 2)), null, false],
        [null, DictionaryValue.Create((1, 1), (2, 2)), false],
        [DictionaryValue.Create((1, 1), (2, 2)), DictionaryValue.Create((1, 1), (2, 2)), true]
    ];

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(DictionaryValue<int, int>? a, DictionaryValue<int, int> b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<DictionaryValue<int, int>?>.Default.Equals(a, b));
        Assert.Equal(expected, Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString =>
    [
        [null, null],
        [DictionaryValue.Create<string, int>(), "[]"],
        [DictionaryValue.Create(("a", 1), ("b", 2)), "[(a, 1),(b, 2)]"]
    ];

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(DictionaryValue<string, int>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson =>
    [
        ["null", null],
        ["{}", DictionaryValue.Create<string, int>()],
        ["""{"a":1,"b":2}""", DictionaryValue.Create(("a", 1), ("b", 2))]
    ];

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, DictionaryValue<string, int> expected)
    {
        var actual = JsonSerializer.Deserialize<DictionaryValue<string, int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));

        actual = JsonSerializer.Deserialize<DictionaryValue<string, int>>(json, DictionaryValueJsonContext.Default.Options);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual, DictionaryValueJsonContext.Default.Options));
    }
}

[JsonSerializable(typeof(DictionaryValue<string, int>))]
internal sealed partial class DictionaryValueJsonContext : JsonSerializerContext;
