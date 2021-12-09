using System;
using System.Runtime.InteropServices;

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
	///			<item name="vssfile">$Workfile: NetworkGroup.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/NetworkGroup.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class NetworkGroup
	{
		#region Structures

		/// <summary>
		/// Win32 Wrapper for NET_DISPLAY_GROUP.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct NetDisplayGroup
		{
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string Name;
			
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string Comment;
			
			/// <summary>
			/// 
			/// </summary>
			public int ID;
			
			/// <summary>
			/// 
			/// </summary>
			public int Attributes;
			
			/// <summary>
			/// 
			/// </summary>
			public int NextIndex;
		}

		/// <summary>
		/// Win32 Wrapper for GROUP_USERS_INFO_0.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct UserInfo
		{
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string Name;
		}

		#endregion

		#region Member Data

		private UserInfo[] m_UsersInfo;
		private NetworkGroups.GroupInfo m_Info;
		private string m_Name;
		private string m_Comment;
		
		#endregion

		#region Accessors

		/// <summary>
		/// Local User Group Name
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set {}
		}

		/// <summary>
		/// Members
		/// </summary>
		public UserInfo[] Members
		{
			get
			{
				int		size			=0x4000;			//Size of bufptr.
				IntPtr	bufptr			=new IntPtr(size);	//Buffer containing returned data from API call.
				int		level			=0;					//Level of specific information to return. (See API call documentation.)
				int		prefmaxlen		=size-1;			//Specifies the preferred maximum length of returned data, in bytes.
				int		entriesread		=0;					//Value set to number of groups read limited by bufptr size.
				int		totalentries	=0;					//Value set to number of groups.
				int		resume_handle	=0;					//Handle to reference if using sequential API calls to gather group data.
				int		err				=0;					//Returned status value.

				//Try reading with the initial bufptr size...
				do
				{
					switch(err=NetGroupGetUsers("\\\\LSG11NT", m_Name, level, out bufptr, prefmaxlen, out entriesread, out totalentries, ref resume_handle))
					{
						case 2123	:	//NERR_BufTooSmall	
						case 234	:	//ERROR_MORE_DATA	
							//If more data is available, double bufptr size and start over...
							size*=2;
							bufptr=new IntPtr(size);
							prefmaxlen=size-1;
							resume_handle=0;

							break;
						case 2351	:	//NERR_InvalidComputer	
							//case 2123	:	//ERROR_ACCESS_DENIED	
						case 0	:		//NERR_Success	
						default	:	break;
					}
				}
				while(err==234);	//ERROR_MORE_DATA	

				UserInfo member;	//Structure for returned data.
				m_UsersInfo=new UserInfo[totalentries];

				IntPtr iter = bufptr;

				for(int i=0; i < totalentries; i++)
				{
					//Get group info structure and set pointer.
					member = (UserInfo)Marshal.PtrToStructure(iter, typeof(UserInfo));
					iter = (IntPtr)((int)iter + Marshal.SizeOf(typeof(UserInfo)));

					//Add a new Local Group object based on the name to this collection.
					m_UsersInfo[i]=member;
				}

				APIBuffer.Free(bufptr);

				return m_UsersInfo;
			}
		}

		#endregion

        #region Constructors and Destructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public NetworkGroup()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		internal NetworkGroup(NetworkGroup.NetDisplayGroup group)
		{
			m_Name=group.Name;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		internal NetworkGroup(NetworkGroups.GroupInfo group)
		{
			m_Name=group.Name;
		}
	
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">group name.</param>
		public NetworkGroup(string name)
		{
			m_Name=name;

			IntPtr bufptr=IntPtr.Zero;
			int err;
			
			switch(err=NetGroupGetInfo("LSG11NT", m_Name, 2, out bufptr))
			{
				case 0:
					m_Info = (NetworkGroups.GroupInfo)Marshal.PtrToStructure(bufptr, typeof(NetworkGroups.GroupInfo));
					//m_SID=new BioRad.Win32.AdvancedWindowsBase.SecurityIdentifier(m_Info.Name, "");
					return;
				case (int)BioRad.Win32.Kernel.ErrorCodes.ACCESS_DENIED:	
					break;
				case (int)ErrorCodes.InvalidComputer:	
					break;
				case (int)ErrorCodes.GroupNotFound:		
					break;
				default:
					return;
			}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">group name.</param>
		/// <param name="comment">group comment.</param>
		public NetworkGroup(string name, string comment)
		{
			m_Name=name;
			m_Comment=comment;
		}

		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">group name.</param>
		/// <param name="comment">group comment.</param>
		/// <param name="SID">group SID.</param>
		/// <param name="attributes">group attributes.</param>
		public NetworkGroup(string name, string comment, string SID, string attributes)
		{
			m_Name=name;
			m_Comment=comment;
			//m_Attributes=attributes;
		}

		#endregion

		#region Unmanaged API

		/// <summary>
		/// Win32 Wrapper for NetLocalGroupEnum
		/// <param name="servername">Size of bufptr.</param>
		/// <param name="groupname">Buffer containing returned data from API call.</param>
		/// <param name="level">Level of specific information to return. (See API call documentation.).</param>
		/// <param name="bufptr">Specifies the preferred maximum length of returned data, in bytes.</param>
		/// </summary>
		[DllImport( "Netapi32.dll", EntryPoint="NetGroupGetInfo", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetGroupGetInfo(
			[MarshalAs(UnmanagedType.LPWStr)] string servername, 
			[MarshalAs(UnmanagedType.LPWStr)] string groupname, 
			int level, 
			out IntPtr bufptr
			);

		/// <summary>
		/// NetLocalGroupGetMembers
		/// </summary>
		/// <param name="servername"></param>
		/// <param name="groupname"></param>
		/// <param name="level"></param>
		/// <param name="bufptr"></param>
		/// <param name="prefmaxlen"></param>
		/// <param name="entriesread"></param>
		/// <param name="totalentries"></param>
		/// <param name="resume_handle"></param>
		/// <returns></returns>
		[DllImport( "Netapi32.dll", EntryPoint="NetGroupGetUsers", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetGroupGetUsers(
			[MarshalAs(UnmanagedType.LPWStr)] string servername, 
			[MarshalAs(UnmanagedType.LPWStr)] string groupname, 
			int level, 
			out IntPtr bufptr,
			int prefmaxlen, 
			out int entriesread, 
			out int totalentries, 
			ref int resume_handle
			);

		#endregion
	}
}
