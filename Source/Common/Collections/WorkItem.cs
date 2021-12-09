using System;
using System.Threading;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Represents a unit of work to performed.
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: WorkItem.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Collections/WorkItem.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 5/08/06 12:09p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public sealed class WorkItem
	{
        #region Member Data
		private DateTime m_CreatedTime;
		private DateTime m_StartedTime;
		private DateTime m_CompletedTime;
		private object m_Item;
		private PriorityQueue.Priority m_Priority;
        #endregion

        #region Accessors
		/// <summary>
		/// Get/Set message priority.
		/// </summary>
		public PriorityQueue.Priority Priority 
		{
			get {return m_Priority;} 
			set{m_Priority = value;}
		}
		/// <summary>
		/// Get reference to item.
		/// </summary>
		public object Item
		{
			get {return m_Item;}
		}
		/// <summary>
		/// Gets or sets the time when the message created, queued for processing.
		/// </summary>
		public DateTime CreatedTime
		{
			get {return m_CreatedTime;}
			set {m_CreatedTime = value;}
		}
		/// <summary>
		///  Gets or sets the time when processing of the message completed.
		/// </summary>
		public DateTime CompletedTime
		{
			get {return m_CompletedTime;}
			set {m_CompletedTime = value;}
		}
		/// <summary>
		/// Gets or sets the time when processing of the message started.
		/// </summary>
		public DateTime StartedTime
		{
			get {return m_StartedTime;}
			set {m_StartedTime = value;}
		}
		/// <summary>
		/// Gets the length of time the message was sitting in the queue.
		/// </summary>
		public TimeSpan QueuedTime
		{
			get {return StartedTime - CreatedTime;}
		}
		/// <summary>
		/// Gets the length of time to process the message.
		/// </summary>
		public TimeSpan ProcessingTime
		{
			get {return CompletedTime - StartedTime;}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		///   Creates a new instance of the <see cref="WorkItem"/> class.
		/// </summary>
		/// <param name="item">Can be anything.</param>
		public WorkItem( object item )
		{
			if ( item == null )
				throw new ArgumentNullException("item");

			m_Item = item;
			m_CreatedTime = DateTime.Now;
			m_Priority = PriorityQueue.Priority.Normal;
		}
		/// <summary>
		///   Creates a new instance of the <see cref="WorkItem"/> class.
		/// </summary>
		/// <param name="item">Can be anything.</param>
		/// <param name="priority">Priority of item.</param>
		public WorkItem( object item, PriorityQueue.Priority priority )
		{
			m_Item = item;
			m_CreatedTime = DateTime.Now;
			m_Priority = priority;
		}
        #endregion
	}
}
