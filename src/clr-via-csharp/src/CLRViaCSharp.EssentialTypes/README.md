# Essential Types

# Chars, Strings and working with text

## Characters

- represented in 16-bit Unicode code values
- A character is represented with an instance of the System.Char structure, so char is a value type

# Strings

- Represents an immutable sequence of characters.
- Is a reference type
- can't use ___new___ operator to construct a string object

# Enumerated Types and Bit flags

An ___enumerated type___ is a type that defines a set of symbolic name and value pairs.

Reasons to use enumerated types:

1. Make the program much easier to write, read, and maintain.
2. Enumerated types are strongly typed

Ever enumerated type has an underlying type, which can be a ___byte___, ___byte___, ___short___, ___ushort___,
___int___ (default), ___uint___.

### Bit flags

Bit flags represent a set of bits, some of which are on, and some of which are off. (Enums may have only one value)

Enumerated types can define methods using __extension method__ syntax.

# Arrays

- Arrays are mechanisms that allow you to treat several items as a single collection.
- Arrays can be single-dimensional, multi-dimensional and jagged (arrays of arrays)
- Arrays are always reference types
- All arrays __implicitly__ implement ___IEnumerable___, ___ICollection___, and ___IList___

__Array covariance__ - when one array type is casted to another array type

# Delegates (aka callback in other languages)

A ___Delegate___ is a type that represents references to methods with a particular parameter list and return type.

- delegates ensure that a callback method is type-safe
- integrate the ability to call multiple methods sequentially
- support the calling of static methods as well as instance methods
- c# and the CLR allow covariance and contra-variance of reference types when binding ta method to a delegate
    - covariance means that a method can return a type that is derived from the delegate's return type
    - contra-variance means that a method can take a parameter that is a base of the delegate's parameter type
    - covariance and contra-variance is supported by reference types only
  ```
  delegate Object MyCallback(FileStream fs);
  string SomeMethod(Stream s); // valid 
  Int32 SomeOtherMethod(Stream s); // invalid
  ```
- ___lambda expressions___ may be used where a delegate is expected

# Custom Attributes

Custom attributes allow to declaratively annotate code constructs, enabling special feature. The allow information to be
defined and applied to almost any metadata table entry. The metadata info can be queried at run time to dynamically
alter the way code executes.
Custom attributes are just a way to associate additional information with target.
Attributes can have prefixes to indicate the target the attribute applies to.

```
[assembly: SomeAttr]
[module: SomeAttr]
[type: SomeAttr]
[field: SomeAttr]
[return: SomeAttr]
[method: SomeAttr]
[param: SomeAttr]
[property: SomeAttr]
[event: SomeAttr]
```