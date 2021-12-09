
using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BioRad.Common;

namespace BioRad.Common
{
	/// <summary>
	/// Summary description for ProgressForm.
	/// </summary>
	internal partial class ProgressForm : System.Windows.Forms.Form, IWaitProgressMeter
	{//defect 3871 and 3872 Ralph
		private Color _progressColor = Color.FromKnownColor(KnownColor.Blue);
		private Font _textFont = new Font("Microsoft Sans Serif", 7.8f);
		private Color colorAltFg = Color.White;		// color for percent string when bar covers the string
		private Color colorBar = Color.White;			// bar color

		private bool _fInApplySkin = false;				// used to prevent recursion on ctlPaint
		private bool displayPercent = true;				// display the percentage

		private int curValue = 0;							// current value
		private int max = 100;								// maximum value
		private int min = 0;								// minimum value
		private int span = 100;								// diff bet min & max
		private int increment = 0;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        //private System.Threading.ManualResetEvent initEvent = new System.Threading.ManualResetEvent(false);

		private bool begin = false;
		private System.Windows.Forms.Panel m_ProgressPanel;
		private System.Windows.Forms.Label m_ProgressInfo;
		private System.Windows.Forms.Label m_ProgressInfoDetail;

		private bool requiresClose = true;

		#region Delegates and Events
		private delegate void BeginInvoker(string title, string text, int minimum, int maximum);
		private delegate void SetTextInvoker(string text);
		private delegate void SetDetailTextInvoker(string text);
		private delegate void IncrementInvoker( int val );
		private delegate void StepToInvoker( int val );
		private delegate void RangeInvoker( int minimum, int maximum );
		#endregion

