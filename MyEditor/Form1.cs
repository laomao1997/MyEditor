using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;

namespace MyEditor
{
    public partial class Form1 : Form

    {

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            Users users = new Users();
            if (users.IsUserExist(username, password) && username!="" && password!="")
            {
                Form editorForm = new ChildForm();
                editorForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login Failure (unknown username or incorrect password)");
            }
        }

        private void buttonNewUser_Click(object sender, EventArgs e)
        {
            Form regForm = new RegForm();
            regForm.Show();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
