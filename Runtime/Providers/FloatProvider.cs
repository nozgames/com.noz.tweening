using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    /// <summary>
    /// Abstract class that provides support for floating point tweens.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public abstract class FloatProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        public override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => from.f + (to.f - from.f) * t;
        public override Variant Add(Variant a, Variant b, uint optionsAsUint) => a.f + b.f;
        public override Variant Read(TTarget target, uint optionsAsUint) => ReadFloat(target);
        public override void Write(TTarget target, Variant v, uint optionsAsUint) => WriteFloat(target, v);

        protected abstract float ReadFloat (TTarget target);
        protected abstract void WriteFloat (TTarget target, float value);
    }

    /// <summary>
    /// Provides support for floating point tweens using open callbacks.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class FloatCallbackProvider<TTarget> : FloatProvider<TTarget> where TTarget : class
    {
        public delegate float GetDelegate(TTarget target);
        public delegate void SetDelegate(TTarget target, float value);

        private GetDelegate _getter;
        private SetDelegate _setter;

        /// <summary>
        /// Construct a floating point tween provider using a getter and setting callback.
        /// </summary>
        /// <param name="getter">Delegate used to get values from the target</param>
        /// <param name="setter">Delegate used to set values from the target</param>
        public FloatCallbackProvider (GetDelegate getter, SetDelegate setter)
        {
            _getter = getter;
            _setter = setter;
        }

        protected override float ReadFloat(TTarget target) => _getter(target);
        protected override void WriteFloat(TTarget target, float value) => _setter(target, value);
    }

    /// <summary>
    /// Provides support for floating point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class FloatMemberProvider<TTarget> : FloatProvider<TTarget> where TTarget : class
    {
        private FastGetter<TTarget, float> _getter;
        private FastSetter<TTarget, float> _setter;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static FloatMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, FloatMemberProvider<TTarget>>.Get(memberName);

        private FloatMemberProvider(string memberName)
        {
            _getter = new FastGetter<TTarget, float>(memberName);
            _setter = new FastSetter<TTarget, float>(memberName);
        }

        protected sealed override float ReadFloat(TTarget target) => _getter.GetValue(target);
        protected sealed override void WriteFloat(TTarget target, float value) => _setter.SetValue(target, value);
    }
}
