﻿using System;
using System.Runtime.InteropServices;

namespace NetRadio.Devices.G313
{
    internal class G313DspApi
    {
        private const string DLL = G313Definitions.DLL;


        /// <summary>
        /// This is the first function that should be called when using the DSP. It announces the API and the driver that a specific range of resources in the DSP is required by the application so no other application will be allowed to access the same resources until they are freed. It also registers a callback function to the API that will be used when interrupts are generated by the DSP.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="mask">One or more of the following constants specifying the DSP resources to be blocked for the application use: WR_G313_DSP_1_1 - 0x00000001 - Lower halves of DSP program and data memories WR_G313_DSP_1_2 - 0x00000002 - Upper halves of DSP program and data memories</param>
        /// <param name="callbackFunc">Pointer to an application defined function that should be called by the API when a DSP interrupt occurs</param>
        /// <param name="callbackTarget">Value to be passed to the application defined function as argument</param>
        /// <returns>The function returns a handle to the reserved DSP resources or 0 in case of error. The call returns with success only if all required DSP resources could be allocated. The handle retuned by dspOpen must be used in all subsequent DSP related functions calls.</returns>
        [DllImport(DLL)]
        public static extern UInt32 dspOpen(int radioHandle, G313Definitions.G313DspMask mask, NativeDefinitions.CallbackFunc callbackFunc, IntPtr callbackTarget);

        /// <summary>
        /// When the DSP resources are nolonger needed the API and the driver must be announced in order to allow other applications use them. That is done by calling dspClose. After it returns, the DSP handle is nolonger valid and DSP interrupts, even if previously enabled and not disabled through software, are nolonger dispatched for the closed handle. In order to allow maximum flexibility, the DSP is not automatically reset upon closing so it will continue running. This feature is very important when facilities like the “Mute audio on exit”, implemented in the standard G313i demodulator plug-in, are required.
        /// </summary>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="dspHandle">DSP handle for which the resources can be freed for other applications; it must be a value previously returned by a dspOpen call.</param>
        /// <returns>The function returns TRUE on successful closing and FALSE if there was an error.</returns>
        [DllImport(DLL)]
        public static extern bool dspClose(int radioHandle, UInt32 dspHandle);

        /// <summary>
        /// Before loading the code into the DSP in order to boot, normally it should be reset, but there may be special situation in which such a behaviour is not desired. Depending on the current usage of the DSP resources, the driver may perform either a hardware or software reset. The initial DSP status after a software reset is unpredictable and thus any code used to boot it should do all the required initializations. Even with this small disadvantage, the software reset is very important when sharing the DSP between two applications.
        /// </summary>
        /// <remarks>
        /// If the Mask argument indicates the entire DSP (WR_G313_1_1|WR_G313_1_2) a hardware reset is issued. Otherwise a software reset of the code loaded into the specified DSP resources will be done.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="dspHandle">DSP handle for which the resources can be freed for other applications; it must be a value previously returned by a dspOpen call.</param>
        /// <param name="mask">One or more of the following constants specifying the DSP resources to be reset: WR_G313_DSP_1_1 - 0x00000001 - Lower halves of DSP program and data memories WR_G313_DSP_1_2 - 0x00000002 - Upper halves of DSP program and data memories</param>
        /// <returns>The function returns TRUE on successful closing and FALSE if there was an error.</returns>
        [DllImport(DLL)]
        public static extern bool dspReset(int radioHandle, UInt32 dspHandle, G313Definitions.G313DspMask mask);

