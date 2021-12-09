using System;
using System.Text;
using System.Runtime.InteropServices;

namespace BioRad.Win32.Security
{
	#region Documentation Tags
	/// <summary>
	/// Class Summary
	/// </summary>
	/// <remarks>
	/// /// //TODO: PW Review: SystemInformation is part of .Net framework, do not use such names.
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
	///			<item name="vssfile">$Workfile: SystemInformation.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/Security/SystemInformation.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Pwalse $</item>
	///			<item name="vssdate">$Date: 9/14/06 1:42p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class SystemInformation
	{
		#region Constants

		/// <summary>
		/// EXTENDED_NAME_FORMAT
		/// </summary>
		/// <remarks>
		/// The EXTENDED_NAME_FORMAT enumeration type contains values that specify a format for a directory service object name.
		/// </remarks>
		public enum ExtendedNameFormat : int
		{
			/// <summary>
			/// Unknown name type. 
			/// </summary>
			Unknown = 0,
		
			/// <summary>
			/// Fully qualified distinguished name 
			/// </summary>
			/// <example>
			/// CN=Jeff Smith,OU=Users,DC=Engineering,DC=Microsoft,DC=Com
			/// </example>
			FullyQualifiedDN = 1,

			/// <summary>
			/// Windows NT� 4.0 account name
			/// </summary>
			/// <example>
			/// Engineering\JSmith
			/// </example>
			/// <remarks>
			/// The domain-only version includes trailing backslashes (\\). 
			/// </remarks>
			SamCompatible = 2,

			/// <summary>
			/// A "friendly" display name.
			/// </summary>
			/// <example>
			/// Jeff Smith
			/// </example>
			/// <remarks>
			/// The display name is not necessarily the defining relative distinguished name (RDN). 
			/// </remarks>
			Display = 3,

			/// <summary>
			/// GUID string that the IIDFromString function returns. 
			/// </summary>
			/// <example>
			/// {4fa050f0-f561-11cf-bdd9-00aa003a77b6}
			/// </example>
			UniqueId = 6,

			/// <summary>
			/// Complete canonical name.
			/// </summary>
			/// <example>
			/// engineering.microsoft.com/software/someone
			/// </example>
			/// <remarks>
			/// The domain-only version includes a trailing forward slash (/). 
			/// </remarks>
			Canonical = 7,

			/// <summary>
			/// User principal name.
			/// </summary>
			/// <example>
			/// someone@example.com
			/// </example>
			UserPrincipal = 8,

			/// <summary>
			/// Same as NameCanonical except that the rightmost forward slash (/) is replaced with a new line character (\n), even in a domain-only case.
			/// </summary>
			/// <example>
			/// engineering.microsoft.com/software\nJSmith
			/// </example>
			CanonicalEx = 9,

			/// <summary>
			/// Generalized service principal name.
			/// </summary>
			/// <example>
			/// www/www.microsoft.com@microsoft.com
			/// </example>
			ServicePrincipal = 10,

			/// <summary>
			/// The DNS domain name followed by a backward-slash and the SAM username.
			/// </summary>
			DnsDomain = 12
		}
	
		#endregion

        #region Member Data
        #endregion

        #region Accessors
        #endregion

		#region Delegates and Events
		#endregion

        #region Constructors and Destructor
        #endregion

        #region Methods

		/// <summary>
		/// The GetUserName function retrieves the name of the user or other security principal associated with the calling thread. 
		/// </summary>
		/// <remarks>
		/// You can specify the format of the returned name.
		/// If the thread is impersonating a client, GetUserName returns the name of the client.
		/// GetUserNameEx wrapper.
		/// </remarks>
		/// <param name="Information"></param>
		/// <returns></returns>
		public static string GetUserName(ExtendedNameFormat Information)
		{
			ulong size=64;
			StringBuilder namebuffer=new StringBuilder((int)size);
			int err=(int)BioRad.Win32.Kernel.ErrorCodes.SUCCESS;
			bool success=false;

			while(!(success=GetUserNameEx((int)Information, namebuffer, out size) && size<1000))
			{
				err = Marshal.GetLastWin32Error();
				if (err == (int)BioRad.Win32.Kernel.ErrorCodes.MORE_DATA)
				{
					namebuffer.EnsureCapacity(namebuffer.MaxCapacity*2);
					size*=2;
					err = (int)BioRad.Win32.Kernel.ErrorCodes.SUCCESS;
					success=GetUserNameEx((int)Information, namebuffer, out size);
						err = Marshal.GetLastWin32Error();
				}
			}
			
			if(success)
				return namebuffer.ToString();
			else
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError("BioRad.Win32.Security.GetUserName");

			return null;
		}

        #endregion

		#region Unmanaged API

		/// <summary>
		/// Entry point for GetUserNameEx.
		/// </summary>
		/// <param name="NameFormat"></param>
		/// <param name="lpNameBuffer"></param>
		/// <param name="nSize"></param>
		/// <returns></returns>
		[DllImport("Secur32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern bool GetUserNameEx(
			int NameFormat,
			StringBuilder lpNameBuffer,
			out ulong nSize
			);

		#endregion
	}
}
