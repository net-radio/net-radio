
namespace NetRadio.Devices
{
    /// <summary>
    /// Represents a generic radio initializer.
    /// </summary>
    /// <typeparam name="T">Radio type</typeparam>
    public interface IRadioProvider<T> where T:Radio<T>
    {
        /// <summary>
        /// Opens a radio device by its index to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="index">Radio device number that you would like to open from 1 to 255. If zero is specified, it will open thenext available device.</param>
        /// <returns>Returns a radio instance</returns>
        T Open(int index);

        /// <summary>
        /// Opens a radio device by its serial number to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="serial">Radio device serial number that you would like to open. If NULL is specified, a demo receiver will beopened. The serial number is a null-terminated string.</param>
        /// <returns>Returns a radio instance</returns>
        T Open(string serial);

        /// <summary>
        /// Opens a radio device based on provided radio informaion to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="info">Radio information that you would like to open.</param>
        /// <returns>Returns a radio instance</returns>
        T Open(BasicRadioInfo info);

        /// <summary>
        /// Opens a radio device by its index to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="index">Radio device number that you would like to open from 1 to 255. If zero is specified, it will open thenext available device.</param>
        /// <param name="radio">Radio instance</param>
        /// <returns>Returns true if the operation is successful</returns>
        bool TryOpen(int index, out T radio);

        /// <summary>
        /// Opens a radio device by its serial number to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="serial">Radio device serial number that you would like to open. If NULL is specified, a demo receiver will beopened. The serial number is a null-terminated string.</param>
        /// <param name="radio">Radio instance</param>
        /// <returns>Returns true if the operation is successful</returns>
        bool TryOpen(string serial, out T radio);

        /// <summary>
        /// Opens a radio device based on provided radio informaion to allow you to control the device using the other API functions.
        /// </summary>
        /// <param name="info">Radio device number that you would like to open from 1 to 255. If zero is specified, it will open thenext available device.</param>
        /// <param name="radio">Radio instance</param>
        /// <returns>Returns true if the operation is successful</returns>
        bool TryOpen(BasicRadioInfo info, out T radio);
    }
}
