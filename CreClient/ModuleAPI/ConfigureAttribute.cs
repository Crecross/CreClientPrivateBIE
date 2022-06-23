#if MELONLOADER

using System;
using System.Reflection;

namespace CreClient.ModuleAPI
{
    public abstract class BaseConfigureAttribute : MemberAttribute
    {
        public static event Action<MemberInfo> MemberConfigured;

        protected BaseConfigureAttribute(string Path) : base(Path) { }

        public abstract event Action<dynamic> DynamicValueChanged;
        public abstract dynamic DynamicValue { get; set; }

        protected static void ConfigureMember(MemberInfo member) => MemberConfigured?.Invoke(member);
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConfigureAttribute<T> : BaseConfigureAttribute
    {
        public override dynamic DynamicValue {
            get => Value;
            set => Value = value;
        }

        public T Value
        {
            get => Getter();
            set {
                T val = Getter();
                // todo: fix edge case where val is non-null and value is null
                if ((value is null && val is null) || value.Equals(val))
                    return;

                if (Entry != null)
                    Entry.Value = value;

                Setter(value);
                ConfigureMember(Info);
            }
        }

        private MelonLoader.MelonPreferences_Entry<T> Entry;
        private Action<T> Setter;
        private Func<T> Getter;
        private MemberInfo Info;

        private readonly T Default;
        private readonly bool Saved;

        public override event Action<dynamic> DynamicValueChanged;

        public ConfigureAttribute(string Path, T Default, bool Saved = true) : base(Path)
        {
            this.Default = Default;
            this.Saved = Saved;
        }

        public override void Setup(Type Type, MemberInfo minfo)
        {
            MinimalSetup(Type, minfo);

            if (Saved)
            {
                Entry = MelonLoader.MelonPreferences.CreateCategory(Section).CreateEntry(Parts[Parts.Length - 1], Default);
                Entry.OnValueChanged += (old, current) => Value = current;
                Value = Entry.Value;
            }
        }

        public void MinimalSetup(Type Type, MemberInfo minfo)
        {
            base.Setup(Type, minfo);

            Info = minfo;

            if (minfo is PropertyInfo prop)
            {
                MethodInfo _setter = prop.GetSetMethod();
                if (_setter is not null)
                    Setter = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), _setter);
                else Setter = (T val) => { };

                MethodInfo _getter = prop.GetGetMethod();
                if (_getter is not null)
                    Getter = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), _getter);
                else Getter = () => default;
            }
            else if (minfo is FieldInfo field)
            {
                Setter = value => field.SetValue(null, value);
                Getter = () => (T)field.GetValue(null);
            }

            MemberConfigured += member =>
            {
                if (member == Info)
                    DynamicValueChanged?.Invoke(Getter());
            };
        }
    }
}

#endif
