namespace NetRadio.Devices.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the radio Demodulator is failed.
    /// </summary>
    public class DemodulatorNotReadyException:WinRadioException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetRadio.Devices.Exceptions.DemodulatorNotReadyException"/> class.
        /// </summary>
        /// <param name="radio">A <see cref="T:NetRadio.Devices.IRadio"/> which represents Radio context.</param>
        public DemodulatorNotReadyException(IRadio radio)
            : base("demodulator unit is not initialized.", null, radio)
        {

        }
    }
}
