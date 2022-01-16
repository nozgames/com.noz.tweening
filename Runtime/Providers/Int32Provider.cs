using NoZ.Tweenz.Internals;
using System;

namespace NoZ.Tweenz
{
    [Flags]
    public enum Int32Options : uint
    {
        /// <summary>
        /// No options specified
        /// </summary>
        None = 0,

        /// <summary>
        /// Round the value up to the nearest integer rather than down
        /// </summary>
        RoundUp = 1 << 0,
    }

    /// <summary>
    /// Abstract class that provides support for floating point tweens.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public abstract class Int32Provider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        public override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint)
        {
            var options = (Int32Options)optionsAsUint;
            var value = from.i + (to.i - from.i) * t;
            if ((options & Int32Options.RoundUp) != 0)
                return (int)MathF.Ceiling(value);

            return (int)value;
        }
        public override Variant Add(Variant a, Variant b, uint options) => a.i + b.i;
        public override Variant Read(TTarget target, uint options) => ReadInt32(target);
        public override void Write(TTarget target, Variant v, uint options)
        {
            WriteInt32(target, v);
        }

        protected abstract int ReadInt32 (TTarget target);
        protected abstract void WriteInt32 (TTarget target, int value);
    }

    /// <summary>
    /// Provides support for floating point tweens using open callbacks.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class Int32CallbackProvider<TTarget> : Int32Provider<TTarget> where TTarget : class
    {
        public delegate int GetDelegate(TTarget target);
        public delegate void SetDelegate(TTarget target, int value);

        private GetDelegate _getter;
        private SetDelegate _setter;

        /// <summary>
        /// Construct a floating point tween provider using a getter and setting callback.
        /// </summary>
        /// <param name="getter">Delegate used to get values from the target</param>
        /// <param name="setter">Delegate used to set values from the target</param>
        public Int32CallbackProvider (GetDelegate getter, SetDelegate setter)
        {
            _getter = getter;
            _setter = setter;
        }

        protected override int ReadInt32(TTarget target) => _getter(target);
        protected override void WriteInt32(TTarget target, int value) => _setter(target, value);
    }

    /// <summary>
    /// Provides support for floating point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class Int32MemberProvider<TTarget> : Int32Provider<TTarget> where TTarget : class
    {
        private FastGetter<TTarget, int> _getter;
        private FastSetter<TTarget, int> _setter;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static Int32MemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, Int32MemberProvider<TTarget>>.Get(memberName);

        private Int32MemberProvider(string memberName)
        {
            _getter = new FastGetter<TTarget, int>(memberName);
            _setter = new FastSetter<TTarget, int>(memberName);
        }

        protected sealed override int ReadInt32(TTarget target) => _getter.GetValue(target);
        protected sealed override void WriteInt32(TTarget target, int value) => _setter.SetValue(target, value);
    }
}
