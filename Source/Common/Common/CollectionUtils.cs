using System;
using System.Collections;
namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// A set of static utility methods for dealing with collections.
	/// </summary>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors: Drew McAuliffe</item>
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
	///			<item name="vssfile">$Workfile: CollectionUtils.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/CollectionUtils.cs $</item>
	///			<item name="vssrevision">$Revision: 8 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Staran $</item>
	///			<item name="vssdate">$Date: 5/30/08 8:04p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class CollectionUtils
	{
        #region Methods
		/// <summary>
		/// Provides a way to add values to a hashtable where they may
		/// exists already, performing an overwrite if the object exists
		/// already at the supplied key. This makes the hashtable easier
		/// to use although it does have consequences.
		/// This is similar to how the Java Map interface's "put" method
		/// works.
		/// </summary>
		/// <param name="ht">Hashtable to work on</param>
		/// <param name="key">Key to place value at</param>
		/// <param name="value">Value to place at supplied key</param>
		public static void HashtableReplace(Hashtable ht, object key,
			object value)
		{
			// todo: Ralph - the below single line does the same thing as the if else below?
			// ht[key] = value;
			if (ht.ContainsKey(key))
			{
				ht.Remove(key);
				ht.Add(key, value);
			}
			else
			{
				ht.Add(key, value);
			}
		}
        /// <summary>
        /// Get single (floating point) value corresponding to 4 bytes binary data  
        /// </summary>
        /// <param name="buffer"> bytes array buffer</param>
        /// <param name="offset"> offset</param>
        public static float ConverToFloat(byte[] buffer, int offset)
        {
            //// Declare an array of 1 single
            System.Single[] s = new System.Single[1];

            // Copy 4 bytes->single
            Buffer.BlockCopy(buffer, offset, s, 0, 4);

            return (float)s[0];
        }

        /// <summary>
        /// Get integer value corresponding to 4 bytes binary data 
        /// </summary>
        /// <param name="buffer"> bytes array buffer</param>
        /// <param name="offset"> offset</param>
        public static int ConverToInt(byte[] buffer, int offset)
        {
            //// Declare an array of 1 single
            int[] s = new int[1];

            // Copy 4 bytes->single
            Buffer.BlockCopy(buffer, offset, s, 0, 4);

            return (int)s[0];
        }

        /// <summary>
        /// Get integer value corresponding to 4 bytes binary data 
        /// </summary>
        /// <param name="sourceInt"> bytes array buffer</param>
        /// <param name="targetBuffer"> bytes array buffer</param>
        /// <param name="offset"> offset</param>
        public static void ConverToBytes(int sourceInt, byte[] targetBuffer, int offset)
        {
            //// Declare an array of 1 single
            int[] s = new int[1];
            s[0] = sourceInt;

            Buffer.BlockCopy(s, 0, targetBuffer, offset, 4);
        }

        /// <summary>
        /// Get integer value corresponding to 4 bytes binary data 
        /// </summary>
        /// <param name="sourceFloat"> bytes array buffer</param>
        /// <param name="targetBuffer"> bytes array buffer</param>
        /// <param name="offset"> offset</param>
        public static void ConverToBytes(float sourceFloat, byte[] targetBuffer, int offset)
        {
            //// Declare an array of 1 single
            float[] s = new float[1];
            s[0] = sourceFloat;

            Buffer.BlockCopy(s, 0, targetBuffer, offset, 4);
        }
		
		/// <summary>
		/// Return byte[] array equivalent to float[] array passed 
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static byte[] ConvertToBytes(float[] source)
		{
			int len = source.Length;
			byte[] target = new byte[len * 4];
			
			int offset=0;
			int i = 0; while (i < len)
			{				
				CollectionUtils.ConverToBytes(source[i], target,offset);
				offset += 4;
				i++;
			}
			return target;
		}
        #endregion
	}
}
