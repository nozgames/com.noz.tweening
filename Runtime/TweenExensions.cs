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
using UnityEngine.UIElements;

namespace NoZ.Tweening
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

        public static Tween TweenLocalRotation(this Transform transform, Vector3 eulerAngles) =>
            Tween.To(_transformLocalRotationProvider, transform, Quaternion.Euler(eulerAngles));

        public static Tween TweenLocalScale(this Transform transform, Vector3 to) =>
            Tween.To(_transformLocalScaleProvider, transform, to);

        public static Tween TweenLocalScale(this Transform transform, float to) =>
            Tween.To(_transformLocalScaleProvider, transform, new Vector3(to,to,to));

        public static Tween TweenPosition(this Transform transform, Vector3 to) =>
            Tween.To(_transformPositionProvider, transform, to);

        public static Tween TweenRotation(this Transform transform, Quaternion to) =>
            Tween.To(_transformRotationProvider, transform, to);

        public static Tween TweenLookAt(this Transform transform, Vector3 to) =>
            Tween.To(_transformRotationProvider, transform, Quaternion.LookRotation(to));

        public static Tween TweenLookAt (this Transform transform, Vector3 to, Vector3 up) =>
            Tween.To(_transformRotationProvider, transform, Quaternion.LookRotation(to, up));

        #endregion

        #region UnityEngine.RectTransform

        private static readonly Vector3MemberProvider<RectTransform> _rectTransformAnchoredPosition3DProvider = Vector3MemberProvider<RectTransform>.Get("anchoredPosition3D");
        private static readonly Vector2MemberProvider<RectTransform> _rectTransformAnchoredPositionProvider = Vector2MemberProvider<RectTransform>.Get("anchoredPosition");
        private static readonly Vector2MemberProvider<RectTransform> _rectTransformAnchorMinProvider = Vector2MemberProvider<RectTransform>.Get("anchorMin");
        private static readonly Vector2MemberProvider<RectTransform> _rectTransformAnchorMaxProvider = Vector2MemberProvider<RectTransform>.Get("anchorMax");
        private static readonly Vector2MemberProvider<RectTransform> _rectTransformOffsetMinProvider = Vector2MemberProvider<RectTransform>.Get("offsetMin");
        private static readonly Vector2MemberProvider<RectTransform> _rectTransformOffsetMaxProvider = Vector2MemberProvider<RectTransform>.Get("offsetMax");
        private static readonly Vector2MemberProvider<RectTransform> _rectTransformSizeDeltaProvider = Vector2MemberProvider<RectTransform>.Get("sizeDelta");
        private static readonly Vector2MemberProvider<RectTransform> _rectTransformPivotProvider = Vector2MemberProvider<RectTransform>.Get("pivot");

        public static Tween TweenAnchorPosition3D (this Transform transform, Vector3 to, Vector3Options options = Vector3Options.None) =>
            Tween.To(_rectTransformAnchoredPosition3DProvider, transform, to, options);

        public static Tween TweenAnchorPosition(this Transform transform, Vector2 to, Vector2Options options = Vector2Options.None) =>
            Tween.To(_rectTransformAnchoredPositionProvider, transform, to, options);

        public static Tween TweenAnchorMin (this Transform transform, Vector2 to, Vector2Options options = Vector2Options.None) =>
            Tween.To(_rectTransformAnchorMinProvider, transform, to, options);

        public static Tween TweenAnchorMax (this Transform transform, Vector2 to, Vector2Options options = Vector2Options.None) =>
            Tween.To(_rectTransformAnchorMaxProvider, transform, to, options);

        public static Tween TweenOffsetMin(this Transform transform, Vector2 to, Vector2Options options = Vector2Options.None) =>
            Tween.To(_rectTransformOffsetMinProvider, transform, to, options);

        public static Tween TweenOffsetMax(this Transform transform, Vector2 to, Vector2Options options = Vector2Options.None) =>
            Tween.To(_rectTransformOffsetMaxProvider, transform, to, options);

        public static Tween TweenSizeDelta (this Transform transform, Vector2 to, Vector2Options options = Vector2Options.None) =>
            Tween.To(_rectTransformSizeDeltaProvider, transform, to, options);

        public static Tween TweenPivot (this Transform transform, Vector2 to, Vector2Options options = Vector2Options.None) =>
            Tween.To(_rectTransformPivotProvider, transform, to, options);

        #endregion

        #region UnityEngine.Camera

        private static readonly FloatMemberProvider<Camera> _cameraFieldOfViewProvider = FloatMemberProvider<Camera>.Get("fieldOfView");

        /// <summary>
        /// Tween the field of view of a camera from the current value to the given <paramref name="to"/> value.
        /// </summary>
        /// <param name="target">Camera target</param>
        /// <param name="to">Ending field of view</param>
        public static Tween TweenFieldOfView (this Camera target, float to) => Tween.To(_cameraFieldOfViewProvider, target, to);

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
            Tween.To(AnimatorIntProvider.Get(id), target, to);

        /// <summary>
        /// Tweens the current value of the target animator integer parameter to the the given <paramref name="to"/> value.
        /// </summary>
        /// <param name="target">Target Animator</param>
        /// <param name="name">Parameter name</param>
        /// <param name="from">Starting value</param>
        /// <param name="to">Ending value</param>
        public static Tween TweenInteger(this Animator target, string name, int to) =>
            Tween.To(AnimatorIntProvider.Get(name), target, to);

        #endregion

        #region UnityEngine.AudioSource

        private static readonly FloatMemberProvider<AudioSource> _audioSourceVolumeProvider = FloatMemberProvider<AudioSource>.Get("volume");
        private static readonly FloatMemberProvider<AudioSource> _audioSourcePitchProvider = FloatMemberProvider<AudioSource>.Get("pitch");

        public static Tween TweenVolume (this AudioSource target, float to) =>
            Tween.To(_audioSourceVolumeProvider, target, to);

        public static Tween TweenPitch (this AudioSource target, float to) =>
            Tween.To(_audioSourcePitchProvider, target, to);

        #endregion

        #region UnityEngine.UI.Graphic

