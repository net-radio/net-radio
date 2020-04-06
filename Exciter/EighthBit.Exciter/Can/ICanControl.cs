using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Can
{
    public interface ICanControl:IDisposable
    {
        void Initialize(ExciterCanBaudRate baudRate);
        void Send(CanFrame frame);
        event EventHandler<CanFrame> Received;
    }
}
