using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlLibrary
{
    /// <summary>
    /// Then in the constructor of each custom control, merge the shared resource with the resources of the custom control before you call InitilizeComponent.  Because the property is static, the ResourceDictionary gets created only once.
    /// </summary>
    /// <see cref="https://blogs.msdn.microsoft.com/wpfsdk/2007/06/08/defining-and-using-shared-resources-in-a-custom-control-library/"/>
    class SharedDictionaryManager
    {
        private static ResourceDictionary _sharedDictionary;

        internal static ResourceDictionary SharedDictionary
        {
            get
            {
                if (_sharedDictionary == null)
                {
                    System.Uri resourceLocater = new System.Uri("/WpfControlLibrary;component/Resource.xaml", System.UriKind.Relative);
                    _sharedDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
                }
                return _sharedDictionary;
            }
        }
    }
}
