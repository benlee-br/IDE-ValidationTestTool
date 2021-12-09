using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using BioRad.Common.Reflection;
using BioRad.Common.Services;
using BioRad.Common.Services.Config;

namespace BioRad.Common.BusinessRules
{
	#region Documentation Tags
	/// <summary>
	/// Configuration type for a business rules service. Defines an array of
	/// configuration elements each of which supply information and methods to construct a
	/// business rule. Rule constructor parameters and initialized properties must be
	/// type-convertible from a string value.
	/// </summary>
	/// <remarks>
	/// Supports deserialization with XML attributes.
	/// Unit tested and functional.
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
	///			<item name="vssfile">$Workfile: BusinessRulesConfig.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/BusinessRules/BusinessRulesConfig.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class BusinessRulesConfig
	{
		#region Contained Classes
		/// <summary>
		/// Business rule configuration object.
		/// Supplies information and methods to construct a particular
		/// business rule, which can be any type fully defined using initializers and
		/// accessors.
		/// </summary>
		public class BusinessRuleConfig
		{
			#region	Member Data
			private	string m_AssemblyName;
			private	string m_ClassName;
			private	string m_Identifier;
			private	TypeConfig[] m_Initializers;
			private	TypeConfigParameter[] m_Parameters;
			/// <summary>
			/// Application utility that instantiates classes by name, 
			/// using reflection.
			/// </summary>
			private ReflectionUtil m_Util = new ReflectionUtil(true);
			#endregion

			#region	Accessors
			/// <summary>
			/// Identifier name must be unique within the service, and is used by the 
			/// service to access this business rule.
			/// </summary>
			public string Identifier
			{
				get{return this.m_Identifier;}
				set{this.m_Identifier = value;}
			}

			/// <summary>
			/// Assembly name is locates class to instantiate.
			/// </summary>
			public string AssemblyName
			{
				get{return this.m_AssemblyName;}
				set{this.m_AssemblyName	= value;}
			}

			/// <summary>
			/// Class name defines type to instantiate.
			/// </summary>
			public string ClassName
			{
				get{return this.m_ClassName;}
				set{this.m_ClassName = value;}
			}

			/// <summary>
			/// List of initializers used by the service for parameterized construction
			/// of an instantiated object.
			/// </summary>
			public TypeConfig[]	Initializers
			{
				get{return this.m_Initializers;}
				set{this.m_Initializers =	value;}
			}

			/// <summary>
			/// List of parameters representing types used by the service
			/// to initialize properties in the instantiated object.
			/// </summary>
			public TypeConfigParameter[] Parameters
			{
				get{return this.m_Parameters;}
				set{this.m_Parameters = value;}
			}

			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Default constructor is for use by configuration providers.
			/// </summary>
			public BusinessRuleConfig()
			{
			}

			/// <summary>
			/// Constructor used to explicitly create a business rule configuration element.
			/// </summary>
			/// <param name="identifier">Business rule will be accessed by this identifier</param>
			/// <param name="assemblyName">Assembly where class is located</param>
			/// <param name="className">Class name for object creation</param>
			/// <param name="initializers">Object parameterized contructor initializers</param>
			/// <param name="parameters">Object property initializers</param>
			public BusinessRuleConfig(string identifier, string assemblyName, string className, 
				TypeConfig[] initializers, TypeConfigParameter[] parameters)
			{
				this.Identifier = identifier;
				this.AssemblyName = assemblyName;
				this.ClassName = className;
				this.Initializers  = initializers;
				this.Parameters = parameters;
			}
			#endregion

			#region Methods
			/// <summary>
			/// Get initializers as objects. May throw exception if type conversion
			/// fails.
			/// </summary>
			/// <returns>array of initializer objects or null if no initializers</returns>
			object [] GetInitializerObjects()
			{
				if ((this.Initializers == null) || (this.Initializers.GetLength(0) == 0))
				{
					return null;
				}
				else
				{
					object[] objs = new object[this.Initializers.GetLength(0)];
					int destIndex = 0;
					for (int index = this.Initializers.GetLowerBound(0); index <= this.Initializers.GetUpperBound(0); index++)
					{
						objs[destIndex++] = this.Initializers[index].GetObject();
					}
					return objs;
				}
			}

			/// <summary>
			/// Instantiate and initialize the business rule defined by this object.
			/// May throw exception if rule cannot be instantiated or initialized.
			/// The rule identifier should be set by the caller.
			/// </summary>
			/// <returns>a business rule</returns>
			public IBusinessRule GetBusinessRule()
			{ 
				// Instantiate the rule
				// TODO: Should failure to cast to IBusinessRule throw a custom exception?
				object [] objs = this.GetInitializerObjects();
				IBusinessRule rule;
				if (objs == null)
				{
					// instantiate with default constructor
					rule = (IBusinessRule) m_Util.InstantiateObject(
						this.ClassName, this.AssemblyName);
				}
				else
				{
					// Instantiate with parameterized constructor
					rule = (IBusinessRule) m_Util.InstantiateObject(
						this.ClassName, 
						this.AssemblyName, 
						objs, 
						true
						);
				}
				// Initialize rule properties, if any
				if ((this.Parameters != null) && (this.Parameters.Length != 0))
				{
					foreach (TypeConfigParameter configParameter in this.Parameters)
					{
						// TODO: Should a single parameter setting failure cause all to fail?
						configParameter.SetParameter(rule);
					}
				}
				// Rule identifier is left to be set by the service
				return rule;
			}
			#endregion

		}

		/// <summary>
		/// Configuration type for creating an object.
		/// </summary>
		public class TypeConfig
		{
			#region	Member Data
			private	string m_Value;
			private	string m_TypeName;
			#endregion

			#region	Accessors
			/// <summary>
			/// Value must be type-convertible from string to destination type.
			/// Destination type will determined by property value is assigned to,
			/// if applicable, otherwise by optional type.
			/// </summary>
			public string Value
			{
				get{return this.m_Value;}
				set{this.m_Value	= value;}
			}

			/// <summary>
			/// Type name is used to select a type converter for the value.
			///	When value is used as a contructor parameter type is indeterminate 
			///	and must be specified.
			/// </summary>
			public string TypeName
			{
				get{return this.m_TypeName;}
				set{this.m_TypeName = value;}
			}
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Default constructor is for use by configuration providers.
			/// </summary>
			public TypeConfig()
			{
			}

			/// <summary>
			/// Constructor used to explicitly create type configuration element.
			/// Type conversion will be determined by the destination type.
			/// </summary>
			/// <param name="typeValue">string representation of type value</param>
			public TypeConfig(string typeValue)
			{
				this.Value = typeValue;
			}

			/// <summary>
			/// Constructor used to explicitly create type configuration element.
			/// </summary>
			/// <param name="typeValue">string representation of type value</param>
			/// <param name="typeName">type name used to specify type conversion</param>
			public TypeConfig(string typeValue, string typeName)
			{
				this.TypeName  = typeName;
				this.Value = typeValue;
			}
			#endregion

			#region Methods
			/// <summary>
			/// Converts value/type pair to an object of the specified type with the
			/// specified value using the invariant culture.
			/// </summary>
			/// <returns>type-converted object</returns>
			public object GetObject()
			{
				if ((this.TypeName != null) && (this.TypeName != String.Empty))
				{
					// Get "convert-to" type from type name - must be a system type
					Type type  =  Type.GetType(this.TypeName);
					// Convert value to given system type using the invariant culture
					object obj = Convert.ChangeType(this.Value, type, CultureInfo.InvariantCulture);
					return obj;
				}
					// Return value as a string if no type name
				else return this.Value;
			}
			#endregion
		}

		/// <summary>
		/// Configuration type for initializing an object property.
		/// </summary>
		public class TypeConfigParameter
		{
			#region	Member Data
			private	string m_Name;
			private	string m_Value;
			#endregion

			#region	Accessors
			/// <summary>
			/// Name identifies property.
			/// </summary>
			public string Name
			{
				get{return this.m_Name;}
				set{this.m_Name = value;}
			}

			/// <summary>
			/// Value must be type-convertible from string to destination type.
			/// Destination type will determined by type of property the value is assigned to.
			/// </summary>
			public string Value
			{
				get{return this.m_Value;}
				set{this.m_Value	= value;}
			}
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Default constructor is for use by configuration providers.
			/// </summary>
			public TypeConfigParameter()
			{
			}

			/// <summary>
			/// Constructor used to explicitly create type configuration element.
			/// Destination property type will determine type conversion of value.
			/// </summary>
			/// <param name="name">Name of property to initialize</param>
			/// <param name="typeValue">string representation of type value</param>
			public TypeConfigParameter(string name, string typeValue)
			{
				this.Name = name;
				this.Value = typeValue;
			}
			#endregion

			#region Methods
			/// <summary>
			/// Sets parameter on given object. Uses type converter for the destination
			/// type if one exists, otherwise conversion is performed using IConvertible.
			/// If no conversion is possible, attempts to set parameter as a string.
			/// May throw exceptions if property does not exist on object or if type
			/// conversion fails.
			/// </summary>
			/// <param name="obj">object with property to set</param>
			public void SetParameter(object obj)
			{
				PropertyInfo pi = obj.GetType().GetProperty(this.Name);
				object setValue = null;
				// If value is directly assignable, don't bother with type converter
				// or IConvertible. This is much more efficient.
				if (pi.PropertyType.IsAssignableFrom(this.Value.GetType()))
				{
					setValue = this.Value;
				}
				else
				{
					// Look for a type converter associated with the property type
					// TODO: It would be more efficient to use IConvertible - should that
					// be first default?
					TypeConverter typeConverter = TypeDescriptor.GetConverter(pi.PropertyType);
					if (typeConverter != null)
					{
						try
						{
							setValue = typeConverter.ConvertFromInvariantString(this.Value);
						}
						catch
						{
							// Ignore conversion errors from type converter, fall through
							// to using IConvertable interface.
							setValue = null;
						}
					}
					if (setValue == null)
					{
						try
						{
							setValue = Convert.ChangeType(this.Value, pi.PropertyType, CultureInfo.InvariantCulture);
						}
						catch
						{
							// ignore errors on type conversion. SetValue will
							// be left as a string.
							setValue = this.Value;
						}
					}
				}
				// Set property value using type-converted object.
				// If value type doesn't match target, this will attempt to
				// convert using default Binder.ChangeType
				pi.SetValue(obj, setValue, BindingFlags.Default, null, null, CultureInfo.InvariantCulture);
			}
			#endregion
		}
		#endregion

		#region	Member Data
		/// <summary>
		/// Array of service configuration elements.
		/// </summary>
		private BusinessRuleConfig[] m_BusinessRuleConfigs;
		#endregion

		#region	Accessors
		/// <summary>
		/// Array of service configuration elements.
		/// </summary>
		public BusinessRuleConfig[] BusinessRuleConfigs
		{
			get{return this.m_BusinessRuleConfigs;}
			set{this.m_BusinessRuleConfigs = value;}
		}
		#endregion

		#region Methods
		#endregion
	}
}
