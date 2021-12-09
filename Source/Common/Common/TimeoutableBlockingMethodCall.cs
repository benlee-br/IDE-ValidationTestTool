using System;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Timeoutable blocking method call.
    /// </summary>
    /// <remarks>
    /// You have one or more API methods that make calls to external objects which, 
    /// for one reason or another, are beyond your control. 
    /// These are blocking calls. What happens when the method is not operational, 
    /// in a "hung" state, or whatever? You have made a blocking method call that never returns, 
    /// and this leaves you in a sorry state indeed. 
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:Ralph Ansell</item>
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
    ///			<item name="vssfile">$Workfile: TimeoutableBlockingMethodCall.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/TimeoutableBlockingMethodCall.cs $</item>
    ///			<item name="vssrevision">$Revision: 6 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 4/19/07 2:52p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class TimeoutableBlockingMethodCall
    {
        #region Member Data
        private long m_Timeused;
        private long m_HasTimeout;
        private int m_ReturnParameter;
        private PerfTimer m_PerfTimer;
        private string m_ExceptionMessage;
        private string m_MethodName;
        private MethodDelegate m_Method;
        #endregion

        #region Delegates
        /// <summary>
        /// Delegate for 
        /// </summary>
        /// <param name="inParams">Array of input parameters.</param>
        /// <param name="outParams">Array of output parameters.</param>
        /// <returns></returns>
        public delegate int MethodDelegate(object[] inParams, out object[] outParams);
        #endregion

        #region Accessor
        /// <summary>Get method name.</summary>
        public string MethodName
        {
            get { return m_MethodName; }
        }
        /// <summary>Get exception message.</summary>
        public string ExceptionMessage
        {
            get { return m_ExceptionMessage; }
        }
        /// <summary>Get time used to execute method.</summary>
        public long TimeUsed
        {
            get { return Interlocked.Read(ref m_Timeused); }
            set { Interlocked.Exchange(ref m_Timeused, value); }
        }
        /// <summary>Get return code. Returns zero if successful.</summary>
        public int ReturnParameter
        {
            get { return m_ReturnParameter; }
            set { m_ReturnParameter = value; }
        }
        /// <summary>Has method time out?</summary>
        public bool HasTimeout
        {
            get { return (Interlocked.Read(ref m_HasTimeout) == 1) ? true : false; }
            set { Interlocked.Exchange(ref m_HasTimeout, value ? 1 : 0); }
        }
        #endregion

        #region Constructors and Destructor
        /// <summary></summary>
        public TimeoutableBlockingMethodCall()
        {
            m_ExceptionMessage = string.Empty;
            m_Timeused = 0;
            m_HasTimeout = 0;
            m_ReturnParameter = 0;
            m_PerfTimer = new PerfTimer();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public TimeoutableBlockingMethodCall(MethodDelegate method)
        {
            m_Method = method;
            m_ExceptionMessage = string.Empty;
            m_Timeused = 0;
            m_HasTimeout = 0;
            m_ReturnParameter = 0;
            m_PerfTimer = new PerfTimer();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="inParams"></param>
        /// <param name="outParams"></param>
        /// <returns></returns>
        public int Invoke(
           int timeout,
           ref object[] inParams,
           out object[] outParams)
        {
            return Invoke(timeout, this.m_Method, ref inParams, out outParams);
        }
        /// <summary>
        /// Timeout-able blocking method call.
        /// </summary>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <param name="method">Reference to the MethodDelegate object.</param>
        /// <param name="inParams">Array of input parameters.</param>
        /// <param name="outParams">Array of output parameters.</param>
        /// <returns>Zero if successful.</returns>
        public int Invoke(
            int timeout,
            MethodDelegate method,
            ref object[] inParams,
            out object[] outParams)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            HasTimeout = false;
            TimeUsed = 0;
            ReturnParameter = -3;
            outParams = new object[0];
            m_ExceptionMessage = string.Empty;

            try
            {
                m_MethodName = method.Method.Name;
                m_PerfTimer.Start();//start timer.

                // Invoke method asychronously.
                IAsyncResult ar = method.BeginInvoke(
                        inParams,    //input parameters
                        out outParams,  //output parameters
                        null,//Callback method, last two arguments should be null.
                        null);

                // Wait for method to complete or timeout.
                if ( ar.AsyncWaitHandle.WaitOne(timeout, false) )
                {
                    // Get the return value generated by the asynchronous operation.
                    ReturnParameter = method.EndInvoke(out outParams, ar);
                }
                else//method timeout
                {
                    m_ExceptionMessage = "";
                    HasTimeout = true;
                    ReturnParameter = -2;//todo: error code for timeouts?
                }
            }
            catch (Exception ex)
            {
                m_ExceptionMessage = ex.Message;
                ReturnParameter = -1;//todo: error code for exceptions?
            }
            finally
            {
                if (m_PerfTimer != null)
                {
                    TimeUsed =
                        (int)m_PerfTimer.Stop().GetAs(TimeType.Units.MilliSeconds);
                }
            }

            return ReturnParameter;
        }
        #endregion
    }
}
