namespace ValueObject.Collections.Tests;

public sealed class SortedSetValueTest
{
    public static object?[][] TestDataEquality =>
    [
        [null, null, true],
        [SortedSetValue.Create(1, 2, 3), null, false],
        [null, SortedSetValue.Create(1, 2, 3), false],
        [SortedSetValue.Create(1, 2, 3), SortedSetValue.Create(1, 2, 3), true]
    ];

    [Theory]
    [MemberData(nameof(TestDataEquality))]
    public void TestEquality(SortedSetValue<int>? a, SortedSetValue<int>? b, bool expected)
    {
        Assert.Equal(expected, EqualityComparer<SortedSetValue<int>?>.Default.Equals(a, b));
        Assert.Equal(expected, Equals(a, b));
        Assert.Equal(expected, a?.Equals(b) ?? b is null);
        Assert.Equal(expected, a == b);
    }

    public static object?[][] TestDataToString =>
    [
        [null, null],
        [SortedSetValue.Create<string>(), "[]"],
        [SortedSetValue.Create<string>("a", "b"), "[a,b]"]
    ];

    [Theory]
    [MemberData(nameof(TestDataToString))]
    public void TestToString(SortedSetValue<string>? input, string? expected)
    {
        Assert.Equal(expected, input?.ToString());
    }

    public static object?[][] TestDataJson =>
    [
        ["null", null],
        ["[]", SortedSetValue.Create<int>()],
        ["[1,2,3]", SortedSetValue.Create(1, 2, 3)]
    ];

    [Theory]
    [MemberData(nameof(TestDataJson))]
    public void TestJson(string json, SortedSetValue<int> expected)
    {
        var actual = JsonSerializer.Deserialize<SortedSetValue<int>>(json);
        Assert.Equal(expected, actual);
        Assert.Equal(json, JsonSerializer.Serialize(actual));
    }
}
