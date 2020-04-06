using NetRadio.G31Ddc.PanelViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetRadio.G31Ddc
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
            : this()
        {
            var vm = new SettingsViewModel(model) { WindowCloseAction = Close };
            DataContext = vm;
        }
    }
}
