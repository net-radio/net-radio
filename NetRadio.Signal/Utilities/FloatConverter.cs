using System;
using System.IO;

namespace NetRadio.Signal.Utilities
{
    public class FloatConverter
    {
        public int SampleRate { get; private set; }
        public int BitRate { get; private set; }
        public int ChannelCount { get; private set; }

        public FloatConverter(int sampleRate, int bitRate, int channelCount)
        {
            SampleRate = sampleRate;
            BitRate = bitRate;
            ChannelCount = channelCount;
        }

        public int Step()
        {
            return BitRate/8;
        }

        public float Convert(byte[] buffer, int index)
        {
            switch (BitRate)
            {
                case 8:
                    return Pcm8ToFloat32(buffer[index]);
                case 16:
                    return Pcm16ToFloat32(buffer, index);
                case 24:
                    return Pcm24ToFloat32(buffer, index);
                case 32:
                    return Pcm32ToFloat32(buffer, index);
                default:
                    throw new NotSupportedException();
            }
        }

        public static short[] Float32ToPcm16(float[] buffer)
        {
            var short16 = new short[buffer.Length];
            for (var i = 0; i < buffer.Length; i++)
                short16[i] = (short) (buffer[i]*32767.0);

            return short16;
        }

        public static byte[] Float32ToPcm16BufferReady(float[] buffer)
        {
            var mem = new MemoryStream(buffer.Length*2);
            using (var bw = new BinaryWriter(mem))
            {
                for (var i = 0; i < buffer.Length; i++)
                    bw.Write((short) (buffer[i]*32767.0));

                return mem.ToArray();
            }
        }

        public static byte[] Float32ToPcm16VirtualStereoBufferReady(float[] buffer, bool left=true,bool right=true)
        {
            const short NoData=0;
            var mem = new MemoryStream(buffer.Length * 4);
            using (var bw = new BinaryWriter(mem))
            {
                for (var i = 0; i < buffer.Length; i++)
                {
                    var sample = (short)(buffer[i] * 32767.0);

                    bw.Write(left ? sample : NoData);//as left channel
                    bw.Write(right ? sample : NoData);//as right channel
                }
                return mem.ToArray();
            }
        }

        public static float[] Pcm16ToFloat32(short[] buffer)
        {
            var floatBuffer = new float[buffer.Length];
            for (var i = 0; i < buffer.Length; i++)
            {
                var f = buffer[i]/32768F;
                //if (f > 1) f = 1;
                //if (f < -1) f = -1;
                floatBuffer[i] = f;
            }
            return floatBuffer;
        }

        public static float Pcm8ToFloat32(byte value)
        {
            var f=value/ 128f - 1.0f;
            return f;
        }

        public static float Pcm16ToFloat32(short value)
        {
            var f = value / 32768f;
            return f;
        }

        public static float Pcm16ToFloat32(byte[] value)
        {
            short s = value[1];
            s <<= 8;
            s |= value[0];
            var f = s/32768F;

            return f;
        }

        public static float Pcm16ToFloat32(byte[] value, int index)
        {
            short s = value[1 + index];
            s <<= 8;
            s |= value[index];
            var f = s/32768F;
            return f;
        }

        public static float Pcm24ToFloat32(byte[] value)
        {
            var f = (((sbyte)value[2] << 16) | (value[1] << 8) | value[0]) / 8388608f;
            return f;
        }

        public static float Pcm24ToFloat32(byte[] value, int index)
        {
            var f = (((sbyte) value[2 + index] << 16) | (value[1 + index] << 8) | value[index])/8388608f;
            return f;
        }

        public static float Pcm32ToFloat32(int value)
        {
            var f = value / 2147483648f;
            return f;
        }

        public static float Pcm32ToFloat32(byte[] value)
        {
            var f = (((sbyte) value[3] << 24 |
                      value[2] << 16) |
                     (value[1] << 8) |
                     value[0])/2147483648f;
            return f;
        }

        public static float Pcm32ToFloat32(byte[] value, int index)
        {
            var f = (((sbyte) value[index + 3] << 24 |
                      value[index + 2] << 16) |
                     (value[index + 1] << 8) |
                     value[index])/2147483648f;
            return f;
        }
    }
}
