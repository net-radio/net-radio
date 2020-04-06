using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRadio.Signal;
using NetRadio.Signal.Utilities;

namespace NetRadio.Devices.G3XDdc.Signal
{
    public class G3XDdcDdc1FloatProvider:IFloatProvider
    {
        public event EventHandler<FloatArgs> FloatChunkRecieved;

        private readonly float[] _floatBuffer;
        private readonly FloatConverter _converter;
        private readonly Ddc1 _ddc1;
        private readonly int _step;

        public bool SeperateIq { get; private set; }
        public int DataLength { get; private set; }

        public int SamplingRate()
        {
            var iqRate = (int) _ddc1.DdcArgs().Info.SampleRate;
            return SeperateIq ? iqRate : iqRate*2;
        }

        public int Bits()
        {
            return (int)_ddc1.DdcArgs().Info.BitsPerSample;
        }

        public int ChannelCount()
        {
            return SeperateIq ? 2 : 1;
        }

        public void Start()
        {
            _ddc1.DataRecieved+=Ddc1_DataRecieved;
        }

        public void Stop()
        {
            _ddc1.DataRecieved -= Ddc1_DataRecieved;
        }

        private void Ddc1_DataRecieved(object sender, Ddc1CallbackArgs e)
        {
            if (FloatChunkRecieved == null)
                return;

            var iqRate = e.SamplingRate;
            iqRate= SeperateIq ? iqRate : iqRate * 2;

            _floatBuffer.Initialize();

            DataLength = e.Data.Length;
            var j = 0;
            for (var i = 0; i < e.Data.Length; i+=_step)
            {
                _floatBuffer[j]=_converter.Convert(e.Data, i);
                j++;
            }

            FloatChunkRecieved(this, new FloatArgs(_floatBuffer, iqRate, j));
        }

        public G3XDdcDdc1FloatProvider(Ddc1 ddc1, bool seperateIq=false,int bufferSize=131072)
        {
            _floatBuffer = new float[bufferSize];

            _ddc1 = ddc1;
            SeperateIq = seperateIq;

            _converter=new FloatConverter(SamplingRate(),Bits(),ChannelCount());
            _step = _converter.Step()*ChannelCount();
        }
    }
}
