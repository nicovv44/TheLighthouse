using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TheLighthouse
{
    public partial class FormGetRpiUri : Form
    {
        public string Uri { get; set; }
        public FormGetRpiUri(string defaultUri)
        {
            InitializeComponent();
            textBoxUri.Text = defaultUri;
        }

        private void TextBoxUri_TextChanged(object sender, EventArgs e)
        {
            Uri = (sender as TextBox).Text;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
