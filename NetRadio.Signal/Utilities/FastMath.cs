using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NetRadio.Signal.Utilities
{
    public class FastMath
    {

        //http://blog.wouldbetheologian.com/2011/11/fast-approximate-sqrt-method-in-c.html
        public static float Sqrt(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            u.f = z;
            u.tmp -= 1 << 23; /* Subtract 2^m. */
            u.tmp >>= 1; /* Divide by 2. */
            u.tmp += 1 << 29; /* Add ((b + 1) / 2) * 2^m. */
            return u.f;
        }

        //http://blog.wouldbetheologian.com/2011/11/fast-approximate-sqrt-method-in-c.html
        public static float SqrtPrecise(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            float xhalf = 0.5f * z;
            u.f = z;
            u.tmp = 0x5f375a86 - (u.tmp >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f * z;
        }

        //http://www.flipcode.com/archives/Fast_log_Function.shtml
        public static float Log2(float value)
        {
            FloatIntUnion u;
            u.tmp = 0;
            u.f = value;

            var log2 = ((u.tmp >> 23) & 255) - 128;
            u.tmp &= ~(255 << 23);
            u.tmp += 127 << 23;

            value = ((-1.0f/3)*value + 2)*value - 2.0f/3; // (1)

            return (value + log2);
        }

        public static double Log10(float value)
        {
            return Log2(value) * 0.30103F;
        }

        public static float Log(float value)
        {
            return Log2(value)*0.69314718F;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatIntUnion
        {
            [FieldOffset(0)]
            public float f;

            [FieldOffset(0)]
            public int tmp;
        }

    }
}
