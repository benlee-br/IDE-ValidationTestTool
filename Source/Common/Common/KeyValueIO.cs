using System;
using System.Data;
using System.IO;
using System.Globalization;
using System.Threading;

namespace BioRad.Common
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
	///			<item name="vssfile">$Workfile: KeyValueIO.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/KeyValueIO.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 1/19/10 12:30p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

    public class KeyValueIO : IDisposable, ICloneable
    {
        #region Member Data
		/// <summary>
		/// 
		/// </summary>
        protected DataSet m_DataSet;
        #endregion

        #region Accessors
        /// <summary>
        /// Indexer for getting value associated with specified key.
        /// </summary>
        public string this[string key]
        {
            get { return this.Value(key); }
        }
        /// <summary>Returns the XML representation of this object.</summary>
        public string GetXml
        {
            get { return m_DataSet.GetXml(); }
        }
        #endregion

        #region Constructors and Destructor
        /// <summary>Initializes a new instance of the KeyValueIO class.</summary>
        public KeyValueIO()
        {
            Init();
        }
        /// <summary>Initializes a new instance of the KeyValueIO class.</summary>
        /// <param name="path"></param>
        public KeyValueIO(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            Init();

            if (File.Exists(path))
            {
                int attemptsToRead = 50;
                while (attemptsToRead-- > 0)
                {
                    try
                    {
                        using (FileStream fs = File.Open(path, FileMode.Open,
                            FileAccess.Read, FileShare.None))
                        {
                            if (fs.Length > 0)
                                m_DataSet.ReadXml(fs);
                            attemptsToRead = 0;
                            break;
                        }
                    }
                    catch(Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        if (attemptsToRead > 0)
                            Thread.Sleep(100);
                    }
                }
            }
        }
        #endregion

        #region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			KeyValueIO io = new KeyValueIO();
			io.m_DataSet = this.m_DataSet.Copy();
			return io;
		}
        /// <summary>Existing key?</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return string.IsNullOrEmpty(this.Value(key)) ? false : true;
        }
        private void Init()
        {
            m_DataSet = new DataSet("KeyValueIO");
            m_DataSet.Locale = CultureInfo.InvariantCulture;

            // Initialize DataSet with two fields (key / value)
            // DataSet is left empty
            DataTable dt = new DataTable("KeyValuePairs");
            dt.Columns.Add("Key", System.Type.GetType("System.String"));
            dt.Columns.Add("Value", System.Type.GetType("System.String"));
            m_DataSet.Tables.Add(dt);
        }
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
            if (disposing)
            {
                if (m_DataSet != null)
                    m_DataSet.Dispose();
            }
        }
		/// <summary>
		/// Get XML representation of the data stored in object.
		/// </summary>
		/// <returns>Returns the XML representation of the data stored in object.</returns>
		public string ToXml()
		{
			if (m_DataSet == null)
				throw new ArgumentNullException("m_DataSet");
			m_DataSet.AcceptChanges();
			return m_DataSet.GetXml();
		}
		/// <summary>
		/// Create data set from XML representation of the data.
		/// </summary>
		/// <param name="xml">XML representation of the data.</param>
		public void FromXml(string xml)
		{
			using (System.IO.StringReader sr = new System.IO.StringReader(xml))
			{
				m_DataSet = new DataSet();
				m_DataSet.ReadXml(sr);
			}
		}
        /// <summary>Presists changes to specified file.</summary>
        /// <param name="path"></param>
        /// <returns>true if successful</returns>
        public bool Write(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            if (m_DataSet == null)
                throw new ArgumentNullException("m_DataSet");

            int attemptsToWrite = 50;
            while (attemptsToWrite-- > 0)
            {
                try
                {
                    using (FileStream fs = File.Open(path, FileMode.Create,
                        FileAccess.Write, FileShare.None))
                    {
                        m_DataSet.AcceptChanges();
                        m_DataSet.WriteXml(fs);
                        fs.Flush();
                        attemptsToWrite = 0;
                        break;
                    }
                }
                catch(Exception ex)
                {
                    string message = ex.Message;
                }
                finally
                {
                    if ( attemptsToWrite > 0 )
                        Thread.Sleep(100);
                }
            }
            return true;
        }
        /// <summary>Get value associated with specified key.</summary>
        /// <param name="key"></param>
        /// <returns>Empty string if not found.</returns>
        public string Value(string key)
        {
            string results = string.Empty;
            try
            {
                DataView dv = m_DataSet.Tables["KeyValuePairs"].DefaultView;
                dv.RowFilter = "Key='" + key + "'";
                if (dv.Count > 0)
                    results = dv[0]["Value"].ToString();
            }
            catch (Exception ex)
            {
                results = string.Empty;
                DiagnosticsLogger.DiagnosticsLogService.GetService.GetDiagnosticsLog(
                      DiagnosticsLogger.WellKnownLogName.MainForm).Info(ex.ToString());
            }
            return results;
        }
        /// <summary>
        /// Add key/value pair. If key/value pair already
        /// exists only the value is changed.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">String value associated with specified key.</param>
        public void Add(string key, string value)
        {
            try
            {
                DataView dv = m_DataSet.Tables["KeyValuePairs"].DefaultView;
                dv.RowFilter = "Key='" + key + "'";
                if (dv.Count > 0)
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
                m_DataSet.AcceptChanges();
            }
            catch(Exception ex)
            {
                DiagnosticsLogger.DiagnosticsLogService.GetService.GetDiagnosticsLog(
                      DiagnosticsLogger.WellKnownLogName.MainForm).Info(ex.ToString());
            }
        }
        #endregion
    }
}
