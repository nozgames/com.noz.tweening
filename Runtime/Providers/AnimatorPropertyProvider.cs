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
    internal class AnimatorFloatProvider : FloatProvider<Animator>
    {
        private int _propertyId;
        protected sealed override float GetValue(Animator target) => target.GetFloat(_propertyId);
        protected sealed override void SetValue(Animator target, float value) => target.SetFloat(_propertyId, value);
        public static AnimatorFloatProvider Get(int propertyId) => ProviderCache<int, AnimatorFloatProvider>.Get(propertyId);
        public static AnimatorFloatProvider Get(string propertyName) => Get(Animator.StringToHash(propertyName));
        private AnimatorFloatProvider(int propertyId) => _propertyId = propertyId;
    }

    internal class AnimatorIntProvider : IntProvider<Animator>
    {
        private int _propertyId;
        protected sealed override int GetValue(Animator target) => target.GetInteger(_propertyId);
        protected sealed override void SetValue(Animator target, int value) => target.SetInteger(_propertyId, value);
        public static AnimatorIntProvider Get(int propertyId) => ProviderCache<int, AnimatorIntProvider>.Get(propertyId);
        public static AnimatorIntProvider Get(string propertyName) => Get(Animator.StringToHash(propertyName));
        private AnimatorIntProvider(int propertyId) => _propertyId = propertyId;
    }
}

