#if MELONLOADER

global using CreClient.ModuleAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CreClient.Managers
{
    public static class ModuleManager
    {
        public static void Register() => Register(Assembly.GetCallingAssembly());
        public static void Register(Assembly assembly)
        {
            IEnumerable<ModuleAttribute> modules = assembly.GetExportedTypes()
                .Select(t =>
                {
                    var attribute = t.GetCustomAttribute<ModuleAttribute>();
                    if (attribute != null)
                        attribute.Type = t;
                    return attribute;
                })
                .Where(x => x is not null);

            foreach (ModuleAttribute module in modules)
            {
                Utils.SmartLogger.Debug("Initializing " + module.Type.FullName);
                try { SetupModule(module); }
                catch (Exception ex) { Utils.SmartLogger.Error("Exception occurred whilst loading " + module.Type.FullName + ": " + ex); }
            };
        }

        public static void SetupModule(ModuleAttribute module)
        {
            RuntimeHelpers.RunClassConstructor(module.Type.TypeHandle);

            foreach (MemberInfo member in module.Type.GetMembers())
            {
                var members = member.GetCustomAttributes<MemberAttribute>();
                foreach (MemberAttribute member2 in members)
                    member2.SetupInternal(module.Type, member);
            };
        }
    }
}

#endif
