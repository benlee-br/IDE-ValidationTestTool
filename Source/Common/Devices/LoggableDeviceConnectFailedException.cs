using System;
using System.Globalization;
using System.Runtime.Serialization;
using BioRad.Common.ApplicationExceptions;
using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common.Devices
{
	#region Documentation Tags
	/// <summary>
	/// Class Summary
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
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
	///			<item name="vssfile">$Workfile: LoggableDeviceConnectFailedException.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Devices/LoggableDeviceConnectFailedException.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class LoggableDeviceConnectFailedException : LoggableApplicationException
	{	
		#region Constants
		/// <summary>
		/// Serialization ID tag for m_FailedDevice
		/// </summary>
		private const string c_DeviceTag = @"LoggableDeviceConnectFailedException.Device";
		#endregion

		#region Member Data
		private Device m_FailedDevice = null;
		#endregion

		#region Accessors
		/// <summary>
		/// Clone of Device which failed to connect.
		/// </summary>
		public Device Device
		{
			get
			{
				return m_FailedDevice;
			}
		}
		#endregion

		#region Constructors and Destructor
        ///	<summary>
        ///	Construct and log exception	from the given parameters.
        ///	The	sender parameter is	used to	determine the appropriate log.
        ///	</summary>
        ///	<param name="sender">Object	originating	the	exception. If object implements
        ///	IProvidesLogName the exception is logged to	that log, otherwise	it is logged
        ///	to the default log.</param>
        ///	<param name="ds">Severity of event.</param>
        ///	<param name="dt">Tag identifying originating subsystem or operation.</param>
        ///	<param name="device">Device that failed to connect (will be cloned).</param>
        ///	<param name="message">The localized message.</param>
        public LoggableDeviceConnectFailedException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            Device device, string message)
            : base(sender, ds, dt, message)
        {
            if (device != null)
                m_FailedDevice = (Device)device.Clone();
        }

        ///	<summary>
        ///	Construct and log a	loggable application exception from	the	given parameters.
        ///	The	sender parameter is	used to	determine the appropriate log.
        ///	</summary>
        ///	<param name="sender">Object	originating	the	exception. If object implements
        ///	IProvidesLogName the exception is logged to	that log, otherwise	it is logged
        ///	to the default log.</param>
        ///	<param name="ds">Severity of event.</param>
        ///	<param name="dt">Tag identifying originating subsystem or operation.</param>
        ///	<param name="device">Device that failed to connect (will be cloned).</param>
        ///	<param name="message">The	localized message.</param>
        ///	<param name="innerException">Inner exception for this exception.</param>
        public LoggableDeviceConnectFailedException(Object sender, DiagnosticSeverity ds, DiagnosticTag dt,
            Device device, string message, Exception innerException)
            : base(sender, ds, dt, message, innerException)
        {
            if (device != null)
                m_FailedDevice = (Device)device.Clone();
        }
        ///	<summary>
		///	De-serialization constructor. Required for remoted exceptions.
		///	</summary>
		///	<param name="info">Holds the data needed to	deserialize	the	object.</param>
		///	<param name="context">Context for the serialization	stream.</param>
		protected LoggableDeviceConnectFailedException(SerializationInfo info, StreamingContext context): base	(info, context)
		{
			// deserialize simple fields
			m_FailedDevice = (Device) info.GetValue(c_DeviceTag, typeof(Device));
		}
		#endregion

		#region Methods
		/// <summary>
		/// Serialization method. Required for remoted exceptions. Calls base method,
		/// then explicitly serializes object fields.
		/// </summary>
		/// <param name="info">Used to serialize object data. </param>
		/// <param name="context">Context for the serialization stream.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(c_DeviceTag, m_FailedDevice);
		}
		#endregion
	}
}
