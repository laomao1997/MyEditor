using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyEditor
{
    public partial class EditorForm : Form
    {
        private string username;

        /**
         * This function clear the RichTextBox to create a new file
         */
        private void newFile()
        {
            richTextBox1.Clear();
        }

        /**
         * This function allows RichTextBox Open a Text or RTF file by opening an OpenFileDialog
         */
        private void openFile()
        {
            Stream stream;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open a Text File";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files(*.*)|*.*";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((stream = openFileDialog.OpenFile()) != null)
                {
                    string fileName = openFileDialog.FileName;
                    // string fileText = File.ReadAllText(fileName);
                    // MessageBox.Show(fileName);
                    richTextBox1.LoadFile(fileName);
                }
            }
        }

        /**
         * This function make the selected text be bold style
         */
        private void doBold()
        {
            richTextBox1.SelectionFont = new Font(this.Font, FontStyle.Bold);
        }

        /**
         * This function make the selected text be italic style
         */
        private void doItalic()
        {
            richTextBox1.SelectionFont = new Font(this.Font, FontStyle.Italic);
        }

        private void doUnderline()
        {
            richTextBox1.SelectionFont = new Font(this.Font, FontStyle.Underline);
        }

        public EditorForm(string username)
        {
            this.username = username;
            InitializeComponent();
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            toolStripLabelUsername.Text = "Username: " + username;
        }

        private void EditorForm_Closing(object sender, FormClosingEventArgs e)
        {
            // System.Environment.Exit(0);
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form loginForm = new Form1();
            loginForm.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            doBold();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            doItalic();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            doUnderline();
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            // MessageBox.Show(toolStripComboBox1.Text);
            string fontName = richTextBox1.SelectionFont.Name;
            richTextBox1.SelectionFont = new Font(fontName, Int32.Parse(toolStripComboBox1.Text));
        }
    }
}
