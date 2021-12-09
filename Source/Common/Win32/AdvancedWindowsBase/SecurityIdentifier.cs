using System;
using System.Runtime.InteropServices;
using System.Text;

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
	///			<item name="vssfile">$Workfile: SecurityIdentifier.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/AdvancedWindowsBase/SecurityIdentifier.cs $</item>
	///			<item name="vssrevision">$Revision: 19 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class SecurityIdentifier
	{
		#region Constants
		
		/// <summary>
		/// Enumeration of possible SID types.
		/// </summary>
		public enum SIDType : int
		{
			/// <summary>
			/// Indicates a user SID. 
			/// </summary>
			User= 1,
			/// <summary>
			/// Indicates a group SID.
			/// </summary>
			Group,
			/// <summary>
			/// Indicates a domain SID.
			/// </summary>
			Domain,
			/// <summary>
			/// Indicates an alias SID.
			/// </summary>
			Alias,
			/// <summary>
			/// Indicates a SID for a well-known group.
			/// </summary>
			WellKnownGroup,
			/// <summary>
			/// Indicates a SID for a deleted account.
			/// </summary>
			DeletedAccount,
			/// <summary>
			/// Indicates an invalid SID.
			/// </summary>
			Invalid,
			/// <summary>
			/// Indicates an unknown SID type.
			/// </summary>
			Unknown,
			/// <summary>
			/// Indicates a SID for a computer.
			/// </summary>
			Computer
		}

		/// <summary>
		/// Local Well-Known Security Identifiers.
		/// </summary>
		public class RIDs
		{
			/// <summary>
			/// The Administrators group has built-in capabilties that give its members full control over the system. 
			/// The group is the default owner of any object that is created by a member of the group.
			/// </summary>
			/// <remarks>
			/// After the initial installation of the operating system, the only member of the group is the Administrator account. 
			/// When a computer joins a domain, the Domain Admins group is added to the Administrators group. 
			/// When a server becomes a domain controller, the Enterprise Admins group also is added to the Administrators group.
			/// </remarks>
			public static readonly string Administrators	=	"S-1-5-32-544";
			/// <summary>
			/// Users can perform tasks such as running applications, using local and network printers, shutting down the computer, and locking the computer. 
			/// Users can install applications that only they are allowed to use if the installation program of the application supports per-user installation.
			/// </summary>
			/// <remarks>
			/// A built-in group. 
			/// After the initial installation of the operating system, the only member is the Authenticated Users group. 
			/// When a computer joins a domain, the Domain Users group is added to the Users group on the computer. 
			/// </remarks>
			public static readonly string Users				=	"S-1-5-32-545";
			/// <summary>
			/// The Guests group allows occasional or one-time users to log on with limited privileges to a computer's built-in Guest account.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the only member is the Guest account.
			/// </remarks>
			public static readonly string Guests			=	"S-1-5-32-546";
			/// <summary>
			/// Power Users can create local users and groups; modify and delete accounts that they have created; and remove users from the Power Users, Users, and Guests groups. 
			/// Power Users also can install most applications; create, manage, and delete local printers; and create and delete file shares.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the only member is the Guest account.
			/// </remarks>
			public static readonly string PowerUsers		=	"S-1-5-32-547";
			/// <summary>
			/// The Administrators group has built-in capabilties that give its members full control over the system. 
			/// The group is the default owner of any object that is created by a member of the group.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the group has no members. This group does not exist on domain controllers. 
			/// </remarks>
			public static readonly string AccountOperators	=	"S-1-5-32-548";
			/// <summary>
			/// Server Operators can log on to a server interactively; create and delete network shares; start and stop services; back up and restore files; format the hard disk of the computer; and shut down the computer.
			/// </summary>
			/// <remarks>
			/// A built-in group that exists only on domain controllers. By default, the group has no members. 
			/// </remarks>
			public static readonly string ServerOperators	=	"S-1-5-32-549";
			/// <summary>
			/// Print Operators can manage printers and document queues.
			/// </summary>
			/// <remarks>
			/// A built-in group that exists only on domain controllers. By default, the only member is the Domain Users group. 
			/// </remarks>
			public static readonly string PrintOperators	=	"S-1-5-32-550";
			/// <summary>
			/// Backup Operators can back up and restore all files on a computer, regardless of the permissions that protect those files. 
			/// Backup Operators also can log on to the computer and shut it down.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the group has no members.
			/// </remarks>
			public static readonly string BackupOperators	=	"S-1-5-32-551";
			/// <summary>
			/// In Windows NT domains, it is a built-in group used by the File Replication service on domain controllers.
			/// </summary>
			/// <remarks>
			/// Not used in Windows 2000. 
			/// </remarks>
			public static readonly string Replicators		=	"S-1-5-32-552";
		}

		/// <summary>
		/// Local Well-Known Security Identifiers.
		/// </summary>
		public enum eRIDS
		{
			/// <summary>
			/// The Administrators group has built-in capabilties that give its members full control over the system. 
			/// The group is the default owner of any object that is created by a member of the group.
			/// </summary>
			/// <remarks>
			/// After the initial installation of the operating system, the only member of the group is the Administrator account. 
			/// When a computer joins a domain, the Domain Admins group is added to the Administrators group. 
			/// When a server becomes a domain controller, the Enterprise Admins group also is added to the Administrators group.
			/// </remarks>
			Administrators	=	0x220,
			/// <summary>
			/// Users can perform tasks such as running applications, using local and network printers, shutting down the computer, and locking the computer. 
			/// Users can install applications that only they are allowed to use if the installation program of the application supports per-user installation.
			/// </summary>
			/// <remarks>
			/// A built-in group. 
			/// After the initial installation of the operating system, the only member is the Authenticated Users group. 
			/// When a computer joins a domain, the Domain Users group is added to the Users group on the computer. 
			/// </remarks>
			Users				=	0x221,
			/// <summary>
			/// The Guests group allows occasional or one-time users to log on with limited privileges to a computer's built-in Guest account.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the only member is the Guest account.
			/// </remarks>
			Guests			=	0x222,
			/// <summary>
			/// Power Users can create local users and groups; modify and delete accounts that they have created; and remove users from the Power Users, Users, and Guests groups. 
			/// Power Users also can install most applications; create, manage, and delete local printers; and create and delete file shares.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the only member is the Guest account.
			/// </remarks>
			PowerUsers		=	0x223,
			/// <summary>
			/// The Administrators group has built-in capabilties that give its members full control over the system. 
			/// The group is the default owner of any object that is created by a member of the group.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the group has no members. This group does not exist on domain controllers. 
			/// </remarks>
			AccountOperators=0x224,
			/// <summary>
			/// Server Operators can log on to a server interactively; create and delete network shares; start and stop services; back up and restore files; format the hard disk of the computer; and shut down the computer.
			/// </summary>
			/// <remarks>
			/// A built-in group that exists only on domain controllers. By default, the group has no members. 
			/// </remarks>
			ServerOperators=0x225,
			/// <summary>
			/// Print Operators can manage printers and document queues.
			/// </summary>
			/// <remarks>
			/// A built-in group that exists only on domain controllers. By default, the only member is the Domain Users group. 
			/// </remarks>
			PrintOperators=0x226,
			/// <summary>
			/// Backup Operators can back up and restore all files on a computer, regardless of the permissions that protect those files. 
			/// Backup Operators also can log on to the computer and shut it down.
			/// </summary>
			/// <remarks>
			/// A built-in group. By default, the group has no members.
			/// </remarks>
			BackupOperators=0x227,
			/// <summary>
			/// In Windows NT domains, it is a built-in group used by the File Replication service on domain controllers.
			/// </summary>
			/// <remarks>
			/// Not used in Windows 2000. 
			/// </remarks>
			Replicators=0x228

		}

		#endregion

		#region Member Data
		
		private IntPtr m_SIDPtr=IntPtr.Zero;

		private string m_SID;
		private string m_Account;
		private string m_Domain;
		private SIDType m_Type;
				
		#endregion

		#region Accessors
 
		/// <summary>
		/// Account accessor.
		/// </summary>
		public string Account
		{
			get
			{
				return m_Account;
			}
		}

		/// <summary>
		/// Domain accessor.
		/// </summary>
		public string Domain
		{
			get
			{
				return m_Domain;
			}
		}

		/// <summary>
		/// SID Type accessor.
		/// </summary>
		public SIDType Type
		{
			get
			{
				return (SIDType)m_Type;
			}
		}

		#endregion

		#region Constructors and Destructor

		/// <summary>
		/// Constructor.
		/// <param name="sID">sid as string.</param>
		/// </summary>
		public SecurityIdentifier(string sID)
		{
			IntPtr sid;

			m_SID=sID;
			ConvertStringSidToSid(m_SID, out sid);

			Update(sid);
		}

		/// <summary>
		/// Constructor.
		/// <param name="sID">sid as string.</param>
		/// </summary>
		public SecurityIdentifier(IntPtr sID)
		{
			Update(sID);
		}

		/// <summary>
		/// Constructor.
		/// <param name="sID">sid as string.</param>
		/// </summary>
		public SecurityIdentifier(byte[] sID)
		{
			int bufferSize = 64;
			uint ubufferSize=(uint)bufferSize;
			
			System.Text.StringBuilder account = new StringBuilder(bufferSize);
			System.Text.StringBuilder domain = new StringBuilder(bufferSize);
			System.Text.StringBuilder sid = new StringBuilder(bufferSize);
			uint sidUse;

			if(0==LookupAccountSid(null, sID, account, ref ubufferSize, domain, ref ubufferSize, out sidUse))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
			else
			{
				IntPtr lptr=IntPtr.Zero;

				m_Account=account.ToString();
				m_Domain=domain.ToString();
				m_Type=(SIDType)sidUse;
				ConvertSidToStringSid(sID, out lptr);
				m_SID=Marshal.PtrToStringAuto(lptr);
			}
		}

		/// <summary>
		/// SecurityIdentifier
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="domainMachineName"></param>
		public SecurityIdentifier(string userName, string domainMachineName)
		{
			byte[] sid = null;
			uint cbSid = 0;
			StringBuilder referencedDomainName = new StringBuilder();
			uint cchReferencedDomainName = (uint)referencedDomainName.Capacity;
			SIDType sidUse;

			System.Text.StringBuilder domainUserName=new StringBuilder();

			if(domainMachineName!=null && domainMachineName.Length>0)
			{
				domainUserName.Append(domainMachineName);
				domainUserName.Append("\\");
			}

			domainUserName.Append(userName);
			
			int err = (int)BioRad.Win32.Kernel.ErrorCodes.SUCCESS;
			if (!LookupAccountName(null, domainUserName.ToString(), sid, ref cbSid,referencedDomainName,ref cchReferencedDomainName,out sidUse))
			{
				err = Marshal.GetLastWin32Error();
				if (err == (int)BioRad.Win32.Kernel.ErrorCodes.INSUFFICIENT_BUFFER)
				{
					sid = new byte[cbSid];
					referencedDomainName.EnsureCapacity((int)cchReferencedDomainName);
					err = (int)BioRad.Win32.Kernel.ErrorCodes.SUCCESS;
					if (!LookupAccountName(null, domainUserName.ToString(),sid,ref cbSid,referencedDomainName,ref cchReferencedDomainName,out sidUse))
						err = Marshal.GetLastWin32Error();
				}
			}
			if (err == (int)BioRad.Win32.Kernel.ErrorCodes.SUCCESS)
			{
				IntPtr ptrSid;
				if (!ConvertSidToStringSid(sid,out ptrSid))
					BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
				else
				{
					IntPtr psid=IntPtr.Zero;
					m_SID = Marshal.PtrToStringAuto(ptrSid);
					BioRad.Win32.Kernel.APIBuffer.Dispose(ptrSid);
					ConvertStringSidToSid(m_SID, out psid);

					Update(psid);
				}
			}
			else
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
		}
		
		/// <summary>
		/// Update
		/// </summary>
		/// <param name="sID"></param>
		private void Update(IntPtr sID)
		{
			int bufferSize = 64;
			uint ubufferSize=(uint)bufferSize;
			
			System.Text.StringBuilder account = new StringBuilder(bufferSize);
			System.Text.StringBuilder domain = new StringBuilder(bufferSize);
			uint sidUse;

			if(0==LookupAccountSid(Environment.MachineName, sID, account, ref ubufferSize, domain, ref ubufferSize, out sidUse))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
			else
			{
				m_Account=account.ToString();
				m_Domain=domain.ToString();
				m_Type=(SIDType)sidUse;

				IntPtr lptr=IntPtr.Zero;

				ConvertSidToStringSid(sID, ref lptr);
				m_SID = Marshal.PtrToStringAuto(lptr);
				m_SIDPtr=sID;
			}
		}
	
		#endregion

		#region Methods

		/// <summary>
		/// ToString.
		/// </summary>
		public override string ToString()
		{
			return m_SID.ToString();
		}

		#endregion

		#region Implicit Operators
 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sid"></param>
		/// <returns></returns>
		public static implicit operator System.IntPtr(BioRad.Win32.AdvancedWindowsBase.SecurityIdentifier sid) 
		{
			return sid.m_SIDPtr;
		}
 
		#endregion

		#region Unmanaged API

		[DllImport( "advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern bool ConvertSidToStringSid( IntPtr psid, ref IntPtr pstr );

		/// <summary>
		/// ConvertSidToStringSid. 
		/// </summary>
		[DllImport("advapi32", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern bool ConvertSidToStringSid(
			IntPtr ptrSid,
			[Out] System.Text.StringBuilder sid
			);

		[DllImport("advapi32", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern bool ConvertSidToStringSid(
			[MarshalAs(UnmanagedType.LPArray)] byte [] pSID, 
			out IntPtr ptrSid);
		
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern uint ConvertStringSidToSid(
			string StringSid,
			out IntPtr ptrSid
			);
		
		/*
		/// <summary>
		/// Win32 Wrapper for NetLocalGroupEnum
		/// <param name="servername">Size of bufptr.</param>
		/// <param name="level">Buffer containing returned data from API call.</param>
		/// <param name="bufptr">Level of specific information to return. (See API call documentation.).</param>
		/// <param name="prefmaxlen">Specifies the preferred maximum length of returned data, in bytes.</param>
		/// <param name="entriesread">Value set to number of groups read limited by bufptr size.</param>
		/// <param name="totalentries">Value set to number of groups.</param>
		/// <param name="resume_handle">Handle to reference if using sequential API calls to gather group data.</param>
		/// </summary>
*/
		[DllImport( "advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern uint LookupAccountSid( 
			string systemName,
			[MarshalAs(UnmanagedType.LPArray)] byte[] sid,
			System.Text.StringBuilder accountName,
			ref uint cbAccount,
			[Out] System.Text.StringBuilder domainName,
			ref uint cbDomainName,
			out uint use 
			);

		[DllImport( "advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern uint LookupAccountSid( 
			string systemName,
			IntPtr sid,
			System.Text.StringBuilder accountName,
			ref uint cbAccount,
			[Out] System.Text.StringBuilder domainName,
			ref uint cbDomainName,
			out uint use 
			);

		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError = true)]
		static extern bool LookupAccountName (
			string lpSystemName,
			string lpAccountName,
			[MarshalAs(UnmanagedType.LPArray)] byte[] sid,
			ref uint cbSid,
			StringBuilder ReferencedDomainName,
			ref uint cchReferencedDomainName,
			out SIDType peUse);        

		#endregion	
	}
}
