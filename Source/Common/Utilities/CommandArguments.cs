using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BioRad.Common.Utilities
{
	#region Documentation Tags
	/// <summary>
	/// CommandArguments command line parser
	/// Allowed command flag switches are "-" or "/".  
	/// Values for a flag (if a list) should be separated by whitespace.
	/// In case of a single-valued flag, the value can also be appended to the flag with "=". 
	/// If the parameter is standalone it will have a value-collection with Count==0
	/// Parameter values may also be enclosed by single or double quotes.
	/// 
	/// -simulation -run SerNum B "C:\Foo Faw.pltd" D:\f1\data\fee.prcl -r cmdresults.xml -l logfile.log -gui none
	/// The above command line arguments parse as follows:
	/// simulation:
	/// run:  SerNum B "C:\Foo Faw.pltd" D:\f1\data\fee.prcl
	/// gui:  none;
	/// r:  cmdresults.xml;
	/// l:  logfile.log;
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Geoff Dreyer, R. Lopes</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: CommandArguments.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Utilities/CommandArguments.cs $</item>
	///			<item name="vssrevision">$Revision: 4 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Vnguyen $</item>
	///			<item name="vssdate">$Date: 9/21/09 3:08p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class CommandArguments
	{
		private Hashtable m_parameters;

		/// <summary>
		/// Get the flags for the command line
		/// </summary>
		public ICollection Flags
		{
			get
			{
				return m_parameters.Keys;
			}
		}

		#region Constructors and Destructor
		/// <summary>Constructor takes the command line arguments to be parsed.</summary>
		public CommandArguments(string[] args)
		{
			m_parameters = new Hashtable();
			Regex splitter = new Regex(@"^-|^/|=",
				 RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex remover = new Regex(@"^['""]?(.*?)['""]?$",
				 RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string param = null;
			string[] parts;
			List<string> values = new List<string>();

			//Example: -param1 value1 value2 -param2 /param3="Test" /param4=happy
			foreach (string arg in args)
			{
				parts = splitter.Split(arg, 3);

				switch (parts.Length)
				{
					// Found value
					case 1:
						// if parameter still waits, add value to values collection
						if (param != null)
						{
							parts[0] = remover.Replace(parts[0], "$1");
							values.Add(parts[0]);
						}
						break;

					// found parameter
					case 2:
						// if parameter was still waiting,
						// then add parameter and values-collection to m_parameters
						// clear values
						if (param != null)
						{
							if (!m_parameters.ContainsKey(param))
							{
								m_parameters.Add(param, values);
								values = new List<string>();
							}
						}
						param = parts[1];
						break;

					// found parameter with enclosed value
					case 3:
						// if parameter was still waiting,
						// then add parameter and values-collection to m_parameters
						// clear values
						if (param != null)
						{
							if (!m_parameters.ContainsKey(param))
							{
								m_parameters.Add(param, values);
								values = new List<string>();
							}
						}

						// Parameter with enclosed value is allowed only one value.
						// add parameter and value to m_parameters
						param = parts[1];
						parts[2] = remover.Replace(parts[2], "$1");
						values.Add(parts[2]);

						if (!m_parameters.ContainsKey(param))
							m_parameters.Add(param, values);

						param = null;
						values = new List<string>();
						break;
				}
			}
			// if  final parameter is still waiting,
			// add parameter and values to m_parameters
			if (param != null)
			{
				if (!m_parameters.ContainsKey(param))
					m_parameters.Add(param, values);
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Whether a flag is contained by this object.
		/// </summary>
		/// <param name="flag">flag to check</param>
		/// <returns>true if the flag is contained, false if not.</returns>
		public bool ContainsFlag(string flag)
		{
			return m_parameters.ContainsKey(flag);
		}
		/// <summary>
		/// Retrieve a parameter value-collection if it exists
		/// </summary>
		/// <param name="param">command line flag</param>
		/// <returns>list of string values for param</returns>
		public List<string> this[string param]
		{
			get
			{
				return (List<string>)(m_parameters[param]);
			}
		}
		/// <summary>
		/// Return Hashtable
		/// </summary>
		public Hashtable CommmandParams
		{
			get { return m_parameters; }
		}
		#endregion

	}
}
