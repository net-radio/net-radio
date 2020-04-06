using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRadio.G31Ddc.Model.Entities
{
    public interface IObservable
    {
        double SignalLevel { get; set; }
    }
}
