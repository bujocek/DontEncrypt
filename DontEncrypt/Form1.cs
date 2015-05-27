using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

namespace DontEncrypt
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
            int hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            while(true)
            {
                hwnd = FindWindow("#32770", "McAfee Endpoint Encryption");
                if (hwnd != 0)
                {
                    //Get a handle for the "No" button
                    hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "&No");

                    //send BN_CLICKED message
                    if (hwndChild != IntPtr.Zero)
                        SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
