using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetRadio.Devices.G313;

namespace NetRadio.G313.Model
{
    class DebugModel
    {
        public class DebugCommand
        {
            private readonly object _parent;
            private readonly MethodInfo _method;
            public object Parameter { get; set; }
            public bool SendMode { get; private set; }

            public void Prepare()
            {
                if (SendMode)
                {
                    var type = _method.GetParameters()[0].ParameterType;
                    Parameter = Activator.CreateInstance(type);
                }
                else
                    Parameter = null;
            }

            public void Execute()
            {
                if (SendMode)
                    Send();
                else
                    Recieve();
            }

            private void Send()
            {
                _method.Invoke(_parent, new[] {Parameter});
            }

            private void Recieve()
            {
                Parameter = _method.Invoke(_parent, null);
            }

            public DebugCommand(object parent, MethodInfo method, bool sendMode)
            {
                _parent = parent;
                _method = method;
                SendMode = sendMode;
            }

            public override string ToString()
            {
                var mode = SendMode ? "[Send]" : "[Recieve]";
                return string.Format("{0} {1}", _method.Name, mode);
            }
        }

        private G313Radio _radio;

        private IEnumerable<MethodInfo> GetSendCommands<T>(T obj)
        {
            var type = obj.GetType();

            var methods =
                type.GetMethods().Where(m => m.ReturnType == typeof(T) && m.GetParameters().Length == 1).ToList();

            return methods;
        }

        private IEnumerable<MethodInfo> GetRecieveCommands<T>(T obj)
        {
            var type = obj.GetType();

            var methods =
                type.GetMethods().Where(m => m.ReturnType != typeof(T) && m.GetParameters().Length == 0).ToList();

            return methods;
        }

        public ICollection<DebugCommand> GetCommands()
        {
            var list = new List<DebugCommand>();

            list.AddRange(GetSendCommands(_radio).Select(m=>new DebugCommand(_radio,m,true)));
            list.AddRange(GetSendCommands(_radio.Demodulator()).Select(m => new DebugCommand(_radio.Demodulator(), m, true)));

            list.AddRange(GetRecieveCommands(_radio).Select(m => new DebugCommand(_radio, m, false)));
            list.AddRange(GetRecieveCommands(_radio.Demodulator()).Select(m => new DebugCommand(_radio.Demodulator(), m, false)));

            return list;
        }

        public DebugModel(G313Radio radio)
        {
            _radio = radio;
        }
    }
}
