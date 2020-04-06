using System;
using NetRadio.Devices.Exceptions;

namespace NetRadio.Devices.G313
{
    /// <summary>
    /// Represents WinRadio G313 software based RIT unit for USB, LSB and CW modulations.
    /// </summary>
    public class G313Clarifier
    {
        private readonly G313Radio _radio;
        private uint _value;
        private bool _active;
        private bool _applied;

        private void CheckClarifier(uint frequency)
        {
            if (frequency > 1000 || frequency < -1000)
                throw new ArgumentOutOfRangeException("frequency", "clarifier frequency should be defined within the hardware domain");
        }

        /// <summary>
        /// Sets specified RIT frequency
        /// </summary>
        /// <param name="frequency">Specified frequency for RIT</param>
        /// <returns>Returns <see cref="G313Clarifier"/></returns>
        public G313Clarifier Rit(uint frequency)
        {
            CheckClarifier(frequency);
            _value = frequency;
            return this;
        }

        /// <summary>
        /// Gets applied RIT value.
        /// </summary>
        /// <returns>Returns applied RIT value, Zero if not applied.</returns>
        public uint Rit()
        {
            return _applied ? _value : 0;
        }

        /// <summary>
        /// Determines RIT Availability.
        /// </summary>
        /// <returns>Returns true if current modulation supports RIT.</returns>
        public bool Available()
        {
            var mode = _radio.Demodulator().Mode();

            return ! (mode != G313Demodulator.DemodulatorMode.Usb || mode != G313Demodulator.DemodulatorMode.Lsb ||
                      mode != G313Demodulator.DemodulatorMode.Cw);
        }

        internal G313Clarifier Apply()
        {
            if (Available() && _active)
                Add();

            if (!Available() && _active)
                Remove();

            return this;
        }

        /// <summary>
        /// Activates RIT filter
        /// </summary>
        /// <returns>Returns <see cref="G313Clarifier"/></returns>
        public G313Clarifier Activate()
        {
            if (_active)
                return this;

            if(!Available())
                throw new InvalidRadioException(_radio);

            Add();
            _active = true;
            return this;
        }

        /// <summary>
        /// Deactivates RIT filter
        /// </summary>
        /// <returns>Returns <see cref="G313Clarifier"/></returns>
        public G313Clarifier Deactivate()
        {
            if (!_active)
                return this;

            if (!Available())
                throw new InvalidRadioException(_radio);

            Remove();
            _active = false;
            return this;
        }

        private void Remove()
        {
            if (!_applied)
                return;

            _radio.Frequency(_radio.Frequency() - _value);
            _applied = false;
        }

        private void Add()
        {
            if(_applied)
                return;

            _radio.Frequency(_radio.Frequency() + _value);
            _applied = true;
        }

        internal G313Clarifier(G313Radio radio)
        {
            _radio = radio;
        }
    }
}
