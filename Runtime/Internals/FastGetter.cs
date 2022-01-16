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
    internal struct FastGetter<T, V> where T : class
    {
        private delegate V GetValueDelegate(T target);

        private GetValueDelegate _getValue;

        public FastGetter(string memberName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
        {
            // Property
            var propertyInfo = typeof(T).GetProperty(memberName, bindingFlags);
            if (null != propertyInfo)
            {
                _getValue = (GetValueDelegate)propertyInfo.GetMethod.CreateDelegate(typeof(GetValueDelegate));
                return;
            }

            // Field
            var fieldInfo = typeof(T).GetField(memberName, bindingFlags);
            if (null != fieldInfo)
            {
                if (fieldInfo == null)
                    throw new ArgumentNullException("fieldInfo");

                var source = Expression.Parameter(typeof(T), "source");
                _getValue = (GetValueDelegate)Expression.Lambda(
                    typeof(GetValueDelegate),
                    Expression.Field(source, fieldInfo),
                    source
                ).Compile();
            }

            throw new InvalidOperationException($"The type `{typeof(T).Name}` does not contain a property or field named `{memberName}` that matches the binding flags.");
        }

        /// <summary>
        /// Return the member value for the given target
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns>Member value</returns>
        public V GetValue(T target) => _getValue(target);
    }
}
