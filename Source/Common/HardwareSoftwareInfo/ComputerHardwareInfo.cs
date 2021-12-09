using System;
using System.Runtime.InteropServices;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>This class contains information about the hardware of the computer used for
	/// the PCR run.</summary>
	/// <remarks>This information is mainly for diagnostic purposes.</remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Shabnam</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: ComputerHardwareInfo.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/HardwareSoftwareInfo/ComputerHardwareInfo.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion
	
	[Serializable]
	public partial class ComputerHardwareInformation
	{
		#region Constants
        #endregion

        #region Member Data
		/// <summary>The Processor architecture used for the PCR run.</summary>
		private int m_ProcessorArchitecture;
		/// <summary>Reserved word.</summary>
		private int m_Reserved;
		/// <summary>The memory page size used for the PCR run.</summary>
		private int m_PageSize;
		/// <summary>The bitmapped processor field used for the PCR run.</summary>
		private int m_ActiveProcessorMask;
		/// <summary>The number of processors.</summary>
		private int m_NumberOfProcessors;
		/// <summary>The Virtual allocation granularity.</summary>
		private int m_AllocationGranularity;
		/// <summary>The Architecture-dependent processor level.</summary>
		private int m_ProcessorLevel;
		/// <summary>The Architecture-dependent processor revision.</summary>
		private int m_ProcessorRevision;
        #endregion

        #region Accessors
		/// <summary>Gets / Sets the  Processor architecture used for the PCR run.</summary>
		public int ProcessorArchitecture
		{ 
			get { return this.m_ProcessorArchitecture;}
			set { this.m_ProcessorArchitecture = value;}
		}
		/// <summary>Gets / Sets the Reserved word.</summary>
		public int Reserved
		{
			get { return this.m_Reserved;}
			set { this.m_Reserved = value;}
		}
		/// <summary>Gets / Sets the  memory page size used for the PCR run.</summary>
		public int PageSize
		{
			get { return this.m_PageSize;}
			set { this.m_PageSize = value; }
		}
		/// <summary>Gets / Sets the  bitmapped processor field used for the PCR run.</summary>
		public int ActiveProcessorMask
		{
			get { return this.m_ActiveProcessorMask;}
			set { this.m_ActiveProcessorMask = value;}
		}
		/// <summary>Gets / Sets the  number of processors.</summary>
		public int NumberOfProcessors
		{
			get { return this.m_NumberOfProcessors;}
			set { this.m_NumberOfProcessors = value;}
		}
		/// <summary>Gets / Sets the  Virtual allocation granularity.</summary>
		public int AllocationGranularity
		{
			get { return this.m_AllocationGranularity;}
			set { this.m_AllocationGranularity = value;}
		}
		/// <summary>Gets / Sets the  Architecture-dependent processor level.</summary>
		public int ProcessorLevel
		{
			get { return this.m_ProcessorLevel;}
			set { this.m_ProcessorLevel = value;}
		}
		/// <summary>Gets / Sets the  Architecture-dependent processor revision..</summary>
		public int ProcessorRevision
		{
			get { return this.m_ProcessorRevision;}
			set { this.m_ProcessorRevision = value;}
		}

        #endregion

		#region Delegates and Events
		#endregion

        #region Constructors and Destructor
		/// <summary>Constructs a new ComputerHardwareInfo instance.</summary>
		/// <remarks>Default constructor.</remarks>
		public ComputerHardwareInformation()
		{
			// Retrieve the hardware information.
			GetHardwareInfo();
		}
		/// <summary>Constructs a new ComputerHardwareInfo instance.</summary>
		/// <remarks>Sets the member data to the passed in parameters</remarks>
		/// <param name="processorArchitecture">The Processor architecture</param>
		/// <param name="reserved">Reserved word.</param>
		/// <param name="pageSize">The memory page size</param>
		/// <param name="activeProcessorMask">The bitmapped processor field</param>
		/// <param name="numOfProcessors">The number of processors.</param>
		/// <param name="allocationGranularity">The Virtual allocation granularity.</param>
		/// <param name="processorLevel">The Architecture-dependent processor level.</param>
		/// <param name="processorRevision">The Architecture-dependent processor revision.</param>
		public ComputerHardwareInformation(int processorArchitecture, int reserved, int pageSize,
			int activeProcessorMask, int numOfProcessors, int allocationGranularity,
			int processorLevel, int processorRevision)
		{
			this.m_ProcessorArchitecture = processorArchitecture;
			this.m_Reserved = reserved;
			this.m_PageSize = pageSize;
			this.m_ActiveProcessorMask = activeProcessorMask;
			this.m_NumberOfProcessors = numOfProcessors;
			this.m_AllocationGranularity = allocationGranularity;
			this.m_ProcessorLevel = processorLevel;
			this.m_ProcessorRevision = processorRevision;
		}
        #endregion

        #region Methods
		[DllImport("kernel32")] 
		static extern void GetSystemInfo(ref SYSTEM_INFO pSI); 

		[StructLayout(LayoutKind.Sequential)] 
		internal struct SYSTEM_INFO 
		{
			internal uint OemId;
//			public uint ProcessorArchitecture;
//			public uint Reserved;
			internal uint PageSize;
			internal uint MinimumApplicationAddress;
			internal uint MaximumApplicationAddress;
			internal uint ActiveProcessorMask;
			internal uint NumberOfProcessors;
			internal uint ProcessorType;
			internal uint AllocationGranularity;
			internal uint ProcessorLevel;
			internal uint ProcessorRevision; 
		}
		
		/// <summary>Retrieve the hardware information.</summary>
		private void GetHardwareInfo()
		{
			SYSTEM_INFO systemInfo = new SYSTEM_INFO();
			GetSystemInfo( ref systemInfo);
//			this.m_ProcessorArchitecture;
//			this.m_Reserved;
			this.m_PageSize  = (int) systemInfo.PageSize;
			this.m_ActiveProcessorMask = (int) systemInfo.ActiveProcessorMask;
			this.m_NumberOfProcessors = (int) systemInfo.NumberOfProcessors;
			this.m_AllocationGranularity = (int) systemInfo.AllocationGranularity;
			this.m_ProcessorLevel = (int) systemInfo.ProcessorLevel;
			this.m_ProcessorRevision = (int) systemInfo.ProcessorRevision;
		}
        #endregion

		#region Event Handlers
		#endregion
	}
}
