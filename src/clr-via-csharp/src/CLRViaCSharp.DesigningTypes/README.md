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

# Parameters

- __Optional and Named Parameters__
    - Default values for the parameters of methods, constructor methods, and parameterful properties (C# indexes) can be
      specified
    - Parameters with default values must come after any parameters that do not have default values with one exception:
      a ___params___ array parameter must come after all parameters, and the array cannot have a default value itself
    - Default values must me constant values known at compile time
    - If parameter variables are renamed, any callers who are passing arguments by parameter name will have to modify
      their code
    - Changing a parameter's default value is potentially dangerous if the method is called from outside the module (a
      call side embeds the default value into its call, so without recompiling caller the old value may be used)
    - default values for ___ref___ or ___out___ parameters cannot be set
- __Passing Parameters by Reference to a Method__
    - All parameters are passed by value by default
        - When reference type objects are passed, the reference to the object is passed (by value) to the method (so the
          caller can modify object fields)
        - For value type instances, a copy of the instance is passed to the method
    - ___ref___ and ___out___ keywords are used to pass parameters by reference
        - If a method's parameter is marked with ___out___, the caller isn't expected to have initialized the object
          prior to calling the method.
            - THE CALLED METHOD MUST write value to the parameter before returning
        - If a method's parameter is marked with ___ref___, THE CALLER MUST initialize the parameter's value prior to
          calling the method.
            - the called method can read from the value and/or write the value
    - With value types, ___out___ and ___ref___ allow a method to manipulate a single value type instance, the caller
      must allocate the memory for the instance, and the callee manipulates that memory
    - With reference types, the caller allocates memory for a pointer to a reference object, and the callee manipulates
      this pointer
- __Passing a variable number of arguments to a method__
    - ___params___ keyword allows us to define a method that can accept a variable number of arguments
    - the ___params___ keyword tells the compiler to apply an instance of ___ParamsArrayAttribute___ custom attribute to
      the parameter
    - only the last parameter to a method can be marked with the ___params___ keyword
- __Parameter and Return type Guidelines__
    - When declaring a method's parameter types, you should specify the weakest type possible, preferring interfaces
      over base classes
    - It is usually best to declare a method's return type by using hte strongest type possible

# Properties

- A property is a ember that provides a flexible mechanism to read, write or compute the alue of a private field.
- They are special methods called ___accessors___
- A ___get___ property accessor is used to return the property value, and a ___set___ property accessor is used to
  assign a new
  value
- The ___value___ keyword is used to define the value being assigned by the set or init accessor.
- Properties can be read-write (they have both a get and a set accessor), read-only (they have a get accessor but no set
  accessor), or write-only (they have a set accessor, but no get accessor). Write-only properties are rare and are most
  commonly used to restrict access to sensitive data.
- Simple properties that require no custom accessor code can be implemented either as expression body definitions or as
  auto-implemented properties.

# Events

Defining an event member means that a type is offering the following capabilities:

- A method can register its interest i the event
- A method can unregister its interest in the event
- Registered methods will be notified when event occurs

#### Designing a type that exposes an event

- Step 1: Define a type that will hold any additional info that should be sent to receivers of the event notification
    - classes that hold event information to be passed to hte event handler should be derived from
      ___System.EventArgs___
    - the name of the class should be suffixed with ___EventArgs___
- Step 2: Define the event member
- Step 3: Define a method responsible for raising the event to notify registered objects that the event has occurred
    - By convention, the class should define a protected, virtual method that is called by code internally withing the
      class and its derived classes whe the event is to be raised
- Step 4: Define a method that translates the input into the desired event

#### Designing a type that listens for an event

- Create a class and define method that type, that exposes the event, will call when event occurs
- Register our callback method in an event
- Unregister callback when no longer need to receive events

# Generics

Generics is a mechanism, offered by the CLR, that provides code reuse in a form of ___algorithm reuse___

### CLR allows the creation of generic:

- reference types
- value types
- interfaces
- delegates
- methods, that defined in a reference type, value type or interface

### Generics provide the following benefits:

- Source code protection
- Type safety
- Cleaner code
- Better performance

### Code explosion

CLR generates native code for every generic method/type combination - which is called code explosion. This end up
increasing hte application's working set, hurting the performance.
To avoid this CLR does some optimisations:

- CLR will compile the code for method/type combination just once.
- CLR considers all reference type arguments to be identical, so the code is shared. (for any ref type the same
  generated code is used)
- For a value types, CLR must produce native code specifically for that value type, because value types can vary in
  size.

## Verifiability and Constraints

- A constraint is a way to limit the number of types that can be specified for a generic argument.

```
public static Min<T>(T o1, T o2) where T : IComparable<T> {
    // implementation goes here
}
```

- The CLR doesn't allow overloading based on type parameter names or constraints; you can overload types or methods
  based only on arity

```
// This is ok
internal sealed class AType {}
internal sealed class AType<T> {}
internal sealed class AType<T1, T2> {}
// This conflicts with AType<T> that has no constraints
internal sealed class AType<T> where T : IComparable<T> {}
// Error: basicaly the same as AType<T1, T2>
internal sealed class AType<T3, T4> {}
```

- when overriding a virtual generic method, the overriding method must specif the same number of type parameters, and
  these type parameters will inherit the constraints specified on the by the base class's method.

### Type parameter constraints

1. Primary constraint
2. Secondary constraint
3. Constructor constraint

#### Primary constraints

- A type parameter can have zero or one primary constraint.
- Can be a ref type that is not sealed
- There are 2 special types of primary constraint: ___class___ and ___struct___

#### Secondary constraints

- A type parameter can specify zero or more secondary constraints where a secondary constraint represents an interface
  type.
- A type parameter constraint allows a generic type or a method to indicate that there ust be a relationship between
  specified type arguments (can be zero or more)

```
private static List<TBase> ConvertIList<T, TBase>(IList<T> list) where T : TBase
{
    List<TBase> baseList = new List<TBase>(list.Count);
    for(int index = 0; index < list.Count; index++)
    {
        baseList.Add(list[index]);
    }
    return baseList;
}
```

#### Constructor constraints

- A type parameter can specify zero constructor constraints or one constructor constraint
- When specified, you are promising hte compiler that a specified type argument will be a non-abstract type that
  implements a public parameterless constructor

# Interfaces

An Interface is a named set of method signatures.

- cannot define any ctor methods
- not allowed to define any instance fields
- C# compiler requires that a method that implements an interface be marked as ___public___
- The CLR requires that interface methods be marked as ___virtual___
- If method is not marked as ___virtual___, compiler marks virtual and sealed; this prevents a derived class from
  overriding the interface method
- Derived class can re-inherit same interface and provide own implementation
- When you cast an instance of a value type to an interface type, the value type instance must be boxed (because an
  interface variable is a ref that must point to an object on the heap)