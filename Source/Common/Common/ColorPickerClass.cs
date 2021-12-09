using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace BioRad.CustomControls.DotNet
{
	/// <summary>Class to get the color under the mouse pointer.</summary>
	public static class ColorPickerClass
	{
		#region DllImports
		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(ref Point lpPoint);

		[DllImport("user32.dll")]
		private static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll")]
		private static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("gdi32.dll")]
		private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
		#endregion

		#region Methods
		/// <summary>Gets the current x and y position of the mouse pointer</summary>
		/// <returns>A Point object.</returns>
		private static Point GetCurrentPosition()
		{
			Point p = new Point();
			GetCursorPos(ref p);
			return p;
		}
		/// <summary>Gets the color of the pixel under the mouse pointer.</summary>
		/// <returns>A color object.</returns>
		public static Color GetPixelColor()
		{
			Point currentPoint = GetCurrentPosition();
			IntPtr hdc = GetDC(IntPtr.Zero);
			uint pixel = GetPixel(hdc, currentPoint.X, currentPoint.Y);
			ReleaseDC(IntPtr.Zero, hdc);
			Color color = Color.FromArgb((int)(pixel & 0x000000FF),
						 (int)(pixel & 0x0000FF00) >> 8,
						 (int)(pixel & 0x00FF0000) >> 16);
			return color;
		}
		/// <summary>A display to display the color info.</summary>
		/// <param name="color">The color object.</param>
		/// <returns>A string.</returns>
		public static string GetColorInfo(Color color)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Color Info: {0}", Environment.NewLine);
			sb.AppendFormat("Red = {0}{1}", color.R.ToString(), Environment.NewLine);
			sb.AppendFormat("Green = {0}{1}", color.G.ToString(), Environment.NewLine);
			sb.AppendFormat("Blue = {0}{1}", color.B.ToString(), Environment.NewLine);
			sb.AppendFormat("Hue = {0}{1}", color.GetHue().ToString(), Environment.NewLine);
			sb.AppendFormat("Saturation = {0}{1}", color.GetSaturation().ToString(), Environment.NewLine);
			sb.AppendFormat("Brightness = {0}", color.GetBrightness().ToString());
			return sb.ToString();
		}
		#endregion
	}
}
