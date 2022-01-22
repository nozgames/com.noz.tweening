using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NoZ.Tweening.Internals;
using System;

namespace NoZ.Tweening.Tests
{
    public class TweenContextTests : TweenTestsBase
    {
        [Test]
        public void Reset()
        {
            Assert.IsTrue(AreNoTweensPlaying());

            this.TweenWait(1).Play().Pause();
            this.TweenWait(1).UpdateMode(UpdateMode.Manual).Play();
            this.TweenGroup().Element(this.TweenWait(2)).Play();
            this.TweenWait(1).Play().Stop();

            Assert.IsTrue(PlayingCount > 0);
            Assert.IsTrue(FreeCount > 0);
            Assert.IsTrue(ElementCount > 0);
            Assert.IsTrue(PausedCount > 0);
            Assert.IsTrue(ManualCount > 0);

            Tween.Reset();

            Assert.IsTrue(PlayingCount == 0);
            Assert.IsTrue(FreeCount == 0);
            Assert.IsTrue(ElementCount == 0);
            Assert.IsTrue(PausedCount == 0);
            Assert.IsTrue(ManualCount == 0);
        }

        [Test]
        public void TweenContextReused ()
        {
            Assert.IsTrue(FreeCount==0);
            this.TweenWait(1.0f).Play().Stop();
            Assert.IsTrue(FreeCount==1);
            var tween = this.TweenWait(1.0f).Play();
            Assert.IsTrue(PlayingCount==1);
            Assert.IsTrue(FreeCount==0);
            tween.Stop();
            Assert.IsTrue(FreeCount==1);
        }

        [UnityTest]
        public IEnumerator WaitForTweenBlocksUntilStop ()
        {
            StartTimer();
            var tween = this.TweenWait(0.1f).Play();
            yield return new WaitForTween(tween);
            Assert.IsTrue(StopTimer() >= 0.1f);
        }

        [UnityTest]
        public IEnumerator StopsAfterDuration()
        {
            var tween = this.TweenWait(0.2f).Play();
            Assert.IsTrue(TweenContext.GetStateCount(TweenContext.State.Playing) == 1);
            yield return new WaitForSeconds(0.2f);
            Assert.IsFalse(tween.isValid);
            Assert.IsTrue(TweenContext.GetStateCount(TweenContext.State.Playing) == 0);
        }

        [UnityTest]
        public IEnumerator ManualStopAfterDuration ()
        {
            var tween = this.TweenWait(0.2f).AutoStop(false).Play();
            Assert.IsTrue(IsOnlyTweenPlaying(tween));
            yield return new WaitForSeconds(0.3f);
            Assert.IsTrue(IsOnlyTweenPlaying(tween));
            tween.Stop();
            Assert.IsFalse(tween.isValid);
            Assert.IsTrue(AreNoTweensPlaying());
        }

        [UnityTest]
        public IEnumerator ManualStopBeforeDuration()
        {
            var tween = this.TweenWait(0.2f).AutoStop(false).Play();
            Assert.IsTrue(IsOnlyTweenPlaying(tween));
            yield return new WaitForSeconds(0.01f);
            Assert.IsTrue(IsOnlyTweenPlaying(tween));
            tween.Stop();
            Assert.IsFalse(tween.isValid);
            Assert.IsTrue(AreNoTweensPlaying());
        }

        [Test]
        public void CleanupOrphanedTweens()
        {
            this.TweenWait(1.0f);
            Assert.IsTrue(CreatedCount == 1);
            TweenContext.Update(UpdateMode.Late, 0.0f, 0.0f);
            Assert.IsTrue(CreatedCount == 0);
            Assert.IsTrue(FreeCount == 1);
        }

        [Test]
        public void OnPlayCalledInStack ()
        {
            var called = false;
            this.TweenWait(0.1f).OnPlay(() => called = true).Play();
            Assert.IsTrue(called);
        }

