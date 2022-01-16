/*
  NoZ Unity Library

  Copyright(c) 2019 NoZ Games, LLC

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
using System.Linq.Expressions;
using System.Reflection;

namespace NoZ.Tweenz.Internals
{
    internal struct FastSetter<T, V> where T : class
    {
        private delegate void SetValueDelegate(T target, V value);

        private SetValueDelegate _setValue;

        public FastSetter(string memberName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            // Property?
            var propertyInfo = typeof(T).GetProperty(memberName, bindingFlags);
            if (null != propertyInfo)
            {
                _setValue = (SetValueDelegate)propertyInfo.SetMethod.CreateDelegate(typeof(SetValueDelegate));
                return;
            }

            // Field?
            var fieldInfo = typeof(T).GetField(memberName, bindingFlags);
            if (null != fieldInfo)
            {
                var source = Expression.Parameter(typeof(T), "source");
                var value = Expression.Parameter(typeof(V), "value");

                _setValue = (SetValueDelegate)Expression.Lambda(
                    typeof(SetValueDelegate),
                    Expression.Assign(Expression.Field(source, fieldInfo), value),
                    source,
                    value
                ).Compile();

                return;
            }

            throw new InvalidOperationException($"The type `{typeof(T).Name}` does not contain a property or field named `{memberName}` that matches the binding flags.");
        }

        /// <summary>
        /// Set the value of the member.  This is the fastest call but requires the 
        /// target type to be known at call time.
        /// </summary>
        /// <param name="target">Target to set the value on</param>
        /// <param name="value">Value to set</param>
        public void SetValue(T target, V value) => _setValue(target, value);
    }
}
