using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace BioRad.Common
{
	/// <summary></summary>
	public partial class ImageUtils
	{
		#region Constants
		#endregion

		#region Member Data
		#endregion

		#region Accessors
		#endregion

		#region Delegates and Events
		#endregion

		#region Constructors
		/// <summary>Initializes a new instance of the ImageUtils class.</summary>
		public ImageUtils() { }
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctl"></param>
		/// <returns></returns>
		public static Bitmap GetImage(Control ctl)
		{
			Bitmap bm = new Bitmap(ctl.Width, ctl.Height);
			ctl.DrawToBitmap(bm, new Rectangle(0, 0, ctl.Width, ctl.Height));
			return bm;
		}
		/// <summary>
		/// Bitmap To Base 64 String
		/// </summary>
		/// <param name="bmp">Bitmap</param>
		/// <param name="imageFormat">Image format</param>
		/// <returns></returns>
		public static string BitmapToBase64String(Bitmap bmp, ImageFormat imageFormat)
		{
			string base64String = string.Empty;
			if (bmp != null)
			{
				MemoryStream memoryStream = null;
				byte[] byteBuffer = null;
				try
				{
					memoryStream = new MemoryStream();
					if (memoryStream != null)
					{
						bmp.Save(memoryStream, imageFormat);

						memoryStream.Position = 0;
						byteBuffer = memoryStream.ToArray();

						base64String = Convert.ToBase64String(byteBuffer);
					}
				}
				catch (Exception ex)
				{
					string m = ex.Message;
				}
				finally
				{
					if (memoryStream != null)
						memoryStream.Close();
					memoryStream = null;

					if (byteBuffer != null)
						byteBuffer = null;
				}
			}
			return base64String;
		}
		/// <summary>
		/// Base 64 String To Bitmap
		/// </summary>
		/// <param name="base64String">base64String string</param>
		/// <returns></returns>
		public static Bitmap Base64StringToBitmap(string base64String)
		{
			Bitmap bmpReturn = null;
			MemoryStream memoryStream = null;
			byte[] byteBuffer = null;
			try
			{
				byteBuffer = Convert.FromBase64String(base64String);
				if (byteBuffer != null)
				{
					memoryStream = new MemoryStream(byteBuffer);
					if (memoryStream != null)
					{
						memoryStream.Position = 0;
						bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
					}
				}
			}
			catch (Exception ex)
			{
				string m = ex.Message;
			}
			finally
			{
				if (memoryStream != null)
					memoryStream.Close();
				memoryStream = null;

				if (byteBuffer != null)
					byteBuffer = null;
			}

			return bmpReturn;
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
