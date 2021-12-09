using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Xml;


namespace BioRad.Common.Utilities
{
	#region Documentation Tags
	/// <summary>
	/// Utility to handle multiple values for a single key in app.config file.
	/// </summary>
	/// <remarks>
	/// Update app.config file.
	/// <example>
	/// <code>
	/// <configuration>
	/// <configSections>
	///		<remove name="appSettings" />
	///		<section name="appSettings" type="BioRad.Common.Utilities.MultipleKeyValuesConfiguration, BioRad.Common" />
	/// </configSections>
	/// <appSettings>
	///		<add key="file" value="myfile1" />
	///		<add key="file" value="myfile2" />
	///		<add key="connectionString" value="my connection string" />
	///		<add key="another multiple values key" value="my value 1" />
	///		<add key="another multiple values key" value="my value 2" />
	///		<add key="another multiple values key" value="my value 3" />
	///	</appSettings>
	/// </configuration>
	///	</code>
	/// </example>	
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse</item>
	///			<item name="review">Last design/code review:3/1/2004, Pramod Walse</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">None</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\None">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: MultipleKeyValuesConfiguration.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Utilities/MultipleKeyValuesConfiguration.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class MultipleKeyValuesConfiguration : IConfigurationSectionHandler
	{
        #region Member Data
		/// <summary>
		/// Colletion Type.
		/// </summary>
		static Type readOnlyNameValueCollectionType = null;
		/// <summary>
		/// Colletion Constructor1.
		/// </summary>
		static ConstructorInfo readOnlyNameValueCollectionConstructor1 = null;
		/// <summary>
		/// Colletion Constructor2.
		/// </summary>
		static ConstructorInfo readOnlyNameValueCollectionConstructor2 = null;
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// Constructor.
		/// </summary>
		static MultipleKeyValuesConfiguration()
		{
			readOnlyNameValueCollectionType = typeof(NameValueCollection).
				Assembly.GetType("System.Configuration.ReadOnlyNameValueCollection");
			if (readOnlyNameValueCollectionType != null)
			{
				readOnlyNameValueCollectionConstructor1 = 
					readOnlyNameValueCollectionType.GetConstructor(
					new Type[] {readOnlyNameValueCollectionType});

				readOnlyNameValueCollectionConstructor2 = 
					readOnlyNameValueCollectionType.GetConstructor(
					new Type[] {typeof(IEqualityComparer), typeof(IComparer)});
			}
		}
        #endregion

        #region Methods
		/// <summary>
		/// Creates collection.
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		static NameValueCollection CreateCollection(object parent)
		{
			if (parent == null)
			{
				return 
					(NameValueCollection)readOnlyNameValueCollectionConstructor2.Invoke(
					new object[] {
									 StringComparer.InvariantCultureIgnoreCase,
									 new CaseInsensitiveComparer(
									 CultureInfo.InvariantCulture)});
			}
			else
			{
				return 
					(NameValueCollection)readOnlyNameValueCollectionConstructor1.Invoke(
					new object[] {parent});

			}
		}
		/// <summary>
		/// Gets Configuration Settings.
		/// </summary>
		/// <param name="sectionName"></param>
		/// <returns></returns>
		static NameValueCollection GetConfig(string sectionName)
		{
			return (NameValueCollection)ConfigurationManager.GetSection(sectionName);
		}
		/// <summary>
		/// Generates actual collection with multiple values.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="context"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object context, XmlNode section)
		{
			NameValueCollection collection = null;

			if (readOnlyNameValueCollectionType != null)
			{
				collection = CreateCollection(parent);
				foreach (XmlNode xmlNode in section.ChildNodes)
				{
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						switch (xmlNode.Name)
						{
							case "add":
								collection.Add(xmlNode.Attributes["key"].Value,
									xmlNode.Attributes["value"].Value);
								break;
							case "remove":
								collection.Remove(xmlNode.Attributes["key"].Value);
								break;
							case "clear":
								collection.Clear();
								break;
						}
					}
				}
			}
			return collection;
		}
        #endregion
	}
}
