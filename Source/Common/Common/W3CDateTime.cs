using System;
using System.Globalization;
using System.Text;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// W3C format date time class.
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Ralph Ansell</item>
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
	///			<item name="vssfile">$Workfile: W3CDateTime.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/W3CDateTime.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
	///			<item name="vssdate">$Date: 8/04/06 3:24p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class W3CDateTime// Test Track 151
	{
		#region Methods
        /// <summary>
        /// Parse ISO 8601 time.
        /// </summary>
        /// <param name="iso8601Time"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        private static DateTime ParseAs(string iso8601Time, CultureInfo cultureInfo)
        {
            const DateTimeStyles style = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
            string[] formats = {
              "yyyy-MM-ddTHH:mm:ss.fffffffK" ,
              "yyyy-MM-ddTHH:mm:ss.ffffffK" ,
              "yyyy-MM-ddTHH:mm:ss.fffffK" ,
              "yyyy-MM-ddTHH:mm:ss.ffffK" ,
              "yyyy-MM-ddTHH:mm:ss.fffK" , // K accepts
              "yyyy-MM-ddTHH:mm:ss.ffK"  , // the 'Z' suffix
              "yyyy-MM-ddTHH:mm:ss.fK"   , // or no suffix
              "yyyy-MM-ddTHH:mm:ssK"     ,
              "yyyy-MM-ddTHH:mm:ssK"     ,
              };

            return DateTime.ParseExact(iso8601Time, formats, cultureInfo, style);
        }
		/// <summary>
        /// Convert ISO 8601 date time string representation to local.
		/// </summary>
        /// <param name="iso8601Time"> ISO 8601 date time string representation.</param>
        /// <param name="cultureInfo"></param>
		/// <returns>Local DateTime object</returns>
        public static DateTime ToLocal(string iso8601Time, CultureInfo cultureInfo)
		{
            DateTime dateTime = DateTime.Now;
            try
            {
                dateTime = ParseAs(iso8601Time, cultureInfo);
            }
            catch
            {
                try// TT 196
                {
                    dateTime = DateTime.Parse(iso8601Time);
                }
                catch
                {
                    dateTime = new DateTime(0, 0, 0, 0, 0, 0, DateTimeKind.Unspecified);
                }
            }

            return dateTime;
		}
		/// <summary>
        /// Converts local time to its ISO 8601 time string representation.
		/// </summary>
		/// <param name="dateTime">Reference to local DateTime object.</param>
        /// <param name="cultureInfo"></param>
		/// <returns>
		/// Local date time in Universal Time Coordinated as a string.
		/// 1994-11-05T08:15:30-05:00 corresponds to November 5, 1994, 8:15:30 am, US Eastern Standard Time.
		/// </returns>
        public static string ToUtc(DateTime dateTime, CultureInfo cultureInfo)
		{
            StringBuilder sb = new StringBuilder();
            TimeZone timeZone = TimeZone.CurrentTimeZone;
            TimeSpan ofs = timeZone.GetUtcOffset(dateTime);

            // Similar to "O" or "o" standard format specifier that complies with ISO 8601 except using 24 hour time.
            sb.Append((dateTime + ofs).ToString("yyyy-MM-ddTHH:mm:ss.fff", cultureInfo));
            sb.Append(FormatOffset(ofs));

            return sb.ToString();
		}
		/// <summary>
        /// Get difference between two ISO 8601 date time string representation.
		/// </summary>
        /// <param name="iso8601Time1">UTC date time string representation.</param>
        /// <param name="iso8601Time2">UTC date time string representation.</param>
        /// <param name="cultureInfo"></param>
		/// <returns>TimeSpan object that represents the difference between the two date times.</returns>
        public static TimeSpan Diff(string iso8601Time1, string iso8601Time2, CultureInfo cultureInfo)
		{
            if (iso8601Time1 == null || iso8601Time1.Length == 0)
				throw new ArgumentNullException();
            if (iso8601Time2 == null || iso8601Time2.Length == 0)
				throw new ArgumentNullException();

            DateTime dt1 = ToLocal(iso8601Time1, cultureInfo);
            DateTime dt2 = ToLocal(iso8601Time2, cultureInfo);
			if ( dt1 < dt2 )
				return dt2 - dt1;
			if ( dt2 < dt1 )
				return dt1 - dt2;
			return dt1 - dt2;
		}
		/// <summary>
		/// Format time zone.
		/// </summary>
		/// <param name="ofs">Time span</param>
		/// <returns>formatted string</returns>
		private static string FormatOffset(TimeSpan ofs)
		{
			string s = string.Empty;
			if (ofs >= TimeSpan.Zero)
				s = "+";
			return s + ofs.Hours.ToString("00") + ":" + ofs.Minutes.ToString("00");
		}
		#endregion
	}
}
