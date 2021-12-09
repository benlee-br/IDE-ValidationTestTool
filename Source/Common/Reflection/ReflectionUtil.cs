using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.DiagnosticsLogger;
using BioRad.Common.Services.Config;

namespace BioRad.Common.Reflection
{
	#region Documentation Tags
	/// <summary>
	/// Utility class for working with the service framework.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Drew McAuliffe</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: ReflectionUtil.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Reflection/ReflectionUtil.cs $</item>
	///			<item name="vssrevision">$Revision: 11 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ReflectionUtil
	{
		#region Member Data
		/// <summary>
		/// Flag that indicates if exceptions caused by setting properties will be
		/// ignored or raised.
		/// </summary>
		private bool m_failOnPropertyError = true;
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public ReflectionUtil(){}
		/// <summary>
		/// Constructor that sets whether errors should be raised on 
		/// property access errors.
		/// </summary>
		/// <param name="failOnPropertyError">Flag indicating that errors
		/// accessing properties should be raised.</param>
		public ReflectionUtil(bool failOnPropertyError)
		{
			this.m_failOnPropertyError = failOnPropertyError;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Instantiates an object using the activator.
		/// </summary>
		/// <param name="className">Name of the class to instantiate</param>
		/// <param name="assemblyName">Name of the assembly containing the class (without ".dll")</param>
		/// <returns>Instantiated class</returns>
		public object InstantiateObject(string className,
			string assemblyName)
		{
			ObjectHandle handle = 
				Activator.CreateInstance(assemblyName, className);
			return handle.Unwrap();
		}

		/// <summary>
		/// Instantiates an object with a parameterized constructor using the activator.
		/// </summary>
		/// <param name="className">Name of the class to instantiate</param>
		/// <param name="assemblyName">Name of the assembly containing the class (without ".dll")</param>
		/// <param name="args">An array of arguments that match in number, order, and type
		///  the parameters of the constructor to invoke. If args is an empty array or a
		///  null reference the default constructor is invoked. Null arguments default to
		///  type Object.</param>																																																																		/// <returns>Instantiated class</returns>
		/// <param name="fallbackToDefaultConstructor">When true, if no constructor matches
		///  argument list object is created using the default constructor, if any.</param>
		public object InstantiateObject(string className,
			string assemblyName,
			object[] args,
			bool fallbackToDefaultConstructor)
		{
			Type[] argsTypes = null;
			if (fallbackToDefaultConstructor)
			{
				// Create array of argument types, converting null arguments explictly
				// to type "object". Only used when testing for fall-back to default
				// constructor.
				argsTypes = new Type[args.Length];
				for (int i = 0; i< args.Length; i++)
				{
					if (args[i] != null)
					{
						argsTypes[i] = args[i].GetType();
					}
					else
					{
						// Note that a constructor will not be found for these
						// arguments unless constructor parameters are explicitly
						// of type "object"
						argsTypes[i] = typeof(Object);
					}
				}
			}
			return this.InstantiateObject(
								className, 
								assemblyName, 
								args, 
								argsTypes, 
								fallbackToDefaultConstructor);
		}

		/// <summary>
		/// Instantiates an object with a parameterized constructor using the activator.
		/// Use this type when fallback to default constructor is enabled but one or
		/// more args may be null.
		/// </summary>
		/// <param name="className">Name of the class to instantiate</param>
		/// <param name="assemblyName">Name of the assembly containing the class (without ".dll")</param>
		/// <param name="args">An array of arguments that match in number, order, and type
		///  the parameters of the constructor to invoke. If args is an empty array or a
		///  null reference the default constructor is invoked.</param>																																																																		/// <returns>Instantiated class</returns>
		/// <param name="argsTypes">An array of argument types corresponding to the given
		/// argument array. Must match in number with args array. Used only when fallback
		/// to default constructor is enabled.</param>																																																																		/// <returns>Instantiated class</returns>
		/// <param name="fallbackToDefaultConstructor">When true, if no constructor matches
		///  argument list object is created using the default constructor, if any.</param>
		public object InstantiateObject(string className,
			string assemblyName,
			object[] args,
			Type[] argsTypes,
			bool fallbackToDefaultConstructor)
		{
			// returned object
			Object obj = null;
			// Create a type object for the activator
			Type type = Type.GetType(Assembly.CreateQualifiedName(assemblyName,className));
			// Check for matching constructor and fall back to default constructor if enabled
			if (fallbackToDefaultConstructor)
			{
				// Check for existance of matching constructor
				if (type.GetConstructor(argsTypes) == null)
				{
					// No matching constructor; use default constructor
					obj = Activator.CreateInstance(type);
				}
			}
			if (obj == null)
			{
				// No fall back constructor, this will throw an exception if argument list
				// does not match a public constructor
				obj =  Activator.CreateInstance(type, args);
			}
			return obj;
		}

		/// <summary>
		/// Uses reflection to set an object's properties using
		/// the values of a supplied set of app service parameters.
		/// Properties are set where the parameter name matches the
		/// property's name (and a set accessor exists). Uses InvariantCulture
		/// for type conversion.
		/// <para>
		/// Type conversion is perfomed as follows:
		/// <list type="numbered">
		/// <item>If parameter defines a valid TypeName this type is used for type conversion, 
		/// if not the type of the target property is used.</item>
		/// <item>If a type converter exists for target type, it's ConvertFromInvariantString method
		/// is used to convert the parameter Value to the target type</item>
		/// <item>If no type converter exists for the target type, or if conversion using the
		/// type converter fails, System.Convert.ChangeType method is used to convert the 
		/// parameter Value to the target type, if possible.</item>
		/// <item>Finally, the converted value (or, if all conversion attempts have failed, the
		/// Value string), is assigned to the target property using PropertyInfo.SetValue using
		/// the default Binder and the InvariantCulture.</item>
		/// </list>
		/// </para>
		/// </summary>
		/// <param name="objWithProperties">Object to set properties on</param>
		/// <param name="parameters">array defining name/value pairs and optional type name for each property to set.</param>
		public void SetPropertiesWithParameters(
			object objWithProperties,
			ServiceConfigParameter[] parameters)
		{
			if (parameters != null)
			{
				Type objType = objWithProperties.GetType();
				foreach(ServiceConfigParameter param in parameters)
				{
					// set each property in the object using reflection	
					try
					{
						Type propertyType = null;
						object setValue = null;
						PropertyInfo pi = objType.GetProperty(param.Name);
						// Assign property directly from string if possible. This is
						// much faster than using type converter or IConvertible.
						if (pi.PropertyType.IsAssignableFrom(param.Value.GetType()))
						{
							setValue = param.Value;
						}
						else
						{
							// Type convert parameter value before assignment
							// Use explictly-defined type if its provided. This must
							// be a system type.
							if (param.TypeName != null)
							{
								// Convert-to type defined explicitly
								propertyType =  Type.GetType(param.TypeName);
							}
							// If no explicitly defined type is provided use type of
							// destination property
							if (propertyType == null)
							{
								// Convert-to type determined by property type
								propertyType = pi.PropertyType;
							}
							// Look for a type converter associated with the property type
							// TODO: It would be more efficient to use IConvertible - should that
							// be first default?
							TypeConverter typeConverter = TypeDescriptor.GetConverter(propertyType);
							if (typeConverter != null)
							{
								try
								{
									setValue = typeConverter.ConvertFromInvariantString(param.Value);
								}
								catch
								{
									// Ignore conversion errors from type converter, fall through
									// to using IConvertable interface.
								}
							}
							if (setValue == null)
							{
								try
								{
									setValue = Convert.ChangeType(param.Value, propertyType, CultureInfo.InvariantCulture);
								}
								catch
								{
									// ignore errors on type conversion. SetValue will
									// be left as a string.
									setValue = param.Value;
								}
							}
						}
						// Set property value using type-converted object.
						// If value type doesn't match target, this will attempt to
						// convert using default Binder.ChangeType
						pi.SetValue(objWithProperties, setValue, BindingFlags.Default, null, null, CultureInfo.InvariantCulture);
					}
					catch (Exception ex)
					{
						string sr =
                            StringUtility.FormatString(Properties.Resources.SetPropertiesWithParametersFailed_3,
							param.Name, 
							param.Value, 
							objWithProperties.GetType().FullName
							); 
						// TODO: remove debug trace
						Debug.WriteLine(sr);
						Debug.WriteLine(ex.ToString());
						if (m_failOnPropertyError)
						{
							// TODO: ReflectionException should be loggable app exception
							throw new ReflectionException(sr, ex);
						}
						else
						{
							// Log only
							// TODO: when replaced by loggable ReflectionException, move up
							// and throw only when enabled.
							LoggableApplicationException logEx = 
								new LoggableApplicationException(
								this, 
								DiagnosticSeverity.Warning, 
								DiagnosticTag.EXCEPTION, 
								sr);
						}
					}
				}
			}
		}
		#endregion
	}
}
