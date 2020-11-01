using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheLighthouse
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
            string uri = GetUri();
            FormLightHouse form1 = new FormLightHouse(uri);
            Application.Run(form1);
        }

        /// <summary>
        /// Ask the user to give the URI of the Raspberry Pi, with a form
        /// </summary>
        /// <returns></returns>
        static string GetUri()
        {
            string defaultUri = Settings1.Default.Uri;
            if (defaultUri.Length == 0) 
            {
                defaultUri = Constant.DefaultUri;
                Settings1.Default.Uri = Constant.DefaultUri;
                Settings1.Default.Save();
            }
            using var form = new FormGetRpiUri(defaultUri);
            var result = form.ShowDialog();
            return form.Uri;
        }
    }
}
