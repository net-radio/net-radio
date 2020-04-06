using EighthBit.Exciter.Can;
using EighthBit.Nmg.ManagedCanToUsb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighthBit.Nmg.ManagedDemoCan
{
    public class NmgManagedDemoCanControl : ICanControl
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public event EventHandler<CanFrame> Received;

        public void Initialize(Exciter.ExciterCanBaudRate baudRate)
        {
            logger.Info("Initializing CanControl ...");
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
        }

        public void Dispose()
        {
        }
    }
}
