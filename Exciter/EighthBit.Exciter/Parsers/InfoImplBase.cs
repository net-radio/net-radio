using EighthBit.Exciter.Can;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EighthBit.Exciter.Parsers
{
    /// <summary>
    /// This class reperesent all 
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public abstract class InfoImplBase : Info, IParser
    {
        public const uint ID_INFO_TEST = 66;
        public const uint ID_INFO_WARNING = 72;
        public const uint ID_INFO_GENERAL_STATUS = 87;

        [FieldOffset(0)]
        protected byte _powerLsb;
        [FieldOffset(1)]
        protected byte _powerMsb;
        [FieldOffset(0)]
        protected ushort _power;

        [FieldOffset(2)]
        protected byte _vswrLsb;
        [FieldOffset(3)]
        protected byte _vswrMsb;
        [FieldOffset(2)]
        protected ushort _vswr;

        [FieldOffset(4)]
        protected byte _remoteSelfTest1;

        [FieldOffset(5)]
        protected byte _remoteSelfTest2;

        [FieldOffset(6)]
        protected byte _tempWarning;

        [FieldOffset(7)]
        protected byte _ovRefWarning;

        [FieldOffset(8)]
        protected byte _vswrWarning;

        [FieldOffset(9)]
        protected byte _overCurrentWarning;

        [FieldOffset(10)]
        protected byte _fuseWarning;

        [FieldOffset(11)]
        protected byte _globalWarning;

        [FieldOffset(12)]
        protected byte _status;

        public byte RemoteSelfTest1
        {
            get { return _remoteSelfTest1; }
        }

        public byte RemoteSelfTest2
        {
            get { return _remoteSelfTest2; }
        }

        public byte TempWarning
        {
            get { return _tempWarning; }
        }

        public byte OverReflectWarning
        {
            get { return _ovRefWarning; }
        }

        public byte VswrWarning
        {
            get { return _vswrWarning; }
        }

        public byte OverCurrentWarning
        {
            get { return _overCurrentWarning; }
        }

        public byte FuseWarning
        {
            get { return _fuseWarning; }
        }

        public byte GlobalWarning
        {
            get { return _globalWarning; }
        }

        public bool Initialize //8-1
        {
            get { return (RemoteSelfTest1 & 0x80) > 0; }
        }

        public bool Supply
        {
            get { return (RemoteSelfTest1 & 0x40) > 0; }
        }

        public bool PreDrive1
        {
            get { return (RemoteSelfTest1 & 0x20) > 0; }
        }

        public bool PreDrive2
        {
            get { return (RemoteSelfTest2 & 0x20) > 0; }
        }

        public bool Module1
        {
            get { return (RemoteSelfTest1 & 0x10) > 0; }
        }

        public bool Module2
        {
            get { return (RemoteSelfTest1 & 0x08) > 0; }
        }

        public bool Module3
        {
            get { return (RemoteSelfTest1 & 0x04) > 0; }
        }

        public bool Module4
        {
            get { return (RemoteSelfTest1 & 0x02) > 0; }
        }

        public bool Module5
        {
            get { return (RemoteSelfTest2 & 0x10) > 0; }
        }

        public bool Module6
        {
            get { return (RemoteSelfTest2 & 0x8) > 0; }
        }

        public bool Module7
        {
            get { return (RemoteSelfTest2 & 0x04) > 0; }
        }

        public bool Module8
        {
            get { return (RemoteSelfTest2 & 0x02) > 0; }
        }

        public bool OutputProbe
        {
            get { return (RemoteSelfTest2 & 0x01) > 0; }
        }

        public void Update(CanFrame frame)
        {
            switch (frame.Id)
            {
                case ID_INFO_TEST:
                    InfoSelfTest(frame);
                    break;
                case ID_INFO_WARNING:
                    InfoWarning(frame);
                    break;
                case ID_INFO_GENERAL_STATUS:
                    InfoGeneralStatus(frame);
                    break;
            }
        }

        /// <summary>
        /// Receive general status information
        /// </summary>
        /// <param name="frame"></param>
        protected abstract void InfoGeneralStatus(CanFrame frame);

        /// <summary>
        /// Receive warning information
        /// </summary>
        /// <param name="frame"></param>
        protected abstract void InfoWarning(CanFrame frame);

        /// <summary>
        /// Receive self test result
        /// </summary>
        /// <param name="frame"></param>
        protected abstract void InfoSelfTest(CanFrame frame);

        public ushort Power
        {
            get { return _power; }
        }

        public ushort Vswr
        {
            get { return (ushort)(_vswr / 10); }
        }

        public bool PowerOn
        {
            get { return (_status & 1) > 0; }
        }

        public bool Error
        {
            get { return (_status & 2) > 0; }
        }

        public bool Warning
        {
            get { return (_status & 4) > 0; }
        }
    }
}
