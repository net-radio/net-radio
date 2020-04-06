using System;

namespace NetRadio.Devices
{
    /// <summary>
    /// Represents generic radio context.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Radio<T>:IRadio where T:Radio<T>
    {
        /// <summary>
        /// Gets specified radio information provider
        /// </summary>
        /// <typeparam name="TP">Type of radio information provider.</typeparam>
        /// <returns>Returns specified radio information provider</returns>
        public static TP Find<TP>()where TP:IRadioInfoProvider<T>
        {
            var provider = Activator.CreateInstance<TP>();
            return provider;
        }

        /// <summary>
        /// Gets specified radio provider
        /// </summary>
        /// <typeparam name="TP">Type of radio provider.</typeparam>
        /// <returns>Returns specified radio provider</returns>
        public static TP Get<TP>() where TP : IRadioProvider<T>
        {
            var provider = Activator.CreateInstance<TP>();
            return provider;
        }

        /// <summary>
        /// Gets specified radio information provider
        /// </summary>
        /// <returns>Returns specified radio information provider</returns>
        public static IRadioInfoProvider<T> Find()
        {
            var instance = (IProviders<T>)Activator.CreateInstance(typeof(T), true);
            return instance.InfoProvider();
        }

        /// <summary>
        /// Gets specified radio provider
        /// </summary>
        /// <returns>Returns specified radio provider</returns>
        public static IRadioProvider<T> Get()
        {
            var instance = (IProviders<T>)Activator.CreateInstance(typeof(T), true);
            return instance.RadioProvider();
        }

        public abstract void Dispose();

        /// <summary>
        /// Gets native handle of radio context.
        /// </summary>
        public IntPtr Handle
        {
            get;
            protected set;
        }
    }
}
