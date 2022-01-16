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

using UnityEngine;

namespace NoZ.Tweenz
{
    public partial struct Tween
    {
        private static Tween From(TweenProvider provider, object target, Variant from, Variant to, uint options = 0) =>
            AllocTween(provider, target, from, to, Flags.From, options);

        public static Tween From<T>(Int32Provider<T> provider, object target, int from, int to, Int32Options options = Int32Options.None) where T : class => From(provider, target, from, to, options);
        public static Tween From<T>(FloatProvider<T> provider, object target, float from) where T : class => From(provider, target, from, from, 0);
        public static Tween From<T>(ColorProvider<T> provider, object target, Color from, ColorOptions options = ColorOptions.None) where T : class => From(provider, target, from, from, (uint)options);
        public static Tween From<T>(Vector2Provider<T> provider, object target, Vector2 from, Vector2Options options = Vector2Options.None) where T : class => From(provider, target, from, (uint)options);
        public static Tween From<T>(Vector3Provider<T> provider, object target, Vector3 from, Vector3Options options = Vector3Options.None) where T : class => From(provider, target, from, (uint)options);
        public static Tween From<T>(Vector4Provider<T> provider, object target, Vector4 from, Vector4Options options = Vector4Options.None) where T : class => From(provider, target, from, (uint)options);
        public static Tween From<T>(QuaternionProvider<T> provider, object target, Quaternion from) where T : class => From(provider, target, from, 0);

        private static Tween To(TweenProvider provider, object target, Variant to, uint options = 0) =>
            AllocTween(provider, target, to, to, Flags.To, options);

        public static Tween To<T>(Int32Provider<T> provider, object target, int to, Int32Options options = Int32Options.None) where T : class => To(provider, target, to, options);
        public static Tween To<T>(FloatProvider<T> provider, object target, float to) where T : class => To(provider, target, to, 0);
        public static Tween To<T>(ColorProvider<T> provider, object target, Color to, ColorOptions options = ColorOptions.None) where T : class => To(provider, target, to, (uint)options);
        public static Tween To<T>(Vector2Provider<T> provider, object target, Vector2 to, Vector2Options options = Vector2Options.None) where T : class => To(provider, target, to, (uint)options);
        public static Tween To<T>(Vector3Provider<T> provider, object target, Vector3 to, Vector3Options options = Vector3Options.None) where T : class => To(provider, target, to, (uint)options);
        public static Tween To<T>(Vector4Provider<T> provider, object target, Vector4 to, Vector4Options options = Vector4Options.None) where T : class => To(provider, target, to, (uint)options);
        public static Tween To<T>(QuaternionProvider<T> provider, object target, Quaternion to) where T : class => To(provider, target, to, 0);


        private static Tween FromTo(TweenProvider provider, object target, Variant from, Variant to, uint options = 0) =>
            AllocTween(provider, target, from, to, Flags.None, options);

        public static Tween FromTo<T>(Int32Provider<T> provider, object target, int from, int to, Int32Options options = Int32Options.None) where T : class => From(provider, target, from, to, options);
        public static Tween FromTo<T>(FloatProvider<T> provider, object target, float from, float to) where T : class => From(provider, target, from, to, 0);
        public static Tween FromTo<T>(ColorProvider<T> provider, object target, Color from, Color to, ColorOptions options = ColorOptions.None) where T : class => From(provider, target, from, to, (uint)options);
        public static Tween FromTo<T>(Vector2Provider<T> provider, object target, Vector2 from, Vector2 to, Vector2Options options = Vector2Options.None) where T : class => From(provider, target, from, to, (uint)options);
        public static Tween FromTo<T>(Vector3Provider<T> provider, object target, Vector3 from, Vector3 to, Vector3Options options = Vector3Options.None) where T : class => From(provider, target, from, to, (uint)options);
        public static Tween FromTo<T>(Vector4Provider<T> provider, object target, Vector4 from, Vector4 to, Vector4Options options = Vector4Options.None) where T : class => From(provider, target, from, to, (uint)options);
        public static Tween FromTo<T>(QuaternionProvider<T> provider, object target, Quaternion from, Quaternion to) where T : class => From(provider, target, from, to, 0);


        public static Tween Wait(object target, float duration = 1.0f) => AllocTween(null, target, Vector4.zero, Vector4.zero).Duration(duration);

        public static Tween Sequence(object target) => AllocTween(null, target, Vector4.zero, Vector4.zero, Flags.Collection | Flags.Sequence);

        public static Tween Group(object target) => AllocTween(null, target, Vector4.zero, Vector4.zero, Flags.Collection);



#if false


        public static Tween Shake(Vector2 positionalIntensity, float rotationalIntensity)
        {
            var tween = AllocTween(_shakeProviders);
            tween._param0 = new Vector3(positionalIntensity.x, positionalIntensity.y, rotationalIntensity);
            tween._param1 = new Vector3(
                Random.Range(0.0f, 100.0f),
                Random.Range(0.0f, 100.0f),
                Random.Range(0.0f, 100.0f));
            tween._update = ShakeUpdateDelegate;
            tween._start = TransformStartDelegate;
            return tween;
        }

        public static Tween ShakePosition(Vector2 intensity)
        {
            var tween = AllocTween();
            tween._param0 = intensity;
            tween._param1 = new Vector3(
                Random.Range(0.0f, 100.0f),
                Random.Range(0.0f, 100.0f), 0);
            tween._update = ShakePositionUpdateDelegate;
            tween._start = TransformStartDelegate;
            return tween;
        }

        public static Tween ShakeRotation(float intensity)
        {
            var tween = AllocTween();
            tween._param0 = new Vector3(0.0f, 0.0f, intensity);
            tween._param1 = new Vector3(0.0f, 0.0f, Random.Range(0.0f, 100.0f));
            tween._update = ShakeRotationUpdateDelegate;
            tween._start = TransformStartDelegate;
            return tween;
        }


        public static Tween Activate()
        {
            var tween = AllocTween();
            tween._start = ActivateStartDelegate;
            tween._update = WaitUpdateDelegate;
            tween._duration = 0.0f;
            return tween;
        }

#endif
    }
}
