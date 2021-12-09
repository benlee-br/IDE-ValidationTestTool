using System;
using System.Collections;
using BioRad.Common;

namespace BioRad.Reports
{
	#region Documentation Tags
	/// <summary>
	/// This is a data type that is used by the report generator to manage
	/// a sortable 2D table and associated column headings. Classes that implement
	/// IReportable should use this data type to associate headings with a data
	/// table.
	/// <para>NOTE: At this time, the report viewer only supports sorting for a single table.
	/// The table with the lowest sort order in the reported data collection is the
	/// table that will be presented for sorting.
	/// </para>
	/// </summary>
	/// <remarks>
	/// Column elements are sorted using their IComparable implementation. Custom types
	/// will need to implement IComparable to be successfully sorted.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Mark Chilcott, Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
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
	///			<item name="vssfile">$Workfile: SortableTable.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Reports/SortableTable.cs $</item>
	///			<item name="vssrevision">$Revision: 14 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class SortableTable
	{
		#region Contained Classes
		/// <summary>
		/// Sort parameters data object.
		/// </summary>
		public class TableSortParameters : ICloneable
		{
			#region Member Data
			private bool m_Ascending = true;
			private bool m_Sort = false;
			private int m_SortedColumn = 0;
			#endregion

			#region Accessors
			/// <summary>
			/// When true, data is to be sorted in ascending order.
			/// </summary>
			public bool Ascending { get { return m_Ascending; } set { m_Ascending = value; } }
			/// <summary>
			/// When true, data is to be sorted. If false, data is presented in default
			/// order.
			/// </summary>
			public bool Sort { get { return m_Sort; } set { m_Sort = value; } }
			/// <summary>
			/// Column number to sort table by (zero-based).
			/// </summary>
			public int SortedColumn { get { return m_SortedColumn; } set { m_SortedColumn = value; } }
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Parameterized constructor initializes object.
			/// </summary>
			/// <param name="sort">True if data is to be sorted.</param>
			/// <param name="ascending">If true, data is sorted in ascending order. If false,
			/// data is sorted in descending order.</param>
			/// <param name="column">Column by which to sort data</param>
			public TableSortParameters(bool sort, bool ascending, int column)
			{
				m_Ascending = ascending;
				m_Sort = sort;
				m_SortedColumn = column;
			}

			/// <summary>
			/// Default constructor: Not sorted, ascending order true, sort column zero.
			/// </summary>
			public TableSortParameters()
			{
			}
			#endregion

			#region Methods
			#region ICloneable Members
			/// <summary>
			/// Create copy of this object.
			/// </summary>
			/// <returns>Copy of object.</returns>
			public object Clone()
			{
				return new TableSortParameters(this.Sort, this.Ascending, this.SortedColumn);
			}
			#endregion

			/// <summary>
			/// Override compares member values.
			/// </summary>
			/// <param name="obj">object to compare.</param>
			/// <returns>true if parameter is not null, is of type SortParameters and
			/// member values match this object.</returns>
			public override bool Equals(object obj)
			{
				TableSortParameters sortParameters = obj as TableSortParameters;
				if (sortParameters == null) return false;
				return ((m_Ascending == sortParameters.Ascending) && 
					(m_SortedColumn == sortParameters.SortedColumn) && 
					(m_Sort == sortParameters.Sort));
			}
			/// <summary>
			/// Override computes hash code as bit-wise or of member data.
			/// </summary>
			/// <returns></returns>
			public override int GetHashCode()
			{
				return (m_Ascending.GetHashCode()) ^ m_SortedColumn ^ (m_Sort.GetHashCode());
			}
			#endregion
		}

		/// <summary>
		/// Allows control of sorting algorithm in an array list. Sorting alogorithm
		/// may be set in constructor or via Accessor. Algorithm takes effect at subsequent
		/// Sort.
		/// </summary>
		public class SortableArrayList : ArrayList, ICloneable
		{
			#region Member Data
			private SortAlgorithms m_SortAlgorithm = SortAlgorithms.Default;
			#endregion

			#region Accessors
			/// <summary>
			/// Get/set sort algorithm. Takes effect at subsequent Sort() method call.
			/// </summary>
			public SortAlgorithms SortAlgorithm
			{
				get { return m_SortAlgorithm; }
				set { m_SortAlgorithm = value; }
			}
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Default constructor defers to base type constructor.
			/// </summary>
			public SortableArrayList(): base()
			{
			}

			/// <summary>
			/// Parameterized constructor. Defers to base type constructor.
			/// </summary>
			/// <param name="c"></param>
			public SortableArrayList(ICollection c): base(c)
			{
			}

			/// <summary>
			/// Parameterized constructor. Defers to base type constructor.
			/// </summary>
			/// <param name="capacity"></param>
			public SortableArrayList(int capacity) : base(capacity)
			{
			}

			/// <summary>
			/// Parameterized constructor. Defers to base type constructor.
			/// Sets sorting algorithm.
			/// </summary>
			/// <param name="algorithm">Determines algorithm used to sort array list.</param>
			public SortableArrayList(SortAlgorithms algorithm): base()
			{
				m_SortAlgorithm = algorithm;
			}

			/// <summary>
			/// Parameterized constructor. Defers to base type constructor.
			/// Sets sorting algorithm.
			/// </summary>
			/// <param name="algorithm">Determines algorithm used to sort array list.</param>
			/// <param name="c"></param>
			public SortableArrayList(SortAlgorithms algorithm, ICollection c): base(c)
			{
				m_SortAlgorithm = algorithm;
			}

			/// <summary>
			/// Parameterized constructor. Defers to base type constructor.
			/// Sets sorting algorithm.
			/// </summary>
			/// <param name="algorithm">Determines algorithm used to sort array list.</param>
			/// <param name="capacity"></param>
			public SortableArrayList(SortAlgorithms algorithm, int capacity) : base(capacity)
			{
				m_SortAlgorithm = algorithm;
			}
			#endregion

			#region Methods
			/// <summary>
			/// Performs a bubble sort on this array list's elements which preserves
			/// sub-ordering of elements.
			/// </summary>
			/// <param name="index">Starting index for sort.</param>
			/// <param name="count">Number of elements to sort.</param>
			/// <param name="comparer">Comparer to use for sort.</param>
			private void BubbleSort(int index, int count, IComparer comparer)
			{
				// Sort only two or more elements
				if (count <= 1) return;
				object temp;
				// indexers for each pass
				int startPass = index;
				int endPass = index + count -2;
				// indexers tracking first and last swapped element indexers
				int currentEnd = endPass;
				int currentStart = currentEnd;
				// Loop until done. Done will be true if a pass with no swap occurs.
				bool done;
				do
				{
					done = true;
					for (int i = startPass; i <= endPass; i++)
					{
						temp = this[i];
						if (comparer.Compare(temp, this[i+1]) > 0)
						{
							// Switch out of order elements
							this[i] = this[i+1];
							this[i+1] = temp;
							done = false;
							// Next pass will begin at the element prior to the first switched
							// element (elements bubble up at most one position)
							if (i <= currentStart) currentStart = i -1;
							// Next pass will end at the element prior to the last switched element
							// (elements bubble down to their final position)
							currentEnd = i - 1;
						}
					}
					endPass = (currentEnd > 0)? currentEnd : 0;
					startPass = (currentStart > 0) ? currentStart : 0;
					// current start will be set by the first switch, if any, on the next pass
					currentStart = currentEnd;
				}
				while (!done);
			}

			/// <summary>
			/// Performs a shell sort on this array list's elements.
			/// </summary>
			/// <param name="index">Starting index for sort.</param>
			/// <param name="count">Number of elements to sort.</param>
			/// <param name="comparer">Comparer to use for sort.</param>
			private void ShellSort(int index, int count, IComparer comparer)
			{
				// Sort only two or more elements
				if (count <= 1) return;
				// Find optimal sort distance (stride)
				int stride = 0;
				do
				{
					stride = 3 * stride + 1;
				}
				while (stride <= count);
				// perform sort until stride is reduced to 1
				object temp;
				int current;
				do
				{
					// reduce stride
					stride = stride / 3;
					for (int i = stride + index; i< index + count; i++)
					{
						temp =  this[i];
						current = i;
						while ( comparer.Compare(this[current - stride], temp) > 0)
						{
							this[current] = this[current -stride];
							current -= stride;
							if ((current - stride) < index) break;
						}
						this[current] = temp;
					}
				}
				while (stride > 1);
			}

			/// <summary>
			/// Override sorts array list based on current SortAlgorithm property
			/// using a default ArrayComparer object.
			/// </summary>
			public override void Sort()
			{
				IComparer comparer = new ArrayComparer(new TableSortParameters());
				this.Sort(comparer);
			}
	
			/// <summary>
			/// Override sorts array list based on current SortAlgorithm property.
			/// </summary>
			/// <param name="comparer"></param>
			public override void Sort(IComparer comparer)
			{
//				DateTime time = DateTime.Now;
				switch (m_SortAlgorithm)
				{
					case SortAlgorithms.Bubble:
						this.BubbleSort(0, this.Count, comparer);
						break;
					case SortAlgorithms.Shell:
						this.ShellSort(0, this.Count, comparer);
						break;
					default:
						base.Sort (comparer);
						break;
				}
//				Console.WriteLine("{0} sort took {1} for {2} element array", m_SortAlgorithm.ToString(), DateTime.Now-time, this.Count);
			}

			/// <summary>
			/// Override sorts array list based on current SortAlgorithm property.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="count"></param>
			/// <param name="comparer"></param>
			public override void Sort(int index, int count, IComparer comparer)
			{
				switch (m_SortAlgorithm)
				{
					case SortAlgorithms.Bubble:
						this.BubbleSort(index, count, comparer);
						break;
					case SortAlgorithms.Shell:
						this.ShellSort(index, count, comparer);
						break;
					default:
						base.Sort (index, count, comparer);
						break;
				}
			}

			#region ICloneable Members
			/// <summary>
			/// Override performs a shallow copy of the array list.
			/// </summary>
			/// <returns>copy of object</returns>
			public override object Clone()
			{
				return new SortableArrayList(this.SortAlgorithm, (ICollection) base.Clone());
			}
			#endregion

			#endregion
		}

		/// <summary>
		/// Custom array comparer. Use to sort arrays based on column index in either
		/// ascending or descending order.
		/// </summary>
		public class ArrayComparer : IComparer
		{
			#region Member Data
			private TableSortParameters m_SortControl;
			#endregion

			#region Constructors and Destructor
			/// <summary>
			/// Parameterized constructor.
			/// </summary>
			/// <param name="sortControl">Determines how arrays are compared.</param>
			public ArrayComparer(TableSortParameters sortControl)
			{
				m_SortControl = sortControl;
			}
			#endregion

			#region Methods
			#region IComparer Members
			/// <summary>
			/// Explicit interface definition for Compare. 
			/// </summary>
			/// <remarks>Will throw InvalidCastException if parameters are not ArrayList type.</remarks>
			/// <param name="x">must be ArrayList</param>
			/// <param name="y">must be ArrayList</param>
			/// <returns>comparison result as controlled by sort parameters.</returns>
			int IComparer.Compare(object x, object y)
			{
				return Compare((ArrayList)x, (ArrayList) y);
			}
			#endregion
			/// <summary>
			/// Type-safe array compare. Uses sort parameters to control which column
			/// to compare and whether comparison results in ascending or descending sort.
			/// If sort is disabled, comparison returns zero (equal).
			/// </summary>
			/// <remarks>If selected value in either array does not support IComparable returns
			/// zero (equal).</remarks>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <returns>comparison result as controlled by sort parameters.</returns>
			public int Compare(ArrayList x, ArrayList y)
			{
				// if sorting is disabled return equal as true
				if (!m_SortControl.Sort) return 0;
				int index = m_SortControl.SortedColumn;
				// validate that selected sort column is valid for parameters
				if ((index >= x.Count) || (index >= y.Count))
				{
					throw new ArgumentException("Array does not contain column selected for sorting.");
				}
				IComparable xElement = x[index] as IComparable;
				IComparable yElement = y[index] as IComparable;
				// if either item to compare is not comparable return equal as true
				if ((x == null) || (y == null)) return 0;
				// defer to array element comparison, returning equal as true if comparison
				// fails
				try
				{
					if (m_SortControl.Ascending)
						// compare for ascending order sort
						return xElement.CompareTo(yElement);
					else
						// compare for descending order sort
						return yElement.CompareTo(xElement);
				}
				catch
				{
					// Fault tolerance - return equal if elements aren't comparable
					return 0;
				}
			}
			#endregion
		}

		#endregion

		#region Constants
		/// <summary>
		/// Sorting Algorithms to be used on SortableTable data.
		/// </summary>
		public enum SortAlgorithms
		{
			/// <summary>
			/// Algorithm used by ArrayList.Sort()
			/// </summary>
			Default,
			/// <summary>
			/// Preserves sub-sort order when sorting.
			/// </summary>
			Bubble,
			/// <summary>
			/// Faster than bubble sort but sub-sort order is not preserved.
			/// </summary>
			Shell
		}
		#endregion

		#region Member Data
		private bool m_AllowSorting = true;
		private bool m_Empty = false;
		private NoReportData m_NoReportData = new NoReportData();
		private string [] m_ColumnDescriptions;
		private string [] m_ColumnHeadings;
		private TableSortParameters m_SortControl = new TableSortParameters();
		private TableSortParameters m_SortControlCurrent = new TableSortParameters();
		private SortableArrayList m_TableData = new SortableArrayList();
		private SortableArrayList m_TableDataSorted = new SortableArrayList();
		private string m_TableDescription = String.Empty;
		private int m_TableSortOrder = 1;
		#endregion

		#region Accessors
		/// <summary>
		/// Indexer to data table.
		/// </summary>
		public object this[int row, int column]
		{
			get
			{
				// Return NoReportData type for any index if table is constructed as empty
				if (m_Empty) return m_NoReportData;

				// validate indices
				if (row < 0 || row > m_TableData.Count)
				{
                    string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_RowIndexOutOfRange);
					throw new ArgumentOutOfRangeException("row", row, sr.ToString());
				}
				if (column < 0 || column > ((ArrayList)m_TableData[row]).Count)
				{
                    string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_ColumnIndexOutOfRange);
					throw new ArgumentOutOfRangeException("column", column, sr.ToString());
				}
				if (m_SortControl.Sort)
				{
					this.UpdateSorting();
					// return sorted data
					return ((ArrayList) m_TableDataSorted[row])[column];
				}
				else
					// If sort control specifies unsorted data, return data in original order
					return ((ArrayList) m_TableData[row])[column];
			}
		}

		/// <summary>
		/// When set to false, table may not be sorted. Default is true unless explicitly
		/// set in parameterized constructor.
		/// </summary>
		public bool AllowSorting
		{
			get { return m_AllowSorting; }
			set { m_AllowSorting = value; }
		}

		/// <summary>
		/// Array of column headings.
		/// </summary>
		public string [] ColumnHeadings
		{
			get { return m_ColumnHeadings; }
		}

		/// <summary>
		/// Array of column descriptions.
		/// </summary>
		public string [] ColumnDescriptions
		{
			get { return m_ColumnDescriptions; }
		}

		/// <summary>
		/// Number of data rows in table.
		/// </summary>
		public int Rows
		{
			get { return m_TableData.Count; }
		}

		/// <summary>
		/// Number of data columns in table.
		/// </summary>
		public int Columns
		{
			get { return m_ColumnHeadings.Length; }
		}

		/// <summary>
		/// Table description
		/// </summary>
		public string Description
		{
			get { return m_TableDescription;}
		}

		/// <summary>
		/// Returns true if table was constructed as empty using the default
		/// constructor.
		/// </summary>
		public bool Empty
		{
			get { return m_Empty;}
		}

		/// <summary>
		/// Whether to sort data. Throws InvalidOperationException if AllowSorting is
		/// false and Sort value is set True.
		/// </summary>
		public bool Sort
		{
			get { return m_SortControl.Sort;}
			set 
			{ 
				if (value && !this.AllowSorting)
				{
                    string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_SortingNotAllowed);
					throw new InvalidOperationException(sr.ToString());
				}
				m_SortControl.Sort = value;
			}
		}

		/// <summary>
		/// Determines the algorithm used to sort data.
		/// </summary>
		public SortAlgorithms SortAlgorithm
		{
			get { return m_TableDataSorted.SortAlgorithm; }
			set 
			{ 
				m_TableData.SortAlgorithm = m_TableDataSorted.SortAlgorithm = value;
			}
		}

		/// <summary>
		/// Is the sort to be in  ascending order.
		/// </summary>
		public bool SortAscending
		{
			get { return m_SortControl.Ascending;}
			set { m_SortControl.Ascending = value;}
		}

		/// <summary>
		/// Which column is the one to be sorted.
		/// </summary>
		public int SortColumn
		{
			get { return m_SortControl.SortedColumn;}
			set { m_SortControl.SortedColumn = value;}
		}

		/// <summary>
		/// It is possible to have more than one sortable table in a report.
		/// This value can be used to control selection of which table the user
		/// will have sorting control over. The lowest number should take precedence.
		/// </summary>
		public int TableSortOrder
		{
			get { return m_TableSortOrder;}
		}
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default constructor initializes a sortable table with no data.
		/// </summary>
		public SortableTable()
		{
			m_AllowSorting = false;
			m_Empty = true;
		}

		/// <summary>
		/// Parameterized constructor initializes object. Uses default sorting
		/// parameters which present data initially as unsorted. Table sort order defaulted to
		/// 1. Sorting algorithm set to Bubble sort.
		/// </summary>
		/// <param name="tableDescription">Text description of table (may be empty string).</param>
		/// <param name="columnHeadings">Array of column heading text. If null, heading text is blank.</param>
		/// <param name="columnDescriptions">Optional array of column heading descriptions. If
		/// null, descriptions will be the same as heading text.</param>
		/// <param name="data">Two dimensional data table (must be rectangular)</param>
		public SortableTable(string tableDescription, string [] columnHeadings, string [] columnDescriptions,
			object [][] data)
		{
			// validate data table - must be non-null and rectangular
			if (data == null)
			{
                string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_Null);
				throw new ArgumentNullException(sr.ToString());
			}
			if (data.GetLength(0) == 0)
			{
                string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_Empty);
				throw new ArgumentException(sr.ToString());
			}
			for (int i = 1; i< data.GetLength(0); i++)
			{
				if (data[i].Length != data[i-1].Length)
				{
                    string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_NotRectangular);
					throw new ArgumentException(sr.ToString());
				}
			}
			// validate column headings and descriptions. If null, they are defaulted
			// to arrays of empty strings
			if (columnHeadings == null)
			{
				// Default column headings to the empty string
				columnHeadings = new string [data[0].Length];
			}
			if ((columnHeadings != null) && (columnHeadings.Length > 0))
			{
				if ((columnDescriptions != null) && (columnHeadings.Length != columnDescriptions.Length))
				{
                    string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_ColumnDescriptionsWrongLength);
					throw new ArgumentException(sr.ToString());
				}

				if (columnHeadings.Length != data[0].Length)
				{
                    string sr = StringUtility.FormatString(BioRad.Common.Properties.Resources.Table_ColumnHeadingsWrongLength);
					throw new ArgumentException(sr.ToString());
				}
			}
			// Copy column headings and descriptions to local arrays
			m_ColumnHeadings= (string []) columnHeadings.Clone();
			if (columnDescriptions == null)
			{
				// If no column descriptions, set descriptions to equal column headings
				m_ColumnDescriptions = (string []) columnHeadings.Clone();
			}
			else
			{
				m_ColumnDescriptions = (string []) columnDescriptions.Clone();
			}
			// Add each data row to data array list
			for (int i = 0; i < data.GetLength(0); i++)
			{
				m_TableData.Add(new ArrayList((object[])data.GetValue(i)));
			}
			// Sorted data table (initially unsorted) is a shallow copy of data table.
			// This works because data is not rearranged or modified within a row.
			m_TableDataSorted = (SortableArrayList) m_TableData.Clone();
			// Set sorting to Bubble sort. There is no significant speed difference for tables
			// of 100 elements among the sorting algorithms.
			this.SortAlgorithm = SortAlgorithms.Bubble;
		}

		/// <summary>
		/// Parameterized constructor initializes object. Uses default sorting
		/// parameters which present data initially as unsorted.
		/// </summary>
		/// <param name="tableDescription">Text description of table (may be empty string).</param>
		/// <param name="columnHeadings">Array of column heading text. If null, heading text is blank.</param>
		/// <param name="columnDescriptions">Optional array of column heading descriptions. If
		/// null, descriptions will be the same as heading text.</param>
		/// <param name="data">Two dimensional data table (must be rectangular)</param>
		/// <param name="tableOrder">Used externally - table with lowest sort order may
		/// be presented to user as sortable.</param>
		public SortableTable(string tableDescription, string [] columnHeadings, string [] columnDescriptions,
			object [][]  data, int tableOrder) : this(tableDescription, columnHeadings, columnDescriptions, data)
		{
			this.m_TableSortOrder = tableOrder;
		}

		/// <summary>
		/// Parameterized constructor initializes object. Uses default sorting
		/// parameters which present data initially as unsorted. Explicit control of whether
		/// sorting is allowed.
		/// </summary>
		/// <param name="tableDescription">Text description of table (may be empty string).</param>
		/// <param name="columnHeadings">Array of column heading text. If null, heading text is blank.</param>
		/// <param name="columnDescriptions">Optional array of column heading descriptions. If
		/// null, descriptions will be the same as heading text.</param>
		/// <param name="data">Two dimensional data table (must be rectangular)</param>
		/// <param name="allowSorting">When false, this table may not be sorted.</param>
		public SortableTable(string tableDescription, string [] columnHeadings, string [] columnDescriptions,
			object [][]  data, bool allowSorting) : this(tableDescription, columnHeadings, columnDescriptions, data)
		{
			this.m_AllowSorting = allowSorting;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns column description for given index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Text of column description.</returns>
		public string ColumnDescription( int index)
		{
			// Return NoReportData type for any index if table is constructed as empty
			if (m_Empty) return m_NoReportData.ToString();

			if (index < 0 || index > m_ColumnDescriptions.Length)
				throw new ArgumentOutOfRangeException("index", index, "Invalid column description index.");
			return m_ColumnDescriptions[index];
		}

		/// <summary>
		/// Returns column heading for given index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Column heading text.</returns>
		public string ColumnHeading(int index)
		{
			// Return NoReportData type for any index if table is constructed as empty
			if (m_Empty) return m_NoReportData.ToString();

			if (index < 0 || index > m_ColumnHeadings.Length)
				throw new ArgumentOutOfRangeException("index", index, "Invalid column heading index.");
			return m_ColumnHeadings[index];
		}

		/// <summary>
		/// Search column descriptions for given description, returning index of match.
		/// </summary>
		/// <param name="description">Description to search column descriptions for.</param>
		/// <returns>-1 if no match found for description, else index of first column description to
		/// match given description parameter.</returns>
		public int ColumnIndex(string description)
		{
			int index = -1;
			for( int i = 0; i< m_ColumnDescriptions.Length; i++)
			{
				if (m_ColumnDescriptions[i] == description)
				{
					index = i;
					break;
				}
			}
			return index;
		}

		/// <summary>
		/// Re-sorts data if sort parameters have changed.
		/// </summary>
		private void UpdateSorting()
		{
			if (!m_SortControl.Equals( m_SortControlCurrent))
			{
				// Only perform re-sort if sorting is enabled. If sorting is not enabled,
				// original table data is used.
				if (m_SortControl.Sort)
				{
					this.m_TableDataSorted.Sort(new ArrayComparer(m_SortControl));
				}
				// Update current sorting parameters
				m_SortControlCurrent = (TableSortParameters) m_SortControl.Clone();
			}
		}
		#endregion
	}
}
