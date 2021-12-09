using System;
using System.IO;
using System.Windows.Forms;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Changes the current cursor to the hour glass. Restores original cursor when class is disposed.
	/// </summary>
	/// <example>
	/// This will create a WaitCursor on a control and restore it to the previous state after the "using" block:
	/// <code>
	///   using( new WaitCursor() )
	///   {
	///      // time consuming code...
	///   }
	/// </code>
	/// </example>
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
	///			<item name="vssfile">$Workfile: WaitCursor.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/WaitCursor.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class WaitCursor : IDisposable
	{
		#region Constants
		/// <summary>
		/// Cursor file name.
		/// </summary>
		private const string c_CursorFileName = "hourglas.ani";
		#endregion

		#region Member Data
		private Cursor m_Saved;
		//private AnimatedCursor animatedCursor;
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// Save current cursor and set current cursor to wait cursor.
		/// </summary>
		public WaitCursor()
		{
			// don't do anything if it is already a wait cursor
			if (Cursor.Current != Cursors.WaitCursor)
			{
				m_Saved = Cursor.Current;
			}
			Cursor.Current = Cursors.WaitCursor;
		}
        #endregion

        #region Methods
		/// <summary>
		/// Restore cursor.
		/// </summary>
		public void Dispose()
		{
			//Bug 3097 (ST) 2005/04/04 
			//Fix for defect 3097-Ipp: Object reference not set to an instance of an object 
			//in WaitCursor.Dispose()
			//Now checking in case Current==null
			if (Cursor.Current != null)
				{
					Cursor.Current.Dispose();
					if (m_Saved != null)
						Cursor.Current = m_Saved;
				}
				else
				{
					if (m_Saved != null)
						Cursor.Current = m_Saved;
				}
		}
        #endregion
	}
}
