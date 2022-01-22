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

namespace NoZ.Tweening
{
    /// <summary>
    /// Defines an abstract class used to read and write tween data.
    /// </summary>
    public abstract class TweenProvider
    {
        /// <summary>
        /// Evaluates 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="normalizedTime"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal abstract Variant Evalulate (Variant from, Variant to, float normalizedTime, uint options);
        protected internal abstract Variant GetValue (object target, uint options);
        protected internal abstract void SetValue (object target, Variant v, uint options);
    }

    public abstract class TweenProvider<TTarget> : TweenProvider where TTarget : class
    {
        protected internal sealed override Variant GetValue (object target, uint options) => GetValue(target as TTarget, options);
        protected internal sealed override void SetValue (object target, Variant v, uint options) => SetValue(target as TTarget, v, options);
        protected internal abstract Variant GetValue (TTarget target, uint options);
        protected internal abstract void SetValue (TTarget target, Variant v, uint options);
    }
}
