using UnityEngine;
using NUnit.Framework;
using System;

namespace NoZ.Tweening.Tests
{
    public class VariantTests : TweenTestsBase
    {
        #region Float

        [Test]
        public void FloatTo ()
        {
            var value = new TestValue<float>(1.0f);
            var tween = value.TweenFloat(nameof(TestValue<float>.Value), 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1.0f);
            Update(0.5f);
            AssertValue(value, 2.0f);
            Update(0.5f);
            AssertValue(value, 3.0f);
        }

        [Test]
        public void FloatFrom ()
        {
            var value = new TestValue<float>(1.0f);
            var tween = Tween.From(FloatMemberProvider<TestValue<float>>.Get(nameof(TestValue<float>.Value)), value, 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3.0f, 1.0f);
        }

        [Test]
        public void FloatFromTo()
        {
            var value = new TestValue<float>();
            var tween = Tween.FromTo(FloatMemberProvider<TestValue<float>>.Get(nameof(TestValue<float>.Value)), value, 3.0f, 1.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3.0f, 1.0f);
        }

        #endregion

        #region Double

        [Test]
        public void DoubleTo()
        {
            var value = new TestValue<double>(1.0);
            var tween = value.TweenDouble(nameof(TestValue<double>.Value), 3.0).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1.0);
            Update(0.5f);
            AssertValue(value, 2.0);
            Update(0.5f);
            AssertValue(value, 3.0);
        }

