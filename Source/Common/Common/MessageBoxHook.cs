#pragma warning disable 0618
using System;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, UnmanagedCode = true)]
namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
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
    ///			<item name="vssfile">$Workfile: MessageBoxHook.cs $</item>
    ///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/MessageBoxHook.cs $</item>
    ///			<item name="vssrevision">$Revision: 1 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 1/25/10 11:35a $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public partial class MessageBoxHook
    {
        #region Constants
        private const int WM_CLOSE = 0x10;
        private const int WH_CALLWNDPROCRET = 12;
        private const int WM_DESTROY = 0x0002;
        private const int WM_INITDIALOG = 0x0110;
        private const int WM_TIMER = 0x0113;
        private const int WM_USER = 0x400;
        private const int DM_GETDEFID = WM_USER + 0;

        private const int WM_SETTEXT = 0x0C;
        private const int WM_GETTEXTLENGTH = 0x000E;
        private const int WM_GETTEXT = 0x0D;
        #endregion

        #region Member Data
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int msg, int wParam, StringBuilder sb);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLengthW", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        [DllImport("user32.dll")]
        private static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetDlgCtrlID(IntPtr hwndCtl);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
        private static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", EntryPoint = "FindWindowExA")]
        static extern IntPtr FindWindowEx(IntPtr hwndParent,
            IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        static extern void MessageBeep(int type);

        [DllImport("kernel32.dll")]
        static extern int Beep(uint freq, uint duration);

        [StructLayout(LayoutKind.Sequential)]
        private struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };

        private static HookProc hookProc;
        [ThreadStatic]
        private static IntPtr hHook;
        #endregion

        #region Accessors
        #endregion

        #region Delegates and Events
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);
        #endregion

        #region Constructors and Destructor
        /// <summary>Initializes a new instance of the MessageBoxHook class.</summary>
		static MessageBoxHook()
		{
			hookProc = new HookProc(MessageBoxHookProc);
			hHook = IntPtr.Zero;
		}

        #endregion

        #region Methods
        /// <summary>
        /// Enables MessageBoxUtil functionality
        /// </summary>
        /// <remarks>
        /// MessageBoxManager functionality is enabled on current thread only.
        /// Each thread that needs MessageBoxUtil functionality has to call this method.
        /// </remarks>
        public static void Register()
        {
            if (hHook != IntPtr.Zero)
                throw new NotSupportedException("One hook per thread allowed.");
            hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
            Beep(100, 100);
        }

        /// <summary>
        /// Disables MessageBoxUtil functionality
        /// </summary>
        /// <remarks>
        /// Disables MessageBoxUtil functionality on current thread only.
        /// </remarks>
        public static void Unregister()
        {
            if (hHook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
            }
        }
        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool hideWindows = true;

            if (nCode < 0)
                return CallNextHookEx(hHook, nCode, wParam, lParam);

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = hHook;

            if (msg.message == WM_INITDIALOG)
            {
                int nLength = GetWindowTextLength(msg.hwnd);
                StringBuilder className = new StringBuilder(10);
                GetClassName(msg.hwnd, className, className.Capacity);
                if (className.ToString() == "#32770")//window class name for dialogs
                {
                    if (hideWindows)
                    {
                        System.Diagnostics.Debug.WriteLine("window class #32770 not raised");

                        // get window text of dialog
                        StringBuilder title = new StringBuilder();
                        int i = GetWindowText(msg.hwnd, title, nLength + 1);
                        System.Diagnostics.Debug.WriteLine(title);

                        // get message text.
                        IntPtr txtHandle = FindWindowEx(msg.hwnd, IntPtr.Zero, "Static", null);
                        if (txtHandle != IntPtr.Zero)
                        {
                            nLength = GetWindowTextLength(txtHandle);
                            StringBuilder text = new StringBuilder(nLength + 1);
                            i = GetWindowText(txtHandle, text, nLength + 1);
                            System.Diagnostics.Debug.WriteLine(text);
                        }

                        // close window
                        SendMessage(msg.hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("window raised");
                    }
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }
        #endregion

        #region Event Handlers
        #endregion
    }
}
