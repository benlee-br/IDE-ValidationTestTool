using System;
using System.Collections.Generic;
using System.Text;

namespace BioRad.Common.Utilities
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
   ///			<item name="vssfile">$Workfile: MJUtilities.cs $</item>
   ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Utilities/MJUtilities.cs $</item>
   ///			<item name="vssrevision">$Revision: 2 $</item>
   ///			<item name="vssauthor">Last Check-in by:$Author: Dprzyby $</item>
   ///			<item name="vssdate">$Date: 8/04/06 3:25p $</item>
   ///		</list>
   /// </archiveinformation>
   #endregion
	public partial class MJUtilities
   {
      #region Methods
      /// <summary>
      /// Creates a dictionary of string/string keys/values out of a serialized 
      /// COptions class.  COptions was a staple of the MJ codebase.
      /// </summary>
      /// <param name="serializedCOptions">the serialized COptions</param>
      /// <returns>dictionary corresponding to the COptions.</returns>
      public static Dictionary<string, string>
         CreateDictionaryFromCOptions(string serializedCOptions)
      {
         // Tokenize the string into a dictionary of key/value pairs where key
         //  is a string and value is a string.
         Dictionary<string, string> dictionary = new Dictionary<string, string>();
         string[] lines = serializedCOptions.Split('\n');
         foreach (string line in lines)
         {
            if (line.Equals(""))
               continue;
            int separatorIndex = line.IndexOf(":");
            if (separatorIndex <= 0)
            {
               System.Diagnostics.Debug.Assert(false);
               continue;
            }
            string key = line.Substring(0, separatorIndex);
            string value = line.Substring(separatorIndex + 1);
            dictionary.Add(key, value);
         }
         return dictionary;
      }
      #endregion
   }
}
