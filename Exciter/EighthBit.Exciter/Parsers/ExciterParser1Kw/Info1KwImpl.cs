using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers.ExciterParser1Kw
{
    public class Info1KwImpl : InfoImplBase, IParser
    {
        protected override void InfoGeneralStatus(CanFrame frame)
        {
            _powerLsb = frame[0];
            _powerMsb = frame[1];
            _vswrLsb = frame[2];
            _vswrMsb = frame[3];
            _status = frame[4];
        }

        protected override void InfoWarning(CanFrame frame)
        {
            _tempWarning = frame[0];
            _ovRefWarning = frame[1];
            _vswrWarning = frame[2];
            _overCurrentWarning = frame[3];
            _fuseWarning = frame[4];
            _globalWarning = frame[5];
        }

        protected override void InfoSelfTest(CanFrame frame)
        {
            _remoteSelfTest1 = frame[0];
        }

        internal Info1KwImpl(IDispatcher dispatcher)
        {
            dispatcher.Register(ID_INFO_TEST, this);
            dispatcher.Register(ID_INFO_WARNING, this);
            dispatcher.Register(ID_INFO_GENERAL_STATUS, this);
        }
    }
}
