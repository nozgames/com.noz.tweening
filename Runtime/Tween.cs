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

using System.Collections.Generic;
using UnityEngine;

namespace NoZ.Tweenz
{
    public partial struct Tween
    {
        private uint _instanceId;
        private Context _context;

        private static uint _nextInstanceId = 1;
        private static LinkedList<Context> _activeContexts = new LinkedList<Context>();
        private static LinkedList<Context> _freeContexts = new LinkedList<Context>();
        private static LinkedList<Context> _stoppingContexts = new LinkedList<Context>();
        private static Updater _updater;

        [System.Flags]
        internal enum Flags : uint
        {
            None = 0,

            /// <summary>
            /// Indicates the tween should process the children as a sequence rather than a group
            /// </summary>
            Sequence = (1 << 0),

            /// <summary>
            /// Delta time should be unscaled
            /// </summary>
            UnscaledTime = (1 << 1),

            /// <summary>
            /// Loop the tween
            /// </summary>
            Loop = (1 << 2),

            /// <summary>
            /// Indicates the tween has been started 
            /// </summary>
            Started = (1 << 3),

            /// <summary>
            /// Indicates the tween should play itself forward, then backward before stopping
            /// </summary>
            PingPong = (1 << 4),

            /// <summary>
            /// Tween is active
            /// </summary>
            Active = (1 << 6),

            /// <summary>
            /// Tween is stopping
            /// </summary>
            Stopping = (1 << 7),

            /// <summary>
            /// Automatically destroy the target object when the tween completes
            /// </summary>
            DestroyOnStop = (1 << 8),

            /// <summary>
            /// Automatically deactivate the target object when the tween completes
            /// </summary>
            DeactivateOnStop = (1 << 9),

            Free = (1 << 10),

            /// <summary>
            /// Indicates that the `from` value of the tween was set
            /// </summary>
            From = 1 << 11,

            /// <summary>
            /// Indicates that the `to` value of the tween was set
            /// </summary>
            To = 1 << 12,

            /// <summary>
            /// Indicates that callbacks should be ignores on this tween.  This generally
            /// gets set when a tween is stopped and the callbacks are disabled.
            /// </summary>
            NoCallbacks = (1 << 13),

            /// <summary>
            /// Indicates that the tween is a collection of child tweens
            /// </summary>
            Collection = 1 << 14,

            /// <summary>
            /// Tween is a child element of a collection
            /// </summary>
            Element = 1 << 15,

            /// <summary>
            /// Tween is running backwards.  This is used in PingPong on the second half
            /// of the ping pong.
            /// </summary>
            Reverse = 1 << 16,

            DisableOnStop = 1 << 18,

            /// <summary>
            /// True when the tween is playing and the time should automatically be advanced
            /// </summary>
            Playing = 1 << 19,
        }

        private class Context
        {
            public uint instanceId;
            public object target;
            public TweenProvider provider;
            public string key;
            public Flags flags;
            public Variant from;
            public Variant to;
            public Variant current;
            public uint options;
            public EaseDelegate easeIn;
            public EaseDelegate easeOut;
            public Vector2 easeInParams;
            public Vector2 easeOutParams;
            public System.Action onStop;
            public System.Action onStart;
            public int loopCount;
            public float duration;
            public float delay;
            public float elapsed;
            public UpdateMode updateMode;
            public LinkedListNode<Context> node;
            public LinkedList<Context> elements;

            public bool HasFlags(Flags flags) => (this.flags & flags) == flags;

            public bool HasAnyFlags(Flags flags) => (this.flags & flags) != 0;

            public void SetFlags(Flags flags, bool value=true)
            {
                if (value)
                    this.flags |= flags;
                else
                    ClearFlags(flags);
            }

            public void ClearFlags(Flags flags) => this.flags &= ~(flags);

            public void Start()
            {
                // TODO: handle resume

                elapsed = 0f;
                delay = 0f;

                // If the from was not set then read it from the provider
                if (!HasFlags(Flags.From))
                    from = provider.Read(target, options);
                
                // If the to was not set then read it from the provider
                if (!HasFlags(Flags.To))
                    to = provider.Read(target, options);

                onStart?.Invoke();
            }

            public void ClearReferences()
            {
                flags = Flags.Free;
                key = null;
                onStop = null;
                onStart = null;
                target = null;
                provider = null;
                easeIn = null;
                easeOut = null;
            }

