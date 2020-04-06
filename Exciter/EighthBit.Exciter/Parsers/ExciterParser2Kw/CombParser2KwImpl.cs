using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Parsers.ExciterParser2Kw
{
    public class CombParser2KwImpl : CombParser, IParser
    {
        public const uint ID_DATA_TRANSFER_ACKNOLDGE = 84;
        //public const uint ID_DATA_ERROR = 85;
        public const uint ID_COMB_AMPLITUTE_OVER_WARNING = 85;
        public const uint ID_END_COMB_DATA_TRANSFER = 86;

        private byte _numberOfCommand;

        public byte NumberOfCommand
        {
            get { return _numberOfCommand; }
        }

        public void Update(Can.CanFrame frame)
        {
            switch (frame.Id)
            {
                case ID_DATA_TRANSFER_ACKNOLDGE: // 84
                    _numberOfCommand = frame[0];
                    break;
                case ID_COMB_AMPLITUTE_OVER_WARNING:
                    break;
            }
        }
                
        internal CombParser2KwImpl(IDispatcher dispatcher)
        {
            dispatcher.Register(ID_DATA_TRANSFER_ACKNOLDGE, this);
            dispatcher.Register(ID_COMB_AMPLITUTE_OVER_WARNING, this);
            dispatcher.Register(ID_END_COMB_DATA_TRANSFER, this);
        }
    }
}
