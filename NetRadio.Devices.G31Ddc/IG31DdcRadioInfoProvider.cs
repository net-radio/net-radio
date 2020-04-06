using System.Collections.Generic;

namespace NetRadio.Devices.G3XDdc
{
    interface IG31DdcRadioInfoProvider<T>:IRadioInfoProvider<T> where T:Radio<T>
    {
        ICollection<G31DdcRadioInfo> List();
    }
}
