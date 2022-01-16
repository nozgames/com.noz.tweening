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
    /// <summary>
    /// Plays an tween on a node
    /// </summary>
#if false
    public partial class Tween
    {
        private static void ShakeUpdate (Tween tween, float t)
        {
            ShakePositionUpdate(tween, t);
            ShakeRotationUpdate(tween, t);
        }

        private static void ShakePositionUpdate(Tween tween, float t)
        {            
            (tween._target as Transform).position =
                new Vector2(
                    tween._param0.x * (Mathf.PerlinNoise(tween._param1.x, t * 20f) - 0.5f) * 2.0f,
                    tween._param0.y * (Mathf.PerlinNoise(tween._param1.y, t * 20f) - 0.5f) * 2.0f
                    ) * (1f - t);
        }

        private static void ShakeRotationUpdate(Tween tween, float t)
        {
            (tween._target as Transform).localRotation =
                Quaternion.Euler(0,0,tween._param0.z * (Mathf.PerlinNoise(tween._param1.z, t * 20f) - 0.5f) * 2.0f * (1f - t));
        }

        private static void MoveUpdate(Tween tween, float t)
        {
            var pos = (Vector2)(tween._param0 * (1 - t) + tween._param1 * t);
            var rectTransform = (tween._target as RectTransform);
            if (rectTransform != null)
                rectTransform.anchoredPosition = pos;
            else
                (tween._target as Transform).localPosition = pos;
        }

        private static void FadeRendererUpdate (Tween tween, float t)
        {
            var renderer = (tween._target as Renderer);
            var color = renderer.material.color;
            color.a = tween._param0.x * (1f - t) + tween._param1.x * t;
            renderer.material.color = color;
        }

        private static void FadeMeshRendererUpdate (Tween tween, float t)
        {
            var renderer = (tween._target as MeshRenderer);
            var color = renderer.material.color;
            color.a = tween._param0.x * (1f - t) + tween._param1.x * t;
            renderer.material.color = color;
        }

        private static void FadeImageUpdate(Tween tween, float t)
        {
            var image = (tween._target as UnityEngine.UI.Image);
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                tween._param0.x * (1f - t) + tween._param1.x * t
                );
        }

        private static void FadeTextMeshProUIUpdate(Tween tween, float t)
        {
            var tmproui = (tween._target as TMPro.TextMeshProUGUI);
            tmproui.color = new Color(
                tmproui.color.r,
                tmproui.color.g,
                tmproui.color.b,
                tween._param0.x * (1f - t) + tween._param1.x * t
                );
        }

        private static void FadeTextMeshProUpdate(Tween tween, float t)
        {
            var tmpro = (tween._target as TMPro.TextMeshPro);
            tmpro.color = new Color(
                tmpro.color.r,
                tmpro.color.g,
                tmpro.color.b,
                tween._param0.x * (1f - t) + tween._param1.x * t
                );
        }

        private static void FadeCanvasGroupUpdate(Tween tween, float t)
        {
            var canvasgroup = (tween._target as CanvasGroup);
            canvasgroup.alpha = tween._param0.x * (1f - t) + tween._param1.x * t;
        }

        private static void ColorSpriteUpdate(Tween tween, float t)
        {
            var sprite = (tween._target as SpriteRenderer);
            var color = tween._param0 * (1f - t) + tween._param1 * t;
            sprite.color = new Color(color.x, color.y, color.z, sprite.color.a);
        }

        private static void ColorImageUpdate(Tween tween, float t)
        {
            var image = (tween._target as UnityEngine.UI.Image);
            var color = tween._param0 * (1f - t) + tween._param1 * t;
            image.color = new Color(color.x, color.y, color.z, image.color.a);
        }

        private static void ColorTextUpdate(Tween tween, float t)
        {
            var image = (tween._target as TMPro.TextMeshProUGUI);
            var color = tween._param0 * (1f - t) + tween._param1 * t;
            image.color = new Color(color.x, color.y, color.z, image.color.a);
        }

        private static void ZoomUpdate (Tween tween, float t)
        {
            (tween._target as Camera).fieldOfView = Mathf.Lerp(tween._param0.x, tween._param1.x, t);
        }

        private static void CustomUpdate(Tween tween, float t)
        {
            var result = tween._custom.Invoke(tween, t);
            if (!result)
                tween.isActive = false;
        }
            

        private static void WaitUpdate(Tween tween, float t) { }

        private static readonly UpdateDelegate MoveUpdateDelegate = MoveUpdate;
        private static readonly UpdateDelegate MoveWorldUpdateDelegate = MoveWorldUpdate;
        private static readonly UpdateDelegate MoveToUpdateDelegate = MoveToUpdate;
        private static readonly UpdateDelegate MoveToWorldUpdateDelegate = MoveToWorldUpdate;
        private static readonly UpdateDelegate ScaleUpdateDelegate = ScaleUpdate;
        private static readonly UpdateDelegate RotateUpdateDelegate = RotateUpdate;
        private static readonly UpdateDelegate RotateQuaternionUpdateDelegate = RotateQuaternionUpdate;
        private static readonly UpdateDelegate RotateQuaternionLocalUpdateDelegate = RotateQuaternionLocalUpdate;
        private static readonly UpdateDelegate FadeSpriteUpdateDelegate = FadeSpriteUpdate;
        private static readonly UpdateDelegate FadeRendererUpdateDelegate = FadeRendererUpdate;
        private static readonly UpdateDelegate FadeImageUpdateDelegate = FadeImageUpdate;
        private static readonly UpdateDelegate FadeTextMeshProUpdateDelegate = FadeTextMeshProUpdate;
        private static readonly UpdateDelegate FadeMeshRendererUpdateDelegate = FadeMeshRendererUpdate;
        private static readonly UpdateDelegate FadeTextMeshProUIUpdateDelegate = FadeTextMeshProUIUpdate;
        private static readonly UpdateDelegate FadeCanvasGroupUpdateDelegate = FadeCanvasGroupUpdate;
        private static readonly UpdateDelegate WaitUpdateDelegate = WaitUpdate;
        private static readonly UpdateDelegate ShakeUpdateDelegate = ShakeUpdate;
        private static readonly UpdateDelegate ShakePositionUpdateDelegate = ShakePositionUpdate;
        private static readonly UpdateDelegate ShakeRotationUpdateDelegate = ShakeRotationUpdate;
        private static readonly UpdateDelegate ColorSpriteUpdateDelegate = ColorSpriteUpdate;
        private static readonly UpdateDelegate ColorImageUpdateDelegate = ColorImageUpdate;
        private static readonly UpdateDelegate ColorTextUpdateDelegate = ColorTextUpdate;
        private static readonly UpdateDelegate ZoomUpdateDelegate = ZoomUpdate;
        private static readonly UpdateDelegate CustomUpdateDelegate = CustomUpdate;
    }
#endif
}
