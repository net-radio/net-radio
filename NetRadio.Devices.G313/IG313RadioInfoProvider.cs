using System.Collections.Generic;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents a G313 radio initializer.
    /// </summary>
    /// <typeparam name="T">Radio type</typeparam>
    public interface IG313RadioInfoProvider<T>:IRadioInfoProvider<T>where T:Radio<T>
    {
        /// <summary>
        /// Gets infromation collection of available radio devices.
        /// </summary>
        /// <returns>Returns <see cref="RadioInfo"/> collection.</returns>
        ICollection<RadioInfo> List();

        /// <summary>
        /// Gets infromation collection of available radio devices.
        /// </summary>
        /// <returns>Returns <see cref="CompleteRadioInfo"/> collection.</returns>
        ICollection<CompleteRadioInfo> ListComplete();

        /// <summary>
        /// Gets infromation collection of available radio devices.
        /// </summary>
        /// <returns>Returns <see cref="LegacyRadioInfo"/> collection.</returns>
        ICollection<LegacyRadioInfo> ListLegacy();
    }
}
