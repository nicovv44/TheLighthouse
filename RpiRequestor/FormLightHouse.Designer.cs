namespace RpiRequestor
{
    partial class FormLightHouse
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLightHouse));
            this.panelColourSign = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelColourSign
            // 
            this.panelColourSign.Location = new System.Drawing.Point(12, 12);
            this.panelColourSign.Name = "panelColourSign";
            this.panelColourSign.Size = new System.Drawing.Size(265, 174);
            this.panelColourSign.TabIndex = 0;
            // 
            // FormLightHouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 201);
            this.Controls.Add(this.panelColourSign);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLightHouse";
            this.Text = "LightHouse";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelColourSign;
    }
}

