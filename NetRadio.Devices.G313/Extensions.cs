namespace NetRadio.Devices.G313
{
    public static class Extensions
    {
        internal static NativeDefinitions.SoftAgcData ToNative(this SoftwareAgc agc)
        {
            return new NativeDefinitions.SoftAgcData
            {
                dwAttackTime = (uint)agc.AttackTime,
                dwDecayTime = (uint)agc.DecayTime,
                iRefLevel = (int)agc.ReferenceLevel
            };
        }

        internal static SoftwareAgc FillManged(this SoftwareAgc agc, NativeDefinitions.SoftAgcData data)
        {
            agc.ReferenceLevel = data.iRefLevel;
            agc.AttackTime = data.dwAttackTime;
            agc.DecayTime = data.dwDecayTime;

            return agc;
        }
    }
}
