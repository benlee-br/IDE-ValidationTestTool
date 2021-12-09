using System;

namespace BioRad.Common
{
	/// <summary>
	/// This defines an interface which can be implemented by UI elements
	/// which indicate the progress of a long operation.
	/// </summary>
	public interface IWaitProgressMeter
	{
		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress callback.
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		void Begin( int minimum, int maximum );
		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress callback, without setting the range
		/// </summary>
		void Begin();
		/// <summary>
		/// Call this method from the worker thread to reset the range in the progress callback
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		void SetRange( int minimum, int maximum );
		/// <summary>
		/// Call this method from the worker thread to increase the progress counter by a specified value.
		/// </summary>
		/// <param name="val">The amount by which to increment the progress indicator</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		void StepTo( int val );
		/// <summary>
		/// Call this method from the worker thread to step the progress meter to a particular value.
		/// </summary>
		/// <param name="val">The value to which to step the meter</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		void Increment( int val );
		/// <summary>
		/// Call this method from the worker thread to finalize the progress meter
		/// </summary>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		void End();
	}
}
