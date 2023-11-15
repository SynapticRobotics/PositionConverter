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
    public partial class RobotDHParamsForm : Form
    {
        private FanucRobot internalRobot;

        public RobotDHParamsForm(FanucRobot fanucRobot)
        {
            InitializeComponent();
            internalRobot = fanucRobot;
            j1LinkAnumericUpDown.Controls[0].Visible = false;
            j1LinkAnumericUpDown.Value = Convert.ToDecimal(fanucRobot.j1LinkA);
            j2LinkAnumericUpDown.Controls[0].Visible = false;
            j2LinkAnumericUpDown.Value = Convert.ToDecimal(fanucRobot.j2LinkA);
            j3LinkAnumericUpDown.Controls[0].Visible = false;
            j3LinkAnumericUpDown.Value = Convert.ToDecimal(fanucRobot.j3LinkA);
            j4LinkDnumericUpDown.Controls[0].Visible = false;
            j4LinkDnumericUpDown.Value = Convert.ToDecimal(fanucRobot.j4LinkD);
            facePlateThicknessNumericUpDown.Controls[0].Visible = false;
            facePlateThicknessNumericUpDown.Value = Convert.ToDecimal(fanucRobot.facePlateThickness);
        }

        private void RobotDHParamsForm_Load(object sender, EventArgs e)
        {

        }

        private void setDHButton_Click(object sender, EventArgs e)
        {
            internalRobot.j1LinkA = Convert.ToDouble(j1LinkAnumericUpDown.Value);
            internalRobot.j2LinkA = Convert.ToDouble(j2LinkAnumericUpDown.Value);
            internalRobot.j3LinkA = Convert.ToDouble(j3LinkAnumericUpDown.Value);
            internalRobot.j4LinkD = Convert.ToDouble(j4LinkDnumericUpDown.Value);
            internalRobot.facePlateThickness = Convert.ToDouble(facePlateThicknessNumericUpDown.Value);
            internalRobot.robotArmType = "CUSTOM";
            internalRobot.SetDHParams(internalRobot.robotArmType);
            setDHButton.Text = "Set!";
        }
    }
}
