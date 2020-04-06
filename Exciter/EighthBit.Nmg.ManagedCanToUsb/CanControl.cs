using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EighthBit.Nmg.ManagedCanToUsb
{
    public class CanControl:IDisposable
    {
        private FTD2XX_NET.FTDI _device = new FTD2XX_NET.FTDI();
        private Thread _thread;
        private bool _looping = true;

        public event EventHandler<FrameArg> Received;

        protected void OnReceived(Frame frame)
        {
            if (Received == null) return;
            Received(this, new FrameArg(frame));
        }

        public void Initialize()
        {
            uint count=0;
            var status = _device.GetNumberOfDevices(ref count);

            if (status != FTD2XX_NET.FTDI.FT_STATUS.FT_OK && count == 0)
                throw new Exception("Device not found");

            var description = new FTD2XX_NET.FTDI.FT_DEVICE_INFO_NODE[count];
            status = _device.GetDeviceList(description);

            if (status != FTD2XX_NET.FTDI.FT_STATUS.FT_OK)
                throw new Exception("Failed to list Devices");

            status=_device.OpenByDescription(description[0].Description);
            if (status != FTD2XX_NET.FTDI.FT_STATUS.FT_OK)
                throw new Exception("Failed to open Device");

            _thread = new Thread(Loop);
            _thread.Name = "CAN Driver Loop";
            _thread.Start();

        }

        private void Loop()
        {
            while (_looping)
            {
                var header0 = ReadBlocking();
                var header1 = ReadBlocking();

                if (!header0.HasValue || !header1.HasValue)
                    continue;

                var frame = new Frame(header0 ?? 0, header1 ?? 0);

                var length = frame.Length;
                var bytes = new byte[length];
                for (var i = 0; i < length; i++)
                {
                    var data = ReadBlocking();
                    bytes[i] = data ?? 0;
                }
                frame.Data = bytes;

                OnReceived(frame);
            }
        }

        private byte? ReadBlocking()
        {
            uint rxWaiting = 0;
            while (_looping)
            {
                rxWaiting = 0;
                var status = _device.GetRxBytesAvailable(ref rxWaiting);

                if (rxWaiting == 0 || status != FTD2XX_NET.FTDI.FT_STATUS.FT_OK)
                {
                    Thread.Sleep(10);
                    continue;
                }

                break;
            }

            if (!_looping)
                return null;

            uint readBytes=0;
            var bytes = new byte[1]; //this is based on reversed-code of CanToUsb.ocx, I may implement  more efficient method later
            var readStatus = _device.Read(bytes, 1, ref readBytes);

            if (readBytes == 0 || readStatus != FTD2XX_NET.FTDI.FT_STATUS.FT_OK)
                throw new InvalidOperationException("failed to read data");

            return bytes[0];
        }

        public void Send(ushort id, bool rtr, IEnumerable<byte> data)
        {
            Send(new Frame { Id = id, Rtr = rtr, Data = data.ToArray() });
        }

        public void Send(Frame frame)
        {
            var buffer=frame.ToCommandArray();
            uint writeCount=0;
            var status = _device.Write(buffer, buffer.Length, ref writeCount);

            if(writeCount!=buffer.Length || status!=FTD2XX_NET.FTDI.FT_STATUS.FT_OK)
                throw new InvalidOperationException("Sending CAN frame failed.");
        }

        public void UpdateCanSettings(CanBaudRate baudRate, byte mask, byte code)
        {
            var bytes = new byte[5];
            bytes[0] = 0x43;
            bytes[1] = (byte)baudRate;
            bytes[2] = 0; //extended is not supported
            bytes[3] = mask;
            bytes[4] = code;

            uint writtenCount=0;
            var status = _device.Write(bytes, bytes.Length, ref writtenCount);
            if (writtenCount != bytes.Length || status != FTD2XX_NET.FTDI.FT_STATUS.FT_OK)
                throw new InvalidOperationException("Failed to send Settings.");
        }

        public void Dispose()
        {
            _looping = false;
        }
    }
}
