using System;

namespace BioRad.Common.DiagnosticsLogger
{
	/// <summary>
	/// 
	/// </summary>
	public interface IFormat
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logItem"></param>
		/// <returns></returns>
		string Format(DiagnosticsLogItem logItem);
	}
}
