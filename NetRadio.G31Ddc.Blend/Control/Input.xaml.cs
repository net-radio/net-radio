using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace NetRadio.G31Ddc.Blend.Control
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : INotifyPropertyChanged
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (Input),
                new PropertyMetadata(default(string), OnInputPropertyChanged));


        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                TextBox.Text = Text;
            }
        }

        private static void OnInputPropertyChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as Input;
            control.OnPropertyChanged("Text");
            control.OnTextPropertyChanged(e);
        }

        private void OnTextPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            TextBox.Text = Text;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Input()
        {
            InitializeComponent();
        }

        private void Enter()
        {
            Text = TextBox.Text;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Text = Text;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            Enter();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Enter();
        }
    }
}
