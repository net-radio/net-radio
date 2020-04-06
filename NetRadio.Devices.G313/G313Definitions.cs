using System;

namespace NetRadio.Devices.G313
{
    internal class G313Definitions
    {
        public const string DLL = "WRG3130API.dll";

        [Flags]
        internal enum G313DspMask : uint
        {
            WR_G313_DSP_1_1 = 0x00000001U,
            WR_G313_DSP_1_2 = 0x00000002U
        }

        internal enum G313Mode
        {
            CW = 0,
            AM = 1,
            FMN = 2,
            LSB = 4,
            USB = 5,
            AMS=8,
            DSB=13,
            ISB=14
        }
    }
}
