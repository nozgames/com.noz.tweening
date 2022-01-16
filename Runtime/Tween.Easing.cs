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

namespace NoZ.Tweenz
{
    /// <summary>
    /// Delegate used to calculate easing for a normalized time of 0-1
    /// </summary>
    /// <param name="normalizedTime">Time value in the range of 0 to 1</param>
    /// <param name="param1">Optional parameter</param>
    /// <param name="param2">Optional parameter</param>
    /// <returns>Eased time value</returns>
    public delegate float EaseDelegate(float normalizedTime, float param1, float param2);

    public partial struct Tween
    {
        private static float EaseCubic(float t, float p1, float p2) => t * t * t;

        private static float EaseBack(float t, float p1, float p2) => Mathf.Pow(t, 3f) - t * Mathf.Max(0f, p1) * Mathf.Sin(Mathf.PI * t);

        private static float EaseBounce(float t, float p1, float p2)
        {
            var Bounces = p1;
            var Bounciness = p2;

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

        private static float EaseElastic(float t, float oscillations, float springiness)
        {
            oscillations = Mathf.Max(0, (int)oscillations);
            springiness = Mathf.Max(0f, springiness);

            float expo;
            if (springiness == 0f)
                expo = t;
            else
                expo = (Mathf.Exp(springiness * t) - 1f) / (Mathf.Exp(springiness) - 1f);

            return expo * (Mathf.Sin((Mathf.PI * 2f * oscillations + Mathf.PI * 0.5f) * t));
        }

        private static float EaseSine (float t, float param1, float param2) =>
            1.0f - Mathf.Sin(Mathf.PI * 0.5f * (1f - t));

        private static float EaseCircle (float t, float p1, float p2) =>
            1.0f - Mathf.Sqrt(1.0f - t * t);

        private static float EaseExponential (float t, float exponent, float p2) =>
            exponent == 0.0f ? t : ((Mathf.Exp(exponent * t) - 1.0f) / Mathf.Exp(exponent) - 1.0f);

        /// <summary>
        /// Set a function to use for easing in
        /// </summary>
        /// <param name="easeDelegate">Delegate to use for easing int</param>
        /// <param name="param1">Optional easing paramter</param>
        /// <param name="param2">Optional easing paramter</param>
        public Tween EaseIn(EaseDelegate easeDelegate, float param1 = 0.0f, float param2 = 0.0f)
        {
            ValidateModifierContext();

            _context.easeIn = easeDelegate;
            _context.easeInParams = new Vector2(param1, param2);
            return this;
        }

        /// <summary>
        /// Set a function to use for easing out 
        /// </summary>
        /// <param name="easeDelegate">Delegate to use for easing out</param>
        /// <param name="param1">Optional easing paramter</param>
        /// <param name="param2">Optional easing paramter</param>
        public Tween EaseOut(EaseDelegate easeDelegate, float param1 = 0.0f, float param2 = 0.0f)
        {
            ValidateModifierContext();

            _context.easeOut = easeDelegate;
            _context.easeOutParams = new Vector2(param1, param2);
            return this;
        }

        private static EaseDelegate _easeCubicDelegate = EaseCubic;
        private static EaseDelegate _easeBackDelegate = EaseBack;
        private static EaseDelegate _easeElasticDelegate = EaseElastic;
        private static EaseDelegate _easeBounceDelegate = EaseBounce;
        private static EaseDelegate _easeSineDelegate = EaseSine;
        private static EaseDelegate _easeCircleDelegate = EaseCircle;
        private static EaseDelegate _easeExponential = EaseExponential;

        public Tween EaseInCubic() => EaseIn(_easeCubicDelegate);
        public Tween EaseOutCubic() => EaseOut(_easeCubicDelegate);
        public Tween EaseInOutCubic() => EaseIn(_easeCubicDelegate).EaseOut(_easeCubicDelegate);

        public Tween EaseInBack(float amplitude = 1f) => EaseIn(_easeBackDelegate, amplitude);
        public Tween EaseOutBack(float amplitude = 1f) => EaseOut(_easeBackDelegate, amplitude);
        public Tween EaseInOutBack(float amplitude = 1f) => EaseIn(_easeBackDelegate, amplitude).EaseOut(_easeBackDelegate, amplitude);

        public Tween EaseInElastic(int oscillations = 3, float springiness = 3f) => EaseIn(_easeElasticDelegate, oscillations, springiness);
        public Tween EaseOutElastic(int oscillations = 3, float springiness = 3f) => EaseOut(_easeElasticDelegate, oscillations, springiness);
        public Tween EaseInOutElastic(int oscillations = 3, float springiness = 3f) => EaseIn(_easeElasticDelegate, oscillations, springiness).EaseOut(_easeElasticDelegate, oscillations, springiness);

        public Tween EaseInBounce(int oscillations = 3, float springiness = 2f) => EaseIn(_easeBounceDelegate, oscillations, springiness);
        public Tween EaseOutBounce(int oscillations = 3, float springiness = 2f) => EaseOut(_easeBounceDelegate, oscillations, springiness);
        public Tween EaseInOutBounce(int oscillations = 3, float springiness = 2f) => EaseIn(_easeBounceDelegate, oscillations, springiness).EaseOut(_easeBounceDelegate, oscillations, springiness);

        public Tween EaseInSine() => EaseIn(_easeSineDelegate);
        public Tween EaseOutSine() => EaseOut(_easeSineDelegate);
        public Tween EaseInOutSine() => EaseIn(_easeSineDelegate).EaseOut(_easeSineDelegate);

        public Tween EaseInCircle() => EaseIn(_easeCircleDelegate);
        public Tween EaseOutCircle() => EaseOut(_easeCircleDelegate);
        public Tween EaseInOutCircle() => EaseIn(_easeCircleDelegate).EaseOut(_easeCircleDelegate);

        public Tween EaseInExponential(float exponent=2.0f) => EaseIn(_easeExponential,exponent);
        public Tween EaseOutExponential(float exponent=2.0f) => EaseOut(_easeExponential,exponent);
        public Tween EaseInOutExponential(float exponent=2.0f) => EaseIn(_easeExponential,exponent).EaseOut(_easeExponential,exponent);
    }
}
