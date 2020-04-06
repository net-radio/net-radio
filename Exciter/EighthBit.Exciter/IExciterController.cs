namespace EighthBit.Exciter
{
    public interface IExciterController : IExciter
    {
        /// <summary>
        /// Call this method to activate exciter
        /// </summary>
        /// <param name="exciter"></param>
        /// <returns></returns>
        IExciter Activate(IExciter exciter);
        ushort Power { get; set; }
        ushort PowerMinimum { get; }
        ushort PowerMaximum { get; }
    }
}
