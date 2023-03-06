# Designing Types

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

# Kinds of Type Members

- __Constants__ - is a symbol that identifies a never-changing data value. Always associated with a type, not an
  instance of a type. Logically, constants are always static members
- __Fields__ - represents a read-oly or read/write data value. A field can be __static__ (considered as part of the
  type's state), __instance__ (nonstatic, considered as part of an object's state).
- __Instance constructors__ - is a special mehtod used to initialize a new object's instance fields to a good initial
  state.
- __Type constructors__ - is a special method used to initialize a type's static fields to a good initial state
- __Methods__ - is a function that performs operations that change or query the state of a type (static method) or an
  object (instance method).
- __Operator overloads__ - method that defined how an object should be manipulated when certain operators are applied to
  the object
- __Conversion operators__ - is a method that defines how to implicitly or explicitly cast or convert an object from one
  type to another type
- __Properties__ - is a mechanism that allows a simple, field-like syntax for setting or querying part of the logical
  state of a type 9static property) or object (instance property) while ensuring that the state doesn't become corrupt
- __Events__ - A static event is a mechanism that allows a type to send a notification to one or more static or instance
  methods. An instance nonstatic) event is a mechanism that allows an object to send a notification to one or more
  static or instance methods
- __Types__ - A type can define other types nested within it. This approach is typically used to break a large, complex
  type down into smaller building blocks to simplify the implementation.

# Type visibility

- __Internal__ - visible to all code within the defining assembly, and the type is not visible to code written in other
  assemblies.
- __Public__ - visible to al code withing the defining assembly as well aas all code written in other assemblies.
- __File__ - restricts a top-level types' scope and visibility to the file in which it's declared

# Member accessibility

- ___private___ - the member is accessible only by methods in the defining type or any nested type.
- ___protected___ - is accessible only by methods in the defining type, any nested type, or one of its derived types
  without regard to assembly.
- ___internal___ - is accessible only by methods in the defining assembly
- ___protected internal___ - is accessible by any nested type, any derived type (regardless of assembly), or any methods
  in the defining assembly
- ___public___ - is accessible to all methods in any assembly

# Methods

- __Instance Constructors and Classes__
    - Constructors are special methods that allow an instance of a type to be
      initialized to a good state.
    - When creating an instance of a ref type, memory is allocated to the instance's data fields, the object's overhead
      fields (type object pointer and sync block index) are initialized, and then the type's instance constructor is
      called to set the initial state of the object.
    - Instance ctors are never inherited.
    - If no ctor defined, the compiler defined a default (parameterless) ctor.
    - Constructors should not call virtual methods.
- __Instance Constructors and Structures__
    - A value type ctor is executed only when explicitly called.
    - C# disallows value types from defining parameterless ctors.
    - Without parameterless ctor, a value type's fields are always initialized to 0/null.
- __Type constructors__ (static constructors, class ctors, type initializers)
    - Can be applied to reference types and value types
    - Used to set initial state of a type (apart from instance ctors used to set initial state of an instance of a type)
    - Never hav parameters
    - Cannot have access modifiers (always private by default)
    - For thread safety,when a type ctor is called, the calling thread acquires a mutually exclusive thread
      synchronization lock.
- __Operator Overload Methods__
    - must be public and static
    - At least one of the operator method's parameters must be the same as the type that the operator method is defined
      within.
- __Conversion Operator Methods__
    - Are methods that convert an object from one type to another type
    - Must be public and static methods
    - Requires that either the parameter or the return type must be the same as the type that the conversion method is
      defined within.
    - You define implicit conversion only when precision or magnitude isn't lost during a conversion
    - You define explicit conversion if precision or magnitude is lost during hte conversion
- __Extension methods__
    - Allows to define a static method that you can invoke using instance method syntax
    - Must me declared in non-generic, static classes
    - C# compiler looks only for extension methods defined in static classes that are themselves defined at the file
      scope
    - Can be defined for interface types
    - When you mark a static method's first parameter with the 'this' keyword, the compiler internally applies a custom
      attribute to the method and this attribute is persisted in the resulting file's metadata [ExtensionAttribute]
- __Partial methods__
    - Can only be declared within a partial class or struct
    - Must always have a return type of void, and they cannot have any parameters marked with the out modifier
    - Declaration and implementation should have identical signatures
    - Partial methods are always considered to be private methods (also compiler forbids putting access modifier)