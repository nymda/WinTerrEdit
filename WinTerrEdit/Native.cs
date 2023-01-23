using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinTerrEdit {
    /**<summary>
     * Collection of native interfaces.
     * </summary>
     */
    public static class Native {
        public static class MessageBoxInterface {
            [DllImport("user32.dll")]
            static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

            public static uint Show(string text, string caption, uint style) =>
                MessageBox(IntPtr.Zero, text, caption, (uint)style);

            public const uint MB_OK = 0;
            public const uint MB_OKCANCEL = 1;
            public const uint MB_ABORTRETRYIGNORE = 2;
            public const uint MB_YESNOCANCEL = 3;
            public const uint MB_YESNO = 4;
            public const uint MB_RETRYCANCEL = 5;
            public const uint MB_CANCELTRYCONTINUE = 6;
            public const uint MB_HELP = 4000;

            public const uint MB_ICONSTOP = 10;
            public const uint MB_ICONERROR = 10;
            public const uint MB_ICONHAND = 10;
            public const uint MB_ICONQUESTION = 20;
            public const uint MB_ICONWARNING = 30;
            public const uint MB_ICONEXCLAMATION = 30;
            public const uint MB_ICONINFORMATION = 40;
            public const uint MB_ICONASTERISK = 40;
        }

        /**<summary>
         * Get the last WinApi error.<br/>
         * docs.microsoft.com/en-us/windows/win32/debug/system-error-codes
         * </summary>
         */
        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        public static class UserInterface {
            [DllImport("user32.dll")]
            static extern bool OpenClipboard(IntPtr hWndNewOwner);

            [DllImport("user32.dll")]
            static extern bool CloseClipboard();

            [DllImport("user32.dll")]
            static extern bool SetClipboardData(uint uFormat, IntPtr data);

            public static void SetClipboardText(string txt) {
                OpenClipboard(IntPtr.Zero);
                var ptr = Marshal.StringToHGlobalUni(txt);
                SetClipboardData(13, ptr);
                CloseClipboard();
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
