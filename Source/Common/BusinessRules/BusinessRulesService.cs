using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using BioRad.Common;
using BioRad.Common.Reflection;
using BioRad.Common.Services;

namespace BioRad.Common.BusinessRules
{
	#region Documentation Tags
	/// <summary>
	/// Base class for services supplying business rules. Exposes a rules collection
	/// which contains rule objects constructed using loaded configuration objects.
	/// </summary>
	/// <remarks>
	/// Unit tested and functional. When rules are accessed via an enum, the enum
	/// is used to construct an identifier of the form enum-type-name_enum-string-value.
	/// Derived services which define enum accessors must ensure that the enum type name
	/// is globally unique so as to prevent collisions.
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
	///			<item name="vssfile">$Workfile: BusinessRulesService.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/BusinessRules/BusinessRulesService.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class BusinessRulesService : AbstractService, IBusinessRulesService
	{
		#region Contained Classes
		#region Documentation Tags
		/// <summary>
		/// Type allowing business rules in the containing class to be accessed via
		/// enum. Can throw ArgumentException.
		/// </summary>
		/// <remarks>
		/// Although this allows derived types to expose Rules collection, as a contained
		/// type it can't be put into an interface without exposing private members of
		/// the BusinessRulesService. Rules are exposed directly by service,
		/// it may not be useful to package collection as a contained collection.
		/// </remarks>
		#endregion
		protected class RulesCollection
		{
			#region Member Data
			/// <summary>
			/// The containing object.
			/// </summary>
			private readonly BusinessRulesService m_Config;
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Constructor for the group collection class.
			/// </summary>
			/// <param name="config">Containing object.</param>
			internal RulesCollection(BusinessRulesService config)
			{
				m_Config = config; 
			}
			#endregion

			#region Accessors					
			/// <summary>
			/// Indexer to the rules collection in the containing object,
			/// keyed by enum. Can throw ArgumentException.
			/// </summary>
			public object this[Enum key, Type type] 
			{
				get 
				{ 
					if (!m_Config.m_Rules.ContainsKey(GetKey(key)))
					{
						// throw an error if rule is not found in service
                        throw new ArgumentException(StringUtility.FormatString(Properties.Resources.NoRule_1,
                            new object[] { GetKey(key) }), "key");
					}
					object obj = m_Config.m_Rules[GetKey(key)];
					if (type.IsAssignableFrom(obj.GetType()))
					{
						return obj;
					}
					else
					{
						// throw an error if rule is not of correct type
                        throw new ArgumentException(StringUtility.FormatString(Properties.Resources.InvalidRuleType_1,
                            new object[] { GetKey(key) }), "type");
					}
				}
			}

			/// <summary>
			/// Get the count of groups in the containing object.
			/// </summary>
			public int Count 
			{
				get 
				{ 
					return m_Config.m_Rules.Count; 
				}
			}
			#endregion

			#region Methods
			/// <summary>
			/// Dispose of all contained rules and clear the collection.
			/// </summary>
			internal void Clear()
			{
				foreach (object rule in this)
				{
					if (rule is IDisposable)
					{
						((IDisposable)rule).Dispose();
					}
				}
				m_Config.m_Rules.Clear();
			}

			/// <summary>
			/// Query whether indicated business rule is available from this
			/// service.
			/// </summary>
			/// <param name="key">rule identifier</param>
			/// <returns>true if business rule is in rules collection</returns>
			public bool ContainsKey(Enum key) 
			{
				return m_Config.m_Rules.ContainsKey(GetKey(key)); 
			}

			/// <summary>
			/// Enumeration support.
			/// </summary>
			/// <returns>An enumerator for the rules collection.</returns>
			public IEnumerator GetEnumerator()
			{
				return m_Config.m_Rules.Values.GetEnumerator();
			}

			/// <summary>
			/// Converts key to string accessor for collection.
			/// Accessor is enum-type-name_enum-string-name.
			/// </summary>
			/// <param name="key">rule enumeration</param>
			/// <returns>string equivalent</returns>
			public string GetKey(Enum key)
			{
				return String.Format("{0}_{1}", key.GetType().Name, key.ToString());
			}
			#endregion
		}		
		#endregion

        #region Member Data
		/// <summary>
		/// Application utility that instantiates classes by name, 
		/// using reflection.
		/// </summary>
		private ReflectionUtil m_Util = new ReflectionUtil(true);
		/// <summary>
		/// Collection of all loaded configuration elements.
		/// </summary>
		private ArrayList m_ConfigurationElements = new ArrayList();
		/// <summary>
		/// Indexed rules collection property - may be indexed by enum and type. 
		/// Returns null if no match found.
		/// </summary>
		protected readonly RulesCollection Rules;
		/// <summary>
		/// A collection of all configured rules, keyed by string value of enum.
		/// </summary>
		private Hashtable m_Rules = new Hashtable();
		#endregion

        #region Accessors
		/// <summary>
		/// Indexer to the rules collection keyed by enum. Can throw ArgumentException if 
		/// rule not found or if rule is not assignable to IBusinessRule type.
		/// </summary>
		public object this[Enum key] 
		{
			get { return this[key, typeof(IBusinessRule)]; }
		}

		/// <summary>
		/// Indexer to the rules collection keyed by enum. 
		/// Can throw ArgumentException if rule is not found or if rule is not assignable to 
		/// given type. If rule implements ICloneable, a clone of the rule is returned.
		/// </summary>
		public object this[Enum key, Type type]
		{
			get
			{
				object rule = Rules[key, type];
				// clone rule if possible
				if (rule is ICloneable)
				{
					return ((ICloneable) rule).Clone();
				}
				else
				{
					return rule;
				}
			}
		}

			/// <summary>
		/// Public access to the rules names
		/// </summary>
		public ICollection RulesNames
		{
			get
			{
				return m_Rules.Keys;
			}
		}
		#endregion

        #region Constructors and Destructor
		/// <summary>
		/// Default constructor initializes rules collection. Collection is empty
		/// until loaded.
		/// </summary>
		public BusinessRulesService()
		{
			Rules = new RulesCollection(this);
		}
		/// <summary>
		/// Finalize method - for garbage collection.
		/// </summary>
		~BusinessRulesService()
		{
			Dispose(false);
		}
		#endregion

        #region Methods
		/// <summary>
		/// Add a rule to collection with given enumeration key.
		/// </summary>
		/// <param name="key">key for the given rule in the collection</param>
		/// <param name="rule">rule to add to collection (may be any type)</param>
		protected void Add(Enum key, object rule)
		{
			m_Rules.Add (Rules.GetKey(key), rule);
		}

		/// <summary>
		/// Add a rule to collection with given string key.
		/// </summary>
		/// <param name="key">key for the given rule in the collection</param>
		/// <param name="rule">rule to add to collection (may be any type)</param>
		private void Add(string key, object rule)
		{
			m_Rules.Add(key, rule);
		}

		/// <summary>
		/// Explicitly dispose contained rules.
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// Explicitly dispose contained rules.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		/// <summary>
		/// Dispose contained rules when explicitly disposing.
		/// Override to clean up a derived type. Call base method if overriden.
		/// </summary>
		/// <param name="disposing">True if explictly disposing</param>
		protected virtual void Dispose(bool disposing)
		{
			lock (this)
			{
				// When explicitly disposing, clear rules collection.
				// This will dispose all contained rules.
				if (disposing)
				{
					if (m_Rules != null)
					{
						Rules.Clear();
						m_Rules = null;
					}
				}
			}
		}

		/// <summary>
		/// Enumeration support.
		/// </summary>
		/// <returns>An enumerator for the configured rules collection.</returns>
		public IEnumerator GetEnumerator()
		{
			return Rules.GetEnumerator();
		}

		/// <summary>
		/// Test if configured rules collection contains an item identified by key.
		/// </summary>
		/// <param name="key">rule enumeration identifier</param>
		/// <returns></returns>
		public bool ContainsKey(Enum key) 
		{
			return Rules.ContainsKey(key); 
		}

		/// <summary>
		/// Load service. Clears rules collection then instantiates rules using
		/// configuration elements and adds them to the service. Attempts to instantiate
		/// all rules but will afterwards throw ServiceLoadException if a rule fails to 
		/// instantiate.
		/// </summary>
		/// <param name="configurationElements">collection of BusinessRuleConfig objects</param>
		public override void Load(ICollection configurationElements)
		{
			ServiceLoadException ex = null;
			Rules.Clear();
			// Create rules using configuration elements, and add
			// to rules collection
			foreach (BusinessRulesConfig.BusinessRuleConfig config in configurationElements)
			{
				try
				{
					// Create business rule from configuration
					IBusinessRule rule = config.GetBusinessRule();
					// Set the identifier in the rule
					SetIdentifier(ref rule, config.Identifier);
					// Add the rule to the service to be accessed by identifier
					this.Add(rule.Identifier, rule);
				}
				catch( Exception e)
				{
					// Save exception to be re-thrown after all business rules are
					// loaded
					ex = new ServiceLoadException(this,
                        StringUtility.FormatString(Properties.Resources.ServiceLoadFailure_2,
						new object [] { this.GetType().Name, config.ClassName}), e);
				}
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Derived services can use this method to convert a defined Enum
		/// to an identifier when overriding SetIdentifier. Left public for testing.
		/// </summary>
		/// <param name="key">Enum rule selector</param>
		/// <returns>String value the base service will use to access the rule</returns>
		public string GetIdentifierFromKey(Enum key)
		{
			return this.Rules.GetKey(key);
		}

		/// <summary>
		/// Sets string identifier in a rule. Derived types override this method to
		/// validate and/or manipulate identifer before the rule is loaded into the
		/// service. Identifier will be used to access rule from service.
		/// This implementation just assigns identifier directly to rule Identifier
		/// property.
		/// </summary>
		/// <param name="rule">reference to rule in which to set identifier</param>
		/// <param name="identifier">identifier as read from Xml file</param>
		protected virtual void SetIdentifier(ref IBusinessRule rule, string identifier)
		{
			// Derived types may wish to validate identifier against
			// allowed enum list.
			rule.Identifier = identifier;
		}
		#endregion
	}
}
