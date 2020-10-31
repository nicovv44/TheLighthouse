using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RpiRequestor
{
    public partial class FormColorFlag : Form
    {
        private readonly Requestor Requestor;
        public FormColorFlag(string uri)
        {
            // Initialise non GUI
            Requestor = new Requestor(uri, UpdateGuiWithNewResult);

            // Initialize GUI
            InitializeComponent();

            //Start actions
            Requestor.StartRequesting();
        }

        void UpdateGuiWithNewResult()
        {
            Trace.WriteLine($"Result: {Requestor.Result}");
            string result = Requestor.Result;
            switch (result)
            {
                case "{\"Button\": 0}":
                    panelColourSign.BackColor = Color.Green;
                    break;
                case "{\"Button\": 1}":
                    panelColourSign.BackColor = Color.Red;
                    break;
            }
        }

    }
}
