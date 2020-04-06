using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.ModelBase.Pax.Definitions
{
    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);
    public delegate void FrequencyChangedEventHandler(object sender, EventArgs e);
}
