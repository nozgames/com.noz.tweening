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
using UnityEngine;

namespace NoZ.Tweenz
{
    public partial struct Tween
    {
        private Context ValidateModifierContext()
        {
            if (isDone)
                throw new System.InvalidOperationException("Invalid Tween");

            if(_context.HasFlags(Flags.Started))
                throw new System.InvalidOperationException("Tween modifiers must be applied before the Tween is started");

            return _context;
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
            if(seconds < 0.0f)
                throw new InvalidOperationException("Tween delay must be greater or equal to zero");

            ValidateModifierContext().delay = seconds; return this; 
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

            ValidateModifierContext().duration = seconds; 
            return this; 
        }

        /// <summary>
        /// Callback to invoke when the Tween is stopped
        /// 
        /// Note that the OnStop callback may not be called if the tween is stopped using 
        /// an executeCallbacks value of false.
        /// </summary>
        /// <param name="callback">Action</param>
        public Tween OnStop(Action callback) { ValidateModifierContext().onStop = callback; return this; }

        /// <summary>
        /// Callback to invoke when the Tween starts (after the delay)
        /// </summary>
        /// <param name="callback">Action</param>
        public Tween OnStart(Action callback) { ValidateModifierContext().onStart = callback; return this; }

        /// <summary>
        /// Set the tween update mode.
        /// </summary>
        /// <param name="value">True to run on fixed update, false otherwise</param>
        public Tween UpdateMode(UpdateMode mode) { ValidateModifierContext().updateMode = mode; return this; }

        /// <summary>
        /// Set the animation key 
        /// </summary>
        /// <param name="key">Animation key</param>
        public Tween Key(string key) { ValidateModifierContext().key = key; return this; }

        /// <summary>
        /// Set the tween to automatically destroy the GameObject attached to the target when the Tween stops
        /// </summary>
        public Tween DestroyOnStop(bool value = true)
        {
            ValidateModifierContext().SetFlags(Flags.DestroyOnStop, value);
            return this;
        }

        /// <summary>
        /// Set the tween to automatically deactivate the GameObject attached to the target when the Tween stops
        /// </summary>
        public Tween DeactivateOnStop(bool value = true)
        {
            ValidateModifierContext().SetFlags(Flags.DeactivateOnStop, value);
            return this;
        }

        /// <summary>
        /// Automatically disable the target Component when the Tween stops
        /// </summary>
        /// <param name="disable">True to enable, false to disable</param>
        /// <returns></returns>
        public Tween DisableOnStop(bool value = true)
        {
            ValidateModifierContext().SetFlags(Flags.DisableOnStop, value);
            return this;
        }

        /// <summary>
        /// Enables or disables PingPong mode.  When PingPong mode is enabled the animation
        /// will play itself fully forward and then then in reverse before stopping.  This means that 
        /// the effective duration of the animation will be doubled. Note that everything is run in reverse 
        /// including easing modes.
        /// </summary>
        /// <param name="pingpong">True to enable PingPong mode.</param>
        public Tween PingPong(bool pingpong = true)
        {
            ValidateModifierContext();

            if (_context.HasFlags(Flags.Collection))
                throw new InvalidOperationException("PingPong is not supported on Collection tweens");

            _context.SetFlags(Flags.PingPong, pingpong);
            return this;
        }

        /// <summary>
        /// Switch the tween to start the configured 'from' value and end at the current value
        /// </summary>
        /// <returns></returns>
        public Tween From() 
        {
            ValidateModifierContext();
            _context.SetFlags(Flags.From);
            _context.ClearFlags(Flags.To);
            return this; 
        }

        /// <summary>
        /// Set the loop state of the animation
        /// </summary>
        /// <param name="loop">True if the animation should loop</param>
        public Tween Loop(int count = -1)
        {
            ValidateModifierContext();

            _context.SetFlags(Flags.Loop);
            _context.loopCount = count;
            return this;
        }

        /// <summary>
        /// Sets the animation to use unscaled time rather than normal scaled time
        /// </summary>
        /// <param name="unscaled">True to use unscaled time</param>
        public Tween UnscaledTime(bool unscaled = true)
        {
            ValidateModifierContext().SetFlags(Flags.UnscaledTime, unscaled);
            return this;
        }

        /// <summary>
        /// Adds an element to a Sequext or Group tween
        /// </summary>
        /// <param name="element">element to add</param>
        public Tween Element (Tween element)
        {
            ValidateModifierContext();

            if (!element.isValid)
                throw new System.InvalidOperationException("Invalid tween specified for Element modifier");

            if (!_context.HasFlags(Flags.Collection))
                throw new System.InvalidOperationException("Element modifier can only be called on a Sequence or Group");

            if (element._context.HasFlags(Flags.Started))
                throw new System.InvalidOperationException("Element modifier must be given an Unstarted tween");
            
            if (element._context.node.List != _activeContexts)
                throw new System.InvalidOperationException("Tween specified for Element modifier is already an element of another Tween");

            if (element._context.HasFlags(Flags.PingPong))
                throw new System.InvalidOperationException("PingPong is not supported on Element tweens");

            if (element._context.HasFlags(Flags.Loop))
                throw new System.InvalidOperationException("Loop is not supported on Element tweens");

            // Remove from the global context list and move to the collection's element list
            element._context.node.List.Remove(element._context.node);
            _context.elements.AddLast(element._context.node);

            // Flag the context as an element for special handling
            element._context.SetFlags(Flags.Element);

            return this;
        }
    }
}
