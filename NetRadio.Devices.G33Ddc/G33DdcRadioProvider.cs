using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRadio.Devices.Exceptions;

namespace NetRadio.Devices.G3XDdc
{
    class G33DdcRadioProvider:IRadioProvider<G33DdcRadio>
    {
        readonly G31DdcApi _api=new G31DdcApi();

        public G33DdcRadio Open(int index)
        {
            G33DdcRadio radio;
            var res = TryOpen(index, out radio);
            if (!res)
                throw new InvalidOperationException("no device");

            return radio;
        }

        public G33DdcRadio Open(string serial)
        {
            G33DdcRadio radio;
            var res = TryOpen(serial, out radio);
            if (!res)
                throw new InvalidOperationException("no device");

            return radio;
        }

        public G33DdcRadio Open(BasicRadioInfo info)
        {
            G33DdcRadio radio;
            var res = TryOpen(info,out radio);
            if(!res)
                throw new InvalidOperationException("no device");

            return radio;
        }

        public bool TryOpen(int index, out G33DdcRadio radio)
        {
            radio = null;

            var infoProvider = new G33DdcRadioInfoProvider();
            var list = infoProvider.List().ToList();
            if (list.Count - 1 < index)
                return false;

            var info = list[index];
            return TryOpen(info, out radio);
        }

        public bool TryOpen(string serial, out G33DdcRadio radio)
        {
            radio = null;

            var handle = _api.OpenDevice(serial);
            if (handle < 0)
                return false;

            radio= new G33DdcRadio(new IntPtr(handle));
            return true;
        }

        public bool TryOpen(BasicRadioInfo info, out G33DdcRadio radio)
        {
            return TryOpen(info.Serial, out radio);
        }
    }
}
