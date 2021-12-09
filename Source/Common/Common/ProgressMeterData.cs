using System;

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
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: ProgressMeterData.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/ProgressMeterData.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ProgressMeterData
	{
        #region Member Data
		/// <summary>The maximum Value.</summary>
		private int m_MaximumValue;
		/// <summary>The Progress Value.</summary>
		private int m_ProgressValue;
		/// <summary>The Progress Information to display.</summary>
		private string m_ProgressInfo;
		/// <summary>The Progress Information details to display.</summary>
		private string m_ProgressInfoDetails;
		/// <summary>Object used to provide singleton access to this object.</summary>
		private static ProgressMeterData m_ProgressMeterData = null;
        #endregion

        #region Accessors
		/// <summary>The Progress Value.</summary>
		public int ProgressValue
		{
			get 
			{
				lock(this)
				{
					return this.m_ProgressValue;}
			}
		
			set
			{
				lock(this)
				{
					this.m_ProgressValue = value;}
			}
		}

		/// <summary>The Progress Information to display.</summary>
		public string ProgressInfo
		{
			get 
			{
				lock(this)
				{
					return this.m_ProgressInfo;}
			}
		
			set
			{
				lock(this)
				{
					this.m_ProgressInfo = value;}
			}
		}
		/// <summary>The Progress Information details to display.</summary>
		public string ProgressInfoDetails
		{
			get 
			{
				lock(this)
				{
					return this.m_ProgressInfoDetails;}
			}
		
			set
			{
				lock(this)
				{
					this.m_ProgressInfoDetails = value;}
			}
		}
		/// <summary>Set the maximum value</summary>
		public int MaximumValue
		{
			get 
			{
				lock(this)
				{
					return this.m_MaximumValue;}
			}
		
			set
			{
				lock(this)
				{
					this.m_MaximumValue = value;}
			}
		}
		#endregion

        #region Constructors and Destructor
		private ProgressMeterData()
		{
			this.ProgressInfo = string.Empty;
			this.ProgressInfoDetails = string.Empty;
			this.ProgressValue = 0;
		}
        #endregion

        #region Methods
		/// <summary>Returns a singleton access to the object</summary>
		/// <returns>The instance of the ProgressData object.</returns>
		public static ProgressMeterData GetInstance()
		{
			if(m_ProgressMeterData == null)
				m_ProgressMeterData = new ProgressMeterData();
			return m_ProgressMeterData;
		}
		/// <summary>Increments the progress value by 1.</summary>
		public void IncrementProgressValue()
		{
			lock(this)
			{
				if(this.m_ProgressValue < this.m_MaximumValue)
					this.m_ProgressValue++;
			}
		}
		/// <summary>Increments the progress value by the value of progressValue.</summary>
		/// <param name="progressValue">The value to increment by.</param>
		public void IncrementProgressValue(int progressValue)
		{
			lock(this)
			{
				this.m_ProgressValue+= progressValue;
			}
		}
		
        #endregion

	}
}
