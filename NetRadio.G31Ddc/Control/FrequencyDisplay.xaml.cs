using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace NetRadio.G31Ddc.Control
{
    /// <summary>
    /// Interaction logic for FrequencyDisplay.xaml
    /// </summary>
    public partial class FrequencyDisplay : INotifyPropertyChanged
    {
        public static readonly DependencyProperty FrequencyProperty =
            DependencyProperty.Register("Frequency", typeof(uint), typeof(FrequencyDisplay),
            new PropertyMetadata(default(uint), OnFrequencyPropertyChanged));

        private uint _frequency;
        private string _frequencyText;

        public event PropertyChangedEventHandler PropertyChanged;

        public uint Frequency
        {
            get { return (uint) GetValue(FrequencyProperty); }
            set { SetValue(FrequencyProperty, value); }
        }

        private static void OnFrequencyPropertyChanged(DependencyObject dependencyObject,
               DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as FrequencyDisplay;
            control.OnPropertyChanged("Frequency");
            control.OnFrequencyPropertyChanged(e);
        }

        private void OnFrequencyPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            _frequency = Frequency;

            var khz = _frequency / 1000.0;
            var mhz = _frequency / 1000000.0;
            _frequencyText = khz > 1000 ? string.Format("{0} MHz", mhz) :khz<1? string.Format("{0} Hz", _frequency): string.Format("{0} Khz", khz);

            TextBox.Text = _frequencyText;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 

        public FrequencyDisplay()
        {
            InitializeComponent();
        }

        private void Enter()
        {
            try
            {
                var frequency = uint.Parse(TextBox.Text);
                Frequency = frequency;

                TextBox.Text = TextBox.IsFocused ? _frequency.ToString(CultureInfo.InvariantCulture) : _frequencyText;
            }
            catch (Exception)
            {
                //TODO: add input report
            }

        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Text = _frequency.ToString(CultureInfo.InvariantCulture);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            Enter();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
                Enter();
        }

        internal void OnInputPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
