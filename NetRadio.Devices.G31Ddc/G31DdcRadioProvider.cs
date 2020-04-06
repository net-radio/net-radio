using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRadio.Devices.Exceptions;

namespace NetRadio.Devices.G3XDdc
{
    class G31DdcRadioProvider:IRadioProvider<G31DdcRadio>
    {
        readonly G31DdcApi _api=new G31DdcApi();

        public G31DdcRadio Open(int index)
        {
            G31DdcRadio radio;
            var res = TryOpen(index, out radio);
            if (!res)
                throw new InvalidOperationException("no device");

            return radio;
        }

        public G31DdcRadio Open(string serial)
        {
            G31DdcRadio radio;
            var res = TryOpen(serial, out radio);
            if (!res)
                throw new InvalidOperationException("no device");

            return radio;
        }

        public G31DdcRadio Open(BasicRadioInfo info)
        {
            G31DdcRadio radio;
            var res = TryOpen(info,out radio);
            if(!res)
                throw new InvalidOperationException("no device");

            return radio;
        }

        public bool TryOpen(int index, out G31DdcRadio radio)
        {
            radio = null;

            var infoProvider = new G31DdcRadioInfoProvider();
            var list = infoProvider.List().ToList();
            if (list.Count - 1 < index)
                return false;

            var info = list[index];
            return TryOpen(info, out radio);
        }

        public bool TryOpen(string serial, out G31DdcRadio radio)
        {
            radio = null;

            var handle = _api.OpenDevice(serial);
            if (handle < 0)
                return false;

            radio= new G31DdcRadio(new IntPtr(handle));
            return true;
        }

        public bool TryOpen(BasicRadioInfo info, out G31DdcRadio radio)
        {
            return TryOpen(info.Serial, out radio);
        }
    }
}
