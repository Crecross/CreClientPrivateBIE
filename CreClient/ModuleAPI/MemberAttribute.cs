#if MELONLOADER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreClient.ModuleAPI
{
    public abstract class MemberAttribute : Attribute
    {
        public static event Action<MemberAttribute> Added;
        public static List<MemberAttribute> All = new();

        public Type Type;

        public string[] Parts;
        public string Display;
        public string Description;

        public string Name { get => Display ?? Parts[Parts.Length - 1]; }
        public string Section { get => string.Join(".", Parts.Take(Parts.Length - 1)); }

        public MemberAttribute(string Path)
        {
            Parts = Path.Split('.');
        }

        public virtual void Setup(Type Type, MemberInfo minfo) => this.Type = Type;
        internal void SetupInternal(Type Type, MemberInfo minfo)
        {
            Setup(Type, minfo);
            All.Add(this);
            Added?.Invoke(this);
        }
    }
}

#endif