        /// <summary>
        /// In order to make the DSP run, it must be booted. That requires two things: a special program to do all initializations and then continue with the actual processing and a way of loading that program into the DSP. dspBoot is the function that insures proper loading of a user specified program into the DSP resources associated to the opened handle. For correct booting the data passed to the function must have a very strict format.
        /// </summary>
        /// <remarks>
        /// The boot data must be generated using VisualDSP++ tools with linker output set for IDMA booting. The output of the linker will have the extension .bnm and must be further processed with the IDMA2C tool presented later in this paper. The resulting file is a C/C++ source file that contains an array that should be passed directly to dspBoot after including it in the application source files. Any other data may cause undesired behaviour and may also crash the system.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="dspHandle">DSP handle for which the resources can be freed for other applications; it must be a value previously returned by a dspOpen call.</param>
        /// <param name="mask">One or more of the following constants specifying the DSP to boot: WR_G313_DSP_1_1 - 0x00000001 - Lower halves of DSP program and data memories WR_G313_DSP_1_2 - 0x00000002 - Upper halves of DSP program and data memories</param>
        /// <param name="data">The data to be used to boot the specified DSP resources</param>
        /// <param name="size">The size of the boot data in words.</param>
        /// <returns>If the booting succeeds the return value is TRUE, else it is FALSE. The booting may fail from many reasons, including incorrect booting data.</returns>
        [DllImport(DLL)]
        public static extern bool dspBoot(int radioHandle, UInt32 dspHandle, G313Definitions.G313DspMask mask, IntPtr data, UInt32 size);

        /// <summary>
        /// This function reads data directly from the DSP memory and stores it in the specified application buffer. The position from which the read is done is application dependant and is given through Offset. This function accesses both the program and the data memories and the only way to differentiate between them is through the Offset value: program memory is located at 0x0000-0x3FFF and data memory is located at 0x4000-0x7FDF. For each program memory location there are two words that must be read from the DSP.
        /// </summary>
        /// <remarks>
        /// Practically the application can read from a DSP memory location outside the resources associated to the specified DSP handle, but that is not the recommended usage except for only one situation – when two plug-ins use the the DSP simultaneously.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="dspHandle">DSP handle for which the resources can be freed for other applications; it must be a value previously returned by a dspOpen call.</param>
        /// <param name="mask">One or more of the following constants specifying the DSP to read from: WR_G313_DSP_1_1 - 0x00000001 - Lower halves of DSP program and data memories WR_G313_DSP_1_2 - 0x00000002 - Upper halves of DSP program and data memories</param>
        /// <param name="offset">The DSP memory address from which the read should be started</param>
        /// <param name="data">Pointer to the buffer into which the read data should be stored</param>
        /// <param name="size">The size of the read buffer in words</param>
        /// <returns>The returned value is TRUE if the read succeeded and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool dspRead(int radioHandle, UInt32 dspHandle, G313Definitions.G313DspMask mask, UInt32 offset, IntPtr data, UInt32 size);

        /// <summary>
        /// This function writes data directly into the DSP memory from the specified application buffer. The position to which the write is done is application dependant and is given through Offset. This function accesses both the program and the data memories and the only way to differentiate between them is through the Offset value: program memory is located at 0x0000-0x3FFF and data memory is located at 0x4000-0x7FDF. For each program memory location there are two words that must be written to the DSP.
        /// </summary>
        /// <remarks>
        /// Practically the application can write to a DSP memory location outside the resources associated to the specified DSP handle, but that is not the recommended usage except for only one situation – when two plug-ins use the the DSP simultaneously.
        /// </remarks>
        /// <param name="radioHandle">Handle to a radio device that was returned by OpenRadioDevice or Open.</param>
        /// <param name="dspHandle">DSP handle for which the resources can be freed for other applications; it must be a value previously returned by a dspOpen call.</param>
        /// <param name="mask">One or more of the following constants specifying the DSP to write to: WR_G313_DSP_1_1 - 0x00000001 - Lower halves of DSP program and data memories WR_G313_DSP_1_2 - 0x00000002 - Upper halves of DSP program and data memories</param>
        /// <param name="offset">The DSP memory address to which the write should be started</param>
        /// <param name="data">Pointer to the buffer from which the write data should be read</param>
        /// <param name="size">The size of the write buffer in words</param>
        /// <returns>The returned value is TRUE if the write succeeded and FALSE if not.</returns>
        [DllImport(DLL)]
        public static extern bool dspWrite(int radioHandle, UInt32 dspHandle, G313Definitions.G313DspMask mask, UInt32 offset, IntPtr data, UInt32 size);
    }
}
