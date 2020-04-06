using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
namespace EighthBit.Exciter
{
    public abstract class ExciterSubSystem : IExciter
    {
        protected static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected ExciterSubSystem(IExciterApi exciter)
        {
            InitializeGlobalMutex();

            exciterApi_ = exciter;
            active_ = false;
        }

        protected readonly IExciterApi exciterApi_;
        protected IExciterController controller_;
        private bool active_;

        public bool Active
        {
            get { return active_; }
        }

        /// <summary>
        /// Mutex is used for restricting more than one access to exciter api 
        /// </summary>
        private static string mutexId_;
        private static MutexSecurity securitySettings_ = new MutexSecurity();
        private static TimeSpan mutexTimeout_ = new TimeSpan(0, 0, 10);

        public string MutexId
        {
            get { return mutexId_; }
        }

        public MutexSecurity SecuritySettings
        {
            get { return securitySettings_; }
        }

        public static TimeSpan MutexTimeout
        {
            get { return mutexTimeout_; }
        }

        private void InitializeGlobalMutex()
        {
            // get application GUID as defined in AssemblyInfo.cs
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();

            // unique id for global mutex - Global prefix means it is global to the machine
            mutexId_ = string.Format("Global\\{{{0}}}", appGuid);

            // edited by Jeremy Wiebe to add example of setting up security for multi-user usage
            // edited by 'Marc' to work also on localized systems (don't use just "Everyone") 
            MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            // MutexSecurity securitySettings = new MutexSecurity();
            SecuritySettings.AddAccessRule(allowEveryoneRule);
        }

        public abstract IExciterController Controller { get; }

        public IExciterApi ExciterApi
        {
            get { return exciterApi_; }
        }

        public virtual IExciter Activate()
        {
            active_ = true;
            return this;
        }

        public virtual IExciter Shutdown()
        {
            active_ = false;
            return this;
        }

        public abstract ExciterOperationMode Mode { get; }

        private bool immediateApply_ = false;
        public bool ImmediateApply
        {
            get { return immediateApply_; }
            set { immediateApply_ = value; }
        }

        public abstract ushort Power { get; set; }
        public abstract ushort PowerMinimum { get; }
        public abstract ushort PowerMaximum { get; }
        
    }
}
