# CLR via C# book code examples

# Type fundamentals

```var employee = new Employee();```

What 'new' operator does:

- Calculates the number of bytes required by all instance fields defined in the type and all of its base types to and
  including System.Object. Every object on the heap requires some additional members called the type object pointer and
  the sync block index - used by the CLR to manage the object. The bytes for these additional members are added the the
  size of the object.
- It allocates memory for the object by allocating hte number fo bytes required for the specified type from the managed
  hep all of these bytes are then set to zero.
- It initializes the object's type object pointer and sync block index members.
- The type's instance constructor is called, passing it any arguments specified in the call to new.
  Most compilers automatically emit code in a constructor to call a base class's constructor.
  Each constructor is responsible for initializing the instance fields defined by the type whose constructor is being
  called.
  Eventually, System.Object's constructor is called, and this constructor method does nothing but return.

# Reference and Value Types

- Reference types are always allocated from the managed heap.
    - The memory must be allocated from the managed heap.
    - Each object allocated on the heap has some additional overhead members associated with it that must be
      initialized.
    - The other bytes in the object (for the fields) are always set to zero.
    - Allocating an object from the managed heap could force a garbage collection to occur.
- Value types instances are USUALLY allocated on the thread's stack (although they an also be embedded as a field in a
  reference type object).
    - All value types are sealed, which prevents a value type from bing used as a base type for any other reference
      type or value type.
