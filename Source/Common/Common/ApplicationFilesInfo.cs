using System;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Data storage class to store information for files like name, version etc..
	/// </summary>
	/// <remarks>
	/// Data storage class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse</item>
	///			<item name="review">Last design/code review:3/10/04 Pramod Walse</item>
	///			<item name="conformancereview">Conformance review:3/10/04 Pramod Walse</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">None</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="None">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ApplicationFilesInfo.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/ApplicationFilesInfo.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class ApplicationFilesInfo : ICloneable
	{
        #region Member Data
		/// <summary>
		/// File Name
		/// </summary>
		private string m_Name;
		/// <summary>
		/// File Version
		/// </summary>
		private string m_Version;
		/// <summary>
		/// File creation date
		/// </summary>
		private DateTime m_CreationDate;
        #endregion

        #region Accessors
		/// <summary>
		/// File Name
		/// </summary>
		public string Name
		{
			get{return m_Name;}
			set{m_Name = value;}
		}
		/// <summary>
		/// File Version
		/// </summary>
		public string Version
		{
			get{return m_Version;}
			set{m_Version = value;}
		}
		/// <summary>
		/// File creation date
		/// </summary>
		public DateTime CreationDate
		{
			get{return m_CreationDate;}
			set{m_CreationDate = value;}
		}

		#endregion

	    #region Constructors and Destructor
		/// <summary>
		/// Default empty constructor.
		/// </summary>
		public ApplicationFilesInfo()
		{
			//Empty.
		}
        #endregion

		#region Methods
		/// <summary>Create a clone of the ApplicationFilesInfo object.</summary>
		/// <returns>The cloned ApplicationFilesInfo object.</returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		#endregion
	}
}
