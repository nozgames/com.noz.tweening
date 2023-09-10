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

using NoZ.Tweening.Internals;

namespace NoZ.Tweening
{
    /// <summary>
    /// Abstract class that provides support for floating point tweens.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public abstract class FloatProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate (Variant from, Variant to, float t, uint optionsAsUint) => Evaluate(from.f, to.f, t);
        protected internal sealed override Variant GetValue (TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue (TTarget target, Variant v, uint optionsAsUint) => SetValue(target, v);

        protected virtual float Evaluate (float from, float to, float normalizedTime) => from + (to - from) * normalizedTime;
        protected abstract float GetValue (TTarget target);
        protected abstract void SetValue (TTarget target, float value);
    }

    /// <summary>
    /// Provides support for floating point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class FloatMemberProvider<TTarget> : FloatProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, float> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static FloatMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, FloatMemberProvider<TTarget>>.Get(memberName);

        private FloatMemberProvider(string memberName) => _member = new FastMember<TTarget, float>(memberName);

        protected sealed override float GetValue (TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue (TTarget target, float value) => _member.SetValue(target, value);

        public float Get(TTarget target) => GetValue(target);
    }
}
