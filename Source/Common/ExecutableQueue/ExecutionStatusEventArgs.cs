using System;
using System.Collections.Specialized;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Status events for reporting the status of a computation or action to a DataStatusListener.
    /// </summary>
    /// <remarks>
    /// Remarks section for class.
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:</item>
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
    ///			<item name="vssfile">$Workfile: ExecutionStatusEventArgs.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/ExecutionStatusEventArgs.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class ExecutionStatusEventArgs : EventArgs
    {
        #region Constants
        #endregion

        #region Member Data
        /// <summary>
        /// Name of AnalysisObject generating the computation.
        /// </summary>
        private string m_OwnerCommonName;
        /// <summary>
        /// Guid identifying specific computation or action.
        /// </summary>
        private Guid m_ActionID;
        /// <summary>
        /// Indicates whether action is Pending, Aborted, Completed, or Unassigned.
        /// </summary>
        private Execution_Constants.CompStatus m_Status;
        #endregion

        #region Accessors
        /// <summary>
        /// Owner: Common name, such as StandardCurveA
        /// </summary>
        public string OwnerCommonName 
        { 
            get { return m_OwnerCommonName; } 
            set { m_OwnerCommonName = value; } 
        }
        /// <summary>
        /// Each computation has a unique ID associated with it.
        /// </summary>
        public Guid ActionID 
        { 
            get { return m_ActionID; } 
            set { m_ActionID = value; } 
        }
        /// <summary>
        /// Pending, Aborted, Completed, or Unassigned.
        /// </summary>
        public Execution_Constants.CompStatus Status { 
            get { return m_Status; } 
            set { m_Status = value; } 
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Eventargs object
        /// </summary>
        /// <param name="generatorCommonName">Common name for initiator of event.</param>
        /// <param name="actionID">TaskID Guid created at initiation of event.</param>
        /// <param name="status">Computational status of action.</param>
        public ExecutionStatusEventArgs(string generatorCommonName, Guid actionID, Execution_Constants.CompStatus status)
        {
            m_OwnerCommonName = generatorCommonName;
            m_ActionID = actionID;
            m_Status = status;
        }
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
    }
}
