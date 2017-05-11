namespace firstForms
{
    partial class FormSensor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBack = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBack)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBack
            // 
            this.pictureBack.Location = new System.Drawing.Point(46, 0);
            this.pictureBack.Name = "pictureBack";
            this.pictureBack.Size = new System.Drawing.Size(32, 32);
            this.pictureBack.TabIndex = 0;
            this.pictureBack.TabStop = false;
            this.pictureBack.Click += new System.EventHandler(this.pictureBack_Click);
            // 
            // FormSensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(125, 32);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBack);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSensor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormSensor";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.FormSensor_Activated);
            this.Click += new System.EventHandler(this.FormSensor_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormSensor_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBack)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBack;
    }
}