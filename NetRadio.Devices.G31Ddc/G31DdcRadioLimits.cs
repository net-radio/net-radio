using NetRadio.Devices.G3XDdc;

namespace NetRadio.Devices.G31Ddc
{
    /// <summary>
    /// Limits information for G313 Radio
    /// </summary>
    public class G31DdcRadioLimits
    {
        /// <summary>
        /// Minimum IF Gain
        /// </summary>
        public const int IFGAIN_MIN = 0;
        /// <summary>
        /// Maximum IF Gain
        /// </summary>
        public const int IFGAIN_MAX = 120;

        /// <summary>
        /// Minimum Squelch value
        /// </summary>
        public const int SQUELCH_MIN = -150;
        /// <summary>
        /// Maximum Squelch value
        /// </summary>
        public const int SQUELCH_MAX = 0;

        /// <summary>
        /// Minimum AF Squelch for FM modulation
        /// </summary>
        public const uint AF_SQUELCH_MIN = 0;
        /// <summary>
        /// Maximum AF Squelch for FM modulation
        /// </summary>
        public const uint AF_SQUELCH_MAX = 100;

        /// <summary>
        /// Minimum Audio volume
        /// </summary>
        public const uint VOLUME_MIN = 0;
        /// <summary>
        /// Maximum Audio volume
        /// </summary>
        public const uint VOLUME_MAX = 31;

        /// <summary>
        /// Minimum IF bandwidth
        /// </summary>
        public const uint IF_BANDWIDTH_MIN = 0;
        /// <summary>
        /// Maximum IF bandwidth
        /// </summary>
        public const uint IF_BANDWIDTH_MAX = 15000; //24000;

        /// <summary>
        /// Minimum IF shift
        /// </summary>
        public const int IF_SHIFT_MIN = -7500; //-12000;
        /// <summary>
        /// Maximum IF shift
        /// </summary>
        public const int IF_SHIFT_MAX = 7500; //12000;

        /// <summary>
        /// Minimum Notch filter bandwidth
        /// </summary>
        public const uint NOTCH_BANDWIDTH_MIN = 0;
        /// <summary>
        /// Maximum Notch filter bandwidth
        /// </summary>
        public const uint NOTCH_BANDWIDTH_MAX = 3000;

        /// <summary>
        /// Minimum absolute frequency of the demodulator for given channel
        /// </summary>
        public const uint FREQUENCY_MIN = 9000;

        /// <summary>
        /// Maximum absolute frequency of the demodulator for given channel
        /// </summary>
        public const uint FREQUENCY_MAX = 49995000;

        /// <summary>
        /// 
        /// </summary>
        private readonly G31DdcRadio _radio;

        /// <summary>
        /// Normalizes given Notch filter bandwidth value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public uint NotchBandwidth(uint value)
        {
            return value > NOTCH_BANDWIDTH_MAX ? NOTCH_BANDWIDTH_MAX : value;
        }
        /// <summary>
        /// Normalizes given IF shift value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public int IfShift(int value)
        {
            return value > IF_SHIFT_MAX ? IF_SHIFT_MAX : value < IF_SHIFT_MIN ? IF_SHIFT_MIN : value;
        }
        /// <summary>
        /// Normalizes given IF bandwidth value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public uint IfBandwidth(uint value)
        {
            return value > IF_BANDWIDTH_MAX ? IF_BANDWIDTH_MAX : value;
        }
        /// <summary>
        /// Normalizes given IF gain value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public int IfGain(int value)
        {
            return value > IFGAIN_MAX ? IFGAIN_MAX : value < IFGAIN_MIN ? IFGAIN_MIN : value;
        }

        /// <summary>
        /// Normalizes given Frequency value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public uint Frequency(uint value)
        {
            return value > FREQUENCY_MAX ? FREQUENCY_MAX : value < FREQUENCY_MIN ? FREQUENCY_MIN : value;
        }

        /// <summary>
        /// Normalizes given Squelch value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public int Squelch(int value)
        {
            return value > SQUELCH_MAX ? SQUELCH_MAX : value < SQUELCH_MIN ? SQUELCH_MIN : value;
        }

        /// <summary>
        /// Normalizes AF Squelch for FM modulation value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public uint AfSquelch(uint value)
        {
            return value > AF_SQUELCH_MAX  ? AF_SQUELCH_MAX : value;
        }
        /// <summary>
        /// Normalizes audio volume value
        /// </summary>
        /// <param name="value">Raw value</param>
        /// <returns>Returns normalized value within specified rang</returns>
        public uint Volume(uint value)
        {
            return value > VOLUME_MAX ? VOLUME_MAX : value;
        }
        /// <summary>
        /// Creates G313 limiter based on provided radio instance
        /// </summary>
        /// <param name="radio">G313 radio instance</param>
        public G31DdcRadioLimits(G31DdcRadio radio)
        {
            _radio = radio;
        }
    }
}
