using System;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G313
{
    internal class NativeDefinitions
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class SoftAgcData
        {
            public int bLength;          // Length of structure in bytes
            public int iRefLevel;        // AGC reference level in dB
            public UInt32 dwAttackTime;   // AGC attack time in ms
            public UInt32 dwDecayTime;    // AGC decay time in ms

            public SoftAgcData()
            {
                bLength = Marshal.SizeOf(this);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class RadioInfo2
        {
            public UInt32 bLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string szSerNum;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string szProdName;
            public UInt64 minFreq;
            public UInt64 maxFreq;
            public Features features;

            public RadioInfo2()
            {
                bLength = (uint)Marshal.SizeOf(this);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class Features
        {
            // ExtRef : 1
            // Reserved : 31
            public uint bits;

            public uint ExtRef
            {
                get
                {
                    return bits & 0x1U;
                }
                set
                {
                    bits = value | bits;
                }
            }

            public uint Reserved
            {
                get
                {
                    var temp = bits & 0xFFFFFFFEU;
                    temp >>= 1;
                    return temp;
                }
                set
                {
                    var temp = value << 1;
                    bits &= temp;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class OldRadioInfo
        {
            public int bLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string szSerNum;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string szProdName;
            public UInt32 dwMinFreq;
            public UInt32 dwMaxFreq;
            public Byte bNumBands;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] dwBandFreq;
            public UInt32 dwLOfreq;
            public Byte bNumVcos;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] dwVcoFreq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt16[] wVcoDiv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Byte[] bVcoBits;
            public UInt32 dwRefClk1;
            public UInt32 dwRefClk2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Byte[] IF1DAC;

            public OldRadioInfo()
            {
                bLength = Marshal.SizeOf(this);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class RadioInfo
        {
            public int bLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string szSerNum;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string szProdName;
            public UInt32 dwMinFreq;
            public UInt32 dwMaxFreq;
            public Byte bNumBands;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] dwBandFreq;
            public UInt32 dwLOfreq;
            public Byte bNumVcos;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] dwVcoFreq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt16[] wVcoDiv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Byte[] bVcoBits;
            public UInt32 dwRefClk1;
            public UInt32 dwRefClk2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Byte[] IF1DAC;
            public int iAGCstart;
            public int iAGCmid;
            public int iAGCend;
            public int iDropLevel;
            public int iRSSItop;
            public int iRSSIref;

            public RadioInfo()
            {
                bLength = Marshal.SizeOf(this);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void CallbackFunc(IntPtr handle);

        /// <summary>
        /// For demodulators that provide more channels (like ISB), both channels are provided in the audio stream with alternating samples.
        /// </summary>
        /// <remarks>
        /// The buffers sent to the application defined callback functions will always have the same size and will always contain the same number of samples of samples sets. In no case they will contain fractions of samples or samples sets.
        /// </remarks>
        /// <param name="target"> application specified value when the callback functions are registered; </param>
        /// <param name="buffer"> buffer with the 16-bit signed integer samples read from the DSP memory; </param>
        /// <param name="bufferSize"> size of the buffer holding the samples in bytes; </param>
        /// <param name="samplingRate"> samples sampling rate.</param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void StreamCallback(IntPtr target, IntPtr buffer, UInt32 bufferSize, UInt32 samplingRate);

    }
}
