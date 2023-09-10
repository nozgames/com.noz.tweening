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
using System.Collections.Generic;
using UnityEngine;

namespace NoZ.Tweening.Internals
{
    internal class TweenContext
    {
        private static int _maxPriority = 0;
        
        private class Updater : MonoBehaviour
        {
            private void Update()
            {
                _maxPriority = int.MinValue;
                TweenContext.Update(UpdateMode.Default, Time.deltaTime, Time.unscaledDeltaTime);   
            }
            
            private void FixedUpdate() => TweenContext.Update(UpdateMode.Fixed, Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);

            private void LateUpdate()
            {
                TweenContext.Update(UpdateMode.Late, Time.deltaTime, Time.unscaledDeltaTime);
                
                if (Tween.IsAnyTweenAnimating && _maxPriority != int.MinValue)
                    Tween.CallFrame(_maxPriority);
            }
            
            private void OnApplicationQuit()
            {
                Tween.StopAll(false);                
                Destroy(gameObject);
            }
        }

        public enum State : byte
        {
            Free,
            Created,
            Playing,
            Paused,
            Manual,
            Element,
            Count
        }

        [Flags]
        internal enum Flags : ushort
        {
            None = 0,

            /// <summary>From value has been set</summary>
            From = 1 << 1,

            /// <summary>To has been set</summary>
            To = 1 << 2,

            /// <summary>Tween is a collection (group or sequence)</summary>
            Collection = 1 << 3,

            /// <summary>Tween is a child element of a collection</summary>
            Element = 1 << 4,

            /// <summary>Collection is a sequence</summary>
            Sequence = 1 << 5,

            /// <summary>Tween will automatically stop when the time has been exceeded</summary>
            AutoStop = 1 << 6,

            /// <summary>Delta time should be unscaled</summary>
            UnscaledTime = 1 << 7,

            /// <summary>Tween should automatically loop</summary>
            Looping = 1 << 8,

            /// <summary>Play forward for the first half of the duration to the end, and in reverse for the second half to the start</summary>
            PingPong = 1 << 9,

            /// <summary>Automatically destroy the target GameObject when the tween completes</summary>
            DestroyOnStop = 1 << 10,

            /// <summary>Automatically deactivate the target GameObject when the tween completes</summary>
            DeactivateOnStop = 1 << 11,

            /// <summary>Automatically disable the target MonoBehavior when the tween completes</summary>
            DisableOnStop = 1 << 12,

            /// <summary>True when the elapsed time is between delay and delay + duration</summary>
            Evaluating = 1 << 13
        }

        private uint _instanceId;
        private object _target;
        private TweenProvider _provider;
        private uint _providerOptions;
        private int _id;
        private Flags _flags;
        private State _state;
        private UpdateMode _updateMode;
        private Variant _from;
        private Variant _to;
        private EaseDelegate _easeIn;
        private EaseDelegate _easeOut;
        private Vector4 _easeInParams;
        private Vector4 _easeOutParams;
        private Action _onStop;
        private Action _onPlay;
        private Action _onPause;
        private int _loopCount;
        private float _duration;
        private float _delay;
        private float _elapsed;
        private LinkedListNode<TweenContext> _stateNode;
        private LinkedListNode<TweenContext> _elementsHead;
        private LinkedListNode<TweenContext> _elementsTail;

        internal int _priority;

        /// <summary>List of contexts per state</summary>
        internal static LinkedList<TweenContext>[] _stateContexts;

        /// <summary>Temporary list used to process contexts</summary>
        internal static List<TweenContext> _tempContexts;

        /// <summary>MonoBehaviour used to update tweens</summary>
        private static Updater _updater;

        /// <summary>Next instanced identifier that will be assigned to a tween</summary>
        private static uint _nextInstanceId = 1;

