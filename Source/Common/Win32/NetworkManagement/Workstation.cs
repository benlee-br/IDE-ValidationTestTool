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
	///			<item name="vssfile">$Workfile: Workstation.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/Workstation.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public partial class Workstation
	{
		#region Structures

		/// <summary>
		/// Win32 Wrapper for WKSTA_INFO_102 .
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct WInfo
		{
			public int PlatformID;
			[MarshalAs(UnmanagedType.LPWStr)] public string Name;
			[MarshalAs(UnmanagedType.LPWStr)] public string Group;
			public int VersionMajor;
			public int VersionMinor;
			[MarshalAs(UnmanagedType.LPWStr)] public string LanRoot;
			public int LoggedOnUsersCount;
		}

		#endregion
		
		#region Member Data

		private WInfo m_Info;

		#endregion

		#region Accessors

		/// <summary>
		/// PlatformID
		/// </summary>
		public int PlatformID
		{
			get
			{
				return m_Info.PlatformID;
			}
		}
   
		/// <summary>
		/// Full name of the workstation.
		/// </summary>
		public string Name
		{
			get
			{
				return m_Info.Name;
			}
		}
  
		/// <summary>
		/// Group name of the workstation.
		/// </summary>
		public string Group
		{
			get
			{
				return m_Info.Group;
			}
		}

		/// <summary>
		/// PlatformID
		/// </summary>
		public int VersionMajor
		{
			get
			{
				return m_Info.VersionMajor;
			}
		}

		/// <summary>
		/// PlatformID
		/// </summary>
		public int VersionMinor
		{
			get
			{
				return m_Info.VersionMinor;
			}
		}
		
		/// <summary>
		/// Group name of the workstation.
		/// </summary>
		public string LanRoot
		{
			get
			{
				return m_Info.LanRoot;
			}
		}

		/// <summary>
		/// PlatformID
		/// </summary>
		public int LoggedOnUsersCount
		{
			get
			{
				return m_Info.LoggedOnUsersCount;
			}
		}

		#endregion

		#region Constructors and Destructor

		/// <summary>
		/// WorkStation
		/// </summary>
		public Workstation()
		{
			//APIBuffer buffer=new APIBuffer();
			IntPtr bufptr= System.IntPtr.Zero;
			m_Info=new WInfo();
			
			switch(NetWkstaGetInfo(null, 102, out bufptr))
			{
				case 0:
					m_Info = (WInfo)Marshal.PtrToStructure(bufptr, typeof(WInfo));
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.ACCESS_DENIED:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.INVALID_LEVEL:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.INVALID_PARAMETER:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.NOT_ENOUGH_MEMORY:	
					break;
			}

			APIBuffer.Free(bufptr);
		}

		#endregion

		#region Unmanaged API

		/// <summary>
		/// Win32 Wrapper for NetWkstaGetInfo
		/// <param name="servername">Size of bufptr.</param>
		/// <param name="level">Buffer containing returned data from API call.</param>
		/// <param name="bufptr">Level of specific information to return. (See API call documentation.).</param>
		/// </summary>
		[DllImport( "Netapi32.dll", EntryPoint="NetWkstaGetInfo", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetWkstaGetInfo(
			string servername, 
			int level, 
			out IntPtr bufptr
			);

		#endregion
	}
}
