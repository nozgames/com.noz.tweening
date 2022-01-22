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
    public enum IntOptions : uint
    {
        /// <summary>No options specified</summary>
        None = 0,

        /// <summary>Round the value up to the nearest integer rather than down</summary>
        RoundUp = 1 << 0,
    }

    /// <summary>
    /// Abstract class that provides support for 32-bit integer tweens
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    public abstract class IntProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint options) => Evalulate(from.i32, to.i32, t, (IntOptions)options);
        protected internal sealed override Variant GetValue(TTarget target, uint options) => GetValue(target);
        protected internal sealed override void SetValue(TTarget target, Variant v, uint options) => SetValue(target, v);

        protected virtual int Evalulate (int from, int to, float normalizedTime, IntOptions options)
        {
            var value = from + (to - from) * normalizedTime;
            if ((options & IntOptions.RoundUp) == IntOptions.RoundUp)
                return Mathf.CeilToInt(value);

            return (int)value;
        }

        protected abstract int GetValue (TTarget target);
        protected abstract void SetValue (TTarget target, int value);
    }

    /// <summary>
    /// Provides support for floating point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class IntMemberProvider<TTarget> : IntProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, int> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns>Member provider for the given <paramref name="memberName"/></returns>
        public static IntMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, IntMemberProvider<TTarget>>.Get(memberName);

        private IntMemberProvider(string memberName) => _member = new FastMember<TTarget, int>(memberName);

        protected sealed override int GetValue (TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue (TTarget target, int value) => _member.SetValue(target, value);
    }
}
