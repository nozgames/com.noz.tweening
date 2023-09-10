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

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using NoZ.Tweening.Internals;
using UnityEngine.UIElements;
using UnityEngine.Scripting;

[assembly: InternalsVisibleTo("NoZ.Tweening.Editor")]
[assembly: InternalsVisibleTo("NoZ.Tweening.Tests")]
[assembly: Preserve]

namespace NoZ.Tweening
{
    public partial struct Tween
    {
        private uint _instanceId;
        private TweenContext _context;

        private static EaseDelegate _easeQuadraticDelegate = Easing.EaseQuadratic;
        private static EaseDelegate _easeCubicDelegate = Easing.EaseCubic;
        private static EaseDelegate _easeBackDelegate = Easing.EaseBack;
        private static EaseDelegate _easeElasticDelegate = Easing.EaseElastic;
        private static EaseDelegate _easeBounceDelegate = Easing.EaseBounce;
        private static EaseDelegate _easeSineDelegate = Easing.EaseSine;
        private static EaseDelegate _easeCircleDelegate = Easing.EaseCircle;
        private static EaseDelegate _easeExponential = Easing.EaseExponential;
        private static EaseDelegate _easeCubicBezier = Easing.EaseCubicBezier;

        /// <summary>
        /// Returns true if any tween is currently animating.
        /// </summary>
        public static bool IsAnyTweenAnimating =>
            (TweenContext._stateContexts[(int)TweenContext.State.Manual].Count +
             TweenContext._stateContexts[(int)TweenContext.State.Playing].Count) > 0;

        /// <summary>
        /// Called each frame that the tween system is running on LateUpdate.
        /// </summary>
        /// <returns></returns>
        public static event Action<int> Frame;

        /// <summary>
        /// Returns true if the tween is valid and currently playing.  Note that false will always
        /// be returned for Element tweens as the play state is managed by the top level collection tween.
        /// </summary>
        public bool isPlaying => isValid && !_context.isElement && (_context.isPlaying || _context.isManual);

        /// <summary>
        /// Returns true if the tween is currently paused
        /// </summary>
        public bool isPaused => isValid && !_context.isElement && _context.isPaused;

        /// <summary>
        /// Returns true if the tween has not been stopped
        /// </summary>
        public bool isValid => _context != null && _context.instanceId == _instanceId && !_context.isFree;

        /// <summary>
        /// Get current elapsed time in seconds.
        /// </summary>
        public float time => isPlaying ? _context.elapsed : 0.0f;

        /// <summary>
        /// Get the current elapsed time using a normalized time of 0-1
        /// </summary>
        public float normalizedTime => isPlaying ? _context.elapsed / (_context.delay + _context.duration) : 0.0f;

        /// <summary>
        /// Returns the tween duration in seconds.  Note that zero will be returned if the tween is not playing.
        /// </summary>
        public float duration => isPlaying ? _context.duration : 0.0f;

        private static Tween From(TweenProvider provider, object target, Variant from, uint options = 0) =>
            AllocTween(provider, target, from, from, TweenContext.Flags.From, options);

