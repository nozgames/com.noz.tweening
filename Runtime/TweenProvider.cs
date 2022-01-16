
namespace NoZ.Tweenz
{
    /// <summary>
    /// Defines an abstract class used to read and write tween data.
    /// </summary>
    public abstract class TweenProvider
    {
        public abstract Variant Evalulate(Variant from, Variant to, float t, uint options);
        public abstract Variant Add(Variant a, Variant b, uint options);
        public abstract Variant Read(object target, uint options);
        public abstract void Write(object target, Variant v, uint options);
    }

    public abstract class TweenProvider<TTarget> : TweenProvider where TTarget : class
    {
        public sealed override Variant Read(object target, uint options) => Read(target as TTarget, options);
        public sealed override void Write(object target, Variant v, uint options) => Write(target as TTarget, v, options);
        public abstract Variant Read(TTarget target, uint options);
        public abstract void Write(TTarget target, Variant v, uint options);
    }
}
