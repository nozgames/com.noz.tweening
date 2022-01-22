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
    public abstract class DoubleProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => Evalulate(from.d, to.d, t);
        protected internal sealed override Variant GetValue(TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue(TTarget target, Variant v, uint optionsAsUint) => SetValue(target, v);

        protected virtual double Evalulate(double from, double to, float normalizedTime) => from + (to - from) * normalizedTime;
        protected abstract double GetValue(TTarget target);
        protected abstract void SetValue(TTarget target, double value);
    }

    /// <summary>
    /// Provides support for floating point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class DoubleMemberProvider<TTarget> : DoubleProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, double> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static DoubleMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, DoubleMemberProvider<TTarget>>.Get(memberName);

        private DoubleMemberProvider(string memberName) => _member = new FastMember<TTarget, double>(memberName);

        protected sealed override double GetValue (TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue (TTarget target, double value) => _member.SetValue(target, value);
    }
}
