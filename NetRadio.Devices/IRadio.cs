using System;

namespace NetRadio.Devices
{
    /// <summary>
    /// Represents a generic radio device
    /// </summary>
    public interface IRadio:IDisposable
    {
        /// <summary>
        /// Gets native radio context handle.
        /// </summary>
        IntPtr Handle { get; }
    }
}
