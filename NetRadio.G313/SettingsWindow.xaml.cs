using System.Windows;
using NetRadio.G313.Model;
using NetRadio.G313.PanelViewModel;

namespace NetRadio.G313
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        internal SettingsWindow(RadioModel model)
            :this()
        {
            var vm=new SettingsViewModel(model) {WindowCloseAction = Close};
            DataContext = vm;
        }
    }
}
