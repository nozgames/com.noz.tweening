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
    public enum Vector4Options : uint
    {
        None = 0,
        IgnoreX = 1 << 0,
        IgnoreY = 1 << 1,
        IgnoreZ = 1 << 2,
        IgnoreW = 1 << 3, 
        IgnoreXY = IgnoreX | IgnoreY,
        IgnoreXZ = IgnoreX | IgnoreZ,
        IgnoreXW = IgnoreX | IgnoreW,
        IgnoreYZ = IgnoreY | IgnoreZ,
        IgnoreYW = IgnoreY | IgnoreW,
        IgnoreZW = IgnoreZ | IgnoreW,
        IgnoreXYZ = IgnoreX | IgnoreY | IgnoreZ,
        IgnoreXYW = IgnoreX | IgnoreY | IgnoreW,
        IgnoreXZW = IgnoreX | IgnoreZ | IgnoreW,
        IgnoreYZW = IgnoreY | IgnoreZ | IgnoreW,
    }

    public abstract class Vector4Provider<TTarget> : TweenProvider<TTarget> where TTarget : class
    {
        protected internal sealed override Variant Evalulate(Variant from, Variant to, float t, uint options) => Evalulate(from.v4, to.v4, t, (Vector4Options)options);
        protected internal sealed override Variant GetValue(TTarget target, uint optionsAsUint) => GetValue(target);
        protected internal sealed override void SetValue(TTarget target, Variant v, uint optionsAsUint)
        {
            var options = (Vector4Options)optionsAsUint;
            if (options != 0)
            {
                var old = GetValue(target);
                if ((options & Vector4Options.IgnoreX) == Vector4Options.IgnoreX) v.v4.x = old.x;
                if ((options & Vector4Options.IgnoreY) == Vector4Options.IgnoreY) v.v4.y = old.y;
                if ((options & Vector4Options.IgnoreZ) == Vector4Options.IgnoreZ) v.v4.z = old.z;
                if ((options & Vector4Options.IgnoreW) == Vector4Options.IgnoreW) v.v4.w = old.w;
            }
            SetValue(target, v);
        }

        protected virtual Vector4 Evalulate(Vector4 from, Vector4 to, float normalizedTime, Vector4Options options) =>
            Vector4.LerpUnclamped(from, to, normalizedTime);

        protected abstract Vector4 GetValue (TTarget target);
        protected abstract void SetValue (TTarget target, Vector4 value);
    }

    /// <summary>
    /// Provides support for vector point tweens using a Property or Field.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class Vector4MemberProvider<TTarget> : Vector4Provider<TTarget> where TTarget : class
    {
        private FastMember<TTarget, Vector4> _member;

        /// <summary>
        /// Returns a cached member provider for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static Vector4MemberProvider<TTarget> Get(string memberName) =>
            ProviderCache<string, Vector4MemberProvider<TTarget>>.Get(memberName);

        private Vector4MemberProvider(string memberName) => _member = new FastMember<TTarget, Vector4>(memberName);

        protected sealed override Vector4 GetValue (TTarget target) => _member.GetValue(target);
        protected sealed override void SetValue (TTarget target, Vector4 value) => _member.SetValue(target, value);
    }
}
