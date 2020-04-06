using System;
using System.Collections.Generic;
using System.Diagnostics;
using EighthBit.Exciter.Can;
using EighthBit.Exciter.Parsers;
using EighthBit.Exciter.Configuration.Comb;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Reflection;
using EighthBit.Exciter.Parsers.ExciterParser2Kw;

namespace EighthBit.Exciter
{
    public class ExciterApi2Kw : ExciterApiBase, IExciterManagerApi, IExciterSpotApi, IExciterBarrageApi, IExciterSweepApi, IExciterMultiSpotApi, IExciterCombApi, IExciterHoppingApi
    {
        public ExciterApi2Kw(ICanControl canControl)
            : base(canControl)
        {
            _info = new Info2KwImpl(this);
            _mmc = new Mmc2KwImpl(this);
            _modulator = new Modulator2KwImpl(this);
            _power = new Power2KwImpl(this);
            _powerSupply = new PowerSupply2KwImpl(this);
            _rfcu1 = new Rfcu2KwImpl(this, Rfcus.Rfcu1);
            _rfcu2 = new Rfcu2KwImpl(this, Rfcus.Rfcu2);
            _dateTime = new ExciterDateTime2KwImpl(this);
            _combParser = new CombParser2KwImpl(this);
        }


        // Exciter Manager
        public IExciterManagerApi ControlMode(AccessMode mode)
        {
            var frame = new CanFrameWriter().Id(41).Write((byte)mode).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public ExciterDateTime DateTime()
        {
            return _dateTime;
        }

        public Info GeneralInfo()
        {
            return _info;
        }

        /// <summary>
        /// Request for getting power data, vswr data, power on flag, error flag and warning flag
        /// For this request we send request with Id: 40 Data: 24
        /// </summary>
        /// <returns></returns>
        public IExciterManagerApi GeneralStatus()
        {
            var frame = new CanFrameWriter().Id(40).Write((byte)24).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi HoppingSwitch(bool status)
        {
            var frame = new CanFrameWriter().Id(33).Write(status).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi Line1(bool source1, bool source2)
        {
            var frame = new CanFrameWriter().Id(ID_SOURCE).Write((byte)(source1 ? 65 : 0)).Write((byte)0).Write((byte)0).Write((byte)0).Write((byte)(source2 ? 65 : 0)).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi Line2(bool source1, bool source2)
        {
            var frame = new CanFrameWriter().Id(ID_SOURCE).Write((byte)(source1 ? 66 : 0)).Write((byte)0).Write((byte)0).Write((byte)0).Write((byte)(source2 ? 66 : 0)).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi Mic(bool source1, bool source2)
        {
            var frame = new CanFrameWriter().Id(ID_SOURCE).Write((byte)(source1 ? 67 : 0)).Write((byte)0).Write((byte)0).Write((byte)0).Write((byte)(source2 ? 67 : 0)).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi Mmc(bool source1, bool source2)
        {
            var frame = new CanFrameWriter().Id(ID_SOURCE).Write((byte)(source1 ? 70 : 0)).Write((byte)0).Write((byte)0).Write((byte)0).Write((byte)(source2 ? 70 : 0)).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi Noise(ExciterNoise noise1, ExciterNoise noise2)
        {
            var frame = new CanFrameWriter().Id(ID_SOURCE).Write((byte)68).Write((byte)noise1).Write((byte)0).Write((byte)0).Write((byte)68).Write((byte)noise2).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi PowerOff()
        {
            var frame = new CanFrameWriter().Id(ID_POWER).Write((byte)79).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi PowerOn(ushort power)
        {
            var frame = new CanFrameWriter().Id(ID_POWER).Write((byte)80).Write(power, true).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi Reset()
        {
            var frame = new CanFrameWriter().Id(ID_POWER).Write((byte)82).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi RfOut(bool status)
        {
            var frame = new CanFrameWriter().Id(ID_POWER).Write((byte)67).Write(status).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi SelfTest()
        {
            var frame = new CanFrameWriter().Id(34).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi ToneFrequency(ushort frequency1, ushort frequency2)
        {
            var frame = new CanFrameWriter().Id(ID_SOURCE).Write((byte)69).Write(frequency1, true).Write((byte)0).Write((byte)69).Write((byte)69).Write(frequency2, true).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi NextTrack()
        {
            var frame = new CanFrameWriter().Id(ID_MMC).Write((byte)78).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi PreviousTrack()
        {
            var frame = new CanFrameWriter().Id(ID_MMC).Write((byte)80).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi RequestStatus()
        {
            var frame = new CanFrameWriter().Id(32).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi EraseLog()
        {
            var frame = new CanFrameWriter().Id(0).Write((byte)255).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi RequestData(byte id)
        {
            var frame = new CanFrameWriter().Id(40).Write(id).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi RequestData(ExciterCanBaudRate baudRate)
        {
            var frame = new CanFrameWriter().Id(31).Write((byte)baudRate).Write((byte)0).Write((byte)255).Write((byte)0).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }
        

        // Spot
        public IExciterSpotApi Spot(uint frequency, ExciterModulation modulation)
        {
            logger.Info(string.Format("Apply exciter spot with frequency: {0}, modulation: {1}", frequency, modulation));
            var frame = new CanFrameWriter().Id(ID_MODULATION).Write((byte)74).Write((byte)65).Write(frequency, true).Write((byte)modulation).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }


        // Barrage
        public IExciterBarrageApi Barrage(uint frequency, ushort bandwidth)
        {
            logger.Info(string.Format("Apply exciter barrage with frequency: {0}, bandwidth: {1}", frequency, bandwidth));
            var frame = new CanFrameWriter().Id(ID_MODULATION).Write((byte)74).Write((byte)66).Write(frequency, true).Write(bandwidth, true).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }


        // Sweep
        public IExciterSweepApi Sweep(uint step, ExciterModulation modulation, byte time)
        {
            logger.Info(string.Format("Apply exciter sweep with step: {0}, modulation: {1}, time: {2}", step, modulation, time));
            var frame = new CanFrameWriter().Id(ID_MODULATION).Write((byte)74).Write((byte)68).Write((byte)modulation).Write(step, true).Write(time).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterSweepApi SweepDomain(uint startFrequency, uint stopFrequency)
        {
            logger.Info(string.Format("Apply exciter sweep domain with start frequency: {0}, stop frequency: {1}", startFrequency, stopFrequency));
            var frame = new CanFrameWriter().Id(13).Write(startFrequency, true).Write(stopFrequency, true).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }
        

        // Multi Spot
        public IExciterMultiSpotApi MultiSpot(byte pointsCount, ExciterModulation modulation)
        {
            var frame = new CanFrameWriter().Id(ID_MODULATION).Write((byte)74).Write((byte)67).Write(pointsCount).Write((byte)modulation).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterMultiSpotApi MultiSpotPoint(byte index, uint frequency, byte time)
        {
            if (index > 15)
                throw new ArgumentOutOfRangeException("index");

            var id = (uint)(index + 14);//first can Id is 14;

            var frame = new CanFrameWriter().Id(id).Write(frequency, true).Write(time).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }


        // Comb
        public IExciterCombApi StartDataTransfer(byte numberOfPackets)
        {
            var frame = new CanFrameWriter().Id(ID_START_DATA_TRANSFER).Write(numberOfPackets).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterCombApi SendCommand(byte[] command, uint param)
        {
            var frame = new CanFrameWriter().Id(ID_COMMAND).Write(command).Write(param, true).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi RequestInfo(byte id)
        {
            var frame = new CanFrameWriter().Id(40).Write(id).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterCombApi Frequency(byte id, uint frequency, double phase)
        {
            var frame = new CanFrameWriter().Id(0x33).Write(id).Write(frequency).Write((ushort)(phase*10.0)).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterCombApi Comb(ExciterModulation modulation, ushort frequencies, ushort amplitude)
        {
            byte cmbModulation;
            switch (modulation)
            {
                case ExciterModulation.Cw:
                    cmbModulation = 1;
                    break;
                case ExciterModulation.Am:
                    cmbModulation = 2;
                    break;
                case ExciterModulation.Fm:
                    cmbModulation = 3;
                    break;
                case ExciterModulation.Usb:
                    cmbModulation = 4;
                    break;
                case ExciterModulation.Lsb:
                    cmbModulation = 5;
                    break;
                case ExciterModulation.Dsb:
                    cmbModulation = 6;
                    break;
                case ExciterModulation.Isb:
                    cmbModulation = 7;
                    break;
                default:
                    cmbModulation = 2;
                    break;
            }

            var frame = new CanFrameWriter().Id(0x34).Write(cmbModulation).Write(frequencies).Write(amplitude).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;

        }

        IExciterHoppingApi IExciterHoppingApi.Apply(uint frequencyStart, uint frequencyStop, ExciterModulation modulation)
        {
            var frame1 = new CanFrameWriter().Id(0x35).Write(frequencyStart).Write(frequencyStop).ToCanFrame(PadZero);
            _canControl.Send(frame1);

            byte cmbModulation;
            switch (modulation)
            {
                case ExciterModulation.Cw:
                    cmbModulation = 1;
                    break;
                case ExciterModulation.Am:
                    cmbModulation = 2;
                    break;
                case ExciterModulation.Fm:
                    cmbModulation = 3;
                    break;
                case ExciterModulation.Usb:
                    cmbModulation = 4;
                    break;
                case ExciterModulation.Lsb:
                    cmbModulation = 5;
                    break;
                case ExciterModulation.Dsb:
                    cmbModulation = 6;
                    break;
                case ExciterModulation.Isb:
                    cmbModulation = 7;
                    break;
                default:
                    cmbModulation = 2;
                    break;
            }

            ushort amplitude = 16384;
            var frame2 = new CanFrameWriter().Id(0x36).Write(true).Write(cmbModulation).Write(amplitude).ToCanFrame(PadZero);
            _canControl.Send(frame2);

            return this;
        }

        /// <summary>
        /// Request for getting all status
        /// For this request we send request with Id: 32
        /// </summary>
        /// <returns></returns>
        public IExciterManagerApi AllStatus()
        {
            var frame = new CanFrameWriter().Id(32).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }


        /// <summary>
        /// Request for getting Supply Status
        /// For this request we send request with Id: 40 Data: 1
        /// </summary>
        /// <returns>We will receive PSCM status with ID: 51</returns>
        public IExciterManagerApi SupplyStatus()
        {
            var frame = new CanFrameWriter().Id(40).Write((byte)1).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        /// <summary>
        /// Request for getting RFCU1 status
        /// For this request we send request with Id: 40 Data: 4
        /// </summary>
        /// <returns>We will receive RFCU1 status with ID: 54</returns>
        public IExciterManagerApi Rfcu1Status()
        {
            var frame = new CanFrameWriter().Id(40).Write((byte)4).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        /// <summary>
        /// Request for getting RFCU2 status
        /// For this request we send request with Id: 40 Data: 16
        /// </summary>
        /// <returns>We will receive RFCU2 status with ID: 68</returns>
        public IExciterManagerApi Rfcu2Status()
        {
            var frame = new CanFrameWriter().Id(40).Write((byte)16).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        /// <summary>
        /// Request for getting warning status
        /// For this request we send request with Id: 40 Data: 18
        /// </summary>
        /// <returns>We will receive temperture warning, over-reflect warning, VSWR warning, over-current warning, fuse warning and global warning status with ID: 72</returns>
        public IExciterManagerApi WarningStatus()
        {
            var frame = new CanFrameWriter().Id(40).Write((byte)18).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }
    }
}
