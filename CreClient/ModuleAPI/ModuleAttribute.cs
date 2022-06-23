#if MELONLOADER

using System;

namespace CreClient.ModuleAPI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute 
    {
        internal Type Type;
    }
}

#endif
