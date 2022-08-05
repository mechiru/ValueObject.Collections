namespace ValueObject.Collections.Tests;

public sealed class SortedSetValueTest
{
    public static object?[][] TestDataEquality => new[]
    {
        new object?[] { null, null, true },
        new object?[] { SortedSetValue.Create(1, 2, 3), null, false },
        new object?[] { null, SortedSetValue.Create(1, 2, 3), false },
        new object?[] { SortedSetValue.Create(1, 2, 3), SortedSetValue.Create(1, 2, 3), true },
    };

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(SortedSetValue<int>? a, SortedSetValue<int>? b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<SortedSetValue<int>?>.Default.Equals(a, b));
        Assert.Equal(expected, object.Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString => new[]
    {
        new object?[]{ null, null },
        new object?[]{SortedSetValue.Create<string>(), "[]" },
        new object?[]{SortedSetValue.Create<string>("a", "b"), "[a,b]" }
    };

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(SortedSetValue<string>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson => new[]
    {
        new object?[] { "null", null },
        new object?[] { "[]", SortedSetValue.Create<int>() },
        new object?[] { @"[1,2,3]", SortedSetValue.Create(1, 2, 3) },
    };

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, SortedSetValue<int> expected)
    {
        var actual = JsonSerializer.Deserialize<SortedSetValue<int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));
    }
}
