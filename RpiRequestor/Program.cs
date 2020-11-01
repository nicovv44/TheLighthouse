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
            if(!GetUri(out string uri)) return;
            FormLightHouse form1 = new FormLightHouse(uri);
            Application.Run(form1);
        }

        /// <summary>
        /// Ask the user to give the URI of the Raspberry Pi, with a form
        /// </summary>
        /// <param name="uri">OUTPUT: the URI of the Raspbarry Pi if the function returned <c>true</c>, empty string otherwise</param>
        /// <returns><code>true</code> if successfully recovered URI from user, <code>false</code>otherwise</returns>
        static bool GetUri(out string uri)
        {
            uri = "";
            string defaultUri = Settings1.Default.Uri;
            if (defaultUri.Length == 0) 
            {
                defaultUri = Constant.DefaultUri;
                Settings1.Default.Uri = Constant.DefaultUri;
                Settings1.Default.Save();
            }
            using var form = new FormGetRpiUri(defaultUri);
            var result = form.ShowDialog();
            if (result != DialogResult.OK) return false;
            uri = form.Uri;
            return true;
        }
    }
}
