using System;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G313
{
    internal class G313DemodulatorApi
    {
        private const string DLL = G313Definitions.DLL;

        /// <summary>
        /// Function for configuring the IF notch filter. The frequency is specified relatively to the IF frequency and is limited to the IF hardware filter bandwidth, meaning from -7500 to +7500.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="active">TRUE if the notch filter should be active and FALSE if not</param>
        /// <param name="freq">The frequency offset of the notch filter relative to the center of the crystal IF bandwidth filter</param>
        /// <param name="bw">The bandwidth of the notch filter</param>
        /// <returns>The return value is TRUE if the notch filter can be configured with the given parameters and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetNotchFilter(int radioHandle, bool active, int freq, UInt32 bw);

        /// <summary>
        /// Function for configuring the noise blanker. The treshold is given as percentage of the maximum acceptable input signal.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="active">TRUE if the noise blanker should be active and FALSE if not</param>
        /// <param name="thres">Noise blanking threshold level given as percent of the whole signal range (0..100)</param>
        /// <returns>The return value is TRUE if the noise blanker can be configured with the given parameters and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetNoiseBlanker(int radioHandle, bool active, UInt32 thres);

        /// <summary>
        /// Function for setting the IF shift. The specified value is added to the IF2 frequency and provides the actual receiving frequency. The resulting value must not exceed the IF hardware filter bandwidth (15kHz for G313), thus the accepted values are in the range -7500...+7500.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="shift">IF shift value relative to the IF crystal filter center</param>
        /// <returns>The return value is TRUE if the IF shift can be configured with the given parameter and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetIFShift(int radioHandle, int shift);

        /// <summary>
        /// Function for setting the IF bandwidth. Values in the 1Hz...15kHz range are accepted. Through this API call the filters that come after the I and Q multipliers are controlled.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="bw">The IF bandwidth value</param>
        /// <returns>The return value is TRUE if the IF bandwidth can be configured with the given parameter and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetIFBandwidth(int radioHandle, UInt32 bw);

        /// <summary>
        /// Function for setting the pass-band offset value. Normally it is 0. Accepted values are from -8kHz...+8kHz. It controls the amount that the spectrum is shifted before actual demodulation.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="pbo">The pass-band offset value to be set</param>
        /// <returns>The return value is TRUE if the pass-band offset can be configured with the given parameter and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetPassbandOffset(int radioHandle, int pbo);

        /// <summary>
        /// Function for setting the desired AGC behaviour. The AGC is active for as long as the reference level is below 0. The attack and decay times are automatically set to 1 if the values passed to the API are 0.
        /// </summary>
        /// <remarks>
        /// Reference level (iRefLevel in SOFTAGC_DATA) should be below -3 and it is given in dB. Normal value is -6...-8 for modes with carrier (AM, AMS, FM, CW) and -15 for modes with no carrier.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="data">Pointer to a SOFTAGC_DATA structure containing the AGC configuration values</param>
        /// <returns>The return value is TRUE if the software AGC can be configured with the given parameters and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetSoftAGC(int radioHandle, NativeDefinitions.SoftAgcData data);

        /// <summary>
        /// Function for selecting the desired demodulation mode. All demodulation modes available in the G313 demodulator can be selected here. The codes for the modes are the corresponding XRS codes - RADIOMODE_xxx.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="mode">The code for the demodulation mode</param>
        /// <returns>The return value is TRUE if the demodulation mode can be set and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetMode(int radioHandle, G313Definitions.G313Mode mode);

        /// <summary>
        /// Function for setting the final audio bandwidth. For linear modulation types it should be equal to IF bandwidth (LSB, USB) or half the IF bandwidth (AM, AMS, DSB, ISB). For exponential modulations (FM) it is transmission dependant. For CW there is a special condition as the audio bandwidth should be the minimum between half IF bandwidth and the CW tone frequency.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="bw">The audio bandwidth in Hz</param>
        /// <returns>The return value is TRUE if the audio bandwidth can be set and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetAudioBandwidth(int radioHandle,UInt32 bw);

        /// <summary>
        /// Function for setting the fixed audio gain. This value is used to provide a fixed audio amplification when the software AGC is disabled.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="gain">The fixed audio gain</param>
        /// <returns>The return value is TRUE if the fixed audio gain can be set and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetAudioGain(int radioHandle, UInt32 gain);

        /// <summary>
        /// Function for setting the audition volume. It can be any value between 0 and 31.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="volume">The audition volume</param>
        /// <returns>The return value is TRUE if the audition volume can be set and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetVolume(int radioHandle, UInt32 volume);

        /// <summary>
        /// Function for setting the AF squelch while demodulating FM transmissions. The level corresponds to the minimum noise level that will mute the audio output. By specifying a value of 0 the AF squelch is disabled.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="level">The AF squelch level</param>
        /// <returns>The return value is TRUE if the FM AF squelch level can be set and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetFMAFSquelchLevel(int radioHandle, UInt32 level);

        /// <summary>
        /// Function for specifying the audio channel that should be sent to the audio output when demodulating ISB transmissions. 0 stands for Left (LSB) and 1 for Right (USB).
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="channel">The channel that is to be sent to the audio output</param>
        /// <returns>The return value is TRUE if the ISB audio channel can be selected and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetISBAudioChannel(int radioHandle, UInt32 channel);

        /// <summary>
        /// Function for setting the frequency of the audible tone when receving CW transmissions.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="freq">The CW tone frequency</param>
        /// <returns>The return value is TRUE if the CW tone frequency can be set and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetCWTone(int radioHandle, UInt32 freq);

        /// <summary>
        /// Function for retrieving the IF notch filter settings.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="freq">Pointer to the location where the current central frequency of the notch filter should be stored</param>
        /// <param name="bw">Pointer to the location where the current bandwidth of the notch filter should be stored</param>
        /// <returns>The return value is TRUE if the notch filter is active and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool  GetNotchFilter(int radioHandle,ref int freq,ref UInt32 bw);

        /// <summary>
        /// Function for retrieving the noise blanker settings.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="thres">Pointer to the location where the current threshold level of the noise blanker should be stored</param>
        /// <returns>The return value is TRUE if the noise blanker is active and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool GetNoiseBlanker(int radioHandle, ref UInt32 thres);

        /// <summary>
        /// Function for retrieving the IF shift value.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the IF shift value in Hz, relative to the center of the crystal IF filter.</returns>
        [DllImport(DLL)]
        public static extern int GetIFShift(int radioHandle);

        /// <summary>
        /// Function for retrieving the IF bandwidth value.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the IF bandwidth in Hz.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetIFBandwidth(int radioHandle);

        /// <summary>
        /// Function for retrieving the pass-band offset value.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the pass-band offset in Hz.</returns>
        [DllImport(DLL)]
        public static extern int GetPassbandOffset(int radioHandle);

        /// <summary>
        /// Function for retrieving the software AGC parameters.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="data">Pointer to a SOFTAGC_DATA structure in which the AGC configuration should be stored</param>
        /// <returns>The return value is TRUE if the software AGC configuration could be retrieved and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool GetSoftAGC(int radioHandle, ref NativeDefinitions.SoftAgcData data);

        /// <summary>
        /// Function for getting the current demodulation mode.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the code of the current demodulation mode.</returns>
        [DllImport(DLL)]
        public static extern G313Definitions.G313Mode GetMode(int radioHandle);

        /// <summary>
        /// Function for getting the current audio bandwidth.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the current audio bandwidth in Hz.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetAudioBandwidth(int radioHandle);

        /// <summary>
        /// Function for getting the current fixed audio gain.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the current fixed audio gain.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetAudioGain(int radioHandle);

        /// <summary>
        /// Function for getting the current audition volume.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the current audition volume.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetVolume(int radioHandle);

        /// <summary>
        /// Function for getting the current FM AF squelch level.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the current FM AF squelch level.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetFMAFSquelchLevel(int radioHandle);

        /// <summary>
        /// Function for getting the currently selected ISB audio channel.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the currently selected ISB audio channel.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetISBAudioChannel(int radioHandle);

        /// <summary>
        ///Function for getting the CW tone frequency. 
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the current CW tone frequency.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetCWTone(int radioHandle);

        /// <summary>
        /// Function for getting the tuning error after the demodulator has been initialized. The resulting value is valid only if the receiver is tuned to a transmissions. The returned value must be substracted from the frequency to which the receiver is currently tuned.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the current tuning frequency error.</returns>
        [DllImport(DLL)]
        public static extern int GetTuneError(int radioHandle);

        /// <summary>
        /// Function for getting the frequency deviation (in Hz) of the currently received transmission.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the frequency deviation of the transmission currently demodulated.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetFrequencyDeviation(int radioHandle);

        /// <summary>
        /// Function for getting the AM depth for the current transmission. The 0..1 range normal for this parameter is returned scaled to 0..1000.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The return value is the current tuning frequency error.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetAMDepth(int radioHandle);

        /// <summary>
        /// Function for activating the IF and/or audio streams callback functions while the demodulator is active.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="ifh">Application provided function to be called by the API when there is new data received through the IF stream from the receiver; if NULL IF streaming is nolonger sent to the application</param>
        /// <param name="ift">Argument to be passed to the IFH application defined callback function</param>
        /// <param name="audioH">Application provided function to be called by the API when there is new data received through the audio stream from the receiver; if NULL audio streaming is nolonger sent to the application</param>
        /// <param name="audioT">Argument to be passed to the AudioH application defined callback function</param>
        /// <returns>The return value is TRUE if the streams callback functions could be properly set and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool SetupStreams(int radioHandle, NativeDefinitions.StreamCallback ifh, IntPtr ift, NativeDefinitions.StreamCallback audioH, IntPtr audioT);
    }
}
