using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Exciter.Configuration.Comb
{
    public class Command
    {
        private byte[] _cmd;
        private uint _param;

        public byte[] Cmd
        {
            get { return _cmd; }
        }
        public uint Param
        {
            get { return _param; }
        }

        public Command(char[] command, uint param)
        {
        }

        public Command(string command, uint param)
        {
            _cmd = new byte[4];
            _param = param;

            _cmd = Encoding.ASCII.GetBytes(command);

            Console.WriteLine();
        }
    }
}
