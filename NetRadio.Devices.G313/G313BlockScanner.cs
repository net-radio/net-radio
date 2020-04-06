using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents G313 radio signal strength scanner
    /// </summary>
    public class G313BlockScanner:IBlockScanner
    {
        private readonly G313Radio _parent;

        private GCHandle _gcHandle;

        private bool _started;
        private uint[] _frequencies;
        private uint[] _rawStrength;
        private int _threshold;
        private Task _task;

        /// <summary>
        /// Occures after a frequency band is scanned.
        /// </summary>
        public event EventHandler<BlockScannerArgs> FrequencyScanned;

        /// <summary>
        /// Occures after issued scan is completed.
        /// </summary>
        public event EventHandler<EventArgs> ScanFinished;

        /// <summary>
        /// Occures after issued scan is started.
        /// </summary>
        public event EventHandler<EventArgs> ScanStarted;

        /// <summary>
        /// Occures after an in-progress scan is paused.
        /// </summary>
        public event EventHandler<EventArgs> ScanPaused;

        /// <summary>
        /// Occures after a paused scan is resumed.
        /// </summary>
        public event EventHandler<EventArgs> ScanResumed;

        /// <summary>
        /// Scans provided collection of frequencies against specified Squelch value.
        /// </summary>
        /// <param name="frequency">a collection of frequency bands which should get scanned</param>
        /// <param name="threshold">Raw squelch value [0-256]</param>
        /// <remarks> The block scanning automatically ends when scanned RAW signal strength value rises <see cref="G313BlockScanner.FrequencyScanned"/>the event.</remarks>
        /// <returns>Returns true if the operation is started</returns>
        /// <exception cref="InvalidOperationException">Cannot start when scanning is in-progress.</exception>
        public bool Start(ICollection<uint> frequency, int threshold = 256)
        {
            if (_started)
                throw new InvalidOperationException("block scanning already in progress");

            _frequencies = frequency.ToArray();
            _rawStrength = new uint[_frequencies.Count()];
            _frequencies.CopyTo(_rawStrength, 0);
            _threshold = threshold;

            _gcHandle = GCHandle.Alloc(_rawStrength, GCHandleType.Pinned);

            var result = G313Api.BlockScan(_parent.Handle.ToInt32(), _gcHandle.AddrOfPinnedObject(), (uint)_frequencies.Length, threshold, uint.MaxValue, IntPtr.Zero, 0);
            if (!result)
            {
                _gcHandle.Free();
                return false;
            }

            _started = true;

            _task = TaskUtility.Run(MonitorScan);
            //_task = Task.Run(() => MonitorScan());
            
            return true;
        }

        /// <summary>
        /// Scans provided collection of frequencies asynchronously against specified Squelch value.
        /// </summary>
        /// <param name="frequency">a collection of frequency bands which should get scanned</param>
        /// <param name="threshold">Raw squelch value [0-256]</param>
        /// <remarks> The block scanning automatically ends when scanned RAW signal strength value rises <see cref="G313BlockScanner.FrequencyScanned"/></remarks>
        /// <returns>Returns issued async task</returns>
        /// <exception cref="InvalidOperationException">Cannot start when scanning is in-progress.</exception>
        public async Task StartAsync(ICollection<uint> frequency, int threshold = 256)
        {
            var state = Start(frequency, threshold);
            if (!state) return;
            await _task;
        }

        private void MonitorScan()
        {
            OnScanStarted();
            for (var i = 0; i < _frequencies.Length; i++)
            {
                while (_rawStrength[i] > byte.MaxValue)
                {
                    if (!_started) break;
                    Thread.Sleep(50);
                }
                if (!_started) break;
                OnFrequencyScanned(new BlockScannerArgs(i / (float)_frequencies.Length, (int)_rawStrength[i], _threshold, _frequencies[i]));
            }

            _started = false;
            _gcHandle.Free();
            OnScanFinished();
        }

        /// <summary>
        /// Raises the <see cref="ScanStarted"/> event.
        /// </summary>
        protected void OnScanStarted()
        {
            if (ScanStarted == null)
                return;
            ScanStarted(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ScanPaused"/> event.
        /// </summary>
        protected void OnScanPaused()
        {
            if (ScanPaused == null)
                return;
            ScanPaused(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ScanResumed"/> event.
        /// </summary>
        protected void OnScanResumed()
        {
            if (ScanResumed == null)
                return;
            ScanResumed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ScanFinished"/> event.
        /// </summary>
        protected void OnScanFinished()
        {
            if (ScanFinished == null)
                return;
            ScanFinished(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="FrequencyScanned"/> event.
        /// </summary>
        /// <param name="args">A <see cref="BlockScannerArgs"/> that contains the event data. </param>
        protected void OnFrequencyScanned(BlockScannerArgs args)
        {
            if (FrequencyScanned == null)
                return;
            FrequencyScanned(this, args);
        }

        /// <summary>
        ///Stops scanning operation. 
        /// </summary>
        /// <returns>Returns true if the stop command is successfully issued.</returns>
        public bool Stop()
        {
            _started = false;
            return G313Api.StopBlockScan(_parent.Handle.ToInt32());
        }

        /// <summary>
        ///Pauses scanning operation. 
        /// </summary>
        /// <returns>Returns true if the pause command is successfully issued.</returns>
        public bool Pause()
        {
            return G313Api.PauseBlockScan(_parent.Handle.ToInt32());
        }

        /// <summary>
        ///Resumes scanning operation. 
        /// </summary>
        /// <returns>Returns true if the resume command is successfully issued.</returns>
        public bool Resume()
        {
            return G313Api.ResumeBlockScan(_parent.Handle.ToInt32());
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="G313BlockScanner"/>.
        /// </summary>
        public void Dispose()
        {
            if (_started)
                Stop();
        }

        /// <summary>
        /// Gets Radio context
        /// </summary>
        /// <returns>Returns <see cref="G313Radio"/> instance.</returns>
        public G313Radio Radio()
        {
            return _parent;
        }

        internal G313BlockScanner(G313Radio parent)
        {
            _parent = parent;
        }
    }
}
