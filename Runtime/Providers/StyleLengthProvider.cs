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
using UnityEngine.UIElements;

namespace NoZ.Tweening
{
    public abstract class StyleLengthProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => Evaluate(from.styleLength, to.styleLength, t);
        protected internal sealed override Variant GetValue(TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue(TTarget target, Variant v, uint optionsAsUint) => SetValue(target, v);

        protected virtual StyleLength Evaluate(StyleLength from, StyleLength to, float normalizedTime) =>
            new StyleLength(new Length(from.value.value + (to.value.value - from.value.value) * normalizedTime, to.value.unit));
        
        protected abstract StyleLength GetValue(TTarget target);
        protected abstract void SetValue(TTarget target, StyleLength value);
    }

    /// <summary>
    /// Provides support for UIElement StyleLength parameters
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class StyleLengthMemberProvider<TTarget> : StyleLengthProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, StyleLength> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static StyleLengthMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, StyleLengthMemberProvider<TTarget>>.Get(memberName);

        private StyleLengthMemberProvider(string memberName) => _member = new FastMember<TTarget, StyleLength>(memberName);

        protected sealed override StyleLength GetValue(TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue(TTarget target, StyleLength value) => _member.SetValue(target, value);

        public void Set(TTarget target, StyleLength value) => SetValue(target, value);
        public StyleLength Get(TTarget target) => GetValue(target);
    }
}
