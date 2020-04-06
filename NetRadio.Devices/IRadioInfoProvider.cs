using System.Collections.Generic;

namespace NetRadio.Devices
{
    /// <summary>
    /// Represents a generic radio initializer.
    /// </summary>
    /// <typeparam name="T">Radio type</typeparam>
    public interface IRadioInfoProvider<T> where T : Radio<T>
    {
        /// <summary>
        /// Gets infromation collection of available radio devices.
        /// </summary>
        /// <returns>Returns <see cref="BasicRadioInfo"/> collection.</returns>
        ICollection<BasicRadioInfo> ListAll();
    }
}
