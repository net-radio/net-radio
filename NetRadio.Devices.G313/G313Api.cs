using System;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G313
{
    internal class G313Api
    {
        private const string DLL = G313Definitions.DLL;

        /// <summary>
        /// Opens a radio device by its serial number to allow you to control the device using the other API functions.
        /// </summary>
        /// <remarks>
        /// If you would like to have more than one application use the same device, you will have to close the device when you have finished using it to allow other applications to access the device. For Win32 applications, multiple threads can access the same device at any time.
        /// </remarks>
        /// <param name="id">Radio device serial number that you would like to open. If NULL is specified, a demo receiver will be opened. The serial number is a null-terminated string.</param>
        /// <returns>Returns a handle to the device that has to be used to call all the other API functions (hRadio). If zero is returned, the specified device doesn't exist or couldn't be opened.</returns>
        [DllImport(DLL)]
        public static extern int Open(string id);

        /// <summary>
        /// Opens a radio device by its index to allow you to control the device using the other API functions.
        /// </summary>
        /// <remarks>
        /// If you would like to have more than one application use the same device, you will have to close the device when you have finished using it to allow other applications to access the device. For Win32 applications, multiple threads can access the same device at any time.
        /// </remarks>
        /// <param name="deviceNumber">
        /// Radio device number that you would like to open from 1 to 255. If zero is specified, it will open the next available device.
        /// </param>
        /// <returns>
        /// Returns a handle to a device that has to be used to call all the other API functions (hRadio). If zero is returned, no devices were available to open or the specified device doesn't exist.</returns>
        [DllImport(DLL)]
        public static extern int OpenRadioDevice(int deviceNumber);

        /// <summary>
        /// Closes a radio device that was previously opened, and allows another program to access the radio device.
        /// </summary>
        /// <param name="radioHandle">Handle to the radio device to close, returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is TRUE, otherwise the return value is FALSE (invalid handle, or has already been closed).</returns>
        [DllImport(DLL)]
        public static extern bool CloseRadioDevice(int radioHandle);

        /// <summary>
        /// The G3GetRadioList2 function returns information about all G313 devices that can be opened. It is a more flexible implementation than in G3GetRadioList allowing future additions to the RADIO_INFO structure while the applications would not need updating.
        /// </summary>
        /// <param name="buffer">The buffer for an array of RADIO_INFO structures. It will contain a RADIO_INFO structure for each unalocated G313 device present in the system.</param>
        /// <param name="bufferSize">The size of the lpRadioInfo buffer. The function doesn't write all structures if the buffer is too small.</param>
        /// <returns>The count of unallocated G313 devices present in the system. This value can be greater than the real number of OLD_RADIO_INFO structures written to the buffer if the buffer is too small.</returns>
        [Obsolete("This function is deprecated. Please consider using the new GetRadioList function instead.")]
        [DllImport(DLL)]
        public static extern uint G3GetRadioList(IntPtr buffer, int bufferSize);

        /// <summary>
        /// The G3GetRadioList2 function returns information about all G313 devices that can be opened. It is a more flexible implementation than in G3GetRadioList allowing future additions to the RADIO_INFO structure while the applications would not need updating.
        /// </summary>
        /// <param name="buffer">The buffer for an array of RADIO_INFO structures. It will contain a RADIO_INFO structure for each unalocated G313 device present in the system.</param>
        /// <param name="bufferSize">The size of the lpRadioInfo buffer. The function doesn't write all structures if the buffer is too small.</param>
        /// <param name="infoSize">Pointer to an integer value that will be written by G3GetRadioList2 with the size of the structure for each available receiver.</param>
        /// <returns>The count of unallocated G313 devices present in the system. This value can be greater than the real number of RADIO_INFO structures written to the buffer if the buffer is too small.</returns>
        [Obsolete("This function is deprecated. Please consider using the new GetRadioList function instead.")]
        [DllImport(DLL)]
        public static extern uint G3GetRadioList2(IntPtr buffer, int bufferSize,ref int infoSize);

        /// <summary>
        /// The GetRadioList function returns information about all G313 devices that can be opened. It is a more flexible implementation than G3GetRadioList and G3GetRadioList2 allowing future additions to the RADIO_INFO2 structure while the applications would not need updating. The returned structure content is a lot cleaner than with the older functions and it also includes clean support for 64-bit code.
        /// </summary>
        /// <param name="info">The buffer for an array of RADIO_INFO2 structures. It will contain a RADIO_INFO2 structure for each unalocated G313 device present in the system.</param>
        /// <param name="bufferSize">The size of the lpRadioInfo buffer. The function doesn't write all structures if the buffer is too small.</param>
        /// <param name="infoSize">Pointer to an integer value that will be written by GetRadioList with the size of the structure for each available receiver.</param>
        /// <returns>The count of unallocated G313 devices present in the system. This value can be greater than the real number of RADIO_INFO2 structures written to the buffer if the buffer is too small.</returns>
        [DllImport(DLL)]
        public static extern int GetRadioList(IntPtr info, int bufferSize, ref int infoSize);

        /// <summary>
        /// The DSP must be initialized before any signal processing, including signal strength measurement, can commence. When initializing the demodulator the full path to a valid calibration data file may be provided if signal strength measuremen must be calibrated. If not so, the passed pointer should be NULL.
        /// </summary>
        /// <remarks>
        /// Once the demodulator is initialized, all handles to the receiver must be closed in order to allow a full re-initialization.
        /// </remarks>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="calFilePath">Pointer to the file containing the calibration data for the receiver in use</param>
        /// <returns>The returned value is TRUE if the demodulator could be initialized and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool InitializeDemodulator(int hRadio, string calFilePath);

        /// <summary>
        /// The GetSignalStrengthdBm function returns the strength of the radio signal in dBm received by the radio device.
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The aproximated value of the signal strength in dBm * 10 (i.e. the value is given in tenths of dBm). If the demodulator is properly initialized and a calibration file is provided, the resulting signal strength is calibrated.</returns>
        [DllImport(DLL)]
        public static extern int GetSignalStrengthdBm(int hRadio);

        /// <summary>
        /// The GetRawSignalStrength function returns the "raw" signal strength value. This is made available for compatibility with applications which expect the signal strength value to be from 0 (min signal level) to 255 (max signal level).
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>RAW signal strength in the interval 0,255.</returns>
        [DllImport(DLL)]
        public static extern int GetRawSignalStrength(int hRadio);

        /// <summary>
        /// The GetLastSSdBm function returns the last measured strength of the radio signal in dBm. Useful during block scanning.
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The aproximated value of the signal strength in dBm * 10 (i.e. the value is given in tenths of dBm).</returns>
        [DllImport(DLL)]
        public static extern int GetLastSSdBm(int hRadio);

        /// <summary>
        /// The GetLastRawSS function returns last measured RAW signal strength value. Useful during block scanning.
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>RAW signal strength 0,255.</returns>
        [DllImport(DLL)]
        public static extern int GetLastRawSS(int hRadio);

        /// <summary>
        /// The GetInternalRSSI function returns a combination of the RSSI and AGC values read from the receiver hardware.
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>AGC value in the upper 16 bits of the result and RSSI value in the lower 16 bits of the result.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetInternalRSSI(int hRadio);

        /// <summary>
        /// The SetFrequency function sets the frequency the device is to be tuned to.
        /// </summary>
        /// <remarks>
        /// The lower and upper limits are specified in the dwMinFreq and dwMaxFreq fields respectively in the RADIO_INFO structure that can be retrieved with the GetInfo or G3GetRadioList function.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="frequency">Specifies the frequency to tune to receiver to.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetFrequency(int radioHandle, UInt32 frequency);

        /// <summary>
        /// The SetFreqAsync function starts the sequence of hardware commands that sets the frequency the device is to be tuned to. In order to insure the sequence has been executed and that the hardware is properly tuned, WaitFreqAsync function must be called.
        /// </summary>
        /// <remarks>
        /// The lower and upper limits are specified in the dwMinFreq and dwMaxFreq fields respectively in the RADIO_INFO structure that can be retrieved with the GetInfo or G3GetRadioList function.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="frequency">Specifies the frequency to tune to receiver to.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetFreqAsync(int radioHandle, UInt32 frequency);

        /// <summary>
        /// The WaitFreqAsync function waits for the end of a receiver tunning started by a SetFreqAsync call.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool WaitFreqAsync(int radioHandle);

        /// <summary>
        /// The SetIF2Frequency function sets the center frequency of the last IF signal.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="iFrequency">Specifies the IF2 frequency. It can be either positive or negative, negative values causing an IF2 spectrum inversion</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetIF2Frequency(int radioHandle,int iFrequency);

        /// <summary>
        /// The SetAtten function activates or deactivates the RF input attenuator. It is used to prevent overloading of the receiver with strong signals.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="atten">If TRUE, the RF attenuator is on, otherwise if FALSE, the RF attenuator is off (more sensitive).</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetAtten(int radioHandle, bool atten);

        /// <summary>
        /// The SetPower function switches the device's power on or off. This function can be used to power down the receiver in portable applications to conserve battery power.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="power">If TRUE, the radio's power is on, otherwise if FALSE, the radio's power is off.</param>
        /// <returns>If the function succeeds, the return value is TRUE. If the function fails, the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetPower(int radioHandle, bool power);

        /// <summary>
        /// The SetAGC function sets the AGC value for given receiver.
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="agc">AGC value 0,3 - off, slow, medium or fast.</param>
        /// <returns>If the function succeeds, the return value is TRUE, else the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetAGC(int hRadio, int agc);

        /// <summary>
        /// Sets IFGain value for the specified receiver.
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="ifGain">IFGain value 0,100 to be set.</param>
        /// <returns>If the function succeeds, the return value is TRUE, else the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetIFGain(int hRadio, int ifGain);

        /// <summary>
        /// Specifies the reference clock frequency and allows switching between internal and external references.
        /// </summary>
        /// <param name="hRadio">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="refClock">The frequency of the external reference clock. If 0 is specified the internal reference clock is used.</param>
        /// <returns>If the function succeeds, the return value is TRUE, else the return value is FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool SetRefClock(int hRadio, UInt32 refClock);

        /// <summary>
        /// The GetFrequency function retrieves the frequency the receiver is tuned to.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The current receiver frequency in Hz. If the handle is invalid, 0 is returned instead.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetFrequency(int radioHandle);

        /// <summary>
        /// The GetIF2Frequency function retrieves the current IF2 frequency.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The current IF2 frequency in Hz. If the handle is invalid, 0 is returned instead.</returns>
        [DllImport(DLL)]
        public static extern int GetIF2Frequency(int radioHandle);

        /// <summary>
        /// The GetAtten function returns the RF input attenuator setting.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>GetAtten returns TRUE if the RF attenuator is on, otherwise it returns FALSE if it is off.</returns>
        [DllImport(DLL)]
        public static extern bool GetAtten(int radioHandle);

        /// <summary>
        /// The GetPower function returns whether the receiver's power is on or off.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>Returns TRUE if the receiver's power is on, otherwise it returns FALSE if it is off.</returns>
        [DllImport(DLL)]
        public static extern bool GetPower(int radioHandle);

        /// <summary>
        /// The GetAGC function returns current AGC value of radio device.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>The current AGC value if successful, otherwise -1 is returned.</returns>
        [DllImport(DLL)]
        public static extern int GetAGC(int radioHandle);

        /// <summary>
        /// Retrieves the IFGain value of the receiver.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is the IFGain value. Otherwise -1 is returned.</returns>
        [DllImport(DLL)]
        public static extern int GetIFGain(int radioHandle);

        /// <summary>
        /// Retrieves the PLLs lock status.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="getType">This argument is reserved for future extensions.</param>
        /// <returns>If the function succeeds, the return value is a combination of the PLLs lock bits. Otherwise 0 is returned.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetLock(int radioHandle, int getType);

        /// <summary>
        /// Retrieves the current reference clock frequency.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is current reference clock frequency, else the return value is 0xFFFFFFFF.</returns>
        [DllImport(DLL)]
        public static extern UInt32 GetRefClock(int radioHandle);

        /// <summary>
        /// Scanned RAW signal strength values are written back to the Freqs buffer. Each frequency in the Freqs buffer is replaced by appropriate RAW value. Parts of this buffer are sent to the appplication after Feedback time interval expires. The block scanning automatically ends when scanned RAW signal strength value rises StopSquelchRaw parameter. The scanned RAW values are sent using window message Msg to the window procedure of the window handle WinHandle with buffer pointer as WParam and buffer length as LParam.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="freqs">The array of frequencies to be scanned.</param>
        /// <param name="count">The count of frequencies to be scanned.</param>
        /// <param name="stopSquelchRaw">The value of the RAW signal strength at which the scanning should stop.</param>
        /// <param name="feedbackTime">The time interval in ms after which G3 API sends you scanned data.</param>
        /// <param name="hWnd">The window handle to which the scanned data will arrive.</param>
        /// <param name="message">The constant for window message that will bring the scanned data.</param>
        /// <returns>If the scan started, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool BlockScan(int radioHandle,IntPtr freqs,UInt32 count,int stopSquelchRaw,UInt32 feedbackTime,IntPtr hWnd,UInt32 message);

        /// <summary>
        /// Stops block scanning if it has been started.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool  StopBlockScan(int radioHandle);

        /// <summary>
        /// Pauses block scanning, if it has been started.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool PauseBlockScan(int radioHandle);

        /// <summary>
        /// Resumes block scanning if it has been paused.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool ResumeBlockScan(int radioHandle);

        /// <summary>
        /// Retrieves the RADIO_INFO2 structure of the receiver.
        /// </summary>
        /// <remarks>
        /// For backward compatibility, depending on the value of the bLength field in the passed structure, also OLD_RADIO_INFO and RADIO_INFO structures are supported.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="info">The pointer to an empty RADIO_INFO2 structure. It must have bLength member initialized to sizeof(RADIO_INFO2).</param>
        /// <returns>If the function succeeds, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool GetInfo(int radioHandle, IntPtr info);

        /// <summary>
        /// Checks the receiver whether it is ready to accept commands.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the receiver is currently busy TRUE is returned. Otherwise the function returns FALSE.</returns>
        [DllImport(DLL)]
        public static extern bool IsBusy(int radioHandle);

        /// <summary>
        /// Retrieves the system pathname of the receiver.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="buffer">Pointer to the buffer that will be filled by the function with the device pathname.</param>
        /// <param name="size">The size of the destination buffer that must not be exceeded by the function.</param>
        /// <returns>If the function succeeds, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool GetPath(int radioHandle, IntPtr buffer, uint size);

        /// <summary>
        /// Retrieves the system pathname of the receiver.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is a pointer to the receiver pathname (PSTR). Otherwise NULL is returned.</returns>
        [DllImport(DLL)]
        public static extern IntPtr GetPath2(int radioHandle);

        /// <summary>
        /// Checks if the receiver is still connected to the computer. It is significant only for external receivers while the internal ones will always be reported as connected.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <returns>If the function succeeds, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool IsDeviceConnected(int radioHandle);

        /// <summary>
        /// Sets the LED flashing pattern for the external receivers to the specified value.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="pattern">The pattern value as an 8-bit mask for 8 time slots; for each of those slots, a bit value of 1 corresponds to LED turned on and a bit value of 0 corresponds to LED turned off.</param>
        /// <returns>If the function succeeds, the return value is TRUE. Otherwise FALSE is returned.</returns>
        [DllImport(DLL)]
        public static extern bool SetLEDFlashPattern(int radioHandle,byte pattern);


    }
}
