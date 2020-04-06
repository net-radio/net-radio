using System.Runtime.InteropServices;
using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers.ExciterParser2Kw
{
    [StructLayout(LayoutKind.Explicit)]
    public class Modulator2KwImpl : Modulator, IParser
    {
        public const uint ID_MODULATOR_SOURCE = 61;
        public const uint ID_MODULATOR_FREQUENCY = 62;
        public const uint ID_MODULATOR_SWEEP = 63;
        public const uint ID_MODULATOR_STATUS = 64;
        public const uint ID_MODULATOR_STEP = 83;

        [FieldOffset(0)]
        private byte _tone1Lsb;
        [FieldOffset(1)]
        private byte _tone1Msb;
        [FieldOffset(0)]
        private ushort _tone1;

        [FieldOffset(2)]
        private byte _tone2Lsb;
        [FieldOffset(3)]
        private byte _tone2Msb;
        [FieldOffset(2)]
        private ushort _tone2;

        [FieldOffset(4)]
        private byte _freq0;
        [FieldOffset(5)]
        private byte _freq1;
        [FieldOffset(6)]
        private byte _freq2;
        [FieldOffset(7)]
        private byte _freq3;
        [FieldOffset(4)]
        private uint _freq;

        [FieldOffset(8)]
        private byte _modulation;

        [FieldOffset(9)]
        private byte _vfLsb;
        [FieldOffset(10)]
        private byte _vfMsb;
        [FieldOffset(9)]
        private ushort _vf;

        [FieldOffset(11)]
        private byte _startFreq0;
        [FieldOffset(12)]
        private byte _startFreq1;
        [FieldOffset(13)]
        private byte _startFreq2;
        [FieldOffset(14)]
        private byte _startFreq3;
        [FieldOffset(11)]
        private uint _startFreq;

        [FieldOffset(15)]
        private byte _stopFreq0;
        [FieldOffset(16)]
        private byte _stopFreq1;
        [FieldOffset(17)]
        private byte _stopFreq2;
        [FieldOffset(18)]
        private byte _stopFreq3;
        [FieldOffset(19)]
        private uint _stopFreq;

        [FieldOffset(20)]
        private byte _warningFlag;
        [FieldOffset(21)]
        private byte _errorFlag;
        [FieldOffset(22)]
        private byte _vforEq;
        [FieldOffset(23)]
        private byte _vrefEq;

        [FieldOffset(24)]
        private byte _aSource1;
        [FieldOffset(25)]
        private byte _noiseBandwidth1;

        [FieldOffset(26)]
        private byte _aSource2;
        [FieldOffset(27)]
        private byte _noiseBandwidth2;

        [FieldOffset(28)]
        private byte _stepFreq0;
        [FieldOffset(29)]
        private byte _stepFreq1;
        [FieldOffset(30)]
        private byte _stepFreq2;
        [FieldOffset(31)]
        private byte _stepFreq3;
        [FieldOffset(32)]
        private uint _stepFreq;

        public byte ASource1
        {
            get { return _aSource1; }
        }

        public byte NoiseBandwidth1
        {
            get { return _noiseBandwidth1; }
        }

        public byte ASource2
        {
            get { return _aSource2; }
        }

        public byte NoiseBandwidth2
        {
            get { return _noiseBandwidth2; }
        }

        public ushort Tone1
        {
            get { return _tone1; }
        }

        public ushort Tone2
        {
            get { return _tone2; }
        }

        public uint Frequency
        {
            get { return _freq; }
        }

        public uint SweepStartFrequency
        {
            get { return _startFreq; }
        }

        public uint SweepStopFrequency
        {
            get { return _stopFreq; }
        }

        public byte WarningFlag
        {
            get { return _warningFlag; }
        }

        public byte ErrorFlag
        {
            get { return _errorFlag; }
        }

        public byte VforEq
        {
            get { return _vforEq; }
        }

        public byte VrefEq
        {
            get { return _vrefEq; }
        }

        public ExciterModulation Modulation
        {
            get { return (ExciterModulation)_modulation; }
        }

        public uint StepFrequency
        {
            get { return _stepFreq; }
        }

        public void Update(CanFrame frame)
        {
            switch (frame.Id)
            {
                case ID_MODULATOR_SOURCE:
                    ModulatorSource(frame);
                    break;
                case ID_MODULATOR_FREQUENCY:
                    ModulatorFrequency(frame);
                    break;
                case ID_MODULATOR_SWEEP:
                    ModulatorSweep(frame);
                    break;
                case ID_MODULATOR_STATUS:
                    ModulatorStatus(frame);
                    break;
                case ID_MODULATOR_STEP:
                    ModulatorStep(frame);
                    break;
            }
        }

        private void ModulatorStep(CanFrame frame)
        {
            _stepFreq0 = frame[0];
            _stepFreq1 = frame[1];
            _stepFreq2 = frame[2];
            _stepFreq3 = frame[3];
        }

        private void ModulatorStatus(CanFrame frame)
        {
            _warningFlag = frame[0];
            _errorFlag = frame[1];
            _vforEq = frame[2];
            _vrefEq = frame[3];
        }

        private void ModulatorSweep(CanFrame frame)
        {
            _startFreq0 = frame[0];
            _startFreq1 = frame[1];
            _startFreq2 = frame[2];
            _startFreq3 = frame[3];

            _stopFreq0 = frame[4];
            _stopFreq1 = frame[5];
            _stopFreq2 = frame[6];
            _stopFreq3 = frame[7];
        }

        private void ModulatorFrequency(CanFrame frame)
        {
            _freq0 = frame[0];
            _freq1 = frame[1];
            _freq2 = frame[2];
            _freq3 = frame[3];
            _modulation = frame[4];
            _vfLsb = frame[5];
            _vfMsb = frame[6];
        }

        private void ModulatorSource(CanFrame frame)
        {
            _aSource1 = frame[0];
            _noiseBandwidth1 = frame[1];
            _tone1Lsb = frame[2];
            _tone1Msb = frame[3];

            _aSource2 = frame[4];
            _noiseBandwidth2 = frame[5];
            _tone2Lsb = frame[6];
            _tone2Msb = frame[7];
        }

        internal Modulator2KwImpl(IDispatcher dispatcher)
        {
            dispatcher.Register(ID_MODULATOR_FREQUENCY, this);
            dispatcher.Register(ID_MODULATOR_SOURCE, this);
            dispatcher.Register(ID_MODULATOR_STATUS, this);
            dispatcher.Register(ID_MODULATOR_STEP, this);
            dispatcher.Register(ID_MODULATOR_SWEEP, this);
        }
    }
}
