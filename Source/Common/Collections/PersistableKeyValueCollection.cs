using System;
using System.Data;
using System.IO;

namespace BioRad.Common.Collections
{
	#region Documentation Tags
	/// <summary>
	/// This class read and write to a file of key/value pairs.
	/// The file exists in the same directory as the executable
	/// (I.E. AppDomain.CurrentDomain.BaseDirectory).
	/// </summary>
	/// <remarks>
	/// <example>
	/// <code>
	/// private void LoadSettings() 
	/// { 
	/// 	PersistableKeyValueCollection myConfig = new PersistableKeyValueCollection("myAppSettings.xml"); 
	/// 	string lastuser = myConfig.GetOption("lastuser"); 
	/// } 
	/// private void SaveSettings() 
	/// { 
	/// 	PersistableKeyValueCollection myConfig = new PersistableKeyValueCollection("myAppSettings.xml"); 
	/// 	myConfig.SetOption("lastuser", "joe"); 
	/// 	myConfig.Store("myAppSettings.xml"); 
	/// } 
	///	</code>
	/// </example>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: PersistableKeyValueCollection.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Collections/PersistableKeyValueCollection.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ssarup $</item>
	///			<item name="vssdate">$Date: 10/10/07 2:28a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class PersistableKeyValueCollection : IDisposable
	{
        #region Member Data
		/// <summary>
		/// Inside this DataSet a single DataTable named ConfigValues is created.
		/// </summary>
		private DataSet m_DataSet;
		/// <summary>
		/// Filename for the DataSet XML serialization.
		/// </summary>
		private string mConfigFileName;
        #endregion

        #region Accessors
		/// <summary>
		/// Filename for the DataSet XML serialization.
		/// </summary>
		public string ConfigFileName
		{
			get{return mConfigFileName;}
		}
		/// <summary>
		/// Indexer for getting value associated with specified key.
		/// </summary>
		public string this[string key]
		{
			get{return Value(key);}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the PersistableKeyValueCollection class.
		/// </summary>
		/// <param name="xmlConfigFile">XML file name.</param>
		public PersistableKeyValueCollection(string xmlConfigFile)
		{
			mConfigFileName = xmlConfigFile;
			m_DataSet = new DataSet("ConfigOpt");
			if(File.Exists(xmlConfigFile))
			{
				// populate DataSet
				m_DataSet.ReadXml(xmlConfigFile);
			}
			else
			{
				// Initialize DataSet with two fields (key / value)
				// DataSet is left empty
				DataTable dt = new DataTable("KeyValuePairs");
				dt.Columns.Add("Key", System.Type.GetType("System.String"));
				dt.Columns.Add("Value", System.Type.GetType("System.String"));
				m_DataSet.Tables.Add(dt);
			}
		}
        #endregion

        #region Methods
		/// <summary>
		/// Explicitly releases all resources used by this object.
		/// </summary>
		public void Dispose()
		{
			// Call the overridden Dispose method that contains common cleanup code
			// Pass true to indicate that it is called from Dispose
			Dispose(true);
			// Prevent subsequent finalization of this object. This is not needed 
			// because managed and unmanaged resources have been explicitly released
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Releases the unmanaged resources used by the object and optionally 
		/// releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; 
		/// false to release only unmanaged resources. 
		/// </param>
		/// <remarks>
		/// Dispose(bool disposing) executes in two distinct scenarios.
		/// If disposing equals true, the method has been called directly
		/// or indirectly by a user's code. Managed and unmanaged resources
		/// can be disposed.
		/// If disposing equals false, the method has been called by the 
		/// runtime from inside the finalizer and you should not reference 
		/// other objects. Only unmanaged resources can be disposed.
		/// </remarks>
		protected void Dispose(bool disposing)
		{
			if( disposing )
			{
				if ( m_DataSet != null )
					m_DataSet.Dispose();
			}
		}
		/// <summary>
		/// Presists changes to XML file.
		/// </summary>
		public void Store()
		{
			Store(mConfigFileName);
		}
		/// <summary>
		/// Presists changes to specified XML file.
		/// </summary>
		/// <param name="ConfigFile"></param>
		public void Store(string ConfigFile)
		{
			mConfigFileName = ConfigFile;

			// Fix for Bug 4159
			if(File.Exists(ConfigFile))
				FileUtilities.RemoveFileAttributesForWrite(ConfigFile);

			m_DataSet.WriteXml(ConfigFile);
		}
		/// <summary>
		/// Get value associated with specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string Value(string key)
		{
			DataView dv = m_DataSet.Tables["KeyValuePairs"].DefaultView;
			dv.RowFilter = "Key='" + key + "'";
			if(dv.Count > 0)
				return dv[0]["Value"].ToString();
			else
				return "";
		}
		/// <summary>
		/// Add key/value pair. If key/value pair already
		/// exists only the value is changed.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <param name="value">String value associated with specified key.</param>
		public void Add(string key, string value)
		{
			DataView dv = m_DataSet.Tables["KeyValuePairs"].DefaultView;
			dv.RowFilter = "Key='" + key + "'";
			if(dv.Count > 0)
			{
				dv[0]["Value"] = value;
			}
			else
			{
				DataRow dr = m_DataSet.Tables["KeyValuePairs"].NewRow();
				dr["Key"] = key;
				dr["Value"] = value;
				m_DataSet.Tables["KeyValuePairs"].Rows.Add(dr);
			}
		}
        #endregion
	}
}
