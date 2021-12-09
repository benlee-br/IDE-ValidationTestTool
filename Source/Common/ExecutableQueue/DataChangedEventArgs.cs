using System;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
	/// This describes the properties associated with a data changed event. This can describe
	/// the DataDirty event or the DataUpdatedEvent.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors: Jeff lerner</item>
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
    ///			<item name="vssfile">$Workfile: DataChangedEventArgs.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/DataChangedEventArgs.cs $</item>
    ///			<item name="vssrevision">$Revision: 7 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Jlerner $</item>
    ///			<item name="vssdate">$Date: 10/31/07 11:30a $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion
	public partial class DataChangedEventArgs : EventArgs
    {
        #region Member Data
		/// <summary>
		/// Describes the DataTime at which the evnt occurred. This gets used as a version number in
		/// determining whether to request new data or not.
		/// </summary>
        private DateTime m_DataChangeDateTime = DateTime.MinValue;
		/// <summary>
		/// Key value pairs describing whether a particular aspect of data represented by an Analysis
		/// Object has changed. The tags are enumerated types represednted as strings and the values are "true" or "false"
		/// depending upon whether that sub data has changed. This allows the client to optimize for reloading
		/// of data.
		/// </summary>
		private ListDictionary m_DataChangedTags = null;
		/// <summary>
		/// Enumerable collection Enum ints describing the data that has changed when the data went dirty.
		/// These are not dirty flags but are just informational. They allow the client to optimize by
		/// eliminating spurious loading of data.
		/// </summary>
		private IEnumerable<int> m_DataChangedEnumTags = null;
		/// <summary>
		/// Last error description. Non localized.
		/// </summary>
		private string m_LastErrorDescriptionInvariantCulture = string.Empty;
		/// <summary>
		/// Localized last error description.
		/// </summary>
		private string m_LastErrorDescriptionLocalized = string.Empty;
		/// <summary>
		/// Level of error of current computation.
		/// </summary>
		private ExecutableObject.ErrorLevel m_ErrorLevel = ExecutableObject.ErrorLevel.NoError;
		/// <summary>
		/// Indicates whether an exception has been thrown.
		/// </summary>
		private bool m_ExceptionThrown = false;
		/// <summary>
		/// When a non-exception error occurs, one may package special error or diagnostics information
		/// as an object. This will be injectd into the DataChangedEventArgs so that the
		/// client may obtain special or custom information without having to call GetData on the AnalysisObject.
		/// </summary>
		private object m_OnDataUpdatedEventCustomData = null;

        #endregion

        #region Accessors
		/// <summary>
		/// Gets and Sets OnDataUpdatedEventCustomData.
		/// </summary>
		public object OnDataUpdatedEventCustomData
		{
			get { return m_OnDataUpdatedEventCustomData; }
			set { m_OnDataUpdatedEventCustomData = value; }
		}

		/// <summary>
		/// Gets and Sets IsExceptionThrown.
		/// </summary>
		public bool IsExceptionThrown
		{
			get { return m_ExceptionThrown; }
			set { m_ExceptionThrown = value; }
		}

		/// <summary>
		/// Gets and Sets LastErrorDescriptionInvariantCulture.
		/// </summary>
		public string LastErrorDescriptionInvariantCulture
		{
			get { return m_LastErrorDescriptionInvariantCulture; }
			set { m_LastErrorDescriptionInvariantCulture = value; }
		}
		/// <summary>
		/// Gets and Sets LastErrorDescriptionLocalized.
		/// </summary>
		public string LastErrorDescriptionLocalized
		{
			get { return m_LastErrorDescriptionLocalized; }
			set { m_LastErrorDescriptionLocalized = value; }
		}

		/// <summary>
		/// Gets and Sets error level.
		/// </summary>
		public ExecutableObject.ErrorLevel ErrorLevel
		{
			get { return m_ErrorLevel; }
			set { m_ErrorLevel = value; }
		}
		
		/// <summary>
        /// Gets and Sets when the data changed.
        /// </summary>
        public DateTime DataChangeDateTime
        {
            get { return m_DataChangeDateTime; }
            set { m_DataChangeDateTime = value; }
        }

		/// <summary>
		/// Gets and Sets data changed tags indicating which internal data has changed.
		/// </summary>
		public ListDictionary DataChangedTags
		{
			get { return m_DataChangedTags; }
			set { m_DataChangedTags = value; }
		}
		/// <summary>
		/// Gets and Sets data changed tags indicating which internal data has changed.
		/// </summary>
		public IEnumerable<int> DataChangedEnumeratedTags
		{
			get { return m_DataChangedEnumTags; }
			set { m_DataChangedEnumTags = value; }
		}
		#endregion

        #region Constructors and Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public DataChangedEventArgs()
        {
        }
        #endregion
    }

    #region Documentation Tags
    /// <summary>
    /// Event arguments
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
    ///			<item name="vssfile">$Workfile: DataChangedEventArgs.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/DataChangedEventArgs.cs $</item>
    ///			<item name="vssrevision">$Revision: 7 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Jlerner $</item>
    ///			<item name="vssdate">$Date: 10/31/07 11:30a $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public class DataUpdateRequiredEventArgs : EventArgs
    {
        #region Constants
        #endregion

        #region Member Data
        private DateTime m_CurrentDataChangedDateTime;
        #endregion

        #region Accessors
        /// <summary>
        /// DateTime at which data set dirty, requiring an update.
        /// </summary>
        public DateTime CurrentDataChangedDateTime
        {
            get { return m_CurrentDataChangedDateTime; }
            set { m_CurrentDataChangedDateTime = value; }
        }
        #endregion

        #region Delegates and Events
        #endregion

        #region Constructors and Destructor
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
    }
}