        public static Tween From<T>(IntProvider<T> provider, object target, int from,
            IntOptions options = IntOptions.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(UIntProvider<T> provider, object target, uint from,
            UIntOptions options = UIntOptions.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(LongProvider<T> provider, object target, long from,
            LongOptions options = LongOptions.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(ULongProvider<T> provider, object target, ulong from,
            ULongOptions options = ULongOptions.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(FloatProvider<T> provider, object target, float from) where T : class =>
            From(provider, target, (Variant)from, 0);

        public static Tween From<T>(DoubleProvider<T> provider, object target, double from) where T : class =>
            From(provider, target, (Variant)from, 0);

        public static Tween From<T>(ColorProvider<T> provider, object target, Color from,
            ColorOptions options = ColorOptions.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(Vector2Provider<T> provider, object target, Vector2 from,
            Vector2Options options = Vector2Options.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(Vector3Provider<T> provider, object target, Vector3 from,
            Vector3Options options = Vector3Options.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(Vector4Provider<T> provider, object target, Vector4 from,
            Vector4Options options = Vector4Options.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        public static Tween From<T>(QuaternionProvider<T> provider, object target, Quaternion from,
            QuaternionOptions options = QuaternionOptions.None) where T : class =>
            From(provider, target, (Variant)from, (uint)options);

        private static Tween To(TweenProvider provider, object target, Variant to, uint options = 0) =>
            AllocTween(provider, target, to, to, TweenContext.Flags.To, options);

        public static Tween To<T>(IntProvider<T> provider, object target, int to, IntOptions options = IntOptions.None)
            where T : class => To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(UIntProvider<T> provider, object target, uint to,
            UIntOptions options = UIntOptions.None) where T : class => To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(LongProvider<T> provider, object target, long to,
            LongOptions options = LongOptions.None) where T : class => To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(ULongProvider<T> provider, object target, ulong to,
            ULongOptions options = ULongOptions.None) where T : class =>
            To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(FloatProvider<T> provider, object target, float to) where T : class =>
            To(provider, target, (Variant)to, 0);

        public static Tween To<T>(DoubleProvider<T> provider, object target, double to) where T : class =>
            To(provider, target, (Variant)to, 0);

        public static Tween To<T>(ColorProvider<T> provider, object target, Color to,
            ColorOptions options = ColorOptions.None) where T : class =>
            To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(Vector2Provider<T> provider, object target, Vector2 to,
            Vector2Options options = Vector2Options.None) where T : class =>
            To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(Vector3Provider<T> provider, object target, Vector3 to,
            Vector3Options options = Vector3Options.None) where T : class =>
            To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(Vector4Provider<T> provider, object target, Vector4 to,
            Vector4Options options = Vector4Options.None) where T : class =>
            To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(QuaternionProvider<T> provider, object target, Quaternion to,
            QuaternionOptions options = QuaternionOptions.None) where T : class =>
            To(provider, target, to, (uint)options);

        public static Tween To<T>(StyleFloatProvider<T> provider, object target, StyleFloat to) where T : class =>
            To(provider, target, (Variant)to, (uint)0);

        public static Tween To<T>(StyleLengthProvider<T> provider, object target, StyleLength to) where T : class =>
            To(provider, target, (Variant)to, (uint)0);

        public static Tween To<T>(StyleScaleProvider<T> provider, object target, StyleScale to) where T : class =>
            To(provider, target, (Variant)to, (uint)0);

        public static Tween To<T>(StyleColorProvider<T> provider, object target, StyleColor to, ColorOptions options)
            where T : class => To(provider, target, (Variant)to, (uint)options);

        public static Tween To<T>(StyleRotateProvider<T> provider, object target, StyleRotate to) where T : class =>
            To(provider, target, (Variant)to, (uint)0);

        public static Tween To<T>(StyleTranslateProvider<T> provider, object target, StyleTranslate to)
            where T : class => To(provider, target, (Variant)to, (uint)0);

        private static Tween FromTo(TweenProvider provider, object target, Variant from, Variant to,
            uint options = 0) =>
            AllocTween(provider, target, from, to, TweenContext.Flags.From | TweenContext.Flags.To, options);

        public static Tween FromTo<T>(IntProvider<T> provider, object target, int from, int to,
            IntOptions options = IntOptions.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(UIntProvider<T> provider, object target, uint from, uint to,
            UIntOptions options = UIntOptions.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(LongProvider<T> provider, object target, long from, long to,
            LongOptions options = LongOptions.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(ULongProvider<T> provider, object target, ulong from, ulong to,
            ULongOptions options = ULongOptions.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(FloatProvider<T> provider, object target, float from, float to) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, 0);

        public static Tween FromTo<T>(DoubleProvider<T> provider, object target, double from, double to)
            where T : class => FromTo(provider, target, (Variant)from, (Variant)to, 0);

        public static Tween FromTo<T>(ColorProvider<T> provider, object target, Color from, Color to,
            ColorOptions options = ColorOptions.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(Vector2Provider<T> provider, object target, Vector2 from, Vector2 to,
            Vector2Options options = Vector2Options.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(Vector3Provider<T> provider, object target, Vector3 from, Vector3 to,
            Vector3Options options = Vector3Options.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(Vector4Provider<T> provider, object target, Vector4 from, Vector4 to,
            Vector4Options options = Vector4Options.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(QuaternionProvider<T> provider, object target, Quaternion from, Quaternion to,
            QuaternionOptions options = QuaternionOptions.None) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(StyleFloatProvider<T> provider, object target, StyleFloat from, StyleFloat to)
            where T : class => FromTo(provider, target, (Variant)from, (Variant)to, (uint)0);

        public static Tween FromTo<T>(StyleLengthProvider<T> provider, object target, StyleLength from, StyleLength to)
            where T : class => FromTo(provider, target, (Variant)from, (Variant)to, (uint)0);

        public static Tween FromTo<T>(StyleScaleProvider<T> provider, object target, StyleScale from, StyleScale to)
            where T : class => FromTo(provider, target, (Variant)from, (Variant)to, (uint)0);

        public static Tween FromTo<T>(StyleColorProvider<T> provider, object target, StyleColor from, StyleColor to,
            ColorOptions options) where T : class =>
            FromTo(provider, target, (Variant)from, (Variant)to, (uint)options);

        public static Tween FromTo<T>(StyleRotateProvider<T> provider, object target, StyleRotate from, StyleRotate to)
            where T : class => FromTo(provider, target, (Variant)from, (Variant)to, (uint)0);

        public static Tween FromTo<T>(StyleTranslateProvider<T> provider, object target, StyleTranslate from,
            StyleTranslate to) where T : class => FromTo(provider, target, (Variant)from, (Variant)to, (uint)0);

        /// <summary>
        /// Call to completely reset the tween system by stopping all running tweens, clearing all the caches, and stopping the updater
        /// </summary>
        public static void Reset() => TweenContext.Reset();

        /// <summary>
        /// Create a tween that just waits for a given amount of time and then finishes.
        /// </summary>
        /// <param name="target">Target object to attach tween to</param>
        /// <param name="duration">Options duration of the Wait</param>
        public static Tween Wait(object target, float duration) =>
            AllocTween(null, target, Vector4.zero, Vector4.zero).Duration(duration);

        /// <summary>
        /// Create a tween that runs a sequence of child tweens in order.  Use the Element method to 
        /// add child tweens to the sequence.  
        /// 
        /// Note Duration cannot be called on a sequence as the duration is automatically calculated from
        /// the total of all child elements.
        /// </summary>
        /// <param name="target">Target object to attach tween to</param>
        public static Tween Sequence(object target) => AllocTween(null, target, Vector4.zero, Vector4.zero,
            TweenContext.Flags.Collection | TweenContext.Flags.Sequence);

        /// <summary>
        /// Create a tween that runs a group of elements in parallel.  Use the Element method to
        /// add child tweens to the group.
        /// 
        /// Note Duration cannot be called on a sequence as the duration is automatically calculated from
        /// the total of all child elements.
        /// </summary>
        /// <param name="target">Target object to attach tween to</param>
        public static Tween Group(object target) =>
            AllocTween(null, target, Vector4.zero, Vector4.zero, TweenContext.Flags.Collection);

        /// <summary>
        /// Stop all tweens running on a target object
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="id">Optional identifier to filter by (0 = no identifier)</param>
        /// <param name="executeCallbacks">True if any remaining callbacks such as OnStop should be called (Default true)</param>
        public static void Stop(object target, int id = 0, bool executeCallbacks = true)
        {
            if (null == target)
                return;

            TweenContext.Stop(target: target, id: id, executeCallbacks: executeCallbacks);
        }

        /// <summary>
        /// Stop all tweens that match the given identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="executeCallbacks">True if callbacks such as OnStop should still be called</param>
        public static void Stop(int id, bool executeCallbacks = true)
        {
            if (id == 0)
                return;

            TweenContext.Stop(executeCallbacks: executeCallbacks, id: id);
        }

        /// <summary>
        /// Stop all playing tweens
        /// </summary>
        /// <param name="executeCallbacks"></param>
        public static void StopAll(bool executeCallbacks = true)
        {
            TweenContext.Stop(executeCallbacks: executeCallbacks);
        }

        /// <summary>
        /// Start the tween playing.  Note that once a tween begins playing it can no longer
        /// me modified.
        /// </summary>
        public Tween Play()
        {
            if (!isValid)
                throw new InvalidOperationException("Invalid tween");

            if (_context.isElement)
                throw new InvalidOperationException(
                    "Play cannot be called on tweens that are part of a collection, instead call Play on the collection");

            _context.Play();

            return this;
        }

        /// <summary>
        /// Stop the tween
        /// </summary>
        /// <param name="executeCallbacks">True to execute any remaining callbacks such as OnStop</param>
        public void Stop(bool executeCallbacks = true)
        {
            if (!isValid)
                return;

            if (_context.isElement)
                throw new System.InvalidOperationException(
                    "Stop cannot be called on tweens that are elements in a collection, instead call Stop on the collection");

            _context.Free(executeCallbacks);
        }

        /// <summary>
        /// Pause the tween until either Play is called again or the tween is stopped
        /// </summary>
        public void Pause()
        {
            // TODO: should we throw an exception here?
            if (!isValid)
                return;

            if (_context.isElement)
                throw new InvalidOperationException(
                    "Pause cannot be called on tweens that are part of a collection, instead call Pause on the collection");

            _context.Pause();
        }

        /// <summary>
        /// Manually update the tween by adding the given amount of delta time
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns>True if the tween is still running</returns>
        public bool Update(float deltaTime)
        {
            if (!isValid)
                return false;

            // Update is only allowed with Manual update mode
            if (!_context.isManual)
                throw new InvalidOperationException("Update can only be called on tweens with the Manual update mode.");

            _context.Update(deltaTime);

            return isPlaying;
        }

        /// <summary>
        /// Sets a user defined value that indicates the priority of the tween.  This priority is
        /// then used in Tween.Frame to allow the application to determine the max priority being run
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public Tween Priority(int priority)
        {
            ValidateModifierContext()._priority = priority;
            return this;
        }

        /// <summary>
        /// Amount of time to wait before starting the tween.  
        /// 
        /// Note that the OnStart method will not be called until after the delay.
        /// 
        /// When Delay is used on a looping tween the delay is only applied once and not on each loop.
        /// </summary>
        /// <param name="seconds">Delay in seconds</param>
        public Tween Delay(float seconds)
        {
            if (seconds < 0.0f)
                throw new InvalidOperationException("Tween delay must be greater or equal to zero");

            ValidateModifierContext().delay = seconds;
            return this;
        }

        /// <summary>
        /// Add a number of seconds to the existing delay amount.  If the value added decreases
        /// the delay below zero the it will be set to zero.
        /// </summary>
        public Tween AddDelay(float seconds)
        {
            ValidateModifierContext().delay = Mathf.Max(0, _context.delay + seconds);
            return this;
        }

        /// <summary>
        /// Set the duration in seconds of the tween.
        /// 
        /// Using the Duration modifier on either a Sequence or a Group will cause the duration of 
        /// all elements to be overridden with the given duration.
        /// </summary>
        /// <param name="seconds">Duration in seconds</param>
        public Tween Duration(float seconds)
        {
            if (seconds <= 0.0f)
                throw new InvalidOperationException("Tween duration must be greater than zero");

            ValidateModifierContext();

            if (_context.isCollection)
                throw new InvalidOperationException("Duration cannot be called on a Group or Sequence");

            _context.duration = seconds;
            return this;
        }

        /// <summary>
        /// Callback to invoke when the Tween starts playing and the delay is finished
        /// </summary>
        /// <param name="callback">Action</param>
        public Tween OnPlay(Action callback)
        {
            ValidateModifierContext().onPlay = callback;
            return this;
        }

        /// <summary>
        /// Callback to invoke when the Tween is stopped
        /// 
        /// Note that the OnStop callback may not be called if the tween is stopped using 
        /// an executeCallbacks value of false.
        /// </summary>
        /// <param name="callback">Action</param>
        public Tween OnStop(Action callback)
        {
            ValidateModifierContext().onStop = callback;
            return this;
        }

        /// <summary>
        /// Set the tween update mode.
        /// </summary>
        /// <param name="value">True to run on fixed update, false otherwise</param>
        public Tween UpdateMode(UpdateMode mode)
        {
            ValidateModifierContext().updateMode = mode;
            return this;
        }

        /// <summary>
        /// Set a unique identifier for the tween. Setting a unique identifier value of zero indicates
        /// that a tween has no identifier.
        /// </summary>
        /// <param name="id">Unique identier</param>
        public Tween Id(int id)
        {
            ValidateModifierContext().id = id;
            return this;
        }

        /// <summary>
        /// Set the tween to automatically destroy the GameObject attached to the target when the Tween stops
        /// </summary>
        public Tween DestroyOnStop(bool value = true)
        {
            ValidateModifierContext().destroyOnStop = value;
            return this;
        }

        /// <summary>
        /// Set the tween to automatically deactivate the GameObject attached to the target when the Tween stops
        /// </summary>
        public Tween DeactivateOnStop(bool value = true)
        {
            ValidateModifierContext().deactivateOnStop = value;
            return this;
        }

        /// <summary>
        /// Automatically disable the target Component when the Tween stops
        /// </summary>
        /// <param name="disable">True to enable, false to disable</param>
        /// <returns></returns>
        public Tween DisableOnStop(bool value = true)
        {
            ValidateModifierContext().disableOnStop = value;
            return this;
        }

        /// <summary>
        /// Whether or not the tween should automatically stop itself when the <see cref="Tween.time"/> is greater
        /// or equal to the <see cref="Tween.duration"/>
        /// 
        /// Note that a tween set to not auto stop will never end until manually stopped.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Tween AutoStop(bool value = true)
        {
            ValidateModifierContext().autoStop = value;
            return this;
        }

        /// <summary>
        /// Enables or disables PingPong mode.  When PingPong mode is enabled the animation
        /// will play itself fully forward and then then in reverse before stopping.  This means that 
        /// the effective duration of the animation will be doubled. Note that everything is run in reverse 
        /// including easing modes.
        /// </summary>
        /// <param name="value">True to enable PingPong mode.</param>
        public Tween PingPong(bool value = true)
        {
            ValidateModifierContext();

            if (_context.isCollection)
                throw new InvalidOperationException(
                    "PingPong is not supported on group or sequence tweens, instead set PingPong on individual elements");

            _context.pingPong = value;
            return this;
        }

        /// <summary>
        /// Switch the tween to start the configured 'from' value and end at the current value
        /// </summary>
        /// <returns></returns>
        public Tween From()
        {
            ValidateModifierContext();

            if (_context.isCollection)
                throw new InvalidOperationException("From is not supported on Group or Sequence tweens");

            _context.hasFrom = true;
            _context.hasTo = false;
            return this;
        }

        /// <summary>
        /// Loop the tween
        /// </summary>
        /// <param name="count">Number of loops or -1 to loop forever</param>
        public Tween Loop(int count = -1)
        {
            ValidateModifierContext();

            _context.looping = true;
            _context.loopCount = Math.Max(-1, count);
            return this;
        }

        /// <summary>
        /// Sets the animation to use unscaled time rather than normal scaled time
        /// </summary>
        /// <param name="unscaled">True to use unscaled time</param>
        public Tween UnscaledTime(bool unscaled = true)
        {
            ValidateModifierContext().unscaledTime = unscaled;
            return this;
        }

        /// <summary>
        /// Adds an element to a Sequext or Group tween
        /// </summary>
        /// <param name="element">element to add</param>
        public Tween Element(Tween element)
        {
            ValidateModifierContext();

            if (!element.isValid)
                throw new InvalidOperationException("Invalid tween specified for Element modifier");

            if (!_context.isCollection)
                throw new InvalidOperationException("Element modifier can only be called on a Sequence or Group");

            var elementContext = element._context;
            if (!elementContext.isCreated)
                throw new InvalidOperationException("Element modifier must be given an Unstarted tween");

            if (elementContext.isElement)
                throw new InvalidOperationException(
                    "Tween specified for Element modifier is already an element of another Tween");

            if (elementContext.looping)
                throw new InvalidOperationException("Loop is not supported on Element tweens");

            elementContext.SetParent(_context);

            return this;
        }

        public Tween EaseIn(EaseDelegate easeDelegate) => EaseIn(easeDelegate, Vector4.zero);

        /// <summary>
        /// Ease in using the given easing function
        /// </summary>
        /// <param name="easeDelegate">Delegate to use for easing int</param>
        public Tween EaseIn(EaseDelegate easeDelegate, Vector4 param)
        {
            ValidateModifierContext();

            if (_context.isCollection)
                throw new InvalidOperationException(
                    "Easing is not supported on group or sequence tweens, instead set easing on individual elements");

            _context.EaseIn(easeDelegate, param);
            return this;
        }

        public Tween EaseOut(EaseDelegate easeDelegate) => EaseOut(easeDelegate, Vector4.zero);

        /// <summary>
        /// Ease out using the given easing function
        /// </summary>
        /// <param name="easeDelegate">Delegate to use for easing out</param>
        /// <param name="easeParams">Optional easing paramters</param>
        public Tween EaseOut(EaseDelegate easeDelegate, Vector4 easeParams)
        {
            ValidateModifierContext();

            if (_context.isCollection)
                throw new InvalidOperationException(
                    "Easing is not supported on group or sequence tweens, instead set easing on individual elements");

            _context.EaseOut(easeDelegate, easeParams);
            return this;
        }

        /// <summary>
        /// Ease in using Quadratic interpolation
        /// </summary>
        public Tween EaseInQuadratic() => EaseIn(_easeQuadraticDelegate);

        /// <summary>
        /// Ease Out using Quadratic interpolation
        /// </summary>
        public Tween EaseOutQuadratic() => EaseOut(_easeQuadraticDelegate);

        /// <summary>
        /// Ease in and out using Quadratic interpolation
        /// </summary>
        public Tween EaseInOutQuadratic() => EaseIn(_easeQuadraticDelegate).EaseOut(_easeQuadraticDelegate);

        /// <summary>
        /// Ease in using Cubic interpolation
        /// </summary>
        public Tween EaseInCubic() => EaseIn(_easeCubicDelegate);

        /// <summary>
        /// Ease Out using Cubic interpolation
        /// </summary>
        public Tween EaseOutCubic() => EaseOut(_easeCubicDelegate);

        /// <summary>
        /// Ease in and out using Cubic interpolation
        /// </summary>
        public Tween EaseInOutCubic() => EaseIn(_easeCubicDelegate).EaseOut(_easeCubicDelegate);

        public Tween EaseInBack(float amplitude = 1f) => EaseIn(_easeBackDelegate, new Vector4(amplitude, 0, 0, 0));
        public Tween EaseOutBack(float amplitude = 1f) => EaseOut(_easeBackDelegate, new Vector4(amplitude, 0, 0, 0));

        public Tween EaseInOutBack(float amplitude = 1f) => EaseIn(_easeBackDelegate, new Vector4(amplitude, 0, 0, 0))
            .EaseOut(_easeBackDelegate, new Vector4(amplitude, 0, 0, 0));

        public Tween EaseInElastic(int oscillations = 3, float springiness = 3f) =>
            EaseIn(_easeElasticDelegate, new Vector4(oscillations, springiness, 0, 0));

        public Tween EaseOutElastic(int oscillations = 3, float springiness = 3f) =>
            EaseOut(_easeElasticDelegate, new Vector4(oscillations, springiness, 0, 0));

        public Tween EaseInOutElastic(int oscillations = 3, float springiness = 3f) =>
            EaseIn(_easeElasticDelegate, new Vector4(oscillations, springiness, 0, 0))
                .EaseOut(_easeElasticDelegate, new Vector4(oscillations, springiness, 0, 0));

        public Tween EaseInBounce(int oscillations = 3, float springiness = 2f) =>
            EaseIn(_easeBounceDelegate, new Vector4(oscillations, springiness, 0, 0));

        public Tween EaseOutBounce(int oscillations = 3, float springiness = 2f) =>
            EaseOut(_easeBounceDelegate, new Vector4(oscillations, springiness, 0, 0));

        public Tween EaseInOutBounce(int oscillations = 3, float springiness = 2f) =>
            EaseIn(_easeBounceDelegate, new Vector4(oscillations, springiness, 0, 0))
                .EaseOut(_easeBounceDelegate, new Vector4(oscillations, springiness, 0, 0));

        public Tween EaseInSine() => EaseIn(_easeSineDelegate);
        public Tween EaseOutSine() => EaseOut(_easeSineDelegate);
        public Tween EaseInOutSine() => EaseIn(_easeSineDelegate).EaseOut(_easeSineDelegate);

        public Tween EaseInCircle() => EaseIn(_easeCircleDelegate);
        public Tween EaseOutCircle() => EaseOut(_easeCircleDelegate);
        public Tween EaseInOutCircle() => EaseIn(_easeCircleDelegate).EaseOut(_easeCircleDelegate);

        public Tween EaseInExponential(float exponent = 2.0f) =>
            EaseIn(_easeExponential, new Vector4(exponent, 0, 0, 0));

        public Tween EaseOutExponential(float exponent = 2.0f) =>
            EaseOut(_easeExponential, new Vector4(exponent, 0, 0, 0));

        public Tween EaseInOutExponential(float exponent = 2.0f) =>
            EaseIn(_easeExponential, new Vector4(exponent, 0, 0, 0))
                .EaseOut(_easeExponential, new Vector4(exponent, 0, 0, 0));

        public Tween EaseInCubicBezier(float p0, float p1, float p2, float p3) =>
            EaseIn(_easeCubicBezier, new Vector4(p0, p1, p2, p3));

        public Tween EaseOutCubicBezier(float p0, float p1, float p2, float p3) =>
            EaseOut(_easeCubicBezier, new Vector4(p0, p1, p2, p3));

        public Tween EaseInOutCubicBezier(float p0, float p1, float p2, float p3) =>
            EaseIn(_easeCubicBezier, new Vector4(p0, p1, p2, p3))
                .EaseOut(_easeExponential, new Vector4(p0, p1, p2, p3));


        private TweenContext ValidateModifierContext()
        {
            if (!isValid)
                throw new InvalidOperationException("Invalid Tween");

            if (!_context.isCreated)
                throw new InvalidOperationException("Tween modifiers must be applied before calling Play");

            return _context;
        }

        /// <summary>
        /// Internal method used to allocate a pooled Tween
        /// </summary>
        /// <returns>Allocated Context</returns>
        private static Tween AllocTween(TweenProvider provider, object target, Variant from, Variant to,
            TweenContext.Flags flags = TweenContext.Flags.None, uint providerOptions = 0)
        {
            var context = TweenContext.Alloc(provider, target, from, to, flags, providerOptions);
            return new Tween { _context = context, _instanceId = context.instanceId };
        }

        internal static void CallFrame(int maxPriority)
        {
            Frame?.Invoke(maxPriority);
        }
    }
}
