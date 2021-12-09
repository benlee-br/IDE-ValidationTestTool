using System;
using System.Runtime.InteropServices;

using BioRad.Win32.Kernel;

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
	///			<item name="vssfile">$Workfile: SecurityDescriptor.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/AdvancedWindowsBase/SecurityDescriptor.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class SecurityDescriptor : IDisposable
	{
		#region Constants

		/// <summary>
		/// SECURITY_INFORMATION
		/// </summary>
		public class SecurityInfo
		{
			/// <summary>
			/// Indicates the owner identifier of the object is being referenced. 
			/// </summary>
			public static readonly  uint OWNER=0x00000001 ; 

			/// <summary>
			/// Indicates the primary group identifier of the object is being referenced. 
			/// </summary>
			public static readonly  uint GROUP=0x00000002 ; 

			/// <summary>
			/// Indicates the DACL of the object is being referenced. 
			/// </summary>
			public static readonly uint DACL=0x00000004 ;
																			
			/// <summary>
			/// Indicates the SACL of the object is being referenced. 
			/// </summary>
			/// <remarks>
			/// Windows NT:  This value is not supported.
			/// </remarks>
			public static readonly  uint SACL=0x00000008 ;

			/// <summary>
			/// Indicates the SACL inherits ACEs from the parent object.
			/// </summary>
			/// <remarks>
			/// Windows NT:  This value is not supported.
			/// </remarks>
			public static readonly  uint UNPROTECTED_SACL=0x10000000; 

			/// <summary>
			/// Indicates the DACL inherits ACEs from the parent object.
			/// </summary>
			/// <remarks>
			/// Windows NT:  This value is not supported.
			/// </remarks>
			public static readonly  uint UNPROTECTED_DACL=0x20000000; 

			/// <summary>
			/// Indicates the SACL cannot inherit ACEs.
			/// </summary>
			/// <remarks>
			/// Windows NT:  This value is not supported.
			/// </remarks>
			public static readonly uint PROTECTED_SACL=0x40000000; 

			/// <summary>
			/// Indicates the DACL cannot inherit access control entries (ACEs).
			/// </summary>
			/// <remarks>
			/// Windows NT:  This value is not supported.
			/// </remarks>
			public static readonly  uint PROTECTED_DACL=0x80000000; 
}

		/// <summary>
		/// SE_OBJECT_TYPE
		/// </summary>
		public enum ObjectType : int
		{
			/// <summary>
			/// Unknown object type.
			/// </summary>
			Unknown= 0,

			/// <summary>
			/// Indicates a file or directory. 
			/// </summary>
			/// <remarks>
			/// The name string that identifies a file or directory object can be in one of the following formats:
			/// A relative path, such as FileName.dat or ..\FileName
			/// An absolute path, such as FileName.dat, C:\DirectoryName\FileName.dat, or G:\RemoteDirectoryName\FileName.dat.
			/// A UNC name, such as \\ComputerName\ShareName\FileName.dat.
			/// A local file system root, such as \\\\.\\C:. Security set on a file system root does not persist when the system is restarted.
			/// </remarks>
			File,

			/// <summary>
			/// Indicates a Windows service. 
			/// </summary>
			/// <remarks>
			/// A service object can be a local service, such as ServiceName, or a remote service, such as \\ComputerName\ServiceName.
			/// </remarks>
			Service,

			/// <summary>
			/// Indicates a printer. 
			/// </summary>
			/// <remarks>
			/// A printer object can be a local printer, such as PrinterName, or a remote printer, such as \\ComputerName\PrinterName.
			/// </remarks>
			Printer,

			/// <summary>
			/// Indicates a registry key. 
			/// </summary>
			/// <remarks>
			/// A registry key object can be in the local registry, such as CLASSES_ROOT\SomePath or in a remote registry, such as \\ComputerName\CLASSES_ROOT\SomePath. 
			/// The names of registry keys must use the following literal strings to identify the predefined registry keys: "CLASSES_ROOT", "CURRENT_USER", "MACHINE", and "USERS".
			/// </remarks>
			RegistryKey,

			/// <summary>
			/// Indicates a network share. 
			/// </summary>
			/// <remarks>
			/// A share object can be local, such as ShareName, or remote, such as \\ComputerName\ShareName.
			/// </remarks>
			LMShare,

			/// <summary>
			/// Indicates a local kernel object. 
			/// </summary>
			/// <remarks>
			///	The GetSecurityInfo and SetSecurityInfo functions support all types of kernel objects. 
			///	The GetNamedSecurityInfo and SetNamedSecurityInfo functions work only with the following kernel objects: 
			///	semaphore, event, mutex, waitable timer, and file mapping.
			/// </remarks>
			Kernel,

			/// <summary>
			/// Indicates a window station or desktop object on the local computer.
			/// </summary>
			/// <remarks>
			///	You cannot use GetNamedSecurityInfo and SetNamedSecurityInfo with these objects 
			///	because the names of window stations or desktops are not unique.
			/// </remarks>
			WindowObject,

			/// <summary>
			/// Indicates a directory service object or a property set or property of a directory service object. 
			/// </summary>
			/// <remarks>
			/// The name string for a directory service object must be in X.500 form, for example:
			/// CN=SomeObject,OU=ou2,OU=ou1,DC=DomainName,DC=CompanyName,DC=com,O=internet
			///	Windows NT and Windows Me/98/95:  This enumeration value is not supported.
			/// </remarks>
			DirectoryService,

			/// <summary>
			/// Indicates a directory service object and all of its property sets and properties.
			/// </summary>
			/// <remarks>
			///	Windows NT and Windows Me/98/95:  This enumeration value is not supported.
			/// </remarks>
			DirectoryServiceAll,

			/// <summary>
			/// Indicates a provider-defined object.
			/// </summary>
			/// <remarks>
			///	Windows NT and Windows Me/98/95:  This enumeration value is not supported.
			/// </remarks>
			ProviderDefined,
			
			/// <summary>
			/// Indicates a WMI object.
			/// </summary>
			/// <remarks>
			///	Windows NT and Windows Me/98/95:  This enumeration value is not supported.
			/// </remarks>
			WMIGUID,
			
			/// <summary>
			/// Indicates an object for a registry entry under WOW64.
			/// </summary>
			/// <remarks>
			///	Windows NT and Windows Me/98/95:  This enumeration value is not supported.
			/// </remarks>
			WOW64_32KEY
		}


		#endregion

        #region Member Data

		private IntPtr m_SecurityDescriptor=IntPtr.Zero;
		private IntPtr m_Handle=IntPtr.Zero;
		private IntPtr m_SidOwner=IntPtr.Zero;
		private IntPtr m_DACL=IntPtr.Zero;
		private ObjectType m_Type=ObjectType.Unknown;

        #endregion

        #region Accessors
 
		/// <summary>
		/// Owner's Security Identifier.
		/// </summary>
		public SecurityIdentifier OwnerSID
		{
			get
			{
				return new SecurityIdentifier(m_SidOwner);
			}
		}

		/// <summary>
		/// Security Identifier's Discretionary ACL.
		/// </summary>
		public ACL DiscretionaryACL
		{
			get
			{
				return new ACL(m_DACL);
			}
		}
		
		#endregion

        #region Constructors and Destructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SecurityDescriptor(){}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="handle">
		/// Handle to the object from which to retrieve security information. 
		/// </param>
		public SecurityDescriptor(IntPtr handle)
		{
			m_Handle=handle;

			Update();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="file">
		/// Handle to the object from which to retrieve security information. 
		/// </param>
		public SecurityDescriptor(BioRad.Win32.Kernel.File file)
		{
			m_Handle=file.GetHandle();
			m_Type=ObjectType.File;

			Update();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="type"></param>
		public SecurityDescriptor(IntPtr handle, ObjectType type)
		{
			m_Handle=handle;
			m_Type=type;

			Update();
		}

        #endregion

        #region Methods

		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			APIBuffer.Dispose(m_SecurityDescriptor);
		}

		private void Update()
		{
			IntPtr pSidGroup= System.IntPtr.Zero;
			IntPtr pSacl= System.IntPtr.Zero;
			IntPtr pSecDes= System.IntPtr.Zero;
			//byte [] sd = new byte[100];
			
			switch(
				GetSecurityInfo(
					m_Handle, 
					(int)m_Type, 
					SecurityInfo.OWNER | SecurityInfo.DACL,
					//SecurityInfo.DACL | SecurityInfo.GROUP | SecurityInfo.OWNER | SecurityInfo.SACL,
					out m_SidOwner, 
					out pSidGroup, 
					out m_DACL, 
					out pSacl, 
					out m_SecurityDescriptor
					)
				)
			{
				case 0:
					bool crap=IsValidSecurityDescriptor(m_SecurityDescriptor);
					
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
		}

        #endregion

		#region Unmanaged

		/// <summary>
		/// GetSecurityInfo
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="ObjectType"></param>
		/// <param name="SecurityInfo"></param>
		/// <param name="pSidOwner"></param>
		/// <param name="pSidGroup"></param>
		/// <param name="pDacl"></param>
		/// <param name="pSacl"></param>
		/// <param name="pSecurityDescriptor"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		static extern uint GetSecurityInfo(
			IntPtr handle,
			int ObjectType,
			uint SecurityInfo,
			out IntPtr pSidOwner,
			out IntPtr pSidGroup,
			out IntPtr pDacl,
			out IntPtr pSacl,
			out IntPtr pSecurityDescriptor
			//[MarshalAs(UnmanagedType.LPArray)] byte[] pSecurityDescriptor
				);

		/// <summary>
		/// IsValidSecurityDescriptor
		/// </summary>
		/// <param name="pSecurityDescriptor"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		static extern bool IsValidSecurityDescriptor(IntPtr pSecurityDescriptor);

		#endregion
	}
}
