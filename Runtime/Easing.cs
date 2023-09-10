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

namespace NoZ.Tweening
{
    /// <summary>
    /// Delegate used to calculate easing for a normalized time of 0-1
    /// </summary>
    /// <param name="normalizedTime">Time value in the range of 0 to 1</param>
    /// <param name="param">Optional parameters</param>
    /// <returns>Eased time value</returns>
    public delegate float EaseDelegate(float normalizedTime, Vector4 param);

    public static class Easing
    {
        public static float EaseQuadratic(float t, Vector4 p) => t * t;

        public static float EaseCubic(float t, Vector4 p) => t * t * t;

        public static float EaseBack(float t, Vector4 p) => Mathf.Pow(t, 3f) - t * Mathf.Max(0f, p.x) * Mathf.Sin(Mathf.PI * t);

        public static float EaseBounce(float t, Vector4 p)
        {
            var Bounces = p.x;
            var Bounciness = p.y;

            var pow = Mathf.Pow(Bounciness, Bounces);
            var invBounciness = 1f - Bounciness;

            var sum_units = (1f - pow) / invBounciness + pow * 0.5f;
            var unit_at_t = t * sum_units;

            var bounce_at_t = Mathf.Log(-unit_at_t * invBounciness + 1f, Bounciness);
            var start = Mathf.Floor(bounce_at_t);
            var end = start + 1f;

            var div = 1f / (invBounciness * sum_units);
            var start_time = (1f - Mathf.Pow(Bounciness, start)) * div;
            var end_time = (1f - Mathf.Pow(Bounciness, end)) * div;

            var mid_time = (start_time + end_time) * 0.5f;
            var peak_time = t - mid_time;
            var radius = mid_time - start_time;
            var amplitude = Mathf.Pow(1f / Bounciness, Bounces - start);

            return (-amplitude / (radius * radius)) * (peak_time - radius) * (peak_time + radius);
        }

        public static float EaseElastic(float t, Vector4 p)
        {
            var oscillations = Mathf.Max(0, (int)p.x);
            var springiness = Mathf.Max(0f, p.y);

            float expo;
            if (springiness == 0f)
                expo = t;
            else
                expo = (Mathf.Exp(springiness * t) - 1f) / (Mathf.Exp(springiness) - 1f);

            return expo * (Mathf.Sin((Mathf.PI * 2f * oscillations + Mathf.PI * 0.5f) * t));
        }

        public static float EaseSine(float t, Vector4 p) =>
            1.0f - Mathf.Sin(Mathf.PI * 0.5f * (1f - t));

        public static float EaseCircle(float t, Vector4 p) =>
            1.0f - Mathf.Sqrt(1.0f - t * t);

        public static float EaseExponential(float t, Vector4 p) =>
            p.x == 0.0f ? t : ((Mathf.Exp(p.x * t) - 1.0f) / (Mathf.Exp(p.x) - 1.0f));

        public static float EaseCubicBezier(float t, Vector4 p)
        {
            var oneMinusT = 1.0f - t;
            var oneMinusT_2 = oneMinusT * oneMinusT;
            var oneMinusT_3 = oneMinusT_2 * oneMinusT;
            var t_2 = t * t;
            var t_3 = t_2 * t;

            var p0 = Vector2.zero;
            var p1 = new Vector2(p.x, p.y);
            var p2 = new Vector2(p.z, p.w);
            var p3 = Vector2.one;

            var r =
                p0 * oneMinusT_3 +
                3.0f * p1 * t * oneMinusT_2 +
                3.0f * p2 * t_2 * oneMinusT +
                p3 * t_3;

            return r.y;
        }
    }
}
