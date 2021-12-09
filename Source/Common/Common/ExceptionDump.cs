using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Class Summary
    /// </summary>
    /// <remarks>
    /// Remarks section for class.
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:Ralph Ansell</item>
    ///			<item name="review">Last design/code review:</item>
    ///			<item name="conformancereview">Conformance review:</item>
    ///			<item name="requirementid">Requirement ID # : 
    ///				<see href="">Replace this text with ID</see> 
    ///			</item>
    ///			<item name="classdiagram">
    ///				<see href="Reference\FileORImageName">Class Diagram</see> 
    ///			</item>
    ///		</list>
    /// </classinformation>
    /// <archiveinformation>
    ///		<list type="bullet">
    ///			<item name="vssfile">$Workfile: ExceptionDump.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/ExceptionDump.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 11/12/07 2:42p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion
    static public class ExceptionDump
    {
        //#region Constants
        //#endregion

        //#region Member Data
        //#endregion

        //#region Accessors
        //#endregion

        //#region Delegates and Events
        //#endregion

        //#region Constructors and Destructor
        //#endregion

        #region Methods
        /// <summary>
        /// Get string representation of exception (message, inner, stack trace).
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ExceptionToString(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            if (exception != null)
            {
                sb.Append(string.Format("Exception class: {0} ", exception.GetType().ToString()));
                sb.Append(string.Format("Exception message: {0} ", exception.Message));
                sb.Append(string.Format("Exception source: {0} ", exception.Source));
                sb.Append(string.Format("Exception location: {0} ", GetExceptionLocation(exception)));
                sb.Append(string.Format("Exception data: {0} ", GetExceptionData(exception)));
                sb.Append(string.Format("Exception Inner exceptions: {0} ", GetExceptionStack(exception)));
                sb.Append(string.Format("Exception Stack Trace: {0} ", exception.StackTrace));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetExceptionStack(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            while (e.InnerException != null)
            {
                e = e.InnerException;
                sb.Append(string.Format("Exception message: {0} ", e.Message));
                sb.Append(" ");
                sb.Append(e.ToString());
            }

            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetExceptionData(Exception e)
        {
            StringBuilder sb = new StringBuilder();

            if (e.Data != null)
            {
                sb.Append("Extra details:");
                foreach (DictionaryEntry de in e.Data)
                    sb.Append(string.Format("  Key=value: '{0}'={1}", de.Key, de.Value));
            }

            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetExceptionLocation(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            if (e.TargetSite != null)
            {
                sb.Append(string.Format("Method name: {0}", e.TargetSite.Name));
                if ( e.TargetSite.DeclaringType != null )
                    sb.Append(string.Format("Type name: {0}", e.TargetSite.DeclaringType.FullName));
            }
            return sb.ToString();
        }
        #endregion

        //#region Event Handlers
        //#endregion
    }
}
