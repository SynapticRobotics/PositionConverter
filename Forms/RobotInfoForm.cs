using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FanucUtilities;

namespace PositionConverter.Forms
{
    public partial class RobotInfoForm : Form
    {

        public RobotInfoForm(FanucRobot fanucRobot)
        {
            InitializeComponent();

            RobotInfoTreeView.Nodes[0].Nodes.Add("GP1: " + fanucRobot.robotArmType);

            for (int i = 0; i < fanucRobot.numOfUTools; i++)
            {
                RobotInfoTreeView.Nodes[1].Nodes.Add($"UTOOL {i}");
                RobotInfoTreeView.Nodes[1].Nodes[i].ToolTipText = fanucRobot.GetUtool(i);
                RobotInfoTreeView.Nodes[1].Nodes[i].ContextMenuStrip = RobotInfoContextMenuStrip;
            }

            for (int i = 0; i < fanucRobot.numOfUFrames; i++)
            {
                RobotInfoTreeView.Nodes[2].Nodes.Add($"UFRAME {i}");
                RobotInfoTreeView.Nodes[2].Nodes[i].ToolTipText = fanucRobot.GetUframe(i);
                RobotInfoTreeView.Nodes[2].Nodes[i].ContextMenuStrip = RobotInfoContextMenuStrip;
            }
        }

        private void RobotInfoForm_Load(object sender, EventArgs e)
        {
            
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(RobotInfoTreeView.SelectedNode.ToolTipText);
        }

        private void RobotInfoTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
