using System;
using System.Text;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This is a psedue global class to allow test info to be passed into the 
	/// app via the command line and later used anywhere in the code
	/// </summary>
	/// <remarks>
	/// For example:
	/// 
	/// //Test crash reporting
	///	ErrorTestingInfoSingleton si = ErrorTestingInfoSingleton.GetSingle();
	///	if (si.TestCaseName == "testerrorhandling_crashreporting_DoGetInfo")
	///	{
	///		throw new ApplicationException(si.TestCaseName);
	///	}
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Mark Chilcott</item>
	///			<item name="review">Last design/code review:</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ErrorTestingInfoSingleton.cs $</item>
	///			<item name="vssfilepath">$Archive: /CFX_15/Source/Core/Common/Common/ErrorTestingInfoSingleton.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Mchilco $</item>
	///			<item name="vssdate">$Date: 10/08/08 8:18a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ErrorTestingInfoSingleton
	{
		#region Member Data
		// The single instance allowed 
		private static ErrorTestingInfoSingleton theSingleInstance = null;
		private string m_TestCaseName = string.Empty;

		#endregion

		#region Accessors
		/// <summary>
		/// The test case name
		/// </summary>
		public string TestCaseName
		{
			get { return m_TestCaseName; }
			set
			{
				m_TestCaseName = value;
			}
		}
		#endregion

		#region Constructors and Destructor

		/// <summary>Initializes a new instance of the ErrorTestingInfoSingleton class.
		/// </summary> Private constructor preventing creating an instance 
		private ErrorTestingInfoSingleton() { }

		#endregion

		#region Methods

		/// <summary>
		/// The only way to get the object's instance 
		/// </summary>
		/// <returns>the instance</returns>
		public static ErrorTestingInfoSingleton GetSingle()
		{
			if (theSingleInstance == null)
			{
				theSingleInstance = new ErrorTestingInfoSingleton();
			}
			return theSingleInstance;
		}

		#endregion
	}
}
