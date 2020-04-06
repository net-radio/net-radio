using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EighthBit.Exciter.Can;
using System.Runtime.InteropServices;
using System.Threading;
using AxCanToUSBConverter;
using System.Windows.Forms;
using System.Diagnostics;

namespace EighthBit.Can.Nmg
{
    public class NmgCanControl:ICanControl
    {
        private  AxCanToUsb _canToUsb;
        private readonly Thread _thread;
        private readonly ManualResetEvent _lock = new ManualResetEvent(false);

        public event EventHandler<CanFrame> Received;

        public NmgCanControl()
        {
            _thread = new Thread(InitializeActiveX);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Start();
            _lock.WaitOne();
        }

        void _canToUsb_Interrupt(object sender, EventArgs e)
        {
            //Debug.Print("int");

            //var count = _canToUsb.USB_GetNumOfPacket();
            //for (int i = 0; i < count; i++)
            //{
            //    Debug.Print("loop");
            //    _canToUsb.USB_Read();
            //    Debug.WriteLine("Id:{0}, L: {1}, D0:{2},D1:{3},D2:{4},D3:{5},D4:{6},D5:{7},D6:{8},D7:{9}"
            //        , _canToUsb.ID
            //        , _canToUsb.DataLen
            //        , _canToUsb.Data0
            //        , _canToUsb.Data1
            //        , _canToUsb.Data2
            //        , _canToUsb.Data3
            //        , _canToUsb.Data4
            //        , _canToUsb.Data5
            //        , _canToUsb.Data6
            //        , _canToUsb.Data7
            //        );

            //    var frame = new CanFrameWriter().Id(Convert.ToUInt32(_canToUsb.ID)).Rtr(Convert.ToBoolean(_canToUsb.RTR))
            //        .Write(Convert.ToByte(_canToUsb.Data0))
            //        .Write(Convert.ToByte(_canToUsb.Data1))
            //        .Write(Convert.ToByte(_canToUsb.Data2))
            //        .Write(Convert.ToByte(_canToUsb.Data3))
            //        .Write(Convert.ToByte(_canToUsb.Data4))
            //        .Write(Convert.ToByte(_canToUsb.Data5))
            //        .Write(Convert.ToByte(_canToUsb.Data6))
            //        .Write(Convert.ToByte(_canToUsb.Data7))
            //        .TrimEnd(Convert.ToByte(_canToUsb.DataLen))
            //        .ToCanFrame();
            //    Debug.WriteLine(frame);
            //}

            var count = _canToUsb.USB_GetNumOfPacket();
            for (int i = 0; i < count; i++)
            {
                _canToUsb.USB_Read();

                var frame = new CanFrameWriter().Id(Convert.ToUInt32(_canToUsb.ID)).Rtr(Convert.ToBoolean(_canToUsb.RTR))
                    .Write(Convert.ToByte(_canToUsb.Data0))
                    .Write(Convert.ToByte(_canToUsb.Data1))
                    .Write(Convert.ToByte(_canToUsb.Data2))
                    .Write(Convert.ToByte(_canToUsb.Data3))
                    .Write(Convert.ToByte(_canToUsb.Data4))
                    .Write(Convert.ToByte(_canToUsb.Data5))
                    .Write(Convert.ToByte(_canToUsb.Data6))
                    .Write(Convert.ToByte(_canToUsb.Data7))
                    .TrimEnd(Convert.ToByte(_canToUsb.DataLen))
                    .ToCanFrame();

                OnRecieved(frame);
            }
        }

        [STAThread]
        private void InitializeActiveX()
        {
            _canToUsb = new AxCanToUsb();
            _canToUsb.BeginInit();
            _canToUsb.CreateControl();
            _canToUsb.Interrupt += new EventHandler(_canToUsb_Interrupt);
            _canToUsb.EndInit();
            _lock.Set();
            Application.Run();
        }   

        public void Send(CanFrame frame)
        {
            _canToUsb.Invoke(new Action(() =>
            {
                _canToUsb.ID = (int)frame.Id;
                _canToUsb.RTR = frame.Rtr ? (byte)1 : (byte)0;
                _canToUsb.DataLen = (byte)frame.Length;

                _canToUsb.Data0 = frame.TryGet(0);
                _canToUsb.Data1 = frame.TryGet(1);
                _canToUsb.Data2 = frame.TryGet(2);
                _canToUsb.Data3 = frame.TryGet(3);
                _canToUsb.Data4 = frame.TryGet(4);
                _canToUsb.Data5 = frame.TryGet(5);
                _canToUsb.Data6 = frame.TryGet(6);
                _canToUsb.Data7 = frame.TryGet(7);

                var res = _canToUsb.USB_Write();
            }));
        }

        protected void OnRecieved(CanFrame frame)
        {
            if (Received == null)
                return;

            Received(this, frame);
        }

        public void Dispose()
        {
            _canToUsb.Invoke(new Action(() =>
            {
                _canToUsb.USB_ClosePort();
            }));

            _thread.Abort();
        }

        public void Initialize(Exciter.ExciterCanBaudRate baudRate)
        {
            _canToUsb.Invoke(new Action(() =>
            {
                _canToUsb.USB_OpenPort();
                byte baud = (byte)baudRate;
                byte id = 0;
                byte mask = 255;
                byte code = 0;

                _canToUsb.USB_SetSetting(ref baud, ref id, ref mask, ref code);
            }));
        }
    }
}
