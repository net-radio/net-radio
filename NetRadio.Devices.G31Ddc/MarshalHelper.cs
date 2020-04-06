using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NetRadio.Devices.G3XDdc
{
    class MarshalHelper
    {
        public static void WriteUInt32(IntPtr ptr, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            Marshal.WriteByte(ptr, bytes[0]);
            Marshal.WriteByte(ptr + 1, bytes[1]);
            Marshal.WriteByte(ptr + 2, bytes[2]);
            Marshal.WriteByte(ptr + 3, bytes[3]);
        }

        public static uint ReadUInt32(IntPtr ptr)
        {
            var bytes = new[]
            {
                Marshal.ReadByte(ptr),
                Marshal.ReadByte(ptr+1),
                Marshal.ReadByte(ptr+2),
                Marshal.ReadByte(ptr+3)
            };

            var value = BitConverter.ToUInt32(bytes,0);
            return value;
        }

        public static double ReadDouble(IntPtr ptr)
        {
            var bytes = new[]
            {
                Marshal.ReadByte(ptr),
                Marshal.ReadByte(ptr + 1),
                Marshal.ReadByte(ptr + 2),
                Marshal.ReadByte(ptr + 3),
                Marshal.ReadByte(ptr + 4),
                Marshal.ReadByte(ptr + 5),
                Marshal.ReadByte(ptr + 6),
                Marshal.ReadByte(ptr + 7)
            };

            var value = BitConverter.ToDouble(bytes, 0);
            return value;
        }
    }
}
