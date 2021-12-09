using System;
using System.Runtime.InteropServices;

namespace BioRad.Win32.Kernel
{
	
	/// <summary>
	/// Win32 Error codes.
	/// </summary>
	internal enum ErrorCodes : long
	{
		/// <summary>
		/// No Error.
		/// </summary>
		SUCCESS=0,

		/// <summary>
		/// File not found.
		/// </summary>
		FILE_NOT_FOUND=2,

		/// <summary>
		/// Access is denied.
		/// </summary>
		ACCESS_DENIED=5,

		/// <summary>
		/// Not enough storage is available to process this command.
		/// </summary>
		NOT_ENOUGH_MEMORY=8,

		/// <summary>
		/// The network path was not found. 
		/// </summary>
		BAD_NETPATH=53,

		/// <summary>
		/// The parameter is incorrect. 
		/// </summary>
		INVALID_PARAMETER=87,

		/// <summary>
		/// The data area passed to a system call is too small.
		/// </summary>
		INSUFFICIENT_BUFFER=122,	

		/// <summary>
		/// The value specified for the level parameter is invalid. 
		/// </summary>
		INVALID_LEVEL=124,	
	
		/// <summary>
		/// More data is available.
		/// </summary>
		MORE_DATA=234,
	
		/// <summary>
		/// The access control list (ACL) structure is invalid.
		/// </summary>
		INVALID_ACL=1336,
	
		/// <summary>
		/// The security ID structure is invalid.
		/// </summary>
		INVALID_SID=1337,
	
		/// <summary>
		/// No more memory is available for security information updates.
		/// </summary>
		ALLOTTED_SPACE_EXCEEDED=1344,
	
		/// <summary>
		/// Indicates two revision levels are incompatible.
		/// </summary>
		REVISION_MISMATCH=1306,
	
		/// <summary>
		/// No mapping between account names and security IDs was done.
		/// </summary>
		NONE_MAPPED=1332L,
	
		/// <summary>
		/// The specified domain either does not exist or could not be contacted.
		/// </summary>
		NO_SUCH_DOMAIN=1355L,
	
