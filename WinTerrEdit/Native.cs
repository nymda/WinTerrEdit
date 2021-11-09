using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WinTerrEdit {
    static class Native {
        static class MessageBoxInterface {
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

            public static int Show(string text, string caption, int style) =>
                MessageBox(IntPtr.Zero, text, caption, (uint)style);

            public const int MB_YESNO = 4;
            public const int MB_YESNOCANCEL = 3;

            public const int MB_ICONSTOP = 10;
            public const int MB_ICONERROR = 10;
            public const int MB_ICONHAND = 10;
            public const int MB_ICONQUESTION = 20;
            public const int MB_ICONWARNING = 30;
            public const int MB_ICONEXCLAMATION = 30;
            public const int MB_ICONINFORMATION = 40;
            public const int MB_ICONASTERISK = 40;
        }
    }
}
