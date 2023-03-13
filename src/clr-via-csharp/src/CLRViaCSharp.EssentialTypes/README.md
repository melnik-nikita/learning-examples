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