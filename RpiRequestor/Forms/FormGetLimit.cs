using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TheLighthouse.Forms
{
    public partial class FormGetLimit : System.Windows.Forms.Form
    {
        /// <summary>
        /// The limit inn meter to use to display alert
        /// </summary>
        public float Limit { get; set; }
        /// <summary>
        /// Tells wether the Limit is correctly parsed
        /// </summary>
        private bool ValidLimit { get; set; }

        /// <summary>
        /// Constructor of the class <see cref="FormGetLimit"/>
        /// </summary>
        public FormGetLimit()
        {
            InitializeComponent();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (ValidLimit) Close();
        }

        private void TextBoxLimit_TextChanged(object sender, EventArgs e)
        {
            ValidLimit = float.TryParse((sender as TextBox).Text, out float limit);
            Limit = limit;
        }
    }
}
