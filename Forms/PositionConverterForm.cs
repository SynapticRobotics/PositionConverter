using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FanucUtilities;
using PositionConverter.Forms;

namespace PositionConverter
{

    public partial class PositionConverterForm : Form
    {
        private FanucRobot robot = new FanucRobot();

        public PositionConverterForm()
        {
            InitializeComponent();
            RobotProgramTreeView.AfterCheck += RobotProgramTreeView_AfterCheck;
        }

        private void BoundaryChecekerForm_Load(object sender, EventArgs e)
        {

        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog1.FileName != null)
                    {
                        robot.programs.Clear();
                        try
                        {
                            robot.ParseBackup(openFileDialog1.FileName);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Robot model not supported"))
                            {
                                MessageBox.Show("The robot model was not found, or is not programmed internally. " +
                                    "The Robot DH parameters page will now show. Please set the DH Parameters with infomation obtained from the robot's datasheet.");
                                RobotDHParamsForm robotDHParamsForm = new RobotDHParamsForm(robot);
                                robotDHParamsForm.ShowDialog();
                                robot.ParseBackup(openFileDialog1.FileName);
                            }
                            else
                            {
                                throw;
                            }
                        }
                        ConversionModeGroupBox.Enabled = true;
                        ConversionExeButton.Enabled = true;
                        robotInfoToolStripMenuItem.Enabled = true;
                        robotDHParametersToolStripMenuItem.Enabled = true;
                        GenerateRobotProgramTreeView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Parsing Robot Backup. Original error: " + ex.Message);
                    Console.Write(ex.StackTrace.ToString());
                    ConversionModeGroupBox.Enabled = false;
                    ConversionExeButton.Enabled = false;
                    robotInfoToolStripMenuItem.Enabled = false;
                    robotDHParametersToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void GenerateRobotProgramTreeView()
        {
            RobotProgramTreeView.Nodes.Clear();
            int pointNodeIndex = 0;
            /*RobotProgramTreeView.Nodes.Add("[POSITION REGISTERS]");
            foreach (FanucPoint pr in robot.positionRegisters)
            {
                if (pr.Name == "")
                {
                    RobotProgramTreeView.Nodes[0].Nodes.Add("PR[" + pr.Index.ToString() + "]");
                }
                else
                {
                    RobotProgramTreeView.Nodes[0].Nodes.Add("PR[" + pr.Index.ToString() + ":" + pr.Name + "]");
                }
                if (pr.IsCartRep & !pr.IsUnintialized)
                {
                    RobotProgramTreeView.Nodes[0].Nodes[pointNodeIndex].ForeColor = Color.Green;
                }
                else if (pr.IsJointRep & !pr.IsUnintialized)
                {
                    RobotProgramTreeView.Nodes[0].Nodes[pointNodeIndex].ForeColor = Color.Blue;
                }
                RobotProgramTreeView.Nodes[0].Nodes[pointNodeIndex].ToolTipText = pr.ToString();
                RobotProgramTreeView.Nodes[0].Nodes[pointNodeIndex].Tag = pr;
                pointNodeIndex++;
            }*/
            int programsWithMotion = 0;
            //int programNodeIndex = 1; was this when the PRs were being parsed.
            int programNodeIndex = 0;
            foreach (FanucProgram robotprogram in robot.programs)
            {
                if (robotprogram.HasPoints & robotprogram.LocksMotionGroup & !robotprogram.IsLineTrackingProgram)
                {
                    programsWithMotion++;
                    RobotProgramTreeView.Nodes.Add(robotprogram.Name);
                    RobotProgramTreeView.Nodes[programNodeIndex].ToolTipText = "Program Comment = \"" + robotprogram.Comment + "\"";
                    pointNodeIndex = 0;
                    foreach (FanucPoint point in robotprogram.Points)
                    {
                        if (point.Name == "")
                        {
                            RobotProgramTreeView.Nodes[programNodeIndex].Nodes.Add("P[" + point.Index.ToString() + "]");
                        }
                        else
                        {
                            RobotProgramTreeView.Nodes[programNodeIndex].Nodes.Add("P[" + point.Index.ToString() + ":" + point.Name + "]");
                        }
                        if (point.IsCartRep & !point.IsUnintialized)
                        {
                            RobotProgramTreeView.Nodes[programNodeIndex].Nodes[pointNodeIndex].ForeColor = Color.Green;
                        }
                        else if (point.IsJointRep & !point.IsUnintialized)
                        {
                            RobotProgramTreeView.Nodes[programNodeIndex].Nodes[pointNodeIndex].ForeColor = Color.Blue;
                        }
                        RobotProgramTreeView.Nodes[programNodeIndex].Nodes[pointNodeIndex].ToolTipText = point.ToString();
                        RobotProgramTreeView.Nodes[programNodeIndex].Nodes[pointNodeIndex].Tag = point; //set the tag of the tree node to the point, so we can manipulate it later.
                        pointNodeIndex++;
                    }
                    RobotProgramTreeView.Nodes[programNodeIndex].Tag = robotprogram;
                    RobotProgramTreeView.Nodes[programNodeIndex].ContextMenuStrip = RobotProgramContextMenuStrip;
                    programNodeIndex++;
                }
            }
            
            if (programsWithMotion == 0)
            {
                toolStripStatusLabel1.Text = "Found 0 programs with motion. Does your backup have .ls files?";
            }
            else if (programsWithMotion == 1)
            {
                toolStripStatusLabel1.Text = "Found " + programsWithMotion.ToString() + " program with motion. ";
            }
            else
            {
                toolStripStatusLabel1.Text = "Found " + programsWithMotion.ToString() + " programs with motion. ";
            }
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        // NOTE   This code can be added to the BeforeCheck event handler instead of the AfterCheck event.
        // After a tree node's Checked property is changed, all its child nodes are updated to the same value.
        private void RobotProgramTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        private void ConversionExeButton_Click(object sender, EventArgs e)
        {
            int pointsConverted = 0;
            bool conversionSuccess = true;
            foreach (TreeNode program in RobotProgramTreeView.Nodes)
            {
                program.Checked = false;
                foreach (TreeNode point in program.Nodes)
                {
                    if (point.Checked)
                    {
                        FanucPoint temp = (FanucPoint)point.Tag;
                        if (temp.IsCartRep && !temp.IsUnintialized && (JPosConvertRadioButton.Checked || SwapConvertRadioButton.Checked))
                        {
                            try
                            {
                                robot.ConvertPointToJoint(program.Text.ToUpper(), temp.Index);
                                point.ToolTipText = temp.ToString();
                                point.ForeColor = Color.Blue;
                            }
                            catch (Exception ex)
                            {
                                point.ToolTipText = "Conversion to Joint Representation FAILED. Point not converted." + Environment.NewLine +
                                                    "Point is either out of reach, in singularity, or in an unsupported configuration." + Environment.NewLine + Environment.NewLine +
                                                    "Original error: " + ex.Message + Environment.NewLine + Environment.NewLine + temp.ToString();
                                point.ForeColor = Color.Red;
                                conversionSuccess = false;
                            }
                            pointsConverted++;
                        }
                        else if (temp.IsJointRep && !temp.IsUnintialized && (LposConvertRadioButton.Checked || SwapConvertRadioButton.Checked))
                        {
                            robot.ConvertPointToXYZWPR(program.Text.ToUpper(), temp.Index);
                            pointsConverted++;
                            point.ToolTipText = temp.ToString();
                            point.ForeColor = Color.Green;
                        }
                        point.Checked = false;
                    }
                }
            }
            //GenerateRobotProgramTreeView();
            toolStripStatusLabel1.Text = "Converted " + pointsConverted.ToString();
            if (pointsConverted <= 1)
            {
                toolStripStatusLabel1.Text += " point.";
            }
            else
            {
                toolStripStatusLabel1.Text += " points.";
            }
            if (!conversionSuccess)
            {
                toolStripStatusLabel1.Text = "Failure converting 1 or more points.";
            }
        }

        private void robotInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RobotInfoForm robotInfoForm = new RobotInfoForm(robot);
            robotInfoForm.Show();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FanucProgram selectedRobotProgram;
            if (RobotProgramTreeView.SelectedNode.Tag is FanucPoint)
            {
                selectedRobotProgram = (FanucProgram)RobotProgramTreeView.SelectedNode.Parent.Tag;
            }
            else
            {
                selectedRobotProgram = (FanucProgram)RobotProgramTreeView.SelectedNode.Tag;
            }
            Clipboard.SetText(selectedRobotProgram.ToString());
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FanucProgram selectedRobotProgram;
            if (RobotProgramTreeView.SelectedNode.Tag is FanucPoint)
            {
                selectedRobotProgram = (FanucProgram)RobotProgramTreeView.SelectedNode.Parent.Tag;
            }
            else
            {
                selectedRobotProgram = (FanucProgram)RobotProgramTreeView.SelectedNode.Tag;
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Robot Program (*.ls)|*.ls|Comma Seperated Values (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FileName = selectedRobotProgram.Name;
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                if (saveFileDialog1.FileName.ToLower().EndsWith(".csv"))
                {
                    byte[] csvPointsBytes;
                    string csvPoints="Position, Rep Type, UF, UT, J1/X, J2/Y, J3/Z, J4/W, J5/P, J6/R" + Environment.NewLine;
                    foreach (FanucPoint point in selectedRobotProgram.Points)
                    {
                        csvPoints += point.ToCsvString();
                    }
                    csvPointsBytes = Encoding.ASCII.GetBytes(csvPoints);
                    fs.Write(csvPointsBytes, 0, csvPointsBytes.Length);
                } 
                else
                {
                    byte[] robotProgram = new UTF8Encoding(true).GetBytes(selectedRobotProgram.ToString());
                    fs.Write(robotProgram, 0, robotProgram.Length);
                }
                fs.Close();
            }
        }

        private void RobotProgramTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "instructions\\instructions.pdf";
            System.Diagnostics.Process.Start(filename);
        }

        private void robotDHParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RobotDHParamsForm robotDHParamsForm = new RobotDHParamsForm(robot);
            robotDHParamsForm.Show();
        }
    }
}
