using EighthBit.Exciter.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  

namespace EighthBit.Exciter
{
    public interface IExciterManagerApi : IExciterApi, IDisposable
    {
        IExciterManagerApi ControlMode(AccessMode exciterMode);
        ExciterDateTime DateTime();
        Power PowerInfo();
        Info ExciterInfo();
        Rfcu Rfcu1();
        Rfcu Rfcu2();
        Modulator ModulatorInfo();
        Mmc MmcInfo();
        PowerSupply PowerSupplyInfo();
        IExciterManagerApi HoppingSwitch(bool status);
        IExciterManagerApi RfOut(bool status);

        /// <summary>
        /// Request for getting all status
        /// For this request we send request with Id: 32
        /// </summary>
        /// <returns></returns>
        IExciterManagerApi AllStatus();

        /// <summary>
        /// Request for getting power data, vswr data, power on flag, error flag and warning flag
        /// For this request we send request with Id: 40 Data: 24
        /// </summary>
        /// <returns>We will receive General Status with ID: 87</returns>
        IExciterManagerApi GeneralStatus();

        /// <summary>
        /// Request for getting supply status
        /// For this request we send request with Id: 40 Data: 1
        /// </summary>
        /// <returns>We will receive PSCM status with ID: 51</returns>
        IExciterManagerApi SupplyStatus();

        /// <summary>
        /// Request for getting RFCU1 status
        /// For this request we send request with Id: 40 Data: 4
        /// </summary>
        /// <returns>We will receive RFCU1 status with ID: 54</returns>
        IExciterManagerApi Rfcu1Status();

        /// <summary>
        /// Request for getting RFCU2 status
        /// For this request we send request with Id: 40 Data: 16
        /// </summary>
        /// <returns>We will receive RFCU2 status with ID: 68</returns>
        IExciterManagerApi Rfcu2Status();

        /// <summary>
        /// Request for getting warning status
        /// For this request we send request with Id: 40 Data: 18
        /// </summary>
        /// <returns>We will receive temperture warning, over reflect warning, VSWR warning, over current warning, fuse warning and global warning status with ID: 72</returns>
        IExciterManagerApi WarningStatus();

        IExciterManagerApi Line1(bool source1, bool source2);
        IExciterManagerApi Line2(bool source1, bool source2);
        IExciterManagerApi Mic(bool source1, bool source2);
        IExciterManagerApi Mmc(bool source1, bool source2);
        IExciterManagerApi Noise(ExciterNoise noise1, ExciterNoise noise2);
        IExciterManagerApi ToneFrequency(ushort frequency1, ushort frequency2);
        IExciterManagerApi PowerOn(ushort power);
        IExciterManagerApi PowerOff();
        IExciterManagerApi SelfTest();
        IExciterManagerApi Reset();
        IExciterManagerApi NextTrack();
        IExciterManagerApi PreviousTrack();
        IExciterManagerApi RequestStatus();
        IExciterManagerApi EraseLog();
        IExciterManagerApi RequestData(byte id);
        IExciterManagerApi RequestData(ExciterCanBaudRate baudRate);

    }
}
