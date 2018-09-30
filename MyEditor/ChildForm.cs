using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyEditor
{
    public partial class ChildForm : Form
    {
        public ChildForm()
        {
            InitializeComponent();

        }

        private void ChildForm_Load(object sender, EventArgs e)
        {

        }

        private void ChildForm_Closing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
