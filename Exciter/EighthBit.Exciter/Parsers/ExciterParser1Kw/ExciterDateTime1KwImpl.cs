using System;
using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers.ExciterParser1Kw
{
    public class ExciterDateTime1KwImpl : ExciterDateTime, IParser
    {
        public const uint ID_DATETIME_TIME = 59;
        public const uint ID_DATETIME_DATE = 60;

        private const int ASCII_OFFSET = 48;

        private byte _sec0;
        private byte _sec1;
        private byte _min0;
        private byte _min1;
        private byte _hour0;
        private byte _hour1;


        private byte _day0;
        private byte _day1;
        private byte _month0;
        private byte _month1;
        private byte _year0;
        private byte _year1;

        public int Second
        {
            get
            {
                return _sec1*10 + _sec0;
            }
        }

        public int Minute
        {
            get
            {
                return _min1*10 + _min0;
            }
        }

        public int Hour
        {
            get
            {
                return _hour1*10 + _hour0;
            }
        }

        public int Day
        {
            get
            {
                return _day1 * 10 + _day0;
            }
        }

        public int Month
        {
            get
            {
                return _month1 * 10 + _month0;
            }
        }

        public int Year
        {
            get
            {
                return (_year1*10 + _year0) + 1900;
            }
        }

        public DateTime DateTime
        {
            get { return new DateTime(Year, Month, Day, Hour, Minute, Second); }
        }

        public void Update(Can.CanFrame frame)
        {
            switch (frame.Id)
            {
                case ID_DATETIME_TIME:
                    Time(frame);
                    break;
                case ID_DATETIME_DATE:
                    Date(frame);
                    break;
            }
        }

        private void Date(CanFrame frame)
        {
            _day0 = (byte) (frame[0] - ASCII_OFFSET);
            _day1 = (byte) (frame[1] - ASCII_OFFSET);

            _month0 = (byte) (frame[3] - ASCII_OFFSET);
            _month1 = (byte) (frame[4] - ASCII_OFFSET);

            _year0 = (byte) (frame[6] - ASCII_OFFSET);
            _year1 = (byte) (frame[7] - ASCII_OFFSET);
        }

        private void Time(CanFrame frame)
        {
            _sec0 = (byte) (frame[0] - ASCII_OFFSET);
            _sec1 = (byte) (frame[1] - ASCII_OFFSET);

            _min0 = (byte) (frame[3] - ASCII_OFFSET);
            _min1 = (byte) (frame[4] - ASCII_OFFSET);

            _hour0 = (byte) (frame[6] - ASCII_OFFSET);
            _hour1 = (byte) (frame[7] - ASCII_OFFSET);
        }

        internal ExciterDateTime1KwImpl(IDispatcher dispatcher)
        {
            dispatcher.Register(ID_DATETIME_TIME,this);
            dispatcher.Register(ID_DATETIME_DATE,this);
        }
    }
}
