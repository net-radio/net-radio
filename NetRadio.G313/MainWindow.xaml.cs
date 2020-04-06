using System;
using System.Windows;
using NetRadio.Devices.G313;
using NetRadio.G313.Model;
using NetRadio.G313.PanelViewModel;

namespace NetRadio.G313
{
    public partial class MainWindow
    {
        #region Fields
        private readonly RadioViewModel _vm;
        private string _startError;
        #endregion

        #region .ctor
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                var model = new RadioModel();
                if (model.AvailableRadio.Length == 0)
                    return;

                if (model.AvailableRadio.Length > 0)
                {
                    var radio = model.AvailableRadio[0];
                    /*
                    if (radio.IsEmulatedDevice())
                        return;
                    */
                }


                model.Initialize(0);

                InitRadio(model);

                //model.Radio.Demodulator().SetupStreams();
                model.InitializeStreams();
                model.ResumeStreams();

                _vm = new RadioViewModel(model, Waveform, FixedChannelChart);
                _vm.ReInitDataContext = () =>
                {
                    DataContext = null;
                    DataContext = _vm;
                };
                DataContext = _vm;
            }
            catch (Exception e)
            {
                _startError = string.Format("Failed to Initialize.\nInfo:\n{0}", e);
            }
        }

        private void InitRadio(RadioModel model)
        {
            var radio = model.Radio;
            if (radio == null)
            {
                MessageBox.Show("radio not found.");
                return;
            }

            radio.Power(true).
                    Attenuator(false).
                    Agc(Agc.Fast).
                    Frequency(28600000).
                    Demodulator().
                    IfBandwidth(15000).
                    DisableSofwareAgc().
                    Mode(G313Demodulator.DemodulatorMode.Fmn).
                    AudioBandwidth(16000).
                    AudioGain(32).
                    Volume(16).
                    SetupStreams();
        }
        #endregion

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            if (_vm != null && string.IsNullOrEmpty(_startError))
                return;

            MessageBox.Show(
                "No compatible radio found,Please connect a G313 compatible radio hardware.\n The application will close now");

            if (!string.IsNullOrEmpty(_startError))
                MessageBox.Show(_startError);

            Close();
        }
    }
}
