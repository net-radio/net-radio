using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G3XDdc
{
    class NativeLoader:IDisposable
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibraryW(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        public string Name { get; private set; }
        public string LoadFrom { get; private set; }

        protected  readonly IntPtr Module;

        public NativeLoader(string name, string path = null)
        {
            LoadFrom = path ?? Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
            Name = name;

            var fullPath = Path.Combine(LoadFrom, name);
            Module = LoadLibraryW(fullPath);
           
            if(Module==IntPtr.Zero)
                throw new FileLoadException(string.Format("cannot load '{0}'",fullPath));

        }

        protected IntPtr GetMethodAddress(string name)
        {
            var address = GetProcAddress(Module, name);
            if(address==IntPtr.Zero)
                throw new MissingMethodException(string.Format("cannot locate '{0}'",name));

            return address;
        }

        protected T GetMethodDelegate<T>(IntPtr address)
        {
           return (T)(object) Marshal.GetDelegateForFunctionPointer(address, typeof (T));
        }

        protected T GetMethodDelegate<T>(string name)
        {
            var address = GetMethodAddress(name);
            return GetMethodDelegate<T>(address);
        }

        protected Delegate GetMethodDelegate(IntPtr address,Type t)
        {
            return Marshal.GetDelegateForFunctionPointer(address, t);
        }

        protected Delegate GetMethodDelegate(string name, Type t)
        {
            var address = GetMethodAddress(name);
            return Marshal.GetDelegateForFunctionPointer(address, t);
        }

        protected IntPtr GetFunctionPointer(Delegate handle)
        {
            return Marshal.GetFunctionPointerForDelegate(handle);
        }

        protected void BindApiCalls()
        {
            var fields = GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in fields)
            {
                var att = f.GetCustomAttributes(typeof (ApiCallAttribute), false);
                if (att.Length == 0)
                    continue;

                var name = f.PropertyType.Name;
                var handle = GetMethodDelegate(name,f.PropertyType);
                f.SetValue(this,handle,null);
            }
        }

        public void Dispose()
        {
            FreeLibrary(Module);
        }
    }
}
