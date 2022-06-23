#if MELONLOADER

using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CreClient.Managers
{
    public static class ModuleManager
    {
        public static void LoadModules()
        {
            foreach (Type type in AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(x => x.GetName().Name == "CreClient")
                .GetExportedTypes()
                .Where(x => x.Namespace.StartsWith("CreClient.Modules") && x.GetCustomAttributes(typeof(Module), false).Count() > 0))
                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }
    }
}

#endif
