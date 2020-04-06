using EighthBit.Exciter.Can;
using EighthBit.Exciter.Configuration.Comb;
using EighthBit.Exciter.Origin;
using EighthBit.Exciter.Parsers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EighthBit.Exciter
{
    public class ExciterCombSepehr : ExciterSubSystem, IExciterComb
    {
        public ExciterCombSepehr(IExciter exciter)
            : base(exciter.ExciterApi)
        {
            controller_ = exciter.Controller;
        }

        private readonly uint[] _frequency = new uint[10];
        private readonly float[] _phase = new float[10];
        private byte _numberOfCommand;
        // private ICollection<Command> _commandCollection;

        public event EventHandler ResponseEventHandler;

        private bool _sending = false;

        private double _peakToAvaragePowerRatio;

        protected IExciterCombApi ExciterCombApi
        {
            get { return base.ExciterApi as IExciterCombApi; }
        }

        public double PeakToAvaragePowerRatio
        {
            get { return _peakToAvaragePowerRatio; }
            set
            {
                _peakToAvaragePowerRatio = value;
            }
        }

        public bool Sending
        {
            get { return _sending; }
            set { _sending = value; }
        }


        private void SendCommand()
        {
            foreach (var command in _commands)
            {
                ExciterCombApi.SendCommand(command.Cmd, command.Param);
                Thread.Sleep(100);
            }
        }

        public bool AmpiltudeOver { get; set; }

        public override IExciter Activate()
        {
            _sending = true;

            Task.Factory.StartNew(() =>
            {
                _numberOfCommand = (byte)(_commands.Count);
                ExciterCombApi.StartDataTransfer(_numberOfCommand);
                Thread.Sleep(800);
                SendCommand();
            });

            return this;
        }

        private void OnExciterAcknoledge(object sender, EventArgs e)
        {
            CanFrame frame = (CanFrame)e;
            uint id = frame.Id;

            switch (id)
            {
                case 50:
                    break; //Supply Temp
                case 51:
                    break; //Supply Status
                case 52:
                    break; //Supply Analogs
                case 53:
                    break; //RF1 Temp
                case 54:
                    break; //RF1 Status
                case 55:
                    break; //RF1 Analogs
                case 56:
                    break; //RF1 Analogs
                case 57:
                    break; //RF1 Analogs
                case 58:
                    break; //powerOn
                // case 58: break; //powerOff
                case 59:
                    break; //Time
                case 60:
                    break; //Date
                case 61:
                    break; //Audio Source
                case 62:
                    break; //
                case 63:
                    break; //
                case 64:
                    break; //
                case 65:
                    break; //MMC
                case 66:
                    break; //
                case 67:
                    break; //RF1 Temp
                case 68:
                    break; //RF1 Status
                case 69:
                    break; //RF1 Analogs
                case 70:
                    break; //RF1 Analogs
                case 71:
                    break; //RF1 Analogs
                case 72:
                    break; //Warning
                case 73:
                    break; //
                case 74:
                    break; //
                case 75:
                    break; //
                case 76:
                    break; //
                case 77:
                    break; //
                case 78:
                    break; //
                case 79:
                    break; //
                case 80:
                    break; //
                case 81:
                    break; //
                case 82:
                    break; //
                case 83:
                    break; //

                    /*
                case CombParser.ID_DATA_TRANSFER_ACKNOLDGE: // 84
                    if (_sending)
                        
                    _sending = false;
                    break;

                case CombParser.ID_COMB_AMPLITUTE_OVER_WARNING: // 85
                    AmpiltudeOver = true;
                    if (ResponseEventHandler != null)
                        ResponseEventHandler(this, EventArgs.Empty);
                    break;

                case CombParser.ID_END_COMB_DATA_TRANSFER: // 86
                    Console.WriteLine();
                    break;
                     */ 
                default:
                    break;
            }
        }

        private List<Command> _commands;

        [Obsolete("This type has been expired", true)]
        public void PrepareSignals(SignalSpecification[] signals)
        {
            uint numberOfFreq = 0;

            List<SignalSpecification> enabledSignal = new List<SignalSpecification>();

            for (int i = 0; i < 10; i++)
            {
                if (signals[i].Enabled)
                    enabledSignal.Add(signals[i]);
            }

            numberOfFreq = (uint)enabledSignal.Count;

            _commands = new List<Command>();
            _commands.Add(new Command("MMOD", 110));
            _commands.Add(new Command("MRST", 28));
            _commands.Add(new Command("CNOC", (uint)numberOfFreq));
            _commands.Add(new Command("PAPR", (uint)(100 * _peakToAvaragePowerRatio)));
            _commands.Add(new Command("CAMP", 1));

            int index = 1;
            foreach (var signal in enabledSignal)
            {
                String cmd = String.Format("CF{0}", String.Format("{0}", index).PadLeft(2, '0'));
                _commands.Add(new Command(cmd, signal.Frequency));
                index++;
            }

            index = 1;
            foreach (var combEntry in enabledSignal)
            {
                String cmd = String.Format("CP{0}", String.Format("{0}", index).PadLeft(2, '0'));
                _commands.Add(new Command(cmd, (uint)(10 * combEntry.Phase)));
                index++;
            }

            _commands.Add(new Command("EXPW", 119 + numberOfFreq));
            _commands.Add(new Command("CSYN", 24));
            _commands.Add(new Command("CSYN", 21));
        }

        public void CombCoreOff()
        {
            /* Sepehr version
            _commands = new List<Command>();
            _commands.Add(new Command("EXPW", 151));

            // Exciter.Updated += OnExciterAcknoledge;
            _sending = true;

            _numberOfCommand = (byte)(_commands.Count);
            ExciterCombApi.StartDataTransfer(_numberOfCommand);
            Thread.Sleep(400);
            SendCommand();
            */ 
        }

        public void SetPower(uint combPower)
        {
            _commands = new List<Command>();
            _commands.Add(new Command("CAMP", combPower));

            // Exciter.Updated += OnExciterAcknoledge;
            AmpiltudeOver = false;

            _sending = true;

            Task.Factory.StartNew(() =>
            {
                _numberOfCommand = (byte)(_commands.Count);
                ExciterCombApi.StartDataTransfer(_numberOfCommand);
                Thread.Sleep(800);
                SendCommand();
            });
        }

        public override IExciterController Controller
        {
            get { return controller_; }
        }

        public new IExciterApi ExciterApi
        {
            get { return base.ExciterApi; }
        }

        protected IExciterCombApi CombApi
        {
            get { return ExciterApi as IExciterCombApi; }
        }

        public override ExciterOperationMode Mode
        {
            get { return ExciterOperationMode.Comb; }
        }

        private ushort power_;

        public override ushort Power
        {
            get { return power_; }
            set
            {
                power_ = value;

                /*
                var refMax = CombView.RefMax;

                if (combPower > refMax)
                    combPower = refMax;
                */

                SetPower(power_);
            }
        }

        public override ushort PowerMinimum
        {
            get { return 0; }
        }

        public override ushort PowerMaximum
        {
            get { return 26; }
        }

        public override IExciter Shutdown()
        {
            CombCoreOff();
            return base.Shutdown();
        }





        public void Apply(ExciterModulation modulation, ushort exists, ushort power)
        {
            throw new NotImplementedException();
        }
    }
}
