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
    public static class TweenExtensions
    {
        #region UnityEngine.Transform

        private static readonly Vector3MemberProvider<Transform> _transformLocalPositionProvider = Vector3MemberProvider<Transform>.Get("localPosition");
        private static readonly QuaternionMemberProvider<Transform> _transformLocalRotationProvider = QuaternionMemberProvider<Transform>.Get("localRotation");
        private static readonly Vector3MemberProvider<Transform> _transformLocalScaleProvider = Vector3MemberProvider<Transform>.Get("localScale");
        private static readonly Vector3MemberProvider<Transform> _transformPositionProvider = Vector3MemberProvider<Transform>.Get("position");
        private static readonly QuaternionMemberProvider<Transform> _transformRotationProvider = QuaternionMemberProvider<Transform>.Get("rotation");

        public static Tween TweenLocalPosition (this Transform transform, Vector3 to, Vector3Options options = Vector3Options.None) =>
            Tween.To (_transformLocalPositionProvider, transform, to, options);

        public static Tween TweenLocalRotation(this Transform transform, Quaternion to) =>
            Tween.To(_transformLocalRotationProvider, transform, to);

        public static Tween TweenLocalScale(this Transform transform, Vector3 to) =>
            Tween.To(_transformLocalScaleProvider, transform, to);
        public static Tween TweenLocalScale(this Transform transform, float to) =>
            Tween.To(_transformLocalScaleProvider, transform, new Vector3(to,to,to));

        public static Tween TweenPosition(this Transform transform, Vector3 to) =>
            Tween.To(_transformPositionProvider, transform, to);

        public static Tween TweenRotation(this Transform transform, Quaternion to) =>
            Tween.To(_transformRotationProvider, transform, to);

        #endregion

        #region UnityEngine.Camera

        private static readonly FloatMemberProvider<Camera> _cameraFieldOfViewProvider = FloatMemberProvider<Camera>.Get("fieldOfView");

        /// <summary>
        /// Tween the field of view of a camera from the current value to the given <paramref name="to"/> value.
        /// </summary>
        /// <param name="target">Camera target</param>
        /// <param name="to">Ending value</param>
        /// <returns></returns>
        public static Tween TweenFieldOfView (this Camera target, float to) =>
            Tween.To(_cameraFieldOfViewProvider, target, to);

        #endregion

        #region UnityEngine.Animator

        /// <summary>
        /// Tweens the current value of the target animator float parameter to the the given <paramref name="to"/> value.
        /// </summary>
        /// <param name="target">Target Animator</param>
        /// <param name="id">Parameter id</param>
        /// <param name="to">Ending value</param>
        public static Tween TweenFloat(this Animator target, int id, float to) =>
            Tween.To(AnimatorFloatProvider.Get(id), target, to);

        /// <summary>
        /// Tweens the current value of the target animator float parameter to the the given <paramref name="to"/> value.
        /// </summary>
        /// <param name="target">Target Animator</param>
        /// <param name="name">Parameter name</param>
        /// <param name="to">Ending value</param>
        public static Tween TweenFloat(this Animator target, string name, float to) =>
            Tween.To(AnimatorFloatProvider.Get(name), target, to);

        /// <summary>
        /// Tweens the current value of the target animator integer parameter to the the given <paramref name="to"/> value.
        /// </summary>
        /// <param name="target">Target Animator</param>
        /// <param name="id">Parameter id</param>
        /// <param name="from">Starting value</param>
        /// <param name="to">Ending value</param>
        public static Tween TweenInteger (this Animator target, int id, int to) =>
            Tween.To(AnimatorInt32Provider.Get(id), target, to);

        /// <summary>
        /// Tweens the current value of the target animator integer parameter to the the given <paramref name="to"/> value.
        /// </summary>
        /// <param name="target">Target Animator</param>
        /// <param name="name">Parameter name</param>
        /// <param name="from">Starting value</param>
        /// <param name="to">Ending value</param>
        public static Tween TweenInteger(this Animator target, string name, int to) =>
            Tween.To(AnimatorInt32Provider.Get(name), target, to);

        #endregion

        #region UnityEngine.UI.Graphic

#if UNITY_UI
        private static readonly ColorMemberProvider<UnityEngine.UI.Graphic> _graphicColorProvider = ColorMemberProvider<UnityEngine.UI.Graphic>.Get("color");
        private static readonly FloatMemberProvider<CanvasGroup> _canvasGroupAlphaProvider = FloatMemberProvider<CanvasGroup>.Get("alpha");

        public static Tween TweenAlpha(this UnityEngine.UI.Graphic graphic, float to) =>
            Tween.To(_graphicColorProvider, graphic, new Color(0,0,0,to));

        public static Tween TweenColor(this UnityEngine.UI.Graphic graphic, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(_graphicColorProvider, graphic, to, options);

        public static Tween TweenAlpha(this CanvasGroup canvasGroup, float to) =>
            Tween.To(_canvasGroupAlphaProvider, canvasGroup, to);
#endif
        #endregion

        #region UnityEngine.SpriteRenderer

        private static readonly ColorMemberProvider<SpriteRenderer> _spriteColorProvider = ColorMemberProvider<SpriteRenderer>.Get("color");

        public static Tween TweenAlpha(this SpriteRenderer image, float to) => 
            Tween.To(_spriteColorProvider, image, new Color(0,0,0,to));

        public static Tween TweenColor(this SpriteRenderer image, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(_spriteColorProvider, image, to, options);

        #endregion

        #region UnityEngine.Material

        private static ColorMemberProvider<Material> _materialColorProvider = ColorMemberProvider<Material>.Get("color");

        public static Tween TweenColor(this Material material, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(_materialColorProvider, material, to, options);

        public static Tween TweenFloat(this Material material, int id, float to) =>
            Tween.To(MaterialFloatProvider.Get(material, id), material, to);
        public static Tween TweenFloat(this Material material, string name, float to) =>
            Tween.To(MaterialFloatProvider.Get(material, Shader.PropertyToID(name)), material, to);

        public static Tween TweenColor(this Material material, int id, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(MaterialColorProvider.Get(material, id), material, to);
        public static Tween TweenColor(this Material material, string name, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(MaterialColorProvider.Get(material, Shader.PropertyToID(name)), material, to);

        public static Tween TweenVector(this Material material, int id, Vector4 to, Vector4Options options = Vector4Options.None) =>
            Tween.To(MaterialVectorProvider.Get(material, id), material, to, options);
        public static Tween TweenVector(this Material material, string name, Vector4 to, Vector4Options options = Vector4Options.None) =>
            Tween.To(MaterialVectorProvider.Get(material, Shader.PropertyToID(name)), material, to, options);

        #endregion

        #region Property & Fields

        public static Tween TweenFloat<T>(this T target, string name, float to) where T : class =>
            Tween.To(FloatMemberProvider<T>.Get(name), target, to);
        public static Tween TweenColor<T>(this T target, string name, Color to, ColorOptions options = ColorOptions.None) where T : class =>
            Tween.To(ColorMemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenVector<T>(this T target, string name, Vector2 to) where T : class =>
            Tween.To(Vector2MemberProvider<T>.Get(name), target, to);
        public static Tween TweenVector<T>(this T target, string name, Vector3 to) where T : class =>
            Tween.To(Vector3MemberProvider<T>.Get(name), target, to);
        public static Tween TweenVector<T>(this T target, string name, Vector4 to) where T : class =>
            Tween.To(Vector4MemberProvider<T>.Get(name), target, to);

        #endregion

        #region Global

        public static Tween TweenWait(this object target, float duration = 1.0f) => Tween.Wait(target, duration);

        public static Tween TweenGroup(this object target) => Tween.Group(target);

        public static Tween TweenSequence(this object target) => Tween.Sequence(target);

        public static void TweenStop(this object target, string key = null, bool executeCallbacks = true) =>
            Tween.Stop(target, key, executeCallbacks);

        #endregion
    }
}
