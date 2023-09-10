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
    public abstract class StyleColorProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint optionsAsUint) => Evaluate(from.styleColor, to.styleColor, t);
        protected internal sealed override Variant GetValue(TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue(TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (ColorOptions)optionsAsUint;
            if (options != 0)
            {
                var old = GetValue(target);
                if ((options & ColorOptions.IgnoreR) == ColorOptions.IgnoreR) v.c.r = old.value.r;
                if ((options & ColorOptions.IgnoreG) == ColorOptions.IgnoreG) v.c.g = old.value.g;
                if ((options & ColorOptions.IgnoreB) == ColorOptions.IgnoreB) v.c.b = old.value.b;
                if ((options & ColorOptions.IgnoreA) == ColorOptions.IgnoreA) v.c.a = old.value.a;
            }
            SetValue(target, v);
        }

        protected virtual StyleColor Evaluate(StyleColor from, StyleColor to, float normalizedTime) =>
            new StyleColor(from.value + (to.value - from.value) * normalizedTime);
        
        protected abstract StyleColor GetValue(TTarget target);
        protected abstract void SetValue(TTarget target, StyleColor value);
    }

    /// <summary>
    /// Provides support for UIElement StyleColor parameters
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public class StyleColorMemberProvider<TTarget> : StyleColorProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, StyleColor> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static StyleColorMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, StyleColorMemberProvider<TTarget>>.Get(memberName);

        private StyleColorMemberProvider(string memberName) => _member = new FastMember<TTarget, StyleColor>(memberName);

        protected sealed override StyleColor GetValue(TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue(TTarget target, StyleColor value) => _member.SetValue(target, value);
    }
}