        static TweenContext()
        {
            // Allocate a linked list for each of the states
            _stateContexts = new LinkedList<TweenContext>[(int)State.Count];
            for (int i = (int)State.Count - 1; i >= 0; i--)
                _stateContexts[i] = new LinkedList<TweenContext>();

            _tempContexts = new List<TweenContext>(128);
        }

        public static int GetStateCount(State state) => _stateContexts[(int)state].Count;

        public bool isPlaying => _state == State.Playing;
        public bool isPaused => _state == State.Paused;
        public bool isManual => _state == State.Manual;
        public bool isFree => _state == State.Free;
        public bool isCreated => _state == State.Created;
        public bool isElement => _state == State.Element;

        public bool isCollection => HasFlags(Flags.Collection);
        public bool isGroup => HasFlags(Flags.Collection) && !HasFlags(Flags.Sequence);
        public bool isSequence => HasFlags(Flags.Collection|Flags.Sequence);
        public bool autoStop { get => HasFlags(Flags.AutoStop); set => SetFlags(Flags.AutoStop, value); }
        public bool pingPong { get => HasFlags(Flags.PingPong); set => SetFlags(Flags.PingPong, value); }

        public bool destroyOnStop { get => HasFlags(Flags.DestroyOnStop); set => SetFlags(Flags.DestroyOnStop, value); }
        public bool deactivateOnStop { get => HasFlags(Flags.DeactivateOnStop); set => SetFlags(Flags.DeactivateOnStop, value); }
        public bool disableOnStop { get => HasFlags(Flags.DisableOnStop); set => SetFlags(Flags.DisableOnStop, value); }

        public Action onPlay { get => _onPlay; set => _onPlay = value; }
        public Action onPause { get => _onPause; set => _onPause= value; }
        public Action onStop { get => _onStop; set => _onStop = value; }

        public bool hasFrom { get => HasFlags(Flags.From); set => SetFlags(Flags.From, value); }
        public bool hasTo { get => HasFlags(Flags.To); set => SetFlags(Flags.To, value); }
        public bool looping { get => HasFlags(Flags.Looping); set => SetFlags(Flags.Looping, value); }
        public bool unscaledTime { get => HasFlags(Flags.UnscaledTime); set => SetFlags(Flags.UnscaledTime, value); }
        public int loopCount { get => _loopCount; set => _loopCount = value; }
        public float delay { get => _delay; set => _delay = value; }
        public float duration { get => _duration; set => _duration = value; }
        public int id { get => _id; set => _id = value; }
        public UpdateMode updateMode { get => _updateMode; set => _updateMode = value; }

        public uint instanceId => _instanceId;
        public float elapsed => _elapsed;

        public State state
        {
            get => _state;
            private set
            {
                if (_state == value || (_state == State.Element && value != State.Free))
                    return;

                _state = value;
                _stateNode.List.Remove(_stateNode);
                _stateContexts[(int)_state].AddLast(_stateNode);
            }
        }

        // Prevent external allocation
        private TweenContext() { }

        private bool HasFlags(Flags flags) => (_flags & flags) == flags;

        private void SetFlags(Flags flags, bool value=true)
        {
            if (value)
                _flags |= flags;
            else
                ClearFlags(flags);
        }

        private void ClearFlags(Flags flags) => _flags &= ~(flags);

        public static void Reset()
        {
            // Stop all running tweens
            Stop(executeCallbacks: false);

            // Clear the free cache which will throw all of the cached tweens into the garbage
            _stateContexts[(int)State.Free].Clear();

            if(_updater != null)
            {
                UnityEngine.Object.Destroy(_updater.gameObject);
                _updater = null;
            }
        }

        public void Play()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
#endif

            if (isElement)
                return;

            // Start the updater GameObject if needed
            if (_updater == null)
            {
                var updaterGameObject = new GameObject("TweenZ Updater");
                updaterGameObject.hideFlags = HideFlags.HideAndDontSave;
                UnityEngine.Object.DontDestroyOnLoad(updaterGameObject);
                _updater = updaterGameObject.AddComponent<Updater>();
            }

