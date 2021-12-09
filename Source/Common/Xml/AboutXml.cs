using System;

namespace BioRad.Common.Xml
{
	#region Documentation Tags
	/// <summary>
	/// <p>
	/// The BioRad.Common.Xml namespace contains classes designed to make it easy to serialize objects 
	/// to and from xml. The .NET framework contains a number of useful classes for dealing 
	/// with xml, some of which overlap or provide multiple ways of doing the same thing. 
	/// </p>
	/// <p>
	/// In order to standardize access to xml files, all xml serialization should use the classes 
	/// in this namespace.
	/// </p>
	/// </summary>
	/// <remarks>
	/// <p>Some of the most powerful xml tools in the .NET framework are the xml serialization 
	/// classes. These classes (most of which are in the "System.Xml.Serialization" namespace) 
	/// provide a relatively easy way to tag classes for xml serialization, as well as 
	/// powerful control over the xml format. One major problem with this method of handling 
	/// serialization is that it relies heavily on the use of attribute tags. To mark a class 
	/// for serialization, you mark it and its fields with serialization-specific attributes. 
	/// This has several problems:</p>
	/// <list type="number">
	/// <item>The class being marked up becomes coupled to the serialization classes. This is 
	/// not at all appropriate for use within the core library, since the persistence of all 
	/// objects is required to be completely decoupled from those objects by the persistence 
	/// framework.</item>
	/// <item>A whole slew of attributes in a large class becomes very hard to manage and 
	/// maintain. To change serialization options, you have sift through the entire class, 
	/// including its normal functionality unassociated with serialization.</item>
	/// </list>
	/// <p>
	/// The first problem is by far the more important. It would seem to prevent the xml 
	/// serialization features from being a valid option, which is a shame because they provide 
	/// so many useful utilities.
	/// </p>
	/// <p>
	/// There is, however, a way to combine the power of the .NET xml serialization features with 
	/// a decoupled approach. Use of serialization attributes to control xml serialization really 
	/// means informing the compiler to do some behind-the-scenes work in instantiating the actual 
	/// code that will do the serialization. Every code attribute used refers to an instance of 
	/// an "Attribute" object containing the specifics of that attribute. .NET provides a way to 
	/// create a set of code attribute objects from code rather than from markup, and then use them 
	/// to override the default serialization using a class called "XmlAttributeOverrides". This is 
	/// a collection of serialization-specific "Attribute" instances that are used with the 
	/// XmlSerializer to serialize and deserialize to and from xml. The original purpose of this 
	/// feature was to allow xml serialization to be used with classes for which you didn't have 
	/// source code (and thus couldn't mark up with attributes).</p>
	/// <p>
	/// The BioRad.Common.Xml classes provide an easy means to access this functionality, greatly 
	/// simplifying the process of mapping a type to its xml representation. There are three 
	/// main classes:</p>
	/// <list type="bullet">
	///		<item>IXmlToFieldMapping: implementations of this interface define how a given field 
	///		on a type should be mapped to xml. Current options include mapping the field to an 
	///		element, to an xml attribute, collection mapping, and "ignore" mapping (basically 
	///		turning the field off for serialization).
	///		</item>
	///		<item>XmlToTypeMapping: the class that handles the mapping of a type. It holds a set 
	///		of IXmlToFieldMapping instances that correspond to the fields on the type, as well as 
	///		information about the name of the xml element representing the type.
	///		</item>
	///		<item>XmlToTypeSerializer: The class that handles serialization. This is what writes 
	///		to and reads from files, using an XmlToTypeMapping.
	///		</item>
	/// </list>
	/// <p>
	/// Note that the somewhat awkward "XmlToType" and "XmlToField" prefixes were used to avoid 
	/// collision with the .NET framework class "XmlTypeMapping".</p>
	/// <p>
	/// Using the classes is quite easy. To use them, you
	/// </p>
	/// <list type="number">
	/// <item>Define a type mapping for the type you want to serialize, mapping out its fields.</item>
	/// <item>Define type mappings for nested types and add them to the root type mapping.</item>
	/// <item>Create an instance of XmlToTypeSerializer using the root type mapping and use it to 
	/// serialize or deserialize.</item>
	/// </list>
	/// <p>The following example shows how a simple class, "Menu", with a collection of "MenuItem" 
	/// objects, would be serialized.</p>
	/// <code>
	/// // create menu mapping
	/// XmlToTypeMapping menuMapping = new XmlToTypeMapping(typeof(Menu), "menu");
	/// menuMapping.AddAttributeMapping("day", "Day", typeof(MenuDayEnum));
	/// menuMapping.AddCollectionMapping("menuItems", "MenuItems", "menuItem", typeof(MenuItem));
	/// // create menu item mapping
	/// XmlToTypeMapping menuItemMapping = new XmlToTypeMapping(typeof(MenuItem), "menuItem");
	/// menuItemMapping.AddAttributeMapping("foodType", "FoodType", typeof(MenuItem.FoodTypeEnum));
	/// menuItemMapping.AddAttributeMapping("description", "Description", typeof(string));
	/// menuItemMapping.AddAttributeMapping("calorieCount", "CalorieCount", typeof(int));
	/// // add menu item mapping to menu mapping
	/// menuMapping.AddNestedTypeMapping(menuItemMapping);
	/// // create a serializer
	/// XmlToTypeSerializer s = new XmlToTypeSerializer(menuMapping);
	/// // deserialize to read from xml
	/// Menu menu = (Menu) s.Deserialize("my-menu-file.xml");
	/// // change the menu
	/// menu.Day = MenuDayEnum.Saturday;
	/// // serialize to write to xml
	/// s.Serialize(menu, "my-changed-menu-file.xml");
	/// </code>
	/// <p>
	/// This is a lot easier than dealing with the attributes directly, and certainly easier than dealing 
	/// with an xml DOM (document object model). It is also fully decoupled from the Menu classes
	/// themselves. They don't need to know anything at all about xml or serialization.
	/// </p>
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
	///			<item name="vssfile">$Workfile: AboutXml.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Xml/AboutXml.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public sealed partial class AboutXml
	{
	}
}
