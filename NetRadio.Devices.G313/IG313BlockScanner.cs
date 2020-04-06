using System;
using System.Collections.Generic;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents the block scanner unit if a radio
    /// </summary>
    public interface IBlockScanner:IDisposable
    {
        /// <summary>
        /// Occures after a frequency band is scanned.
        /// </summary>
        event EventHandler<BlockScannerArgs> FrequencyScanned;

        /// <summary>
        /// Occures after issued scan is completed.
        /// </summary>
        event EventHandler<EventArgs> ScanFinished;

        /// <summary>
        /// Occures after issued scan is started.
        /// </summary>
        event EventHandler<EventArgs> ScanStarted;

        /// <summary>
        /// Occures after an in-progress scan is paused.
        /// </summary>
        event EventHandler<EventArgs> ScanPaused;

        /// <summary>
        /// Occures after a paused scan is resumed.
        /// </summary>
        event EventHandler<EventArgs> ScanResumed;

        /// <summary>
        /// Scans provided collection of frequencies against specified Squelch value.
        /// </summary>
        /// <param name="frequency">a collection of frequency bands which should get scanned</param>
        /// <param name="threshold">Raw squelch value [0-256]</param>
        /// <remarks> The block scanning automatically ends when scanned RAW signal strength value rises <see cref="IBlockScanner.FrequencyScanned"/>the event.</remarks>
        /// <returns>Returns true if the operation is started</returns>
        /// <exception cref="InvalidOperationException">Cannot start when scanning is in-progress.</exception>
        bool Start(ICollection<uint> frequency,int threshold);

        /// <summary>
        ///Stops scanning operation. 
        /// </summary>
        /// <returns>Returns true if the stop command is successfully issued.</returns>
        bool Stop();

        /// <summary>
        ///Pauses scanning operation. 
        /// </summary>
        /// <returns>Returns true if the pause command is successfully issued.</returns>
        bool Pause();

        /// <summary>
        ///Resumes scanning operation. 
        /// </summary>
        /// <returns>Returns true if the resume command is successfully issued.</returns>
        bool Resume();
    }
}
