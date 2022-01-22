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

using System;
using UnityEngine;
using NoZ.Tweening.Internals;

namespace NoZ.Tweening
{
    [Flags]
    public enum ColorOptions : uint
    {
        None = 0,
        IgnoreR = 1 << 0,
        IgnoreG = 1 << 1,
        IgnoreB = 1 << 2,
        IgnoreA = 1 << 3,
        IgnoreRGB = IgnoreR | IgnoreG | IgnoreB
    }

    public abstract class ColorProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint options) => Evalulate(from.c, to.c, t, (ColorOptions)options);
        protected internal sealed override Variant GetValue (TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue (TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (ColorOptions)optionsAsUint;
            if (options != 0)
            {
                var old = GetValue(target);
                if ((options & ColorOptions.IgnoreR) == ColorOptions.IgnoreR) v.c.r = old.r;
                if ((options & ColorOptions.IgnoreG) == ColorOptions.IgnoreG) v.c.g = old.g;
                if ((options & ColorOptions.IgnoreB) == ColorOptions.IgnoreB) v.c.b = old.b;
                if ((options & ColorOptions.IgnoreA) == ColorOptions.IgnoreA) v.c.a = old.a;
            }
            SetValue(target, v);
        }

        protected virtual Color Evalulate (Color from, Color to, float normalizedTime, ColorOptions options) =>
            from + (to - from) * normalizedTime;

        protected abstract Color GetValue (TTarget target);
        protected abstract void SetValue (TTarget target, Color value);
    }

    public class ColorMemberProvider<TTarget> : ColorProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, Color> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns>Cached color provider</returns>
        public static ColorMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, ColorMemberProvider<TTarget>>.Get(memberName);

        private ColorMemberProvider(string memberName) => _member = new FastMember<TTarget, Color>(memberName);

        protected override Color GetValue (TTarget target) => _member.GetValue(target);
        protected override void SetValue (TTarget target, Color value) => _member.SetValue(target, value);
    }
}
