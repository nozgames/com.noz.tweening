using System;
using UnityEngine;
using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    public abstract class QuaternionProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        public override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => Quaternion.Slerp(from, to, t);
        public override Variant Add(Variant a, Variant b, uint optionsAsUint) => a.q * b.q;
        public override Variant Read(TTarget target, uint optionsAsUint) => ReadQuaternion(target);
        public override void Write(TTarget target, Variant v, uint optionsAsUint) => WriteQuaternion(target, v);

        protected abstract Quaternion ReadQuaternion(TTarget target);
        protected abstract void WriteQuaternion(TTarget target, Quaternion value);
    }

    /// <summary>
    /// Provides support for quaternion tweens using open callbacks.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class QuaternionCallbackProvider<TTarget> : QuaternionProvider<TTarget> where TTarget : class
    {
        public delegate Quaternion GetDelegate(TTarget target);
        public delegate void SetDelegate(TTarget target, Quaternion value);

        private GetDelegate _getter;
        private SetDelegate _setter;

        /// <summary>
        /// Construct a quaternion tween provider using a getter and setting callback.
        /// </summary>
        /// <param name="getter">Delegate used to get values from the target</param>
        /// <param name="setter">Delegate used to set values from the target</param>
        public QuaternionCallbackProvider(GetDelegate getter, SetDelegate setter)
        {
            _getter = getter;
            _setter = setter;
        }

        protected override Quaternion ReadQuaternion(TTarget target) => _getter(target);
        protected override void WriteQuaternion(TTarget target, Quaternion value) => _setter(target, value);
    }

    /// <summary>
    /// Provides support for quaternion tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class QuaternionMemberProvider<TTarget> : QuaternionProvider<TTarget> where TTarget : class
    {
        private FastGetter<TTarget, Quaternion> _getter;
        private FastSetter<TTarget, Quaternion> _setter;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static QuaternionMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, QuaternionMemberProvider<TTarget>>.Get(memberName);

        private QuaternionMemberProvider(string memberName)
        {
            _getter = new FastGetter<TTarget, Quaternion>(memberName);
            _setter = new FastSetter<TTarget, Quaternion>(memberName);
        }

        protected sealed override Quaternion ReadQuaternion(TTarget target) => _getter.GetValue(target);
        protected sealed override void WriteQuaternion(TTarget target, Quaternion value) => _setter.SetValue(target, value);
    }
}

