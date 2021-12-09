using System;

namespace BioRad.Common.ApplicationExceptions
{
	#region Documentation Tags
	/// <summary>
	/// The ApplicationExceptions package implements the framework for creating
	/// custom iQ2 exception classes, primarily by providing the <see cref="LoggableApplicationException"/>
	/// class as a base class for custom exceptions.
	/// These custom exceptions enhance structured exception
	/// handling in a number of ways.
	/// <list type="bullet">
	///		<item>
	///			They are fully serializable, so may be thrown accross
	///			process and application boundaries. 
	///		</item>
	///		<item>
	///			They embed support for automatic fault logging.
	///		</item>
	///		<item>
	///			The exception message is localized to the current culture and
	///			supports relocalization to any supported culture.
	///		</item>
	///		<item>
	///			Type-based exception hierarchies allow exception handlers to catch
	///			only those exceptions for which the handler needs to perform
	///			specific clean-up.
	///		</item>
	///		<item>
	///			Type-based exception hierarchies allow exception handlers to catch
	///			a class of exceptions and create and rethrow a more specific exception
	///			adding contextual information known to the exception handler.
	///		</item>
	/// </list>
	/// </summary>
	/// <remarks>
	/// All exception processing should be done using appropriate combinations of
	/// the try-catch-finally blocks directly supported by the C# language.
	/// <para>
	/// Deciding when to catch exceptions, and whether to rethrow the same exception
	/// or to nest the caught exception in a newly created custom exception is more
	/// of an art than an excact science. However, there are a number best practices
	/// guidelines that should be adhered to.
	/// </para>
	/// <para>
	/// Chapter 18: Exceptions in �Applied Microsoft .NET Framework Programming� 
	/// by Jeffrey Richter is a useful reference for exception design decisions.
	/// </para>
	/// <list type="bullet">
	/// <item>
	/// Create a new exception type to allow exception handlers to detect a specific
	/// exception. In general, exception hierarchies are useful only when an 
	/// exception handler will want to filter exceptions based on a level in the 
	/// hierarchy.
	/// </item>
	/// <item>
	/// In many situations, an exception is caught in order to perform specific 
	/// error handling, and then either that exception or a new, more specific, 
	/// exception is thrown. Steps should be taken to preserve as much information 
	/// as possible in each case.
	/// <para>
	/// When rethrowing the same exception, use the contruct �throw� rather than
	/// �throw e� so as to preserve the stack starting point (<see cref="Exception.StackTrace"/> property)
	/// for the exception, as in:
	/// <code>
	/// try { � }
	///	catch (Exception e) 
	///	{
	///		�
	///		throw;
	///	}
	/// </code>
	/// </para>
	/// </item>
	/// <item>
	/// In some situations an exception is caught and a new, more specific 
	/// exception is thrown in its place. In this case it is important to 
	/// preserve information about the original source of the exception. 
	/// To do this, the inner exception property of the new exception should 
	/// refer to the caught exception. This is accomplished by using an 
	/// exception constructor that takes an inner exception parameter.
	/// To support this, all derived exception types need to implement public
	/// constructors that take an instance of an Exception-derived type.
	/// </item>
	/// <item>
	/// When an exception is thrown as a result of a method call on a remote 
	/// object, the exception is transparently marshaled to the client. Remoting 
	/// configuration determines if the full exception information is marshaled 
	/// or if the information is filtered.
	/// </item>
	/// </list>
	///  All iQ2 custom exceptions should be derived from <see cref="LoggableApplicationException"/> . 
	/// <list type="number">
	/// <item>
	/// <term>Exception Construction</term>
	/// <description>
	/// The base <see cref="Exception"/> type has three public constructors: a default 
	/// constructor, a constructor that defines a string message, and a 
	/// constructor that defines a string message and an inner exception. 
	/// Our <see cref="LoggableApplicationException"/>  type defines constructors corresponding 
	/// to the base constructors that do and do not define an inner exception, 
	/// as well as a de-serialization constructor. Constructors are provided 
	/// to allow construction without logging, to log to a specific log file, 
	/// and to derive the appropriate log from the sender�s base type.
	/// <para>
	/// Derived types based on loggable application exceptions must at minimum 
	/// supply a de-serialization constructor and a pair of constructors (with 
	/// and without inner exceptions) that defer to the base constructors.
	/// </para>
	/// <para>
	/// <see cref="LoggableApplicationException"/> types are constructed with a message resource 
	/// object rather than a simple string message. This object allows the message 
	/// to be re-localized to either the current or a specific culture.
	/// </para>
	/// </description>
	/// </item>
	/// <item>
	/// <term>Exception Properties</term>
	/// <description>
	/// Exceptions may require parameterized messages, for example an exception 
	/// reporting an invalid plate setup file may wish to report the file name.
	/// If the parameter is to be exposed as a custom property, 
	/// the exception constructors must take and store one or more parameters 
	/// along with the message resource object.
	/// <para>
	/// The <see cref="LoggableApplicationException.Message"/>  property is overridden in this case to provide a formatted 
	/// string based on the localized message and the contained parameters.
	/// </para>
	/// <para>
	/// Note that if it is not necessary
	/// to expose the parameter as a custom property it can simply be embedded within
	/// the parameterized BioRad.Common.StringResources.StringResource  object.
	/// </para>
	/// </description>
	/// </item>
	/// <item>
	/// <term>Exception Serialization</term>
	/// <description>
	/// All exceptions must implement serialization so that they can be passed 
	/// across remoting boundaries. In all cases, the [Serializable] attribute (<see cref="SerializableAttribute"/>)
	/// must be applied to the derived class and the class must implement the 
	/// <see cref="System.Runtime.Serialization.ISerializable"/> interface. Where no additional fields are defined it is 
	/// sufficient to define the special protected (private if the type is sealed) 
	/// de-serialization constructor which invokes the base de-serialization 
	/// constructor. In cases where a derived exception defines additional fields, 
	/// the exception must also override the <see cref="LoggableApplicationException.GetObjectData"/> method, call the 
	/// base method, and explicitly serialize the added fields, as well as 
	/// explicitly de-serialize the added fields in the de-serialization constructor.
	/// <para>
	/// Thought must be given to the meaning of the field in the new process 
	/// context when explicitly serializing fields across the remote boundary. 
	/// For example, a handle is unlikely to have the same meaning.
	/// </para>
	/// <para>
	/// Note that the serialization tag used for each field must be unique. 
	/// To prevent collisions in derived types it is recommended that the 
	/// type name be pre-pended to the field name to create the serialization tag 
	/// string.
	/// </para>
	/// </description>
	/// </item>
	/// </list>
	/// <para>
	/// Chapter 18: Exceptions in �Applied Microsoft .NET Framework Programming� 
	/// by Jeffrey Richter contains detailed instructions on how to design exception constructors 
	/// and support serialization. However, his example of using Explicit Interface 
	/// Member Implementation for <see cref="LoggableApplicationException.GetObjectData"/> should not be followed unless the 
	/// class is sealed. Instead, <see cref="LoggableApplicationException.GetObjectData"/> should be implemented as a public 
	/// virtual method so that it can be invoked from a derived class using: 
	/// <code>
	/// base.GetObjectData()
	/// </code>
	/// </para>
	/// Another good reference is a FAQ on the website www.ingorammer.com.
	/// <seealso cref="LoggableApplicationException"/>
	/// </remarks>
	/// <example>
	/// Defining derived custom exceptions:
	/// <code>
	///using System;
	///using System.Runtime.Serialization;
	///using BioRad.Common.ApplicationExceptions;
	///using BioRad.Common.DiagnosticsLogger;
	///using BioRad.Common.StringResources;
	///
	///namespace BioRad.Common.Sample
	///{
	///	// Demonstrates minumum custom exception implementation
	///	public class CustomException: LoggableApplicationException
	///	{
	///		// Constructor if no inner exception. Defers to constructor below with
	///		// null inner exception.
	///		public CustomException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt, 
	///			StringResource message) : this( sender, ds, dt, message, (Exception) null)
	///		{
	///		}
	///
	///		// Constructor wrapps an inner exception. Defers to base class constructor
	///		// with no logging, then logs this exception.
	///		public CustomException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt, 
	///			StringResource message, Exception innerException)
	///			: base(ds, dt, message, innerException,  String.Empty, false)
	///		{
	///			this.Log(sender);
	///		}
	///
	///		// Required deserialization constructor defers to base class deserialization constructor
	///		protected CustomException(SerializationInfo info, StreamingContext context): 
	///			base (info, context)
	///		{
	///		}
	///	}
	///
	///	// Demonstrates custom exception implementation for exceptions that define
	///	// additional fields, in this case a FileName field.
	///	public class CustomParameterizedException: LoggableApplicationException
	///	{
	///		//	Serialization ID tag for m_FileName. Convention is TypeName.FieldName.
	///		private const string c_FileNameTag = @"CustomParameterizedException.FileName";
	///
	///		// Backing variable for added property.
	///		private string m_FileName;
	///
	///		// Constructor if no inner exception. Defers to constructor below with
	///		// null inner exception.
	///		public CustomParameterizedException(string fileName, Object sender, DiagnosticSeverity ds, DiagnosticTag dt, 
	///			StringResource message) : this( fileName, sender, ds, dt, message, (Exception) null)
	///		{
	///		}
	///
	///		// Constructor wrapps an inner exception. Defers to protected class constructor
	///		// with no logging, then logs this exception.
	///		public CustomParameterizedException(string fileName, Object sender, DiagnosticSeverity ds, DiagnosticTag dt, 
	///			StringResource message, Exception innerException)
	///			: this(fileName, ds, dt, message, innerException,  String.Empty, false)
	///		{
	///			this.Log(sender);
	///		}
	///
	///		// Required deserialization constructor defers to base class deserialization constructor,
	///		// then deserializes added fields
	///		protected CustomParameterizedException(SerializationInfo info, StreamingContext context):
	///			base (info, context)
	///		{
	///			// deserialize added fields
	///			m_FileName = info.GetString(c_FileNameTag);
	///		}
	///
	///		// Define this constructor when initializing additional fields.
	///		protected CustomParameterizedException(string fileName, DiagnosticSeverity ds, DiagnosticTag dt, 
	///			StringResource message, Exception innerException, string logName, bool logException)
	///			: base(ds, dt, message, innerException,  logName, logException)
	///		{
	///			// Initialize additional fields here.
	///			m_FileName = fileName;
	///		}
	///
	///		// Example of an exposed additional field.
	///		public string FileName {get {return m_FileName;}}
	///
	///		// Required serialization method override. Calls base method,
	///		// then explicitly serializes object fields.
	///		public override void GetObjectData(SerializationInfo info, StreamingContext context)
	///		{
	///			base.GetObjectData(info, context);
	///			info.AddValue(c_FileNameTag, m_FileName);
	///		}
	///
	///		// Overrides Message accessor to add FileName to message.
	///		public override string Message 
	///		{
	///			get
	///			{ 
	///				// Actual implementation should create the string resource using
	///				// an associated string resource class - see AboutStringResources.
	///				// This example uses a non-localized string resource for simplicity
	///				object[] param = {this.FileName};
	///				StringResource sr = new StringResource("Filename is {0}.", param); 
	///				// Concatenate this exception-specific message with base class message.
	///				string str = String.Concat(base.Message,sr.ToString());
	///				return str;
	///			}
	///		}
	///	}
	///}
	/// </code>
	///	Throwing custom exceptions:
	///	<code>
	///	// Actual implementation should use a localized string resource obtained
	///	// from an associated string resource class.
	///	StringResource sr = new StringResource("Throwing an example exception.");
	///	try
	///	{
	///		// Create exception with appropriate severity and tag. Tag is used
	///		// for filtering in the diagnostic log. Severity is used both by UI exception
	///		// reporting mechanism and by diagnostic log for filtering.
	///		CustomException ex = new CustomException(
	///			this, 
	///			DiagnosticSeverity.Serious, 
	///			DiagnosticTag.Unassigned, 
	///			sr);
	///		throw (ex);
	///	}
	///	catch (CustomException ex)
	///	{
	///		try
	///		{
	///			// Displaying custom exception message
	///			Console.WriteLine(ex.Message);
	///			// Another example of exception throwing, this time with a parameter
	///			// and a nested exception. 
	///			throw new CustomParameterizedException(
	///				"MyFile", 
	///				this, 
	///				DiagnosticSeverity.Warning, 
	///				DiagnosticTag.Unassigned, 
	///				new StringResource(String.Empty),
	///				ex);
	///		}
	///		catch (CustomParameterizedException exp)
	///		{
	///			// Displaying custom parameterized exception message
	///			Console.WriteLine(exp.Message);
	///		}
	///	}
	///	</code>
	/// </example>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
	///			<item name="review">Last design review: 1/22/04</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">901</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\ApplicationExceptions.wmf">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: AboutApplicationExceptions.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/ApplicationExceptions/AboutApplicationExceptions.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public sealed partial class AboutApplicationExceptions
	{
	}
}
