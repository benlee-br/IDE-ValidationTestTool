using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

using BioRad.Common;
namespace BioRad.Reports
{
	#region Documentation Tags
	/// <summary>
	/// Collection of objects keyed by string value.
	/// </summary>
	/// <remarks>
	/// Keys must be unique for each element. When objects are placed in collection
	/// they are keyed by an Enum. When retrieved, they are retrieved by string value
	/// of the Enum or by a ReportTag object.
	/// <para>Once collection is made read-only it cannot be modified.</para>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review: LvS 9/14/04</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">679</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: ReportedCollection.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Reports/ReportedCollection.cs $</item>
	///			<item name="vssrevision">$Revision: 13 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ReportedCollection : NameObjectCollectionBase
	{
		#region Constants
		private static readonly string FormatExtension = "_f";
		private static readonly string DataExtension = "";
		#endregion

        #region Accessors
		/// <summary>
		/// Gets a key-and-value pair from the collection (DictionaryEntry) using an index.
		/// </summary>
		protected DictionaryEntry this[ int index ]  
		{
			get  
			{
				return(new DictionaryEntry(this.BaseGetKey(index),this.BaseGet(index)));
			}
		}

		/// <summary>
		/// Gets or sets the collection value associated with the specified key.
		/// </summary>
		protected object this[ string key ]  
		{
			get  
			{
				return( this.BaseGet( key ) );
			}
			set  
			{
				this.BaseSet( key, value );
			}
		}

		/// <summary>
		/// Gets a String array that contains all the keys in the collection.
		/// </summary>
		protected string[] AllKeys  
		{
			get  
			{
				return( this.BaseGetAllKeys() );
			}
		}

		/// <summary>
		/// Gets an Object array that contains all the values in the collection.
		/// </summary>
		public Array AllValues  
		{
			get  
			{
				return( this.BaseGetAllValues() );
			}
		}

		// 
		/// <summary>
		/// Gets a value indicating if the collection is empty. The collection is considered
		/// empty if either it contains no keys or only keys that are null.
		/// </summary>
		public bool IsEmpty  
		{
			get  
			{
				return( !this.BaseHasKeys() );
			}
		}
		#endregion

        #region Constructors and Destructor
		/// <summary>
		/// Creates an empty collection.
		/// </summary>
		public ReportedCollection()  
		{
		}

		// Adds elements from an IDictionary into the new collection.
		/// <summary>
		/// Creates a collection containing elements from an IDictionary.
		/// </summary>
		/// <param name="d">dictionary supplying elements for new collection.</param>
		public ReportedCollection( IDictionary d)  
		{
			foreach ( DictionaryEntry de in d )  
			{
				this.BaseAdd( (String) de.Key, de.Value );
			}
		}
		#endregion

        #region Methods
		/// <summary>
		/// Adds a data entry and associated format string to the collection.
		/// </summary>
		/// <param name="key">key to be used as indexer for this value.</param>
		/// <param name="data">data to be added to the collection.</param>
		/// <param name="format">format string to be added to the collection.</param>
		public void Add( Enum key, object data , string format)  
		{
			this.AddData(key, data);
			this.AddFormat(key, format);
		}

		/// <summary>
		/// Adds a data entry to the collection.
		/// </summary>
		/// <param name="key">key to be used as indexer for this value.</param>
		/// <param name="data">data to be added to the collection.</param>
		public void AddData( Enum key, object data )  
		{
			this.BaseAdd( String.Concat(key.ToString(), DataExtension), data );
		}

		/// <summary>
		/// Adds a format string to the collection.
		/// </summary>
		/// <param name="key">key to be used as indexer for this value.</param>
		/// <param name="format">format string to be added to the collection.</param>
		public void AddFormat( Enum key, string format )  
		{
			this.BaseAdd( String.Concat(key.ToString(), FormatExtension), format );
		}

		/// <summary>
		/// Clears all the elements in the collection.
		/// </summary>
		public void Clear()  
		{
			this.BaseClear();
		}

		/// <summary>
		/// True if collection contains a given item and item is not null.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(string key)
		{
			return this[key] != null;
		}

		/// <summary>
		/// Get a data element from the collection.
		/// </summary>
		/// <param name="key">key to be used as indexer for this value</param>
		/// <returns>data element for given key</returns>
		public object GetData( string key)
		{
			return this[String.Concat(key, DataExtension)];
		}

		/// <summary>
		/// Get a data element from the collection.
		/// </summary>
		/// <param name="tag">report tag to be used as indexer for this value</param>
		/// <returns>data element for given tag</returns>
		public object GetData( ReportTag tag)
		{
			object data = this[String.Concat(tag.Name, DataExtension)];
			if ((data != null) || !tag.IsHeading)
			{
				// return data in collection matching tag name
				return data;
			}
			else
			{
				// Tag indicates a table heading and there is no data in collection directly
				// matching tag name. Retrieve table headings from table data, if any.
				SortableTable table = this[String.Concat(tag.BaseName, DataExtension)] as SortableTable;
				if (table != null)
				{
					if (table.Empty)
						// return special no report data type if table is empty
						return new NoReportData();
					else
						// return column headings if found
						return table.ColumnHeadings;
				}
				else return null;
			}
		}

		/// <summary>
		/// Get a format string from the collection.
		/// </summary>
		/// <param name="key">key to be used as indexer for this value</param>
		/// <returns>format string for given key</returns>
		public string GetFormat( string key)
		{
			return (string) this[String.Concat(key, FormatExtension)];
		}

		/// <summary>
		/// Get a format string from the collection.
		/// </summary>
		/// <param name="tag">report tag to be used as indexer for this value</param>
		/// <returns>format string for given tag</returns>
		public string GetFormat( ReportTag tag)
		{
			return (string) this[String.Concat(tag.Name, FormatExtension)];
		}

		/// <summary>
		/// Make the collection read-only.
		/// </summary>
		/// <remarks>Cannot be undone.</remarks>
		public void MakeReadOnly()
		{
			this.IsReadOnly = true;
		}

		/// <summary>
		/// Removes data and format string entries with the specified tag from the collection.
		/// </summary>
		/// <remarks>attempts to remove format string only if it exists</remarks>
		/// <param name="tag">key used to identify value to be removed.</param>
		public void Remove( string tag )  
		{
			this.BaseRemove( String.Concat(tag, DataExtension) );
			string formatTag = String.Concat(tag, FormatExtension);
			if (this[formatTag] != null)
				this.BaseRemove(formatTag);
		}

		/// <summary>
		/// Removes an entry in the specified index from the collection.
		/// </summary>
		/// <param name="index">index of entry to be removed.</param>
		protected void Remove( int index )  
		{
			this.BaseRemoveAt( index );
		}
		
		/// <summary>
		/// Return a localizable formatted date for reports 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public string FormattedDateForReports(DateTime d) 
		{			
			//Return localizable string
			return d.ToString();
			/*bool longDate=true;
			bool localize=false;
			DateTimeFormatInfo myDTFI;
			
			if (localize) 
			{
				myDTFI= CultureInfo.CurrentUICulture.DateTimeFormat;
			}
			else
			{
				myDTFI= CultureInfo.InvariantCulture.DateTimeFormat;					
			}

			if (longDate)
			{				
				return d.ToString(myDTFI.FullDateTimePattern,CultureInfo.InvariantCulture);
			}
			else
			            
				//d.ToString(CultureInfo.InvariantCulture);
				
				return d.ToString(myDTFI.LongDatePattern,CultureInfo.InvariantCulture);				
			}*/
		}
		
		/// <summary>
		/// return formatted string for reports
		/// </summary>
		/// <param name="item1"></param>
		/// <param name="item2"></param>
		/// <param name="item3"></param>
		/// <returns></returns>
		public string FormattedString(string item1,string item2,string item3)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(item1);
			sb.Append(" (");
			sb.Append(item2);
			sb.Append(item3);
			sb.Append(")");
			return sb.ToString();
		}

