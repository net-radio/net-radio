using NetRadio.Devices.G3XDdc;
using NetRadio.G31Ddc.Arction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NetRadio.G31Ddc.PanelViewModel
{
    public class IfSpectrumViewModel : SpectrumViewModel
    {
        public IfSpectrumViewModel(RadioViewModel radioViewModel, ISpectrumAnalyzer spectrumAnalyzer)
            : base(radioViewModel, spectrumAnalyzer)
        {
            SelectedResolution = Resolutions.ToArray<ResolutionBandwidth>()[3];
        }

        private ResolutionBandwidth _selectedResolution;

        public override ResolutionBandwidth SelectedResolution
        {
            get { return _selectedResolution; }
            set
            {
                _selectedResolution = value;
                int sampleCount = RadioModel.IF_FFT / 2;
                sampleCount /= _selectedResolution.Resolution;

                if (SpectrumAnalyzer != null)
                {
                    SpectrumAnalyzer.PreInitialize(0f, 32000000f, 0f, 50000000f / sampleCount);
                    SpectrumAnalyzer.ReInitialize(sampleCount);

                    //SpectrumAnalyzer.SetSpectrumFrequencyRange(0, 32000000);
                    //SpectrumAnalyzer.SetSampleFrequencyRange(0, 50000000);
                    SpectrumAnalyzer.SetNotch(12000, 12000);
                    NotifyPropertyChanged("SelectedResolution");
                }
            }
        }

        public override uint Bandwidth
        {
            get
            {
                return RadioView.Radio.Radio.Ddc1().DdcArgs().Info.SampleRate;
            }
            set
            {
                DdcInfo ddcInfo = null;
                foreach (var ddc1 in RadioView.Radio.AvailableDdc1)
                {
                    ddcInfo = ddc1;
                    if (value <= ddc1.SampleRate)
                        break;                    
                }
                RadioView.ActiveDdc1 = ddcInfo;
                NotifyPropertyChanged("Bandwidth");
            }
        }

        public override long Shift
        {
            get
            {
                return RadioView.Radio.Ddc1Frequency;
            }
            set
            {
                if (value >= uint.MinValue && value <= uint.MaxValue)
                {
                    RadioView.Ddc1Frequency = (uint)value;
                    NotifyPropertyChanged("Shift");
                }
            }
        }

        //public override double SpectrumStartFrequency
        //{
        //    get { return 0.0; }
        //}

        //public override double SpectrumStopFrequency
        //{
        //    get { return 50000000f; }
        //}

        public override double Precision
        {
            get
            {
                int sampleCount = RadioModel.IF_FFT / 2;
                sampleCount /= _selectedResolution.Resolution;
                return 50000000f / sampleCount; 
            }
        }

        public override double StartFrequency
        {
            get { return 0f; }
        }

        public override double StopFrequency
        {
            get { return 32000000f; }
        }
    }
}
