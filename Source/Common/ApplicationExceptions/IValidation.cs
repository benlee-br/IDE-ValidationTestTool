using System;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>All data-centric model classes support the IValidation interface.  The 
	/// IValidation interface is polymorphically implemented by each supporting model 
	/// class.  Calling Validate() on any model class will typically result in internal 
	/// calls to all aggregated model class objects until the entire object has either 
	/// passed or failed the validation request.  If the validation request fails, the 
	/// Validate() method will throw an exception derived from 
	/// LoggableApplicationException.</summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:DSpyr.</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">918</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: IValidation.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/ApplicationExceptions/IValidation.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dspyr $</item>
	///			<item name="vssdate">$Date: 7/06/03 10:11p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IValidation
	{
        #region Accessors
        /// <summary>Indicates whether the current instance has passed the internal
        /// validation check.</summary>
        bool IsValid
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>Validate the current instance.</summary>
        void Validate();
        #endregion
	}
}
