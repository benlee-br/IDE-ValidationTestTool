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
	///			<item name="vssfile">$Workfile: LocalGroups.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/LocalGroups.cs $</item>
	///			<item name="vssrevision">$Revision: 10 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class LocalGroups
	{
		#region Constructors and Destructor

		/// <summary>
		/// LocalGroups
		/// </summary>
		public LocalGroups()
		{
			Update(null);
		}

		/// <summary>
		/// LocalGroups
		/// </summary>
		public LocalGroups(string server)
		{
			m_Server=server;
			Update(server);
		}

		#endregion

		#region Member Data

		private ArrayList m_Groups=new ArrayList();
		private string m_Server;

		#endregion

		#region Accessors

		/// <summary>
		/// string based indexer.
		/// </summary>
		public LocalGroup this[string key]
		{
			get
			{
				foreach(LocalGroup.GroupInfo group in m_Groups)
				{
					if(group.Name.ToLower()==key.ToLower())
						return new LocalGroup(group);
				}
				
				throw new System.IndexOutOfRangeException(key+" is not a valid group name.");
			}
		}

		/// <summary>
		/// Collection Indexer based on collection index.
		/// <param name="index">Index of Local Group to reference.</param>
		/// </summary>
		public LocalGroup this[int index]
		{
			get 
			{
				return new LocalGroup(((LocalGroup.GroupInfo)m_Groups[index]));
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
			int		level			=1;					//Level of specific information to return. (See API call documentation.)
			int		prefmaxlen		=size-1;			//Specifies the preferred maximum length of returned data, in bytes.
			int		entriesread		=0;					//Value set to number of groups read limited by bufptr size.
			int		totalentries	=0;					//Value set to number of groups.
			int		resume_handle	=0;					//Handle to reference if using sequential API calls to gather group data.
			int		err				=0;					//Returned status value.

			//Try reading with the initial bufptr size...
			do
			{
				switch(err=NetLocalGroupEnum(server, level, out bufptr, prefmaxlen, out entriesread, out totalentries, ref resume_handle))
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

			LocalGroup.GroupInfo group;	//Structure for returned data.

			IntPtr iter = bufptr;

			for(int i=0; i < totalentries; i++)
			{
				//Get group info structure and set pointer.
				group = (LocalGroup.GroupInfo)Marshal.PtrToStructure(iter, typeof(LocalGroup.GroupInfo));
				iter = (IntPtr)((int)iter + Marshal.SizeOf(typeof(BioRad.Win32.NetworkManagement.LocalGroup.GroupInfo)));

				//Add a new Local Group object based on the name to this collection.
				m_Groups.Add(group);
			}

			APIBuffer.Free(bufptr);
		}

		#endregion

		#region Unmanaged API

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
		[DllImport( "Netapi32.dll", EntryPoint="NetLocalGroupEnum", CharSet=CharSet.Auto, SetLastError=true)]
		private extern static int NetLocalGroupEnum(
			[MarshalAs(UnmanagedType.LPWStr)] 
			string servername, 
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
