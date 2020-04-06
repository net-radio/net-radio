using NetRadio.G31Ddc.Audio.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NetRadio.G31Ddc.Audio.ViewModels
{
    class MainViewModel : ViewModelBase, IDisposable
    {
        public ICommand WindowClosing { get; set; }
        public RadioViewModel RadioView { get; private set; }

        public MainViewModel()
        {
            RadioView = new RadioViewModel();

            WindowClosing = new DelegateCommand(() => { if (MessageBox.Show("Are you sure?") == MessageBoxResult.OK) Dispose(); });
        }

        public void Dispose()
        {
            RadioView.Dispose();
        }
    }
}