            // Already playing or stopped?
            if (isPlaying || isFree)
                return;

            // Set the state based on the update mode
            var oldState = state;
            state = _updateMode == UpdateMode.Manual? State.Manual : State.Playing;

            // Handle the first time play is called
            if (oldState == State.Created)
            {
                _elapsed = 0.0f;
                CalculateDuration();
            }

            // Force OnPlay to be called again by clearing the evaluating flag
            ClearFlags(Flags.Evaluating);              

            Evaluate(_elapsed);
        }

        public void Pause()
        {
            if (!isPlaying)
                return;

            if(HasFlags(Flags.Evaluating))
                _onPause?.Invoke();

            state = State.Paused;
        }

        /// <summary>
        /// Recursively calculate the total duration of a tween
        /// </summary>
        /// <returns>Duration in seconds</returns>
        private float CalculateDuration()
        {
            if (isSequence)
            {
                var delay = 0.0f;
                _duration = 0.0f;
                for (var element = _elementsHead; element != null && element.Previous != _elementsTail; element = element.Next)
                {
                    var duration = element.Value.CalculateDuration();
                    _duration += duration;
                    element.Value._delay += delay;
                    delay += duration;
                }
            }
            else if (isGroup)
            {
                _duration = 0.0f;
                for (var element = _elementsHead; element != null && element.Previous != _elementsTail; element = element.Next)
                    _duration = Mathf.Max(duration, element.Value.CalculateDuration());
            }

            return _duration + _delay;
        }

        public void Update (float deltaTime) => Evaluate(_elapsed + deltaTime);
        
        private void Restart ()
        {
            if(HasFlags(Flags.Evaluating))
                _onStop?.Invoke();

            ClearFlags(Flags.Evaluating);
            _elapsed = 0.0f;

            // Restart all children as well
            for (var element = _elementsHead; element != null && element.Previous != _elementsTail; element = element.Next)
                element.Value.Restart();
        }

        /// <summary>
        /// Evalulate the tween by calculating the current value given the time in seconds.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        private void Evaluate(float time)
        {
            if (time < _elapsed) return;

            // If the target is null it is likely a UnityObject that was destroyed.  In this 
            // case we will stop the tween and not call its callbacks as this is an unclean case.
            if ((_target is UnityEngine.Object unityTarget) && unityTarget == null)
            {
                if (!isElement) Free(false);
                return;
            }

            // Clamp the time to our timeline
            var oldTime = _elapsed;
            var clampedTime = Mathf.Clamp(time, 0, _delay + _duration);

            _elapsed = clampedTime;

            // Not finished with delay?
            if (clampedTime < _delay)
                return;

            // First time being evaluated post delay?
            if (!HasFlags(Flags.Evaluating))
            {
                SetFlags(Flags.Evaluating);
                onPlay?.Invoke();
            }
   
            // Provider
            if (_provider != null)
            {
                if (!hasFrom)
                {
                    hasFrom = true;
                    _from = _provider.GetValue(_target, _providerOptions);
                }

                if (!hasTo)
                {
                    hasTo = true;
                    _to = _provider.GetValue(_target, _providerOptions);
                }

                float normalizedTime = (clampedTime - _delay) / _duration;

                // Ping pong
                if (pingPong)
                {
                    if (normalizedTime >= 0.5f)
                        normalizedTime = 1.0f - ((normalizedTime - 0.5f) / 0.5f);
                    else
                        normalizedTime = normalizedTime / 0.5f;
                }

                // Ease In / Out
                if (_easeIn != null && _easeOut != null)
                {
                    if (normalizedTime <= 0.5f)
                        normalizedTime = _easeIn(normalizedTime * 2f, _easeInParams) * 0.5f;
                    else if (normalizedTime > 0.5f)
                        normalizedTime = (1f - _easeOut((1f - normalizedTime) * 2f, _easeOutParams)) * 0.5f + 0.5f;
                }
                // Ease In
                else if (_easeIn != null)
                    normalizedTime = _easeIn(normalizedTime, _easeInParams);
                // Ease Out
                else if (_easeOut != null)
                    normalizedTime = 1f - _easeOut(1f - normalizedTime, _easeOutParams);

                _provider.SetValue(_target, _provider.Evalulate(_from, _to, normalizedTime, _providerOptions), _providerOptions);
            }

            // For collections we need to recursively evaluate their elements as well
            else if (isCollection)
            {
                var elementTime = clampedTime - _delay;
                for (var element = _elementsHead; element != null && element.Previous != _elementsTail; element = element.Next)
                    element.Value.Evaluate(elementTime);
            }

            // Not done?
            if (time < _duration + _delay)
                return;

            // Loop?
            if (looping)
            {
                // Clear the delay after looping
                _delay = 0f;

                // Loop count
                if (_loopCount > 0)
                {
                    _loopCount--;
                    if (_loopCount == 0)
                        looping = false;
                }

                // Restart time at zero
                Restart();

                // Evaluate using any extra time we had
                Evaluate(time - clampedTime);
                return;
            }

            // When a non-element is done and has auto stop enabled free it
            if (!isElement && autoStop)
                Free();
            // When an element stops just mark it as stopped
            else if (isElement)
            {
                ClearFlags(Flags.Evaluating);
                _onStop?.Invoke();
            }
        }

