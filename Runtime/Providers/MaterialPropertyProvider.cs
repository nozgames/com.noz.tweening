using UnityEngine;
using NoZ.Tweenz.Internals;

namespace NoZ.Tweenz
{
    internal class MaterialFloatProvider : FloatProvider<Material>
    {
        private int _propertyId;
        protected sealed override float ReadFloat(Material target) => target.GetFloat(_propertyId);
        protected sealed override void WriteFloat(Material target, float value) => target.SetFloat(_propertyId, value);
        public static MaterialFloatProvider Get(Material target, int propertyId)
        {
            if (!target.HasProperty(propertyId))
                throw new System.InvalidOperationException("Specified material proerty does not exist");

            return ProviderCache<int, MaterialFloatProvider>.Get(propertyId);
        }
        private MaterialFloatProvider(int propertyId) => _propertyId = propertyId;
    }

    internal class MaterialColorProvider : ColorProvider<Material>
    {
        private int _propertyId;
        protected sealed override Color ReadColor(Material target) => target.GetColor(_propertyId);
        protected sealed override void WriteColor(Material target, Color value) => target.SetColor(_propertyId, value);
        public static MaterialColorProvider Get(Material target, int propertyId)
        {
            if (!target.HasProperty(propertyId))
                throw new System.InvalidOperationException("Specified material proerty does not exist");

            return ProviderCache<int, MaterialColorProvider>.Get(propertyId);
        }
        private MaterialColorProvider(int propertyId) => _propertyId = propertyId;
    }

    internal class MaterialVectorProvider : Vector4Provider<Material>
    {
        private int _propertyId;
        protected sealed override Vector4 ReadVector(Material target) => target.GetVector(_propertyId);
        protected sealed override void WriteVector(Material target, Vector4 value) => target.SetVector(_propertyId, value);
        public static MaterialVectorProvider Get(Material target, int propertyId)
        {
            if (!target.HasProperty(propertyId))
                throw new System.InvalidOperationException("Specified material proerty does not exist");

            return ProviderCache<int, MaterialVectorProvider>.Get(propertyId);
        }
        private MaterialVectorProvider(int propertyId) => _propertyId = propertyId;
    }
}

