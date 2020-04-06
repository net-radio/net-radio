using NetRadio.G31Ddc.Audio.ViewModels;
using System.Windows;

namespace NetRadio.G31Ddc.Audio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainViewModel();

            DataContext = _vm;
        }
    }
}
