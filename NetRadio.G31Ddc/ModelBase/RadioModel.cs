using System.Collections.Generic;
using NetRadio.Devices;
using NetRadio.Devices.G3XDdc;
using NetRadio.Devices.G3XDdc.Signal;
using NetRadio.G31Ddc.ModelBase;
using NetRadio.Signal;
using NetRadio.Devices.G31Ddc;
using NetRadio.G31Ddc.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRadio.G31Ddc
{
    public class RadioModel : IDisposable
    {
        public const int IF_FFT = 32768;
        public const int DDC1_FFT = 32768;
        public const int DDC2_FFT = 4096;

        private G3XDdcIfProvider _ifProvider;
        private G3XDdcDdc1StreamProvider _ddc1Provider;
        public SettingsModel Settings { get; private set; }
        public G31DdcRadioLimits Limits { get; private set; }

        public LiveFftAnalyzer FftAnalyzerDdc1 { get; private set; }
        public LiveFftAnalyzer FftAnalyzerIf { get; private set; }

        public G31DdcRadio Radio { get; private set; }
        public DdcInfo[] AvailableDdc1 { get; private set; }
        public Ddc2Model[] Ddc2 { get; private set; }

        public uint Ddc1Frequency
        {
            get { return Radio.Ddc1().Frequency(); }
            set { Radio.Ddc1().Frequency(value); }
        }

        public AttenuatorValue Attenuator
        {
            get { return Radio.Attenuator(); }
            set { Radio.Attenuator(value); }
        }

        public bool Dithering
        {
            get { return Radio.Dithering(); }
            set { Radio.Dithering(value); }
        }

        public bool MwFilter
        {
            get { return Radio.MwFilter(); }
            set { Radio.MwFilter(value); }
        }

        public bool PowerOn
        {
            get { return Radio.Power(); }
            set { Radio.Power(value); }
        }

        public NoiseBlanker AdcNoiseBlanker
        {
            get { return Radio.AdcNoiseBlanker(); }
            set { Radio.AdcNoiseBlanker(value); }
        }

        public void Initialize(/*uint index*/)
        {
            Limits = new G31DdcRadioLimits(Radio);

            Settings = SettingsModel.LoadOrCreate();
            Settings.Initialize();

            FillAvailableDdc1();

            Radio.Power(true);
            Radio.Ddc1().Ddc((uint)(AvailableDdc1.Length - 1));

            Start(/*index*/);
        }

        public void Start(/*uint index*/)
        {
            Radio.StartIf(100);
            Radio.Ddc1().Start(2048 * 16);

            Ddc2[0].Start();
            Ddc2[1].Start();
            // Ddc2[2].Start();
        }

        public void Stop(/*uint index*/)
        {
            Ddc2[0].Stop();
            Ddc2[1].Stop();
            // Ddc2[2].Stop();

            Radio.Ddc1().Stop();

            Radio.StoptIf();
        }

        private void FillAvailableDdc1()
        {
            var c = Radio.Ddc1().DdcCount();
            var ddcInfos = new List<DdcInfo>();
            for (uint i = 0; i < c; i++)
                ddcInfos.Add(Radio.DdcInfo(i));

            AvailableDdc1 = ddcInfos.ToArray();
        }

        public RadioModel()
        {
            BasicRadioInfo basicRadioInfo = new BasicRadioInfo();

            var basicRadioInfos = Radio<G31DdcRadio>.Find().ListAll();
            bool b = basicRadioInfo.IsEmulatedDevice();

            if (basicRadioInfos.Count > 0)
            {
                basicRadioInfo = basicRadioInfos.First();
                // this.Title = "NMG31";
                // this.Title = String.Format("WR-G31DDC - {1}", basicRadioInfo.Name, basicRadioInfo.Serial);
            }

            Radio = basicRadioInfo.Open<G31DdcRadio>();

            Ddc2 = new Ddc2Model[3];
            Ddc2[0] = new Ddc2Model(Radio, 0);
            Ddc2[1] = new Ddc2Model(Radio, 1);
            Ddc2[2] = new Ddc2Model(Radio, 2);

            _ifProvider = new G3XDdcIfProvider(Radio);
            _ddc1Provider = new G3XDdcDdc1StreamProvider(Radio.Ddc1());

            FftAnalyzerIf = new LiveFftAnalyzer(_ifProvider, IF_FFT);
            FftAnalyzerIf.Start();

            FftAnalyzerDdc1 = new LiveFftAnalyzer(_ddc1Provider, DDC1_FFT, false) { ForceInstantFft = false };
            FftAnalyzerDdc1.Start();
        }

        public void Dispose()
        {
            FftAnalyzerDdc1.Stop();
            FftAnalyzerIf.Stop();
        }
    }
}
