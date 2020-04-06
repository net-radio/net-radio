using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace NetRadio.G313.Control
{
    /// <summary>
    /// Interaction logic for Knob.xaml
    /// </summary>
    public partial class Knob : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("KnobValue", typeof(long), typeof(Knob),
        new PropertyMetadata(default(long), OnValuePropertyChanged));

        public static readonly DependencyProperty MinProperty =
        DependencyProperty.Register("KnobMin", typeof(long), typeof(Knob),
        new PropertyMetadata(default(long), OnMinPropertyChanged));

        public static readonly DependencyProperty MaxProperty =
        DependencyProperty.Register("KnobMax", typeof(long), typeof(Knob),
        new PropertyMetadata(default(long), OnMaxPropertyChanged));

        public static readonly DependencyProperty StepProperty =
        DependencyProperty.Register("KnobStep", typeof(long), typeof(Knob),
        new PropertyMetadata(default(long), OnStepPropertyChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Timer _timer;
        private long _stepInUse;

        public long KnobValue
        {
            get { return (long)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public long KnobMin
        {
            get { return (long)GetValue(MinProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public long KnobMax
        {
            get { return (long)GetValue(MaxProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public long KnobStep
        {
            get { return (long)GetValue(StepProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValuePropertyChanged(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as Knob;
            control.OnPropertyChanged("KnobValue");
            control.OnWheelChanged(e);
        }

        private static void OnMinPropertyChanged(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as Knob;
            control.OnPropertyChanged("KnobMin");
            control.OnWheelChanged(e);
        }

        private static void OnMaxPropertyChanged(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as Knob;
            control.OnPropertyChanged("KnobMax");
            control.OnWheelChanged(e);
        }

        private static void OnStepPropertyChanged(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as Knob;
            control.OnPropertyChanged("KnobStep");
            control.OnWheelChanged(e);
        }

        private void OnWheelChanged(DependencyPropertyChangedEventArgs e)
        {
            TransformWheel();
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public Knob()
        {
            InitializeComponent();

            _timer = new Timer { Enabled = false, Interval = 200 };
            _timer.Elapsed += OnTick;
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(
                new Action(() =>
                {
                    KnobValue += _stepInUse;
                    TransformWheel();
                }
                    ));
        }

        private void TransformWheel()
        {
            var domain = KnobMax - KnobMin;
            var degreePerUnit = domain/360.0;
            var angle = degreePerUnit * KnobValue;

            var halfWidth = ActualWidth/2;
            var halfHeight = ActualHeight/2;

            Wheel.RenderTransform =
                new RotateTransform {Angle = angle, CenterX = halfWidth, CenterY = halfHeight};
        }

        private void LeftPushed(object sender, MouseButtonEventArgs e)
        {
            _stepInUse = -KnobStep;
            _timer.Enabled = true;

        }

        private void RightPushed(object sender, MouseButtonEventArgs e)
        {
            _stepInUse = KnobStep;
            _timer.Enabled = true;

        }

        private void LeftDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
                return;

            _stepInUse = -KnobStep;
            _timer.Enabled = true;
        }

        private void RightDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
                return;

            _stepInUse = KnobStep;
            _timer.Enabled = true;
        }

        private void Up(object sender, EventArgs e)
        {
            _timer.Enabled = false;
        }

        private void Up(object sender, KeyEventArgs e)
        {

        }
    }
}
