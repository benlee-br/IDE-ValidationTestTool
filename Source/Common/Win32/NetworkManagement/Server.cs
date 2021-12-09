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
	///			<item name="vssfile">$Workfile: Server.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/Server.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class Server
	{
		#region Structures

		/// <summary>
		/// Win32 Wrapper for SERVER_INFO_102.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct Info
		{
			public uint PlatformID;
			[MarshalAs(UnmanagedType.LPWStr)] public string Name;
			public uint VersionMajor;
			public uint VersionMinor;
			public uint Type;
			[MarshalAs(UnmanagedType.LPWStr)] public string Comment;
			public uint Users;
			public long Disc;
			public bool Hidden;
			public uint Announce;
			public uint AnnDelta;
			public uint Licenses;
			[MarshalAs(UnmanagedType.LPWStr)] public string Userpath;
		}

		#endregion
		
		#region Member Data

		private Info m_Info;

        #endregion

        #region Accessors
   
		/// <summary>
		/// Full name of the user.
		/// </summary>
		public string Name
		{
			get
			{
				return m_Info.Name;
			}
		}
		
		#endregion

        #region Constructors and Destructor

		/// <summary>
		/// Server
		/// </summary>
		public Server()
		{
			//APIBuffer buffer=new APIBuffer();
			IntPtr bufptr;
			
			switch(NetServerGetInfo(null, 102, out bufptr))
			{
				case 0:
					m_Info = (Info)Marshal.PtrToStructure(bufptr, typeof(Info));
					APIBuffer.Free(bufptr);
					return;
				case (int)BioRad.Win32.Kernel.ErrorCodes.ACCESS_DENIED:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.INVALID_LEVEL:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.INVALID_PARAMETER:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.NOT_ENOUGH_MEMORY:	
					break;
				default:
					APIBuffer.Free(bufptr);
					return;
			}

		}

		/// <summary>
		/// Server
		/// </summary>
		/// <param name="serverName"></param>
		public Server(string serverName)
		{
			//APIBuffer buffer=new APIBuffer();
			IntPtr bufptr;
			
			switch(NetServerGetInfo("\\\\"+serverName, 102, out bufptr))
			{
				case 0:
					m_Info = (Info)Marshal.PtrToStructure(bufptr, typeof(Info));
					APIBuffer.Free(bufptr);
					return;
				case (int)BioRad.Win32.Kernel.ErrorCodes.ACCESS_DENIED:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.INVALID_LEVEL:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.INVALID_PARAMETER:	
					break;
				case (int)BioRad.Win32.Kernel.ErrorCodes.NOT_ENOUGH_MEMORY:	
					break;
				default:
					APIBuffer.Free(bufptr);
					return;
			}

		}

		#endregion

		#region Unmanaged API

		/// <summary>
		/// NetServerGetInfo
		/// </summary>
		/// <param name="servername"></param>
		/// <param name="level"></param>
		/// <param name="bufptr"></param>
		/// <returns></returns>
		[DllImport( "Netapi32.dll", EntryPoint="NetServerGetInfo", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetServerGetInfo(
			string servername, 
			int level, 
			out IntPtr bufptr
			);

		#endregion
	}
}
