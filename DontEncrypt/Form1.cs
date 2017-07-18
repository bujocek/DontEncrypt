using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

namespace NCRWindowHelper
{
    public partial class Form1 : Form
    {
        private const int WM_CLOSE = 16;
        private const int BN_CLICKED = 245;
        private System.Windows.Forms.Button btnCloseCalc;

        /// <summary>
        /// The FindWindow API
        /// </summary>
        /// <param name="lpClassName">the class name for the window to search for</param>
        /// <param name="lpWindowName">the name of the window to search for</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        /// <summary>
        /// The SendMessage API
        /// </summary>
        /// <param name="hWnd">handle to the required window</param>
        /// <param name="msg">the system/Custom message to send</param>
        /// <param name="wParam">first message parameter</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

        /// <summary>
        /// The FindWindowEx API
        /// </summary>
        /// <param name="parentHandle">a handle to the parent window </param>
        /// <param name="childAfter">a handle to the child window to start search after</param>
        /// <param name="className">the class name for the window to search for</param>
        /// <param name="windowTitle">the name of the window to search for</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public Form1()
        {
            //InitializeComponent();
            MainLoop();
        }

        private void MainLoop()
        {
            while(true)
            {
                checkForMcAfee();
                checkForNCRSecurityMessage();
                checkForRestartAfterUpdateDialog();
                //checkForUnspecifiedError();
                System.Threading.Thread.Sleep(500);
            }
        }

        private void checkForUnspecifiedError()
        {
            int hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            hwnd = FindWindow("#32770", "Microsoft Visual Studio");
            while (hwnd != 0)
            {
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "OK");
                if (hwndChild != IntPtr.Zero)
                    SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);
                hwnd = FindWindowEx(IntPtr.Zero, (IntPtr)hwnd, "#32770", "Microsoft Visual Studio").ToInt32();
            }
        }

        private void checkForMcAfee()
        {
            int hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            hwnd = FindWindow("#32770", "McAfee Endpoint Encryption");
            if (hwnd == 0)
                hwnd = FindWindow("#32770", "McAfee File and Removable Media Protection");
            if (hwnd != 0)
            {
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "&No");
                if (hwndChild != IntPtr.Zero)
                    SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);
            }
        }

        private void checkForNCRSecurityMessage()
        {
            int hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            hwnd = FindWindow("#32770", "NCR Security Message");
            if (hwnd == 0)
                hwnd = FindWindow("#32770", "NCR USB Usage Warning");
            if (hwnd != 0)
            {
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "OK");
                if (hwndChild != IntPtr.Zero)
                    SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);
            }
        }

        private void checkForRestartAfterUpdateDialog()
        {
            int hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            IntPtr hwndButton = IntPtr.Zero;
            IntPtr hwndSomethingOverButton = IntPtr.Zero;
            hwnd = FindWindow("#32770", "Windows Update");
            if (hwnd != 0)
            {
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "DirectUIHWND", null);
                if (hwndChild != IntPtr.Zero)
                {
                    hwndSomethingOverButton = FindWindowEx((IntPtr)hwndChild, IntPtr.Zero, "CtrlNotifySink", null);
                    while (hwndSomethingOverButton != IntPtr.Zero)
                    {
                        hwndButton = FindWindowEx((IntPtr)hwndSomethingOverButton, IntPtr.Zero, "Button", "&Postpone");
                        if (hwndButton != IntPtr.Zero)
                        { 
                            SendMessage((int)hwndButton, BN_CLICKED, 0, IntPtr.Zero);
                            break;
                        }
                        hwndSomethingOverButton = FindWindowEx((IntPtr)hwndChild, hwndSomethingOverButton, "CtrlNotifySink", null);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