        public void EaseIn(EaseDelegate easeDelegate) => EaseIn(easeDelegate, Vector4.zero);
        public void EaseIn(EaseDelegate easeDelegate, Vector4 easeParams)
        {
            _easeIn = easeDelegate;
            _easeInParams = easeParams;
        }

        public void EaseOut(EaseDelegate easeDelegate) => EaseOut(easeDelegate, Vector4.zero);
        public void EaseOut(EaseDelegate easeDelegate, Vector4 easeParams)
        {
            _easeOut = easeDelegate;
            _easeOutParams = easeParams;
        }

        public void SetParent (TweenContext parent)
        {
            _state = State.Element;
            _stateNode.List.Remove(_stateNode);

            if (parent._elementsHead == null)
            {
                parent._elementsHead = _stateNode;
                _stateContexts[(int)State.Element].AddLast(_stateNode);
            }
            else
            {
                _stateContexts[(int)State.Element].AddAfter(parent._elementsTail, _stateNode);
            }

            parent._elementsTail = _stateNode;

            // All elemens must be auto stop
            SetFlags(Flags.AutoStop);
        }

        internal static void Update (UpdateMode updateMode, float deltaTime, float unscaledDeltaTime)
        {
            // Move all of the playing nodes that match the update mode into the update list.  We do
            // this because it is possible for contexts's to switch states during the update and 
            // the original list would be modified.
            var tempContextStart = _tempContexts.Count;
            var contexts = _stateContexts[(int)State.Playing];
            for (var node = contexts.First; node != null; node = node.Next)
                if(node.Value._updateMode == updateMode)
                    _tempContexts.Add(node.Value);

            var tempContextEnd = _tempContexts.Count;
            for(int tempContextIndex=tempContextStart;  tempContextIndex < tempContextEnd; tempContextIndex++)
            {
                // It is possible the state changed during the update, if so just skip it
                var context = _tempContexts[tempContextIndex];
                if (context._state == State.Playing)
                {
                    _maxPriority = Mathf.Max(context._priority, _maxPriority);
                    context.Update(context.HasFlags(Flags.UnscaledTime) ? unscaledDeltaTime : deltaTime);
                }
            }

            _tempContexts.RemoveRange(tempContextStart, tempContextEnd - tempContextStart);

            // Stop any global tweens that were orphaned by creating them without calling Play
            if (updateMode == UpdateMode.Late)
            {
                for (var node = _stateContexts[(int)State.Created].First; node != null; node = node.Next)
                    node.Value.Free();
            }
        }

