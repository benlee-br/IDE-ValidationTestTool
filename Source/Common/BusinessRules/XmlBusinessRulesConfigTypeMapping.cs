using System;
using BioRad.Common.Xml;

namespace BioRad.Common.BusinessRules
{
	#region Documentation Tags
	/// <summary>
	/// Methods support adding attribute mappings for business rule service configuration
	/// elements.
	/// </summary>
	/// <remarks>
	/// The exposed methods are not static, so that this may be subclassed and the methods
	/// overridden.
	/// </remarks>
	/// <example>
	///<!-- test configuration for a business rules service -->
	///<!-- Defines 3 business rules: -->
	///<!-- parameterized, parameterized constructor, parameterized with custom type -->
	///<business-rule-service>
	///	<!-- list of business rule configuration elements -->
	///	<businessRules>
	///		<!-- A simple business rule with one system type property -->
	///		<businessRule identifier="SimpleRule" 
	///						className="BioRad.Common.BusinessRules.BusinessRulesServiceUT+SimpleBusinessRule"
	///						assemblyName="BioRad.PCR.AcquisitionControlUT">
	///			<!-- optional list of property initializers for constructed business rule -->
	///			<propertyInitializers>
	///				<!-- used to initialize a property -->
	///				<propertyInitializer propertyName="SimpleValue" value="100"></propertyInitializer>
	///			</propertyInitializers>
	///		</businessRule>
	///		<!-- a business rule with a parameterized constructor taking a single value -->
	///		<businessRule identifier="ParameterizedConstructorRule" 
	///						className="BioRad.Common.BusinessRules.BusinessRulesServiceUT+ParameterizedConstructorBusinessRule"
	///						assemblyName="BioRad.PCR.AcquisitionControlUT">
	///			<!-- optional list of business rule constructor parameters -->
	///			<!-- if a constructor is found matching this parameter list it will -->
	///			<!-- be used to construct the business rule -->
	///			<constructorParameters>
	///				<!-- used to construct a constructor parameter -->
	///				<typeInitializer value="200" typeName="System.Int32"></typeInitializer>
	///			</constructorParameters>
	///		</businessRule>
	///		<!-- a business rule with a custom type property -->
	///		<businessRule identifier="CustomTypeRule" 
	///						className="BioRad.Common.BusinessRules.BusinessRulesServiceUT+CustomTypeBusinessRule"
	///						assemblyName="BioRad.PCR.AcquisitionControlUT">
	///			<propertyInitializers>
	///				<!-- used to initialize a property -->
	///				<propertyInitializer propertyName="CustomValue" value="300"></propertyInitializer>
	///			</propertyInitializers>
	///		</businessRule>
	///	</businessRules>
	///</business-rule-service>
	/// </example>
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
	///			<item name="vssfile">$Workfile: XmlBusinessRulesConfigTypeMapping.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/BusinessRules/XmlBusinessRulesConfigTypeMapping.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class XmlBusinessRulesConfigTypeMapping
	{
		/// <summary>
		/// Provides mapping for BusinessRulesConfig type.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetBusinessRulesConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(BusinessRulesConfig),
				"business-rule-service");
			mapping.AddCollectionMapping("businessRules", "BusinessRuleConfigs", "businessRule", 
				typeof(BusinessRulesConfig.BusinessRuleConfig));
			mapping.AddNestedTypeMapping(GetBusinessRuleConfigMapping());
			return mapping;
		}

		/// <summary>
		/// Provides mapping for BusinessRulesConfig.BusinessRuleConfig type.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetBusinessRuleConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(BusinessRulesConfig.BusinessRuleConfig),
				"businessRule");
			mapping.AddAttributeMapping("identifier", "Identifier", typeof(string));
			mapping.AddAttributeMapping("className", "ClassName", typeof(string));
			mapping.AddAttributeMapping("assemblyName", "AssemblyName", typeof(string));
			mapping.AddCollectionMapping("constructorParameters", "Initializers",
				"typeInitializer", typeof(BusinessRulesConfig.TypeConfig ));
			mapping.AddNestedTypeMapping(GetTypeConfigMapping());
			mapping.AddCollectionMapping("propertyInitializers", "Parameters",
				"propertyInitializer", typeof(BusinessRulesConfig.TypeConfigParameter));
			mapping.AddNestedTypeMapping(GetTypeConfigParameterMapping());
			return mapping;
		}

		/// <summary>
		/// Provides mapping for BusinessRulesConfig.TypeConfig type
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetTypeConfigMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(BusinessRulesConfig.TypeConfig),
				"typeInitializer");
			mapping.AddAttributeMapping("value", "Value", typeof(string));
			mapping.AddAttributeMapping("typeName", "TypeName", typeof(string));
			return mapping;
		}

		/// <summary>
		/// Provides mapping for BusinessRulesConfig.TypeConfigParameter type.
		/// </summary>
		/// <returns>The type mapping.</returns>
		public virtual XmlToTypeMapping GetTypeConfigParameterMapping()
		{
			XmlToTypeMapping mapping = new XmlToTypeMapping(typeof(BusinessRulesConfig.TypeConfigParameter),
				"propertyInitializer");
			mapping.AddAttributeMapping("propertyName", "Name", typeof(string));
			mapping.AddAttributeMapping("value", "Value", typeof(string));
			return mapping;
		}
	}
}
