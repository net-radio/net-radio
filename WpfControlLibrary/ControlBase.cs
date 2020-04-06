using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlLibrary
{
    public class ControlBase : UserControl, INotifyPropertyChanged, IDisposable
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            logger.Debug("OnPropertyChanged");
            logger.Debug(string.Format("\tDependency Object: {0}", dependencyObject.ToString()));
            logger.Debug(string.Format("\tDependency Property Changed: {0} : {1} -> {2}", e.Property.Name, e.OldValue, e.NewValue));

            // var control = dependencyObject as FrequencyDisplay;

            Type type = dependencyObject.GetType();
            PropertyInfo pi = type.GetProperty(e.Property.Name);
            pi.SetValue(dependencyObject, e.NewValue);
        }

        protected log4net.ILog Logger
        {
            get { return logger; }
        }

        public void Dispose()
        {
        }
    }
}
