using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Controles
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        // ********************************************* SendMessage

        #region SendMessage

        // DLL Import for CustomSliders

        internal const int TBM_GETTHUMBRECT = 0x419;
        //private const int TBM_GETCHANNELRECT = 0x41A;

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT { public int left, top, right, bottom; }

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, ref RECT lp);

        #endregion

        // ********************************************* ShellExecuteEx - For File Properties Info Dialog

        #region ShellExecuteEx

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        internal static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        internal const int SW_SHOW = 5;
        internal const uint SEE_MASK_INVOKEIDLIST = 12;

        #endregion

        // ********************************************* BitBlt

        #region BitBlt

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32.dll")]
        internal static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest,
            IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, uint dwRop);

        [DllImport("gdi32.dll")]
        internal static extern int SetStretchBltMode(IntPtr hdc, int iStretchMode);

        [DllImport("Msimg32.dll")]
        internal static extern bool TransparentBlt(IntPtr hdcDest, // handle to destination DC
        int nXOriginDest, // x-coord of destination upper-left corner
        int nYOriginDest, // y-coord of destination upper-left corner
        int nWidthDest, // width of destination rectangle
        int hHeightDest, // height of destination rectangle
        IntPtr hdcSrc, // handle to source DC
        int nXOriginSrc, // x-coord of source upper-left corner
        int nYOriginSrc, // y-coord of source upper-left corner
        int nWidthSrc, // width of source rectangle
        int nHeightSrc, // height of source rectangle
        int crTransparent // color to make transparent
        );

        #endregion

        // ********************************************* SelectObject / DeleteObject

        #region SelectObject / DeleteObject

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        #endregion

        // ********************************************* AddFontMemResourceEx

        #region AddFontMemResourceEx

        [DllImport("gdi32.dll")]
        internal static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        #endregion

        // ********************************************* Filename Compare (Sort like Explorer)

        #region Filename Compare

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern int StrCmpLogicalW(String x, String y);

        #endregion

        // ********************************************* Controls with Rounded Corners

        #region Controls with Rounded Corners

        [DllImport("Gdi32.dll")] // EntryPoint = "CreateRoundRectRgn")]
        internal static extern IntPtr CreateRoundRectRgn
            (
                int nLeftRect,      // x-coordinate of upper-left corner
                int nTopRect,       // y-coordinate of upper-left corner
                int nRightRect,     // x-coordinate of lower-right corner
                int nBottomRect,    // y-coordinate of lower-right corner
                int nWidthEllipse,  // height of ellipse
                int nHeightEllipse  // width of ellipse
             );

        // See above SelectObject / DeleteObject
        //[DllImport("gdi32.dll")]
        //internal static extern bool DeleteObject(IntPtr hObject);

        #endregion


        // ********************************************* Switch Off Window Animation

        #region Switch Off Window Animation

        internal const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        internal static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, bool attrValue, int attrSize);

        #endregion
    }
}
