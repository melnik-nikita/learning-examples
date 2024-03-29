﻿using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

internal sealed class ReferenceAndValueTypes : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        // Reference types are always allocated from the managed heap.
        // Value type instances are USUALLY allocated on the thread's stack

        // What 'new' operator does:
        //  - Calculates the number of bytes required by all instance fields defined in the type and all of its base types to and including System.Object.
        //      Every object on the heap requires some additional members called the type object pointer and the sync block index - used by the CLR to manage the object.
        //      The bytes for these additional members are added the the size of the object.
        //  - It allocates memory for the object by allocating hte number fo bytes required for the specified type from the managed hep all of these bytes are then set to zero.
        //  - I initializes the object's type object pointer and sync block index members.
        //  - Tye type's instance constructor is called, passing it any arguments specified in the call to new.
        //      Most compilers automatically emit code in a constructor to call a base class's constructor.
        //      Each constructor is responsible for initializing the instance fields defined by the type whose constructor is being called.
        //      Eventually, System.Object's constructor is called, and this constructor method does nothing but return.
        var employee = new Employee();
    }
}

file sealed class Employee : Object
{
}
