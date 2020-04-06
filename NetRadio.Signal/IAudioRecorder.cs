using System.IO;

namespace NetRadio.Signal
{
    /// <summary>
    /// Represents generic audio recorder.
    /// </summary>
    public interface IAudioRecorder<T>where T:IAudioRecorder<T>
    {
        /// <summary>
        /// Starts recording.
        /// </summary>
        /// <returns>Returns Recorder.</returns>
        T Record(Stream stream);

        /// <summary>
        /// Stops recording.
        /// </summary>
        /// <returns>Returns Recorder.</returns>
        T Stop();
    }
}
