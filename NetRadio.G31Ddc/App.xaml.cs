using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NetRadio.G31Ddc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly log4net.ILog logger = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            IDisposable disposableViewModel = null;

            //Create and show window while storing datacontext
            this.Startup += (sender, args) =>
            {
                MainWindow = new G31DdcMainWnd();
                disposableViewModel = MainWindow.DataContext as IDisposable;

                MainWindow.Show();
            };

            //Dispose on unhandled exception
            this.DispatcherUnhandledException += (sender, args) =>
            {
                if (disposableViewModel != null) disposableViewModel.Dispose();
            };

            //Dispose on exit
            this.Exit += (sender, args) =>
            {
                if (disposableViewModel != null) disposableViewModel.Dispose();
            };
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            logger.Info(string.Format("Application started on {0}", DateTime.Now.ToString("F")));
            /*
            UserMemoryWnd wnd = new UserMemoryWnd();
            wnd.Show();
            */ 
        }
    }
}
