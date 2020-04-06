using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Enums
{
    public enum UserMode
    {
        NotDefined = -1,
        Memory = 0, 
        Search = 1, 
        Scan = 2, 
        Hopping = 3, 
        Skip = 4,
        SearchHistory = 5,
        ScanHistory = 6,
    }
}
