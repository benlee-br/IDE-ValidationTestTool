using System;

#if UNITTESTDEBUG

namespace BioRad.UnitTest
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
    ///			<item name="vssfile">$Workfile: IUnitTest.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/IUnitTest.cs $</item>
    ///			<item name="vssrevision">$Revision: 5 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
    ///			<item name="vssdate">$Date: 8/02/06 11:49a $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public interface IUnitTest
    {
        #region Methods

        /// <summary>
        /// Initialize Test
        /// </summary>
        void zzNUnitInitTest();

        /// <summary>
        /// Dispose of Test
        /// </summary>
        void zzNUnitDisposeTest();

        /// <summary>
        /// Initialize Fixture
        /// </summary>
        void zzNUnitInitFixture();

        /// <summary>
        /// Dispose of Fixture
        /// </summary>
        void zzNUnitDisposeFixture();

        #endregion
    }
}

#endif