		/// <summary>
		/// 
		/// </summary>
		internal ProgressForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		/// <summary>
		/// Handles the form load, and sets an event to ensure that
		/// intialization is synchronized with the appearance of the form.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad( e );
            //initEvent.Set();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
			this.m_ProgressPanel = new System.Windows.Forms.Panel();
			this.m_ProgressInfo = new System.Windows.Forms.Label();
			this.m_ProgressInfoDetail = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ProgressPanel
			// 
			this.m_ProgressPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.m_ProgressPanel, "m_ProgressPanel");
			this.m_ProgressPanel.Name = "m_ProgressPanel";
			this.m_ProgressPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.m_ProgressPanel_Paint);
			// 
			// m_ProgressInfo
			// 
			resources.ApplyResources(this.m_ProgressInfo, "m_ProgressInfo");
			this.m_ProgressInfo.Name = "m_ProgressInfo";
			// 
			// m_ProgressInfoDetail
			// 
			resources.ApplyResources(this.m_ProgressInfoDetail, "m_ProgressInfoDetail");
			this.m_ProgressInfoDetail.Name = "m_ProgressInfoDetail";
			// 
			// ProgressForm
			// 
			resources.ApplyResources(this, "$this");
			this.ControlBox = false;
			this.Controls.Add(this.m_ProgressInfoDetail);
			this.Controls.Add(this.m_ProgressInfo);
			this.Controls.Add(this.m_ProgressPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.ResumeLayout(false);

		}
		#endregion

		#region Implementation of IWaitProgressMeter
		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress meter.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="text"></param>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		public void Begin( string title, string text, int minimum, int maximum )
		{
            //initEvent.WaitOne();
			if(this.Visible)
				Invoke( new BeginInvoker( DoBegin ), new object[] { title, text, minimum, maximum } );
		}
		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress meter.
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		public void Begin( int minimum, int maximum )
		{
            //initEvent.WaitOne();
			if(this.Visible)
				Invoke( new RangeInvoker( DoBegin ), new object[] { minimum, maximum } );
		}
		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress callback, without setting the range
		/// </summary>
		public void Begin()
		{
            //initEvent.WaitOne();
			if(this.Visible)
				Invoke( new MethodInvoker( DoBegin ) );
		}
		/// <summary>
		/// Call this method from the worker thread to reset the range in the progress callback
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public void SetRange( int minimum, int maximum )
		{
			if(this.Visible)
				Invoke( new RangeInvoker( DoSetRange ), new object[] { minimum, maximum } );
		}
		/// <summary>
		/// Call this method from the worker thread to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		public void SetTitle( string text )
		{
			if(this.Visible)
				Invoke( new SetTextInvoker(DoSetTitle), new object[] { text } );
		}
		/// <summary>
		/// Call this method from the worker thread to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		public void SetText( String text )
		{
			if(this.Visible)
				Invoke( new SetTextInvoker(DoSetText), new object[] { text } );
		}
		/// <summary>
		/// Call this method from the worker thread to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		public void SetDetailText( String text )
		{
			if(this.Visible)
				Invoke( new SetDetailTextInvoker(DoSetDetailText), new object[] { text } );
		}
		/// <summary>
		/// Call this method from the worker thread to increase the progress counter by a specified value.
		/// </summary>
		/// <param name="val">The amount by which to increment the progress indicator</param>
		public void Increment( int val )
		{
			if(this.Visible)
				Invoke( new IncrementInvoker( DoIncrement ), new object[] { val } );
		}
		/// <summary>
		/// Call this method from the worker thread to step the progress meter to a particular value.
		/// </summary>
		/// <param name="val"></param>
		public void StepTo( int val )
		{
			if(this.Visible)
				Invoke( new StepToInvoker( DoStepTo ), new object[] { val } );
		}
		/// <summary>
		/// Call this method from the worker thread to finalize the progress meter
		/// </summary>
		public void End()
		{
			if( requiresClose )
			{
				if(this.Visible)
					Invoke( new MethodInvoker( DoEnd ) );
			}
		}
		#endregion
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		private void DoSetTitle( string text )
		{
			if ( !begin )
				throw new ApplicationException("begin must be called first.");
			if ( text.Length == 0 )
				this.Text = " ";
			else
				this.Text = text;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		private void DoSetText( String text )
		{
			if ( !begin )
				throw new ApplicationException("begin must be called first.");
			this.m_ProgressInfo.Text = text;
			DrawText(text);
		}
		private void DoSetDetailText(string text)
		{
			if ( !begin )
				throw new ApplicationException("begin must be called first.");
			this.m_ProgressInfoDetail.Text = text;
			DrawText(text);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		private void DoIncrement( int val )
		{
			if ( !begin )
				throw new ApplicationException("begin must be called first.");

			curValue += val;

			UpdateStatusText();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		private void DoStepTo( int val )
		{
			if ( !begin )
				throw new ApplicationException("begin must be called first.");

			curValue = val;

			UpdateStatusText();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="text"></param>
		/// <param name="minimum"></param>
		/// <param name="maximum"></param>
		private void DoBegin( string title, string text, int minimum, int maximum )
		{
			DoBegin();
			DoSetRange( minimum, maximum );
			DoSetText(text);
			DoSetTitle(title);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="minimum"></param>
		/// <param name="maximum"></param>
		private void DoBegin( int minimum, int maximum )
		{
			DoBegin();
			DoSetRange( minimum, maximum );
		}
		/// <summary>
		/// 
		/// </summary>
		private void DoBegin()
		{
			begin = true;
			UpdateStatusText();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="minimum"></param>
		/// <param name="maximum"></param>
		private void DoSetRange( int minimum, int maximum )
		{
			if ( !begin )
				throw new ApplicationException("begin must be called first.");

			curValue = minimum;
			min = minimum;
			max = maximum;
			span = max - min;
			increment++;

			paint();
		}
		/// <summary>
		/// 
		/// </summary>
		private void DoEnd()
		{
			this.Close();
			Application.DoEvents();
		}
		/// <summary>
		/// Utility function that formats and updates the title bar text
		/// </summary>
		private void UpdateStatusText()
		{
			paint();
			this.Update();
		}
		private void DrawText(string text) 
		{
			//Graphics g = label.CreateGraphics();
			//g.Clear(this.BackColor);
			//SolidBrush brush = new SolidBrush(this.ForeColor);
			//g.DrawString(text, this.Font, brush, label.Left, label.Top);
			//brush.Dispose();
			//g.Dispose();
			//paint();
		}
		/// <summary>
		/// paint our progress bar after fetching the Graphics object.
		/// </summary>
		private void paint()
		{
			Graphics g = Graphics.FromHwnd(this.m_ProgressPanel.Handle);
			paint(g);
			g.Dispose();
		}
		/// <summary>
		/// Paint our progress bar.
		/// </summary>
		/// <param name="g">Graphics object for painting.</param>
		private void paint(Graphics g)
		{
			if(_fInApplySkin)			// avoid recursion
				return;
			ApplySkin();				// set colors/font from skin
			Rectangle r = this.m_ProgressPanel.ClientRectangle;				// get current panel rectangle
			if ( span != 0 )
				r.Width = ((curValue - min) * r.Width) / span;		// compute how much of bar to fill
			int barRight = r.Right;										// save pos of right side of bar
			using ( System.Drawing.Brush br = new System.Drawing.SolidBrush(colorBar) )
			{
				g.FillRectangle(br, r);										// fill client area
			}

			if(r.Width != m_ProgressPanel.ClientRectangle.Width)			// check if anything left over
			{
				r = new Rectangle(r.Right, r.Top, m_ProgressPanel.Width - r.Width, r.Height);
				using ( System.Drawing.Brush br = new System.Drawing.SolidBrush(m_ProgressPanel.BackColor) )
				{
					g.FillRectangle(br, r);									// fill client area
				}
			}
			if(displayPercent == true)
			{
				int pct = 0;
				if ( span != 0 )
					pct = ((curValue - min) * 100) / span;		// get percentage
				String strPct = pct.ToString() + "%";				// string for percentage
				SizeF sizePct = g.MeasureString(strPct, m_ProgressPanel.Font); // get rect needed for % string
				if((sizePct.Width > this.m_ProgressPanel.ClientRectangle.Width) || (sizePct.Height > this.m_ProgressPanel.ClientRectangle.Height))
					return;													// can't paint string if too big
				PointF pt = new PointF( (this.m_ProgressPanel.ClientRectangle.Left + (this.m_ProgressPanel.ClientRectangle.Width - sizePct.Width) / 2),
					(this.m_ProgressPanel.ClientRectangle.Top + (this.m_ProgressPanel.ClientRectangle.Height - sizePct.Height) / 2));
				
				System.Drawing.Brush br = null;
				if(Convert.ToInt32(pt.X + sizePct.Width) <= barRight)	// is % indicator covered by bar?
					br = new SolidBrush(this.colorAltFg);				// use alternate fg color
				else
					br = new System.Drawing.SolidBrush(this.m_ProgressPanel.ForeColor); // use panel's foreground color
				g.DrawString(strPct, m_ProgressPanel.Font, br, pt);
				br.Dispose();
			}
		}
		/// <summary>
		/// Apply appearance parameters from skin.
		/// </summary>
		private void ApplySkin()
		{
			_fInApplySkin = true;
			this.BarColor = _progressColor; // use bar color from skin
			m_ProgressPanel.Font = _textFont;
			_fInApplySkin = false;
		}
		private Color BarColor
		{
			get{ return colorBar; }
			set
			{
				colorBar = value;
				paint();						// repaint
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private bool DisplayPercent
		{
			get{ return displayPercent; }
			set
			{
				displayPercent = value;
				paint();						// repaint
			}
		}
		private void m_ProgressPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			paint(e.Graphics);
		}
	}
}
