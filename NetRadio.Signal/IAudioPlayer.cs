namespace NetRadio.Signal
{
    /// <summary>
    /// Represents generic audio player.
    /// </summary>
    /// <typeparam name="T">Type of audio player</typeparam>
    public interface IAudioPlayer<T>where T:IAudioPlayer<T>
    {
        /// <summary>
        /// Starts playback.
        /// </summary>
        /// <returns>Returns Player.</returns>
        T Play();

        /// <summary>
        /// Stops playback.
        /// </summary>
        /// <returns>Returns Player.</returns>
        T Stop();

        /// <summary>
        /// Gets underlying stream bit rate.
        /// </summary>
        /// <returns>Returns bit rate.</returns>
        int BitRate();
    }
}