            public void Evalulate ()
            {
                // Usually only true for Wait and Collecctions
                if (provider == null)
                    return;

                var t = 1.0f;

                // Easing
                if (duration > 0.0f)
                {
                    // Linear time
                    t = elapsed / duration;

                    // Ease In / Out
                    if(easeIn != null && easeOut != null)
                    {
                        if (t <= 0.5f)
                            t = easeIn(t * 2f, easeInParams.x, easeInParams.y) * 0.5f;
                        else if (t > 0.5f)
                            t = (1f - easeOut((1f - t) * 2f, easeOutParams.x, easeOutParams.y)) * 0.5f + 0.5f;
                    }
                    // Ease In
                    else if (easeIn != null)
                    {
                        t = easeIn(t, easeInParams.x, easeInParams.y);
                    }
                    // Ease Out
                    else if (easeOut != null)
                    {
                        t = easeOut(t, easeOutParams.x, easeOutParams.y);
                    }
                }

                if (HasFlags(Flags.Reverse))
                    t = 1f - t;

                t = Mathf.Clamp01(t);

                provider.Write(target, provider.Evalulate(from, to, t, options), options);
            }
        }

        private class Updater : MonoBehaviour
        {
            private void Update() => Tween.Update(NoZ.Tweenz.UpdateMode.Default);
            private void FixedUpdate() => Tween.Update(NoZ.Tweenz.UpdateMode.Fixed);
            private void LateUpdate() => Tween.Update(NoZ.Tweenz.UpdateMode.Late);
        }

        /// <summary>
        /// Internal method used to allocate a pooled Tween
        /// </summary>
        /// <returns>Allocated Context</returns>
        private static Tween AllocTween(TweenProvider provider, object target, Variant from, Variant to, Flags flags = Flags.None, uint options = 0)
        {
            // Pooled tweens
            Context context;
            if (_freeContexts.Count > 0)
            {
                context = _freeContexts.First.Value;
                _freeContexts.Remove(context.node);

                if (!context.HasFlags(Flags.Free))
                    throw new System.InvalidOperationException("Tween in free list that was not free");
            }
            else
            {
                context = new Context();
                context.node = new LinkedListNode<Context>(context);
            }

            // Initialize the tween
            context.instanceId = _nextInstanceId++;
            context.delay = 0f;
            context.flags = flags | Flags.Active;
            context.elapsed = 0f;
            context.duration = 1f;
            context.from = from;
            context.to = to;
            context.provider = provider;
            context.target = target;
            context.options = options;
            context.updateMode = Tweenz.UpdateMode.Default;

            // If the context is a collection make sure the children list is created
            if (context.HasFlags(Flags.Collection) && context.elements == null)
                context.elements = new LinkedList<Context>();                

            // Add to the active list now even though the context is not active yet.  This will
            // ensure if Start was never called on the Context that it will be reclaimed in the 
            // next update phase.
            _activeContexts.AddLast(context.node);

            return new Tween { _context = context, _instanceId = context.instanceId };
        }

        private static float UpdateCollectionDuration (Context context)
        {
            if (!context.HasFlags(Flags.Collection))
                return context.duration + context.delay;

            var duration = 0.0f;
            if (context.HasFlags(Flags.Sequence))
            {
                for (var node = context.elements.First; node != null; node = node.Next)
                    duration += UpdateCollectionDuration(node.Value);
            }
            else
            {
                for (var node = context.elements.First; node != null; node = node.Next)
                    duration = Mathf.Max(duration, UpdateCollectionDuration(node.Value));
            }

            context.duration = duration;

            return context.delay + duration;
        }

        private static void StartOrDelay(Context context)
        {
            context.elapsed = 0f;
            context.SetFlags(Flags.Started);

            // Force our duration on our children
            // TODO: should we only do this if a child does not have a duration?
            var duration = context.duration;
            if (duration > 0f && context.elements != null)
                for (var node = context.elements.First; node != null; node = node.Next)
                    node.Value.duration = duration;

            // If there is no delay start now and run a single frame
            if (context.delay <= 0f)
            {
                context.Start();
                UpdateContext(context, 0f);
            }
        }

