namespace PositionConverter
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.button1 = new System.Windows.Forms.Button();
            this.SynapticRoboticsPictureBox = new System.Windows.Forms.PictureBox();
            this.infoText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SynapticRoboticsPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 282);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SynapticRoboticsPictureBox
            // 
            this.SynapticRoboticsPictureBox.BackColor = System.Drawing.Color.White;
            this.SynapticRoboticsPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("SynapticRoboticsPictureBox.Image")));
            this.SynapticRoboticsPictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("SynapticRoboticsPictureBox.InitialImage")));
            this.SynapticRoboticsPictureBox.Location = new System.Drawing.Point(8, 8);
            this.SynapticRoboticsPictureBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SynapticRoboticsPictureBox.Name = "SynapticRoboticsPictureBox";
            this.SynapticRoboticsPictureBox.Size = new System.Drawing.Size(215, 107);
            this.SynapticRoboticsPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SynapticRoboticsPictureBox.TabIndex = 1;
            this.SynapticRoboticsPictureBox.TabStop = false;
            this.SynapticRoboticsPictureBox.Click += new System.EventHandler(this.SynapticRoboticsPictureBox_Click);
            // 
            // infoText
            // 
            this.infoText.AutoSize = true;
            this.infoText.Location = new System.Drawing.Point(8, 125);
            this.infoText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(220, 117);
            this.infoText.TabIndex = 2;
            this.infoText.Text = resources.GetString("infoText.Text");
            this.infoText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.infoText.Click += new System.EventHandler(this.label1_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(231, 312);
            this.Controls.Add(this.infoText);
            this.Controls.Add(this.SynapticRoboticsPictureBox);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SynapticRoboticsPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox SynapticRoboticsPictureBox;
        private System.Windows.Forms.Label infoText;
    }
}