using System;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// Contains information for the dynamic selection of algorithms.
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
	///			<item name="vssfile">$Workfile: Algorithms.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/Algorithms.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	[Serializable]
	public partial class Algorithms : ICloneable
	{
		#region Constants
        #endregion

        #region Member Data
		/// <summary>Algorithm type</summary>
		private bool m_IsExposureCalculator;
		/// <summary>Is it a default.</summary>
		private bool m_IsDefault;
		/// <summary>Algorithm type</summary>
		private string m_Type;
		/// <summary>Display name</summary>
		private string m_Displayname;
		/// <summary>Class name</summary>
		private string m_Classname;
		/// <summary>Assembly name</summary>
		private string m_Assemblyname;
		/// <summary>Interface name</summary>
		private string m_Interfacename;
		/// <summary>Algorithm description</summary>
		private string m_Description;
        #endregion

        #region Accessors
		/// <summary></summary>
		public bool IsDefault
		{
			get { return this.m_IsDefault;}
			set { this.m_IsDefault = value;}
		}
		/// <summary></summary>
		public string Type
		{
			get { return this.m_Type;}
			set { this.m_Type = value;}
		}
		/// <summary></summary>
		public bool IsExposureCalculator
		{
			get { return this.m_IsExposureCalculator;}
			set { this.m_IsExposureCalculator = value;}
		}
		/// <summary></summary>
		public string Displayname
		{
			get { return this.m_Displayname;}
			set { this.m_Displayname = value;}
		}
		/// <summary></summary>
		public string Classname
		{
			get { return this.m_Classname;}
			set { this.m_Classname = value;}
		}
		/// <summary></summary>
		public string Assemblyname
		{
			get { return this.m_Assemblyname;}
			set { this.m_Assemblyname = value;}
		}
		/// <summary></summary>
		public string Interfacename
		{
			get { return this.m_Interfacename;}
			set { this.m_Interfacename = value;}
		}
		/// <summary></summary>
		public string Description
		{
			get { return this.m_Description;}
			set { this.m_Description = value;}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		public Algorithms()
		{
		}
        #endregion

        #region Methods
		/// <summary>Create a clone of the Algorithms object.</summary>
		/// <returns>An object of the Algorithms type.</returns>
		public object Clone()
		{
			Algorithms clonedObject = (Algorithms)this.MemberwiseClone();
			return clonedObject;
		}
        #endregion
	}
}
