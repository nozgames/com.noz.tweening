using System.Diagnostics;
using NoZ.Tweening.Internals;
using System;
using NUnit.Framework;
using UnityEngine;

namespace NoZ.Tweening.Tests
{
    public class TweenTestsBase
    {
        private Stopwatch _stopwatch = new Stopwatch();

        protected void StartTimer() => _stopwatch.Restart();
        protected float StopTimer() { _stopwatch.Stop(); return _stopwatch.ElapsedMilliseconds / 1000.0f; }

        protected bool IsOneTweenPlaying() => TweenContext.GetStateCount(TweenContext.State.Playing) == 1;
        protected bool IsOnlyTweenPlaying(Tween tween) => tween.isPlaying && IsOneTweenPlaying();
        protected bool AreNoTweensPlaying() => TweenContext.GetStateCount(TweenContext.State.Playing) == 0;
        protected int PlayingCount => TweenContext.GetStateCount(TweenContext.State.Playing);
        protected int FreeCount => TweenContext.GetStateCount(TweenContext.State.Free);
        protected int ManualCount => TweenContext.GetStateCount(TweenContext.State.Manual);
        protected int PausedCount => TweenContext.GetStateCount(TweenContext.State.Paused);
        protected int ElementCount => TweenContext.GetStateCount(TweenContext.State.Element);
        protected int CreatedCount => TweenContext.GetStateCount(TweenContext.State.Created);

        protected void Update(float deltaTime, UpdateMode updateMode = UpdateMode.Default) =>
            TweenContext.Update(updateMode, deltaTime, deltaTime);

        protected class TestValue<T> where T : IEquatable<T>
        {
            public T Value { get; set; }
            public bool Is(T value) => Value.Equals(value);
            public TestValue() { }
            public TestValue(T value) => Value = value;
        }

        protected void AssertValid(Tween tween) => Assert.IsTrue(tween.isValid);
        protected void AssertPlaying(Tween tween) => Assert.IsTrue(tween.isPlaying);
        protected void AssertValue<T>(TestValue<T> value, T compareTo) where T : IEquatable<T> => Assert.IsTrue(value.Is(compareTo));
        protected void AssertValue<T>(TestValue<T> value, T before, T after, float duration=1.0f) where T : IEquatable<T>
        {
            AssertValue<T>(value, before);
            Update(duration);
            AssertValue(value, after);
        }
        protected void AssertValue(TestValue<Quaternion> a, Quaternion b) => 
            Assert.IsTrue(1.0f - Mathf.Abs(Quaternion.Dot(a.Value, b)) < Quaternion.kEpsilon);

        protected void AssertValue(TestValue<Quaternion> value, Quaternion before, Quaternion after, float duration = 1.0f)
        {
            AssertValue(value, before);
            Update(duration);
            AssertValue(value, after);
        }

        [SetUp]
        public void Startup()
        {
            Tween.Reset();
        }

        [TearDown]
        public void Teardown()
        {
            Tween.Reset();
        }
    }
}