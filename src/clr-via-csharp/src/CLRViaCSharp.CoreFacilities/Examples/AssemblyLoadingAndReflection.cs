using System.Data;
using System.Reflection;
using System.Text;
using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.CoreFacilities.Examples;

internal class AssemblyLoadingAndReflection : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Assembly Loading and Reflection example start");
        var assembly = Assembly.GetAssembly(typeof(DataTable));

        if (assembly?.FullName is not null)
        {
            LoadAssemblyAndShowPubicTypes(assembly.FullName);
        }

        Console.WriteLine("---------------------------");

        ShowExceptionsHierarchy();
        Console.WriteLine("---------------------------");

        ShowMethodsInfo();

        Console.WriteLine("Assembly Loading and Reflection example end");
        Console.WriteLine("---------------------------");
    }

    private static void ShowMethodsInfo()
    {
        // Loop through all assemblies loaded in this appDomain
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        void Show(int indent, string format, object[] args)
        {
            Console.WriteLine(new string(' ', 3 * indent) + format, args);
        }

        foreach (var assembly in assemblies)
        {
            Show(0, "Assembly: {0}", new object[] { assembly });

            // FInd Types in the assembly
            foreach (var type in assembly.ExportedTypes)
            {
                Show(1, "Type: {0}", new object[] { type });

                // Discover the type's members
                foreach (var memberInfo in type.GetTypeInfo().DeclaredMembers)
                {
                    var typeName = memberInfo switch
                    {
                        Type => "(Nested) Type",
                        FieldInfo => "FieldInfo",
                        MethodInfo => "MethodInfo",
                        ConstructorInfo => "ConstructorInfo",
                        PropertyInfo => "PropertyInfo",
                        EventInfo => "EventInfo",
                        _ => string.Empty
                    };
                    Show(2, "{0}: {1}", new object[] { typeName, memberInfo });
                }
            }
        }
    }

    private static void LoadAssemblyAndShowPubicTypes(string assemblyId)
    {
        // Explicitly load an assembly in to this AppDomain
        var a = Assembly.Load(assemblyId);

        // Execute this loop once for each Type
        // publicly-exported from the loaded assembly
        foreach (var type in a.ExportedTypes)
        {
            // Display the full name of the type
            Console.WriteLine(type.FullName);
        }
    }

    private static void ShowExceptionsHierarchy()
    {
        // Explicitly load the assemblies that we want to reflect over
        LoadAssemblies();

        // Filter & sort all the types
        var allTypes = (from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.ExportedTypes
                where typeof(Exception).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo())
                orderby t.Name
                select t
            ).ToArray();

        // Build the inheritance hierarchy tree and show it
        Console.WriteLine(WalkInheritanceHierarchy(new StringBuilder(), 0, typeof(Exception), allTypes));
    }

    private static void LoadAssemblies()
    {
        String[] assemblies =
        {
            "System,                            PublicKeyToken={0}", "System.Core,                   PublicKeyToken={1}",
            "System.Data,                       PublicKeyToken={1}", "System.Drawing,                PublicKeyToken={1}",
            "System.Security,                   PublicKeyToken={1}", "System.ServiceProcess,         PublicKeyToken={1}",
            "System.Text.RegularExpressions,    PublicKeyToken={1}", "System.Web,                    PublicKeyToken={1}",
            "System.Xml,                        PublicKeyToken={0}"
        };

        // Get the version of the assembly containing System.Object
        // We'll assume the same version for all the other assemblies
        var baseAssembly = typeof(Object).Assembly;
        var version = baseAssembly.GetName().Version;

        var ecmaPublicKeyToken = "b77a5c561934e089";
        var mSPublicKeyToken = "b03f5f7f11d50a3a";

        // Explicitly load the assemblies that we want to reflect over
        foreach (var assembly in assemblies)
        {
            var assemblyIdentity = String.Format(assembly, ecmaPublicKeyToken, mSPublicKeyToken) + ", Culture=neutral, Version=4.0.0.0";
            Assembly.Load(assemblyIdentity);
        }
    }

    private static string WalkInheritanceHierarchy(StringBuilder stringBuilder, int indent, Type baseType, IEnumerable<Type> allTypes)
    {
        var spaces = new string(' ', indent * 3);
        stringBuilder.AppendLine(spaces + baseType.FullName);
        foreach (var type in allTypes)
        {
            if (type.GetTypeInfo().BaseType != baseType)
            {
                continue;
            }

            WalkInheritanceHierarchy(stringBuilder, indent + 1, type, allTypes);
        }

        return stringBuilder.ToString();
    }
}
