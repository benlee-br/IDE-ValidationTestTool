using System;
using System.Text;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Wraps and object that implements the IForcedBeginEndUpdate interface and
	/// forces Begin and End methods to be called when used in an C#
	/// using statement.
	/// </summary>
	/// <remarks>
	/// <example>
	/// <code>
	/// public class MyObject : IForcedBeginEndUpdate
	/// {
	///		public void Begin()
	///		{
	///			// do begin stuff here....
	///		}
	///		public void End()
	///		{
	///			// do end stuff here....
	///		}
	/// }
	/// MyObject myObject = new MyObject();
	/// 
	/// // myObject.Begin() will get called in constructor of ForcedBeginEndUpdate.
	/// // myObject.End() will get called when ForcedBeginEndUpdate goes out of scope.
	/// using (ForcedBeginEndUpdate helper = new ForcedBeginEndUpdate(myObject))
	/// {
	///		// do your thing here....
	/// }
	/// </code>
	/// </example>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: ForcedBeginEndUpdate.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/ForcedBeginEndUpdate.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 6/21/07 12:48p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class ForcedBeginEndUpdate : IDisposable, IForcedBeginEndUpdate
	{
		#region Member Data
		private object m_ForcedBeginEndUpdateObject;
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public ForcedBeginEndUpdate(object obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (!(obj is IForcedBeginEndUpdate))
				throw new ArgumentException("obj", "obj does not implement interface IForcedBeginEndUpdate");

			m_ForcedBeginEndUpdateObject = obj;

			Begin();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Method called in constructor of ForcedBeginEndUpdate object.
		/// </summary>
		public virtual void Begin()
		{
			((IForcedBeginEndUpdate)m_ForcedBeginEndUpdateObject).Begin();
		}
		/// <summary>
		/// Method called in Dispose method when ForcedBeginEndUpdate object goes out of scope.
		/// </summary>
		public virtual void End()
		{
			((IForcedBeginEndUpdate)m_ForcedBeginEndUpdateObject).End();
			m_ForcedBeginEndUpdateObject = null;
		}
		/// <summary>Dispose method.</summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				End();
			}
		}
		/// <summary>Public Dispose method.</summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// 
		/// </summary>
		~ForcedBeginEndUpdate()
		{
			Dispose(false);
		}
		#endregion
	}
	/// <summary>
	/// Interface for forced begin and end update method pair.
	/// </summary>
	public interface IForcedBeginEndUpdate
	{
		/// <summary>
		/// Method called in constructor of ForcedBeginEndUpdate object.
		/// </summary>
		void Begin();
		/// <summary>
		/// Method called in Dispose method when ForcedBeginEndUpdate object goes out of scope.
		/// </summary>
		void End();
	}
}
