using System;
using System.Runtime.Serialization;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>
	/// Throw this exception to indicate an operation has been cancelled.
	/// Exception can be expicitly caught and used to initialize after a
	/// cancelled operation.
	/// </summary>
	/// <remarks>
	/// This exception is not automatically logged, and is not intended to
	/// percolate to the top level.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Lisa von Schlegell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:</item>
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
	///			<item name="vssfile">$Workfile: CancelOperationException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ApplicationExceptions/CancelOperationException.cs $</item>
	///			<item name="vssrevision">$Revision: 2 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	[Serializable]
	public partial class CancelOperationException : ApplicationException
	{
		/// <summary>
		/// Defers to base type constructor.
		/// </summary>
		/// <param name="message">exception message</param>
		public CancelOperationException( string message) : base(message)
		{
		}

		/// <summary>
		/// Defers to base type constructor.
		/// </summary>
		/// <param name="message">exception message</param>
		/// <param name="innerEx">inner exception</param>
		public CancelOperationException( string message, Exception innerEx): base(message, innerEx)
		{
		}

		/// <summary>
		/// De-serialization constructor. Required for remoted exceptions.
		/// </summary>
		/// <param name="info">Holds the data needed to deserialize the object.</param>
		/// <param name="context">Context for the serialization stream.</param>
		protected CancelOperationException(SerializationInfo info, StreamingContext context): 
			base (info, context)
		{
		}
	}
}
