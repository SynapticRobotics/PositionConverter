using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PositionConverter
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            string version = "v" + Application.ProductVersion;
            infoText.Text = "Current Version: " + version + Environment.NewLine + Environment.NewLine + infoText.Text;
        }

        private void SynapticRoboticsPictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
