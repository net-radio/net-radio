using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRadio.Devices.G313.Signal
{
    /// <summary>
    /// Represents Sweep arguments.
    /// </summary>
    public class SweepedArgs:EventArgs
    {
        /// <summary>
        /// Gets Sweeped frequency.
        /// </summary>
        public uint Frequency { get; private set; }

        /// <summary>
        /// Gets Sweep precision.
        /// </summary>
        public double Precision { get; private set; }

        /// <summary>
        /// Gets Sweep data.
        /// </summary>
        public ICollection<SweepedFrequency> Data { get; private set; }

        /// <summary>
        /// Gets Sweepe data as 2D array.
        /// </summary>
        /// <returns>Returns sweep data.</returns>
        public double[][] ToArray()
        {
            var data = new double[3][];

            data[0] = Data.Select(d => d.Max).ToArray();
            data[1] = Data.Select(d => d.Current).ToArray();
            data[2] = Data.Select(d =>d.Min).ToArray();

            return data;
        }

        internal SweepedArgs(uint frequency, double precision, ICollection<SweepedFrequency> data)
        {
            Frequency = frequency;
            Precision = precision;
            Data = data;
        }
    }
}
