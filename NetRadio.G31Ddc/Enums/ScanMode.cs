using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Enumerations
{
    /// <summary>
    /// Determines the type of a scan mode.
    /// </summary>
    public enum ScanMode
    {
        /// <summary>
        /// Indicates a default value for delay time 
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Indicates a user value for delay time 
        /// </summary>
        Delay = 1,
        /// <summary>
        /// Indicates an ultimate value for delay time 
        /// </summary>
        Lock = 2
    }
}
