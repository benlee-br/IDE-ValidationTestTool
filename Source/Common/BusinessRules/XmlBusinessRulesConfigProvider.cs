using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BioRad.Common;
using BioRad.Common.Services;
using BioRad.Common.Utilities;
using BioRad.Common.Xml;

namespace BioRad.Common.BusinessRules
{
	#region Documentation Tags
	/// <summary>
	/// Deserializes business rules configuration information from an XML
	/// file.
	/// </summary>
	/// <remarks>
	/// Use FileName property to set XML configuration file name.
	/// Functional and unit tested.
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
	///			<item name="vssfile">$Workfile: XmlBusinessRulesConfigProvider.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/BusinessRules/XmlBusinessRulesConfigProvider.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class XmlBusinessRulesConfigProvider : AbstractXmlConfigurationProvider
	{
		#region Constructors and Destructor
		/// <summary>
		/// Default constructor. Configuration elements filename must be set 
		/// using property before GetConfigurationElements is called.
		/// </summary>
		public XmlBusinessRulesConfigProvider()
		{
		}

		/// <summary>
		/// Constructor to explicitly assign Filename.
		/// </summary>
		/// <param name="fileName">XML configuration file name.</param>
		public XmlBusinessRulesConfigProvider(string fileName)
		{
			this.FileName = fileName;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets business rule service configuration elements. 
		/// </summary>
		/// <returns>The configuration elements.</returns>
		public override ICollection GetConfigurationElements()
		{
			ArrayList configElements = new ArrayList();
			// Convert file name to valid path
			string fileName = this.GetValidPathForFile();
			try
			{
				// A FileStream is needed to read the XML document.
				using (Stream fs = FileCryptor.GetInstance.DecryptFileContentsToStream(fileName))
				{
					XmlReader reader = null;
					XmlDocument document = null;
					try
					{
						reader = new XmlTextReader(fs);
						document = new XmlDocument();
						document.Load(reader);
						XmlNode docElement = document.DocumentElement;
						XmlNodeList elements = docElement.SelectNodes("/business-rule-service/businessRules/businessRule");
						foreach (XmlNode node in elements)
						{
							BusinessRulesConfig.BusinessRuleConfig businessRuleConfig = 
								new BusinessRulesConfig.BusinessRuleConfig();

							businessRuleConfig.AssemblyName = 
								node.Attributes["assemblyName"] != null ? node.Attributes["assemblyName"].Value : null;
							businessRuleConfig.ClassName = 
								node.Attributes["className"] != null ? node.Attributes["className"].Value : null;
							businessRuleConfig.Identifier = 
								node.Attributes["identifier"] != null ? node.Attributes["identifier"].Value : null;
							//TypeConfig Parameters
							XmlNodeList paramElements = node.SelectNodes("constructorParameters/typeInitializer");
							ArrayList paramsList = new ArrayList();
							foreach(XmlNode param in paramElements)
							{
								BusinessRulesConfig.TypeConfig typeConfig = 
									new BusinessRulesConfig.TypeConfig();
								typeConfig.TypeName = 
									param.Attributes["typeName"] != null ? param.Attributes["typeName"].Value : null;
								typeConfig.Value = 
									param.Attributes["value"] != null ? param.Attributes["value"].Value : null;
								paramsList.Add(typeConfig);
							}
							businessRuleConfig.Initializers = (BusinessRulesConfig.TypeConfig[])
								paramsList.ToArray(typeof(BusinessRulesConfig.TypeConfig));
							//TypeConfigParameter Parameters
							paramElements = node.SelectNodes("propertyInitializers/propertyInitializer");
							paramsList = new ArrayList();
							foreach(XmlNode param in paramElements)
							{
								BusinessRulesConfig.TypeConfigParameter typeConfigParameter = 
									new BusinessRulesConfig.TypeConfigParameter();
								typeConfigParameter.Name = 
									param.Attributes["propertyName"] != null ? param.Attributes["propertyName"].Value : null;
								typeConfigParameter.Value = 
									param.Attributes["value"] != null ? param.Attributes["value"].Value : null;
								paramsList.Add(typeConfigParameter);
							}
							businessRuleConfig.Parameters = (BusinessRulesConfig.TypeConfigParameter[])
								paramsList.ToArray(typeof(BusinessRulesConfig.TypeConfigParameter));

							//Add configured object
							configElements.Add(businessRuleConfig);
						}
					}
					finally
					{
						// always make sure file is closed
						if(reader != null)
						{
							reader.Close();
						}
						document = null;
					}
				}
			}
			catch (Exception ex)
			{
				throw new ServiceLoadException(this,
                    StringUtility.FormatString(Properties.Resources.ConfigLoadFailure_1, 
					new object[]{this.FileName}
					), ex);
			}
			return configElements.ToArray(typeof(BusinessRulesConfig.BusinessRuleConfig));
		}
		#endregion
	}
}
