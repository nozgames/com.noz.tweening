using UnityEngine;
using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    public enum Vector2Options : uint
    {
        None = 0,
        IgnoreX = 1 << 0,
        IgnoreY = 1 << 1,
    }

    public abstract class Vector2Provider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        public override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => Vector2.Lerp(from, to, t);
        public override Variant Add(Variant a, Variant b, uint optionsAsUint) => a.v2 + b.v2;
        public override Variant Read(TTarget target, uint optionsAsUint) => ReadVector(target);
        public override void Write(TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (Vector2Options)optionsAsUint;
            if (options != 0)
            {
                var old = ReadVector(target);
                if ((options & Vector2Options.IgnoreX) == Vector2Options.IgnoreX) v.v2.x = old.x;
                if ((options & Vector2Options.IgnoreY) == Vector2Options.IgnoreY) v.v2.y = old.y;
            }
            WriteVector(target, v);
        }

        protected abstract Vector2 ReadVector(TTarget target);
        protected abstract void WriteVector(TTarget target, Vector2 value);
    }

    /// <summary>
    /// Provides support for vector tweens using open callbacks.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class Vector2CallbackProvider<TTarget> : Vector2Provider<TTarget> where TTarget : class
    {
        public delegate Vector2 GetDelegate(TTarget target);
        public delegate void SetDelegate(TTarget target, Vector2 value);

        private GetDelegate _getter;
        private SetDelegate _setter;

        /// <summary>
        /// Construct a vector tween provider using a getter and setting callback.
        /// </summary>
        /// <param name="getter">Delegate used to get values from the target</param>
        /// <param name="setter">Delegate used to set values from the target</param>
        public Vector2CallbackProvider(GetDelegate getter, SetDelegate setter)
        {
            _getter = getter;
            _setter = setter;
        }

        protected override Vector2 ReadVector(TTarget target) => _getter(target);
        protected override void WriteVector(TTarget target, Vector2 value) => _setter(target, value);
    }

    /// <summary>
    /// Provides support for vector point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class Vector2MemberProvider<TTarget> : Vector2Provider<TTarget> where TTarget : class
    {
        private FastGetter<TTarget, Vector2> _getter;
        private FastSetter<TTarget, Vector2> _setter;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static Vector2MemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, Vector2MemberProvider<TTarget>>.Get(memberName);

        private Vector2MemberProvider(string memberName)
        {
            _getter = new FastGetter<TTarget, Vector2>(memberName);
            _setter = new FastSetter<TTarget, Vector2>(memberName);
        }

        protected sealed override Vector2 ReadVector(TTarget target) => _getter.GetValue(target);
        protected sealed override void WriteVector(TTarget target, Vector2 value) => _setter.SetValue(target, value);
    }
}
