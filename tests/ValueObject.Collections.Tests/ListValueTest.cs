using System.Text.Json.Serialization;

namespace ValueObject.Collections.Tests;

public sealed class ListValueTest
{
    public static object?[][] TestDataEquality =>
    [
        [null, null, true],
        [ListValue.Create(1, 2, 3), null, false],
        [null, ListValue.Create(1, 2, 3), false],
        [ListValue.Create(1, 2, 3), ListValue.Create(1, 2, 3), true]
    ];

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(ListValue<int>? a, ListValue<int>? b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<ListValue<int>?>.Default.Equals(a, b));
        Assert.Equal(expected, Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString =>
    [
        [null, null],
        [ListValue.Create<string>(), "[]"],
        [ListValue.Create<string>("a", "b"), "[a,b]"]
    ];

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(ListValue<string>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson =>
    [
        ["null", null],
        ["[]", ListValue.Create<int>()],
        ["[1,2,3]", ListValue.Create(1, 2, 3)]
    ];

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, ListValue<int> expected)
    {
        var actual = JsonSerializer.Deserialize<ListValue<int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));

        actual = JsonSerializer.Deserialize<ListValue<int>>(json, ListValueJsonContext.Default.Options);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual, ListValueJsonContext.Default.Options));
    }
}

[JsonSerializable(typeof(ListValue<int>))]
internal sealed partial class ListValueJsonContext : JsonSerializerContext;
