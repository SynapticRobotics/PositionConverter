namespace PositionConverter.Forms
{
    partial class RobotInfoForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("RobotType");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("UserTools");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("UserFrames");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobotInfoForm));
            this.RobotInfoTreeView = new System.Windows.Forms.TreeView();
            this.RobotInfoContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RobotInfoContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // RobotInfoTreeView
            // 
            this.RobotInfoTreeView.Location = new System.Drawing.Point(12, 12);
            this.RobotInfoTreeView.Name = "RobotInfoTreeView";
            treeNode1.Name = "RobotType";
            treeNode1.Text = "RobotType";
            treeNode2.Name = "UserTools";
            treeNode2.Text = "UserTools";
            treeNode3.Name = "UserFrames";
            treeNode3.Text = "UserFrames";
            this.RobotInfoTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.RobotInfoTreeView.ShowNodeToolTips = true;
            this.RobotInfoTreeView.Size = new System.Drawing.Size(254, 425);
            this.RobotInfoTreeView.TabIndex = 0;
            this.RobotInfoTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RobotInfoTreeView_AfterSelect);
            // 
            // RobotInfoContextMenuStrip
            // 
            this.RobotInfoContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.RobotInfoContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.RobotInfoContextMenuStrip.Name = "RobotInfoContextMenuStrip";
            this.RobotInfoContextMenuStrip.Size = new System.Drawing.Size(127, 34);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(126, 30);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // RobotInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 444);
            this.Controls.Add(this.RobotInfoTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RobotInfoForm";
            this.Text = "Robot Info";
            this.Load += new System.EventHandler(this.RobotInfoForm_Load);
            this.RobotInfoContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView RobotInfoTreeView;
        private System.Windows.Forms.ContextMenuStrip RobotInfoContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}