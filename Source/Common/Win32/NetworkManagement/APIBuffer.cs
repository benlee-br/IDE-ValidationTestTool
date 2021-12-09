using System;
using System.Runtime.InteropServices;

namespace BioRad.Win32.NetworkManagement
{
	
	/// <summary>
	/// NetAPI32 Error codes.
	/// </summary>
	internal enum ErrorCodes : int
	{
		/// <summary>
		/// Success.
		/// </summary>
		Success=0,

		/// <summary>
		/// The group name could not be found.
		/// </summary>
		GroupNotFound=2220,

		/// <summary>
		/// The user name could not be found.
		/// </summary>
		UserNotFound=2221,

		/// <summary>
		/// Buffer too small.
		/// </summary>
		BufTooSmall=2123,

		/// <summary>
		/// This computer name is invalid. 
		/// </summary>
		InvalidComputer=2351
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
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Win32/NetworkManagement/APIBuffer.cs $</item>
	///			<item name="vssrevision">$Revision: 11 $</item>
	///			<item name="vssauthor">Last Check-in by: $Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	internal partial class APIBuffer : IDisposable
	{
		#region Member Data

		System.IntPtr m_This=System.IntPtr.Zero;

		#endregion
		
		#region Constructors and Destructor
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public APIBuffer()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public APIBuffer(int size)
		{
			switch(NetApiBufferAllocate(size, out m_This))
			{
				case	0:
					return;
				default:
					break;
			}
		}

		#endregion

        #region Methods

		/// <summary>
		/// ThrowThisError
		/// </summary>
		/// <param name="error"></param>
		/// <param name="source"></param>
		public static void ThrowThisError(ErrorCodes error, string source)
		{
			System.Exception ex=null;
			
			switch(error)
			{
				case ErrorCodes.Success				:	return;	//	No Error.
				case ErrorCodes.GroupNotFound		:	ex=new System.Exception("The group name could not be found.");	break;
				case ErrorCodes.UserNotFound		:	ex=new System.Exception("The user name could not be found.");	break;
				case ErrorCodes.BufTooSmall			:	ex=new System.Exception("Buffer too small.");	break;
				case ErrorCodes.InvalidComputer		:	ex=new System.Exception("This computer name is invalid.");	break;
				default								:	BioRad.Win32.Kernel.APIBuffer.ThrowThisError((BioRad.Win32.Kernel.ErrorCodes)error,source);	break;
			}

			if(source!=null && source.Length>0)
				ex.Source=source;

			throw ex;
		}
 
		public void Dispose()
		{
			BioRad.Win32.Kernel.APIBuffer.ThrowThisError((BioRad.Win32.Kernel.ErrorCodes)NetApiBufferFree(m_This), "BioRad.Win32.NetworkManagement.Dispose");
		}

		/// <summary>
		/// Win32 Wrapper for NetApiBufferFree
		/// <param name="buffer">Buffer containing returned data from API call.</param>
		/// </summary>
		public static void Free(IntPtr buffer)
		{
			BioRad.Win32.Kernel.APIBuffer.ThrowThisError((BioRad.Win32.Kernel.ErrorCodes)NetApiBufferFree(buffer), "BioRad.Win32.NetworkManagement.Free");
		}

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
		
		#endregion

		#region Implicit Operators
 
		public static implicit operator System.IntPtr(BioRad.Win32.NetworkManagement.APIBuffer buffer) 
		{
			return buffer.m_This;
		}
 
		#endregion

		#region Unmanaged API

		/// <summary>
		/// Entry point for NetApiBufferFree
		/// </summary>
		/// <param name="byteCount"></param>
		/// <param name="buffer"></param>
		/// <returns></returns>
		[DllImport( "Netapi32.dll", EntryPoint="NetApiBufferAllocate", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int NetApiBufferAllocate(int byteCount, out IntPtr buffer);

		/// <summary>
		/// Entry point for NetApiBufferFree
		/// <param name="buffer">Buffer containing returned data from API call.</param>
		/// </summary>
		[DllImport( "Netapi32.dll", EntryPoint="NetApiBufferFree", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int NetApiBufferFree(IntPtr buffer);

		#endregion
	}
}
