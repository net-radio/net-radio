using System;
using System.Runtime.InteropServices;

namespace EighthBit.Nmg.ManagedCanToUsb
{
    [StructLayout(LayoutKind.Explicit)]
    public class Frame
    {
        private const byte PACKET_DATA = 0x44;

        private const ushort FILTER_LENGTH = 0xF;
        private const ushort FILTER_RTR = 0x10;
        private const ushort FILTER_ID = 0xFFE0;

        [FieldOffset(1)]
        private byte _header0;
        [FieldOffset(0)]
        private byte _header1;
        [FieldOffset(0)]
        private ushort _header;

        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.LPArray)]
        private byte[] _data;

        public bool Rtr
        {
            get{return (_header & FILTER_RTR) > 0;}
            set
            {
                _header = value ? (ushort)(_header | FILTER_RTR) : (ushort)(_header & ~FILTER_RTR);
            }
        }

        public byte Length
        {
            get { return (byte)(_header & FILTER_LENGTH); }
            set
            {
                if (value > 8)
                    throw new ArgumentOutOfRangeException("maximum data length is 8.");

                _header = (ushort)(_header & ~FILTER_LENGTH); //cleaning length field
                _header |= value;
            }
        }

        public ushort Id
        {
            get
            {
                var id = _header & FILTER_ID;
                id >>= 5;
                return (ushort)id;
            }
            set
            {
                var id = value << 5;
                id &= FILTER_ID;

                _header = (ushort)(_header & ~FILTER_ID); //cleaning id field
                _header |= (ushort)id;
            }
        }

        public byte[] Data
        {
            get { return _data; }
            set
            {
                if(value.Length>8)
                    throw new ArgumentOutOfRangeException("maximum data length is 8.");

                _data = value;
                Length = (byte)_data.Length;
            }
        }

        internal byte[] ToCommandArray()
        {
            var bytes = new byte[Length + 3];
            bytes[0] = PACKET_DATA;
            bytes[1] = _header0;
            bytes[2]=_header1;

            _data.CopyTo(bytes, 3);

            return bytes;
        }

        public Frame()
        {
        }

        internal Frame(byte header0, byte header1)
        {
            _header0 = header0;
            _header1 = header1;
        }
    }
}
