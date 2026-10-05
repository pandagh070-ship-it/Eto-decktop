using System;
using System.Windows.Forms;

namespace ETODesktop
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (Array.Exists(args, x => string.Equals(x, "--restore", StringComparison.OrdinalIgnoreCase)))
            {
                NativeMethods.ShowTaskbar();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
