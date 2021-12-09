using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BioRad.Common.Common
{
	#region Documentation Tags
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: SavableDocument.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/SavableDocument.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 10/20/10 3:15p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class SavableDocumentMethods
	{
		//#region Constants
		//#endregion

		//#region Member Data
		//#endregion

		//#region Accessors
		//#endregion

		//#region Delegates and Events
		//#endregion

		#region Constructors and Destructor

		/// <summary>Initializes a new instance of the SavableDocument class.</summary>
		public SavableDocumentMethods() { }

		#endregion

		#region Methods
		/// <summary>
		/// Discover whether any ISavableDocuments are open, dirty, and unsaved.
		/// </summary>
		/// <returns>true if any documents are open, dirty, and unsaved, false if not.</returns>
		public static bool AreAnySavableDocumentsDirtyAndUnsaved()
		{
			foreach (Form form in Application.OpenForms)
			{
				ISavableDocument savableDocument = form as ISavableDocument;
				if (savableDocument == null)
					continue;

				savableDocument.Validate();
				if (savableDocument.IsDirty)
					return true;
			}

			return false;
		}
		/// <summary>
		/// Find all savable documents in the application's open forms collection, and close all which are clean (i.e.
		/// which do not have savable changes).  If any are dirty, alert user, and return false.  If all are clean and
		/// closed by this method, return true.  
		/// </summary>
		/// <returns>true if all savable documents are clean.  False if at least one is dirty.</returns>
		public static bool CloseCleanSavableDocuments()
		{
			bool anyDocumentsDirty = false;
			List<ISavableDocument> appClosedDocuments = new List<ISavableDocument>();
			foreach (Form form in Application.OpenForms)
			{
				ISavableDocument savableDocument = form as ISavableDocument;
				if (savableDocument != null)
				{
					savableDocument.Validate();
					if (savableDocument.IsDirty == false)
					{
						// Allow the app to manage the closing.
						savableDocument.AppClosing = true;
						appClosedDocuments.Add(savableDocument);
					}
					else
					{
						// Allow the user to manage the closing. The closing of the
						// app will be cancelled to allow this.
						anyDocumentsDirty = true;
						savableDocument.AppClosing = false;
					}
				}
			}

			foreach (ISavableDocument doc in appClosedDocuments)
				doc.Close();

			return anyDocumentsDirty == false;
		}
		/// <summary>Bring Savable Documents To Front</summary>
		public static void BringSavableDocumentsToFront()
		{
			// Pull any remaining savable documents to the front.  They all have unsaved changes.
			foreach (Form form in Application.OpenForms)
			{
				if (form is ISavableDocument)
					form.Focus();
			}
		}
		#endregion

		//#region Event Handlers
		//#endregion
	}


	/// <summary>
	/// interface for editable / savable documents which may or may not have unsaved changes (be dirty).
	/// </summary>
	public interface ISavableDocument
	{
		/// <summary>
		/// Whether this document has unsaved changes.
		/// </summary>
		bool IsDirty
		{
			get;
		}
		/// <summary>
		/// Whether subsequent closes are due to the app (true) or human interaction (false).
		/// </summary>
		bool AppClosing
		{
			set;
		}
		/// <summary>
		/// Close the document.
		/// </summary>
		void Close();
		/// <summary>
		/// Validate
		/// </summary>
		bool Validate();
	}
}
