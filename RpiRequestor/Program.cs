using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RpiRequestor
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string defaultRaspberryPiUri = "http://192.168.45.118";
            string uri = GetUri(defaultRaspberryPiUri);
            FormColorFlag form1 = new FormColorFlag(uri) { TopMost = true };
            Application.Run(form1);
        }

        static string GetUri(string defaultUri)
        {
            using var form = new FormGetRpiUri(defaultUri);
            var result = form.ShowDialog();
            return form.Uri;
        }
    }
}
