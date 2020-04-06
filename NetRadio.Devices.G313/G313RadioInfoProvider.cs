using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G313
{
    public class G313RadioInfoProvider : IG313RadioInfoProvider<G313Radio>
    {
        public ICollection<BasicRadioInfo> ListAll()
        {
            var info = List();
            return info.Cast<BasicRadioInfo>().ToList();
        } 

        public ICollection<RadioInfo> List()
        {
            var info = new NativeDefinitions.RadioInfo2();
            var ptr = Marshal.AllocHGlobal((int)info.bLength);

            var structSize = 0;
            var count = G313Api.GetRadioList(ptr, (int)info.bLength, ref structSize);

            if (info.bLength != structSize)
                throw new InvalidOperationException("info size mismatch");

            Marshal.PtrToStructure(ptr, info);
            Marshal.FreeHGlobal(ptr);

            return count > 1 ? ListBig(count, structSize) : new[] { new RadioInfo(info) };
        }

        [Obsolete("use List instead")]
        public ICollection<LegacyRadioInfo> ListLegacy()
        {
            var info = new NativeDefinitions.OldRadioInfo();
            var ptr = Marshal.AllocHGlobal(info.bLength);

            var count = G313Api.G3GetRadioList(ptr, info.bLength);

            Marshal.PtrToStructure(ptr, info);
            Marshal.FreeHGlobal(ptr);

            return count > 1 ? ListLegacyBig((int)count, info.bLength) : new[] { new LegacyRadioInfo(info) };
        }

        [Obsolete("use List instead")]
        public ICollection<CompleteRadioInfo> ListComplete()
        {
            var info = new NativeDefinitions.RadioInfo();
            var ptr = Marshal.AllocHGlobal(info.bLength);

            var structSize = 0;
            var count = G313Api.G3GetRadioList2(ptr, info.bLength, ref structSize);

            if (info.bLength != structSize)
                throw new InvalidOperationException("info size mismatch");

            Marshal.PtrToStructure(ptr, info);
            Marshal.FreeHGlobal(ptr);

            return count > 1 ? ListCompleteBig((int)count, structSize) : new[] { new CompleteRadioInfo(info) };
        }

        [Obsolete("use List instead")]
        private static ICollection<LegacyRadioInfo> ListLegacyBig(int deviceCount, int size)
        {
            var bufferSize = deviceCount * size;
            var ptr = Marshal.AllocHGlobal(bufferSize);

            var count = G313Api.G3GetRadioList(ptr, bufferSize);

            var buffer = ToManagedStructure<NativeDefinitions.OldRadioInfo>(ptr, size, (int)count);
            Marshal.FreeHGlobal(ptr);

            if (count != deviceCount)
                throw new InvalidOperationException("device count mismatch!");

            return buffer.Select(raw => new LegacyRadioInfo(raw)).ToArray();
        }

        [Obsolete("use List instead")]
        private static ICollection<CompleteRadioInfo> ListCompleteBig(int deviceCount, int size)
        {
            var bufferSize = deviceCount * size;
            var ptr = Marshal.AllocHGlobal(bufferSize);

            var structSize = 0;
            var count = G313Api.G3GetRadioList2(ptr, bufferSize, ref structSize);

            var buffer = ToManagedStructure<NativeDefinitions.RadioInfo>(ptr, structSize, (int)count);
            Marshal.FreeHGlobal(ptr);

            if (count != deviceCount)
                throw new InvalidOperationException("device count mismatch!");

            return buffer.Select(raw => new CompleteRadioInfo(raw)).ToArray();
        }

        private static ICollection<RadioInfo> ListBig(int deviceCount, int size)
        {
            var bufferSize = deviceCount * size;
            var ptr = Marshal.AllocHGlobal(bufferSize);

            var structSize = 0;

            var count = G313Api.GetRadioList(ptr, bufferSize, ref structSize);

            var buffer = ToManagedStructure<NativeDefinitions.RadioInfo2>(ptr, structSize, count);
            Marshal.FreeHGlobal(ptr);

            if (count != deviceCount)
                throw new InvalidOperationException("device count mismatch!");

            return buffer.Select(raw => new RadioInfo(raw)).ToArray();
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
