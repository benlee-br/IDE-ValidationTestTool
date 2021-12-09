using System;
using System.Runtime.InteropServices;

using BioRad.Win32.AdvancedWindowsBase;

namespace BioRad.Win32.NetworkManagement
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
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/User.cs $</item>
	///			<item name="vssrevision">$Revision: 15 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class User
	{
		#region Structures

		/// <summary>
		/// Structure for GROUP_USERS_INFO_0.
		/// </summary>
		[StructLayoutAttribute (LayoutKind.Sequential)]
		private struct Info0
		{
			#region IInfo0 Members

			/// <summary>
			/// Name.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Name;

			#endregion
		}
		
		/// <summary>
		/// Structure for GROUP_USERS_INFO_1.
		/// </summary>
		[StructLayoutAttribute (LayoutKind.Sequential)]
		private struct Info1
		{
			#region IInfo0 Members

			/// <summary>
			/// Name.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Name;

			#endregion

			#region IInfo1 Members

			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Password;

			/// <summary>
			/// PasswordAge.
			/// </summary>
			public uint PasswordAge;

			/// <summary>
			/// Privilege.
			/// </summary>
			public uint Privilege;

			/// <summary>
			/// Directory.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Directory;

			/// <summary>
			/// Comment.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Comment;

			/// <summary>
			/// Flags.
			/// </summary>
			public uint Flags;

			/// <summary>
			/// ScriptPath.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ScriptPath;

			#endregion
		}
		
		/// <summary>
		/// Structure for USER_INFO_2.
		/// </summary>
		[StructLayoutAttribute (LayoutKind.Sequential)]
		private struct Info2
		{
			#region IInfo0 Members

			/// <summary>
			/// Name.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Name;

			#endregion

			#region IInfo1 Members

			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Password;

			/// <summary>
			/// PasswordAge.
			/// </summary>
			public uint PasswordAge;

			/// <summary>
			/// Privilege.
			/// </summary>
			public uint Privilege;

			/// <summary>
			/// Directory.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Directory;

			/// <summary>
			/// Comment.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Comment;

			/// <summary>
			/// Flags.
			/// </summary>
			public uint Flags;

			/// <summary>
			/// ScriptPath.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ScriptPath;

			#endregion

			#region IInfo2 Members

			/// <summary>
			/// AuthorizationFlags.
			/// </summary>
			public uint AuthorizationFlags;
			/// <summary>
			/// FullName.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string FullName;
			/// <summary>
			/// UserComment.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string UserComment;
			/// <summary>
			/// Reserved.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Reserved;
			/// <summary>
			/// Workstations.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Workstations;
			/// <summary>
			/// LastLogon.
			/// </summary>
			public uint LastLogon;
			/// <summary>
			/// LastLogoff.
			/// </summary>
			public uint LastLogoff;
			/// <summary>
			/// AccountExpires.
			/// </summary>
			public uint AccountExpires;
			/// <summary>
			/// MaxStorage.
			/// </summary>
			public uint MaxStorage;
			/// <summary>
			/// UnitsPerWeek.
			/// </summary>
			public uint UnitsPerWeek;
			/// <summary>
			/// LogonHours.
			/// </summary>
			[MarshalAs(UnmanagedType.U1)] 
			public byte LogonHours;
			/// <summary>
			/// BadPasswordCount.
			/// </summary>
			public uint BadPasswordCount;
			/// <summary>
			/// NumberOfLogons.
			/// </summary>
			public uint NumberOfLogons;
			/// <summary>
			/// LogonServer.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string LogonServer;
			/// <summary>
			/// CountryCode.
			/// </summary>
			public uint CountryCode;
			/// <summary>
			/// CodePage.
			/// </summary>
			public uint CodePage;

			#endregion
		}
		/// <summary>
		/// Structure for USER_INFO_3.
		/// </summary>
		[StructLayoutAttribute (LayoutKind.Sequential)]
		private struct Info3
		{
			#region IInfo0 Members

			/// <summary>
			/// Name.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Name;

			#endregion

			#region IInfo1 Members

			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Password;

			/// <summary>
			/// PasswordAge.
			/// </summary>
			public uint PasswordAge;

			/// <summary>
			/// Privilege.
			/// </summary>
			public uint Privilege;

			/// <summary>
			/// Directory.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Directory;

			/// <summary>
			/// Comment.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Comment;

			/// <summary>
			/// Flags.
			/// </summary>
			public uint Flags;

			/// <summary>
			/// ScriptPath.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ScriptPath;

			#endregion

			#region IInfo2 Members

			/// <summary>
			/// AuthorizationFlags.
			/// </summary>
			public uint AuthorizationFlags;
			/// <summary>
			/// FullName.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string FullName;
			/// <summary>
			/// UserComment.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string UserComment;
			/// <summary>
			/// Reserved.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Reserved;
			/// <summary>
			/// Workstations.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Workstations;
			/// <summary>
			/// LastLogon.
			/// </summary>
			public uint LastLogon;
			/// <summary>
			/// LastLogoff.
			/// </summary>
			public uint LastLogoff;
			/// <summary>
			/// AccountExpires.
			/// </summary>
			public uint AccountExpires;
			/// <summary>
			/// MaxStorage.
			/// </summary>
			public uint MaxStorage;
			/// <summary>
			/// UnitsPerWeek.
			/// </summary>
			public uint UnitsPerWeek;
			/// <summary>
			/// LogonHours.
			/// </summary>
			[MarshalAs(UnmanagedType.U1)] 
			public byte LogonHours;
			/// <summary>
			/// BadPasswordCount.
			/// </summary>
			public uint BadPasswordCount;
			/// <summary>
			/// NumberOfLogons.
			/// </summary>
			public uint NumberOfLogons;
			/// <summary>
			/// LogonServer.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string LogonServer;
			/// <summary>
			/// CountryCode.
			/// </summary>
			public uint CountryCode;
			/// <summary>
			/// CodePage.
			/// </summary>
			public uint CodePage;

			#endregion
			
			#region IInfo3 Members

			/// <summary>
			/// UserID.
			/// </summary>
			public uint UserID;
			/// <summary>
			/// PrimaryGroupID.
			/// </summary>
			public uint PrimaryGroupID;
			/// <summary>
			/// Profile.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Profile;
			/// <summary>
			/// HomeDirectoryDrive.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string HomeDirectoryDrive;
			/// <summary>
			/// PasswordExpired.
			/// </summary>
			public uint PasswordExpired;

			#endregion
		}

		/// <summary>
		/// Structure for USER_INFO_4.
		/// </summary>
		[StructLayoutAttribute (LayoutKind.Sequential)]
		private struct Info4
		{
			#region IInfo0 Members

			/// <summary>
			/// Name.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Name;

			#endregion

			#region IInfo1 Members

			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Password;

			/// <summary>
			/// PasswordAge.
			/// </summary>
			public uint PasswordAge;

			/// <summary>
			/// Privilege.
			/// </summary>
			public uint Privilege;

			/// <summary>
			/// Directory.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Directory;

			/// <summary>
			/// Comment.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Comment;

			/// <summary>
			/// Flags.
			/// </summary>
			public uint Flags;

			/// <summary>
			/// ScriptPath.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ScriptPath;

			#endregion

			#region IInfo2 Members

			/// <summary>
			/// AuthorizationFlags.
			/// </summary>
			public uint AuthorizationFlags;
			/// <summary>
			/// FullName.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string FullName;
			/// <summary>
			/// UserComment.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string UserComment;
			/// <summary>
			/// Reserved.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Reserved;
			/// <summary>
			/// Workstations.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Workstations;
			/// <summary>
			/// LastLogon.
			/// </summary>
			public uint LastLogon;
			/// <summary>
			/// LastLogoff.
			/// </summary>
			public uint LastLogoff;
			/// <summary>
			/// AccountExpires.
			/// </summary>
			public uint AccountExpires;
			/// <summary>
			/// MaxStorage.
			/// </summary>
			public uint MaxStorage;
			/// <summary>
			/// UnitsPerWeek.
			/// </summary>
			public uint UnitsPerWeek;
			/// <summary>
			/// LogonHours.
			/// </summary>
			[MarshalAs(UnmanagedType.U1)] 
			public byte LogonHours;
			/// <summary>
			/// BadPasswordCount.
			/// </summary>
			public uint BadPasswordCount;
			/// <summary>
			/// NumberOfLogons.
			/// </summary>
			public uint NumberOfLogons;
			/// <summary>
			/// LogonServer.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string LogonServer;
			/// <summary>
			/// CountryCode.
			/// </summary>
			public uint CountryCode;
			/// <summary>
			/// CodePage.
			/// </summary>
			public uint CodePage;

			#endregion
			
			#region IInfo4 Members

			/// <summary>
			/// UserID.
			/// </summary>
			public IntPtr SID;
			/// <summary>
			/// PrimaryGroupID.
			/// </summary>
			public uint PrimaryGroupID;
			/// <summary>
			/// Profile.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string Profile;
			/// <summary>
			/// HomeDirectoryDrive.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] 
			public string HomeDirectoryDrive;
			/// <summary>
			/// PasswordExpired.
			/// </summary>
			public uint PasswordExpired;

			#endregion
		}
		
		/// <summary>
		/// Structure for USER_INFO_10.
		/// </summary>
			[StructLayoutAttribute (LayoutKind.Sequential)]
		private struct info10
		{
			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Name;
			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Comment;
			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string UserComment;
			/// <summary>
			/// Password.
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Fullname;

		}

		#endregion

		#region Constants

		/// <summary>
		/// Specifies the information level of the data.
		/// </summary>
		public enum InfoLevel : uint
		{
			/// <summary>
			/// Return the user account name.
			/// </summary>
			/// <remarks>
			/// The bufptr parameter points to a Info0 (USER_INFO_0) structure.
			/// </remarks>
			L0=0,

			/// <summary>
			/// Return detailed information about the user account.
			/// </summary>
			/// <remarks>
			/// The bufptr parameter points to a Info1 (USER_INFO_1) structure.
			/// This structure is not implemented here at this time.
			/// </remarks>
			L1=1,

			/// <summary>
			/// Return level one information and additional attributes about the user account.
			/// </summary>
			/// <remarks>
			/// The bufptr parameter points to a Info2 (USER_INFO_2) structure.
			/// This structure is not implemented here at this time.
			/// </remarks>
			L2=2,

			/// <summary>
			/// Return level two information and additional attributes about the user account.
			/// </summary>
			/// <remarks>
			/// This level is valid only on servers. 
			/// The bufptr parameter points to a Info3 (USER_INFO_3) structure.
			/// Note that it is recommended that you use USER_INFO_4 instead.
			/// </remarks>
			L3=3,

			/// <summary>
			/// Return level two information and additional attributes about the user account.
			/// </summary>
			/// <remarks>
			/// This level is valid only on servers. 
			/// The bufptr parameter points to a Info4 (USER_INFO_4) structure.
			/// Windows 2000/NT:  This level is not supported.
			/// </remarks>
			L4=4,

			/// <summary>
			/// Return user and account names and comments.
			/// </summary>
			/// <remarks>
			/// The bufptr parameter points to a Info10 (USER_INFO_10) structure.
			/// </remarks>
			L10=10,

			/*
			/// <summary>
			/// Return detailed information about the user account.
			/// </summary>
			/// <remarks>
			/// This level is valid only on servers. 
			/// The bufptr parameter points to a Info11 (USER_INFO_11) structure.
			/// This structure is not implemented here at this time.
			/// </remarks>
			L11=11,
			*/
			/*
			/// <summary>
			/// Return level two information and additional attributes about the user account.
			/// </summary>
			/// <remarks>
			/// The bufptr parameter points to a Info4 (USER_INFO_4) structure.
			/// This structure is not implemented here at this time.
			/// Note that on Windows XP and later, it is recommended that you use USER_INFO_23 instead.
			/// </remarks>
			L20=20,
			*/
			/*
			/// <summary>
			/// Return the user's name and identifier and various account attributes.
			/// </summary>
			/// <remarks>
			/// This level is valid only on servers. 
			/// The bufptr parameter points to a Info23 (USER_INFO_23) structure.
			/// This structure is not implemented here at this time.
			/// </remarks>
			L23=23,
			*/
		}
		
		#endregion

		#region Member Data

		private Info2 m_UserInfo;
		private Info0[] m_Groups;
		private Info0[] m_LocalGroups;
		private SecurityIdentifier m_SID;

		#endregion

        #region Accessors
 
		/// <summary>
		/// Full name of the user.
		/// </summary>
		public string FullName
		{
			get
			{
				//return m_UserInfo.FullName;
				return null;
			}
		}

		/// <summary>
		/// User name.
		/// </summary>
		public string Username
		{
			get
			{
				return m_UserInfo.Name;
			}
		}

		/// <summary>
		/// User name.
		/// </summary>
		public string DomainName
		{
			get
			{
				return m_SID.Domain;
			}
		}
/*
		/// <summary>
		/// SecurityIdentifier accessor.
		/// </summary>
		public SecurityIdentifier SecurityIdentifier
		{
			get
			{
				//return new SecurityIdentifier(m_UserInfo.usri4_user_sid);
				throw new System.NotImplementedException("BioRad.Win32.NetworkManagement.User.SecurityIdentifier (get)");
			}
		}
*/
		#endregion

        #region Constructors and Destructor

		/// <summary>
		/// Win32 Wrapper for NetLocalGroupEnum
		/// <param name="userName">Size of bufptr.</param>
		/// <param name="machineName">Size of bufptr.</param>
		/// </summary>
		public User(string userName, string machineName)
		{
			Update(machineName, userName);
		}

		/// <summary>
		/// Win32 Wrapper for NetLocalGroupEnum
		/// <param name="userName">Size of bufptr.</param>
		/// </summary>
		public User(string userName)
		{
			Update(null, userName);
		}

		/// <summary>
		/// Win32 Wrapper for NetLocalGroupEnum
		/// <param name="sID">security identifier</param>
		/// </summary>
		public User(SecurityIdentifier sID)
		{
			m_SID=sID;

			Update(sID.Domain, sID.Account);
		}


		private void Update(string machineName, string account)
		{
			IntPtr pBuf=IntPtr.Zero;
			string domainMachineNameStr=null;

			System.Text.StringBuilder domainMachineName=new System.Text.StringBuilder("\\\\");
			if(machineName!=null && machineName.Length>0)
			{
				domainMachineName.Append(machineName);
				domainMachineNameStr=domainMachineName.ToString();
			}

			APIBuffer.ThrowThisError((ErrorCodes)NetUserGetInfo(domainMachineNameStr, account, (uint)InfoLevel.L2, out pBuf), "BioRad.Win32.NetworkManagement.Update");
			
			m_UserInfo = (Info2)Marshal.PtrToStructure(pBuf, typeof(Info2));
			APIBuffer.Free(pBuf);
		}

        #endregion

        #region Methods

		/// <summary>
		/// IsMemberOfGroup
		/// </summary>
		/// <param name="groupname"></param>
		/// <returns></returns>
		public bool IsMemberOfGroup(string groupname)
		{
			LocalGroups lgs=new LocalGroups();
			LocalGroup lg=lgs[groupname];

			foreach(LocalGroup.MemberInfo member in lg.Members)
			{
				if(member.DomainAndName.ToLower()==(this.DomainName+"\\"+this.Username).ToLower())
					return true;
			}

			return false;
		}
 
		/// <summary>
		/// Method to update this collection with Local User Group information via Win32 API call.
		/// </summary>
		private void GetLocalGroups(string server)
		{
			int			size			=0x4000;			//Size of bufptr.
			IntPtr		bufptr			=new IntPtr(size);	//Buffer containing returned data from API call.
			const int	level			=1;					//Level of specific information to return. (See API call documentation.)
			const int	flags			=0;
			int			prefmaxlen		=size-1;			//Specifies the preferred maximum length of returned data, in bytes.
			int			entriesread		=0;					//Value set to number of groups read limited by bufptr size.
			int			totalentries	=0;					//Value set to number of groups.
			int			err				=0;					//Returned status value.

			//Try reading with the initial bufptr size...
			do
			{
				switch(err=NetUserGetLocalGroups(server, this.Username, flags, level, out bufptr, prefmaxlen, out entriesread, out totalentries))
				{
					case (int)Kernel.ErrorCodes.MORE_DATA	:
						//If more data is available, double bufptr size and start over...
						size*=2;
						bufptr=new IntPtr(size);
						prefmaxlen=size-1;

						break;
					default										:	APIBuffer.ThrowThisError((ErrorCodes)err, "BioRad.Win32.NetworkManagement.GetLocalGroups");	break;
				}
			}
			while(err==(int)Kernel.ErrorCodes.MORE_DATA);	//ERROR_MORE_DATA	

			Info0 group;	//Structure for returned data.

			IntPtr iter = bufptr;
			m_LocalGroups=new BioRad.Win32.NetworkManagement.User.Info0[totalentries];

			for(int i=0; i < totalentries; i++)
			{
				//Get group info structure and set pointer.
				group = (Info0)Marshal.PtrToStructure(iter, typeof(Info0));
				iter = (IntPtr)((int)iter + Marshal.SizeOf(typeof(BioRad.Win32.NetworkManagement.LocalGroup.GroupInfo)));

				//Add a new Local Group object based on the name to this collection.
				m_LocalGroups.SetValue(group, i);
			}

			APIBuffer.Free(bufptr);
		}
		
		/// <summary>
		/// Method to update this collection with Local User Group information via Win32 API call.
		/// </summary>
		private void GetGroups(string server)
		{
			int			size			=0x4000;			//Size of bufptr.
			IntPtr		bufptr			=new IntPtr(size);	//Buffer containing returned data from API call.
			const int	level			=0;					//Level of specific information to return. (See API call documentation.)
			int			prefmaxlen		=size-1;			//Specifies the preferred maximum length of returned data, in bytes.
			int			entriesread		=0;					//Value set to number of groups read limited by bufptr size.
			int			totalentries	=0;					//Value set to number of groups.
			int			err				=0;					//Returned status value.

			//Try reading with the initial bufptr size...
			do
			{
				switch(err=NetUserGetGroups(server, this.Username, level, out bufptr, prefmaxlen, out entriesread, out totalentries))
				{
					case (int)Kernel.ErrorCodes.MORE_DATA	:
						//If more data is available, double bufptr size and start over...
						size*=2;
						bufptr=new IntPtr(size);
						prefmaxlen=size-1;

						break;
					default										:	APIBuffer.ThrowThisError((ErrorCodes)err, "BioRad.Win32.NetworkManagement.GetGroups");	break;
				}
			}
			while(err==(int)Kernel.ErrorCodes.MORE_DATA);	//ERROR_MORE_DATA	

			Info0 group;	//Structure for returned data.

			IntPtr iter = bufptr;
			m_Groups=new BioRad.Win32.NetworkManagement.User.Info0[totalentries];

			for(int i=0; i < totalentries; i++)
			{
				//Get group info structure and set pointer.
				group = (Info0)Marshal.PtrToStructure(iter, typeof(Info0));
				iter = (IntPtr)((int)iter + Marshal.SizeOf(typeof(BioRad.Win32.NetworkManagement.LocalGroup.GroupInfo)));

				//Add a new Local Group object based on the name to this collection.
				m_Groups.SetValue(group, i);
			}

			APIBuffer.Free(bufptr);
		}
		
		#endregion

		#region Unmanaged API

		/// <summary>
		/// Win32 Wrapper for NetLocalGroupEnum
		/// <param name="uName">Size of bufptr.</param>
		/// <param name="mName">Buffer containing returned data from API call.</param>
		/// <param name="level">Level of specific information to return. (See API call documentation.).</param>
		/// <param name="bufptr">Specifies the preferred maximum length of returned data, in bytes.</param>
		/// </summary>
		[DllImport("netapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int NetUserGetInfo(String uName, String mName, uint level, out IntPtr bufptr);

		/// <summary>
		/// NetUserGetLocalGroups
		/// </summary>
		/// <param name="servername"></param>
		/// <param name="username"></param>
		/// <param name="level"></param>
		/// <param name="flags"></param>
		/// <param name="bufptr"></param>
		/// <param name="prefmaxlen"></param>
		/// <param name="entriesread"></param>
		/// <param name="totalentries"></param>
		/// <returns></returns>
		[DllImport("netapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int NetUserGetLocalGroups(
			[MarshalAs(UnmanagedType.LPWStr)] 
			string  servername,
			[MarshalAs(UnmanagedType.LPWStr)] 
			string  username,
			int level,
			int flags,
			out IntPtr bufptr,
			int prefmaxlen,
			out int entriesread,
			out int totalentries
			);

		/// <summary>
		/// NetUserGetGroups
		/// </summary>
		/// <param name="servername"></param>
		/// <param name="username"></param>
		/// <param name="level"></param>
		/// <param name="bufptr"></param>
		/// <param name="prefmaxlen"></param>
		/// <param name="entriesread"></param>
		/// <param name="totalentries"></param>
		/// <returns></returns>
		[DllImport("netapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int NetUserGetGroups(
			[MarshalAs(UnmanagedType.LPWStr)] 
			string  servername,
			[MarshalAs(UnmanagedType.LPWStr)] 
			string  username,
			int level,
			out IntPtr bufptr,
			int prefmaxlen,
			out int entriesread,
			out int totalentries
			);

		#endregion
	}
}
