using System;

namespace NetRadio.Devices
{
    /// <summary>
    /// Represents firmware image handle
    /// </summary>
    public interface IImage:IDisposable
    {
        /// <summary>
        /// Locks firmware data in memory.
        /// </summary>
        /// <returns>Returns <see cref="IntPtr"/> which points to firmware data</returns>
        IntPtr LockBits();

        /// <summary>
        /// Unlocks firmware data in memory.
        /// </summary>
        void UnlockBits();

        /// <summary>
        /// Gets size of firmware (number of WORDs).
        /// </summary>
        uint Size { get; }
    }
}
