using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRadio.G31Ddc.Operations
{
    public class SearchEventArgs : EventArgs
    {
        /// <summary>
        /// Creates Event arg.
        /// </summary>
        /// <param name="result"></param>
        [DebuggerStepThrough]
        internal SearchEventArgs(IFrequencyBins bins) { Bins = bins; }

        protected SearchEventArgs() { }

        /// <summary>
        /// Gets raw FFT result.
        /// </summary>
        internal protected virtual IFrequencyBins Bins { get; protected set; }
    }
}
