using UnityEngine;
using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    internal class AnimatorFloatProvider : FloatProvider<Animator>
    {
        private int _propertyId;
        protected sealed override float ReadFloat(Animator target) => target.GetFloat(_propertyId);
        protected sealed override void WriteFloat(Animator target, float value) => target.SetFloat(_propertyId, value);
        public static AnimatorFloatProvider Get(int propertyId) => ProviderCache<int, AnimatorFloatProvider>.Get(propertyId);
        public static AnimatorFloatProvider Get(string propertyName) => Get(Animator.StringToHash(propertyName));
        private AnimatorFloatProvider(int propertyId) => _propertyId = propertyId;
    }

    internal class AnimatorInt32Provider : Int32Provider<Animator>
    {
        private int _propertyId;
        protected sealed override int ReadInt32(Animator target) => target.GetInteger(_propertyId);
        protected sealed override void WriteInt32(Animator target, int value) => target.SetInteger(_propertyId, value);
        public static AnimatorInt32Provider Get(int propertyId) => ProviderCache<int, AnimatorInt32Provider>.Get(propertyId);
        public static AnimatorInt32Provider Get(string propertyName) => Get(Animator.StringToHash(propertyName));
        private AnimatorInt32Provider(int propertyId) => _propertyId = propertyId;
    }
}

