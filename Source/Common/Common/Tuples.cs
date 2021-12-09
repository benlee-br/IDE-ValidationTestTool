using System;
using System.Text;

namespace BioRad.Common.Common
{
	#region Documentation Tags
	/// <summary>Contains two objects of specified types.</summary>
	/// <typeparam name="T1">type of first object.</typeparam>
	/// <typeparam name="T2">type of second object.</typeparam>
	/// <remarks></remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:TH</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: Tuples.cs $</item>
	///			<item name="vssfilepath">$Archive: /CFX_HRM/Source/Core/Common/Common/Tuples.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 9/29/08 6:00p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public class Duple<T1, T2>
	{
		/// <summary>first item in the Duple.</summary>
		private T1 m_Item1;
		/// <summary>second item in the Duple.</summary>
		private T2 m_Item2;

		#region Accessors
		/// <summary>first item in the Duple.</summary>
		public T1 Item1
		{
			get { return m_Item1; }
			set { m_Item1 = value; }
		}
		/// <summary>second item in the Duple.</summary>
		public T2 Item2
		{
			get { return m_Item2; }
			set { m_Item2 = value; }
		}
		#endregion

		#region Constructors
		/// <summary>Constructs a new Duple with the given initial items.</summary>
		/// <param name="item1">initial Item1</param>
		/// <param name="item2">initial Item2</param>
		public Duple(T1 item1, T2 item2)
		{
			m_Item1 = item1;
			m_Item2 = item2;
		}
		#endregion

		#region Methods
		/// <summary>value equals comparer.</summary>
		/// <param name="other">object to compare to</param>
		/// <returns>true if value equals, else false.</returns>
		public override bool Equals(object other)
		{
			Duple<T1, T2> otherDuple = other as Duple<T1, T2>;
			if (otherDuple != null)
				return Item1.Equals(otherDuple.Item1) && Item2.Equals(otherDuple.Item2);
			else
				return base.Equals(other);
		}
		/// <summary>Gets a hash code for this object</summary>
		/// <returns>See summary.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return Item1.GetHashCode() + Item2.GetHashCode();
			}
		}
		#endregion
	}
	#region Documentation Tags
	/// <summary>Contains three objects of specified types.</summary>
	/// <typeparam name="T1">type of first object.</typeparam>
	/// <typeparam name="T2">type of second object.</typeparam>
	/// <typeparam name="T3">type of third object.</typeparam>
	/// <remarks></remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:TH</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: Tuples.cs $</item>
	///			<item name="vssfilepath">$Archive: /CFX_HRM/Source/Core/Common/Common/Tuples.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 9/29/08 6:00p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	public class Triple<T1, T2, T3>
	{
		/// <summary>first item in the Triple.</summary>
		private T1 m_Item1;
		/// <summary>second item in the Triple.</summary>
		private T2 m_Item2;
		/// <summary>third item in Triple.</summary>
		private T3 m_Item3;

		#region Accessors
		/// <summary>first item in the Triple.</summary>
		public T1 Item1
		{
			get { return m_Item1; }
			set { m_Item1 = value; }
		}
		/// <summary>second item in the Triple.</summary>
		public T2 Item2
		{
			get { return m_Item2; }
			set { m_Item2 = value; }
		}
		/// <summary>third item in the Triple.</summary>
		public T3 Item3
		{
			get { return m_Item3; }
			set { m_Item3 = value; }
		}
		#endregion

		#region Constructors
		/// <summary>Constructs a new Duple with the given initial items.</summary>
		/// <param name="item1">initial Item1</param>
		/// <param name="item2">initial Item2</param>
		/// <param name="item3">initial Item3</param>
		public Triple(T1 item1, T2 item2, T3 item3)
		{
			m_Item1 = item1;
			m_Item2 = item2;
			m_Item3 = item3;
		}
		#endregion

		#region Methods
		/// <summary>value equals comparer.</summary>
		/// <param name="other">object to compare to</param>
		/// <returns>true if value equals, else false.</returns>
		public override bool Equals(object other)
		{
			Triple<T1, T2, T3> otherTriple = other as Triple<T1, T2, T3>;
			if (otherTriple == null)
				return base.Equals(other);

			return
				Item1.Equals(otherTriple.Item1) &&
				Item2.Equals(otherTriple.Item2) &&
				Item3.Equals(otherTriple.Item3);
		}
		/// <summary>Gets a hash code for this object</summary>
		/// <returns>See summary.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return Item1.GetHashCode() + Item2.GetHashCode() + Item3.GetHashCode();
			}
		}
		#endregion
	}
}
