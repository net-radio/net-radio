namespace NetRadio.Devices.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an invalid command is issued.
    /// </summary>
    public class InvalidRadioException:WinRadioException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetRadio.Devices.Exceptions.InvalidRadioException"/> class.
        /// </summary>
        /// <param name="radio">A <see cref="T:NetRadio.Devices.IRadio"/> which represents Radio context.</param>
        public InvalidRadioException(IRadio radio) : base(string.Empty, null, radio) { }
    }
}
