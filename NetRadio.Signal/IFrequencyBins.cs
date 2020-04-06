using System.Collections.Generic;
public interface IFrequencyBins
{
    /// <summary>
    /// Gets Spectrum width.
    /// </summary>
    int SpectralWidth { get; }

    /// <summary>
    /// Gets minimum available intensity.
    /// </summary>
    double MinimumIntensity { get; }

    /// <summary>
    /// Gets length of FFT
    /// </summary>
    int FftLength { get; }

    /// <summary>
    /// Gets number of FFT bins.
    /// </summary>
    int BinsCount { get; }

    /// <summary>
    /// Gets extracted frequencies.
    /// </summary>
    /// <returns></returns>
    ICollection<double> Frequencies();

    /// <summary>
    /// Gets all extracted intensities.
    /// </summary>
    /// <returns>Returns a collection of extracted intensities.</returns>
    ICollection<double> Intensities();
}