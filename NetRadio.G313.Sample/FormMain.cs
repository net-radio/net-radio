using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using NetRadio.Devices;
using NetRadio.Devices.G313;
using NetRadio.Devices.G313.Signal;
using NetRadio.Signal;
using NetRadio.Signal.Utilities;

namespace NetRadio.G313.Sample
{
    public partial class FormMain : Form
    {
        private Model _model;

        public FormMain()
        {
            InitializeComponent();

            PowerOn();

            UpdateStatus();
        }

        private void PowerOn()
        {
            _model = new Model();
            _model.Initialize(0);

            InitRadio();
            _model.InitializeStreams();

            _model.LiveIf.FftCalculated += LiveIf_FftCalculated;
            _model.LiveAudio.FftCalculated += LiveAudio_FftCalculated;
            _model.FftAnalyzer.FftCalculated += FftAnalyzer_FftCalculated;
        }

        private void FftAnalyzer_FftCalculated(object sender, FftEventArgs e)
        {
            FrequencyBins bin;
            spectrumAnalyzer.Clear();
            if (!_model.VideoFilter)
                bin = new FrequencyBins(e, _model.Radio.BinParametersDefault());
            else
                bin = new FrequencyBins(e,
                    _model.Radio.BinParametersVideoFilter(_model.FftAnalyzer.FftLength, _model.VideoPoints));
            Debug.Print("peak at:{0}: {1}", bin.MaxIntensityAt(),bin.MaxIntensity());

            spectrumAnalyzer.Update(bin);

            TaskUtility.Run(() =>
            {
                Invoke(new Action(() =>
                {
                    try
                    {
                        labelPeakFrequency.Text = (bin.MaxIntensityAt()/1000000).ToString();
                        labelFrequenccyError.Text =
                            ((bin.MaxIntensityAt() - (double) numericUpDownFrequency.Value)/1000000).ToString();
                        labelPowerDbm.Text = _model.Radio.Demodulator().SignalStrength().ToString();
                        labelPowerUVolt.Text =
                            RfMath.DbmToMicroVolts(_model.Radio.Demodulator().SignalStrength()).ToString();
                    }
                    catch
                    {
                        
                    }
                }));
            });
        }

        void LiveAudio_FftCalculated(object sender, FftEventArgs e)
        {
            var bin = new FrequencyBins(e, _model.Radio.BinParametersDefault());
            Debug.Print("peak at:{0}", bin.MaxIntensityAt());

            spectrumAnalyzer.Update(bin);
        }

        void LiveIf_FftCalculated(object sender, FftEventArgs e)
        {
            var bin = new FrequencyBins(e, _model.Radio.BinParametersDefault());
            Debug.Print("peak at:{0}", bin.MaxIntensityAt());

            spectrumAnalyzer.Update(bin);
        }

        private void UpdateStatus()
        {
            if (_model.Radio == null)
                return;

            toolStripStatusLabelName.Text = _model.Radio.CachedInfo.Name;
            toolStripStatusLabelSerial.Text = _model.Radio.CachedInfo.Serial;

            textBoxAudioFile.Text = _model.AudioFile;
            textBoxAudioMp3File.Text = _model.AudioMp3File;

            textBoxIfFile.Text = _model.IfFile;
            textBoxIfMp3File.Text = _model.IfMp3File;
        }

        private void InitRadio()
        {
            var radio = _model.Radio;
            if (radio == null)
            {
                MessageBox.Show("radio not found.");
                return;
            }

            radio.Power(true).
                    Attenuator(false).
                    Agc(Agc.Fast).
                    Frequency(88600000).
                    Demodulator().
                    IfBandwidth(15000).
                    DisableSofwareAgc().
                    Mode(G313Demodulator.DemodulatorMode.Fmn).
                    AudioBandwidth(16000).
                    AudioGain(32).
                    Volume(16).
                    SetupStreams();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _model.Dispose();
        }

        private void buttonLiveStart_Click(object sender, EventArgs e)
        {
            _model.LiveAudio.Play();
        }

        private void buttonLiveStop_Click(object sender, EventArgs e)
        {
            _model.LiveAudio.Stop();
        }

        private void buttonAudioStart_Click(object sender, EventArgs e)
        {
            _model.RecordAudio.Record(File.Open(_model.AudioFile, FileMode.Create));
        }

        private void buttonAudioStop_Click(object sender, EventArgs e)
        {
            _model.RecordAudio.Stop();
        }

