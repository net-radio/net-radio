using System;
using System.Runtime.InteropServices;
using NetRadio.Devices.Exceptions;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents WinRadio G313 radio receiver context.
    /// </summary>
    public class G313Radio:Radio<G313Radio>,IProviders<G313Radio>
    {
        private readonly G313BlockScanner _scanner;
        private readonly G313Dsp _dsp;
        private readonly G313Demodulator _demodulator;

        /// <summary>
        /// Gets Demodulator state.
        /// </summary>
        public bool DemodulatorInitialized { get; private set; }

        /// <summary>
        /// Gets cached information about underlying Radio hardware.
        /// </summary>
        public RadioInfo CachedInfo { get; private set; }

        /// <summary>
        /// Checks fluent result of execution.
        /// </summary>
        /// <param name="result">Result of execution</param>
        /// <param name="message">Failure message of execution.</param>
        /// <returns>Return <see cref="G313Demodulator"/></returns>
        protected G313Radio CheckFluent(bool result, string message = null)
        {
            if (!result)
                throw new OperationFailedException(message, this);

            return this;
        }

        /// <summary>
        /// Checks fluent result upper limit.
        /// </summary>
        /// <param name="result">Result of execution.</param>
        protected void CheckResultOnMax(uint result)
        {
            if (result == 0xFFFFFFFF)
                throw new InvalidRadioException(this);
        }

        /// <summary>
        /// Checks fluent result invalid negative state.
        /// </summary>
        /// <param name="result">Result of execution.</param>
        protected void CheckResultOnNegative(int result)
        {
            if (result == -1)
                throw new InvalidRadioException(this);
        }

        /// <summary>
        /// Checks fluent result invalid zero state.
        /// </summary>
        /// <param name="result">Result of execution.</param>
        protected void CheckResultOnZero(uint result)
        {
            if (result == 0)
                throw new InvalidRadioException(this);
        }

        /// <summary>
        /// Checks fluent result invalid zero state.
        /// </summary>
        /// <param name="result">Result of execution.</param>
        protected void CheckResultOnZero(int result)
        {
            if (result == 0)
                throw new InvalidRadioException(this);
        }

        /// <summary>
        /// Checks demodulator state.
        /// </summary>
        protected void CheckDemodulator()
        {
            if (!DemodulatorInitialized)
                throw new DemodulatorNotReadyException(this);
        }

        private void CheckFrequency(uint frequency)
        {
            if (frequency > CachedInfo.MaxFrequency || frequency < CachedInfo.MinFrequency)
                throw new ArgumentOutOfRangeException("frequency", "frequency should be defined within the harware domain");
        }

        private void CheckIfGain(int value)
        {
            if (value < 0 || value > 120)
                throw new ArgumentOutOfRangeException("value", "IF gain should set within 0-100 boundry");
        }

        private int GetHandle()
        {
            return Handle.ToInt32();
        }

        /// <summary>
        /// The DSP must be initialized before any signal processing, including signal strength measurement, can commence. When initializing the demodulator the full path to a valid calibration data file may be provided if signal strength measuremen must be calibrated. If not so, the passed pointer should be NULL.
        /// </summary>
        /// <param name="calibrateDataPath">The file containing the calibration data for the receiver in use.</param>
        /// <returns>Returns <see cref="G313Demodulator"/> context of demodulator.</returns>
        public G313Demodulator Demodulator(string calibrateDataPath=null)
        {
            if (DemodulatorInitialized)
                return _demodulator;

            DemodulatorInitialized = G313Api.InitializeDemodulator(GetHandle(), calibrateDataPath);
            if (!DemodulatorInitialized)
                throw new InvalidOperationException("failed to initialize demodulator.");
            _demodulator.SetupStreams();

            return _demodulator;
        }

        /// <summary>
        /// The GetInternalRSSI function returns a combination of the RSSI and AGC values read from the receiver hardware.
        /// </summary>
        /// <returns>Returns <see cref="G313.InternalRssi"/> read parameters from receiver</returns>
        public InternalRssi InternalRssi()
        {
            var result= G313Api.GetInternalRSSI(GetHandle());
            return new InternalRssi(result);
        }

        /// <summary>
        /// Sets the frequency the device is to be tuned to.
        /// </summary>
        /// <param name="frequency">Specified frequency</param>
        /// <returns>Returns true if the specified frequency is set.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the specified frequency is out of operation range</exception>
        public bool TryFrequency(uint frequency)
        {
            CheckFrequency(frequency);
            return G313Api.SetFrequency(GetHandle(), frequency);
        }

        /// <summary>
        /// Sets the frequency the device is to be tuned to.
        /// </summary>
        /// <param name="frequency">Specified frequency</param>
        /// <returns>Returns <see cref="G313Radio"/> radio instance</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the specified frequency is out of operation range</exception>
        public G313Radio Frequency(uint frequency)
        {
            var result = TryFrequency(frequency);
            return CheckFluent(result, string.Format("failed to set frequency, value:'{0}'", frequency));
        }

        /// <summary>
        /// Retrieves the frequency the receiver is tuned to.
        /// </summary>
        /// <returns>Returns tuned frequency.</returns>
        public uint Frequency()
        {
            var result= G313Api.GetFrequency(GetHandle());
            CheckResultOnZero(result);

            return result;
        }

        /// <summary>
        /// Starts the sequence of hardware commands that sets the frequency the device is to be tuned to. In order to insure the sequence has been executed and that the hardware is properly tuned, WaitFrequencyAsync function must be called.
        /// </summary>
        /// <param name="frequency">Specified frequency.</param>
        /// <returns>Returns true if the command is issued.</returns>
        public bool TryFrequencyAsync(uint frequency)
        {
            CheckFrequency(frequency);
            return G313Api.SetFreqAsync(GetHandle(), frequency);
        }

        /// <summary>
        /// Starts the sequence of hardware commands that sets the frequency the device is to be tuned to. In order to insure the sequence has been executed and that the hardware is properly tuned, WaitFrequencyAsync function must be called.
        /// </summary>
        /// <param name="frequency">Specified frequency.</param>
        /// <returns><see cref="G313Radio"/> radio instance.</returns>
        public G313Radio FrequencyAsync(uint frequency)
        {
            var result = TryFrequencyAsync(frequency);
            return CheckFluent(result, string.Format("failed to set frequency, value:'{0}'", frequency));
        }

        /// <summary>
        /// Waits for the end of a receiver tunning started by a <see cref="FrequencyAsync"/> call.
        /// </summary>
        /// <returns>Returns true on successful block.</returns>
        public bool WaitFrequencyAsync()
        {
            return G313Api.WaitFreqAsync(GetHandle());
        }

        /// <summary>
        /// sets the center frequency of the last IF signal.
        /// </summary>
        /// <param name="frequency">Specifies the IF2 frequency. It can be either positive or negative, negative values causing an IF2 spectrum inversion</param>
        /// <returns>Returns true if the specified value is set.</returns>
        public bool TryIf2Frequency(int frequency)
        {
            return G313Api.SetIF2Frequency(GetHandle(), frequency);
        }

        /// <summary>
        /// sets the center frequency of the last IF signal.
        /// </summary>
        /// <param name="frequency">Specifies the IF2 frequency. It can be either positive or negative, negative values causing an IF2 spectrum inversion</param>
        /// <returns><see cref="G313Radio"/> radio instance.</returns>
        public G313Radio If2Frequency(int frequency)
        {
            var result = TryIf2Frequency(frequency);
            return CheckFluent(result, string.Format("failed to set IF frequency, value:'{0}'", frequency));
        }

        /// <summary>
        /// Retrieves the current IF2 frequency.
        /// </summary>
        /// <returns>The current receiver frequency in Hz. If the handle is invalid, 0 is returned instead.</returns>
        public int If2Frequency()
        {
            var result= G313Api.GetIF2Frequency(GetHandle());
            CheckResultOnZero(result);

            return result;
        }

        /// <summary>
        /// Activates or deactivates the RF input attenuator. It is used to prevent overloading of the receiver with strong signals.
        /// </summary>
        /// <param name="state">Specified state.</param>
        /// <returns>Returns true if the specified state is set.</returns>
        public bool TryAttenuator(bool state)
        {
            return G313Api.SetAtten(GetHandle(), state);
        }

        /// <summary>
        /// Activates or deactivates the RF input attenuator. It is used to prevent overloading of the receiver with strong signals.
        /// </summary>
        /// <param name="state">Specified state.</param>
        /// <returns><see cref="G313Radio"/> radio instance.</returns>
        public G313Radio Attenuator(bool state)
        {
            var result = TryAttenuator(state);
            return CheckFluent(result, string.Format("failed to set attenuator state, value:'{0}'", state));
        }

        /// <summary>
        /// Retrieves the RF input attenuator setting.
        /// </summary>
        /// <returns>Returns the attenuator value.</returns>
        public bool Attenuator()
        {
            return G313Api.GetAtten(GetHandle());
        }

        /// <summary>
        /// Sets radio power state.
        /// </summary>
        /// <param name="state">Specified power state.</param>
        /// <returns>Returns true if the specified value is set.</returns>
        public bool TryPower(bool state)
        {
            return G313Api.SetPower(GetHandle(), state);
        }

        /// <summary>
        /// Sets radio power state.
        /// </summary>
        /// <param name="state">Specified power state.</param>
        /// <returns>Returns <see cref="G313Radio"/> radio instance</returns>
        public G313Radio Power(bool state)
        {
            var result = TryPower(state);
            return CheckFluent(result, string.Format("failed to set power state, value:'{0}'", state));
        }

        /// <summary>
        /// Retrieves radio power state.
        /// </summary>
        /// <returns>Returns power state.</returns>
        public bool Power()
        {
            return G313Api.GetPower(GetHandle());
        }

        /// <summary>
        /// Sets Automatic gain control mode.
        /// </summary>
        /// <param name="value"><see cref="G313.Agc"/> Specified mode</param>
        /// <returns>Returns true if the specified mode is set.</returns>
        public bool TryAgc(Agc value)
        {
            return G313Api.SetAGC(GetHandle(), (int)value);
        }


        /// <summary>
        /// Sets Automatic gain control mode.
        /// </summary>
        /// <param name="value"><see cref="G313.Agc"/> Specified mode</param>
        /// <returns>Returns <see cref="G313Radio"/> radio instance.</returns>
        public G313Radio Agc(Agc value)
        {
            var result = TryAgc(value);
            return CheckFluent(result, string.Format("failed to set agc, value:'{0}'", value));
        }

        /// <summary>
        /// Gets Automatic gain control mode.
        /// </summary>
        /// <returns>Returns <see cref="G313.Agc"/> Automatic gain control state</returns>
        public Agc Agc()
        {
            var result= G313Api.GetAGC(GetHandle());
            CheckResultOnNegative(result);

            return (Agc)result;
        }

        /// <summary>
        /// Sets IFGain value.
        /// </summary>
        /// <param name="value">Specified value</param>
        /// <returns>Returns true if the specified value is set.</returns>
        public bool TryIfGain(int value)
        {
            CheckIfGain(value);
            return G313Api.SetIFGain(GetHandle(), value);
        }

        /// <summary>
        /// Sets IFGain value.
        /// </summary>
        /// <param name="value">Specified value</param>
        /// <returns>Returns <see cref="G313Radio"/> radio instance.</returns>
        public G313Radio IfGain(int value)
        {
            var result = TryIfGain(value);
            return CheckFluent(result, string.Format("failed to set IF gain, value:'{0}'", value));
        }

        /// <summary>
        /// Retrieves IF gain value.
        /// </summary>
        /// <returns>returns IF gain value.</returns>
        public int IfGain()
        {
            var result= G313Api.GetIFGain(GetHandle());
            CheckResultOnNegative(result);

            return result;
        }

        /// <summary>
        /// Specifies the reference clock frequency and allows switching between internal and external references.
        /// </summary>
        /// <param name="value">Specified value</param>
        /// <returns>Returns true if the specified value is set</returns>
        public bool TryReferenceClock(uint value)
        {
            return G313Api.SetRefClock(GetHandle(), value);
        }

        /// <summary>
        /// Specifies the reference clock frequency and allows switching between internal and external references.
        /// </summary>
        /// <param name="value">Specified value</param>
        /// <returns>Returns <see cref="G313Radio"/> radio instance</returns>
        public G313Radio ReferenceClock(uint value)
        {
            var result = TryReferenceClock(value);
            return CheckFluent(result, string.Format("failed to set reference clock, value:'{0}'", value));
        }

        /// <summary>
        /// Specifies default reference clock frequency and allows switching between internal and external references.
        /// </summary>
        /// <returns>Returns true if internal clock is used.</returns>
        public bool TryUseInternalClock()
        {
            return G313Api.SetRefClock(GetHandle(), 0);
        }

        /// <summary>
        /// Specifies default reference clock frequency and allows switching between internal and external references.
        /// </summary>
        /// <returns>Returns <see cref="G313Radio"/> radio instance</returns>
        public G313Radio UseInternalClock()
        {
            var result = TryUseInternalClock();
            return CheckFluent(result);
        }

        /// <summary>
        /// Retrieves the PLLs lock status.
        /// </summary>
        /// <returns>If the function succeeds, the return value is a combination of the PLLs lock bits. Otherwise 0 is returned.</returns>
        public uint PllLock()
        {
            var result= G313Api.GetLock(GetHandle(),0);
            CheckResultOnZero(result);

            return result;
        }

        /// <summary>
        /// Retrieves the current reference clock frequency.
        /// </summary>
        /// <returns>Returns current clock.</returns>
        public uint ReferenceClock()
        {
            var result= G313Api.GetRefClock(GetHandle());
            CheckResultOnMax(result);

            return result;
        }

        /// <summary>
        /// Gets block scanner context of current receiver.
        /// </summary>
        /// <returns>Returns <see cref="G313Dsp"/> block scanner context</returns>
        public G313BlockScanner BlockScanner()
        {
            return _scanner;
        }

        /// <summary>
        /// Gets DSP context of current receiver.
        /// </summary>
        /// <returns>Returns <see cref="G313Dsp"/> DSP context</returns>
        public G313Dsp Dsp()
        {
            return _dsp;
        }

        /// <summary>
        /// Retrieves receiver information.
        /// </summary>
        /// <returns>Returns <see cref="RadioInfo"/> retrieved information.</returns>
        public RadioInfo Info()
        {
            var info = new NativeDefinitions.RadioInfo2();
            var ptr = Marshal.AllocHGlobal((int)info.bLength);
            Marshal.StructureToPtr(info, ptr, true);

            G313Api.GetInfo(GetHandle(), ptr);

            Marshal.PtrToStructure(ptr, info);
            Marshal.FreeHGlobal(ptr);

            CachedInfo = new RadioInfo(info);
            return CachedInfo;
        }

        /// <summary>
        /// Checks the receiver whether it is ready to accept commands.
        /// </summary>
        /// <returns>Returns true if the device is busy.</returns>
        public bool Busy()
        {
            return G313Api.IsBusy(GetHandle());
        }

        /// <summary>
        /// Sets Radio panel LED patterns
        /// </summary>
        /// <param name="mask">Specified mask pattern</param>
        /// <returns>Returns true if the specified pattern is set.</returns>
        public bool TryLedPattern(byte mask)
        {
            return G313Api.SetLEDFlashPattern(GetHandle(), mask);
        }

        /// <summary>
        /// Retrieves the system pathname of the receiver.
        /// </summary>
        /// <returns>Returns path name.</returns>
        public string SystemPath()
        {
            var ptr = G313Api.GetPath2(GetHandle());
            if (ptr.ToInt32() == 0)
                throw new OperationFailedException("failed to retrieve system path", this);

            var path=Marshal.PtrToStringAuto(ptr);
            return path;
        }

        /// <summary>
        /// Determines device connection.
        /// </summary>
        /// <returns>Returns true if the device is connected.</returns>
        public bool Connected()
        {
            return G313Api.IsDeviceConnected(GetHandle());
        }

        /// <summary>
        /// Sets Radio panel LED patterns
        /// </summary>
        /// <param name="mask">Specified mask pattern</param>
        /// <returns>Returns <see cref="G313Radio"/> radio instance</returns>
        public G313Radio LedPattern(byte mask)
        {
            var result = TryLedPattern(mask);
            return CheckFluent(result, "failed to set LED pattern mask");
        }


        public override void Dispose()
        {
            var result = G313Api.CloseRadioDevice(GetHandle());
            if (!result)
                throw new InvalidOperationException("failed to close device");
        }
        
        internal G313Radio(IntPtr handle):this()
        {
            Handle = handle;

            _scanner = new G313BlockScanner(this);
            _dsp = new G313Dsp(this);
            _demodulator = new G313Demodulator(this);

            Info();        
            //InitializeDemodulator();
        }

        IRadioInfoProvider<G313Radio> IProviders<G313Radio>.InfoProvider()
        {
            return new G313RadioInfoProvider();
        }

        IRadioProvider<G313Radio> IProviders<G313Radio>.RadioProvider()
        {
            return new G313RadioProvider();
        }

        private G313Radio()
        {

        }
    }
}
