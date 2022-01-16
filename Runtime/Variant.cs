using System.Runtime.InteropServices;
using UnityEngine;

namespace NoZ.Tweenz
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
        public short s;

        [FieldOffset(0)]
        public int i;

        [FieldOffset(0)]
        public long l;

        [FieldOffset(0)]
        public ushort us;

        [FieldOffset(0)]
        public uint ui;

        [FieldOffset(0)]
        public ulong ul;

        [FieldOffset(0)]
        public double d;

        [FieldOffset(0)]
        public byte b;

        [FieldOffset(0)]
        public Color c;

        public static implicit operator Variant(byte v) => new Variant { b = v };
        public static implicit operator Variant(short v) => new Variant { s = v };
        public static implicit operator Variant(int v) => new Variant { i = v };
        public static implicit operator Variant(long v) => new Variant { l = v };
        public static implicit operator Variant(ushort v) => new Variant { us = v };
        public static implicit operator Variant(uint v) => new Variant { ui = v };
        public static implicit operator Variant(ulong v) => new Variant { ul = v };
        public static implicit operator Variant(float v) => new Variant { f = v };
        public static implicit operator Variant(double v) => new Variant { d = v };
        public static implicit operator Variant(Quaternion v) => new Variant { q = v };
        public static implicit operator Variant(Vector2 v) => new Variant { v2 = v };
        public static implicit operator Variant(Vector3 v) => new Variant { v3 = v };
        public static implicit operator Variant(Vector4 v) => new Variant { v4 = v };
        public static implicit operator Variant(Color v) => new Variant { c = v };

        public static implicit operator byte(Variant v) => v.b;
        public static implicit operator short(Variant v) => v.s;
        public static implicit operator int(Variant v) => v.i;
        public static implicit operator long(Variant v) => v.l;
        public static implicit operator ushort(Variant v) => v.us;
        public static implicit operator uint(Variant v) => v.ui;
        public static implicit operator ulong(Variant v) => v.ul;
        public static implicit operator float(Variant v) => v.f;
        public static implicit operator double(Variant v) => v.d;
        public static implicit operator Quaternion(Variant v) => v.q;
        public static implicit operator Vector2(Variant v) => v.v2;
        public static implicit operator Vector3(Variant v) => v.v3;
        public static implicit operator Vector4(Variant v) => v.v4;
        public static implicit operator Color(Variant v) => v.c;
    }
}