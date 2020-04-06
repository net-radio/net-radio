using System;
using System.Threading;
using EighthBit.Exciter.Can;
using EighthBit.Exciter.Parsers;
using System.Linq;
using System.Threading.Tasks;
using EighthBit.Exciter.Configuration.Comb;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EighthBit.Exciter
{
    public class ExciterManager : ExciterSubSystem, IExciter, IExciterController, IDisposable
    {
        public ExciterManager(ICanControl control)
            : base(new ExciterApi2Kw(control))
        {
            controller_ = this;
            Spot = new ExciterSpot(this);
            Sweep = new ExciterSweep(this);
            Barrage = new ExciterBarrage(this);
            MultiSpot = new ExciterMultiSpot(this);
            Comb = new ExciterComb(this);
            Hopping = new ExciterHopping(this);

            InitializeStatusLoop();

            // ActivateCurrentMode();
            logger.Info("Exciter Initialize Default ...");
            InitializeDefault();
        }

        protected IExciterManagerApi ExciterManagerApi { get { return base.ExciterApi as IExciterManagerApi; } }


        private bool _working = true;
        private ExciterOperationMode operation_;
        private ExciterOutputLine _outputLine1;
        private ExciterOutputLine _outputLine2;
        private bool _hoppingSwitch;
        private bool _rfOut;

        protected IExciterSpotApi ExciterSpotApi { get { return base.ExciterApi as IExciterSpotApi; } }
        public IExciterApi ExciterApi { get { return base.ExciterApi; } }
        public ExciterSpot Spot { get; private set; }
        public ExciterSweep Sweep { get; private set; }
        public ExciterBarrage Barrage { get; private set; }
        public ExciterMultiSpot MultiSpot { get; private set; }
        public IExciterComb Comb { get; private set; }
        public IExciterHopping Hopping { get; private set; }


        public ExciterDateTime DateTime { get { return ExciterManagerApi.DateTime(); } }
        public Power PowerStatus { get { return ExciterManagerApi.PowerInfo(); } }
        public PowerSupply PowerSupplyStatus { get { return ExciterManagerApi.PowerSupplyInfo(); } }
        public Mmc MmcStatus { get { return ExciterManagerApi.MmcInfo(); } }
        public Modulator ModulatorStatus { get { return ExciterManagerApi.ModulatorInfo(); } }
        public Info ExciterInfo { get { return ExciterManagerApi.ExciterInfo(); } }
        public Rfcu Tray1Status { get { return ExciterManagerApi.Rfcu1(); } }
        public Rfcu Tray2Status { get { return ExciterManagerApi.Rfcu2(); } }
        public Info Warnings { get { return ExciterManagerApi.ExciterInfo(); } }
        public ushort ToneSource1 { get; set; }
        public ushort ToneSource2 { get; set; }
        public ushort MicVolume1 { get; set; }
        public ushort MicVolume2 { get; set; }
        public ExciterNoise NoiseSource1 { get; set; }
        public ExciterNoise NoiseSource2 { get; set; }

        public bool HoppingSwitch
        {
            get { return _hoppingSwitch; }
            set
            {
                _hoppingSwitch = value;
                ExciterManagerApi.HoppingSwitch(_hoppingSwitch);
            }
        }

        public bool RfOut
        {
            get { return _rfOut; }
            set
            {
                if (_rfOut == value)
                    return;

                _rfOut = value;
                ExciterManagerApi.RfOut(_rfOut);
            }
        }

        public void InitializeDefault()
        {
            // Need a place to store a return value in Mutex() constructor call
            bool createdNew;

            using (Mutex mutex = new Mutex(false, MutexId, out createdNew, SecuritySettings))
            {
                bool hasHandle = false;
                try
                {
                    try
                    {
                        // mutex.WaitOne(Timeout.Infinite, false);
                        hasHandle = mutex.WaitOne(MutexTimeout, false);
                        if (hasHandle == false)
                            throw new TimeoutException("Timeout waiting for exclusive access");                        
                    }
                    catch (AbandonedMutexException)
                    {
                        // Log the fact that the mutex was abandoned in another process, it will still get acquired
                        hasHandle = true;
                    }
                    catch (TimeoutException ex)
                    {
                        logger.Info("InitializeDefault failed by TimeoutException", ex);
                    }

                    // Perform your work here.
                    ExciterSpotApi.Spot(5000000, ExciterModulation.Cw);
                    Thread.Sleep(400);

                    ExciterManagerApi.ToneFrequency(1000, 1000);
                    Thread.Sleep(400);

                    //Exciter.MultiSpotPoint(0, 5000000, 20);
                    MultiSpot[0] = new Tuple<uint, byte>(5000000, 20);
                    Thread.Sleep(400);

                    //Exciter.MultiSpotPoint(1, 5000000, 20);
                    MultiSpot[1] = new Tuple<uint, byte>(5000000, 20);
                    Thread.Sleep(400);

                    for (byte i = 2; i < 16; i++)
                    {
                        MultiSpot[i] = new Tuple<uint, byte>(5000000, 0);
                        //Exciter.MultiSpotPoint(i, 5000000, 0);
                        Thread.Sleep(400);
                    }

                    Hopping.Gain = 20f;

                    Exciter = Spot;
                }
                finally
                {
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }
        }

        public void MutexSample()
        {
            // Need a place to store a return value in Mutex() constructor call
            bool createdNew;

            using (Mutex mutex = new Mutex(false, MutexId, out createdNew, SecuritySettings))
            {
                bool hasHandle = false;
                try
                {
                    try
                    {
                        // mutex.WaitOne(Timeout.Infinite, false);
                        hasHandle = mutex.WaitOne(MutexTimeout, false);
                        if (hasHandle == false)
                            throw new TimeoutException("Timeout waiting for exclusive access");
                    }
                    catch (AbandonedMutexException)
                    {
                        // Log the fact that the mutex was abandoned in another process, it will still get acquired
                        hasHandle = true;
                    }
                    catch (TimeoutException ex)
                    {
                        logger.Info("InitializeDefault failed by TimeoutException", ex);
                    }

                    // Perform your work here.
                    
                }
                finally
                {
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }
        }


        public void ActivateCurrentMode()
        {
            Activate(operation_);
        }

        private void Activate(ExciterOperationMode value)
        {
            operation_ = value;
            switch (operation_)
            {
                case ExciterOperationMode.Barrage:
                    Barrage.Activate();
                    break;
                case ExciterOperationMode.MultiSpot:
                    MultiSpot.Activate();
                    break;
                case ExciterOperationMode.Spot:
                    Spot.Activate();
                    break;
                case ExciterOperationMode.Sweep:
                    Sweep.Activate();
                    break;
                case ExciterOperationMode.Comb:
                    Comb.Activate();
                    break;
            }
        }

        public void SetModeInactvie(ExciterOperationMode operation)
        {
            operation_ = operation;
        }

        public ExciterOutputLine OutputLine1
        {
            get { return _outputLine1; }
            set
            {
                if (_outputLine1 == value)
                    return;

                ForceOutputLine1(value);
            }
        }

        public void ForceOutputLine1(ExciterOutputLine value)
        {
            _outputLine1 = value;
            switch (_outputLine1)
            {
                case ExciterOutputLine.Line1:
                    ExciterManagerApi.Line1(true, false);
                    break;
                case ExciterOutputLine.Line2:
                    ExciterManagerApi.Line2(true, false);
                    break;
                case ExciterOutputLine.Mic:
                    ExciterManagerApi.Mic(true, false);
                    break;
                case ExciterOutputLine.Mmc:
                    ExciterManagerApi.Mmc(true, false);
                    break;
                case ExciterOutputLine.Noise:
                    ExciterManagerApi.Noise(NoiseSource1, ExciterNoise.Off);
                    break;
                case ExciterOutputLine.Tone:
                    ExciterManagerApi.ToneFrequency(ToneSource1, 0);
                    break;
            }
        }

        public ExciterOutputLine OutputLine2
        {
            get { return _outputLine2; }
            set
            {
                if (_outputLine2 == value)
                    return;

                ForceOutputLine2(value);
            }
        }

        public void ForceOutputLine2(ExciterOutputLine value)
        {
            _outputLine2 = value;
            switch (_outputLine2)
            {
                case ExciterOutputLine.Line1:
                    ExciterManagerApi.Line1(false, true);
                    break;
                case ExciterOutputLine.Line2:
                    ExciterManagerApi.Line2(false, true);
                    break;
                case ExciterOutputLine.Mic:
                    ExciterManagerApi.Mic(false, true);
                    break;
                case ExciterOutputLine.Mmc:
                    ExciterManagerApi.Mmc(false, true);
                    break;
                case ExciterOutputLine.Noise:
                    ExciterManagerApi.Noise(ExciterNoise.Off, NoiseSource2);
                    break;
                case ExciterOutputLine.Tone:
                    ExciterManagerApi.ToneFrequency(0, ToneSource2);
                    break;
            }
        }

        private ushort _power;
        public override ushort Power
        {
            get { return _power; }
            set
            {
                _power = value;

                if (PowerState)
                    PowerOn();
            }
        }

        private bool _powerState = false;

        public bool PowerState
        {
            get { return _powerState; }
            set
            {
                _powerState = value;
                if (!PowerState)
                    PowerOff();
            }
        }

        public void PowerOn()
        {
            ExciterManagerApi.PowerOn(Power);
        }

        public void PowerOff()
        {
            ExciterManagerApi.PowerOff();
        }

        public void SwitchToLocalMode()
        {
            ExciterManagerApi.ControlMode(AccessMode.Local);
        }

        public void SwitchToRemoteMode()
        {
            ExciterManagerApi.ControlMode(AccessMode.Remote);
        }

        public void Reset()
        {
            ExciterManagerApi.Reset();
        }

        public void SelfTest()
        {
            ExciterManagerApi.SelfTest();
        }

        private void InitializeStatusLoop()
        {
            var thread = new Thread(() =>
            {
                while (_working)
                {
                    // Need a place to store a return value in Mutex() constructor call
                    bool createdNew;

                    using (Mutex mutex = new Mutex(false, MutexId, out createdNew, SecuritySettings))
                    {
                        bool hasHandle = false;
                        try
                        {
                            try
                            {
                                // mutex.WaitOne(Timeout.Infinite, false);
                                hasHandle = mutex.WaitOne(MutexTimeout, false);
                                if (hasHandle == false)
                                    throw new TimeoutException("Timeout waiting for exclusive access");
                            }
                            catch (AbandonedMutexException)
                            {
                                // Log the fact that the mutex was abandoned in another process, it will still get acquired
                                hasHandle = true;
                            }
                            catch (TimeoutException ex)
                            {
                                logger.Error("Request general status failed by TimeoutException", ex);
                            }

                            // Perform your work here.
                            try
                            {
                                logger.Info("Request General Status");
                                ExciterManagerApi.GeneralStatus();

                                logger.Info("Request Supply Status");
                                ExciterManagerApi.SupplyStatus();

                                logger.Info("Request Rfcu1 Status");
                                ExciterManagerApi.Rfcu1Status();

                                logger.Info("Request Rfcu2 Status");
                                ExciterManagerApi.Rfcu2Status();

                                logger.Info("Request Warning Status");
                                ExciterManagerApi.WarningStatus();
                            }
                            catch (InvalidOperationException ex)
                            {
                                logger.Error("Request general status failed: ", ex);
                                ExciterManagerApi.Dispose();
                            }
                            Thread.Sleep(2000);
                        }
                        finally
                        {
                            if (hasHandle)
                                mutex.ReleaseMutex();
                        }
                    }                    
                }

                ExciterManagerApi.Dispose();
            })
            {
                Name = "Exciter Device Status"
            };

            thread.Start();
        }

        public void Dispose()
        {
            _working = false;
        }

        public override sealed IExciter Activate()
        {
            //Default is Spot mode;
            operation_ = ExciterOperationMode.Spot;
            Spot.Activate();
            return this;
        }

        public IExciter Activate(IExciter exciter)
        {
            if (Exciter != exciter)
                Shutdown(Exciter);
            Exciter = exciter.Activate();
            operation_ = Exciter.Mode;
            return Exciter;
        }

        private void Shutdown(IExciter Exciter)
        {
            if (Exciter != null)
                Exciter.Shutdown();
        }

        public override IExciterController Controller
        {
            get { return this; }
        }

        /// <summary>
        /// Active exciter (operation exciter)
        /// </summary>
        public IExciter Exciter
        {
            get { return exciter_; }
            set { exciter_ = value; }
        }

        private IExciter exciter_;

        public ExciterOperationMode OperationMode
        {
            get { return operation_; }
            set
            {
                if (operation_ == value)
                    return;

                Activate(value);
            }
        }

        public override ExciterOperationMode Mode
        {
            get { throw new NotImplementedException(); }
        }

        public override ushort PowerMinimum
        {
            get { return 100; }
        }

        public override ushort PowerMaximum
        {
            get { return 2000; }
        }
    }
}
