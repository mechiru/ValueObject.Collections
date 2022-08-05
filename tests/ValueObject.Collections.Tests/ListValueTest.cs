namespace ValueObject.Collections.Tests;

public sealed class ListValueTest
{
    public static object?[][] TestDataEquality => new[]
    {
        new object?[] { null, null, true },
        new object?[] { ListValue.Create(1, 2, 3), null, false },
        new object?[] { null, ListValue.Create(1, 2, 3), false },
        new object?[] { ListValue.Create(1, 2, 3), ListValue.Create(1, 2, 3), true },
    };

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(ListValue<int>? a, ListValue<int>? b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<ListValue<int>?>.Default.Equals(a, b));
        Assert.Equal(expected, object.Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString => new[]
    {
        new object?[]{ null, null },
        new object?[]{ListValue.Create<string>(), "[]" },
        new object?[]{ListValue.Create<string>("a", "b"), "[a,b]" }
    };

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(ListValue<string>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson => new[]
    {
        new object?[] { "null", null },
        new object?[] { "[]", ListValue.Create<int>() },
        new object?[] { @"[1,2,3]", ListValue.Create(1, 2, 3) },
    };

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, ListValue<int> expected)
    {
        var actual = JsonSerializer.Deserialize<ListValue<int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));
    }
}