        private void buttonIfStart_Click(object sender, EventArgs e)
        {
            _model.RecordIf.Record(File.Open(_model.IfFile, FileMode.Create));
        }

        private void buttonIfStop_Click(object sender, EventArgs e)
        {
            _model.RecordIf.Stop();
        }

        private void buttonAudioMp3Start_Click(object sender, EventArgs e)
        {
            _model.RecordMp3Audio.Record(File.Open(_model.AudioMp3File, FileMode.Create));
        }

        private void buttonAudioMp3Stop_Click(object sender, EventArgs e)
        {
            _model.RecordMp3Audio.Stop();
        }

        private void buttonIfMp3Start_Click(object sender, EventArgs e)
        {
            _model.RecordMp3If.Record(File.Open(_model.IfMp3File, FileMode.Create));
        }

        private void buttonifMp3Stop_Click(object sender, EventArgs e)
        {
            _model.RecordMp3If.Stop();
        }

        private void buttonLiveIfStart_Click(object sender, EventArgs e)
        {
            _model.LiveIf.Play();
        }

        private void buttonLiveIfStop_Click(object sender, EventArgs e)
        {
            _model.LiveIf.Stop();
        }

        private void radioButtonAgcOff_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Agc(Agc.Off);
        }

        private void radioButtonAgcSlow_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Agc(Agc.Slow);
        }

        private void radioButtonAgcMedium_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Agc(Agc.Medium);
        }

        private void radioButtonAgcFast_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Agc(Agc.Fast);
        }

        private void radioButtonAttenOff_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Attenuator(false);
        }

        private void radioButtonAttenOn_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Attenuator(true);
        }

        private void buttonOff_Click(object sender, EventArgs e)
        {
            _model.Radio.Power(false);
            _model.Dispose();
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            PowerOn();
            //_model.Radio.Power(true);
        }

        private void radioButtonAm_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Am);
        }

        private void radioButtonIsb_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Isb);
        }

        private void radioButtonAms_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Ams);
        }

        private void radioButtonDsb_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Dsb);
        }

        private void radioButtonLsb_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Lsb);
            numericUpDownIfBandwidth.Value = 7500;
            numericUpDownIfShift.Value = -3750;
            buttonIfBandwidth_Click(null, null);
            buttonIfShift_Click(null, null);
        }

        private void radioButtonFmn_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Fmn);
        }

        private void radioButtonUsb_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Usb);
            numericUpDownIfBandwidth.Value = 7500;
            numericUpDownIfShift.Value = 3750;
            buttonIfBandwidth_Click(null, null);
            buttonIfShift_Click(null, null);
        }

        private void radioButtonCw_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Mode(G313Demodulator.DemodulatorMode.Cw);
        }

        private void buttonIfBandwidth_Click(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().IfBandwidth((uint)numericUpDownIfBandwidth.Value);
            spectrumAnalyzer.IfBandwidth = (int) numericUpDownIfBandwidth.Value;
        }

        private void buttonAudioBandwidth_Click(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().AudioBandwidth((uint)numericUpDownAudioBandwidth.Value);
        }

        private void buttonFrequency_Click(object sender, EventArgs e)
        {
            _model.Radio.Frequency((uint)numericUpDownFrequency.Value);
        }

        private void buttonVolume_Click(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().Volume((uint)numericUpDownVolume.Value);
        }

        private void buttonAudioGain_Click(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().AudioGain((uint)numericUpDownAudioGain.Value);
        }

        private void radioButtonSagcOff_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().DisableSofwareAgc();
        }

        private void radioButtonSAgcSlow_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().SoftwareAgc(new SoftwareAgc { ReferenceLevel = -8, AttackTime = 25, DecayTime = 4000 });
        }

        private void radioButtonSAgcMedium_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().SoftwareAgc(new SoftwareAgc { ReferenceLevel = -8, AttackTime = 15, DecayTime = 2000 });
        }

        private void radioButtonSagcFast_CheckedChanged(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().SoftwareAgc(new SoftwareAgc { ReferenceLevel = -8, AttackTime = 5, DecayTime = 200 });
        }

        private void buttonMute_Click(object sender, EventArgs e)
        {
            _model.LiveIf.Volume(0.0F);
        }

        private void buttonUnMute_Click(object sender, EventArgs e)
        {
            _model.LiveIf.Volume(1.0F);
        }

        private void buttonFftOn_Click(object sender, EventArgs e)
        {
            _model.FftAnalyzer.Start();
        }

        private void buttonFftOff_Click(object sender, EventArgs e)
        {
            _model.FftAnalyzer.Stop();
        }

        private void buttonIfGain_Click(object sender, EventArgs e)
        {
            var res = _model.Radio.IfGain();
            var res2 = _model.Radio.If2Frequency();
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            buttonFftOff_Click(null, null);
            buttonLiveIfStop_Click(null, null);
            buttonLiveStop_Click(null, null);

            var sweepedData = new List<SweepedFrequency>();
            var sweeper = _model.Radio.Sweeper(uint.Parse(textBoxSweepFrom.Text), uint.Parse(textBoxSweepTo.Text));
            var points = sweeper.SampleCount();
            sweeper.FrequencySweeped += (s, ee) => sweepedData.AddRange(ee.Data);
            sweeper.SweepDone += (s, ee) =>
            {
                Invoke(new Action(() => sweeperAnalyzer.Update(sweepedData)));
                sweepedData.Clear();
            };
            sweeper.SweepAsync();


            //var scanner = _model.Radio.BlockScanner();
            //scanner.FrequencyScanned += (s, ee) => Debug.Print("frequency:{0}, power:{1}, raw:{2}", ee.Frequency, (ee.Strength * 1000 / 255 - 1300) / 10, ee.Strength);
            //scanner.ScanFinished += (s, ee) => Debug.Print("finished.");

            //var list = new List<uint>();
            //for (uint i = 88590000; i < 88610000; i += 15)
            //    list.Add(i);

            //scanner.Start(list);
        }

        private void buttonLastdBm_Click(object sender, EventArgs e)
        {
            _model.Radio.Agc(Agc.Off).IfGain(50);

            //this.buttonLastdBm.Text = (_model.Radio.Demodulator().SignalStrength()/10).ToString();
        }

        private void textBoxAudioFile_TextChanged(object sender, EventArgs e)
        {
            _model.AudioFile = textBoxAudioFile.Text;
        }

        private void textBoxIfFile_TextChanged(object sender, EventArgs e)
        {
            _model.IfFile = textBoxIfFile.Text;
        }

        private void textBoxAudioMp3File_TextChanged(object sender, EventArgs e)
        {
            _model.AudioMp3File = textBoxAudioMp3File.Text;
        }

        private void textBoxIfMp3File_TextChanged(object sender, EventArgs e)
        {
            _model.IfMp3File = textBoxIfMp3File.Text;
        }

        private void buttonIfShift_Click(object sender, EventArgs e)
        {
            _model.Radio.Demodulator().IfShift((int)numericUpDownIfShift.Value);
            spectrumAnalyzer.IfShift = (int) numericUpDownIfShift.Value;

            numericUpDownIfShift.Value = _model.Radio.Demodulator().IfShift();
        }

        private void buttonNotchEnable_Click(object sender, EventArgs e)
        {
            _model.Radio.Demodulator()
                .NotchFilter(new NotchFilter
                {
                    Active = true,
                    Bandwidth = (uint)numericUpDownNotchBandwidth.Value,
                    Frequency = (int)numericUpDownNotchFreq.Value
                });

            spectrumAnalyzer.NotchFreq = (int) numericUpDownNotchFreq.Value;
            spectrumAnalyzer.NotchBandwidth = (int) numericUpDownNotchBandwidth.Value;
        }

        private void buttonNotchDisable_Click(object sender, EventArgs e)
        {
            _model.Radio.Demodulator()
                .NotchFilter(new NotchFilter
                {
                    Active = false,
                    Bandwidth = (uint)numericUpDownNotchBandwidth.Value,
                    Frequency = (int)numericUpDownNotchFreq.Value
                });

            spectrumAnalyzer.NotchFreq = 0;
            spectrumAnalyzer.NotchBandwidth = 0;
        }

        private void buttonEnableVideoFilter_Click(object sender, EventArgs e)
        {
            //var val=_model.Radio.If2Frequency();
            //_model.Radio.TryIf2Frequency(20000);

            _model.VideoFilter = true;
            _model.VideoPoints = (int) numericUpDownVideoFilter.Value;

            spectrumAnalyzer.Clear();
        }

        private void buttonDisableVideoFilter_Click(object sender, EventArgs e)
        {
            _model.VideoFilter = false;
            //spectrumAnalyzer.Clear();
        }

        private void openSoundControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsHelpers.OpenPlaybackConfig();
        }
    }
}
