namespace PositionConverter.Forms
{
    partial class RobotDHParamsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobotDHParamsForm));
            this.dhPictureBox = new System.Windows.Forms.PictureBox();
            this.j1LinkAnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.j2LinkAnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.j3LinkAnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.j4LinkDnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.facePlateThicknessNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.setDHButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dhPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.j1LinkAnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.j2LinkAnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.j3LinkAnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.j4LinkDnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.facePlateThicknessNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // dhPictureBox
            // 
            this.dhPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("dhPictureBox.Image")));
            this.dhPictureBox.Location = new System.Drawing.Point(12, 12);
            this.dhPictureBox.Name = "dhPictureBox";
            this.dhPictureBox.Size = new System.Drawing.Size(1200, 900);
            this.dhPictureBox.TabIndex = 0;
            this.dhPictureBox.TabStop = false;
            // 
            // j1LinkAnumericUpDown
            // 
            this.j1LinkAnumericUpDown.DecimalPlaces = 1;
            this.j1LinkAnumericUpDown.Location = new System.Drawing.Point(230, 533);
            this.j1LinkAnumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.j1LinkAnumericUpDown.Name = "j1LinkAnumericUpDown";
            this.j1LinkAnumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.j1LinkAnumericUpDown.TabIndex = 1;
            // 
            // j2LinkAnumericUpDown
            // 
            this.j2LinkAnumericUpDown.DecimalPlaces = 1;
            this.j2LinkAnumericUpDown.Location = new System.Drawing.Point(553, 468);
            this.j2LinkAnumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.j2LinkAnumericUpDown.Name = "j2LinkAnumericUpDown";
            this.j2LinkAnumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.j2LinkAnumericUpDown.TabIndex = 2;
            // 
            // j3LinkAnumericUpDown
            // 
            this.j3LinkAnumericUpDown.DecimalPlaces = 1;
            this.j3LinkAnumericUpDown.Location = new System.Drawing.Point(113, 243);
            this.j3LinkAnumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.j3LinkAnumericUpDown.Name = "j3LinkAnumericUpDown";
            this.j3LinkAnumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.j3LinkAnumericUpDown.TabIndex = 3;
            // 
            // j4LinkDnumericUpDown
            // 
            this.j4LinkDnumericUpDown.DecimalPlaces = 1;
            this.j4LinkDnumericUpDown.Location = new System.Drawing.Point(513, 34);
            this.j4LinkDnumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.j4LinkDnumericUpDown.Name = "j4LinkDnumericUpDown";
            this.j4LinkDnumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.j4LinkDnumericUpDown.TabIndex = 4;
            // 
            // facePlateThicknessNumericUpDown
            // 
            this.facePlateThicknessNumericUpDown.DecimalPlaces = 1;
            this.facePlateThicknessNumericUpDown.Location = new System.Drawing.Point(763, 34);
            this.facePlateThicknessNumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.facePlateThicknessNumericUpDown.Name = "facePlateThicknessNumericUpDown";
            this.facePlateThicknessNumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.facePlateThicknessNumericUpDown.TabIndex = 5;
            // 
            // setDHButton
            // 
            this.setDHButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setDHButton.Location = new System.Drawing.Point(930, 852);
            this.setDHButton.Name = "setDHButton";
            this.setDHButton.Size = new System.Drawing.Size(261, 48);
            this.setDHButton.TabIndex = 6;
            this.setDHButton.Text = "Set DH Parameters";
            this.setDHButton.UseVisualStyleBackColor = true;
            this.setDHButton.Click += new System.EventHandler(this.setDHButton_Click);
            // 
            // RobotDHParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1218, 925);
            this.Controls.Add(this.setDHButton);
            this.Controls.Add(this.facePlateThicknessNumericUpDown);
            this.Controls.Add(this.j4LinkDnumericUpDown);
            this.Controls.Add(this.j3LinkAnumericUpDown);
            this.Controls.Add(this.j2LinkAnumericUpDown);
            this.Controls.Add(this.j1LinkAnumericUpDown);
            this.Controls.Add(this.dhPictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RobotDHParamsForm";
            this.Text = "Robot DH Parameters";
            this.Load += new System.EventHandler(this.RobotDHParamsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dhPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.j1LinkAnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.j2LinkAnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.j3LinkAnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.j4LinkDnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.facePlateThicknessNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox dhPictureBox;
        private System.Windows.Forms.NumericUpDown j1LinkAnumericUpDown;
        private System.Windows.Forms.NumericUpDown j2LinkAnumericUpDown;
        private System.Windows.Forms.NumericUpDown j3LinkAnumericUpDown;
        private System.Windows.Forms.NumericUpDown j4LinkDnumericUpDown;
        private System.Windows.Forms.NumericUpDown facePlateThicknessNumericUpDown;
        private System.Windows.Forms.Button setDHButton;
    }
}