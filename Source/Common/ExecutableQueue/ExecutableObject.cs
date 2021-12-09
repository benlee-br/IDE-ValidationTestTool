using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Base class for any queued action. The derived class contains the method that will 
    /// be executed or computed.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors: JLerner</item>
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
    ///			<item name="vssfile">$Workfile: ExecutableObject.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ExecutableQueue/ExecutableObject.cs $</item>
    ///			<item name="vssrevision">$Revision: 8 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Jlerner $</item>
    ///			<item name="vssdate">$Date: 10/16/07 4:00p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

	public partial class ExecutableObject 
    {
        #region Constants
		/// <summary>
		/// Error level
		/// </summary>
		/// <remarks>
		/// Warning: Informational. Something occurred that allows computation to proceed.
		/// Recoverable: Data may have been corrupted such that it may be recalculated correctly.
		/// Severe: Data is invalid but the applicaiton may be shut down gracefully if desired.
		/// Fatal: Crash. Current AppDomain must kill itself.
		/// </remarks>
		public enum ErrorLevel : int
		{
			/// <summary>
			/// Unassigned
			/// </summary>
			Unassigned,
			/// <summary>
			/// NoError: No error has occurred.
			/// </summary>
			NoError,
			/// <summary>
			/// Warning: Informational. Something occurred that allows computation to proceed.
			/// </summary>
			Warning,
			/// <summary>
			/// Recoverable: Minor error has occurred. Data not corrupted, so it may be calculated correctly.
			/// </summary>
			Recoverable,
			/// <summary>
			/// Severe: Data is invalid but the applicaiton may be shut down gracefully if desired.
			/// </summary>
			Severe,
			/// <summary>
			/// Fatal: Crash. Current AppDomain must kill itself.
			/// </summary>
			Fatal
		}
        #endregion

        #region Member Data
        #endregion

        #region Accessors
        #endregion

        #region Delegates and Events
		/// <summary>
		/// Delegate type defining the prototype of the callback method to be called on the subscriber to report an event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void ErrorReportingEventHandler(object sender, ErrorReportingEventArgs args);
		/// <summary>
		/// Event the subscribers use to receive error reports.
		/// </summary>
		public event ErrorReportingEventHandler ErrorReportingEvent; 
        /// <summary>
        /// Delegate type defining the prototype of the method to be called on the ExecutableObject.
        /// It will be called synchronously and sequentially on the Synch ExecutablePriorityQueue,
        /// which is operating asynchronously.
        /// </summary>
        public delegate void IExecutionMethodDelegate(ListDictionary arguments);

        /// <summary>
        /// This event is sent to indicate that an action is Pending, Aborted, Completed.
        /// </summary>
        public event ExecutionStatusEventHandler ExecutionStatusEvent;

        /// <summary>
        /// Delegate type defining the prototype of the callback method that receivers must implement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void ExecutionStatusEventHandler(object sender, ExecutionStatusEventArgs args);

        /// <summary>
        /// Indicates how far along the process or computation is.
        /// </summary>
        public event ProgressEventHandler ProgressStatusEvent;

        /// <summary>
        /// Delegate definng the prototype of the callback method that the receivers must implement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void ProgressEventHandler(object sender, ProgressStatusEventArgs args);

        /// <summary>
        /// Indicates that core data has changed and that the AnalysisObjects need to tell their
        /// subscribers that they need to call AsyncUpdate if they are dirty and, for example, they are a form and 
        /// they are visible.
        /// </summary>
        public event DataUpdateRequiredEventHandler DataUpdateRequiredEvent;

        /// <summary>
        /// Delegate definng the prototype of the callback method that the receivers must implement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void DataUpdateRequiredEventHandler(object sender, DataChangedEventArgs args);

        /// <summary>
        /// This event is sent to indicate that data has been updated.
        /// </summary>
        public event DataUpdatedEventHandler DataUpdatedEvent;

        /// <summary>
        /// Delegate type defining the prototype of the callback method that receivers must implement
        /// for the data updated event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void DataUpdatedEventHandler(object sender, DataChangedEventArgs args);


		/// <summary>
		/// This event is sent to indicate that data has become dirty.
		/// </summary>
		public event DataDirtyEventHandler DataDirtyEvent;

		/// <summary>
		/// Delegate type defining the prototype of the callback method that receivers must implement
		/// for the data dirty event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void DataDirtyEventHandler(object sender, DataChangedEventArgs args);

		/// <summary>
		/// This event is sent to indicate that data must be updated.
		/// </summary>
		public event UpdateRealTimeDataEventHandler UpdateRealTimeDataEvent;

		/// <summary>
		/// Delegate type defining the prototype of the callback method that receivers must implement
		/// for the UpdateRealTimeData event. The results of synchronously processing this event should 
		/// be to add new data to the EDO so it can be processed by the Analysis Engine.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args">No special arguments for this event.</param>
		public delegate void UpdateRealTimeDataEventHandler(object sender, EventArgs args);

		/// <summary>
		/// This event is sent to indicate that can safely be accessed since it has been forced clean.
		/// </summary>
		public event ForceDataValidEventHandler ForceDataValidEvent;

		/// <summary>
		/// Delegate type defining the prototype of the callback method that receivers must implement
		/// for the ForceDataValidEvent event. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args">No special arguments for this event.</param>
		public delegate void ForceDataValidEventHandler(object sender, DataChangedEventArgs args);

		/// <summary>
		/// This event is sent to indicate that all clients have been notified that the data has
		/// been forced valid. It is threfore ASSUMED that if those clients are controls, then the
		/// data may be exported from those controls in whatever form is appropriate, such as a
		/// bitmap from a graph.
		/// </summary>
		public event ClientsNotifiedEventHandler ClientsNotifiedEvent;

		/// <summary>
		/// Delegate type defining the prototype of the callback method that receivers must implement
		/// for the ClientsNotifiedEvent event. THis indicates that all clients have been notified that
		/// their data has been forced valid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args">No special arguments for this event.</param>
		public delegate void ClientsNotifiedEventHandler(object sender, EventArgs args);

        #endregion

        #region Constructors and Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ExecutableObject()
        {
        }
        #endregion

        #region Methods
		/// <summary>
		/// Fire Asynchronous event to subscribers reporting the error which has occurred.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnAsyncErrorReportingEvent(ErrorReportingEventArgs e)
		{
			if (ErrorReportingEvent != null)
			{
				ErrorReportingEvent.BeginInvoke(this, e,null,null);
			}
		}
        /// <summary>
        /// Fire event synchronously to subscribers. The default subscribers are all DataStatusListener objects.
        /// This describes whether the execution is Pending, Ready, or Aborted.
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnExecutionStatusEvent(ExecutionStatusEventArgs e)
        {
            if (ExecutionStatusEvent != null)
            {
                ExecutionStatusEvent(this, e);
            }
        }

        /// <summary>
        /// Asynchronously fire an event to whatever is subscribed to track the progress
        /// of the task or computation currently executing.
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnAsyncProgressEvent(ProgressStatusEventArgs e)
        {
            if (ProgressStatusEvent != null)
            {
                ProgressStatusEvent.BeginInvoke(this, e, null, null);
            }
        }

        /// <summary>
        /// Asynchronously fire an event to whatever is subscribed to indicate that 
        /// data has changed and set the AnalysisObject data flags to dirty.
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnDataUpdatedEvent(DataChangedEventArgs e)
        {
            if (DataUpdatedEvent != null)
            {
                DataUpdatedEvent(this, e);
            }
        }
        /// <summary>
        /// Asynchronously fire an event to whatever is subscribed to indicate that 
        /// data has changed and set the AnalysisObject data flags to dirty.
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnDataUpdateRequiredEvent(DataChangedEventArgs e)
        {
            if (DataUpdateRequiredEvent != null)
            {
                DataUpdateRequiredEvent(this, e);
            }
        }

		/// <summary>
		/// Fire an event to whatever is subscribed to indicate that 
		/// data has changed and set the AnalysisObject data flags to dirty.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnDataDirtyEvent(DataChangedEventArgs e)
		{
			if (DataDirtyEvent != null)
			{
				DataDirtyEvent(this, e);
			}
		}

		/// <summary>
		/// Fire an event to whatever is subscribed to indicate data must be updated.
		/// This is used to tell the globber that it should add any new readings to the EDO.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnUpdateRealTimeDataEvent(EventArgs e)
		{
			if (UpdateRealTimeDataEvent != null)
			{
				UpdateRealTimeDataEvent(this, e);
			}
		}

		/// <summary>
		/// Fire an event to whatever is subscribed to indicate that the data has been forced valid
		/// and that it is safe to access it.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnForceDataValidEvent(DataChangedEventArgs e)
		{
			if (ForceDataValidEvent != null)
			{
				ForceDataValidEvent(this, e);
			}
		}

		/// <summary>
		/// Fire an event to whatever is subscribed to indicate that the clients have all been notified that
		/// the data is safe to load. This is used for reporting so that if we force the data valid, and tell
		/// the controls that they may load, and assume that the controls have loaded theri data, then we can
		/// tell the report generator that it really can now get the data synchronously in the event handler.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnClientsNotifiedEvent(EventArgs e)
		{
			if (ClientsNotifiedEvent != null)
			{
				ClientsNotifiedEvent(this, e);
			}
		}
        
        /// <summary>
        /// Execute action using arguments. Must be derived.
        /// </summary>
        public virtual void Execute()
        {
            throw new ApplicationException("Not implemented here. Must be overridden.");
        }
            
        #endregion

        #region Event Handlers
        #endregion
    }
 
}
