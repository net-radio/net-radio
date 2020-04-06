using EighthBit.Exciter.Can;
using EighthBit.Exciter.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace EighthBit.Exciter
{
    public abstract class ExciterApiBase : IExciterApi, IDispatcher, IDisposable
    {
        protected static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ExciterApiBase(ICanControl canControl)
        {
            _canControl = canControl;
            _canControl.Received += FrameReceived;
        }

        public const uint ID_POWER = 10;
        public const uint ID_SOURCE = 11;
        public const uint ID_MODULATION = 12;
        public const uint ID_MMC = 30;
        public const uint ID_START_DATA_TRANSFER = 48;
        public const uint ID_COMMAND = 49;


        protected readonly ICanControl _canControl;

        /*
        protected readonly Info _info;
        protected readonly Mmc _mmc;
        protected readonly Modulator _modulator;
        protected readonly Power _power;
        protected readonly PowerSupply _powerSupply;
        protected readonly Rfcu _rfcu1;
        protected readonly Rfcu _rfcu2;
        protected readonly ExciterDateTime _dateTime;
        protected readonly CombParser _combParser;
        */

        protected Info _info;
        protected Mmc _mmc;
        protected Modulator _modulator;
        protected Power _power;
        protected PowerSupply _powerSupply;
        protected Rfcu _rfcu1;
        protected Rfcu _rfcu2;
        protected ExciterDateTime _dateTime;
        protected CombParser _combParser;

        private readonly Dictionary<uint, IParser> _parsers = new Dictionary<uint, IParser>();

        public event EventHandler<EventArgs> Updated;

        public bool PadZero { get; set; }


        void IDispatcher.Register(uint id, IParser parser)
        {
            _parsers.Add(id, parser);
        }

        public void Dispose()
        {
            _canControl.Dispose();
        }

        private void FrameReceived(object sender, CanFrame frame)
        {
            Debug.WriteLine(frame);

            //if(_parsers.ContainsKey(frame.Id))
            try
            {
                _parsers[frame.Id].Update(frame);
            }
            catch (KeyNotFoundException ex)
            {
                logger.Info("Exciter frame received failed: ", ex);
            }
            catch (Exception ex)
            {
                logger.Info("Exciter frame received failed: ", ex);
            }

            if (Updated == null)
                return;

            Updated(this, frame);
        }

        public Mmc MmcInfo()
        {
            return _mmc;
        }

        public Modulator ModulatorInfo()
        {
            return _modulator;
        }

        public Power PowerInfo()
        {
            return _power;
        }

        public PowerSupply PowerSupplyInfo()
        {
            return _powerSupply;
        }

        public Rfcu Rfcu1()
        {
            return _rfcu1;
        }

        public Rfcu Rfcu2()
        {
            return _rfcu2;
        }

        public Info ExciterInfo()
        {
            return _info;
        }
    }
}
