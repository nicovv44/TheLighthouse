using System;
using System.Windows.Forms;

namespace TheLighthouse
{
    /// <summary>
    /// The class of a form used to ask a URI to the user
    /// </summary>
    public partial class FormGetRpiUri : Form
    {
        /// <summary>
        /// The URI property used as form output
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Constructor of the class <see cref="FormGetRpiUri"/> 
        /// </summary>
        /// <param name="defaultUri">The default URI to display in the texbox</param>
        public FormGetRpiUri(string defaultUri)
        {
            // Initialize GUI
            InitializeComponent();
            textBoxUri.Text = defaultUri;
        }

        /// <summary>
        /// The function called when the text in URI textbox is changed
        /// </summary>
        /// <param name="sender">The <see cref="TextBox"/> sending the event</param>
        /// <param name="e">The data from the textbox</param>
        private void TextBoxUri_TextChanged(object sender, EventArgs e)
        {
            Uri = (sender as TextBox).Text;
            Settings1.Default.Uri = (sender as TextBox).Text;
            Settings1.Default.Save();
        }

        /// <summary>
        /// The function called when the OK button is clicked: it closes the form
        /// </summary>
        /// <param name="sender">The <see cref="Button"/> sending the event</param>
        /// <param name="e">The data from the button</param>
        private void ButtonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