		/// <summary>
		/// The trust relationship between this workstation and the primary domain failed.
		/// </summary>
		TRUSTED_RELATIONSHIP_FAILURE=1789L,
	}


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
	///			<item name="vssfile">$Workfile: APIBuffer.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/Kernel/APIBuffer.cs $</item>
	///			<item name="vssrevision">$Revision: 16 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	internal partial class APIBuffer : IDisposable
	{
		#region Constants
		
		public enum Memory
		{
			/// <summary>
			/// Allocates fixed memory. The return value is a pointer to the memory object.
			/// </summary>
			FIXED=0x0000,

			/// <summary>
			/// Same as LMEM_FIXED. 
			/// </summary>
			NONZEROLPTR=0x0000,

			/// <summary>
			/// Allocates movable memory. Memory blocks are never moved in physical memory, but they can be moved within the default heap. 
			/// The return value is a handle to the memory object. To translate the handle to a pointer, use the LocalLock function.
			/// This value cannot be combined with LMEM_FIXED.
			/// </summary>
			MOVEABLE=0x0002,

			/// <summary>
			/// Same as LMEM_MOVEABLE. 
			/// </summary>
			NONZEROLHND=0x0002, 

			/// <summary>
			/// Initializes memory contents to zero. 
			/// </summary>
			ZEROINIT=0x0040,

			/// <summary>
			/// Combines LMEM_FIXED and LMEM_ZEROINIT. 
			/// </summary>
			LPTR=0x0040,

			/// <summary>
			/// Combines LMEM_MOVEABLE and LMEM_ZEROINIT.
			/// </summary>
			LHND=0x0042,
		}


		/// <summary>
		/// Formatting options, and how to interpret the lpSource parameter. 
		/// </summary>
		/// <remarks>
		/// The low-order byte of dwFlags specifies how the function handles line breaks in the output buffer. 
		/// The low-order byte can also specify the maximum width of a formatted output line. 
		/// This parameter can be one or more of the following values.
		/// </remarks>
		public enum MessageFormat : long
		{
			/// <summary>
			/// The lpBuffer parameter is a pointer to a PVOID pointer, and that the nSize parameter specifies the minimum number of TCHARs to allocate for an output message buffer.
			/// </summary>
			/// <remarks>
			/// The function allocates a buffer large enough to hold the formatted message, and places a pointer to the allocated buffer at the address specified by lpBuffer. 
			/// The caller should use the LocalFree function to free the buffer when it is no longer needed.
			/// </remarks>
			Allocate=0x00000100,

			/// <summary>
			/// Insert sequences in the message definition are to be ignored and passed through to the output buffer unchanged.
			/// </summary>
			/// <remarks>
			/// This flag is useful for fetching a message for later formatting. If this flag is set, the Arguments parameter is ignored.
			/// </remarks>
			IgnoreInserts=0x00000200,

			/// <summary>
			/// The lpSource parameter is a pointer to a null-terminated message definition. 
			/// </summary>
			/// <remarks>
			/// If this lpSource handle is NULL, the current process's application image file will be searched. 
			/// Cannot be used with FORMAT_MESSAGE_FROM_STRING.
			/// If the module has no message table resource, the function fails with ERROR_RESOURCE_TYPE_NOT_FOUND.
			/// </remarks>
			FromString=0x00000400,

			/// <summary>
			/// The lpSource parameter is a module handle containing the message-table resource(s) to search. 
			/// </summary>
			/// <remarks>
			/// The message definition may contain insert sequences, just as the message text in a message table resource may. 
			/// Cannot be used with FORMAT_MESSAGE_FROM_HMODULE or FORMAT_MESSAGE_FROM_SYSTEM.
			/// </remarks>
			FromHModule=0x00000800,

			/// <summary>
			/// The function should search the system message-table resource(s) for the requested message.
			/// </summary>
			/// <remarks>
			/// If this lpSource handle is NULL, the current process's application image file will be searched.
			/// Cannot be used with FORMAT_MESSAGE_FROM_STRING.
			/// If the module has no message table resource, the function fails with ERROR_RESOURCE_TYPE_NOT_FOUND.
			/// </remarks>
			FromSystem=0x00001000,

			/// <summary>
			/// The Arguments parameter is not a va_list structure, but is a pointer to an array of values that represent the arguments.
			/// </summary>
			/// <remarks>
			/// If this flag is specified with FORMAT_MESSAGE_FROM_HMODULE, the function searches the system message table if the message is not found in the module specified by lpSource. Cannot be used with FORMAT_MESSAGE_FROM_STRING.
			/// If this flag is specified, an application can pass the result of the GetLastError function to retrieve the message text for a system-defined error.
			/// </remarks>
			ArgumentArray=0x00002000
		}

		#endregion

        #region Member Data

		private System.IntPtr m_This;
		private Memory m_Flags;

        #endregion

        #region Accessors
  
		/// <summary>
		/// IntPtr accessor.
		/// </summary>
		public System.IntPtr Member
		{
			get
			{
				return m_This;
			}
		}
  
		/// <summary>
		/// Size accessor.
		/// </summary>
		public int Size
		{
			get
			{
				return LocalSize(m_This);
			}
		}
		
		#endregion

        #region Constructors and Destructor

		public APIBuffer(){}

		public APIBuffer(int size)
		{
			m_Flags=Memory.FIXED;
			m_This=LocalAlloc(0, size);

			if((int)m_This==0)
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
		}

		#endregion

        #region Methods

		/// <summary>
		/// ThrowGetLastError
		/// </summary>
		public static void ThrowGetLastError()
		{
			ThrowGetLastError(null);
		}
 
		/// <summary>
		/// ThrowGetLastError
		/// </summary>
		/// <param name="source"></param>
		public static void ThrowGetLastError(string source)
		{
			ThrowThisError((ErrorCodes)Marshal.GetLastWin32Error(),source);
		}
 
		/// <summary>
		/// ThrowThisError
		/// </summary>
		/// <param name="error"></param>
		/// <param name="source"></param>
		public static void ThrowThisError(ErrorCodes error, string source)
		{
			System.Exception ex=null;
			
			//TODO: Localize these strings
				switch(error)
			{
				case ErrorCodes.SUCCESS							:	return;	//	No Error.
				case ErrorCodes.FILE_NOT_FOUND					:	ex=new System.Exception("File not found.");	break;
				case ErrorCodes.ACCESS_DENIED					:	ex=new System.Exception("Access is denied.");	break;
				case ErrorCodes.NOT_ENOUGH_MEMORY				:	ex=new System.Exception("Not enough storage is available to process this command.");	break;
				case ErrorCodes.BAD_NETPATH						:	ex=new System.Exception("The network path was not found.");	break;
				case ErrorCodes.INVALID_PARAMETER				:	ex=new System.Exception("The parameter is incorrect.");	break;
				case ErrorCodes.INSUFFICIENT_BUFFER				:	ex=new System.Exception("The data area passed to a system call is too small.");	break;
				case ErrorCodes.INVALID_LEVEL					:	ex=new System.Exception("The value specified for the level parameter is invalid.");	break;
				case ErrorCodes.MORE_DATA						:	ex=new System.Exception("More data is available.");	break;
				case ErrorCodes.INVALID_ACL						:	ex=new System.Exception("The access control list (ACL) structure is invalid.");	break;
				case ErrorCodes.INVALID_SID						:	ex=new System.Exception("The security ID structure is invalid.");	break;
				case ErrorCodes.ALLOTTED_SPACE_EXCEEDED			:	ex=new System.Exception("No more memory is available for security information updates.");	break;
				case ErrorCodes.REVISION_MISMATCH				:	ex=new System.Exception("Indicates two revision levels are incompatible.");	break;
				case ErrorCodes.NONE_MAPPED						:	ex=new System.Exception("No mapping between account names and security IDs was done.");	break;
				case ErrorCodes.NO_SUCH_DOMAIN					:	ex=new System.Exception("The specified domain either does not exist or could not be contacted.");	break;
				case ErrorCodes.TRUSTED_RELATIONSHIP_FAILURE	:	ex=new System.Exception("The trust relationship between this workstation and the primary domain failed.");	break;
				default											:	ex=new System.Exception("Unknown Error: "+error.ToString());	break;
			}

			if(source!=null && source.Length>0)
				ex.Source=source;

			throw ex;
		}
 
		/// <summary>
		/// Win32 Wrapper for NetApiBufferFree
		/// <param name="buffer">Buffer containing returned data from API call.</param>
		/// </summary>
		public static void Dispose(IntPtr buffer)
		{
			if(0!=(int)(buffer=LocalFree(buffer)))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
		}
		
		/// <summary>
		/// Win32 Wrapper for NetApiBufferFree
		/// <param name="buffer">Buffer containing returned data from API call.</param>
		/// </summary>
		public static void Dispose(byte[] buffer)
		{
			LocalFree(buffer);
		}
		
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			if(0!=(int)(m_This=LocalFree(m_This)))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
		}

