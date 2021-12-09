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
	///			<item name="vssfile">$Workfile: LocalGroup.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/LocalGroup.cs $</item>
	///			<item name="vssrevision">$Revision: 11 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class LocalGroup
	{
		#region Structures

		/// <summary>
		/// Win32 Wrapper for LOCALGROUP_INFO_1.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal struct GroupInfo
		{
			[MarshalAs(UnmanagedType.LPWStr)] public string Name;
			[MarshalAs(UnmanagedType.LPWStr)] public string Comment;
		}

		/// <summary>
		/// typedef struct _LOCALGROUP_MEMBERS_INFO_3 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct MemberInfo
		{
			/// <summary>
			/// DomainAndName
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string DomainAndName;
		}
		
		#endregion

		#region Member Data

		/// <summary>
		/// Member containing group info.
		/// </summary>
		private GroupInfo m_Info;
		private MemberInfo[] m_MembersInfo;

		#endregion

		#region Constructors and Destructor

		/// <summary>
		/// Constructor.
		/// </summary>
		internal LocalGroup(GroupInfo group)
		{
			m_Info=group;
		}
		
		/// <summary>
		/// Constructor.
		/// <param name="groupName">The name of this local user group.</param>
		/// <param name="domainMachineName">The name description of this local user group.</param>
		/// </summary>
		public LocalGroup(string groupName, string domainMachineName)
		{
			IntPtr bufptr=IntPtr.Zero;
			
			switch(NetLocalGroupGetInfo(domainMachineName, groupName, 1, out bufptr))
			{
				case 0:
					m_Info = (GroupInfo)Marshal.PtrToStructure(bufptr, typeof(GroupInfo));
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

		#endregion

		#region Accessors

		/// <summary>
		/// Local User Group Name
		/// </summary>
		public string Name
		{
			get {return m_Info.Name;}
			set {}
		}

		/// <summary>
		/// Local User Group Description
		/// </summary>
		public string Comment
		{
			get {return m_Info.Comment;}
			set {}
		}

		/// <summary>
		/// Members
		/// </summary>
		public MemberInfo[] Members
		{
			get
			{
				int		size			=0x4000;			//Size of bufptr.
				IntPtr	bufptr			=new IntPtr(size);	//Buffer containing returned data from API call.
				int		level			=3;					//Level of specific information to return. (See API call documentation.)
				int		prefmaxlen		=size-1;			//Specifies the preferred maximum length of returned data, in bytes.
				int		entriesread		=0;					//Value set to number of groups read limited by bufptr size.
				int		totalentries	=0;					//Value set to number of groups.
				int		resume_handle	=0;					//Handle to reference if using sequential API calls to gather group data.
				int		err				=0;					//Returned status value.

				//Try reading with the initial bufptr size...
				do
				{
					switch(err=NetLocalGroupGetMembers(null, m_Info.Name, level, out bufptr, prefmaxlen, out entriesread, out totalentries, ref resume_handle))
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

				MemberInfo member;	//Structure for returned data.
				m_MembersInfo=new MemberInfo[totalentries];

				IntPtr iter = bufptr;

				for(int i=0; i < totalentries; i++)
				{
					//Get group info structure and set pointer.
					member = (MemberInfo)Marshal.PtrToStructure(iter, typeof(MemberInfo));
					iter = (IntPtr)((int)iter + Marshal.SizeOf(typeof(MemberInfo)));

					//Add a new Local Group object based on the name to this collection.
					m_MembersInfo[i]=member;
				}

				APIBuffer.Free(bufptr);

				return m_MembersInfo;
			}
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
		[DllImport( "Netapi32.dll", EntryPoint="NetLocalGroupGetInfo", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetLocalGroupGetInfo(
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
		[DllImport( "Netapi32.dll", EntryPoint="NetLocalGroupGetMembers", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetLocalGroupGetMembers(
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
