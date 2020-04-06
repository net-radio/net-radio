using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NetRadio.Devices.Exceptions;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents WinRadio G313 demodulator unit.
    /// </summary>
    public class G313Demodulator
    {
        private readonly NativeDefinitions.StreamCallback _ifCallback;
        private readonly NativeDefinitions.StreamCallback _audioCallback;

        /// <summary>
        /// Represents available modes of demodulation.
        /// </summary>
        public enum DemodulatorMode
        {
            /// <summary>
            /// continuous wave.
            /// </summary>
            Cw = G313Definitions.G313Mode.CW,
            /// <summary>
            /// Amplitude modulation.
            /// </summary>
            Am = G313Definitions.G313Mode.AM,
            /// <summary>
            /// Narrow band frequency modulation.
            /// </summary>
            Fmn = G313Definitions.G313Mode.FMN,
            /// <summary>
            /// Lower side band.
            /// </summary>
            Lsb = G313Definitions.G313Mode.LSB,
            /// <summary>
            /// Upper side band.
            /// </summary>
            Usb = G313Definitions.G313Mode.USB,
            /// <summary>
            /// Plural amplitude madulation.
            /// </summary>
            Ams = G313Definitions.G313Mode.AMS,
            /// <summary>
            /// Double side band.
            /// </summary>
            Dsb = G313Definitions.G313Mode.DSB,
            /// <summary>
            /// Independent side band.
            /// </summary>
            Isb = G313Definitions.G313Mode.ISB,
        }

        /// <summary>
        /// Represents available Independent side bands.
        /// </summary>
        public enum IsbAudioChannels:uint
        {
            /// <summary>
            /// Lower side band.
            /// </summary>
            Lsb=0,
            /// <summary>
            /// Upper side band.
            /// </summary>
            Usb=1
        }

        /// <summary>
        /// Occures when audio data is recieved.
        /// </summary>
        public event EventHandler<ChunkArgs> AudioChunkRecieved;

        /// <summary>
        /// Occures when IF data is recieved
        /// </summary>
        public event EventHandler<ChunkArgs> IfChunkRecieved;

        private readonly G313Radio _parent;

        private uint _audioSampleRate;
        private uint _ifSampleRate;

        private void CheckPassbandOffset(int offset)
        {
            if (offset < -8000 || offset > 8000)
                throw new ArgumentOutOfRangeException("offset", "passband offset should be between [-8000,8000] Hz");
        }
        private void CheckVolume(uint volume)
        {
            if (volume > 31)
                throw new ArgumentOutOfRangeException("volume", "volume range is [0,31]");
        }
        private void CheckIfShift(int shift)
        {
            if (shift < -7500 && shift > 7500)
                throw new ArgumentOutOfRangeException("shift", "shift values should be between [-7500,7500]");
        }

        private void CheckIfBandwidth(uint bandwidth)
        {
            if (bandwidth < 1 && bandwidth > 15000)
                throw new ArgumentOutOfRangeException("bandwidth", "IF bandwidth should be between [1,15000] Hz");
        }

        /// <summary>
        /// Checks fluent result of execution.
        /// </summary>
        /// <param name="result">Result of execution</param>
        /// <param name="message">Failure message of execution.</param>
        /// <returns>Return <see cref="G313Demodulator"/></returns>
        protected G313Demodulator CheckFluent(bool result, string message = null)
        {
            if (!result)
                throw new OperationFailedException(message, _parent);

            return this;
        }
        private int GetHandle()
        {
            return _parent.Handle.ToInt32();
        }

        /// <summary>
        /// Gets signal strength based on dBm.
        /// </summary>
        /// <returns>Returns value of signal strength</returns>
        public double SignalStrength()
        {
            return G313Api.GetSignalStrengthdBm(GetHandle())/10.0;
        }

        /// <summary>
        /// Returns the "raw" signal strength value. This is made available for compatibility with applications which expect the signal strength value to be from 0 (min signal level) to 255 (max signal level).
        /// </summary>
        /// <returns>Returns the "raw" signal strength value.</returns>
        public int RawSignalStrength()
        {
            return G313Api.GetRawSignalStrength(GetHandle());
        }

        /// <summary>
        /// Returns the last measured strength of the radio signal in dBm. Useful during block scanning.
        /// </summary>
        /// <returns>Returns the last measured strength.</returns>
        public double LastSignalStrength()
        {
            return G313Api.GetLastSSdBm(GetHandle())/10.0;
        }

        /// <summary>
        /// Returns last measured RAW signal strength value. Useful during block scanning.
        /// </summary>
        /// <returns>Returns last measured RAW signal.</returns>
        public int LastRawSignalStrength()
        {
            return G313Api.GetLastRawSS(GetHandle());
        }

        /// <summary>
        /// Function for configuring the IF notch filter. 
        /// </summary>
        /// <param name="notchFilter"><see cref="Devices.NotchFilter"/> filter parameters</param>
        /// <returns>Returns true if Notch filter parameters are set.</returns>
        public bool TryNotchFilter(NotchFilter notchFilter)
        {
            return G313DemodulatorApi.SetNotchFilter(GetHandle(), notchFilter.Active, notchFilter.Frequency, notchFilter.Bandwidth);
        }

        /// <summary>
        /// Function for configuring the IF notch filter. 
        /// </summary>
        /// <param name="notchFilter"><see cref="Devices.NotchFilter"/> Notch parameters</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator NotchFilter(NotchFilter notchFilter)
        {
            var result = TryNotchFilter(notchFilter);
            return CheckFluent(result, "failed to set notch filter");
        }

        /// <summary>
        /// Function for configuring the noise blanker
        /// </summary>
        /// <param name="noiseBlanker"><see cref="Devices.NoiseBlanker"/> Blanker parameters</param>
        /// <returns>Return true if Noise blanker parameters are set.</returns>
        public bool TryNoiseBlanker(NoiseBlanker noiseBlanker)
        {
            return G313DemodulatorApi.SetNoiseBlanker(GetHandle(), noiseBlanker.Active, (uint)noiseBlanker.Threshold);
        }

        /// <summary>
        /// Function for configuring the noise blanker
        /// </summary>
        /// <param name="noiseBlanker"><see cref="Devices.NoiseBlanker"/> Blanker parameters</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator NoiseBlanker(NoiseBlanker noiseBlanker)
        {
            var result = TryNoiseBlanker(noiseBlanker);
            return CheckFluent(result, "failed to set noise blanker");
        }

        /// <summary>
        /// Function for setting the IF shift. The specified value is added to the IF2 frequency and provides the actual receiving frequency. The resulting value must not exceed the IF hardware filter bandwidth (15kHz for G313), thus the accepted values are in the range -7500...+7500.
        /// </summary>
        /// <param name="shift">Shift value.</param>
        /// <returns>Returns true if If shift value is set.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if If shift value is out of range.</exception>
        public bool TryIfShift(int shift)
        {
            CheckIfShift(shift);
            return G313DemodulatorApi.SetIFShift(GetHandle(), shift);
        }

        /// <summary>
        /// Function for setting the IF shift. The specified value is added to the IF2 frequency and provides the actual receiving frequency. The resulting value must not exceed the IF hardware filter bandwidth (15kHz for G313), thus the accepted values are in the range -7500...+7500.
        /// </summary>
        /// <param name="shift">Shift value.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if If shift value is out of range.</exception>
        public G313Demodulator IfShift(int shift)
        {
            var result = TryIfShift(shift);
            return CheckFluent(result, "failed to sey IF shift");
        }

        /// <summary>
        /// Function for setting the IF bandwidth. Values in the 1Hz...15kHz range are accepted. Through this API call the filters that come after the I and Q multipliers are controlled.
        /// </summary>
        /// <param name="bandwidth">bandwidth value</param>
        /// <returns>Returns true if If bandwidth value is set.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if If bandwidth value is out of range.</exception>
        public bool TryIfBandwidth(uint bandwidth)
        {
            CheckIfBandwidth(bandwidth);
            return G313DemodulatorApi.SetIFBandwidth(GetHandle(), bandwidth);
        }

        /// <summary>
        /// Function for setting the IF bandwidth. Values in the 1Hz...15kHz range are accepted. Through this API call the filters that come after the I and Q multipliers are controlled.
        /// </summary>
        /// <param name="bandwidth">bandwidth value</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if If bandwidth value is out of range.</exception>
        public G313Demodulator IfBandwidth(uint bandwidth)
        {
            var result = TryIfBandwidth(bandwidth);
            return CheckFluent(result, "failed to set IF bandwidth");
        }

        /// <summary>
        /// Function for setting the pass-band offset value. Normally it is 0. Accepted values are from -8kHz...+8kHz. It controls the amount that the spectrum is shifted before actual demodulation.
        /// </summary>
        /// <param name="offset">Offset value</param>
        /// <returns>Return true if passband offset is set.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if Passband offset value is out of range.</exception>
        public bool TryPassbandOffset(int offset)
        {
            CheckPassbandOffset(offset);
            return G313DemodulatorApi.SetPassbandOffset(GetHandle(), offset);
        }

        /// <summary>
        /// Function for setting the pass-band offset value. Normally it is 0. Accepted values are from -8kHz...+8kHz. It controls the amount that the spectrum is shifted before actual demodulation.
        /// </summary>
        /// <param name="offset">Offset value</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if Passband offset value is out of range.</exception>
        public G313Demodulator PassbandOffset(int offset)
        {
            var result = TryPassbandOffset(offset);
            return CheckFluent(result, "failed to set passband offset");
        }

        /// <summary>
        /// Function for setting the desired AGC behaviour. The AGC is active for as long as the reference level is below 0. The attack and decay times are automatically set to 1 if the values passed to the API are 0.
        /// </summary>
        /// <param name="agc"><see cref="Devices.SoftwareAgc"/> Gain control parameters</param>
        /// <returns>Returns true if parametars are set.</returns>
        public bool TrySoftwareAgc(SoftwareAgc agc)
        {
            return G313DemodulatorApi.SetSoftAGC(GetHandle(), agc.ToNative());
        }

        /// <summary>
        /// Disables Software based automatic gain control.
        /// </summary>
        /// <returns>Returns true if the feature is disabled.</returns>
        public bool TryDisableSoftwareAgc()
        {
            return G313DemodulatorApi.SetSoftAGC(GetHandle(), new SoftwareAgc().ToNative());
        }

        /// <summary>
        /// Function for setting the desired AGC behaviour. The AGC is active for as long as the reference level is below 0. The attack and decay times are automatically set to 1 if the values passed to the API are 0.
        /// </summary>
        /// <param name="agc"><see cref="Devices.SoftwareAgc"/> Gain control parameters</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator SoftwareAgc(SoftwareAgc agc)
        {
            var result = TrySoftwareAgc(agc);
            return CheckFluent(result, "failed to set software agc");
        }

        /// <summary>
        /// Disables Software based automatic gain control.
        /// </summary>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator DisableSofwareAgc()
        {
            var result = TryDisableSoftwareAgc();
            return CheckFluent(result, "failed to disable software agc");
        }

        /// <summary>
        /// Function for selecting the desired demodulation mode. All demodulation modes available in the G313 demodulator can be selected here. The codes for the modes are the corresponding XRS codes RADIOMODE_xxx.
        /// </summary>
        /// <param name="mode"><see cref="DemodulatorMode"/> Specified mode.</param>
        /// <returns>Returns true if the specified mode is set.</returns>
        public bool TryMode(DemodulatorMode mode)
        {
            return G313DemodulatorApi.SetMode(GetHandle(), (G313Definitions.G313Mode)mode);
        }

        /// <summary>
        /// Function for selecting the desired demodulation mode. All demodulation modes available in the G313 demodulator can be selected here. The codes for the modes are the corresponding XRS codes RADIOMODE_xxx.
        /// </summary>
        /// <param name="mode"><see cref="DemodulatorMode"/> Specified mode.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator Mode(DemodulatorMode mode)
        {
            var result = TryMode(mode);
            return CheckFluent(result, "failed to set demodulator mode");
        }

        /// <summary>
        /// Function for setting the final audio bandwidth. For linear modulation types it should be equal to IF bandwidth (LSB, USB) or half the IF bandwidth (AM, AMS, DSB, ISB). For exponential modulations (FM) it is transmission dependant. For CW there is a special condition as the audio bandwidth should be the minimum between half IF bandwidth and the CW tone frequency.
        /// </summary>
        /// <param name="bandwidth">Specified audio bandwidth.</param>
        /// <returns>Returns true if the Specified audio bandwidth is set.</returns>
        public bool TryAudioBandwidth(uint bandwidth)
        {
            return G313DemodulatorApi.SetAudioBandwidth(GetHandle(), bandwidth);
        }

        /// <summary>
        /// Function for setting the final audio bandwidth. For linear modulation types it should be equal to IF bandwidth (LSB, USB) or half the IF bandwidth (AM, AMS, DSB, ISB). For exponential modulations (FM) it is transmission dependant. For CW there is a special condition as the audio bandwidth should be the minimum between half IF bandwidth and the CW tone frequency.
        /// </summary>
        /// <param name="bandwidth">Specified audio bandwidth.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator AudioBandwidth(uint bandwidth)
        {
            var result = TryAudioBandwidth(bandwidth);
            return CheckFluent(result, "failed to set audio bandwidth");
        }

        /// <summary>
        /// Function for setting the fixed audio gain. This value is used to provide a fixed audio amplification when the software AGC is disabled.
        /// </summary>
        /// <param name="gain">Specified audio gain.</param>
        /// <returns>Returns true if the specified audio gain is set.</returns>
        public bool TryAudioGain(uint gain)
        {
            return G313DemodulatorApi.SetAudioGain(GetHandle(), gain);
        }

        /// <summary>
        /// Function for setting the fixed audio gain. This value is used to provide a fixed audio amplification when the software AGC is disabled.
        /// </summary>
        /// <param name="gain">Specified audio gain.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator AudioGain(uint gain)
        {
            var result = TryAudioGain(gain);
            return CheckFluent(result, "failed to set audio gain");
        }

        /// <summary>
        /// Function for setting the audition volume. It can be any value between 0 and 31.
        /// </summary>
        /// <param name="volume">Specified audition volume.</param>
        /// <returns>Returns true if the specified audition volume is set.</returns>
        public bool TryVolume(uint volume)
        {
            CheckVolume(volume);
            return G313DemodulatorApi.SetVolume(GetHandle(), volume);
        }

        /// <summary>
        /// Function for setting the audition volume. It can be any value between 0 and 31.
        /// </summary>
        /// <param name="volume">Specified audition volume.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator Volume(uint volume)
        {
            var result = TryVolume(volume);
            return CheckFluent(result, "failed to set volume");
        }

        /// <summary>
        /// Function for setting the AF squelch while demodulating FM transmissions. The level corresponds to the minimum noise level that will mute the audio output. By specifying a value of 0 the AF squelch is disabled.
        /// </summary>
        /// <param name="level">Specified squelch level.</param>
        /// <returns>Returns true if the specified squelch level is set.</returns>
        public bool TryAfSquelchLevel(uint level)
        {
            return G313DemodulatorApi.SetFMAFSquelchLevel(GetHandle(), level);
        }

        /// <summary>
        /// Function for setting the AF squelch while demodulating FM transmissions. The level corresponds to the minimum noise level that will mute the audio output. By specifying a value of 0 the AF squelch is disabled.
        /// </summary>
        /// <param name="level">Specified squelch level.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator AfSquelchLevel(uint level)
        {
            var result = TryAfSquelchLevel(level);
            return CheckFluent(result, "failed to set FM AF squelch");
        }

        /// <summary>
        /// Disables AF squelch used during FM demodulation.
        /// </summary>
        /// <returns>Returns true if the feature is disabled.</returns>
        public bool TryDisableAfSquelch()
        {
            return TryAfSquelchLevel(0);
        }

        /// <summary>
        /// Disables AF squelch used during FM demodulation.
        /// </summary>
        /// <returns>Returns true if the feature is disabled.</returns>
        public G313Demodulator DisableAfSquelch()
        {
            var result = TryDisableAfSquelch();
            return CheckFluent(result, "failed to disable FM AF squelch");
        }

        /// <summary>
        ///Function for specifying the audio channel that should be sent to the audio output when demodulating ISB transmissions. 0 stands for Left (LSB) and 1 for Right (USB).
        /// </summary>
        /// <param name="channel"><see cref="IsbAudioChannels"/> Specified audio channel.</param>
        /// <returns>Returns true if the specified audio channel is set.</returns>
        public bool TryIsbAudioChannel(IsbAudioChannels channel)
        {
            return G313DemodulatorApi.SetISBAudioChannel(GetHandle(), (uint)channel);
        }

        /// <summary>
        ///Function for specifying the audio channel that should be sent to the audio output when demodulating ISB transmissions. 0 stands for Left (LSB) and 1 for Right (USB).
        /// </summary>
        /// <param name="channel"><see cref="IsbAudioChannels"/> Specified audio channel.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator IsbAudioChannel(IsbAudioChannels channel)
        {
            var result = TryIsbAudioChannel(channel);
            return CheckFluent(result, "failed to set ISB channel");
        }

        /// <summary>
        /// Function for setting the frequency of the audible tone when receving CW transmissions.
        /// </summary>
        /// <param name="frequency">Specified tone frequency.</param>
        /// <returns>Returns true if the specified value is set.</returns>
        public bool TryCwTone(uint frequency)
        {
            return G313DemodulatorApi.SetCWTone(GetHandle(), frequency);
        }

        /// <summary>
        /// Function for setting the frequency of the audible tone when receving CW transmissions.
        /// </summary>
        /// <param name="frequency">Specified tone frequency.</param>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator CwTone(uint frequency)
        {
            var result = TryCwTone(frequency);
            return CheckFluent(result, "failed to set CW tone");
        }

        /// <summary>
        /// Function for retrieving the IF notch filter settings.
        /// </summary>
        /// <returns>Returns <see cref="Devices.NotchFilter"/> settings.</returns>
        public NotchFilter NotchFilter()
        {
            uint bandwidth=default(uint);
            int frequency=default(int);
            var active = G313DemodulatorApi.GetNotchFilter(GetHandle(), ref frequency, ref bandwidth);

            return new NotchFilter { Bandwidth = bandwidth, Frequency = frequency ,Active=active};
        }

        /// <summary>
        /// Function for retrieving the Noise blanker settings.
        /// </summary>
        /// <returns>Returns <see cref="Devices.NoiseBlanker"/> settings.</returns>
        public NoiseBlanker NoiseBlanker()
        {
            uint threshold = default(uint);
            var active = G313DemodulatorApi.GetNoiseBlanker(GetHandle(), ref threshold);

            return new NoiseBlanker { Active = active, Threshold = threshold };
        }

        /// <summary>
        /// Function for retrieving the IF shift value.
        /// </summary>
        /// <returns>Returns If shift value.</returns>
        public int IfShift()
        {
            return G313DemodulatorApi.GetIFShift(GetHandle());
        }

        /// <summary>
        /// Function for retrieving the IF bandwidth value.
        /// </summary>
        /// <returns>Returns If bandwidth value.</returns>
        public uint IfBandwidth()
        {
            return G313DemodulatorApi.GetIFBandwidth(GetHandle());
        }

        /// <summary>
        /// Function for retrieving the Passband offset value.
        /// </summary>
        /// <returns>Returns passband offset value.</returns>
        public int PassbandOffset()
        {
            return G313DemodulatorApi.GetPassbandOffset(GetHandle());
        }

        /// <summary>
        /// Function for retrieving the software Automatic gain control settings.
        /// </summary>
        /// <returns>Returns <see cref="Devices.SoftwareAgc"/> settings.</returns>
        public SoftwareAgc SoftwareAgc()
        {
            var agc=new NativeDefinitions.SoftAgcData();
            var result = G313DemodulatorApi.GetSoftAGC(GetHandle(), ref agc);

            if (!result)
                throw new OperationFailedException("failed to retrieve software Agc", _parent);

            return new SoftwareAgc().FillManged(agc);
        }

        /// <summary>
        /// Function for getting the current demodulation mode.
        /// </summary>
        /// <returns>Returns <see cref="DemodulatorMode"/> current mode.</returns>
        public DemodulatorMode Mode()
        {
            return (DemodulatorMode)G313DemodulatorApi.GetMode(GetHandle());
        }

        /// <summary>
        /// Function for retrieving the audio bandwidth value.
        /// </summary>
        /// <returns>Returns audio bandwidth value</returns>
        public uint AudioBandwidth()
        {
            return G313DemodulatorApi.GetAudioBandwidth(GetHandle());
        }

        /// <summary>
        /// Function for retrieving the audio gain value.
        /// </summary>
        /// <returns>Returns audio gain value</returns>
        public uint AudioGain()
        {
            return G313DemodulatorApi.GetAudioGain(GetHandle());
        }

        /// <summary>
        /// Function for retrieving the audition volume value.
        /// </summary>
        /// <returns>Returns audition volume value</returns>
        public uint Volume()
        {
            return G313DemodulatorApi.GetVolume(GetHandle());
        }

        /// <summary>
        /// Function for getting the current FM AF squelch level.
        /// </summary>
        /// <returns>Returns current squlech level</returns>
        public uint AfSquelchLevel()
        {
            return G313DemodulatorApi.GetFMAFSquelchLevel(GetHandle());
        }

        /// <summary>
        /// Function for getting the current ISB audio channel.
        /// </summary>
        /// <returns>Returns current channel.</returns>
        public IsbAudioChannels IsbAudioChannel()
        {
            return (IsbAudioChannels)G313DemodulatorApi.GetISBAudioChannel(GetHandle());
        }

        /// <summary>
        /// Function for getting the current CW tone frequency.
        /// </summary>
        /// <returns>Returns current frequency.</returns>
        public uint CwTone()
        {
            return G313DemodulatorApi.GetCWTone(GetHandle());
        }

        /// <summary>
        /// Function for getting the tuning error after the demodulator has been initialized. The resulting value is valid only if the receiver is tuned to a transmissions. The returned value must be substracted from the frequency to which the receiver is currently tuned.
        /// </summary>
        /// <returns>Returns tune error value.</returns>
        public int TuneError()
        {
            return G313DemodulatorApi.GetTuneError(GetHandle());
        }

        /// <summary>
        /// Function for getting the frequency deviation (in Hz) of the currently received transmission.
        /// </summary>
        /// <returns>Returns frequency deviations.</returns>
        public uint FrequencyDeviations()
        {
            return G313DemodulatorApi.GetFrequencyDeviation(GetHandle());
        }

        /// <summary>
        /// Function for getting the AM depth for the current transmission. The 0..1 range normal for this parameter is returned scaled to 0..1000.
        /// </summary>
        /// <returns>returns depth value.</returns>
        public uint AmDepth()
        {
            return G313DemodulatorApi.GetAMDepth(GetHandle());
        }

        private void AudioCallback(IntPtr target, IntPtr buffer, uint bufferSize, uint samplingRate)
        {
            var rawData = CopyAray(buffer, bufferSize);
            OnAudioChunkRecieved(samplingRate, rawData);
        }

        private void IfCallback(IntPtr target, IntPtr buffer, uint bufferSize, uint samplingRate)
        {
            var rawData = CopyAray(buffer, bufferSize);
            OnIfChunkRecieved(samplingRate, rawData);
        }

        private static byte[] CopyAray(IntPtr buffer, uint bufferSize)
        {
            var rawData = new byte[bufferSize];
            Marshal.Copy(buffer, rawData, 0, (int)bufferSize);
            return rawData;
        }

        /// <summary>
        /// Raises <see cref="IfChunkRecieved"/>
        /// </summary>
        /// <param name="samplingRate">Sampling rate of data.</param>
        /// <param name="rawData">Raw data represtation.</param>
        protected void OnIfChunkRecieved(uint samplingRate, byte[] rawData)
        {
            _ifSampleRate = samplingRate;

            if (IfChunkRecieved == null)
                return;

            IfChunkRecieved(_parent, new ChunkArgs(rawData, samplingRate));
        }

        /// <summary>
        /// Raises <see cref="AudioChunkRecieved"/>
        /// </summary>
        /// <param name="samplingRate">Sampling rate of data.</param>
        /// <param name="rawData">Raw data represtation.</param>
        protected void OnAudioChunkRecieved(uint samplingRate, byte[] rawData)
        {
            try
            {
                _audioSampleRate = samplingRate;

                if (AudioChunkRecieved == null)
                    return;

                AudioChunkRecieved(_parent, new ChunkArgs(rawData, samplingRate));
            }
            catch (Exception)
            {
                Debug.Print("Pressure on data provider, so many pause and replay!");
            }
        }

        /// <summary>
        /// Function for activating the IF and audio streams while the demodulator is active.
        /// </summary>
        /// <returns>Returns <see cref="G313Demodulator"/></returns>
        public G313Demodulator SetupStreams()
        {
            var result = G313DemodulatorApi.SetupStreams(GetHandle(), _ifCallback, IntPtr.Zero, _audioCallback, IntPtr.Zero);
            if (!result)
                throw new OperationFailedException("failed to setup audio streams", _parent);

            return this;
        }

        /// <summary>
        /// Get current audio sampling rate.
        /// </summary>
        /// <returns>Returns sampleing rate.</returns>
        public uint AudioSamplingRate()
        {
            return _audioSampleRate;
        }

        /// <summary>
        /// Get current IF sampling rate.
        /// </summary>
        /// <returns>Returns sampleing rate.</returns>
        public uint IfSamplingRate()
        {
            return _ifSampleRate;
        }

        /// <summary>
        /// Gets Radio context
        /// </summary>
        /// <returns>Returns <see cref="G313Radio"/> instance.</returns>
        public G313Radio Radio()
        {
            return _parent;
        }

        internal G313Demodulator(G313Radio radio)
        {
            _parent = radio;
            _ifCallback = IfCallback;
            _audioCallback = AudioCallback;
        }
    }
}
