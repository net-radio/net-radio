using System.Windows;
using NetRadio.G313.Model;
using NetRadio.G313.PanelViewModel;

namespace NetRadio.G313
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        internal AboutWindow(RadioModel model)
            : this()
        {
            var vm = new AboutViewModel(model) {WindowCloseAction = Close};
            DataContext = vm;
        }
    }
}