		/// <summary>
		/// ReAllocate
		/// </summary>
		/// <param name="size"></param>
		public void ReAllocate(int size)
		{
			if(0==(int)(m_This=LocalReAlloc(m_This, size, (int)m_Flags)))
				BioRad.Win32.Kernel.APIBuffer.ThrowGetLastError();
		}

		#endregion

        #region Implicit Operators
 
		public static implicit operator System.IntPtr(BioRad.Win32.Kernel.APIBuffer intptr) 
		{
			return intptr.m_This;
		}
 
		#endregion

		#region Unmanaged API
		
		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern uint FormatMessage(
			uint dwFlags, 
			IntPtr lpSource,
			uint dwMessageId, 
			uint dwLanguageId, 
			[Out] System.Text.StringBuilder lpBuffer,
			uint nSize, 
			IntPtr Arguments
			);

		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern System.IntPtr LocalAlloc(int flg, int cb);
		
		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern System.IntPtr LocalFree([MarshalAs(UnmanagedType.LPArray)] byte[] ptr );
		
		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern System.IntPtr LocalFree( System.IntPtr ptr );
		
		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern System.IntPtr LocalReAlloc(
			System.IntPtr hMem, 
			int uBytes, 
			int fuFlags 
			); 	
		
		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int LocalSize(System.IntPtr hMem);

		#endregion
	}
}
