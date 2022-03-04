/*
  NoZ Unity Library

  Copyright(c) 2022 NoZ Games, LLC

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files(the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions :

  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*/

using UnityEngine;
using NoZ.Tweening.Internals;

namespace NoZ.Tweening
{
    internal static class MaterialPropertyBlockProviderUtils
    {
        public static TProvider GetProvider<TProvider>(MaterialPropertyBlock target, int propertyId) where TProvider : TweenProvider
        {
            if (!target.HasProperty(propertyId))
                throw new System.InvalidOperationException("Specified material proerty does not exist");

            return ProviderCache<int, TProvider>.Get(propertyId);
        }
    }

    internal class MaterialPropertyBlockFloatProvider : FloatProvider<MaterialPropertyBlock>
    {
        private int _propertyId;
        protected sealed override float GetValue(MaterialPropertyBlock target) => target.GetFloat(_propertyId);
        protected sealed override void SetValue(MaterialPropertyBlock target, float value) => target.SetFloat(_propertyId, value);
        private MaterialPropertyBlockFloatProvider(int propertyId) => _propertyId = propertyId;
    }

    internal class MaterialPropertyBlockColorProvider : ColorProvider<MaterialPropertyBlock>
    {
        private int _propertyId;
        protected sealed override Color GetValue(MaterialPropertyBlock target) => target.GetColor(_propertyId);
        protected sealed override void SetValue(MaterialPropertyBlock target, Color value) => target.SetColor(_propertyId, value);
        private MaterialPropertyBlockColorProvider(int propertyId) => _propertyId = propertyId;
    }

    internal class MaterialPropertyBlockVectorProvider : Vector4Provider<MaterialPropertyBlock>
    {
        private int _propertyId;
        protected sealed override Vector4 GetValue(MaterialPropertyBlock target) => target.GetVector(_propertyId);
        protected sealed override void SetValue(MaterialPropertyBlock target, Vector4 value) => target.SetVector(_propertyId, value);
        private MaterialPropertyBlockVectorProvider(int propertyId) => _propertyId = propertyId;
    }
}