        [Test]
        public void DoubleFrom()
        {
            var value = new TestValue<double>(1.0);
            var tween = Tween.From(DoubleMemberProvider<TestValue<double>>.Get(nameof(TestValue<double>.Value)), value, 3.0).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3.0, 1.0);
        }

        [Test]
        public void DoubleFromTo()
        {
            var value = new TestValue<double>();
            var tween = Tween.FromTo(DoubleMemberProvider<TestValue<double>>.Get(nameof(TestValue<double>.Value)), value, 3.0, 1.0).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3.0, 1.0);
        }

        #endregion

        #region Int

        [Test]
        public void IntFrom()
        {
            var value = new TestValue<int>(-1);
            var tween = Tween.From(IntMemberProvider<TestValue<int>>.Get(nameof(TestValue<int>.Value)), value, 3).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3, -1);
        }

        [Test]
        public void IntFromTo()
        {
            var value = new TestValue<int>(0);
            var tween = Tween.FromTo(IntMemberProvider<TestValue<int>>.Get(nameof(TestValue<int>.Value)), value, 3, -1).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3, -1);
        }

        [Test]
        public void IntTo ()
        {
            var value = new TestValue<int>(1);
            var tween = value.TweenInt(nameof(TestValue<int>.Value), 3).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1);
            Update(0.1f);
            AssertValue(value, 1);
            Update(0.4f);
            AssertValue(value, 2);
            Update(0.5f);
            AssertValue(value, 3);
        }

        [Test]
        public void IntToRoundUp()
        {
            var value = new TestValue<int>(1);
            var tween = value.TweenInt(nameof(TestValue<int>.Value), 2, IntOptions.RoundUp).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1);
            Update(0.1f);
            AssertValue(value, 2);
            Update(1.0f);
            AssertValue(value, 2);
        }

        #endregion

        #region Long

        [Test]
        public void LongFrom()
        {
            var value = new TestValue<long>(1L);
            var tween = Tween.From(LongMemberProvider<TestValue<long>>.Get(nameof(TestValue<long>.Value)), value, 3L).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3L, 1L);
        }

        [Test]
        public void LongFromTo()
        {
            var value = new TestValue<long>(0);
            var tween = Tween.FromTo(LongMemberProvider<TestValue<long>>.Get(nameof(TestValue<long>.Value)), value, 3L, 1L).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3L, 1L);
        }

        [Test]
        public void LongTo()
        {
            var value = new TestValue<long>(1);
            var tween = value.TweenLong(nameof(TestValue<long>.Value), 3L).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1L);
            Update(0.1f);
            AssertValue(value, 1L);
            Update(0.4f);
            AssertValue(value, 2L);
            Update(0.5f);
            AssertValue(value, 3L);
        }

        [Test]
        public void LongToRoundUp()
        {
            var value = new TestValue<long>(1);
            var tween = value.TweenLong(nameof(TestValue<long>.Value), 2L, LongOptions.RoundUp).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1L);
            Update(0.1f);
            AssertValue(value, 2L);
            Update(1.0f);
            AssertValue(value, 2L);
        }

        #endregion

        #region UInt

        [Test]
        public void UIntFrom()
        {
            var value = new TestValue<uint>(1U);
            var tween = Tween.From(UIntMemberProvider<TestValue<uint>>.Get(nameof(TestValue<uint>.Value)), value, 3U).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3U, 1U);
        }

        [Test]
        public void UIntFromTo()
        {
            var value = new TestValue<uint>(0U);
            var tween = Tween.FromTo(UIntMemberProvider<TestValue<uint>>.Get(nameof(TestValue<uint>.Value)), value, 3U, 1U).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3U, 1U);
        }

        [Test]
        public void UIntTo()
        {
            var value = new TestValue<uint>(1U);
            var tween = value.TweenUInt(nameof(TestValue<uint>.Value), 3U).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1U);
            Update(0.1f);
            AssertValue(value, 1U);
            Update(0.4f);
            AssertValue(value, 2U);
            Update(0.5f);
            AssertValue(value, 3U);
        }

        [Test]
        public void UIntRoundUp()
        {
            var value = new TestValue<uint>(1U);
            var tween = value.TweenUInt(nameof(TestValue<uint>.Value), 2U, UIntOptions.RoundUp).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1U);
            Update(0.1f);
            AssertValue(value, 2U);
            Update(1.0f);
            AssertValue(value, 2U);
        }

        [Test]
        public void UIntLowToHigh()
        {
            var value = new TestValue<uint>(1U);
            var tween = value.TweenUInt(nameof(TestValue<uint>.Value), 10U).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1U, 10U);
        }

        [Test]
        public void UIntHighToLow()
        {
            var value = new TestValue<uint>(10);
            var tween = value.TweenUInt(nameof(TestValue<uint>.Value), 1U).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 10U, 1U);
        }

        #endregion

        #region ULong

        [Test]
        public void ULongFrom()
        {
            var value = new TestValue<ulong>(1UL);
            var tween = Tween.From(ULongMemberProvider<TestValue<ulong>>.Get(nameof(TestValue<ulong>.Value)), value, 3UL).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3UL, 1UL);
        }

        [Test]
        public void ULongFromTo()
        {
            var value = new TestValue<ulong>(0);
            var tween = Tween.FromTo(ULongMemberProvider<TestValue<ulong>>.Get(nameof(TestValue<ulong>.Value)), value, 3UL, 1UL).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 3UL, 1UL);
        }

        [Test]
        public void ULongTo()
        {
            var value = new TestValue<ulong>(1);
            var tween = value.TweenULong(nameof(TestValue<ulong>.Value), 3UL).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1UL);
            Update(0.1f);
            AssertValue(value, 1UL);
            Update(0.4f);
            AssertValue(value, 2UL);
            Update(0.5f);
            AssertValue(value, 3UL);
        }

        [Test]
        public void ULongRoundUp()
        {
            var value = new TestValue<ulong>(1UL);
            var tween = value.TweenULong(nameof(TestValue<ulong>.Value), 2UL, ULongOptions.RoundUp).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1UL);
            Update(0.1f);
            AssertValue(value, 2UL);
            Update(1.0f);
            AssertValue(value, 2UL);
        }

        [Test]
        public void ULongLowToHigh()
        {
            var value = new TestValue<ulong>(1UL);
            var tween = value.TweenULong(nameof(TestValue<ulong>.Value), 10UL).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 1UL, 10UL);
        }

        [Test]
        public void ULongHighToLow()
        {
            var value = new TestValue<ulong>(10UL);
            var tween = value.TweenULong(nameof(TestValue<ulong>.Value), 1UL).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, 10UL, 1UL);
        }

        #endregion

        #region Vector2

        [Test]
        public void Vector2To()
        {
            var value = new TestValue<Vector2>(Vector2.one);
            var tween = value.TweenVector(nameof(TestValue<Vector2>.Value), Vector2.one * 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector2.one);
            Update(0.5f);
            AssertValue(value, Vector2.one * 2.0f);
            Update(0.5f);
            AssertValue(value, Vector2.one * 3.0f);
        }

        [Test]
        public void Vector2From()
        {
            var value = new TestValue<Vector2>(Vector2.one);
            var tween = Tween.From(Vector2MemberProvider<TestValue<Vector2>>.Get(nameof(TestValue<Vector2>.Value)), value, Vector2.one * 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector2.one * 3.0f, Vector2.one);
        }

        [Test]
        public void Vector2FromTo()
        {
            var value = new TestValue<Vector2>();
            var tween = Tween.FromTo(Vector2MemberProvider<TestValue<Vector2>>.Get(nameof(TestValue<Vector2>.Value)), value, Vector2.one * 3.0f, Vector2.one).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector2.one * 3.0f, Vector2.one);
        }

        [Test]
        public void Vector2IgnoreX()
        {
            var value = new TestValue<Vector2>(new Vector2(0.0f, 3.0f));
            var tween = value.TweenVector (nameof(TestValue<Vector2>.Value), Vector2.one, Vector2Options.IgnoreX).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, new Vector2(0.0f, 3.0f), new Vector2 (0.0f, 1.0f));
        }

        [Test]
        public void Vector2IgnoreY()
        {
            var value = new TestValue<Vector2>(new Vector2(3.0f, 0.0f));
            var tween = value.TweenVector(nameof(TestValue<Vector2>.Value), Vector2.one, Vector2Options.IgnoreY).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, new Vector2(3.0f, 0.0f), new Vector2(1.0f, 0.0f));
        }

        #endregion

        #region Vector3

        [Test]
        public void Vector3To()
        {
            var value = new TestValue<Vector3>(Vector3.one);
            var tween = value.TweenVector(nameof(TestValue<Vector3>.Value), Vector3.one * 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector3.one);
            Update(0.5f);
            AssertValue(value, Vector3.one * 2.0f);
            Update(0.5f);
            AssertValue(value, Vector3.one * 3.0f);
        }

        [Test]
        public void Vector3From()
        {
            var value = new TestValue<Vector3>(Vector3.one);
            var tween = Tween.From(Vector3MemberProvider<TestValue<Vector3>>.Get(nameof(TestValue<Vector3>.Value)), value, Vector3.one * 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector3.one * 3.0f, Vector3.one);
        }

        [Test]
        public void Vector3FromTo()
        {
            var value = new TestValue<Vector3>();
            var tween = Tween.FromTo(Vector3MemberProvider<TestValue<Vector3>>.Get(nameof(TestValue<Vector3>.Value)), value, Vector3.one * 3.0f, Vector3.one).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector3.one * 3.0f, Vector3.one);
        }

        public static Tuple<Vector3Options, Vector3>[] _vector3Ignore = new Tuple<Vector3Options, Vector3>[]
        {
            new (Vector3Options.IgnoreX, new Vector3(0.0f, 1.0f, 1.0f)),
            new (Vector3Options.IgnoreY, new Vector3(1.0f, 0.0f, 1.0f)),
            new (Vector3Options.IgnoreZ, new Vector3(1.0f, 1.0f, 0.0f)),
            new (Vector3Options.IgnoreXY, new Vector3(0.0f, 0.0f, 1.0f)),
            new (Vector3Options.IgnoreYZ, new Vector3(1.0f, 0.0f, 0.0f)),
            new (Vector3Options.IgnoreXZ, new Vector3(0.0f, 1.0f, 0.0f))
        };

        [Test]
        public void Vector3Ignore ([ValueSource(nameof(_vector3Ignore))] Tuple<Vector3Options, Vector3> test)
        {
            var value = new TestValue<Vector3>();
            var tween = Tween.FromTo(Vector3MemberProvider<TestValue<Vector3>>.Get(nameof(TestValue<Vector3>.Value)), value, Vector3.one * 3.0f, Vector3.one, test.Item1).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, test.Item2 * 3.0f, test.Item2);
        }

        #endregion

        #region Vector4

        [Test]
        public void Vector4To()
        {
            var value = new TestValue<Vector4>(Vector4.one);
            var tween = value.TweenVector(nameof(TestValue<Vector4>.Value), Vector4.one * 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector4.one);
            Update(0.5f);
            AssertValue(value, Vector4.one * 2.0f);
            Update(0.5f);
            AssertValue(value, Vector4.one * 3.0f);
        }

        [Test]
        public void Vector4From()
        {
            var value = new TestValue<Vector4>(Vector4.one);
            var tween = Tween.From(Vector4MemberProvider<TestValue<Vector4>>.Get(nameof(TestValue<Vector4>.Value)), value, Vector4.one * 3.0f).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector4.one * 3.0f, Vector4.one);
        }

        [Test]
        public void Vector4FromTo()
        {
            var value = new TestValue<Vector4>();
            var tween = Tween.FromTo(Vector4MemberProvider<TestValue<Vector4>>.Get(nameof(TestValue<Vector4>.Value)), value, Vector4.one * 3.0f, Vector4.one).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, Vector4.one * 3.0f, Vector4.one);
        }

        public static Tuple<Vector4Options, Vector4>[] _vector4Ignore = new Tuple<Vector4Options, Vector4>[]
        {
            new (Vector4Options.IgnoreX, new Vector4(0.0f, 1.0f, 1.0f, 1.0f)),
            new (Vector4Options.IgnoreY, new Vector4(1.0f, 0.0f, 1.0f, 1.0f)),
            new (Vector4Options.IgnoreZ, new Vector4(1.0f, 1.0f, 0.0f, 1.0f)),
            new (Vector4Options.IgnoreW, new Vector4(1.0f, 1.0f, 1.0f, 0.0f)),
            new (Vector4Options.IgnoreXY, new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
            new (Vector4Options.IgnoreYZ, new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
            new (Vector4Options.IgnoreXZ, new Vector4(0.0f, 1.0f, 0.0f, 1.0f)),
            new (Vector4Options.IgnoreXW, new Vector4(0.0f, 1.0f, 1.0f, 0.0f)),
            new (Vector4Options.IgnoreYW, new Vector4(1.0f, 0.0f, 1.0f, 0.0f)),
            new (Vector4Options.IgnoreZW, new Vector4(1.0f, 1.0f, 0.0f, 0.0f)),
            new (Vector4Options.IgnoreXYZ, new Vector4(0.0f, 0.0f, 0.0f, 1.0f)),
            new (Vector4Options.IgnoreXYW, new Vector4(0.0f, 0.0f, 1.0f, 0.0f)),
            new (Vector4Options.IgnoreXZW, new Vector4(0.0f, 1.0f, 0.0f, 0.0f)),
            new (Vector4Options.IgnoreYZW, new Vector4(1.0f, 0.0f, 0.0f, 0.0f))
        };

        [Test]
        public void Vector4Ignore([ValueSource(nameof(_vector4Ignore))] Tuple<Vector4Options, Vector4> test)
        {
            var value = new TestValue<Vector4>();
            var tween = Tween.FromTo(Vector4MemberProvider<TestValue<Vector4>>.Get(nameof(TestValue<Vector4>.Value)), value, Vector4.one * 3.0f, Vector4.one, test.Item1).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, test.Item2 * 3.0f, test.Item2);
        }

        #endregion

        #region Quaternion

        private static readonly Quaternion kQuaternionEuler1 = Quaternion.Euler(1.0f, 1.0f, 1.0f);
        private static readonly Quaternion kQuaternionEuler2 = Quaternion.Euler(2.0f, 2.0f, 2.0f);
        private static readonly Quaternion kQuaternionEuler3 = Quaternion.Euler(3.0f, 3.0f, 3.0f);

        [Test]
        public void QuaternionTo()
        {
            var value = new TestValue<Quaternion>(kQuaternionEuler1);
            var tween = value.TweenQuaternion(nameof(TestValue<Quaternion>.Value), kQuaternionEuler3).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, kQuaternionEuler1, kQuaternionEuler3);
        }

        [Test]
        public void Quaternion4From()
        {
            var value = new TestValue<Quaternion>(kQuaternionEuler1);
            var tween = Tween.From(QuaternionMemberProvider<TestValue<Quaternion>>.Get(nameof(TestValue<Quaternion>.Value)), value, kQuaternionEuler3).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, kQuaternionEuler3, kQuaternionEuler1);
        }

        [Test]
        public void QuaternionFromTo()
        {
            var value = new TestValue<Quaternion>();
            var tween = Tween.FromTo(QuaternionMemberProvider<TestValue<Quaternion>>.Get(nameof(TestValue<Quaternion>.Value)), value, kQuaternionEuler3, kQuaternionEuler1).Duration(1.0f).Play();
            AssertPlaying(tween);
            AssertValue(value, kQuaternionEuler3, kQuaternionEuler1);
        }

        #endregion
    }
}