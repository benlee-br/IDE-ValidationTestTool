using System;
using System.Text;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:</item>
    ///			<item name="review">Last design/code review:</item>
    ///			<item name="requirementid">Requirement ID # : 
    ///				<see href=""></see> 
    ///			</item>
    ///			<item name="classdiagram">
    ///				<see href="Reference\FileORImageName">Class Diagram</see> 
    ///			</item>
    ///		</list>
    /// </classinformation>
    /// <archiveinformation>
    ///		<list type="bullet">
    ///			<item name="vssfile">$Workfile: PowerManagement.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/PowerManagement.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 7/26/06 2:53p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class PowerManagement
    {
        #region Constants

        /// <summary>
        /// Thread's execution requirements.
        /// </summary>
        public enum Flags : uint
        {
            /// <summary>
            /// Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS and one of the other state flags is cleared.
            /// </summary>
            ES_CONTINUOUS = 0x80000000,
            /// <summary>
            /// Forces the display to be on by resetting the display idle timer.
            /// </summary>
            ES_DISPLAY_REQUIRED = 0x00000002,
            /// <summary>
            /// Forces the system to be in the working state by resetting the system idle timer.
            /// </summary>
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enables applications to inform the system that it is in use, 
        /// thereby preventing the system from entering the sleeping power state 
        /// or turning off the display while the application is running.
        /// Note: We no longer throw an exception if the call fails.
        /// (just set Results accordingly in case of failure) 
        /// </summary>
        /// <param name="flags">Thread's execution requirements.
        /// This is a combination of bits:
        /// ES_DISPLAY_REQUIRED ->Reset display timer to the default wait period 
        ///		(defined for Window's screen saver)
        /// ES_SYSTEM_REQUIRED  ->Reset system timer to the default wait period 
        /// ES_DISPLAY_REQUIRED|ES_SYSTEM_REQUIRED ->Reset both the display and system timers 
        /// ES_CONTINUOUS -> Leave continous mode for both the display and system
        /// ES_CONTINUOUS|ES_DISPLAY_REQUIRED ->Display never goes into sleep mode
        /// ES_CONTINUOUS|ES_SYSTEM_REQUIRED  ->System never goes into sleep mode
        /// ES_CONTINUOUS|ES_DISPLAY_REQUIRED|ES_SYSTEM_REQUIRED 
        ///		->Display and system never goes into sleep mode
        /// </param>
        /// <param name="previousState">Previous thread execution state.</param>		
        /// <returns></returns>
        public static Results SetThreadExecutionState(ulong flags,
            ref ulong previousState)
        {
            //TODO: Would make sense to change Results SetThreadExecutionState(ulong flags,ref ulong previousState)
            //to a: SetThreadExecutionState(uint flags,ref uint previousState)
            //since the unmanaged call to SetThreadExecutionState(...) is apparently passing and returning a uint 
            //instead of long.             

            Results results = new Results();
            results.SetSuccess();

            //Call a first time to set new state and query previous state
            previousState = (ulong)BioRad.Win32.Kernel.PowerManagement.SetThreadExecutionState(
                (BioRad.Win32.Kernel.PowerManagement.Flags)flags
                ) & 0xffffffff;

            //Call a second time with the same parameters to make sure it was set properly
            ulong newState = (ulong)BioRad.Win32.Kernel.PowerManagement.SetThreadExecutionState(
                (BioRad.Win32.Kernel.PowerManagement.Flags)flags
                ) & 0xffffffff;

            //Now we compare the return values 

            //If the ES_CONTINUOUS bit is set...
            if ((flags & (ulong)Flags.ES_CONTINUOUS) != 0)
            {
                //and the LSB bits are set, it means there is a request to have power 
                //saving permanently disabled				
                if ((flags & (ulong)(Flags.ES_DISPLAY_REQUIRED | Flags.ES_SYSTEM_REQUIRED)) != 0)
                {
                    //In that case, flags should equal newState
                    if (flags != newState)
                    {
                        //If not, it means that power saving might not have been 
                        //disabled properly. Return false to inform caller that
                        //the command might have failed
                        //TODO: localize that string
                        results.SetFailed("Attempt to prevent Windows from entering sleep mode"
                            + "might have failed.\nMake sure that the Standby mode is disabled "
                            + "in Windows to prevent failures during runs.");
                    }
                }
            }
            return results;
        }

        #endregion
    }
}
