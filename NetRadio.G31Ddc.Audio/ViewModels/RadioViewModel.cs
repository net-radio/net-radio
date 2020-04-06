using NetRadio.Devices;
using NetRadio.Devices.Exceptions;
using NetRadio.Devices.G3XDdc;
using NetRadio.G31Ddc.Audio.Models;
using NetRadio.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Audio.ViewModels
{
    class RadioViewModel : ViewModelBase, IDisposable
    {
        private readonly RadioModel _model;
        // private uint _channel = 0;
        // private FastFrequencyBins _ddcBin;

        public RadioViewModel()
        {
            _model = new RadioModel();
            try
            {
                _model.Initialize();
                InitializeReceiver();
            }
            catch (OperationFailedException ex)
            {
                logger.Debug("Failed to initialize radio.", ex);
                _model.Dispose();
                throw;
            }
        }

        private void InitializeReceiver()
        {
            AbsoluteFrequency = 10000000;

            DemodulatorBandwidth = 7500;

            _model.Ddc2[0].Gain = 150;

            /*
            _model.Ddc2[0].Agc = new SoftwareAgc
            {
                ReferenceLevel = 0
            };
            */
 
            _model.Ddc2[0].AgcActive = true;

            _model.Ddc2[0].AudioFilter = new AudioFilter
            {
                CutOffHigh = 3500,
                CutOffLow = 300,
                DeEmphasis = -6.0
            };

            _model.Ddc2[0].AudioGain = 12;


            /*
            _model.Ddc2[_channel].AbsoluteFrequency = 585000;

            _model.Ddc2[_channel].PlayStart();

            */

            /*
            _ddcBin =
                new FastFrequencyBins(
                    new BinParameters
                    {
                        MinimumIntensityDb = -120,
                        SamplingRate = 2500000,
                        CarrierFrequency = _model.Ddc1Frequency,
                        Relative = true
                    }, _model.FftAnalyzerDdc1.FftLength, false);
            _model.FftAnalyzerDdc1.FftCalculated += FftAnalyzerDdc1_FftCalculated;
            */

            _model.Ddc2[0].PlayStart();
            _model.Ddc2[0].AudioPlayer.Volume(1);
        }

        private void FftAnalyzerDdc1_FftCalculated(object sender, Signal.FftEventArgs e)
        {
            // _ddcBin.Update(e);
        }

        public uint AbsoluteFrequency
        {
            get
            {
                // return _model.Radio.AbsoluteFrequency(1);
                return _model.Ddc2[0].AbsoluteFrequency;
            }
            set
            {
                var frequency = _model.Limits.Frequency(value);
                _model.Ddc2[0].AbsoluteFrequency = frequency;
                // _model.Radio.AbsoluteFrequency(frequency, 1);

                long bandwidth = _model.Radio.Ddc1().DdcArgs().Info.Bandwidth;
                long sampleRate = _model.Radio.Ddc1().DdcArgs().Info.SampleRate;
                long ddc1Frequency = _model.Ddc1Frequency;

                NotifyPropertyChanged("Ddc1Frequency");
                NotifyPropertyChanged("Ddc2RelativeFrequency");
                NotifyPropertyChanged("Ddc2AbsoluteFrequency");
                NotifyPropertyChanged("AbsoluteFrequency");
            }
        }

        public String Name
        {
            get { return _model.DeviceInfo.Name; }
        }

        public String Serial
        {
            get { return _model.DeviceInfo.Serial; }
        }

        public float Volume
        {
            get { return _model.Ddc2[0].AudioPlayer.VolumeLevel; }
            set
            {
                _model.Ddc2[0].AudioPlayer.VolumeLevel = value;
                NotifyPropertyChanged("Volume");
            }
        }


        private string _morseText;

        public string MorseText
        {
            get { return _morseText; }
            set
            {
                _morseText = value;
                NotifyPropertyChanged("MorseText");
            }
        }

        public void Dispose()
        {
            _model.Stop();
            _model.Dispose();
        }

        public DemodulatorMode ModeState
        {
            get
            {
                if (_model.Ddc2[0].DemodulatorMode == DemodulatorMode.Lsb
                    && _model.Ddc2[1].DemodulatorMode == DemodulatorMode.Usb
                    && _model.Ddc2[1].IsStart)
                    return DemodulatorMode.Dsb;
                else
                    return _model.Ddc2[0].DemodulatorMode;
            }
            set
            {
                switch (value)
                {
                    case DemodulatorMode.Dsb:
                        _model.Ddc2[1].Start();
                        _model.Ddc2[0].DemodulatorMode = DemodulatorMode.Lsb;
                        _model.Ddc2[1].DemodulatorMode = DemodulatorMode.Usb;

                        if (MuteState)
                            Mute();
                        else
                            Unmute();
                        break;
                    case DemodulatorMode.Isb:
                        break;
                    default:
                        _model.Ddc2[1].Stop();
                        _model.Ddc2[0].DemodulatorMode = value;
                        break;
                }

                NotifyPropertyChanged("ModeState");
            }
        }

        private void Mute()
        {
            _model.Ddc2[0].Mute();
        }

        private void Unmute()
        {
            switch (ModeState)
            {
                case DemodulatorMode.Dsb:
                    _model.Ddc2[0].Unmute(true, false);
                    _model.Ddc2[1].Unmute(false, true);
                    break;

                default:
                    _model.Ddc2[0].Unmute();
                    break;
            }
        }        

        private bool _muteState = false;
        public bool MuteState
        {
            get { return _muteState; }
            set
            {
                _muteState = value;
                if (value)
                    _model.Ddc2[0].Mute();
                else
                    _model.Ddc2[0].Unmute();
                NotifyPropertyChanged("MuteState");
            }
        }

        public uint DemodulatorBandwidth
        {
            get { return _model.Ddc2[0].DemodulatorBandwidth; }
            set
            {
                _model.Ddc2[0].DemodulatorBandwidth = value;
                NotifyPropertyChanged("DemodulatorBandwidth");
            }
        }



    }
}
