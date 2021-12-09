using System;
using System.Runtime.InteropServices;
using System.Collections;

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
	///			<item name="vssfile">$Workfile: NetworkGroups.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/NetworkGroups.cs $</item>
	///			<item name="vssrevision">$Revision: 7 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class NetworkGroups
	{
		#region Structures

		/// <summary>
		/// Win32 Wrapper for LOCALGROUP_INFO_1.
		/// </summary>
		//TODO: Expand class to use GroupInfo3 when SID information becomes important.
		[StructLayout(LayoutKind.Sequential)]
			public struct NetDisplayMachine
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
			public int Flags;
			/// <summary>
			/// 
			/// </summary>
			public int ID;
			/// <summary>
			/// 
			/// </summary>
			public int NextIndex;
		}

		/// <summary>
		/// Win32 Wrapper for GROUP_INFO_2.
		/// </summary>
		public struct GroupInfo 
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
		}
					
		#endregion

		#region Constructors and Destructor
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public NetworkGroups()
		{
			Update(null);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="server">server name.</param>
		public NetworkGroups(string server)
		{
			m_Server=server;
			Update(m_Server);
		}

		#endregion

		#region Member Data

		private string		m_Server;
		private ArrayList	m_Groups=new ArrayList();

		#endregion

		#region Accessors
		/// <summary>
		/// string based indexer.
		/// </summary>
		public NetworkGroup this[string key]
		{
			get
			{
				foreach(NetworkGroups.GroupInfo group in m_Groups)
				{
					if(group.Name.ToLower()==key.ToLower())
						return new NetworkGroup(group);
				}
				
				throw new System.IndexOutOfRangeException(key+" is not a valid group name.");
			}
		}

		/// <summary>
		/// Collection Indexer based on collection index.
		/// <param name="index">Index of Local Group to reference.</param>
		/// </summary>
		public NetworkGroup this[int index]
		{
			get 
			{
				return new NetworkGroup(((NetworkGroups.GroupInfo)m_Groups[index]));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return m_Groups.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Server
		{
			get
			{
				return m_Server;
			}
			set
			{
				m_Server=value;
				Update(m_Server);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Method to update this collection with Local User Group information via Win32 API call.
		/// </summary>
		private void Update(string server)
		{
			int		size			=0x4000;			//Size of bufptr.
			IntPtr	bufptr			=new IntPtr(size);	//Buffer containing returned data from API call.
			int		level			=2;					//Level of specific information to return. (See API call documentation.)
			int		prefmaxlen		=size-1;			//Specifies the preferred maximum length of returned data, in bytes.
			int		entriesread		=0;					//Value set to number of groups read limited by bufptr size.
			int		totalentries	=0;					//Value set to number of groups.
			int		resume_handle	=0;					//Handle to reference if using sequential API calls to gather group data.
			int		err				=0;					//Returned status value.

			//Try reading with the initial bufptr size...
			do
			{
				switch(err=NetGroupEnum(server, level, out bufptr, prefmaxlen, out entriesread, out totalentries, ref resume_handle))
				{
					case (int)ErrorCodes.BufTooSmall 	:
					case (int)BioRad.Win32.Kernel.ErrorCodes.MORE_DATA :
						//If more data is available, double bufptr size and start over...
						size*=2;
						bufptr=new IntPtr(size);
						prefmaxlen=size-1;
						resume_handle=0;

						break;
					case (int)ErrorCodes.InvalidComputer	:
					case (int)BioRad.Win32.Kernel.ErrorCodes.SUCCESS	:
					default	:	break;
				}
			}
			while(err==(int)BioRad.Win32.Kernel.ErrorCodes.MORE_DATA);

			NetworkGroups.GroupInfo group;	//Structure for returned data.

			IntPtr iter = bufptr;

			for(int i=0; i < totalentries; i++)
			{
				//Get group info structure and set pointer.
				group = (NetworkGroups.GroupInfo)Marshal.PtrToStructure(iter, typeof(NetworkGroups.GroupInfo));
				iter = (IntPtr)((int)iter + Marshal.SizeOf(typeof(BioRad.Win32.NetworkManagement.NetworkGroups.GroupInfo)));

				//Add a new Local Group object based on the name to this collection.
				m_Groups.Add(group);
			}

			APIBuffer.Free(bufptr);
		}

		#endregion

		#region Unmanaged

		/// <summary>
		/// Win32 Wrapper for NetGroupEnum
		/// <param name="servername">Size of bufptr.</param>
		/// <param name="level">Buffer containing returned data from API call.</param>
		/// <param name="bufptr">Level of specific information to return. (See API call documentation.).</param>
		/// <param name="prefmaxlen">Specifies the preferred maximum length of returned data, in bytes.</param>
		/// <param name="entriesread">Value set to number of groups read limited by bufptr size.</param>
		/// <param name="totalentries">Value set to number of groups.</param>
		/// <param name="resume_handle">Handle to reference if using sequential API calls to gather group data.</param>
		/// </summary>
		[DllImport( "Netapi32.dll", EntryPoint="NetGroupEnum", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetGroupEnum(
			[MarshalAs(UnmanagedType.LPWStr)] 
			string servername, 
			int level, 
			out IntPtr bufptr, 
			int prefmaxlen, 
			out int entriesread, 
			out int totalentries, 
			ref int resume_handle
			);

		[DllImport( "Netapi32.dll", EntryPoint="NetQueryDisplayInformation", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetQueryDisplayInformation(
			[MarshalAs(UnmanagedType.LPWStr)] string servername, 
			int level, 
			int index, 
			int entriesreq, 
			int prefmaxlen, 
			out int entriesread, 
			out IntPtr bufptr
			);

		#endregion
	}
}
