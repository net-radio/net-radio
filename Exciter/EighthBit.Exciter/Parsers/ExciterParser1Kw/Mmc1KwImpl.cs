using System.Text;

namespace EighthBit.Exciter.Parsers.ExciterParser1Kw
{
    public class Mmc1KwImpl : Mmc, IParser
    {
        public const uint ID_MMC_INFO = 65;
        public const uint ID_MMC_FILE_0 = 73;
        public const uint ID_MMC_FILE_1 = 74;
        public const uint ID_MMC_FILE_2 = 75;
        public const uint ID_MMC_FILE_3 = 76;
        public const uint ID_MMC_FILE_4 = 77;
        public const uint ID_MMC_FILE_5 = 78;
        public const uint ID_MMC_FILE_6 = 79;
        public const uint ID_MMC_FILE_7 = 80;
        public const uint ID_MMC_FILE_8 = 81;
        public const uint ID_MMC_FILE_9 = 82;


        private readonly string[] _mmcFiles = new string[10];

        public byte MmcFileCount { get; private set; }
        public byte MmcFileIndex { get; private set; }

        public string Selected
        {
            get { return _mmcFiles[MmcFileIndex]; }
        }

        public string this[int index]
        {
            get { return _mmcFiles[index]; }
        }

        public void Update(Can.CanFrame frame)
        {
            if (frame.Id == ID_MMC_INFO)
            {
                MmcFileCount = frame[0];
                MmcFileIndex = frame[1];
                return;
            }

            if (frame.Id >= ID_MMC_FILE_0 && frame.Id <= ID_MMC_FILE_9)
            {
                var name = Encoding.ASCII.GetString(frame.Data());
                _mmcFiles[frame.Id - ID_MMC_FILE_0] = name;
            }
        }

        internal Mmc1KwImpl(IDispatcher dispatcher)
        {
            dispatcher.Register(ID_MMC_INFO, this);
            dispatcher.Register(ID_MMC_FILE_0,this);
            dispatcher.Register(ID_MMC_FILE_1, this);
            dispatcher.Register(ID_MMC_FILE_2, this);
            dispatcher.Register(ID_MMC_FILE_3, this);
            dispatcher.Register(ID_MMC_FILE_4, this);
            dispatcher.Register(ID_MMC_FILE_5, this);
            dispatcher.Register(ID_MMC_FILE_6, this);
            dispatcher.Register(ID_MMC_FILE_7, this);
            dispatcher.Register(ID_MMC_FILE_8, this);
            dispatcher.Register(ID_MMC_FILE_9, this);
        }
    }
}
