using EighthBit.Exciter.Can;

namespace EighthBit.Exciter.Parsers
{
    public interface IParser
    {
        void Update(CanFrame frame);
    }
}
