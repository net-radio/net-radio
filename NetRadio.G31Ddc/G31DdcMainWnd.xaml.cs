using NetRadio.G31Ddc.PanelViewModel;
using NetRadio.G31Ddc.ViewModel.Exciter;
using NetRadio.G31Ddc.WaterfallSample;
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
    /// Interaction logic for G31DdcMainWnd.xaml
    /// </summary>
    public partial class G31DdcMainWnd : Window
    {
        public G31DdcMainWnd()
        {
            InitializeComponent();

            Waterfall.Begin(new WaterfallConfig
            {
                DownSample = 8,
                History = 50,
                MaxIntensity = 0,
                MinIntensity = -100,
                MinFrequency = 0,
                MaxFrequency = 50000000,
                Points = 2048,
                RefreshRate = 2
            });
        }

        private void MainWnd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RadioViewModel.GetInstance().Dispose();
        }
    }
}