        /// <summary>
        /// Free a tween context and return it to the context pool
        /// </summary>
        /// <param name="context">Context to free</param>
        private static void FreeContext (Context context)
        {
            // Make sure we are not aleady free
            if (context.HasFlags(Flags.Free))
                return;

            // Before we free the tween save off some information we need after
            var onStop = context.onStop;
            var target = context.target;
            var destroyOnStop = context.HasFlags(Flags.DestroyOnStop);
            var deactivateOnStop = context.HasFlags(Flags.DeactivateOnStop);
            var disableOnStop = context.HasFlags(Flags.DisableOnStop);

            // Free the tween by adding it to the free pool and clearing its data
            // TODO: max pool size
            context.node.List.Remove(context.node);
            _freeContexts.AddLast(context.node);
            
            context.ClearReferences();

            // Call onStop
            // TODO: why do we require the target to be valid?
            if (!context.HasFlags(Flags.NoCallbacks))
                onStop?.Invoke();


            if(destroyOnStop || deactivateOnStop || disableOnStop)
            {
                var component = target as MonoBehaviour;
                var gameObject = component != null ? component.gameObject : target as GameObject;

                if (deactivateOnStop && gameObject != null)
                    gameObject.SetActive(false);

                if (disableOnStop && component != null && component is MonoBehaviour monoBehaviour)
                    monoBehaviour.enabled = false;

                if (destroyOnStop && gameObject != null)
                    Object.Destroy(gameObject);
            }
        }

        private static void Update(UpdateMode updateMode)
        {
            var elapsedNormal = Time.deltaTime;
            var elapsedUnscaled = Time.unscaledDeltaTime;

            if (updateMode == NoZ.Tweenz.UpdateMode.Fixed)
            {
                elapsedNormal = Time.fixedDeltaTime;
                elapsedUnscaled = Time.fixedUnscaledDeltaTime;
            }

            LinkedListNode<Context> next;
            for (var node = _activeContexts.First; node != null; node = next)
            {
                next = node.Next;
                var context = node.Value;

                // Skip the tween if the update mode does not match
                if (updateMode != context.updateMode)
                    continue;

                // Skip the tween if it is not currently playing
                if (!context.HasFlags(Flags.Playing))
                    continue;

                if (context.HasFlags(Flags.UnscaledTime))
                    UpdateContext(context,elapsedUnscaled);
                else
                    UpdateContext(context,elapsedNormal);
            }

            EmptyStopQueue();
        }

        private static void EmptyStopQueue()
        {
            while (_stoppingContexts.Count > 0)
                FreeContext(_stoppingContexts.First.Value);
        }

        private static float UpdateDelay (Context context, float deltaTime)
        {
            ref var elapsed = ref context.elapsed;
            ref var delay = ref context.delay;

            // Delay finished?
            if (elapsed >= delay)
                return deltaTime;

            // Advance the elapsed time and if the delay threshold has
            // not been met return 0 to indicate the entire delta time was used
            elapsed += deltaTime;
            if (elapsed < delay)
                return 0f;

            // Reduce the delta time to the amount of time past the delay that is remaining
            deltaTime = elapsed - delay;

            // Start the context
            context.Start();

            return deltaTime;
        }

        private static float UpdateContext (Context context, float deltaTime)
        {
            // If the context was never started then automatically stop the
            // tween to make sure it doesnt leak
            if(!context.HasFlags (Flags.Started))
            {
                QueueStop(context, false);
                return deltaTime;
            }

            // If the target is null it is likely a UnityObject that was destroyed.  In this 
            // case we will stop the tween and not call its callbacks as this is an unclean case.
            if (context.target == null)
            {
                QueueStop(context, false);
                return deltaTime;
            }

            // Handle delay
            deltaTime = UpdateDelay(context, deltaTime);
            if (deltaTime <= 0.0f)
                return 0.0f;

            context.elapsed += deltaTime;

            // Handle collection tweens different
            if (context.HasFlags(Flags.Collection))
            {
                if (context.HasFlags(Flags.Sequence))
                    deltaTime = UpdateSequence(context, deltaTime);
                else
                    deltaTime = UpdateGroup(context, deltaTime);
            }
            else
                deltaTime = UpdateProvider(context, deltaTime);
            
            return deltaTime;
        }

        private static float UpdateProvider(Context context, float deltaTime)
        {
            ref var elapsed = ref context.elapsed;
            var duration = context.duration;
            var done = elapsed >= duration;

            if (done)
            {
                // Hangle reverse of ping-pong
                if (context.HasFlags(Flags.PingPong) && !context.HasFlags(Flags.Reverse))
                {
                    elapsed -= duration;
                    context.SetFlags(Flags.Reverse);
                    return UpdateProvider(context, deltaTime);
                } 

                context.ClearFlags(Flags.Reverse);
                elapsed = duration;
            }

            // Always write, even when done, to make sure we hit the final mark
            context.Evalulate();

            if (done)
                return LoopOrStop(context, elapsed - duration);
    
            return deltaTime;
        }

