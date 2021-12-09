using System;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// An Action is a task that may be queued up for execution on a QueuedPriorityQueue.
    /// </summary>
    /// <remarks>
    /// Remarks section for class.
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:JLerner</item>
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
    ///			<item name="vssfile">$Workfile: Action.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/Action.cs $</item>
    ///			<item name="vssrevision">$Revision: 4 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Jlerner $</item>
    ///			<item name="vssdate">$Date: 1/19/07 6:34p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class Action : ExecutableObject
    {
        #region Constants
		/// <summary>
		/// Type of action. Is this a queued scenario, or a queued data update?
		/// </summary>
		public enum ActionType : int
		{
			/// <summary>
			/// Unassigned.
			/// </summary>
			Unassigned,
			/// <summary>
			/// Queued Scenario.
			/// </summary>
			QueuedScenarioType,
			/// <summary>
			/// Queued Data Computation update.
			/// </summary>
			QueuedDataUpdateType
		}

        #endregion

        #region Member Data 
		/// <summary>
		/// String representing the scenario ID enum string, if this is a queued scenario.
		/// </summary>
		private string m_ScenarioIDString = null;
		/// <summary>
		/// Action type.
		/// </summary>
		private ActionType m_ActionType = ActionType.Unassigned;
        /// <summary>
        /// Unique GUID for action.
        /// </summary>
        private Guid m_Identifier;
        /// <summary>
        /// Name, if any, of source object initiating the computation.
        /// Owner of computation.
        /// </summary>
        private string m_CompSource = null;
        /// <summary>
        /// Arguments to the action. Key/Value based.
        /// </summary>
        private ListDictionary m_ActionArguments = null;

        /// <summary>
        /// Method delegate to execute this action.
        /// </summary>
        private IExecutionMethodDelegate m_ExecutionMethodDelegate;

        /// <summary>
        /// Timeout interval in milliseconds.
        /// </summary>
        private int m_TimeOutInterval = -1;

        /// <summary>
        /// There are two types of data, global data, such as the arguments,
        /// and per instance data, which is specific to this instance. This data is
        /// identified by the tag "PerInstance" and is extrwacted from the shared data
        /// when this object is created.
        /// </summary>
        private object m_PerInstanceData;

        #endregion

        #region Accessors
		/// <summary>
		/// Gets and Sets enum string if this is a queued scenario.
		/// </summary>
		public string ScenarioIDEnumString
		{
			get { return m_ScenarioIDString; }
			set { m_ScenarioIDString = value; }
		}
		/// <summary>
		/// Gets and Sets Action type, queued scenario or queued data computation update.
		/// </summary>
		public ActionType ActionTypeEnum
		{
			get { return m_ActionType; }
			set { m_ActionType = value; }
		}

        /// <summary>
        /// Gets and Sets ID for computation.
        /// </summary>
        public Guid CompID
        {
            get { return m_Identifier; }
            internal set { m_Identifier = value; }
        }
        /// <summary>
        /// Gets and Sets Name of AnalysisObject initiating this computation.
        /// </summary>
        public string CompSourceName
        {
            get { return m_CompSource; }
            internal set { m_CompSource = value; }
        }
        /// <summary>
        /// Gets Timeout interval in milliseconds.
        /// </summary>
        public int TimeOutInterval
        {
            get { return m_TimeOutInterval; }
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Constructor. This also creates a unique ID for the computation.
        /// </summary>
        /// <param name="timeOutMS">Timeout in milliseconds.</param>
        /// <param name="compOwner">Name of the Analysis object 
        /// initating the computation.</param>
        /// <param name="executionMethodDelegate">Delegate for method to execute.</param>
        /// <param name="taskID">Unique iD of the task.</param>
        /// <param name="computationArguments">Function arguments.</param>
        public Action(int timeOutMS, string compOwner, IExecutionMethodDelegate executionMethodDelegate, Guid taskID, ListDictionary computationArguments)
        {
            m_CompSource = compOwner.Clone() as string;
            m_Identifier = taskID;
            m_ExecutionMethodDelegate = executionMethodDelegate;
            m_ActionArguments = computationArguments;
            m_TimeOutInterval = timeOutMS;
            object perInstance = computationArguments["PerInstance"] as object;
            if (perInstance != null)
                m_PerInstanceData = computationArguments["PerInstance"] as object;
            else
                m_PerInstanceData = null;
        }

        /// <summary>
        /// Constructor. This also creates a unique ID for the computation.
        /// </summary>
        /// <remarks>
        /// Required tags:
        /// tagname = "Owner"
        /// </remarks>
        /// <param name="executionMethodDelegate">Queued method to exeute.</param>
        /// <param name="arguments">Function arguments.</param>
        public Action(IExecutionMethodDelegate executionMethodDelegate, ListDictionary arguments)
        {
            m_ExecutionMethodDelegate = executionMethodDelegate;
            m_ActionArguments = arguments;
            object perInstance = arguments["PerInstance"] as object;
            if (perInstance != null)
                m_PerInstanceData = arguments["PerInstance"] as object;
            else
                m_PerInstanceData = null;
        }

        #endregion
        
        #region Methods
        /// <summary>
        /// Execute action using arguments.
        /// </summary>
        public override void Execute()
        {
            m_ExecutionMethodDelegate(m_ActionArguments);
        } 
        #endregion

        #region Event Handlers
        #endregion
    }
}
