using System;

namespace BioRad.Common.AuditTracking
{
	/// <summary>
	/// Summary description for AuditTrackingUtilities.
	/// </summary>
	public partial class Utilities
	{
		/// <summary>Constructor</summary>
		public Utilities()
		{
		}
		/// <summary>
		/// Throws up a message box if any inconsistencies are detected in a sequence
		/// of log entries.</summary>
		/// <param name="trail">the audit trail to be validated.</param>
		/// <returns>true if no inconsistencies were detected, false otherwise.</returns>
		public static bool ValidateAuditTrail( AuditSaveHeader[] trail )
		{
			if( trail != null )
			{
				bool first = true;
				System.DateTime lastTime = new System.DateTime(0);
				foreach( AuditSaveHeader item in trail )
				{
					if( first == false )
					{
						// Audit save headers are stored oldest to newest, so > is the 
						// appropriate comparison to check whether something is out of
						// order here.
						if( item.Time.Ticks > lastTime.Ticks )
							return false;
					}
					lastTime = item.Time;
					first = false;
				}
			}
			return true;
		}
	}
}

