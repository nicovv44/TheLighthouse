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
            string uri = GetUri(TheLighthouse.Properties.Resources.Uri);
            FormLightHouse form1 = new FormLightHouse(uri) { TopMost = true };
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
