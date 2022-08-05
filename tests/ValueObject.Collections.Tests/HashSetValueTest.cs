namespace ValueObject.Collections.Tests;

public sealed class HashSetValueTest
{
    public static object?[][] TestDataEquality => new[]
    {
        new object?[] { null, null, true },
        new object?[] { HashSetValue.Create(1, 2, 3), null, false },
        new object?[] { null, HashSetValue.Create(1, 2, 3), false },
        new object?[] { HashSetValue.Create(1, 2, 3), HashSetValue.Create(1, 2, 3), true },
    };

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(HashSetValue<int>? a, HashSetValue<int>? b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<HashSetValue<int>?>.Default.Equals(a, b));
        Assert.Equal(expected, object.Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString => new[]
    {
        new object?[]{ null, null },
        new object?[]{HashSetValue.Create<string>(), "[]" },
        new object?[]{HashSetValue.Create<string>("a", "b"), "[a,b]" }
    };

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(HashSetValue<string>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson => new[]
    {
        new object?[] { "null", null },
        new object?[] { "[]", HashSetValue.Create<int>() },
        new object?[] { @"[1,2,3]", HashSetValue.Create(1, 2, 3) },
    };

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, HashSetValue<int> expected)
    {
        var actual = JsonSerializer.Deserialize<HashSetValue<int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));
    }
}
