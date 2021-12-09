using System;
using System.Threading;
using System.Text;
using System.Windows.Forms;

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
	///			<item name="vssfile">$Workfile: WaitProgressMeter.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/WaitProgressMeter.cs $</item>
	///			<item name="vssrevision">$Revision: 14 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class WaitProgressMeter : IDisposable, IWaitProgressMeter
	{
		#region Member Data
		private static System.Windows.Forms.Form m_ProgressForm = null;//defect 3871 and 3872 Ralph
		private Form m_Parent = null;
		private ProgressForm progressForm = null;
		private StringBuilder sb = new StringBuilder();
		private ProgressMeterData m_ProgressMeterData = null;
		private bool m_ThreadRunning = false;
		private int m_PrevProgressValue;
		#endregion

		#region Accessors
		/// <summary>
		/// Get reference to progress form or null.
		/// </summary>
		public static System.Windows.Forms.Form ProgressForm
		{
			get{return m_ProgressForm;}//defect 3871 and 3872 Ralph
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		public WaitProgressMeter(bool begin)
		{
			progressForm = new ProgressForm();
			if ( m_ProgressForm != null )//check if we have nested progress forms.
				System.Diagnostics.Debug.Assert(false,"Nested progress meters?");
			m_ProgressForm = progressForm;

			m_Parent = null;

			// get the ProgressMeterData object
			this.m_ProgressMeterData = ProgressMeterData.GetInstance();
			if ( begin )
				Begin();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="begin"></param>
		public WaitProgressMeter(Form parent, bool begin)
		{
			progressForm = new ProgressForm();
			if ( m_ProgressForm != null )//check if we have nested progress forms.
				System.Diagnostics.Debug.Assert(false,"Nested progress meters?");
			m_ProgressForm = progressForm;

			m_Parent = parent;

			// get the ProgressMeterData object
			this.m_ProgressMeterData = ProgressMeterData.GetInstance();
			if ( begin )
				Begin();
		}
		/// <summary>
		/// Restore cursor.
		/// </summary>
		public void Dispose()
		{
			End();
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		private void ShowWaitDialog(object state)
		{
			//progressForm.ShowDialog(m_Parent);
		}
		private void WaitForUpdate()
		{
			while(m_ThreadRunning)
			{
				Thread.Sleep(200);
				if(m_ThreadRunning)
				{
					UpdateProgressBar();
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void StopUpdate()
		{
			m_ThreadRunning = false;
			Thread.Sleep(500);
		}
		private void UpdateProgressBar()
		{
			int progressValue = this.m_ProgressMeterData.ProgressValue;
			string progressInfo = this.m_ProgressMeterData.ProgressInfo;
			string progressDetail = this.m_ProgressMeterData.ProgressInfoDetails;

			if(this.m_PrevProgressValue != progressValue)
			{
				progressForm.Increment(progressValue - this.m_PrevProgressValue);
				this.m_PrevProgressValue = progressValue;
			}
			SetText(progressInfo);
			SetDetailText(progressDetail);
		}
		#endregion

		#region Implementation of IWaitProgressMeter
		/// <summary>
		/// Call this method to initialize the progress meter.
		/// </summary>
		/// <param name="title">The title for the Progress bar</param>
		/// <param name="text">The text for the Progress bar.</param>
		/// <param name="detailText">The detail text for the Progress bar.</param>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		public void Begin( string title, string text, string detailText, int minimum, int maximum )
		{
			Begin();
			SetTitle(title);
			if(this.m_ProgressMeterData == null)
				this.m_ProgressMeterData = ProgressMeterData.GetInstance();
			this.m_ProgressMeterData.ProgressInfo = text;
			this.m_ProgressMeterData.ProgressInfoDetails = detailText;
			SetText(text);
			SetDetailText(detailText);
			SetRange(minimum, maximum);
		}
		/// <summary>
		/// Call this method to initialize the progress meter.
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		public void Begin( int minimum, int maximum )
		{
			Begin();
			SetText(string.Empty);
			SetDetailText(string.Empty);
			SetRange(minimum, maximum);
		}
		/// <summary>
		/// Call this method to initialize
		/// the progress without setting the range.
		/// </summary>
		public void Begin()
		{
			// Create a callback to subroutine ShowWaitDialog
			WaitCallback dialog = new WaitCallback(ShowWaitDialog);
			// && put in a request to ThreadPool to run the process.
			ThreadPool.QueueUserWorkItem(dialog, null);

			progressForm.Begin();
			SetText(string.Empty);
			SetDetailText(string.Empty);
			SetRange(0, 0);
			Increment(0);

			// start the thread that will update the progress form based on the 
			// ProgressMeterData object
			m_ThreadRunning = true;
			Thread t = new Thread(new ThreadStart(WaitForUpdate));
			t.Name = "UpdateThread";
			t.Start();
		}
		/// <summary>
		/// Call this method to reset the range of the progress.
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public void SetRange( int minimum, int maximum )
		{
			progressForm.SetRange(minimum, maximum);
		}
		/// <summary>
		/// Call this method to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		public void SetText( string text )
		{
			//ClearText();
			progressForm.SetText(text);
		}
		/// <summary>Call this method to update the detail text</summary>
		/// <param name="text">The progress detail text to display</param>
		public void SetDetailText (string text)
		{
			//ClearDetailText();
			progressForm.SetDetailText(text);
		}
		/// <summary>
		/// Call this method to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		public void SetTitle( string text )
		{
			progressForm.SetTitle(text);
		}
		/// <summary>
		/// Call this method to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		private void AppendText( String text )
		{
			sb.Append(text);
			progressForm.SetText(sb.ToString());
		}
		/// <summary>
		/// Call this method to update the progress text.
		/// </summary>
		private void ClearText()
		{
			sb.Remove(0,sb.Length);
			progressForm.SetText(sb.ToString());
		}
		/// <summary>Call this method to clear out the detail text</summary>
		private void ClearDetailText()
		{
			progressForm.SetDetailText(string.Empty);
		}
		/// <summary>
		/// Call this method to increase the progress counter by a specified value.
		/// </summary>
		/// <param name="val">The amount by which to increment the progress indicator</param>
		public void Increment( int val )
		{
			progressForm.Increment( val );
		}
		/// <summary>
		/// Call this method to step the progress meter to a particular value.
		/// </summary>
		/// <param name="val"></param>
		public void StepTo( int val )
		{
			progressForm.StepTo( val );
		}
		/// <summary>
		/// Call this method to finalize the progress meter.
		/// </summary>
		public void End()
		{
			if(m_ThreadRunning)
			{
				// update for the final time before ending
				UpdateProgressBar();
				progressForm.Refresh();
				StopUpdate();
				progressForm.End();
				m_ProgressForm = null;
			}
		}
		#endregion
	}
}
