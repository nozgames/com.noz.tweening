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
    public enum UIntOptions : uint
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
    public abstract class UIntProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint options) => Evalulate(from.ui32, to.ui32, t, (UIntOptions)options);
        protected internal sealed override Variant GetValue(TTarget target, uint options) => GetValue(target);
        protected internal sealed override void SetValue(TTarget target, Variant v, uint options) => SetValue(target, v);

        protected virtual uint Evalulate(uint from, uint to, float normalizedTime, UIntOptions options)
        {
            var value = to > from ?
                from + (to - from) * normalizedTime :
                to + (from - to) * (1.0f - normalizedTime);

            if ((options & UIntOptions.RoundUp) == UIntOptions.RoundUp)
                return (uint)Mathf.Ceil(value);

            return (uint)value;
        }

        protected abstract uint GetValue (TTarget target);
        protected abstract void SetValue (TTarget target, uint value);
    }

    /// <summary>
    /// Provides support for floating point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class UIntMemberProvider<TTarget> : UIntProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, uint> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns>Member provider for the given <paramref name="memberName"/></returns>
        public static UIntMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, UIntMemberProvider<TTarget>>.Get(memberName);

        private UIntMemberProvider(string memberName) => _member = new FastMember<TTarget, uint>(memberName);

        protected sealed override uint GetValue (TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue (TTarget target, uint value) => _member.SetValue(target, value);
    }
}
