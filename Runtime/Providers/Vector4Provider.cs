using UnityEngine;
using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    public enum Vector4Options : uint
    {
        None = 0,
        IgnoreX = 1 << 0,
        IgnoreY = 1 << 1,
        IgnoreZ = 1 << 2,
        IgnoreW = 1 << 3, 
        IgnoreXY = IgnoreX | IgnoreY,
        IgnoreXZ = IgnoreX | IgnoreZ,
        IgnoreXW = IgnoreX | IgnoreW,
        IgnoreYZ = IgnoreY | IgnoreZ,
        IgnoreYW = IgnoreY | IgnoreW,
        IgnoreZW = IgnoreZ | IgnoreW,
        IgnoreXYZ = IgnoreX | IgnoreY | IgnoreZ,
        IgnoreXYW = IgnoreX | IgnoreY | IgnoreW,
        IgnoreXZW = IgnoreX | IgnoreZ | IgnoreW,
        IgnoreYZW = IgnoreY | IgnoreZ | IgnoreW,
    }

    public abstract class Vector4Provider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        public override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => Vector4.Lerp(from, to, t);
        public override Variant Add(Variant a, Variant b, uint optionsAsUint) => a.v4 + b.v4;
        public override Variant Read(TTarget target, uint optionsAsUint) => ReadVector(target);
        public override void Write(TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (Vector4Options)optionsAsUint;
            if (options != 0)
            {
                var old = ReadVector(target);
                if ((options & Vector4Options.IgnoreX) == Vector4Options.IgnoreX) v.v4.x = old.x;
                if ((options & Vector4Options.IgnoreY) == Vector4Options.IgnoreY) v.v4.y = old.y;
                if ((options & Vector4Options.IgnoreZ) == Vector4Options.IgnoreZ) v.v4.z = old.z;
                if ((options & Vector4Options.IgnoreW) == Vector4Options.IgnoreW) v.v4.w = old.w;
            }
            WriteVector(target, v);
        }

        protected abstract Vector4 ReadVector(TTarget target);
        protected abstract void WriteVector(TTarget target, Vector4 value);
    }

    /// <summary>
    /// Provides support for vector tweens using open callbacks.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class Vector4CallbackProvider<TTarget> : Vector4Provider<TTarget> where TTarget : class
    {
        public delegate Vector4 GetDelegate(TTarget target);
        public delegate void SetDelegate(TTarget target, Vector4 value);

        private GetDelegate _getter;
        private SetDelegate _setter;

        /// <summary>
        /// Construct a vector tween provider using a getter and setting callback.
        /// </summary>
        /// <param name="getter">Delegate used to get values from the target</param>
        /// <param name="setter">Delegate used to set values from the target</param>
        public Vector4CallbackProvider(GetDelegate getter, SetDelegate setter)
        {
            _getter = getter;
            _setter = setter;
        }

        protected override Vector4 ReadVector(TTarget target) => _getter(target);
        protected override void WriteVector(TTarget target, Vector4 value) => _setter(target, value);
    }

    /// <summary>
    /// Provides support for vector point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class Vector4MemberProvider<TTarget> : Vector4Provider<TTarget> where TTarget : class
    {
        private FastGetter<TTarget, Vector4> _getter;
        private FastSetter<TTarget, Vector4> _setter;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static Vector4MemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, Vector4MemberProvider<TTarget>>.Get(memberName);

        private Vector4MemberProvider(string memberName)
        {
            _getter = new FastGetter<TTarget, Vector4>(memberName);
            _setter = new FastSetter<TTarget, Vector4>(memberName);
        }

        protected sealed override Vector4 ReadVector(TTarget target) => _getter.GetValue(target);
        protected sealed override void WriteVector(TTarget target, Vector4 value) => _setter.SetValue(target, value);
    }
}
