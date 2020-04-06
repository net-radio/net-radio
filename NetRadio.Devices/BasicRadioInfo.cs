using System.Linq;

namespace NetRadio.Devices
{
    /// <summary>
    /// Represents WinRadio receivers Information
    /// </summary>
    public class BasicRadioInfo
    {
        /// <summary>
        /// Gets 8 characters long serial number of device - this string may be directly passed to the Open function
        /// </summary>
        public string Serial { get; protected set; }

        /// <summary>
        /// Gets 8 characters long product name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Checks device against emulation parameters
        /// </summary>
        /// <returns>Returns true for emulated (demo) devices, false for real devices</returns>
        public bool IsEmulatedDevice()
        {
            if (string.IsNullOrEmpty(Serial) || string.IsNullOrEmpty(Name))
                return true;

            return Serial.Any(c => !char.IsLetterOrDigit(c));
        }

        /// <summary>
        /// Checks device against real parameters
        /// </summary>
        /// <returns>Returns true for real devices, false for emulated (demo) devices</returns>
        public bool IsRealDevice()
        {
            return !IsEmulatedDevice();
        }
    }
}
