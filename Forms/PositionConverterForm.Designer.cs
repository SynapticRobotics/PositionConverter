namespace PositionConverter
{
    partial class PositionConverterForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PositionConverterForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.robotInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.RobotProgramTreeView = new System.Windows.Forms.TreeView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SwapConvertRadioButton = new System.Windows.Forms.RadioButton();
            this.ConversionModeGroupBox = new System.Windows.Forms.GroupBox();
            this.JPosConvertRadioButton = new System.Windows.Forms.RadioButton();
            this.LposConvertRadioButton = new System.Windows.Forms.RadioButton();
            this.ConversionExeButton = new System.Windows.Forms.Button();
            this.RobotProgramContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.robotDHParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ConversionModeGroupBox.SuspendLayout();
            this.RobotProgramContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(219, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.robotInfoToolStripMenuItem,
            this.robotDHParametersToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // robotInfoToolStripMenuItem
            // 
            this.robotInfoToolStripMenuItem.Enabled = false;
            this.robotInfoToolStripMenuItem.Name = "robotInfoToolStripMenuItem";
            this.robotInfoToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.robotInfoToolStripMenuItem.Text = "Robot Info";
            this.robotInfoToolStripMenuItem.Click += new System.EventHandler(this.robotInfoToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howToUseToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.howToUseToolStripMenuItem.Text = "How to Use";
            this.howToUseToolStripMenuItem.Click += new System.EventHandler(this.howToUseToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(132, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Robot Backup|backdate.dt";
            this.openFileDialog1.Title = "Please select a robot backup";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 608);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(219, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(207, 17);
            this.toolStripStatusLabel1.Text = "Please open a full MD: robot backup...";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(67, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBar1.Visible = false;
            // 
            // RobotProgramTreeView
            // 
            this.RobotProgramTreeView.CheckBoxes = true;
            this.RobotProgramTreeView.Location = new System.Drawing.Point(8, 29);
            this.RobotProgramTreeView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RobotProgramTreeView.MaximumSize = new System.Drawing.Size(201, 446);
            this.RobotProgramTreeView.MinimumSize = new System.Drawing.Size(201, 446);
            this.RobotProgramTreeView.Name = "RobotProgramTreeView";
            this.RobotProgramTreeView.ShowNodeToolTips = true;
            this.RobotProgramTreeView.Size = new System.Drawing.Size(201, 446);
            this.RobotProgramTreeView.TabIndex = 4;
            this.RobotProgramTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RobotProgramTreeView_AfterSelect);
            // 
            // SwapConvertRadioButton
            // 
            this.SwapConvertRadioButton.AutoSize = true;
            this.SwapConvertRadioButton.Location = new System.Drawing.Point(5, 55);
            this.SwapConvertRadioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SwapConvertRadioButton.Name = "SwapConvertRadioButton";
            this.SwapConvertRadioButton.Size = new System.Drawing.Size(189, 17);
            this.SwapConvertRadioButton.TabIndex = 2;
            this.SwapConvertRadioButton.Text = "Swap Representations of Selected";
            this.toolTip1.SetToolTip(this.SwapConvertRadioButton, "Convert XYZWPR points to JPOS, and\r\nJPOS points to XYZWPR.");
            this.SwapConvertRadioButton.UseVisualStyleBackColor = true;
            // 
            // ConversionModeGroupBox
            // 
            this.ConversionModeGroupBox.Controls.Add(this.SwapConvertRadioButton);
            this.ConversionModeGroupBox.Controls.Add(this.JPosConvertRadioButton);
            this.ConversionModeGroupBox.Controls.Add(this.LposConvertRadioButton);
            this.ConversionModeGroupBox.Enabled = false;
            this.ConversionModeGroupBox.Location = new System.Drawing.Point(8, 488);
            this.ConversionModeGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ConversionModeGroupBox.Name = "ConversionModeGroupBox";
            this.ConversionModeGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ConversionModeGroupBox.Size = new System.Drawing.Size(200, 78);
            this.ConversionModeGroupBox.TabIndex = 5;
            this.ConversionModeGroupBox.TabStop = false;
            this.ConversionModeGroupBox.Text = "Conversion Mode";
            // 
            // JPosConvertRadioButton
            // 
            this.JPosConvertRadioButton.AutoSize = true;
            this.JPosConvertRadioButton.ForeColor = System.Drawing.Color.Blue;
            this.JPosConvertRadioButton.Location = new System.Drawing.Point(5, 36);
            this.JPosConvertRadioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.JPosConvertRadioButton.Name = "JPosConvertRadioButton";
            this.JPosConvertRadioButton.Size = new System.Drawing.Size(149, 17);
            this.JPosConvertRadioButton.TabIndex = 1;
            this.JPosConvertRadioButton.Text = "Convert Selected to JPOS";
            this.JPosConvertRadioButton.UseVisualStyleBackColor = true;
            // 
            // LposConvertRadioButton
            // 
            this.LposConvertRadioButton.AutoSize = true;
            this.LposConvertRadioButton.BackColor = System.Drawing.SystemColors.Control;
            this.LposConvertRadioButton.Checked = true;
            this.LposConvertRadioButton.ForeColor = System.Drawing.Color.Green;
            this.LposConvertRadioButton.Location = new System.Drawing.Point(5, 16);
            this.LposConvertRadioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LposConvertRadioButton.Name = "LposConvertRadioButton";
            this.LposConvertRadioButton.Size = new System.Drawing.Size(169, 17);
            this.LposConvertRadioButton.TabIndex = 0;
            this.LposConvertRadioButton.TabStop = true;
            this.LposConvertRadioButton.Text = "Convert Selected to XYZWPR";
            this.LposConvertRadioButton.UseVisualStyleBackColor = false;
            // 
            // ConversionExeButton
            // 
            this.ConversionExeButton.Enabled = false;
            this.ConversionExeButton.Location = new System.Drawing.Point(8, 570);
            this.ConversionExeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ConversionExeButton.Name = "ConversionExeButton";
            this.ConversionExeButton.Size = new System.Drawing.Size(200, 32);
            this.ConversionExeButton.TabIndex = 6;
            this.ConversionExeButton.Text = "Execute Conversion";
            this.ConversionExeButton.UseVisualStyleBackColor = true;
            this.ConversionExeButton.Click += new System.EventHandler(this.ConversionExeButton_Click);
            // 
            // RobotProgramContextMenuStrip
            // 
            this.RobotProgramContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.RobotProgramContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem,
            this.copyToolStripMenuItem});
            this.RobotProgramContextMenuStrip.Name = "RobotProgramContextMenuStrip";
            this.RobotProgramContextMenuStrip.Size = new System.Drawing.Size(124, 48);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "ls";
            this.saveFileDialog1.Title = "Select where to save the converted program.";
            // 
            // robotDHParametersToolStripMenuItem
            // 
            this.robotDHParametersToolStripMenuItem.Enabled = false;
            this.robotDHParametersToolStripMenuItem.Name = "robotDHParametersToolStripMenuItem";
            this.robotDHParametersToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.robotDHParametersToolStripMenuItem.Text = "Robot DH Parameters";
            this.robotDHParametersToolStripMenuItem.Click += new System.EventHandler(this.robotDHParametersToolStripMenuItem_Click);
            // 
            // PositionConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 630);
            this.Controls.Add(this.ConversionExeButton);
            this.Controls.Add(this.ConversionModeGroupBox);
            this.Controls.Add(this.RobotProgramTreeView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PositionConverterForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Position Converter";
            this.Load += new System.EventHandler(this.BoundaryChecekerForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ConversionModeGroupBox.ResumeLayout(false);
            this.ConversionModeGroupBox.PerformLayout();
            this.RobotProgramContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TreeView RobotProgramTreeView;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox ConversionModeGroupBox;
        private System.Windows.Forms.RadioButton SwapConvertRadioButton;
        private System.Windows.Forms.RadioButton JPosConvertRadioButton;
        private System.Windows.Forms.RadioButton LposConvertRadioButton;
        private System.Windows.Forms.Button ConversionExeButton;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem robotInfoToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip RobotProgramContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem howToUseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem robotDHParametersToolStripMenuItem;
    }
}

