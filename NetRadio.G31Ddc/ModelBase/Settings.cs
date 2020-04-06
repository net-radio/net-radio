using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetRadio.G31Ddc.ModelBase
{
    public abstract class Settings
    {
        public static string GetPath()
        {
            var path = Path.Combine(DataPath(), "settings.json");
            return path;
        }

        public static string DataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NetRadio");
        }
    }
}
