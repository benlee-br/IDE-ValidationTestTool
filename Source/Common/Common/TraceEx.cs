using System;
using System.Diagnostics;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This is a wrapper class for the .Net System.Diagnostics.Trace Functions to be used for
	/// Debug purposes.
	/// 
	/// There are 4 levels of Tracing defined:
	/// 
	/// Off, Error, Warning, Info, and Verbose 
	/// 
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Mark Chilcott</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: TraceEx.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/TraceEx.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class TraceEx
	{
		#region Member Data
		/// <summary>
		/// The is a Switch to set the Trace level
		/// </summary>
		static TraceSwitch m_Switch = new TraceSwitch("General", "Entire Application");
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the Trace class
		/// </summary>
		public TraceEx()
		{
		}
		#endregion

		#region Accessors
		/// <summary>Gets the Trace Level.
		/// </summary>
		public TraceLevel Level
		{
			get
			{return m_Switch.Level;}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Sets the Trace level to Off.
		/// </summary>
		public void SetOff()
		{
			m_Switch.Level = TraceLevel.Off;
			Trace.WriteLine("Setting Trace Level to " + m_Switch.Level.ToString());
		}
		/// <summary>
		/// Sets the Trace level to Error.
		/// </summary>
		public void SetError()
		{
			m_Switch.Level = TraceLevel.Error;
			Trace.WriteLine("Setting Trace Level to " + m_Switch.Level.ToString());

		}
		/// <summary>
		/// Sets the Trace level to Warning.
		/// </summary>
		public void SetWarning()
		{
			m_Switch.Level = TraceLevel.Warning;
			Trace.WriteLine("Setting Trace Level to " + m_Switch.Level.ToString());

		}
		/// <summary>
		/// Sets the Trace level to Info.
		/// </summary>
		public void SetInfo()
		{
			m_Switch.Level = TraceLevel.Info;
			Trace.WriteLine("Setting Trace Level to " + m_Switch.Level.ToString());

		}

		/// <summary>
		/// Sets the Trace level to Verbose.
		/// </summary>
		public void SetVerbose()
		{
			m_Switch.Level = TraceLevel.Verbose;
			Trace.WriteLine("Setting Trace Level to " + m_Switch.Level.ToString());
		}

		/// <summary>
		/// Writes and Error Message to the current Trace Output
		/// </summary>
		/// <param name="Message">The Message</param>
		public void WriteError(string Message)
		{
			if (m_Switch.Level >= TraceLevel.Error)
			{
				Trace.WriteLine(Message);
			}
		}

		/// <summary>
		/// Writes and Warning Message to the current Trace Output
		/// </summary>
		/// <param name="Message">The Message</param>
		public void WriteWarning(string Message)
		{
			if (m_Switch.Level >= TraceLevel.Warning)
			{
				Trace.WriteLine(Message);
			}
		}

		/// <summary>
		/// Writes and Infromational Message to the current Trace Output
		/// </summary>
		/// <param name="Message">The Message</param>
		public void WriteInfo(string Message)
		{
			if (m_Switch.Level >= TraceLevel.Info)
			{
				Trace.WriteLine(Message);
			}
		}

		/// <summary>
		/// Writes and Verbose Message to the current Trace Output
		/// </summary>
		/// <param name="Message">The Message</param>
		public void WriteVerbose(string Message)
		{
			if (m_Switch.Level >= TraceLevel.Verbose)
			{
				Trace.WriteLine(Message);
			}
		}
		#endregion
	} // class TraceEx
}
