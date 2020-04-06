namespace NetRadio.Devices
{
    /// <summary>
    /// Basic Radio browsing facilities.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Opens a radio device based on provided Radio information.
        /// </summary>
        /// <typeparam name="T">Type of radio</typeparam>
        /// <param name="info">Specified information</param>
        /// <returns>Returns a radio context based on specified information.</returns>
        public static T Open<T>(this BasicRadioInfo info)where T:Radio<T>
        {
            return Radio<T>.Get().Open(info);
        }

        /// <summary>
        /// Opens a radio device based on provided Radio information.
        /// </summary>
        /// <typeparam name="T">Type of radio</typeparam>
        /// <param name="info">Specified information</param>
        /// <param name="radio">Radio context based on specified information.</param>
        /// <returns>Returns true if context creation is successful. </returns>
        public static bool TryOpen<T>(this BasicRadioInfo info,out T radio)where T : Radio<T>
        {
            return Radio<T>.Get().TryOpen(info, out radio);
        }
    }
}
