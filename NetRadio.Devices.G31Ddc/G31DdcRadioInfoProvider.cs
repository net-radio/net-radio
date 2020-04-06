using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NetRadio.Devices.Exceptions;

namespace NetRadio.Devices.G3XDdc
{
    class G31DdcRadioInfoProvider:IG31DdcRadioInfoProvider<G31DdcRadio>
    {
        readonly G31DdcApi _api=new G31DdcApi();

        public ICollection<BasicRadioInfo> ListAll()
        {
            return List().Cast<BasicRadioInfo>().ToList();
        }

        public ICollection<G31DdcRadioInfo> List()
        {
            var count = _api.GetDeviceList(IntPtr.Zero, 0);//device Count;

            if(count<0)
                throw new InvalidOperationException("failed to get device count");

            if (count == 0)
                return new G31DdcRadioInfo[] {};

            var size = Marshal.SizeOf(typeof (NativeDefinitions.G31DDC_DEVICE_INFO));

            return List(count, size);
        }

        private  ICollection<G31DdcRadioInfo> List(int deviceCount, int size)
        {
            var bufferSize = deviceCount * size;
            var ptr = Marshal.AllocHGlobal(bufferSize);

            var count = _api.GetDeviceList(ptr, (uint)bufferSize);
            if (count < 0)
                throw new InvalidOperationException("failed to get device list");

            var buffer = ToManagedStructure<NativeDefinitions.G31DDC_DEVICE_INFO>(ptr, size, count);
            Marshal.FreeHGlobal(ptr);

            if (count != deviceCount)
                throw new InvalidOperationException("device count mismatch!");

            return buffer.Select(raw => new G31DdcRadioInfo(raw)).ToArray();
        }

        private static ICollection<T> ToManagedStructure<T>(IntPtr ptr, int structSize, int count)
        {
            var list = new List<T>();

            for (var i = 0; i < count; i++)
            {
                var instance = Activator.CreateInstance<T>();
                Marshal.PtrToStructure(ptr, instance);
                list.Add(instance);

                ptr += structSize;
            }

            return list;
        }
    }
}
