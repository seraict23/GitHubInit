using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform_test01
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstLayer.DataSource = null;
            lstLinetype.DataSource = null;
            lstTextstyle.DataSource = null;
            lblLayercount.Text = "";
            lblLinecount.Text = "";
            lblTextcount.Text = "";
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            string choice = "";

            //using combobox
            choice = cboOptions.SelectedItem.ToString();
            MyCommands myCommands = new MyCommands();
            if (choice == "Layer")
            {
                ArrayList layers = myCommands.GetLayers();
                lstLayer.DataSource = layers;
                lstLinetype.DataSource = null;
                lstTextstyle.DataSource = null;
                lblLayercount.Text = "Layers count: " + layers.Count.ToString();
            } 
            else if (choice == "LineType")
            {
                ArrayList linetypes = myCommands.getLineTypes();
                lstLayer.DataSource = null;
                lstLinetype.DataSource = linetypes;
                lstTextstyle.DataSource = null;
                lblLinecount.Text = "LineType count: " + linetypes.Count.ToString();
            }
            else if (choice == "TextStyle")
            {
                ArrayList textstyles = myCommands.getTextStyles();
                lstLayer.DataSource = null;
                lstLinetype.DataSource = null;
                lstTextstyle.DataSource = textstyles;
                lblTextcount.Text = "LineType count: " + textstyles.Count.ToString();
            }
            else if (choice == "All")
            {
                ArrayList layers = myCommands.GetLayers();
                ArrayList linetypes = myCommands.getLineTypes();
                ArrayList textstyles = myCommands.getTextStyles();
                lstLayer.DataSource = layers;
                lstLinetype.DataSource = linetypes;
                lstTextstyle.DataSource = textstyles;
                lblLayercount.Text = "Layers count: " + layers.Count.ToString();
                lblLinecount.Text = "LineType count: " + linetypes.Count.ToString();
                lblTextcount.Text = "LineType count: " + textstyles.Count.ToString();
            }
        }
    }
}
