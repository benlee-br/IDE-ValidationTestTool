using System;
using System.Collections;
using System.Text;

namespace BioRad.Common.Collections
{
	#region Documentation Tags
	/// <summary>
	/// Represents a set of unordered integer keys mapped to a single object value.
	/// </summary>
	/// <remarks>
	/// For example, matching a set of fluorophor ID's to its deconvolution matrix.
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
	///			<item name="vssfile">$Workfile: ManyUnorderedKeys2SingleObject.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Collections/ManyUnorderedKeys2SingleObject.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class ManyUnorderedKeys2SingleObject : IDisposable
	{
        #region Member Data
		/// <summary>
		/// Internal data structure for storing key-value pairs.
		/// </summary>
		private Hashtable m_Hashtable;
        #endregion

        #region Accessors
		/// <summary>
		/// Gets the number of key-and-value pairs contained in the ManyUnorderedKeys2SingleObject instance.
		/// </summary>
		public int Count
		{
			get{return m_Hashtable.Count;}
		}
		/// <summary>
		/// Gets an ICollection containing the values.
		/// </summary>
		public ICollection Values 
		{
			get{return m_Hashtable.Values;}
		}
        #endregion

        #region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the ManyUnorderedKeys2SingleObject class.
		/// </summary>
		public ManyUnorderedKeys2SingleObject()
		{
			m_Hashtable = new Hashtable();
		}
        #endregion

        #region Methods
		/// <summary>
		/// Explicitly releases all resources used by this object.
		/// </summary>
		public void Dispose()
		{
			// Prevent subsequent finalization of this object. This is not needed 
			// because managed and unmanaged resources have been explicitly released
			GC.SuppressFinalize(this);
			// Call the overridden Dispose method that contains common cleanup code
			// Pass true to indicate that it is called from Dispose
			Dispose(true);
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
		protected virtual void Dispose(bool disposing)
		{
			lock (this)
			{
				// release references.
				if (m_Hashtable != null)
				{
					m_Hashtable.Clear();
					m_Hashtable = null;
				}
			}
		}
		/// <summary>
		/// Create a unique key from a set of unorder integers.
		/// </summary>
		/// <param name="keys">Reference to set of unorder integers.</param>
		/// <returns>Key in the form of a string.</returns>
		private string MakeKey(int[] keys)
		{
			Array.Sort(keys);

			int len = keys.Length;
 			StringBuilder sb = new StringBuilder();

			for(int i=0; i<len; i++)
			{
				sb.Append(keys[i].ToString());
				if ( i < len-1 )
					sb.Append(",");
			}

			return sb.ToString();
		}
		/// <summary>
		/// Adds an entry with the specified set of integers and value into the ManyUnorderedKeys2SingleObject instance.
		/// </summary>
		/// <remarks>
		/// If key-value pair exists, then existing value is replaced.
		/// </remarks>
		/// <param name="keys">Reference to set of unorder integers.</param>
		/// <param name="obj">The Object value of the entry to add. The value can be a null reference.</param>
		/// <exception cref="System.ApplicationException">
		/// key is a null reference.
		/// </exception>
		public void Add(int[] keys, object obj)
		{
			if ( keys == null && keys.Length == 0 )
				throw new ApplicationException("key is a null reference.");//todo

			string s  = MakeKey(keys);

			if ( !m_Hashtable.ContainsKey(s) )
				m_Hashtable.Add(s, obj);// add new key-value pair.

			s = null;
		}
		/// <summary>
		/// Gets the value of the specified entry from the ManyUnorderedKeys2SingleObject instance.
		/// </summary>
		/// <param name="keys">Set of unorder integers.</param>
		/// <exception cref="System.ApplicationException">
		/// key is a null reference.
		/// </exception>
		/// <exception cref="System.ApplicationException">
		/// key does not exist in the collection.
		/// </exception>
		/// <returns>
		/// Object that was added with the same set of unorder 
		/// integers or null if does not exists.
		/// </returns>
		public object GetObject(int[] keys)
		{
			object obj = null;

			if ( keys == null && keys.Length == 0 )
				throw new ApplicationException("key is a null reference.");//todo

			string s  = MakeKey(keys);

			if ( m_Hashtable.ContainsKey(s) )
				obj = m_Hashtable[s];

			s = null;
			return obj;
		}
        #endregion
	}
}
