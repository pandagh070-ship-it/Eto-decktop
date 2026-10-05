using System;
using System.Runtime.InteropServices;

namespace ETODesktop
{
    internal static class NativeMethods
    {
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        internal static void HideTaskbar()
        {
            IntPtr h = FindWindow("Shell_TrayWnd", null);
            if (h != IntPtr.Zero) ShowWindow(h, SW_HIDE);
        }

        internal static void ShowTaskbar()
        {
            IntPtr h = FindWindow("Shell_TrayWnd", null);
            if (h != IntPtr.Zero) ShowWindow(h, SW_SHOW);
        }

        internal static void SetWallpaperBehindIcons(IntPtr hwnd)
        {
            IntPtr progman = FindWindow("Progman", null);
            if (progman == IntPtr.Zero) return;
            const uint WM = 0x052C;
            SendMessage(progman, WM, new IntPtr(0xD), IntPtr.Zero);
            SendMessage(progman, WM, new IntPtr(0xD), new IntPtr(1));
            SendMessage(progman, WM, new IntPtr(0xD), new IntPtr(2));
            SetParent(hwnd, progman);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
}
