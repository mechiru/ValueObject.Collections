# ValueObject.Collections

This library provides value object for collections.

## Notes

Currently, if you use a `List<T>` as a property of a record class or struct,
comparisons between the values of that record will return `false` unless the
references of the properties using the `List<T>` are equal.

Therefore, collections are compared based on the equality of element
values, and all properties of records can be compared correctly as
values.

Using this library's collections makes it easy to assert types using
records without any additional libraries.

This may not be necessary if the code that is automatically generated
when there is a collection such as `List<T>` as a C# record property
changes.

## Examples

### Equality
```csharp
var a = ListValue.Create(1, 2, 3);
var b = ListValue.Create(1, 2, 3);

EqualityComparer<ListValue<int>>.Default.Equals(a, b);
// => true
object.Equals(a, b);
// => true
a.Equals(b) ?? b;
// => true
a == b;
// => true
```

### ToString
```csharp
ListValue.Create(1, 2, 3).ToString();
// => [1,2,3]

DictionaryValue.Create(("a", 1), ("b", 2)).ToString();
// => [(a, 1),(b, 2)]
```

## Lisence
This library is under the MIT License.
