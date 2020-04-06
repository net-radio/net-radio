using EighthBit.Exciter.Can;
using EighthBit.Exciter.Parsers;
using EighthBit.Exciter.Parsers.ExciterParser1Kw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter
{
    class ExciterApi1Kw : ExciterApiBase, IExciterManagerApi, IExciterSpotApi, IExciterBarrageApi, IExciterSweepApi, IExciterMultiSpotApi
    {
        public ExciterApi1Kw(ICanControl canControl)
            : base(canControl)
        {
            _info = new Info1KwImpl(this);
            _mmc = new Mmc1KwImpl(this);
            _modulator = new Modulator1KwImpl(this);
            _power = new Power1KwImpl(this);
            _powerSupply = new PowerSupply1KwImpl(this);
            _rfcu1 = new Rfcu1KwImpl(this, Rfcus.Rfcu1);
            // _rfcu2 = new Rfcu1KwImpl(this, Rfcus.Rfcu2);
            _dateTime = new ExciterDateTime1KwImpl(this);
            _combParser = new CombParser1KwImpl(this);
        }

        // Exciter Manager
        /*
        public IExciterManagerApi ControlMode(ExciterMode mode)
        {
            var frame = new CanFrameWriter().Id(41).Write((byte)mode).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }
        */

        public IExciterManagerApi ControlMode(AccessMode exciterMode)
        {
            throw new NotImplementedException();
        }

        public ExciterDateTime DateTime()
        {
            return _dateTime;
        }

        public Info GeneralInfo()
        {
            return _info;
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
            var frame = new CanFrameWriter().Id(ID_POWER).Write((byte)80).Write(power, true).Write(true).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterManagerApi Reset()
        {
            var frame = new CanFrameWriter().Id(ID_POWER).Write((byte)82).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        /*
        public IExciterManagerApi RfOut(bool status)
        {
            var frame = new CanFrameWriter().Id(ID_POWER).Write((byte)67).Write(status).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }
        */

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
            if (id == 21)
                Console.WriteLine();
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
            var frame = new CanFrameWriter().Id(ID_MODULATION).Write((byte)74).Write((byte)65).Write(frequency, true).Write((byte)modulation).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }


        // Barrage
        public IExciterBarrageApi Barrage(uint frequency, ushort bandwidth)
        {
            var frame = new CanFrameWriter().Id(ID_MODULATION).Write((byte)74).Write((byte)66).Write(frequency, true).Write(bandwidth, true).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }


        // Sweep
        public IExciterSweepApi Sweep(uint step, ExciterModulation modulation, byte time)
        {
            var frame = new CanFrameWriter().Id(ID_MODULATION).Write((byte)74).Write((byte)68).Write((byte)modulation).Write(step, true).Write(time).ToCanFrame(PadZero);
            _canControl.Send(frame);

            return this;
        }

        public IExciterSweepApi SweepDomain(uint startFrequency, uint stopFrequency)
        {
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

        public IExciterManagerApi RfOut(bool status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Request for getting power data, vswr data, power on flag, error flag and warning flag
        /// For this request we send request with Id: 40 Data: 24
        /// </summary>
        /// <returns></returns> 
        public IExciterManagerApi GeneralStatus()
        {
            return RequestInfo(24);
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
        /// Request special information by send ID: 40
        /// </summary>
        /// <param name="id"></param>
        /// <returns>We will receive exciter status based on request ID</returns>
        private IExciterManagerApi RequestInfo(byte id)
        {
            var frame = new CanFrameWriter().Id(40).Write(id).ToCanFrame(PadZero);
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
            return RequestInfo(1);
        }

        /// <summary>
        /// Request for getting RFCU1 status
        /// For this request we send request with Id: 40 Data: 4
        /// </summary>
        /// <returns>We will receive RFCU1 status with ID: 54</returns>
        public IExciterManagerApi Rfcu1Status()
        {
            return RequestInfo(4);
        }

        /// <summary>
        /// Exciter 1Kw do not support RFCU2
        /// </summary>
        /// <returns>Exception</returns>
        public IExciterManagerApi Rfcu2Status()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Request for getting warning status
        /// For this request we send request with Id: 40 Data: 18
        /// </summary>
        /// <returns>We will receive temperture warning, over reflect warning, VSWR warning, over current warning, fuse warning and global warning status with ID: 72</returns>
        public IExciterManagerApi WarningStatus()
        {
            return RequestInfo(18);
        }
    }
}
