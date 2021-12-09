using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>Provides helper methods for printing.</summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
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
	///			<item name="vssfile">$Workfile: PrintUtility.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/PrintUtility.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 5/23/07 3:11a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion


	public partial class PrintUtility
	{
        #region Member Data
		/// <summary>The PrintDocument object.</summary>
		PrintDocument m_PrintDoc;
		/// <summary>The bitmap to print</summary>
		Bitmap m_Bitmap;
		/// <summary>The name of the file to print.</summary>
		private string m_FileName;
		/// <summary>The stream reader object to use when printing a file.</summary>
		private StreamReader m_StreamToPrint ;
		/// <summary>The font to use when printing.</summary>
		private Font m_PrintFont;
		/// <summary>The print document's header.</summary>
		private string m_PrintHeader;
		#endregion
		
		#region Accessors
		/// <summary>Message to display if no installed printers were found</summary>
		public static string NoPrinterMessage
		{
            get { return StringUtility.FormatString(Properties.Resources.NoPrinter); }
		}
		#endregion

		#region Methods

		/// <summary>Checks for installed printers.</summary>
		/// <returns>Returns true, if the system has a printer, else false.</returns>
		public static bool PrinterFound()
		{
			return (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count 
				== 0) ? false : true;
		}
		/// <summary>Prints the bitmap.</summary>
		/// <param name="bitmap">The bitmap to print.</param>
		/// <param name="printLandscape">If true, prints in Landscape mode else
		/// prints in portrait mode.</param>
		/// <param name="preview">Preview or not.</param>
		public void Print(Bitmap bitmap, bool printLandscape,
			bool preview)
		{
			if(bitmap == null)
				return;

			m_Bitmap = bitmap;
			m_PrintDoc = new PrintDocument();
			m_PrintDoc.DefaultPageSettings.Landscape = printLandscape;
			m_PrintDoc.PrintPage += new PrintPageEventHandler(Doc_PrintPage);

			if (preview)
			{
				PrintPreviewDialog dialog = new PrintPreviewDialog();
				dialog.Document = m_PrintDoc;
				dialog.ShowDialog();
			}
			else
			{
				PrintDialog dialog = new PrintDialog();
				dialog.Document = m_PrintDoc;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					m_PrintDoc.Print();
				}		
			}
			// destroy the bitmap
			if(bitmap != null)
			{
				bitmap.Dispose();
				bitmap = null;
			}
			if(this.m_Bitmap != null)
			{
				this.m_Bitmap.Dispose();
				this.m_Bitmap = null;
			}
			if(this.m_PrintDoc != null)
			{
				this.m_PrintDoc.PrintPage -= new PrintPageEventHandler(Doc_PrintPage);
				this.m_PrintDoc.Dispose();
				this.m_PrintDoc = null;
			}
		}
		/// <summary>Prints the specified file.</summary>
		/// <param name="tempFileName">The file to print.</param>
		/// <param name="printDocumentName">The name of the document as displayed in the 
		/// print header.</param>
		/// <param name="printLandscape">If true, prints in Landscape mode else
		/// prints in portrait mode.</param>
		/// <param name="font">The font to use.</param>
		/// <param name="preview">Preview or not.</param>
		public void Print(string tempFileName, string printDocumentName, bool printLandscape,
			Font font, bool preview)
		{
			if (string.IsNullOrEmpty(tempFileName))
				return;

			this.m_FileName = tempFileName;
			this.m_PrintFont = font;

			// create the print header
			this.m_PrintHeader = string.Concat(ApplicationStateData.GetInstance.ProductName,
				" ", DateTime.Now.ToShortDateString(), " ",
				DateTime.Now.ToShortTimeString(), " - ", printDocumentName);

			m_PrintDoc = new PrintDocument();
			m_PrintDoc.DefaultPageSettings.Landscape = printLandscape;
			m_PrintDoc.PrintPage += new PrintPageEventHandler(Doc_PrintPage);
			if (preview)
			{
				PrintPreviewDialog dialog = new PrintPreviewDialog();
				dialog.Document = m_PrintDoc;
				dialog.ShowDialog();
			}
			else
			{
				PrintDialog dialog = new PrintDialog();
				dialog.Document = m_PrintDoc;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					m_PrintDoc.Print();
				}
			}
			
			if (this.m_PrintDoc != null)
			{
				this.m_PrintDoc.PrintPage -= new PrintPageEventHandler(Doc_PrintPage);
				this.m_PrintDoc.Dispose();
				this.m_PrintDoc = null;
			}
		}

		private void Doc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			if (m_Bitmap != null)
			{
				// the print area
				int intPrintAreaHeight;
				int intPrintAreaWidth;
				int marginLeft;
				int marginTop;

				// Initialize local variables that contain the bounds of the printing 
				// area rectangle.
				intPrintAreaHeight = m_PrintDoc.DefaultPageSettings.Bounds.Height -
					m_PrintDoc.DefaultPageSettings.Margins.Top - m_PrintDoc.DefaultPageSettings.Margins.Bottom;
				intPrintAreaWidth = m_PrintDoc.DefaultPageSettings.Bounds.Width -
					m_PrintDoc.DefaultPageSettings.Margins.Left - m_PrintDoc.DefaultPageSettings.Margins.Right;
				marginLeft = m_PrintDoc.DefaultPageSettings.Margins.Left; // X coordinate
				marginTop = m_PrintDoc.DefaultPageSettings.Margins.Top; // Y coordinate

				// Initialize the rectangle structure that defines the printing area.
				RectangleF rectPrintingArea = new RectangleF();
				rectPrintingArea.X = marginLeft;
				rectPrintingArea.Y = marginTop;
				rectPrintingArea.Width = (this.m_Bitmap.Width > intPrintAreaWidth) ?
					intPrintAreaWidth : this.m_Bitmap.Width;
				rectPrintingArea.Height = (this.m_Bitmap.Height > intPrintAreaHeight) ?
									intPrintAreaHeight : this.m_Bitmap.Height;

				// draw the image
				e.Graphics.DrawImage(m_Bitmap, rectPrintingArea,
					new RectangleF(0, 0, m_Bitmap.Width, m_Bitmap.Height), GraphicsUnit.Pixel);
			}
			else if (!string.IsNullOrEmpty(this.m_FileName))
			{
				m_StreamToPrint = new StreamReader(this.m_FileName);
				try
				{
					float linesPerPage = 0;
					float yPos = 0;
					int count = 0;
					float leftMargin = e.MarginBounds.Left;
					float topMargin = e.MarginBounds.Top;
					string line = null;

					// Calculate the number of lines per page.
					linesPerPage = e.MarginBounds.Height /
					   this.m_PrintFont.GetHeight(e.Graphics);

					// Print the header 
					yPos = topMargin;
					e.Graphics.DrawString(this.m_PrintHeader, this.m_PrintFont, Brushes.Black,
							   leftMargin, yPos, new StringFormat());
					count++;
					yPos = topMargin + (count * this.m_PrintFont.GetHeight(e.Graphics));
					e.Graphics.DrawString(" ", this.m_PrintFont, Brushes.Black,
						   leftMargin, yPos, new StringFormat());
					count++;

					// Print each line of the file.
					while (count < linesPerPage &&
					   ((line = m_StreamToPrint.ReadLine()) != null))
					{
						yPos = topMargin + (count *
						   this.m_PrintFont.GetHeight(e.Graphics));
						e.Graphics.DrawString(line, this.m_PrintFont, Brushes.Black,
						   leftMargin, yPos, new StringFormat());
						count++;
					}

					// If more lines exist, print another page.
					if (line != null)
						e.HasMorePages = true;
					else
						e.HasMorePages = false;
				}
				finally
				{
					m_StreamToPrint.Close();
				}
			}
		}
		#endregion
	}
}
