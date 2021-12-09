using System;

namespace BioRad.Common.BusinessRules
{
	#region Documentation Tags
	/// <summary>
	/// Business rules should either derive from this abstract base class or implement
	/// IBusinessRule directly.
	/// </summary>
	/// <remarks>
	/// A business rule is a configurable set of logic used by the application.
	/// A specific business rule must be accessed only via a custom interface rather
	/// than directly. Business rules are sourced by a service implementing
	/// IBusinessRulesService.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
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
	///			<item name="vssfile">$Workfile: AbstractBusinessRule.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/BusinessRules/AbstractBusinessRule.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public abstract partial class AbstractBusinessRule : IBusinessRule
	{
		#region Member Data
		private string m_Identifier = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Name used to access rule in service. May only be set once.
		/// </summary>
		public string Identifier
		{
			get { return m_Identifier;}
			set 
			{ // One time set
				if (m_Identifier == null)
				{
					m_Identifier = value;
				}
				else
				{
                    throw new InvalidOperationException
                    (StringUtility.FormatString(Properties.Resources.SetOnceProperty_1, new object[] { "Identifier" }));
				}
			}
		}
		#endregion
	}
}
