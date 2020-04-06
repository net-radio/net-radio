using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Origin
{
    public class SignalSpecification
    {
        private bool _enabled;
        private uint _frequency;
        private double _phase;
        private SignalState _state;

        public SignalSpecification()
        {
            _state = SignalState.None;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
            }
        }

        public uint Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
            }
        }

        public double Phase
        {
            get { return _phase; }
            set
            {
                _phase = value;
            }
        }

        public SignalState State
        {
            get { return _state; }
            set
            {
                _state = value;
            }
        }
    }
}
