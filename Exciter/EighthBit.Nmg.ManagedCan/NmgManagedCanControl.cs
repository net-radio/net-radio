using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EighthBit.Exciter.Can;
using EighthBit.Nmg.ManagedCanToUsb;
using System.Threading;

namespace EighthBit.Nmg.ManagedCan
{
    public class NmgManagedCanControl : ICanControl
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CanControl _device = new CanControl();

        public event EventHandler<CanFrame> Received;

        public void Initialize(Exciter.ExciterCanBaudRate baudRate)
        {
            logger.Info("Initializing CanControl ...");
            _device.Initialize();
            _device.UpdateCanSettings((CanBaudRate)baudRate, 255, 0);
            _device.Received += new EventHandler<FrameArg>(OnReceived);
        }

        private void OnReceived(object sender, FrameArg e)
        {
            if (Received == null)
                return;

            var frame = new CanFrameWriter().Id(e.Frame.Id).Rtr(e.Frame.Rtr).Write(e.Frame.Data).ToCanFrame();
            Debug.WriteLine("Received: {0}",frame);
            logger.Info(string.Format("Received: {0}", frame));
            Received(this, frame);
        }

        public void Send(CanFrame frame)
        {
            Debug.WriteLine("Sent: {0}", frame);
            logger.Info(string.Format("Sent: {0}", frame));
            _device.Send((ushort)frame.Id, frame.Rtr, frame.Data());
            Thread.Sleep(50);
        }

        public void Dispose()
        {
            _device.Dispose();
        }
    }
}