        private static float UpdateGroup(Context context, float deltaTime)
        {
            // Advance all children
            var done = true;
            var remainingTime = deltaTime;
            for (var element = context.elements.First; element != null; element = element.Next)
            {
                var elementContext = element.Value;

                // If the element context is not active then it has already finished
                if (!elementContext.HasFlags(Flags.Active))
                    continue;

                // Start the element if not yet started.
                if (!elementContext.HasFlags(Flags.Started))
                    StartOrDelay(elementContext);

                // Advance the child
                remainingTime = Mathf.Min(remainingTime, UpdateContext(elementContext, deltaTime));

                // We are done if all elements are done
                done &= !elementContext.HasFlags(Flags.Active);
            }

            if (done)
                return LoopOrStop(context, remainingTime);

            return 0.0f;
        }

        private static float UpdateSequence(Context context, float deltaTime)
        {
#if false
            while (_firstChild != null)
            {
                var child = _firstChild;

                if (!child.IsStarted)
                    child.StartOrDelay();
                else if (!child.isActive)
                {
                    Stop(child);
                    continue;
                }                

                deltaTime = child.UpdateInternal(deltaTime);

                if (!child.isActive)
                {
                    if (IsLooping)
                    {
                        // Move the child to the end of the list
                        child.SetParent(null);
                        child.SetParent(this);
                    }
                    else
                    {
                        Stop(child);
                    }
                }

                if (deltaTime <= 0f)
                    break;
            }

            isActive = _firstChild != null;

            return deltaTime;
#else
            return 0;
#endif
        }

        private static float LoopOrStop (Context context, float deltaTime)
        {
            // Handle loop count
            ref var loopCount = ref context.loopCount;
            var loop = context.HasFlags(Flags.Loop);
            if (loopCount > 0 && loop)
            {
                // Handle optoinal loop count, otherwise loop forever
                if (loopCount > 0 && --loopCount == 0)
                {
                    context.ClearFlags(Flags.Loop);
                    loop = false;
                }
            }

            // Still looping?
            if(loop)
            {
                // Reset time
                context.elapsed %= context.duration;

                // Set all child elements as active and not started so they act as if they were never run
                if(context.HasFlags(Flags.Collection))
                {
                    for (var element = context.elements.First; element != null; element = element.Next)
                    {
                        element.Value.ClearFlags(Flags.Started);
                        element.Value.SetFlags(Flags.Active);
                    }
                }

                // If there is any remainingTime we should recursively call ourselves 
                // to make sure it gets applied
                if (deltaTime > 0f)
                    deltaTime = UpdateContext(context, deltaTime);
            }
            else
            {
                QueueStop(context, true);
            }

            return deltaTime;
        }


        private static void QueueStop(Context context, bool executeCallbacks)
        {
            // Make sure a context in the free list does not attempt to stop
            if (context.HasFlags(Flags.Free))
                return;

            // When stopping a child element rather than adding it to the stop queue 
            // we instead just mark it inactive and call its stop method if needed
            if(context.HasFlags(Flags.Element))
            {
                // Clear the Active to mark the element as stopped
                context.ClearFlags(Flags.Active);

                // Execute the onStop callback if there is one
                if(executeCallbacks)
                    context.onStop?.Invoke();

                return;
            }

            // Already in the stop queue
            if (context.HasFlags(Flags.Stopping))
                return;

            // Flag the context as stopping to indicate it is in the stopping list
            context.SetFlags(Flags.Stopping);

            // If a collection then stop the children too
            if (context.HasFlags(Flags.Collection))
            {
                for (var node = context.elements.First; node != null; node = node.Next)
                {
                    // Clear the element flags so queue will actually queue this element 
                    // rather than just marking it as inactive
                    node.Value.ClearFlags(Flags.Element);

                    QueueStop(node.Value, executeCallbacks);
                }
            }

            // If execute callbacks is disabled then add the NoCallbacks flag
            // to the tween to prevent its OnStop from being called
            if (!executeCallbacks)
                context.SetFlags(Flags.NoCallbacks);

            // Remove ourself from the current list
            context.node.List.Remove(context.node);

            // Add ourself to the stop queue
            _stoppingContexts.AddLast(context.node);
        }

