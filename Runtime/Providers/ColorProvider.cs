
using System;
using UnityEngine;
using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    [Flags]
    public enum ColorOptions : uint
    {
        None = 0,
        IgnoreR = 1 << 0,
        IgnoreG = 1 << 1,
        IgnoreB = 1 << 2,
        IgnoreA = 1 << 3,
        IgnoreRGB = IgnoreR | IgnoreG | IgnoreB
    }

    public abstract class ColorProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        public sealed override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => from.f + (to.f - from.f) * t;
        public sealed override Variant Add(Variant a, Variant b, uint optionsAsUint) => a.f + b.f;
        public sealed override Variant Read(TTarget target, uint optionsAsUint) => ReadColor(target);
        public sealed override void Write(TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (ColorOptions)optionsAsUint;
            if (options != 0)
            {
                var old = ReadColor(target);
                if ((options & ColorOptions.IgnoreR) == ColorOptions.IgnoreR) v.c.r = old.r;
                if ((options & ColorOptions.IgnoreG) == ColorOptions.IgnoreG) v.c.g = old.g;
                if ((options & ColorOptions.IgnoreB) == ColorOptions.IgnoreB) v.c.b = old.b;
                if ((options & ColorOptions.IgnoreA) == ColorOptions.IgnoreA) v.c.a = old.a;
            }
            WriteColor(target, v);
        }

        protected abstract Color ReadColor(TTarget target);
        protected abstract void WriteColor(TTarget target, Color value);
    }

    public class ColorCallbackProvider<TTarget> : ColorProvider<TTarget> where TTarget : class
    {
        public delegate Color GetDelegate(TTarget target);
        public delegate void SetDelegate(TTarget target, Color value);

        private GetDelegate _getter;
        private SetDelegate _setter;

        public ColorCallbackProvider(GetDelegate getter, SetDelegate setter)
        {
            _getter = getter;
            _setter = setter;
        }

        protected override Color ReadColor(TTarget target) => _getter(target);
        protected override void WriteColor(TTarget target, Color value) => _setter(target, value);
    }

    public class ColorMemberProvider<TTarget> : ColorProvider<TTarget> where TTarget : class
    {
        private FastGetter<TTarget, Color> _getter;
        private FastSetter<TTarget, Color> _setter;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns>Cached color provider</returns>
        public static ColorMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, ColorMemberProvider<TTarget>>.Get(memberName);

        private ColorMemberProvider(string memberName)
        {
            _getter = new FastGetter<TTarget, Color>(memberName);
            _setter = new FastSetter<TTarget, Color>(memberName);
        }

        protected override Color ReadColor(TTarget target) => _getter.GetValue(target);
        protected override void WriteColor(TTarget target, Color value) => _setter.SetValue(target, value);
    }
}
