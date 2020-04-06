using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlLibrary
{
    public class UserControlBase : UserControl, INotifyPropertyChanged, IDisposable
    {
        protected static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject;

            logger.Debug(string.Format("Dependency Object: {0}\tDependency Property : {1} : {2} -> {3}",
                dependencyObject.ToString(), e.Property.Name, e.OldValue, e.NewValue));

            Type type = dependencyObject.GetType();
            PropertyInfo property = type.GetProperty(e.Property.Name);
            property.SetValue(dependencyObject, e.NewValue);
        }

        public void Dispose()
        {
        }
    }
}
