using System.Diagnostics;

namespace NetRadio.Signal.Utilities
{
    /// <summary>
    /// Provides basic Operating system specifics helpers.
    /// </summary>
    public static class OsHelpers
    {
        /// <summary>
        /// Opens windows Playback settings panel.
        /// </summary>
        public static void OpenPlaybackConfig()
        {
            Process.Start("rundll32.exe","shell32.dll,Control_RunDLL mmsys.cpl,,0");
        }

        /// <summary>
        /// Opens windows Recording settings panel.
        /// </summary>
        public static void OpenRecordingConfig()
        {
            Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL mmsys.cpl,,1");
        }
    }
}
