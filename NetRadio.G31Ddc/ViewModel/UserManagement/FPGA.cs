using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace NetRadio.G31Ddc.ViewModel.UserManagement
{
    public class FPGA : IDisposable
    {
        public static byte cFPGA_ID_Addr = 0;
        public static byte cStream_Ctrl_Addr = 2;
        public static byte cAdc3_Mean_Rpt_Addr = 3;
        public static byte cAdc4_Mean_Rpt_Addr = 4;
        public static byte cRec_In_Sel_Addr = 8;
        public static byte cRec_FC_Addr = 9;
        public static byte cRec_Ctrl_Addr = 10;
        public static byte cRec_GainEn_Addr = 11;
        public static byte cRec_Threshold_Addr = 12;
        public static byte cAbsMin_Report_Addr = 13;
        public static byte cAbsMax_Report_Addr = 14;
        public static byte cRec_TestDDS_FC_Addr = 24;
        public static byte cRec_TestDDS_Amp_Addr = 25;


        public static byte cFFT_Thr_Addr = 26;
        public static byte cFFT_StartP_Addr = 27;
        public static byte cFFT_StopP_Addr = 28;
        public static byte cFFT_ChRes_Addr = 29;
        public static byte cFFT_FFT_Indx_Addr = 30;
        public static byte cFFT_FFT_Peak_Addr = 31;
        public static byte cFFT_Gain_Addr = 23;

        static FPGA()
        {
            port = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);
            try
            {
                port.Open();
            }
            catch (IOException e)
            {
                port.Close();
            }
        }

        public static void WriteRegister32(byte addr, uint regValue)
        {
            // BOOL     bWriteRC;
            ulong iBytesWritten;

            byte[] BUff = new byte[8];

            BUff[0] = 0x5A; //Header
            BUff[1] = 0x80;
            BUff[2] = addr;
            BUff[3] = (byte)((regValue >> 24) & 0xFF);
            BUff[4] = (byte)((regValue >> 16) & 0xFF);
            BUff[5] = (byte)((regValue >> 8) & 0xFF);
            BUff[6] = (byte)(regValue & 0xFF);
            BUff[7] = 0xD8;


            if (port.IsOpen)
                port.Write(BUff, 0, 8);
        }

        private static SerialPort port;

        public void Dispose()
        {
            if (port!= null && port.IsOpen)
                port.Close();
        }
    }
}
