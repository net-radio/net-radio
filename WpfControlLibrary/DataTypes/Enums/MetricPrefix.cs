using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlLibrary.DataTypes.Enums
{
    public enum MetricPrefix : sbyte
    {
        Y = 24,  // Yotta
        Z = 21,  // Zetta
        E = 18,  // Exa
        P = 15,  // Peta
        T = 12,  // Tera
        G = 9,   // Giga	
        M = 6,   // Mega
        k = 3,   // Kilo
        h = 2,   // Hecto
        da = 1,  // Deca
        None = 0,
        d = -1,  // Deci
        c = -2,  // Centi
        m = -3,  // Milli
        μ = -6,  // Micro
        n = -9,  // Nano
        p = -12, // Pico
        f = -15, // Femto
        a = -18, // Atto
        z = -21, // Zepto
        y = -24, // Yocto
    }
}
