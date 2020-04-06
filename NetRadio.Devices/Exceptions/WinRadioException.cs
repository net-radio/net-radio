using System;

namespace NetRadio.Devices.Exceptions
{
    /// <summary>
    /// Generic WinRadio Exception
    /// </summary>
    public class WinRadioException : Exception
    {
        /// <summary>
        /// Gets Radio Instance which caused the exception
        /// </summary>
        public IRadio Radio { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetRadio.Devices.Exceptions.OperationFailedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the error. The content of <paramref name="message"/> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
        /// <param name="radio">A <see cref="T:NetRadio.Devices.IRadio"/> which represents Radio context.</param>
        public WinRadioException(string message, Exception innerException, IRadio radio)
            : base(message, innerException)
        {
            Radio = radio;
        }
    }
}
