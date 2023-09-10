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

using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace NoZ.Tweening
{
    /// <summary>
    /// Defines a generic data structure to hold the various data types supported by Tween
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct Variant
    {
        [FieldOffset(0)]
        public Quaternion q;

        [FieldOffset(0)]
        public Vector4 v4;

        [FieldOffset(0)]
        public Vector3 v3;

        [FieldOffset(0)]
        public Vector3 v2;

        [FieldOffset(0)]
        public float f;

        [FieldOffset(0)]
        public char i8;

        [FieldOffset(0)]
        public short i16;

        [FieldOffset(0)]
        public int i32;

        [FieldOffset(0)]
        public long i64;

        [FieldOffset(0)]
        public byte ui8;

        [FieldOffset(0)]
        public ushort ui16;

        [FieldOffset(0)]
        public uint ui32;

        [FieldOffset(0)]
        public ulong ui64;

        [FieldOffset(0)]
        public double d;

        [FieldOffset(0)]
        public Color c;

        [FieldOffset(0)]
        public StyleInt styleInt;

        [FieldOffset(0)]
        public StyleFloat styleFloat;

        [FieldOffset(0)]
        public StyleLength styleLength;

        [FieldOffset(0)]
        public StyleScale styleScale;

        [FieldOffset(0)]
        public StyleColor styleColor;

        [FieldOffset(0)]
        public StyleRotate styleRotate;

        [FieldOffset(0)]
        public StyleTranslate styleTranslate;

        public static implicit operator Variant(char v) => new Variant { i8 = v };
        public static implicit operator Variant(byte v) => new Variant { ui8 = v };
        public static implicit operator Variant(short v) => new Variant { i16 = v };
        public static implicit operator Variant(int v) => new Variant { i32 = v };
        public static implicit operator Variant(long v) => new Variant { i64 = v };
        public static implicit operator Variant(ushort v) => new Variant { ui16 = v };
        public static implicit operator Variant(uint v) => new Variant { ui32 = v };
        public static implicit operator Variant(ulong v) => new Variant { ui64 = v };
        public static implicit operator Variant(float v) => new Variant { f = v };
        public static implicit operator Variant(double v) => new Variant { d = v };
        public static implicit operator Variant(Quaternion v) => new Variant { q = v };
        public static implicit operator Variant(Vector2 v) => new Variant { v2 = v };
        public static implicit operator Variant(Vector3 v) => new Variant { v3 = v };
        public static implicit operator Variant(Vector4 v) => new Variant { v4 = v };
        public static implicit operator Variant(Color v) => new Variant { c = v };
        public static implicit operator Variant(StyleInt v) => new Variant { styleInt = v };
        public static implicit operator Variant(StyleFloat v) => new Variant { styleFloat = v };
        public static implicit operator Variant(StyleLength v) => new Variant { styleLength = v };
        public static implicit operator Variant(StyleColor v) => new Variant { styleColor = v };
        public static implicit operator Variant(StyleScale v) => new Variant { styleScale = v };
        public static implicit operator Variant(StyleRotate v) => new Variant { styleRotate = v };
        public static implicit operator Variant(StyleTranslate v) => new Variant { styleTranslate = v };

        public static implicit operator char(Variant v) => v.i8;
        public static implicit operator byte(Variant v) => v.ui8;
        public static implicit operator short(Variant v) => v.i16;
        public static implicit operator int(Variant v) => v.i32;
        public static implicit operator long(Variant v) => v.i64;
        public static implicit operator ushort(Variant v) => v.ui16;
        public static implicit operator uint(Variant v) => v.ui32;
        public static implicit operator ulong(Variant v) => v.ui64;
        public static implicit operator float(Variant v) => v.f;
        public static implicit operator double(Variant v) => v.d;
        public static implicit operator Quaternion(Variant v) => v.q;
        public static implicit operator Vector2(Variant v) => v.v2;
        public static implicit operator Vector3(Variant v) => v.v3;
        public static implicit operator Vector4(Variant v) => v.v4;
        public static implicit operator Color(Variant v) => v.c;
        public static implicit operator StyleInt(Variant v) => v.styleInt;
        public static implicit operator StyleFloat(Variant v) => v.styleFloat;
        public static implicit operator StyleLength(Variant v) => v.styleLength;
        public static implicit operator StyleScale(Variant v) => v.styleScale;
        public static implicit operator StyleColor(Variant v) => v.styleColor;
        public static implicit operator StyleRotate(Variant v) => v.styleRotate;
        public static implicit operator StyleTranslate(Variant v) => v.styleTranslate;
    }
}