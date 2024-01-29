using System.Text.Json.Serialization;

namespace ValueObject.Collections.Tests;

public sealed class HashSetValueTest
{
    public static object?[][] TestDataEquality =>
    [
        [null, null, true],
        [HashSetValue.Create(1, 2, 3), null, false],
        [null, HashSetValue.Create(1, 2, 3), false],
        [HashSetValue.Create(1, 2, 3), HashSetValue.Create(1, 2, 3), true]
    ];

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(HashSetValue<int>? a, HashSetValue<int>? b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<HashSetValue<int>?>.Default.Equals(a, b));
        Assert.Equal(expected, Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString =>
    [
        [null, null],
        [HashSetValue.Create<string>(), "[]"],
        [HashSetValue.Create<string>("a", "b"), "[a,b]"]
    ];

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(HashSetValue<string>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson =>
    [
        ["null", null],
        ["[]", HashSetValue.Create<int>()],
        ["[1,2,3]", HashSetValue.Create(1, 2, 3)]
    ];

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, HashSetValue<int> expected)
    {
        var actual = JsonSerializer.Deserialize<HashSetValue<int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));

        actual = JsonSerializer.Deserialize<HashSetValue<int>>(json, HashSetValueJsonContext.Default.Options);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual, HashSetValueJsonContext.Default.Options));
    }
}

[JsonSerializable(typeof(HashSetValue<int>))]
internal sealed partial class HashSetValueJsonContext : JsonSerializerContext;
