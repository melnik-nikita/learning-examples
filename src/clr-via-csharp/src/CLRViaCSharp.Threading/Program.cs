using System.Reflection;
using CLRViaCSharp.Common;

ExamplesRunner.RunExamples(Assembly.GetCallingAssembly());

_ = string.Equals("test", "test", StringComparison.Ordinal);
