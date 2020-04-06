using NetRadio.Devices.G3XDdc;
using NetRadio.G31Ddc.Arction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.PanelViewModel
{
    public class Ddc2SpectrumViewModel : SpectrumViewModel
    {
        public Ddc2SpectrumViewModel(RadioViewModel radioViewModel, ISpectrumAnalyzer spectrumAnalyzer)
            : base(radioViewModel, spectrumAnalyzer)
        {
            SelectedResolution = Resolutions.ToArray<ResolutionBandwidth>()[2];
        }

        private ResolutionBandwidth _selectedResolution;

        public override ResolutionBandwidth SelectedResolution
        {
            get { return _selectedResolution; }
            set
            {
                _selectedResolution = value;
                int sampleCount = RadioModel.DDC2_FFT / _selectedResolution.Resolution;
                uint sampleRate = RadioView.Ddc2Info.SampleRate;

                if (SpectrumAnalyzer != null)
                {
                    SpectrumAnalyzer.PreInitialize(RadioView.Ddc2AbsoluteFrequency - sampleRate / 2.0, RadioView.Ddc2AbsoluteFrequency + sampleRate / 2.0, RadioView.Ddc2AbsoluteFrequency - sampleRate / 2.0, (double)sampleRate / sampleCount);
                    SpectrumAnalyzer.ReInitialize(sampleCount);
                    //SpectrumAnalyzer.SetSpectrumFrequencyRange(RadioView.Ddc2AbsoluteFrequency - sampleRate / 2, RadioView.Ddc2AbsoluteFrequency + sampleRate / 2);
                    //SpectrumAnalyzer.SetSampleFrequencyRange(RadioView.Ddc2AbsoluteFrequency - sampleRate / 2, RadioView.Ddc2AbsoluteFrequency + sampleRate / 2);
                    SpectrumAnalyzer.SetNotch(12000, 12000);
                    NotifyPropertyChanged("SelectedResolution");
                }
            }
        }

        public override uint Bandwidth
        {
            get
            {
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
                return RadioView.Ddc2AbsoluteFrequency + RadioView.DemodulatorFrequency;
            }
            set
            {
                long demodulatorFrequency = value - RadioView.Ddc2AbsoluteFrequency;
                if (demodulatorFrequency >= int.MinValue && demodulatorFrequency <= int.MaxValue)
                {
                    RadioView.DemodulatorFrequency = (int)demodulatorFrequency;
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
                DdcInfo info = RadioView.Radio.Radio.Ddc2Info(out index);
                return info.SampleRate * -0.5;
            }
        }

        public override double SpectrumStopFrequency
        {
            get
            {
                uint index;
                DdcInfo info = RadioView.Radio.Radio.Ddc2Info(out index);
                return info.SampleRate * 0.5;
            }
        }
        */

        public override double Precision
        {
            get
            {
                int sampleCount = RadioModel.DDC2_FFT / _selectedResolution.Resolution;
                uint sampleRate = RadioView.Ddc2Info.SampleRate;
                return (double)sampleRate / sampleCount;
            }
        }

        public override double StartFrequency
        {
            get { return RadioView.Ddc2AbsoluteFrequency - (double)RadioView.Ddc2Info.SampleRate / 2f; }
        }

        public override double StopFrequency
        {
            get { return RadioView.Ddc2AbsoluteFrequency + (double)RadioView.Ddc2Info.SampleRate / 2f; }
        }
    }
}
