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
    public partial class RegForm : Form
    {
        public RegForm()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (checkComplete())
            {
                string newUserInfo = "";
                string username = textBoxUsername.Text;
                string password = textBoxPassword.Text;
                string firstName = textBoxFirstName.Text;
                string lastName = textBoxLastName.Text;
                string doBirth = metroDateTime1.Text;
                string userType = metroComboBox1.Text;
                newUserInfo = "Registration success!\nWelcome " + firstName + lastName + ".\nYour user type is: " + userType + ".";
                Users users = new Users();
                if (!users.IsUserExist(username))
                {
                    DialogResult dialog = MessageBox.Show(newUserInfo, "Registration Success");
                    users.AddUser(username, password, firstName, lastName, doBirth, userType);
                    if (dialog == DialogResult.OK)
                    {
                        this.Hide();
                        var loginForm = new Form1();
                        loginForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("User already exist!");
                }
            }
            
        }

        // Check whether all the text boxes are filled correctly.
        private bool checkComplete()
        {
            bool isComplete = true;
            string uncompleteMessage = "";
            if (textBoxPassword2.Text != textBoxPassword.Text)
            {
                uncompleteMessage += "The password is not same. Please check\n";
                isComplete = false;
            }

            if (textBoxFirstName.Text.Equals(""))
            {
                uncompleteMessage += "First name cannot be empty.\n";
                isComplete = false;
            }

            if (textBoxLastName.Text.Equals(""))
            {
                uncompleteMessage += "Second name cannot be empty.\n";
                isComplete = false;
            }

            if (metroComboBox1.Text.Equals(""))
            {
                uncompleteMessage += "User type cannot be empty.\n";
                isComplete = false;
            }

            if (textBoxUsername.Text.Length < 4)
            {
                uncompleteMessage += "The length of username cannot less then 4.\n";
                isComplete = false;
            }
            if (textBoxPassword.Text.Length < 4)
            {
                uncompleteMessage += "The length of password cannot less then 4.\n";
                isComplete = false;
            }

            if (!isComplete) MessageBox.Show(uncompleteMessage);
            return isComplete;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form loginForm = new Form1();
            loginForm.Show();
        }

        private void RegForm_Load(object sender, EventArgs e)
        {

        }
    }
}
