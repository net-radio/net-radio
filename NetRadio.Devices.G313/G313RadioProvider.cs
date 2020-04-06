using System;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Standard G313 Radio Provider
    /// </summary>
    internal class G313RadioProvider:IRadioProvider<G313Radio>
    {
        /// <summary>
        /// Opens a radio device by its index to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="index">Radio device number that you would like to open from 1 to 255. If zero is specified, it will open thenext available device.</param>
        /// <exception cref="InvalidOperationException">Throws if the device is not available</exception>
        /// <returns>Returns a G313Radio instance</returns>
        public G313Radio Open(int index)
        {
            G313Radio radio;
            var result = TryOpen(index, out radio);

            if (!result)
                throw new InvalidOperationException("no device");
            
            return radio;
        }

        /// <summary>
        /// Opens a radio device by its serial number to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="serial">Radio device serial number that you would like to open. If NULL is specified, a demo receiver will beopened. The serial number is a null-terminated string.</param>
        /// <exception cref="InvalidOperationException">Throws if the device is not available</exception>
        /// <returns>Returns a G313Radio instance</returns>
        public G313Radio Open(string serial)
        {
            G313Radio radio;
            var result = TryOpen(serial, out radio);

            if (!result)
                throw new InvalidOperationException("no device");

            return radio;
        }

        /// <summary>
        /// Opens a radio device based on provided radio informaion to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="info">Radio information that you would like to open.</param>
        /// <exception cref="InvalidOperationException">Throws if the device is not available</exception>
        /// <returns>Returns a G313Radio instance</returns>
        public G313Radio Open(BasicRadioInfo info)
        {
            return Open(info.Serial);
        }

        /// <summary>
        /// Opens a radio device by its index to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="index">Radio device number that you would like to open from 1 to 255. If zero is specified, it will open thenext available device.</param>
        /// <param name="radio">G313Radio instance</param>
        /// <returns>Returns true if the operation is successful</returns>
        public bool TryOpen(int index, out G313Radio radio)
        {
            var handle = G313Api.OpenRadioDevice(index);
            if (handle == 0)
            {
                radio = null;
                return false;
            }

            radio = new G313Radio(new IntPtr(handle));
            return true;
        }

        /// <summary>
        /// Opens a radio device by its serial number to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="serial">Radio device serial number that you would like to open. If NULL is specified, a demo receiver will beopened. The serial number is a null-terminated string.</param>
        /// <param name="radio">G313Radio instance</param>
        /// <returns>Returns true if the operation is successful</returns>
        public bool TryOpen(string serial, out G313Radio radio)
        {
            var handle = G313Api.Open(serial);
            if (handle == 0)
            {
                radio = null;
                return false;
            }

            radio = new G313Radio(new IntPtr(handle));
            return true;
        }

        /// <summary>
        /// Opens a radio device based on provided radio informaion to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="info">Radio device number that you would like to open from 1 to 255. If zero is specified, it will open thenext available device.</param>
        /// <param name="radio">G313Radio instance</param>
        /// <returns>Returns true if the operation is successful</returns>
        public bool TryOpen(BasicRadioInfo info, out G313Radio radio)
        {
            return TryOpen(info.Serial, out radio);
        }
    }
}
