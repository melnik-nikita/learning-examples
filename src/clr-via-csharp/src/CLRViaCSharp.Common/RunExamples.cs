using System.Reflection;
using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.Common;

public static class ExamplesRunner
{
    public static void RunExamples(Assembly assembly)
    {
        var types = assembly.GetTypes();
        var examples = types.Where(x => x.GetInterface(nameof(IExample)) is not null);

        foreach (var example in examples)
        {
            example.GetMethod(nameof(IExample.Run))!.Invoke(example, null);
        }
    }
}
