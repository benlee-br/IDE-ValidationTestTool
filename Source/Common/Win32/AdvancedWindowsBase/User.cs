using System;
using System.Runtime.InteropServices;

namespace BioRad.Win32.AdvancedWindowsBase
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
	///			<item name="vssfile">$Workfile: User.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/AdvancedWindowsBase/User.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class User
	{
		#region Constants

		/// <summary>
		/// The type of logon operation to perform.
		/// </summary>
		public enum Type
		{
			/// <summary>
			/// This logon type is intended for users who will be interactively using the computer,
			/// such as a user being logged on by a terminal server, remote shell, or similar process. 
			/// This logon type has the additional expense of caching logon information for 
			/// disconnected operations; therefore, it is inappropriate for some client/server 
			/// applications, such as a mail server. 
			/// </summary>
			Interactive=2
		}

		/// <summary>
		/// Specifies the logon provider.
		/// </summary>
		public enum Provider
		{
			/// <summary>
			/// Use the standard logon provider for the system. 
			/// The default security provider is negotiate, 
			/// unless you pass NULL for the domain name and the user name 
			/// is not in UPN format. In this case, the default provider is NTLM. 
			/// Windows 2000/NT:The default security provider is NTLM.
			/// </summary>
			Default=0
		}

		#endregion

        #region Member Data

		private IntPtr m_Token=IntPtr.Zero;

        #endregion

        #region Accessors
        #endregion

        #region Constructors and Destructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public User(){}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public User(string domain, string username, string password)
		{
			if ( username == null || password == null || domain == null )
				return;

			try
			{
				IntPtr dummy=IntPtr.Zero;
				IntPtr sid=IntPtr.Zero;
				IntPtr quota=IntPtr.Zero;
				IntPtr token=IntPtr.Zero;
				int ProfileLength=0;

				bool loginSuccess = LogonUserEx(
					username, 
					domain, 
					password,
					Type.Interactive, 
					Provider.Default, 
					out token,
					out dummy,
					out sid,
					out ProfileLength,
					out quota
					);

				if(loginSuccess)
					m_Token=token;
				else
					BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
			}
			catch(System.Exception ex)
			{
				Console.WriteLine(ex.Message);
				m_Token = IntPtr.Zero; 
			}

		}

        #endregion

        #region Methods
        #endregion

		#region Implicit Operators
 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static implicit operator System.IntPtr(User user) 
		{
			return user.m_Token;
		}
 
		#endregion

		#region DLL Imports

		/// <summary>
		/// The LogonUser function attempts to log a user on to the local computer. 
		/// The local computer is the computer from which LogonUser was called. 
		/// You cannot use LogonUser to log on to a remote computer. 
		/// You specify the user with a user name and domain, 
		/// and authenticate the user with a plaintext password. 
		/// If the function succeeds, you receive a handle to a token that 
		/// represents the logged-on user. You can then use this token handle 
		/// to impersonate the specified user or, in most cases, to create a 
		/// process that runs in the context of the specified user.
		/// </summary>
		/// <param name="lpszUsername">A pointer to a null-terminated string that specifies the name of the user.</param>
		/// <param name="lpszDomain">A pointer to a null-terminated string that specifies the name of the domain or server whose account database contains the lpszUsername account.</param>
		/// <param name="lpszPassword">A pointer to a null-terminated string that specifies the plaintext password for the user account specified by lpszUsername.</param>
		/// <param name="dwLogonType">The type of logon operation to perform.</param>
		/// <param name="dwLogonProvider">Specifies the logon provider.</param>
		/// <param name="phToken">A pointer to a handle variable that receives a handle to a token that represents the specified user. </param>
		/// <returns></returns>
		[DllImport("advapi32.DLL", SetLastError=true)] 
		private static extern int LogonUser(
			string lpszUsername, 
			string lpszDomain, 
			string lpszPassword, 
			Type dwLogonType, 
			Provider dwLogonProvider, 
			out IntPtr phToken
			); 

		[DllImport("advapi32.DLL", SetLastError=true)] 
		private static extern bool LogonUserEx(
			string lpszUsername,
			string lpszDomain,
			string lpszPassword,
			Type dwLogonType,
			Provider dwLogonProvider,
			out IntPtr phToken,
			out IntPtr ppLogonSid,
			out IntPtr ppProfileBuffer,
			out int pdwProfileLength,
			out IntPtr pQuotaLimits
			);
		#endregion
	}
}
