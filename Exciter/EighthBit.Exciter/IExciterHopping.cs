using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    public interface IExciterHopping : IExciter
    {
        uint FrequencyStart { get; set; }
        uint FrequencyStop { get; set; }
        float ThresholdMinimum { get; set; }
        float ThresholdMaximum { get; set; }
        ExciterModulation Modulation { get; set; }
        float Gain { get; set; }
    }
}
