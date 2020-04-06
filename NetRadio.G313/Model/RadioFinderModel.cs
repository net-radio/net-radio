using System.Linq;
using NetRadio.Devices;
using NetRadio.Devices.G313;

namespace NetRadio.G313.Model
{
    class RadioFinderModel
    {
        public RadioInfo[] AvailableRadios { get; private set; }

        public RadioFinderModel()
        {
            AvailableRadios=Radio<G313Radio>.Find<G313RadioInfoProvider>().List().ToArray();
        }

        public G313Radio Radio(int index)
        {
            return AvailableRadios[index].Open<G313Radio>();
        }
    }
}
