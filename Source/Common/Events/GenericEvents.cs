using System;

namespace BioRad.Common.Events
{
	#region Documentation Tags
	/// <summary>
	/// The GenericEvents class provides a loosely coupled way for objects to receive events from other 
	/// objects.
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
	///			<item name="vssfile">$Workfile: GenericEvents.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Events/GenericEvents.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class GenericEvents
	{
		#region Delegates and Events
		/// <summary>
		/// The generic event.
		/// </summary>
		public event GenericEventHandler GenericEvent;
		/// <summary>
		/// Delegate type specifying the prototype of the method that will be called when the event fires.
		/// </summary>
		/// <remarks>
		///  Second parameter is an EventArgs type containing any additional information 
		///  that receivers of the notification require. Receivers must check/cast the EventArgs type
		///  is a type they are interested in.
		/// </remarks>
		public delegate void GenericEventHandler(object sender, EventArgs info);
		#endregion

		#region Methods
		/// <summary>
		/// Fires the event to all register objects.
		/// </summary>
		/// <param name="e"></param>
		public void FireEvent(EventArgs e) 
		{
			OnGenericEvent(e);
		}
		#endregion

		#region Event Handlers
		/// <summary>
		/// Publishes the event to all registed objects.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnGenericEvent(EventArgs e)
		{
			if (GenericEvent != null)
				GenericEvent(this, e);
		}

		#endregion
	}
}
