using System;
using System.Runtime.InteropServices;

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
	///			<item name="vssfile">$Workfile: ACL.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Win32/AdvancedWindowsBase/ACL.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 6/08/06 3:18p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	public class ACL
	{
		#region Structures

		/// <summary>
		/// Win32 Wrapper for ACL_SIZE_INFORMATION.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct SizeInformation
		{
			/// <summary>
			/// Count.
			/// </summary>
			public uint Count;
			
			/// <summary>
			/// BytesInUse.
			/// </summary>
			public uint BytesInUse;

			/// <summary>
			/// BytesFree.
			/// </summary>
			public uint BytesFree;
		}
		
		/// <summary>
		/// Win32 Wrapper for ACL_REVISION_INFORMATION.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct RevisionInformation
		{
			/// <summary>
			/// 
			/// </summary>
			public int Revision;
		}
		
		/// <summary>
		/// Win32 Wrapper for ACL_REVISION_INFORMATION.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct acl
		{
			/// <summary>
			/// 
			/// </summary>
			public int Revision;
			/// <summary>
			/// 
			/// </summary>
			public int Sbz1;
			/// <summary>
			/// 
			/// </summary>
			public int Size;
			/// <summary>
			/// 
			/// </summary>
			public int Count;
			/// <summary>
			/// 
			/// </summary>
			public int Sbz2;
		}
		
 
		/// <summary>
		/// Win32 Wrapper for TRUSTEE.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct Trustee
		{
			/// <summary>
			/// MultipleTrustee.
			/// </summary>
			public IntPtr MultipleTrustee;
			
			/// <summary>
			/// MultipleTrusteeOperation.
			/// </summary>
			public MultipleTrusteeOperation MultipleTrusteeOp;

			/// <summary>
			/// TrusteeForm.
			/// </summary>
			public TrusteeForm trusteeForm;

			/// <summary>
			/// TrusteeForm.
			/// </summary>
			public TrusteeType trusteeType;

			/// <summary>
			/// Name.
			/// </summary>
			public string Name;
		}

		/// <summary>
		/// Win32 Wrapper for EXPLICIT_ACCESS.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct ExplicitAccessStruct
		{
			/// <summary>
			/// A set of bit flags that use the ACCESS_MASK format to specify the access rights that an ACE allows, denies, or audits for the trustee. The functions that use the EXPLICIT_ACCESS structure do not convert, interpret, or validate the bits in this mask. 
			/// </summary>
			public AccessMask AccessPermissions;
			
			/// <summary>
			/// A value from the ACCESS_MODE enumeration. For a discretionary access control list (DACL), this flag indicates whether the ACL allows or denies the specified access rights. For a system access control list (SACL), this flag indicates whether the ACL generates audit messages for successful attempts to use the specified access rights, or failed attempts, or both. When modifying an existing ACL, you can specify the REVOKE_ACCESS flag to remove any existing ACEs for the specified trustee. 
			/// </summary>
			public AccessMode AccessMode;

			/// <summary>
			/// A set of bit flags that determines whether other containers or objects can inherit the ACE from the primary object to which the ACL is attached. The value of this member corresponds to the inheritance portion (low-order byte) of the AceFlags member of the ACE_HEADER structure. This parameter can be NO_INHERITANCE to indicate that the ACE is not inheritable; or it can be a combination of the following values.
			/// </summary>
			public AceFlags Inheritance;

			/// <summary>
			/// A TRUSTEE structure that identifies the user, group, or program (such as a Windows service) to which the ACE applies. 
			/// </summary>
			public Trustee trustee;
		}

		/// <summary>
		/// TRUSTEE_TYPE
		/// </summary>
		public enum TrusteeType
		{
			/// <summary>
			/// The trustee type is unknown, but not necessarily invalid. 
			/// </summary>
			Unknown,

			/// <summary>
			/// Indicates a user. 
			/// </summary>
			User,

			/// <summary>
			/// Indicates a group. 
			/// </summary>
			Group,

			/// <summary>
			/// Indicates a domain. 
			/// </summary>
			Domian,

			/// <summary>
			/// Indicates an alias.
			/// </summary>
			Alias,

			/// <summary>
			/// Indicates a well-known group. 
			/// </summary>
			IsWellKnownGroup,

			/// <summary>
			/// Indicates a deleted account. 
			/// </summary>
			IsDeleted,

			/// <summary>
			/// Indicates an invalid trustee type. 
			/// </summary>
			IsInvalid,

			/// <summary>
			/// Indicates a computer. 
			/// </summary>
			IsComputer,
		}

		#endregion

		#region Constants

		/// <summary>
		/// Win32 Wrapper for ACL_INFORMATION_CLASS.
		/// </summary>
		public enum Information : int
		{
			/// <summary>
			/// ACL revision information.
			/// </summary>
			Revision= 1,

			/// <summary>
			/// Indicates ACL size information.
			/// </summary>
			Size 
		}

		/// <summary>
		/// Win32 Wrapper for ACL_REVISION_INFORMATION.
		/// </summary>
		public enum Revision : int
		{
			/// <summary>
			/// ACL revision information.
			/// </summary>
			Revision=2,

			/// <summary>
			/// Indicates ACL size information.
			/// </summary>
			RevisionDS=4 
		}
	
		/// <summary>
		/// A set of bit flags that control ACE inheritance. The function sets these flags in the AceFlags member of the ACE_HEADER structure of the new ACE. This parameter can be a combination of the following values.
		/// </summary>
		public enum AceFlags : int
		{
			/// <summary>
			/// The ACE is inherited by noncontainer objects.
			/// </summary>
			OBJECT_INHERIT_ACE=0x1,

			/// <summary>
			/// The ACE is inherited by container objects.
			/// </summary>
			CONTAINER_INHERIT_ACE=0x2,

			/// <summary>
			/// The OBJECT_INHERIT_ACE and CONTAINER_INHERIT_ACE bits are not propagated to an inherited ACE.
			/// </summary>
			NO_PROPAGATE_INHERIT_ACE=0x4,
	
			/// <summary>
			/// The ACE does not apply to the object to which the access control list (ACL) is assigned, but it can be inherited by child objects.
			/// </summary>
			INHERIT_ONLY_ACE=0x8,
	
			/// <summary>
			/// Indicates an inherited ACE. This flag allows operations that change the security on a tree of objects to modify inherited ACEs while not changing ACEs that were directly applied to the object.
			/// </summary>
			INHERITED_ACE=0x10,
	
			/// <summary>
			/// Indicates ACL size information.
			/// </summary>
			VALID_INHERIT_FLAGS=0x1F 
		}

		/// <summary>
		/// ACCESS_MODE
		/// </summary>
		public enum AccessMode
		{
			/// <summary>
			/// Value not used
			/// </summary>
			NotUsed= 0,

			/// <summary>
			/// An input flag that creates an ACCESS_ALLOWED_ACE structure. The new ACE combines the specified rights with any existing allowed or denied rights of the trustee. 
			/// </summary>
			Grant,

			/// <summary>
			/// Indicates an ACCESS_ALLOWED_ACE structure that allows the specified rights. 
			/// </summary>
			/// <remarks>
			/// On input, this flag discards any existing access control information for the trustee.
			/// </remarks>
			Set,

			/// <summary>
			/// Indicates an ACCESS_DENIED_ACE structure that denies the specified rights. 
			/// </summary>
			/// <remarks>
			/// On input, this flag denies the specified rights in addition to any currently denied rights of the trustee.
			/// </remarks>
			Deny,

			/// <summary>
			/// An input flag that removes all existing ACCESS_ALLOWED_ACE or SYSTEM_AUDIT_ACE structures for the specified trustee. 
			/// </summary>
			Revoke,

			/// <summary>
			/// Indicates a SYSTEM_AUDIT_ACE structure that generates audit messages for successful attempts to use the specified access rights. You can combine this value with the SET_AUDIT_FAILURE member. 
			/// </summary>
			/// <remarks>
			/// On input, this flag combines the specified rights with any existing audited access rights for the trustee.
			/// </remarks>
			SetAuditSuccess,

			/// <summary>
			/// Indicates a SYSTEM_AUDIT_ACE structure that generates audit messages for failed attempts to use the specified access rights. You can combine this value with the SET_AUDIT_SUCCESS member. 
			/// </summary>
			/// <remarks>
			/// On input, this flag combines the specified rights with any existing audited access rights for the trustee.
			/// </remarks>
			SetAuditFailure
		}

		/// <summary>
		/// MULTIPLE_TRUSTEE_OPERATION
		/// </summary>
		public enum MultipleTrusteeOperation
		{
			/// <summary>
			/// Value not used
			/// </summary>
			NoMultipleTrustee,

			/// <summary>
			/// An input flag that creates an ACCESS_ALLOWED_ACE structure. The new ACE combines the specified rights with any existing allowed or denied rights of the trustee. 
			/// </summary>
			TrusteeIsImpersonate,
		}

		/// <summary>
		/// TRUSTEE_FORM
		/// </summary>
		public enum TrusteeForm
		{
			/// <summary>
			/// The ptstrName member is a pointer to a security identifier (SID) that identifies the trustee. 
			/// </summary>
			IsSID,

			/// <summary>
			/// The ptstrName member is a pointer to a null-terminated string that identifies the trustee. 
			/// </summary>
			IsName,

			/// <summary>
			/// Indicates an invalid trustee form. 
			/// </summary>
			BadForm,

			/// <summary>
			/// The ptstrName member is a pointer to an OBJECTS_AND_SID structure that contains the SID of the trustee and the GUIDs of the object types in an object-specific access control entry (ACE). 
			/// </summary>
			IsObjectsAndSID,

			/// <summary>
			/// The ptstrName member is a pointer to an OBJECTS_AND_NAME structure that contains the name of the trustee and the names of the object types in an object-specific ACE.
			/// </summary>
			IsObjectsAndName,
		}

		/// <summary>
		/// ACCESS_MASK
		/// </summary>
		/// <remarks>
		/// The ACCESS_MASK data type is a double word value that defines standard, specific, and generic rights. These rights are used in access control entries (ACEs) and are the primary means of specifying the requested or granted access to an object.
		/// </remarks>
		public enum AccessMask : long
		{
			/// <summary>
			/// The following constants represent the specific and standard access rights.
			/// </summary>
			SPECIFIC_RIGHTS_ALL=0x0000FFFFL,

			/// <summary>
			/// The following constants represent the specific and standard access rights.
			/// </summary>
			STANDARD_RIGHTS_ALL=0x001F0000L,

			/// <summary>
			/// The following constants represent the specific and standard access rights.
			/// </summary>
			STANDARD_RIGHTS_REQUIRED=0x000F0000L,

			/// <summary>
			/// Delete access.
			/// </summary>
			DELETE=0x00010000L,

			/// <summary>
			/// Read access to the owner, group, and discretionary access control list (DACL) of the security descriptor.
			/// </summary>
			READ_CONTROL=0x00020000L,

			/// <summary>
			/// Write access to the DACL.
			/// </summary>
			WRITE_DAC=0x00040000L,

			/// <summary>
			/// Write access to owner.
			/// </summary>
			WRITE_OWNER=0x00040000L,

			/// <summary>
			/// Synchronize access.
			/// </summary>
			SYNCHRONIZE=0x00100000L,

			/// <summary>
			/// 
			/// </summary>
			STANDARD_RIGHTS_READ=0x00020000L,

			/// <summary>
			/// 
			/// </summary>
			STANDARD_RIGHTS_WRITE=0x00020000L,

			/// <summary>
			/// 
			/// </summary>
			STANDARD_RIGHTS_EXECUTE=0x00020000L,

			/// <summary>
			/// Access system security (ACCESS_SYSTEM_SECURITY). It is used to indicate access to a system access control list (SACL). This type of access requires the calling process to have the SE_SECURITY_NAME (Manage auditing and security log) privilege. If this flag is set in the access mask of an audit access ACE (successful or unsuccessful access), the SACL access will be audited.
			/// </summary>
			ACCESS_SYSTEM_SECURITY=0x01000000L,

			/// <summary>
			/// Maximum allowed (MAXIMUM_ALLOWED).
			/// </summary>
			MAXIMUM_ALLOWED=0x02000000L,

			/// <summary>
			/// Generic all.
			/// </summary>
			GENERIC_ALL=0x10000000L,

			/// <summary>
			/// Generic execute.
			/// </summary>
			GENERIC_EXECUTE=0x20000000L,

			/// <summary>
			/// Generic write.
			/// </summary>
			GENERIC_WRITE=0x40000000L,

			/// <summary>
			/// Generic read.
			/// </summary>
			GENERIC_READ=0x80000000L
		}

		#endregion

        #region Member Data

		private IntPtr m_ACL=IntPtr.Zero;
		private IntPtr m_ACLInfo=IntPtr.Zero;
		private ExplicitAccessStruct m_ExplicitAccess;

        #endregion

        #region Accessors

		/// <summary>
		/// ExplicitAccess member accessor.
		/// </summary>
		public ExplicitAccessStruct ExplicitAccess
		{
			get
			{
				return m_ExplicitAccess;
			}
			set
			{
				m_ExplicitAccess=value;
			}
		}
		
		#endregion

        #region Constructors and Destructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ACL()
		{
			m_ExplicitAccess=new ExplicitAccessStruct();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="acl"></param>
		public ACL(IntPtr acl)
		{
			if(!IsValidAcl(acl))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
			
			m_ExplicitAccess=new ExplicitAccessStruct();

			m_ACL=acl;
			//byte [] info = new byte[20];
			IntPtr info=IntPtr.Zero;

			bool result=GetAclInformation(acl, out info, 100, (int)Information.Size );
			if(!result)
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
			//SizeInformation m_rInfo=new SizeInformation();
			//m_rInfo = (SizeInformation)Marshal.PtrToStructure(info, typeof(SizeInformation));
		}

		#endregion

        #region Methods
 
		/// <summary>
		/// AddAccessAllowedAce
		/// </summary>
		/// <param name="sid"></param>
		public void AddAccessAllowedAce(SecurityIdentifier sid)
		{
			if(!AddAccessAllowedAceEx(m_ACL, (int)Revision.Revision, (int)AceFlags.CONTAINER_INHERIT_ACE, 0, sid))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetEntriesInAcl()
		{
			IntPtr ea=new IntPtr(100);
			Marshal.StructureToPtr(this.ExplicitAccess, ea, false);

			SetEntriesInAcl(1, ea, m_ACL, out m_ACL);
		}
		
		#endregion

		#region Unmanaged

		/// <summary>
		/// IsValidSecurityDescriptor
		/// </summary>
		/// <param name="pAcl"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		static extern bool IsValidAcl(IntPtr pAcl);

		/// <summary>
		/// GetAclInformation
		/// </summary>
		/// <param name="acl"></param>
		/// <param name="info"></param>
		/// <param name="aclInformationLength"></param>
		/// <param name="aclInfoClass"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
		private static extern bool GetAclInformation(
			IntPtr acl,
			//[MarshalAs(UnmanagedType.LPArray)] byte[] aclInfo,
			out IntPtr info,
			int aclInformationLength,
			int aclInfoClass
			);

		/// <summary>
		/// AddAccessAllowedAce
		/// </summary>
		/// <param name="acl"></param>
		/// <param name="dwAceRevision"></param>
		/// <param name="AccessMask"></param>
		/// <param name="pSid"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
		private static extern bool AddAccessAllowedAce(
			IntPtr acl,
			int dwAceRevision,
			int AccessMask,
			IntPtr pSid
			);

		/// <summary>
		/// AddAccessAllowedAceEx
		/// </summary>
		/// <param name="acl"></param>
		/// <param name="dwAceRevision"></param>
		/// <param name="AceFlags"></param>
		/// <param name="AccessMask"></param>
		/// <param name="pSid"></param>
		/// <returns></returns>
		[DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
		private static extern bool AddAccessAllowedAceEx(
			IntPtr acl,
			int dwAceRevision,
			int AceFlags,
			int AccessMask,
			IntPtr pSid
			);

		[DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
		private static extern uint SetEntriesInAcl(
			int cCountOfExplicitEntries,
			IntPtr pListOfExplicitEntries,
			IntPtr OldAcl,
			out IntPtr NewAcl
			);
		#endregion
	}
}
