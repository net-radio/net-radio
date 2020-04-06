using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Definitions
{
    [Flags]
    enum CombFrequencyCollection
    {
        None = 0,
        First = 1,
        Second = 1 << 1,
        Third = 1 << 2,
        Fourth = 1 << 3,
        Fifth = 1 << 4,
        Sixth = 1 << 5,
        Seventh = 1 << 6,
        Eighth = 1 << 7,
        Ninth = 1 << 8,
        Tenth = 1 << 9,
        Eleventh = 1 << 10,
        Twelfth = 1 << 11,
        Thirteenth = 1 << 12,
        Fourteenth = 1 << 13,
        Fifteenth = 1 << 14,
        Sixteenth = 1 << 15
    }
}
