using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Can
{
    public class CanFrame:EventArgs
    {
        public const int MAX_FRAME_SIZE = 8;

        private byte[] _bytes;

        public uint Id { get; private set; }
        public bool Rtr { get; private set; }
        public bool Extended { get; private set; }

        public int Length
        {
            get { return _bytes == null ? 0 : _bytes.Length; }
        }

        public byte this[int index]
        {
            get { return _bytes[index]; }
        }

        public byte[] Data()
        {
            return _bytes;
        }

        public byte TryGet(int index)
        {
            return index < _bytes.Length ? _bytes[index] : (byte)0;
        }

        private CanFrame()
        {
        }

        public static CanFrame Create(uint id, byte[] data,bool extended=false,bool padZeros = false)
        {
            if (data.Length > MAX_FRAME_SIZE)
                throw new NotSupportedException("extended CAN frame is not supported, maximum length is 8.");

            if (padZeros && data.Length<MAX_FRAME_SIZE)
            {
                var temp = data;
                data = new byte[MAX_FRAME_SIZE];
                temp.CopyTo(data, 0);
            }

            return new CanFrame { Id = id,Extended=extended, _bytes = data };
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Id: {0:x2}, Length: {1}, RTR: {2}, Data:", Id, Length, Rtr);
            foreach (var data in _bytes)
                builder.AppendFormat(" {0:x2}", data);

            return builder.ToString();
        }
    }
}
