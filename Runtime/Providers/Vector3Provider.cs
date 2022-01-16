using UnityEngine;
using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    public enum Vector3Options : uint
    {
        None = 0,
        IgnoreX = 1 << 0,
        IgnoreY = 1 << 1,
        IgnoreZ = 1 << 2,
        IgnoreXY = IgnoreX | IgnoreY,
        IgnoreXZ = IgnoreX | IgnoreZ,
        IgnoreYZ = IgnoreY | IgnoreZ
    }

    public abstract class Vector3Provider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        public override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => Vector3.Lerp(from, to, t);
        public override Variant Add(Variant a, Variant b, uint optionsAsUint) => a.v3 + b.v3;
        public override Variant Read(TTarget target, uint optionsAsUint) => ReadVector(target);
        public override void Write(TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (Vector3Options)optionsAsUint;
            if (options != 0)
            {
                var old = ReadVector(target);
                if ((options & Vector3Options.IgnoreX) == Vector3Options.IgnoreX) v.v3.x = old.x;
                if ((options & Vector3Options.IgnoreY) == Vector3Options.IgnoreY) v.v3.y = old.y;
                if ((options & Vector3Options.IgnoreZ) == Vector3Options.IgnoreY) v.v3.z = old.z;
            }
            WriteVector(target, v);
        }

        protected abstract Vector3 ReadVector(TTarget target);
        protected abstract void WriteVector(TTarget target, Vector3 value);
    }

    /// <summary>
    /// Provides support for vector tweens using open callbacks.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class Vector3CallbackProvider<TTarget> : Vector3Provider<TTarget> where TTarget : class
    {
        public delegate Vector3 GetDelegate(TTarget target);
        public delegate void SetDelegate(TTarget target, Vector3 value);

        private GetDelegate _getter;
        private SetDelegate _setter;

        /// <summary>
        /// Construct a vector tween provider using a getter and setting callback.
        /// </summary>
        /// <param name="getter">Delegate used to get values from the target</param>
        /// <param name="setter">Delegate used to set values from the target</param>
        public Vector3CallbackProvider(GetDelegate getter, SetDelegate setter)
        {
            _getter = getter;
            _setter = setter;
        }

        protected override Vector3 ReadVector(TTarget target) => _getter(target);
        protected override void WriteVector(TTarget target, Vector3 value) => _setter(target, value);
    }

    /// <summary>
    /// Provides support for vector point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class Vector3MemberProvider<TTarget> : Vector3Provider<TTarget> where TTarget : class
    {
        private FastGetter<TTarget, Vector3> _getter;
        private FastSetter<TTarget, Vector3> _setter;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static Vector3MemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, Vector3MemberProvider<TTarget>>.Get(memberName);

        private Vector3MemberProvider(string memberName)
        {
            _getter = new FastGetter<TTarget, Vector3>(memberName);
            _setter = new FastSetter<TTarget, Vector3>(memberName);
        }

        protected sealed override Vector3 ReadVector(TTarget target) => _getter.GetValue(target);
        protected sealed override void WriteVector(TTarget target, Vector3 value) => _setter.SetValue(target, value);
    }
}
