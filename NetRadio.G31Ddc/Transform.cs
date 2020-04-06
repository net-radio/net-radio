using System;

namespace NetRadio.G31Ddc.WaterfallSample
{
    class Transform
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _min;
        private readonly double _max;
        //[a* (x - min_s)] + min_d
        public Transform(double srcMin, double srcMax, double dstMin, double dstMax)
        {
            _a = (dstMax - dstMin)/(srcMax - srcMin);
            _b = -_a*srcMin + dstMin;

            _min = srcMin;
            _max = srcMax;
        }

        public double Apply(double value)
        {
            if (value > _max)
                value = _max;
            if (value < _min)
                value = _min;

            return (value*_a) + _b;
        }
    }
}
