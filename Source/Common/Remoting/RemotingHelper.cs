using System;
using System.Collections;
using System.Runtime.Remoting;

namespace BioRad.Common.Remoting
{
	#region Documentation Tags
	/// <summary>
	/// Per Ingo Rammer - Helper class to cash URLs of well-known
	/// client types. Supports getting a remote object by interface type.
	/// </summary>
	/// <remarks>
	/// Tested.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlgell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">1035</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: RemotingHelper.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Remoting/RemotingHelper.cs $</item>
	///			<item name="vssrevision">$Revision: 9 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class RemotingHelper
	{
		#region Constants
		#endregion

		#region Member Data
		/// <summary>
		/// Flag to force initialization on first GetObject access.
		/// </summary>
		private static bool m_IsInit;
		/// <summary>
		/// Dictionary of types pubishes by the server.
		/// </summary>
		private static IDictionary m_WellKnownTypes;
		#endregion

		#region Accessors
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors and Destructor
		#endregion

		#region Methods
		/// <summary>
		/// Static method configures .NET remoting.
		/// </summary>
		/// <param name="configurationFileName">Path name of the configuration file
		/// to be used to configure remoting.</param>
		public static void ConfigureRemoteConnection(string configurationFileName)
		{
			try
			{
				// This magic call for some inexplicable reason improves response
				// time of first DNS call. String value is irrelevant. Found on newsgroups.
				System.Configuration.ConfigurationManager.GetSection("DNS");
				// Configure the remoting connection.
				RemotingConfiguration.Configure(configurationFileName, true);
			}
			catch (Exception ex)
			{
				// Fix for defect #3511: clearer error message when remoting configuration fails
                throw new RemotingException
                    (StringUtility.FormatString(Properties.Resources.ConfigurationFailed), ex);
			}
		}

		/// <summary>
		/// Get a well known remote object by type. Supports access by interface.
		/// When accessing by interface, the server should publish only one type implementing
		/// a specific interface. Remote interfaces must be configured correctly.
		/// </summary>
		/// <param name="type">Type to get.</param>
		/// <returns>Reference to a remote object of the specified type.</returns>
		public static Object GetObject(Type type) 
		{
			// Initialize on first access
			if (! m_IsInit) InitTypeCache();
			// Find requested type in the dictionary
			WellKnownClientTypeEntry entr = (WellKnownClientTypeEntry) m_WellKnownTypes[type];
			if (entr == null) 
			{
				// Fix for defect #3511: clearer error message
                throw new RemotingException
                (StringUtility.FormatString(Properties.Resources.RequestUnknownObject_1, type.Name));
			}
			// Return a reference to the remote object
			return Activator.GetObject(entr.ObjectType,entr.ObjectUrl);
		}

		/// <summary>
		/// Fill dictionary with registered well-known remote types.
		/// </summary>
		public static void InitTypeCache() 
		{
			m_IsInit = true;
			m_WellKnownTypes= new Hashtable();
			// iterate on an array of registered well known types from remoting
			foreach (WellKnownClientTypeEntry entr in RemotingConfiguration.GetRegisteredWellKnownClientTypes()) 
			{
				// If no types are registered, entry is null
				if (entr != null)
				{
					m_WellKnownTypes.Add (entr.ObjectType,entr);
				}
			}
			
		}    
		#endregion

		#region Event Handlers
		#endregion
	}
}