        /// <summary>
        /// Stop all tweens running on a target object
        /// 
        /// If the object is a GameObject then all tweens running on an Components of 
        /// that GameObject will also be stopped.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="key">Optional key to filter by</param>
        /// <param name="executeCallbacks">True if any remaining callbacks such as OnStop should be called (Default true)</param>
        public static void Stop(object target, string key = null, bool executeCallbacks = true)
        {
            if (null == target)
                return;

            var isGameObject = target is GameObject;

            for (var node = _activeContexts.First; node != null; node = node.Next)
            {
                // Skip tweens that are not started yet
                if (!node.Value.HasFlags(Flags.Started))
                    continue;

                // Make sure the target matches 
                if (node.Value.target != target)
                {
                    // If our stop target is a GameObject then see if this tween is running on a component of that GameObject 
                    if (!isGameObject || !(node.Value.target is Component component) || target != (object)component.gameObject)
                        continue;
                }

                // If there is a key make sure it matches
                if (key != null && key != node.Value.key)
                    continue;

                QueueStop(node.Value, executeCallbacks);
            }

            EmptyStopQueue();
        }

        /// <summary>
        /// Stop all tweens that match the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="executeCallbacks">True if callbacks such as OnStop should still be called</param>
        public static void Stop(string key, bool executeCallbacks = true)
        {
            if (key == null)
                return;

            for (var node = _activeContexts.First; node != null; node = node.Next)
            {
                // Skip tweens that are not started yet
                if (!node.Value.HasFlags(Flags.Started))
                    continue;

                if (key == node.Value.key)
                    QueueStop(node.Value, executeCallbacks);
            }

            EmptyStopQueue();
        }

        /// <summary>
        /// Stop all active tweens
        /// </summary>
        /// <param name="executeCallbacks"></param>
        public static void StopAll (bool executeCallbacks=true)
        {
            for (var node = _activeContexts.First; node != null; node = node.Next)
            {
                // Skip tweens that are not started yet
                if (!node.Value.HasFlags(Flags.Started))
                    continue;

                QueueStop(node.Value, executeCallbacks);
            }
        }

        /// <summary>
        /// Returns true if the tween has finished
        /// </summary>
        public bool isDone => _context == null || _context.instanceId != _instanceId || _context.HasFlags(Flags.Free);

        public bool isValid => !isDone;


        /// <summary>
        /// Start the tween.  Note that once a tween is started it can no longer be modified.
        /// </summary>
        public Tween Play()
        {
            if (isDone)
                throw new System.InvalidOperationException("Invalid tween");

            // Already playing?
            if (_context.HasFlags(Flags.Playing))
                return this;

            // If not playing but already started then this must be a resume so just set the 
            // playing flag and return
            if (_context.HasFlags(Flags.Started))
            {
                _context.SetFlags(Flags.Playing);
                return this;
            }

            _context.SetFlags(Flags.Playing);

            // Start the updater GameObject if needed
            if (_updater == null)
            {
                var updaterGameObject = new GameObject("TweenZ Updater");
                updaterGameObject.hideFlags = HideFlags.HideAndDontSave;
                Object.DontDestroyOnLoad(updaterGameObject);

                _updater = updaterGameObject.AddComponent<Updater>();
            }

            UpdateCollectionDuration(_context);

            // If the tween has a key stop any other tweens with the same key running on the same target
            if (!string.IsNullOrEmpty(_context.key))
                Stop(_context.target, _context.key);

            StartOrDelay(_context);

            return this;
        }

        /// <summary>
        /// Stop the tween
        /// </summary>
        /// <param name="executeCallbacks">True to execute any remaining callbacks such as OnStop</param>
        public void Stop(bool executeCallbacks = true)
        {
            if (isDone)
                return;

            QueueStop(_context, executeCallbacks);
            EmptyStopQueue();
        }

        /// <summary>
        /// Pause the tween until either Play is called again or the tween is stopped
        /// </summary>
        public void Pause ()
        {
            // TODO: should we throw an exception here?
            if (!isValid)
                return;

            _context.ClearFlags(Flags.Playing);
        }

        /// <summary>
        /// Evalulates the current value of the tween and updates the target
        /// </summary>
        /// <param name="normalizedTime">Time value in the range of 0-1</param>
        public void Evaluate (float normalizedTime)
        {
            if (!isValid)
                return;

            // If play was never called then call it quit and remove play
            if(!_context.HasFlags(Flags.Started))
            {
                Play();
                _context.ClearFlags(Flags.Playing);
            }

            var currentNormalized = _context.elapsed / _context.duration;
            if (normalizedTime < currentNormalized)
            {
                _context.elapsed = 0;
                UpdateContext(_context, normalizedTime * _context.duration);
            }
            else
                UpdateContext(_context, normalizedTime * _context.duration - _context.elapsed);
        }
    }
}