        [Test]
        public void OnPlayCalledAfterDelay()
        {
            var called = false;
            this.TweenWait(0.1f).Delay(0.5f).OnPlay(() => called = true).Play();
            Assert.IsFalse(called);
            TweenContext.Update(UpdateMode.Default, 1.0f, 1.0f);
            Assert.IsTrue(called);
        }

        [Test]
        public void OnPlayStoppedBeforeDelay ()
        {
            var called = false;
            var tween = this.TweenWait(0.1f).Delay(0.5f).OnPlay(() => called = true).Play();
            Assert.IsFalse(called);
            TweenContext.Update(UpdateMode.Default, 0.1f, 0.1f);
            Assert.IsFalse(called);
            tween.Stop();
        }

        [Test]
        public void OnStopStoppedBeforeDelay ()
        {
            var called = false;
            var tween = this.TweenWait(0.1f).Delay(0.5f).OnStop(() => called = true).Play();
            Assert.IsFalse(called);
            tween.Stop();
            Assert.IsTrue (called);
        }

        [Test]
        public void OnStopCalledInStack()
        {
            var called = false;
            var tween = this.TweenWait(0.1f).OnStop(() => called = true).Play();
            Assert.IsFalse(called);
            tween.Stop();
            Assert.IsTrue(called);
        }

        [Test]
        public void OnStopIgnored ()
        {
            var called = false;
            var tween = this.TweenWait(0.1f).Delay(0.5f).OnStop(() => called = true).Play();
            Assert.IsFalse(called);
            tween.Stop(executeCallbacks:false);
            Assert.IsFalse(called);
        }

        [Test]
        public void OnStopAutoStop ()
        {
            var called = false;
            var tween = this.TweenWait(0.1f).Delay(0.5f).OnStop(() => called = true).Play();
            Assert.IsFalse(called);
            TweenContext.Update(UpdateMode.Default, 10.0f, 10.0f);
            Assert.IsTrue(called);
        }

        [Test]
        public void PauseAndResume ()
        {
            var test = new TestValue<float>();
            var tween = test.TweenFloat("Value", 1.0f).Play();
            Assert.IsTrue(PlayingCount == 1);
            Assert.IsTrue(PausedCount == 0);

            // Pause and ensure the value doesnt change
            tween.Pause();
            Assert.IsTrue(PlayingCount == 0);
            Assert.IsTrue(PausedCount == 1);
            Assert.IsTrue(test.Is(0.0f));
            Update(10.0f);
            Assert.IsTrue(test.Is(0.0f));

            // Resume and make sure the value now changes
            tween.Play();
            Assert.IsTrue(PlayingCount == 1);
            Assert.IsTrue(PausedCount == 0);
            Assert.IsTrue(test.Is(0.0f));
            Update(10.0f);
            Assert.IsTrue(test.Is(1.0f));
        }

        [Test]
        public void PingPong ()
        {
            var value = new TestValue<float>();
            var tween = value.TweenFloat("Value", 1.0f).Duration(1.0f).PingPong().Play();
            Assert.IsTrue(tween.isPlaying);
            Assert.IsTrue(value.Is(0.0f));
            Update(0.5f);
            Assert.IsTrue(value.Is(1.0f));
            Update(0.5f);
            Assert.IsTrue(value.Is(0.0f));
            Assert.IsFalse(tween.isPlaying);
        }

        [Test]
        public void Manual ()
        {
            var value = new TestValue<float>(2.0f);
            var tween = value.TweenFloat(nameof(TestValue<float>.Value), 1.0f).Duration(1.0f).UpdateMode(UpdateMode.Manual).Play();
            Assert.IsTrue(ManualCount == 1);
            AssertPlaying(tween);
            AssertValue(value, 2.0f);
            tween.Update(0.5f);
            AssertValue(value, 1.5f);
            tween.Update(0.5f);
            AssertValue(value, 1.0f);
            tween.Stop();
            Assert.IsTrue(ManualCount == 0);
        }
    }
}

