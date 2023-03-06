﻿using CLRViaCSharp.DesigningTypes.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

internal sealed class ReferenceAndValueTypes : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        // Reference types are always allocated from the managed heap.
        // Value type instances are USUALLY allocated on the thread's stack
    }
}