using System;
using System.Collections;
using BioRad.Common.Services;

namespace BioRad.Common.BusinessRules
{
	#region Documentation Tags
	/// <summary>
	/// All services providing business rules implement this interface.
	/// This interface is the public face of the service.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1595</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: IBusinessRulesService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/BusinessRules/IBusinessRulesService.cs $</item>
	///			<item name="vssrevision">$Revision: 1 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Lvs $</item>
	///			<item name="vssdate">$Date: 5/25/04 10:12p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public interface IBusinessRulesService : IService, IEnumerable, IDisposable
	{
        #region Accessors
		/// <summary>
		/// Indexer to the rules collection keyed by enum. Returned object
		/// is guaranteed to be assignable to given type. If rule implements ICloneable, 
		/// a clone of the rule is returned.
		/// </summary>
		object this[Enum key, Type type] { get; }

		/// <summary>
		/// Indexer to the rules collection keyed by enum. Returned object
		/// is guaranteed to be assignable to IBusinessRule type. If rule implements 
		/// ICloneable, a clone of the rule is returned.
		/// </summary>
		object this[Enum key] { get; }
		#endregion

        #region Methods
		/// <summary>
		/// Implement Close as for Dispose.
		/// </summary>
		void Close();
        #endregion
	}


}
