using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.G31Ddc.Audio.DataTypes
{
    internal abstract class LoggerBase
    {
        protected static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