		/// <summary>
		/// Escape the slash characters in string and remove carriage returns
		/// </summary>
		/// <param name="originalString"></param>
		/// <returns>string</returns>
		public string EscapeString(string originalString)
		{
			string newString=originalString.Replace("\\","\\\\");
			//(ST) 2005-11-30 Remove carriage returns
			//Fix for defect 3712-Comments with carriage returns in audit trails can disorganize report header
			newString=newString.Replace("\r"," "); 
			newString=newString.Replace("\n"," "); 
			return newString;
		}

		/// <summary>
		/// Populate ArrayList dataArray by add Data from File Header
		/// </summary>
		/// <param name="dataArray">populated ArrayList</param>
		/// <param name="computerName"></param>
		/// <param name="createdByClientApp"></param>
		/// <param name="createdByClientAppVersion"></param>
		/// <param name="createdByUser"></param>
		/// <param name="createdDate"></param>
		/// <param name="createdInRE"></param>
		/// <param name="GUID"></param>
		/// <param name="modifiedByUser"></param>
		/// <param name="modifiedDate"></param>
		/// <param name="osBuildNumber"></param>
		/// <param name="servicePack"></param>
		public void AddDataForFileHeader(ref ArrayList dataArray, 
			string computerName,
			string createdByClientApp,
			string createdByClientAppVersion,
			string createdByUser,
			DateTime createdDate,
			bool createdInRE,
			string GUID,
			string modifiedByUser,
			DateTime modifiedDate,
			string osBuildNumber,
			string servicePack)
		{
			ArrayList dataSubArray = new ArrayList();

            string yes = StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderYes);
            string no = StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderNo);

			//Empty line
			dataSubArray.Add(""); dataSubArray.Add(EscapeString(""));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Computer name"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderComputerName));
			dataSubArray.Add(EscapeString(computerName));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Created by app"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderCreatedByApp));
			dataSubArray.Add(EscapeString(FormattedString(createdByClientApp,"v",
				createdByClientAppVersion)));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Created by user"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderCreatedByUser));
			dataSubArray.Add(EscapeString(createdByUser));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Creation Date" 
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderCreationDate));				
			dataSubArray.Add(EscapeString(FormattedDateForReports(createdDate)));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//Fix for defect 4173-Replace "RE" with "Security Edition" in reports header
			//(ST) 2006-03-20
			//"Created in Security Edition"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderCreatedInSecurityEdition));
			dataSubArray.Add(EscapeString(createdInRE?yes:no));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Last Creation GUID"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderLastCreationGUID));

			dataSubArray.Add(EscapeString(GUID));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Modified by user"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderModifiedByUser));

			dataSubArray.Add(EscapeString(modifiedByUser));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Last modified date"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderLastModifiedDate));

			dataSubArray.Add(EscapeString(FormattedDateForReports(modifiedDate)));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"OS Build and Service Pack"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderOSBuildAndServicePack));

			dataSubArray.Add(
				EscapeString(FormattedString(osBuildNumber,
				"",
				servicePack)));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();
		}

		/// <summary>
		/// Add Audit trail specific Data
		/// </summary>
		/// <param name="dataArray">array list to populate</param>
		/// <param name="auditHeaderApplication"></param>
		/// <param name="auditHeaderApplicationVersion"></param>
		/// <param name="auditHeaderComment"></param>
		/// <param name="auditHeaderFullUserName"></param>
		/// <param name="auditHeaderUser"></param>
		/// <param name="auditHeaderGUID"></param>
		/// <param name="auditHeaderMachineName"></param>
		/// <param name="auditHeaderNumChanges"></param>
		/// <param name="auditHeaderSignature"></param>
		/// <param name="auditHeaderSignatureComment"></param>
		/// <param name="auditHeaderTime"></param>
		/// <param name="auditHeaderVersion"></param>
		public void AddDataForAudit(ref ArrayList dataArray,
			string auditHeaderApplication,
			string auditHeaderApplicationVersion,
			string auditHeaderComment,
			string auditHeaderFullUserName,
			string auditHeaderUser,
			string auditHeaderGUID,
			string auditHeaderMachineName,
			int auditHeaderNumChanges,
			string auditHeaderSignature,
			string auditHeaderSignatureComment,
			DateTime auditHeaderTime,
			int auditHeaderVersion)
		{														
			ArrayList dataSubArray=new ArrayList();

			dataSubArray.Add(""); dataSubArray.Add(EscapeString(""));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Audit Information:"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderAuditInformation));	
				
			dataSubArray.Add("");
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Application"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderApplication));
			dataSubArray.Add(EscapeString(
				FormattedString(auditHeaderApplication
				,"v",
				auditHeaderApplicationVersion)));
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Comment"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderComment));
			dataSubArray.Add(EscapeString(auditHeaderComment)); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"User Name and ID"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderUserNameAndID));
			dataSubArray.Add(EscapeString(
				FormattedString(auditHeaderFullUserName,
				"",
				auditHeaderUser))); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"GUID"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderGUID));

			dataSubArray.Add(EscapeString(auditHeaderGUID)); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Machine Name"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderMachineName));
				
			dataSubArray.Add(EscapeString(auditHeaderMachineName)); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Number of Changes"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderNumberOfChanges));

			dataSubArray.Add(auditHeaderNumChanges.ToString()); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Signature"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderSignature));

			dataSubArray.Add(EscapeString(auditHeaderSignature)); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Signature Comment"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderSignatureComment));
			dataSubArray.Add(EscapeString(auditHeaderSignatureComment)); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Date"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderDate));

			dataSubArray.Add(EscapeString(auditHeaderTime.ToString())); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();

			//"Version"
			dataSubArray.Add(
                StringUtility.FormatString(BioRad.Common.Properties.Resources.ReportHeaderVersion));
			dataSubArray.Add(auditHeaderVersion.ToString()); 
			dataArray.Add((object[]) dataSubArray.ToArray(typeof(object)));dataSubArray.Clear();
		}
		#endregion
	}
}
