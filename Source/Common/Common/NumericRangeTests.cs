using System;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>This class contains general purpose range tests for numeric types.
	/// </summary>
	/// <remarks>This class is stateless.  Therefore, all methods are static.</remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:DSpyr.</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">923</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: NumericRangeTests.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/NumericRangeTests.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class NumericRangeTests
	{
        #region Methods
        /// <summary>Range test utility for double.</summary>
        /// <param name="test">Value to test.</param>
        /// <param name="low">Low limit (inclusive).</param>
        /// <param name="high">High limit (inclusive).</param>
        /// <returns>true if test value is in range else false.</returns>
        public static bool InRange(double test, double low, double high)
        {
            return test >= low && test <= high;
        }
        /// <summary>Range test utility for int.</summary>
        /// <param name="test">Value to test.</param>
        /// <param name="low">Low limit (inclusive).</param>
        /// <param name="high">High limit (inclusive).</param>
        /// <returns>true if test value is in range else false.</returns>
        public static bool InRange(int test, int low, int high)
        {
            return test >= low && test <= high;
        }
        /// <summary>Range test utility for short.</summary>
        /// <param name="test">Value to test.</param>
        /// <param name="low">Low limit (inclusive).</param>
        /// <param name="high">High limit (inclusive).</param>
        /// <returns>true if test value is in range else false.</returns>
        public static bool InRange(short test, short low, short high)
        {
            return test >= low && test <= high;
        }
        #endregion
	}
}
