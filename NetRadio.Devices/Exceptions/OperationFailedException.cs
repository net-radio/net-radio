namespace NetRadio.Devices.Exceptions
{
    /// <summary>
    /// The exception that is thrown when there is a failure in radio operations.
    /// </summary>
    public class OperationFailedException:WinRadioException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetRadio.Devices.Exceptions.OperationFailedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the error. The content of <paramref name="message"/> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
        /// <param name="radio">A <see cref="T:NetRadio.Devices.IRadio"/> which represents Radio context.</param>
        public OperationFailedException(string message, IRadio radio) : base(message, null, radio) { }
    }
}
