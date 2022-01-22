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
    public enum Vector3Options : uint
    {
        None = 0,
        IgnoreX = 1 << 0,
        IgnoreY = 1 << 1,
        IgnoreZ = 1 << 2,
        IgnoreXY = IgnoreX | IgnoreY,
        IgnoreXZ = IgnoreX | IgnoreZ,
        IgnoreYZ = IgnoreY | IgnoreZ
    }

    public abstract class Vector3Provider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint options) => Evalulate(from.v3, to.v3, t, (Vector3Options)options);
        protected internal sealed override Variant GetValue(TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue(TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (Vector3Options)optionsAsUint;
            if (options != 0)
            {
                var old = GetValue(target);
                if ((options & Vector3Options.IgnoreX) == Vector3Options.IgnoreX) v.v3.x = old.x;
                if ((options & Vector3Options.IgnoreY) == Vector3Options.IgnoreY) v.v3.y = old.y;
                if ((options & Vector3Options.IgnoreZ) == Vector3Options.IgnoreZ) v.v3.z = old.z;
            }
            SetValue(target, v);
        }

        protected virtual Vector3 Evalulate(Vector3 from, Vector3 to, float normalizedTime, Vector3Options options) =>
            Vector3.LerpUnclamped(from, to, normalizedTime);

        protected abstract Vector3 GetValue (TTarget target);
        protected abstract void SetValue (TTarget target, Vector3 value);
    }

    /// <summary>
    /// Provides support for vector point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class Vector3MemberProvider<TTarget> : Vector3Provider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, Vector3> _member;
        
        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static Vector3MemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, Vector3MemberProvider<TTarget>>.Get(memberName);

        private Vector3MemberProvider(string memberName) => _member = new FastMember<TTarget, Vector3>(memberName);

        protected sealed override Vector3 GetValue(TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue(TTarget target, Vector3 value) => _member.SetValue(target, value);
    }
}
