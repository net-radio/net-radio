using System;

namespace NetRadio.G31Ddc.Core.Extension
{
    public static class Extensions
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="math"></param>
        /// <param name="b">base</param>
        /// <param name="p">exponent</param>
        /// <returns></returns>
        public static int Pow(int b, uint p)
        {            
            int ret = 1;
            while (p != 0)
            {
                if ((p & 1) == 1)
                    ret *= b;
                b *= b;
                p >>= 1;
            }
            return ret;
        }
    }
}
