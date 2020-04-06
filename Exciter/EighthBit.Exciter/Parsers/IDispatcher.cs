using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Parsers
{
    interface IDispatcher
    {
        void Register(uint id, IParser parser);
    }
}
