using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected static readonly log4net.ILog logger = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public log4net.ILog Logger
        {
            get { return logger; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            logger.Info(string.Format("Application started on {0}", DateTime.Now.ToString("F")));
            base.OnStartup(e);
        }
    }
}
