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
    public enum QuaternionOptions
    {
        None = 0,
    }

    /// <summary>
    /// Abstract support for tweening a quaternion.
    /// </summary>
    /// <typeparam name="TTarget">Target object type</typeparam>
    public abstract class QuaternionProvider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate (Variant from, Variant to, float t, uint options) => Evalulate(from.q, to.q, t, (QuaternionOptions)options);
        protected internal sealed override Variant GetValue (TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue (TTarget target, Variant v, uint optionsAsUint) => SetValue(target, v);

        /// <summary>
        /// Evaluate an interpolated value
        /// </summary>
        /// <param name="from">Starting value</param>
        /// <param name="to">Ending value</param>
        /// <param name="normalizedTime">Time from 0-1</param>
        /// <returns>Interpolated value</returns>
        protected virtual Quaternion Evalulate(Quaternion from, Quaternion to, float normalizedTime, QuaternionOptions options) =>
            Quaternion.SlerpUnclamped(from, to, normalizedTime);

        /// <summary>
        /// Method should be implemented to return the current value for the given <paramref name="target"/>
        /// </summary>
        /// <param name="target">Target Object</param>
        /// <returns>Current value</returns>
        protected abstract Quaternion GetValue (TTarget target);

        /// <summary>
        /// Method should be implemented to set the current value for the given <paramref name="target"/>
        /// </summary>
        /// <param name="target">Target Object</param>
        /// <param name="value">New value</param>
        protected abstract void SetValue (TTarget target, Quaternion value);
    }

    /// <summary>
    /// Provides support for quaternion tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class QuaternionMemberProvider<TTarget> : QuaternionProvider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, Quaternion> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static QuaternionMemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, QuaternionMemberProvider<TTarget>>.Get(memberName);

        private QuaternionMemberProvider(string memberName) => _member = new FastMember<TTarget, Quaternion>(memberName);

        protected sealed override Quaternion GetValue (TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue (TTarget target, Quaternion value) => _member.SetValue(target, value);
    }
}

