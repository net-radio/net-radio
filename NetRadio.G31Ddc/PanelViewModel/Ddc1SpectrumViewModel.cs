using NetRadio.Devices.G3XDdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.PanelViewModel
{
    public class Ddc1SpectrumViewModel:SpectrumViewModel
    {
        public Ddc1SpectrumViewModel(RadioViewModel radioViewModel, Arction.ISpectrumAnalyzer spectrumAnalyzer)
            : base(radioViewModel, spectrumAnalyzer)
        {
            SelectedResolution = Resolutions.ToArray<ResolutionBandwidth>()[4];
        }

        private ResolutionBandwidth _selectedResolution;        

        public override ResolutionBandwidth SelectedResolution
        {
            get { return _selectedResolution; }
            set
            {
                _selectedResolution = value;
                int sampleCount = RadioModel.DDC1_FFT / _selectedResolution.Resolution;
                uint sampleRate = RadioView.Ddc1Info.SampleRate;

                if (SpectrumAnalyzer != null)
                {
                    SpectrumAnalyzer.PreInitialize(RadioView.Ddc1Frequency - sampleRate / 2.0, RadioView.Ddc1Frequency + sampleRate / 2.0, RadioView.Ddc1Frequency - sampleRate / 2.0, (double)sampleRate / sampleCount);
                    SpectrumAnalyzer.ReInitialize(sampleCount);
                    //SpectrumAnalyzer.SetSpectrumFrequencyRange(RadioView.Ddc1Frequency - sampleRate / 2.0, RadioView.Ddc1Frequency + sampleRate / 2.0);
                    //SpectrumAnalyzer.SetSampleFrequencyRange(RadioView.Ddc1Frequency - sampleRate / 2.0, RadioView.Ddc1Frequency + sampleRate / 2.0);
                    SpectrumAnalyzer.SetNotch(12000, 12000);
                    NotifyPropertyChanged("SelectedResolution");
                }
            }
        }

        public override uint Bandwidth
        {
            get
            {
                // return RadioView.Ddc2Info.SampleRate;
                return RadioView.DemodulatorBandwidth;
            }
            set
            {
                if (value > 0)
                {
                    RadioView.DemodulatorBandwidth = value;
                    NotifyPropertyChanged("Bandwidth");
                }
            }
        }

        public override long Shift
        {
            get
            {
                return RadioView.Ddc2AbsoluteFrequency;
            }
            set
            {
                long relativeFrequency = value - RadioView.Ddc1Frequency;
                if (relativeFrequency >= int.MinValue && relativeFrequency <= int.MaxValue)
                {
                    RadioView.Ddc2RelativeFrequency = (int)relativeFrequency;
                    NotifyPropertyChanged("Shift");
                }
            }
        }

        /*
        public override double SpectrumStartFrequency
        {
            get
            {
                uint index;
                // DdcInfo info = RadioView.Radio.Radio.Ddc2Info(out index);
                DdcInfo info = RadioView.Ddc1Info;
                uint bandwidth = info.SampleRate;


                // long frequency = RadioView.Radio.Ddc1Frequency;
                // return frequency - bandwidth / 2;
                return bandwidth * -0.5;
            }
        }

        public override double SpectrumStopFrequency
        {
            get
            {
                uint index;
                // DdcInfo info = RadioView.Radio.Radio.Ddc2Info(out index);
                DdcInfo info = RadioView.Radio.Radio.Ddc1().DdcArgs().Info;
                uint bandwidth = info.SampleRate;


                // long frequency = RadioView.Radio.Ddc1Frequency;
                // return frequency + bandwidth / 2;
                return bandwidth / 2;
            }
        }
        */

        public override double Precision
        {
            get
            {
                int sampleCount = RadioModel.DDC1_FFT / _selectedResolution.Resolution;
                uint sampleRate = RadioView.Ddc1Info.SampleRate;
                return (double)sampleRate / sampleCount;
            }
        }

        public override double StartFrequency
        {
            get { return RadioView.Ddc1Frequency - (double)RadioView.Ddc1Info.SampleRate / 2f; }
        }

        public override double StopFrequency
        {
            get { return RadioView.Ddc1Frequency + (double)RadioView.Ddc1Info.SampleRate / 2f; }
        }
    }
}