        public static TweenContext Alloc (TweenProvider provider, object target, Variant from, Variant to, Flags flags = Flags.None, uint providerOptions = 0)
        {
            // Pooled tweens
            TweenContext context;
            var freeContexts = _stateContexts[(int)State.Free];
            if (freeContexts.Count > 0)
            {
                context = freeContexts.First.Value;
                context.state = State.Created;
            }
            else
            {
                context = new TweenContext { _state = State.Created };
                context._stateNode = new LinkedListNode<TweenContext>(context);
                _stateContexts[(int)State.Created].AddLast(context._stateNode);
            }

            // Initialize the tween
            context._instanceId = _nextInstanceId++;
            context._delay = 0f;
            context._flags = flags | Flags.AutoStop;
            context._elapsed = 0f;
            context._duration = 1f;
            context._from = from;
            context._to = to;
            context._provider = provider;
            context._target = target;
            context._priority = 0;
            context._providerOptions = providerOptions;
            context._updateMode = UpdateMode.Default;

            return context;
        }

        public void Free(bool executeCallbacks = true)
        {
            if (_state == State.Free)
                return;

            // Free all child elements 
            LinkedListNode<TweenContext> next;
            for (var element = _elementsHead; element != null; element = next)
            {
                next = element.Next;
                element.Value.Free(executeCallbacks);
                if (element == _elementsTail)
                    break;
            }

            // Before we free the tween save off some information we need after
            var onStop = executeCallbacks ? _onStop : null;
            var target = _target;
            var destroyOnStop = HasFlags(Flags.DestroyOnStop);
            var deactivateOnStop = HasFlags(Flags.DeactivateOnStop);
            var disableOnStop = HasFlags(Flags.DisableOnStop);

            // Clear all references
            _flags = Flags.None;
            _id = 0;
            _onStop = null;
            _onPlay = null;
            _onPause = null;
            _target = null;
            _provider = null;
            _easeIn = null;
            _easeOut = null;
            _elementsHead = null;
            _elementsTail = null;

            // Move the tween to the free state so it can be reclaimed.
            state = State.Free;

            onStop?.Invoke();

            if (destroyOnStop || deactivateOnStop || disableOnStop)
            {
                var component = target as Component;
                var gameObject = component != null ? component.gameObject : target as GameObject;

                if (deactivateOnStop && gameObject != null)
                    gameObject.SetActive(false);

                if (disableOnStop && component != null && component is MonoBehaviour monoBehaviour)
                    monoBehaviour.enabled = false;

                if (destroyOnStop && gameObject != null)
                    UnityEngine.Object.Destroy(gameObject);
            }
        }

        /// <summary>
        /// Stop all playing contexts that match the given filters
        /// </summary>
        public static void Stop (int id=0, object target=null, bool executeCallbacks=true)
        {
            // Add all nodes that match the given filter to the temp contexts list to handle
            // any list updates that will occur when Free is called
            var tempContextStart = _tempContexts.Count;
            for(int i=(int)State.Playing; i<(int)State.Element; i++)
            {
                for(var node = _stateContexts[i].First; node != null; node = node.Next)
                {
                    if (id != 0 && node.Value._id != id) continue;
                    if (target != null && node.Value._target != target) continue;

                    _tempContexts.Add(node.Value);
                }
            }

            // Process the filtered list.
            var tempContextEnd = _tempContexts.Count;
            for (int tempContextIndex = tempContextStart; tempContextIndex < tempContextEnd; tempContextIndex++)
                _tempContexts[tempContextIndex].Free(executeCallbacks);

            // Remove our temporary contexts
            _tempContexts.RemoveRange(tempContextStart, tempContextEnd - tempContextStart);
        }
    }
}

