using System;
using System.Drawing;
using System.Collections;
using System.Text;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.DiagnosticsLogger;


namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// This class provides access to common methods which are designed for generic functions
	/// for plate seup module. Typical example is get plate index by its name etc...
	/// </summary>
	/// <remarks>
	/// Methods defined in ths class would always need some form of input data to work with.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Pramod Walse</item>
	///			<item name="review">Last design/code review:4/2/04, Pramod Walse</item>
	///			<item name="conformancereview">Conformance review:4/2/04, Pramod Walse</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="none">None</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: PlateUtility.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/PlateUtility.cs $</item>
	///			<item name="vssrevision">$Revision: 13 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 11/26/07 6:55a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	[Serializable]
	public partial class PlateUtility
	{
		#region Member Data
		/// <summary>
		/// All alphabets.
		/// </summary>
		private static string[] m_Alphabets = 			//Initialize Alphabets
				"A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
		/// <summary>
		/// For use with GetPlateRowLabel
		/// </summary>
		static string s_rowLabels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public PlateUtility()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets black or white color(for backcolor/forecolor usage).
		/// Default is White.
		/// </summary>
		/// <param name="color">Input Color</param>
		/// <returns>Color</returns>
		public static Color GetOppositeColor(Color color)
		{
			//Lime,Green,SkyBlue,Blue,Magenta,Brown,Navy,SlateBlue,Yellow,ForestGreen,
			//Cyan,Red,OrangeRed,DeepPink,Fuchsia,HotPink,Pink,LightPink,Chocolate,
			//DarkOrange,SandyBrown,Tan,LightYellow,Gold,MediumBlue,RoyalBlue,
			//CornflowerBlue,DodgerBlue,Aqua,LightCyan,DimGray,MidnightBlue,SteelBlue,
			//LightSlateGray,DarkSeaGreen,PowderBlue,BlueViolet,DarkViolet,SlateBlue,
			//LightSteelBlue,Gray,DarkGray,DarkGreen,LimeGreen,LightGreen,LawnGreen,
			//GreenYellow,Black
			if (color.Equals(Color.Lime) || color.Equals(Color.SkyBlue)
				|| color.Equals(Color.SlateBlue) || color.Equals(Color.Yellow)
				|| color.Equals(Color.Cyan) || color.Equals(Color.DeepPink)
				|| color.Equals(Color.Fuchsia) || color.Equals(Color.HotPink)
				|| color.Equals(Color.Pink) || color.Equals(Color.LightPink)
				|| color.Equals(Color.Tan) || color.Equals(Color.LightYellow)
				|| color.Equals(Color.Gold) || color.Equals(Color.Aqua)
				|| color.Equals(Color.LightCyan) || color.Equals(Color.DimGray)
				|| color.Equals(Color.LightSlateGray) || color.Equals(Color.Gray)
				|| color.Equals(Color.LimeGreen) || color.Equals(Color.LightGreen)
				|| color.Equals(Color.GreenYellow) || color.Equals(Color.White)
				|| color.Equals(Color.DarkGray)
				)
			{
				return Color.Black;
			}
			if (color.Equals(Color.Green)
				|| color.Equals(Color.Blue) || color.Equals(Color.Magenta)
				|| color.Equals(Color.Brown) || color.Equals(Color.Navy)
				|| color.Equals(Color.ForestGreen) || color.Equals(Color.Red)
				|| color.Equals(Color.OrangeRed) || color.Equals(Color.Chocolate)
				|| color.Equals(Color.DarkOrange) || color.Equals(Color.SandyBrown)
				|| color.Equals(Color.MediumBlue) || color.Equals(Color.RoyalBlue)
				|| color.Equals(Color.CornflowerBlue) || color.Equals(Color.DodgerBlue)
				|| color.Equals(Color.MidnightBlue) || color.Equals(Color.SteelBlue)
				|| color.Equals(Color.DarkSeaGreen) || color.Equals(Color.PowderBlue)
				|| color.Equals(Color.BlueViolet) || color.Equals(Color.DarkViolet)
				|| color.Equals(Color.SlateBlue) || color.Equals(Color.LightSteelBlue)
				|| color.Equals(Color.LawnGreen)
				|| color.Equals(Color.DarkGreen) || color.Equals(Color.Black)
				)
			{
				return Color.White;
			}
			return Color.White;
		}
		/// <summary>
		/// Gets PlateIndex by provided well name(like A1, H8 etc). Assumes that rows are 
		/// named by alphabets(one) and columns are named by numbers.
		/// </summary>
		/// <param name="wellName">Well Name like A1, H12 etc..</param>
		/// <param name="rows">Number of rows on plate.</param>
		/// <param name="columns">Number of columns on plate.</param>
		/// <returns>PLate Index value</returns>
		public static int GetPlateIndexByWellName(string wellName, int rows, int columns)
		{
			//Get row name and column number.
			string rowName = wellName.Substring(0, 1).ToUpper();
			int columnNumber = int.Parse(wellName.Substring(1)) - 1;

			int currentLocation = 0;
			//Run thru each row. 
			for (int row = 0; row < rows; row++)
			{
				if (m_Alphabets[row].ToString().Equals(rowName))
				{
					//Run thru each column.
					for (int column = 0; column < columns; column++)
					{
						if (columnNumber.Equals(column))
						{
							return currentLocation + column;
						}
					}
				}
				currentLocation = currentLocation + columns;
			}
			throw new ApplicationException("Invalid well name provided.");
		}
		/// <summary>
		/// Gets WellName (like A1, H8 etc) by provided PlateIndex . Assumes that rows are 
		/// named by alphabets(one) and columns are named by numbers.
		/// </summary>
		/// <param name="plateIndex">Plate Index for well.</param>
		/// <param name="rows">Number of rows on plate.</param>
		/// <param name="columns">Number of columns on plate.</param>
		/// <returns>Well Name like A1, H12 etc..</returns>
		public string GetWellNameByPlateIndex(int plateIndex, int rows, int columns)
		{
			int rowColumnValue = 0;
			long remainder = 0;
			//Run thru each row. 
			for (int row = 0; row < rows; row++)
			{
				rowColumnValue = (row + 1) * columns;
				if (plateIndex < rowColumnValue && plateIndex >= 0)
				{
					string wellName = m_Alphabets[row].ToString();
					Math.DivRem(plateIndex, columns, out remainder);
					int columnNumber = Convert.ToInt32(remainder) + 1;
					return String.Concat(wellName, columnNumber.ToString());
				}
			}
			throw new ApplicationException("Invalid plate index provided.");
		}
		/// <summary>
		/// Gets PlateIndex by provided location on grid.
		/// </summary>
		/// <param name="rowNumber">Row number, X position, zero based.</param>
		/// <param name="columnNumber">Column number, Y position, zero based.</param>
		/// <param name="rows">Number of rows on plate.</param>
		/// <param name="columns">Number of columns on plate.</param>
		/// <returns>Plate Index value</returns>
		public int GetPlateIndexByGridLocation(int rowNumber, int columnNumber,
			int rows, int columns)
		{
			int currentLocation = 0;
			//Run thru each row. 
			for (int row = 0; row < rows; row++)
			{
				if (row.Equals(rowNumber))
				{
					//Run thru each column.
					for (int column = 0; column < columns; column++)
					{
						if (columnNumber.Equals(column))
						{
							return currentLocation + column;
						}
					}
				}
				currentLocation = currentLocation + columns;
			}
			throw new ApplicationException("Invalid well name provided.");
		}
		/// <summary>
		/// Gets grid location (XY position of the cell)by provided PlateIndex . 
		/// Assumes that rows are named by alphabets(one) and columns are named by numbers.
		/// </summary>
		/// <param name="plateIndex">Plate Index for well.</param>
		/// <param name="rows">Number of rows on plate.</param>
		/// <param name="columns">Number of columns on plate.</param>
		/// <returns>A Point object with X and Y position.</returns>
		public Point GetGridLocationByPlateIndex(int plateIndex, int rows, int columns)
		{
			Point point = new Point();
			int rowColumnValue = 0;
			int column = 0;
			long remainder = 0;
			//Run thru each row. 
			for (int row = 0; row < rows; row++)
			{
				rowColumnValue = (row + 1) * columns;
				if (plateIndex < rowColumnValue && plateIndex >= 0)
				{
					Math.DivRem(plateIndex, columns, out remainder);
					column = Convert.ToInt32(remainder);
					point.X = row;
					point.Y = column;
					return point;
				}
			}
			throw new ApplicationException("Invalid plate index provided.");
		}
		/// <summary>
		/// Gets grid location (XY position of the cell)by 
		/// provided PlateIndex with rowspan>1 on grid. 
		/// Assumes that rows are named by alphabets(one) and columns are named by numbers.
		/// </summary>
		/// <param name="plateIndex">Plate Index for well.</param>
		/// <param name="rows">Number of rows on plate.</param>
		/// <param name="columns">Number of columns on plate.</param>
		/// <param name="rowspan">Roespan count for grid.</param>
		/// <returns>A Point object with X and Y position.</returns>
		public Point GetGridLocationByPlateIndexAndRowSpan(int plateIndex, int rows,
			int columns, int rowspan)
		{
			Point point = new Point();
			int rowColumnValue = 0;
			int column = 0;
			long remainder = 0;
			//Run thru each row. 
			for (int row = 0; row < rows; row++)
			{
				rowColumnValue = (row + 1) * columns;
				if (plateIndex < rowColumnValue && plateIndex >= 0)
				{
					Math.DivRem(plateIndex, columns, out remainder);
					column = Convert.ToInt32(remainder);
					//set new row.
					if (plateIndex >= columns)
					{
						row = row * rowspan;
					}
					point.X = row;
					point.Y = column;
					return point;
				}
			}
			throw new ApplicationException("Invalid plate index provided.");
		}
		/// <summary>
		/// Get the well label for a given zero based well index and number of plate columns.
		/// </summary>
		/// <param name="wellIndex">zero based well index.</param>
		/// <param name="numberColumns">number of columns on the plate.</param>
		/// <returns>well label</returns>
		public static string GetPlateWellLabelByWellIndex(int wellIndex, int numberColumns)
		{
			int row = wellIndex / numberColumns;
			int column = wellIndex % numberColumns;
			return GetPlateWellLabel(row, column);
		}
		/// <summary>
		/// gets the well label for a certain well coordinate.  Works for up to 384 well plates.
		/// </summary>
		/// <param name="row">row index (zero based)</param>
		/// <param name="column">column index (zero based)</param>
		/// <returns>the label in the form of "A1" etc.</returns>
		public static string GetPlateWellLabel(int row, int column)
		{
			return GetPlateWellLabel(row, column, 0);
		}
		/// <summary>
		/// gets the well label for a certain well coordinate and with a certain zero-padding for the column
		/// numbering.  This zero padding is necessary for the well coordinate string to alphabetize correctly
		/// in excel exports.  This method works for up to 384 well plates.
		/// </summary>
		/// <param name="row">row index (zero based)</param>
		/// <param name="column">column index (zero based)</param>
		/// <param name="forcedZeroPaddedLengthForColumn"></param>
		/// <returns>the label in the form of "A1" etc.</returns>
		public static string GetPlateWellLabel(int row, int column, int forcedZeroPaddedLengthForColumn)
		{
			StringBuilder label = new StringBuilder();
			label.Append(GetPlateRowLabel(row));
			label.Append(GetPlateColumnLabel(column, forcedZeroPaddedLengthForColumn));
			return label.ToString();
		}
		/// <summary>
		/// Get the microtitreplate row label.
		/// Works for up to 384 well plate.
		/// </summary>
		/// <param name="row">row</param>
		/// <returns>label</returns>
		public static string GetPlateRowLabel(int row)
		{
			// This function return value does not need to be localized because microtitre plates
			// have standard look throughout the world.
			System.Diagnostics.Debug.Assert(row >= 0 && row < s_rowLabels.Length);
			return new string(s_rowLabels[row], 1);
		}

		/// <summary>
		/// get the microtitreplate column label.
		/// works for any size plate.
		/// </summary>
		/// <param name="column">column</param>
		/// <returns>label</returns>
		public static string GetPlateColumnLabel(int column)
		{
			return (column + 1).ToString();
		}
		/// <summary>
		/// get the microtitreplate column label for a certain column.
		/// works for any size plate.
		/// </summary>
		/// <param name="column">column number.</param>
		/// <param name="forcedZeroPaddedLength">If the string representation of the column number has length less
		/// than this, then zeros will be prepended to pad it out to this length.  This allows for alphabetic sorting.
		/// </param>
		/// <returns></returns>
		public static string GetPlateColumnLabel(int column, int forcedZeroPaddedLength)
		{
			StringBuilder format = new StringBuilder("0");
			for (int i = 1; i < forcedZeroPaddedLength; i += 1)
				format.Append("0");
			return (column + 1).ToString(format.ToString());
		}
		#endregion
	}
}
