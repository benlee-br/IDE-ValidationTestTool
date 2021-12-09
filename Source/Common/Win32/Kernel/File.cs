using System;
using System.Runtime.InteropServices;

namespace BioRad.Win32.Kernel
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
	///			<item name="vssfile">$Workfile: File.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/Kernel/File.cs $</item>
	///			<item name="vssrevision">$Revision: 5 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class File
	{
		#region Structures
		
		/// <summary>
		/// OFSTRUCT
		/// </summary>
		[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
			public struct FileInfo
		{
			/// <summary>
			/// Length of the structure, in bytes. 
			/// </summary>
			public byte cBytes;
			/// <summary>
			/// If this member is nonzero, the file is on a hard (fixed) disk. Otherwise, it is not. 
			/// </summary>
			public byte fFixedDisc;
			/// <summary>
			/// MS-DOS error code if the OpenFile function failed. 
			/// </summary>
			public UInt16 nErrCode;
			/// <summary>
			/// Reserved; do not use. 
			/// </summary>
			public UInt16 Reserved1;
			/// <summary>
			/// Reserved; do not use. 
			/// </summary>
			public UInt16 Reserved2;
			/// <summary>
			/// Path and file name of the file. 
			/// </summary>
			[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)] 
			public string szPathName;
		}
		
		#endregion

		#region Constants

		/// <summary>
		/// 
		/// </summary>
		[Flags]
			public enum EFileAccess : uint
		{
			/// <summary>
			/// 
			/// </summary>
			GenericRead = 0x80000000,
			/// <summary>
			/// 
			/// </summary>
			GenericWrite = 0x40000000,
			/// <summary>
			/// 
			/// </summary>
			GenericExecute = 0x20000000,
			/// <summary>
			/// 
			/// </summary>
			GenericAll = 0x10000000,
		}

		/// <summary>
		/// 
		/// </summary>
		[Flags]
			public enum EFileShare : uint
		{
			/// <summary>
			/// 
			/// </summary>
			None = 0x00000000,
			/// <summary>
			/// 
			/// </summary>
			Read = 0x00000001,
			/// <summary>
			/// 
			/// </summary>
			Write = 0x00000002,
			/// <summary>
			/// 
			/// </summary>
			Delete = 0x00000004,
		}

		/// <summary>
		/// 
		/// </summary>
		public enum ECreationDisposition : uint
		{
			/// <summary>
			/// 
			/// </summary>
			New = 1,
			/// <summary>
			/// 
			/// </summary>
			CreateAlways = 2,
			/// <summary>
			/// 
			/// </summary>
			OpenExisting = 3,
			/// <summary>
			/// 
			/// </summary>
			OpenAlways = 4,
			/// <summary>
			/// 
			/// </summary>
			TruncateExisting = 5,
		}

		/// <summary>
		/// 
		/// </summary>
		[Flags]
			public enum EFileAttributes : uint
		{
			/// <summary>
			/// 
			/// </summary>
			Readonly = 0x00000001,
			/// <summary>
			/// 
			/// </summary>
			Hidden = 0x00000002,
			/// <summary>
			/// 
			/// </summary>
			System = 0x00000004,
			/// <summary>
			/// 
			/// </summary>
			Directory = 0x00000010,
			/// <summary>
			/// 
			/// </summary>
			Archive = 0x00000020,
			/// <summary>
			/// 
			/// </summary>
			Device = 0x00000040,
			/// <summary>
			/// 
			/// </summary>
			Normal = 0x00000080,
			/// <summary>
			/// 
			/// </summary>
			Temporary = 0x00000100,
			/// <summary>
			/// 
			/// </summary>
			SparseFile = 0x00000200,
			/// <summary>
			/// 
			/// </summary>
			ReparsePoint = 0x00000400,
			/// <summary>
			/// 
			/// </summary>
			Compressed = 0x00000800,
			/// <summary>
			/// 
			/// </summary>
			Offline= 0x00001000,
			/// <summary>
			/// 
			/// </summary>
			NotContentIndexed = 0x00002000,
			/// <summary>
			/// 
			/// </summary>
			Encrypted = 0x00004000,
			/// <summary>
			/// 
			/// </summary>
			Write_Through = 0x80000000,
			/// <summary>
			/// 
			/// </summary>
			Overlapped = 0x40000000,
			/// <summary>
			/// 
			/// </summary>
			NoBuffering = 0x20000000,
			/// <summary>
			/// 
			/// </summary>
			RandomAccess = 0x10000000,
			/// <summary>
			/// 
			/// </summary>
			SequentialScan = 0x08000000,
			/// <summary>
			/// 
			/// </summary>
			DeleteOnClose = 0x04000000,
			/// <summary>
			/// 
			/// </summary>
			BackupSemantics = 0x02000000,
			/// <summary>
			/// 
			/// </summary>
			PosixSemantics = 0x01000000,
			/// <summary>
			/// 
			/// </summary>
			OpenReparsePoint = 0x00200000,
			/// <summary>
			/// 
			/// </summary>
			OpenNoRecall = 0x00100000,
			/// <summary>
			/// 
			/// </summary>
			FirstPipeInstance = 0x00080000
		}
		
		#endregion

        #region Member Data

		//private FileInfo m_Info;
		private IntPtr m_Handle=IntPtr.Zero;

        #endregion

        #region Accessors
        #endregion

        #region Constructors and Destructor

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public File(){}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="creationdisposition"></param>
		public File(string name, ECreationDisposition creationdisposition)
		{
			IntPtr info=IntPtr.Zero;
			int err=0;
			
			m_Handle=CreateFile(name, EFileAccess.GenericRead, EFileShare.Read, IntPtr.Zero, creationdisposition, EFileAttributes.Normal, IntPtr.Zero);
			
			if(-1==(int)(m_Handle))
				err=Marshal.GetLastWin32Error();


		}

        #endregion

        #region Methods

		internal IntPtr GetHandle()
		{
			return m_Handle;
		}

		private void Update()
		{
		}

        #endregion

		#region Unmanaged

		[DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
		private static extern IntPtr CreateFile(
			string lpFileName,
			EFileAccess dwDesiredAccess,
			EFileShare dwShareMode,
			IntPtr lpSecurityAttributes,
			ECreationDisposition dwCreationDisposition,
			EFileAttributes dwFlagsAndAttributes,
			IntPtr hTemplateFile
			);

		#endregion
	}
}