#if UNITY_UI
        private static readonly ColorMemberProvider<UnityEngine.UI.Graphic> _graphicColorProvider = ColorMemberProvider<UnityEngine.UI.Graphic>.Get("color");
        private static readonly FloatMemberProvider<CanvasGroup> _canvasGroupAlphaProvider = FloatMemberProvider<CanvasGroup>.Get("alpha");

        public static Tween TweenAlpha(this UnityEngine.UI.Graphic graphic, float to) =>
            Tween.To(_graphicColorProvider, graphic, new Color(0,0,0,to), ColorOptions.IgnoreRGB);

        public static Tween TweenColor(this UnityEngine.UI.Graphic graphic, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(_graphicColorProvider, graphic, to, options);

        public static Tween TweenAlpha(this CanvasGroup canvasGroup, float to) =>
            Tween.To(_canvasGroupAlphaProvider, canvasGroup, to);
#endif
        #endregion

        #region UnityEngine.Light

        private static readonly FloatMemberProvider<Light> _lightIntensityProvider = FloatMemberProvider<Light>.Get("intensity");
        private static readonly ColorMemberProvider<Light> _lightColorProvider = ColorMemberProvider<Light>.Get("color");

        public static Tween TweenIntensity (this Light target, float to) =>
            Tween.To(_lightIntensityProvider, target, to);

        public static Tween TweenColor (this Light target, Color to) =>
            Tween.To(_lightColorProvider, target, to);

        #endregion

        #region UnityEngine.SpriteRenderer

        private static readonly ColorMemberProvider<SpriteRenderer> _spriteColorProvider = ColorMemberProvider<SpriteRenderer>.Get("color");

        public static Tween TweenAlpha(this SpriteRenderer image, float to) => 
            Tween.To(_spriteColorProvider, image, new Color(0,0,0,to), ColorOptions.IgnoreRGB);

        public static Tween TweenColor(this SpriteRenderer image, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(_spriteColorProvider, image, to, options);

        #endregion

        #region UnityEngine.Material

        private static ColorMemberProvider<Material> _materialColorProvider = ColorMemberProvider<Material>.Get("color");

        public static Tween TweenColor(this Material material, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(_materialColorProvider, material, to, options);

        public static Tween TweenAlpha(this Material material, float to) =>
            Tween.To(_materialColorProvider, material, new Color(0,0,0,to), ColorOptions.IgnoreRGB);

        public static Tween TweenFloat(this Material material, int id, float to) =>
            Tween.To(MaterialProviderUtils.GetProvider<MaterialFloatProvider>(material, id), material, to);
        public static Tween TweenFloat(this Material material, string name, float to) =>
            Tween.To(MaterialProviderUtils.GetProvider<MaterialFloatProvider>(material, Shader.PropertyToID(name)), material, to);

        public static Tween TweenColor(this Material material, int id, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(MaterialProviderUtils.GetProvider<MaterialColorProvider>(material, id), material, to, options);
        public static Tween TweenColor(this Material material, string name, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(MaterialProviderUtils.GetProvider<MaterialColorProvider>(material, Shader.PropertyToID(name)), material, to, options);

        public static Tween TweenVector(this Material material, int id, Vector4 to, Vector4Options options = Vector4Options.None) =>
            Tween.To(MaterialProviderUtils.GetProvider<MaterialVectorProvider>(material, id), material, to, options);
        public static Tween TweenVector(this Material material, string name, Vector4 to, Vector4Options options = Vector4Options.None) =>
            Tween.To(MaterialProviderUtils.GetProvider<MaterialVectorProvider>(material, Shader.PropertyToID(name)), material, to, options);

        #endregion

        #region UnityEngine.MaterialPropertyBlock

        public static Tween TweenFloat(this MaterialPropertyBlock material, int id, float to) =>
            Tween.To(MaterialPropertyBlockProviderUtils.GetProvider<MaterialFloatProvider>(material, id), material, to);
        public static Tween TweenFloat(this MaterialPropertyBlock material, string name, float to) =>
            Tween.To(MaterialPropertyBlockProviderUtils.GetProvider<MaterialFloatProvider>(material, Shader.PropertyToID(name)), material, to);

        public static Tween TweenColor(this MaterialPropertyBlock material, int id, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(MaterialPropertyBlockProviderUtils.GetProvider<MaterialColorProvider>(material, id), material, to, options);
        public static Tween TweenColor(this MaterialPropertyBlock material, string name, Color to, ColorOptions options = ColorOptions.None) =>
            Tween.To(MaterialPropertyBlockProviderUtils.GetProvider<MaterialColorProvider>(material, Shader.PropertyToID(name)), material, to, options);

        public static Tween TweenVector(this MaterialPropertyBlock material, int id, Vector4 to, Vector4Options options = Vector4Options.None) =>
            Tween.To(MaterialPropertyBlockProviderUtils.GetProvider<MaterialVectorProvider>(material, id), material, to, options);
        public static Tween TweenVector(this MaterialPropertyBlock material, string name, Vector4 to, Vector4Options options = Vector4Options.None) =>
            Tween.To(MaterialPropertyBlockProviderUtils.GetProvider<MaterialVectorProvider>(material, Shader.PropertyToID(name)), material, to, options);

        #endregion

        #region Property & Fields

        public static Tween TweenFloat<T>(this T target, string name, float to) where T : class =>
            Tween.To(FloatMemberProvider<T>.Get(name), target, to);
        public static Tween TweenFloat<T>(this T target, string name, StyleFloat to) where T : class =>
            Tween.To(StyleFloatMemberProvider<T>.Get(name), target, to);

        public static Tween TweenDouble<T>(this T target, string name, double to) where T : class =>
            Tween.To(DoubleMemberProvider<T>.Get(name), target, to);
        public static Tween TweenInt<T>(this T target, string name, int to, IntOptions options = IntOptions.None) where T : class =>
            Tween.To(IntMemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenUInt<T>(this T target, string name, uint to, UIntOptions options = UIntOptions.None) where T : class =>
            Tween.To(UIntMemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenLong<T>(this T target, string name, long to, LongOptions options = LongOptions.None) where T : class =>
            Tween.To(LongMemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenULong<T>(this T target, string name, ulong to, ULongOptions options = ULongOptions.None) where T : class =>
            Tween.To(ULongMemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenColor<T>(this T target, string name, Color to, ColorOptions options = ColorOptions.None) where T : class =>
            Tween.To(ColorMemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenVector<T>(this T target, string name, Vector2 to, Vector2Options options = Vector2Options.None) where T : class =>
            Tween.To(Vector2MemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenVector<T>(this T target, string name, Vector3 to, Vector3Options options = Vector3Options.None) where T : class =>
            Tween.To(Vector3MemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenVector<T>(this T target, string name, Vector4 to, Vector4Options options = Vector4Options.None) where T : class =>
            Tween.To(Vector4MemberProvider<T>.Get(name), target, to, options);
        public static Tween TweenQuaternion<T>(this T target, string name, Quaternion to) where T : class =>
            Tween.To(QuaternionMemberProvider<T>.Get(name), target, to);

        public static Tween TweenLength<T>(this T target, string name, StyleLength to) where T : class =>
            Tween.To(StyleLengthMemberProvider<T>.Get(name), target, to);

        #endregion

        #region Global

        public static Tween TweenWait(this object target, float duration = 1.0f) => Tween.Wait(target, duration);

        public static Tween TweenGroup(this object target) => Tween.Group(target);

        public static Tween TweenSequence(this object target) => Tween.Sequence(target);

        public static void TweenStop(this object target, int id=0, bool executeCallbacks=true) =>
            Tween.Stop(target, id, executeCallbacks);

        #endregion
    }
}
