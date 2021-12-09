using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Halt a screen from painting until all the controls on the form are initialized.
	/// </summary>
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
	///			<item name="vssfile">$Workfile: FreezePainting.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/UIUtils/FreezePainting.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 12/13/07 12:31p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class FreezePainting : IDisposable
	{
		#region Constants
		private const int WM_SETREDRAW = 0xB;
		#endregion

		#region User32 EntryPoints
		[DllImport("User32")]
		private static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		#endregion

		#region Member Data
		private Form m_Form;
		#endregion

		#region Accessors
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Halt a screen from painting until all the controls on the form are initialized.
		/// </summary>
		/// <param name="form"></param>
		public FreezePainting(Form form)
		{
			m_Form = form;
			if (form == null)
				throw new ArgumentNullException("form");

			// Halt painting.
			// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn. 

			form.SuspendLayout();
			//SendMessage(m_Form.Handle, WM_SETREDRAW, 0, 0);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enable painting.
		/// </summary>
		public void Dispose()
		{
			// Enable painting.
			//SendMessage(m_Form.Handle, WM_SETREDRAW, 1, 0);
			//m_Form.Invalidate(true);
			m_Form.ResumeLayout();
		}
		#endregion
	}
}
