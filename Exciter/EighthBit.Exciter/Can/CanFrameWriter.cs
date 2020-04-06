using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Can
{
    public class CanFrameWriter
    {
        private uint _id;
        private bool _rtr;
        private bool _extended;

        private readonly List<byte> _data=new List<byte>();

        public CanFrameWriter Id(uint id)
        {
            _id = id;
            return this;
        }

        public uint Id()
        {
            return _id;
        }

        public CanFrameWriter Rtr(bool rtr)
        {
            _rtr = rtr;
            return this;
        }

        public bool Rtr()
        {
            return _rtr;
        }

        public CanFrameWriter Extended(bool extended)
        {
            _extended = extended;
            return this;
        }

        public bool Extended()
        {
            return _extended;
        }

        public CanFrameWriter Purge()
        {
            _data.Clear();
            return this;
        }

        public CanFrameWriter TrimEnd(int count)
        {
            _data.RemoveRange(count-1, _data.Count- count);

            return this;
        }

        public CanFrameWriter TrimStart(int count)
        {
            _data.RemoveRange(0, count);

            return this;
        }

        public CanFrameWriter Write(IEnumerable<byte> data, bool bigIndian=false)
        {
            _data.AddRange(bigIndian?data.Reverse(): data);
            return this;
        }

        public CanFrameWriter Write(byte value)
        {
            _data.Add(value);
            return this;
        }

        public CanFrameWriter Write(bool value)
        {
            _data.Add(value ? (byte)1 : (byte)0);
            return this;
        }

        public CanFrameWriter Write(sbyte value)
        {
            return Write(BitConverter.GetBytes(value), false);
        }

        public CanFrameWriter Write(ushort value, bool bigIndian=false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(uint value, bool bigIndian = false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(ulong value, bool bigIndian = false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(short value, bool bigIndian = false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(int value, bool bigIndian = false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(long value, bool bigIndian = false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(float value, bool bigIndian = false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(double value, bool bigIndian = false)
        {
            return Write(BitConverter.GetBytes(value), bigIndian);
        }

        public CanFrameWriter Write(string value,Encoding encoding, bool bigIndian = false)
        {
            var bytes = encoding.GetBytes(value);
            return Write(bytes, bigIndian);
        }

        public CanFrameWriter Write(string value, bool bigIndian = false)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Write(bytes, bigIndian);
        }

        public CanFrameWriter WriteAscii(string value, bool bigIndian = false)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            return Write(bytes, bigIndian);
        }

        public CanFrame ToCanFrame(bool padZero = false)
        {
            return CanFrame.Create(_id, _data.ToArray(), _extended, padZero);
        }
    }
}
