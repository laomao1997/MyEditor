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

/*
----------------------------------------------------
|   EditorForm(username: string, password: string) |
----------------------------------------------------
| - username: string                               |
| - usertype: string                               |
| - fileName: string                               |
----------------------------------------------------
| + newFile()                                      |
| + openFile()                                     |
| + saveFile()                                     |
| + saveAsFile()                                   |
| + ChangeFontStyle(style: FontStyle)              |
| + ChangeFont(fontName: string)                   |
| + ChangeFontSize(fontSize: float)                |
| + DoCut()                                        |
| + DoCopy()                                       |
| + DoPaste()                                      |
----------------------------------------------------
*/

namespace MyEditor
{
    public partial class EditorForm : Form
    {
        private string username = "";
        private string usertype = "view";
        private string fileName = "";

        ///<summary>
        /// Clear the RichTextBox to create a new file
        /// </summary>
        private void newFile()
        {
            richTextBox1.Clear();
            fileName = "";
        }

        ///<summary>  
        ///Open a Text or RTF file by opening an OpenFileDialog
        ///</summary>  
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
                    fileName = openFileDialog.FileName;
                    string suffixName = fileName.Substring(fileName.IndexOf(".")+1);
                    if (suffixName.Equals("rtf"))
                    {
                        richTextBox1.LoadFile(openFileDialog.FileName);
                    }
                    else if (suffixName.Equals("txt"))
                    {
                        Font deFont = new Font("Consolas", 12, FontStyle.Regular);
                        string fileText = File.ReadAllText(fileName);
                        richTextBox1.Text = fileText;
                        richTextBox1.Font = DefaultFont;
                    }
                    else
                    {
                        MessageBox.Show("Unsupported file.");
                    }
                }
                stream.Close();
            }
            

            toolStripComboBox1.Text = "9";
            toolStripComboBox2.Text = "Consolas";
        }

        ///<summary>  
        ///Save all lines in RichTextBox into the RTF file.
        ///</summary>  
        private void saveFile()
        {
            if(fileName.Equals("")) saveAsFile();
            else if(fileName.Substring(fileName.IndexOf(".") + 1).Equals("txt"))
            {
                saveAsFile();
            }
            else
            {
                richTextBox1.SaveFile(fileName);
            }
        }

        /// <summary>
        /// Save all lines in RichTextBox into a new RTF file.
        /// </summary>
        private void saveAsFile()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Rich Text Format (*.rtf)|*.rtf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    fileName = saveFileDialog1.FileName;
                    myStream.Close();
                    richTextBox1.SaveFile(fileName);
                }
            }
        }

        ///<summary>  
        ///Set font style: Bold, Italic, Underline  
        ///</summary>   
        private void ChangeFontStyle(FontStyle style)
        {
            if (!usertype.Equals("Edit")) return;
            if (style != FontStyle.Bold && style != FontStyle.Italic &&
                style != FontStyle.Underline)
                throw new System.InvalidProgramException("Wrong font style.");
            RichTextBox tempRichTextBox = new RichTextBox();  // tempRichTextBox is used save the copy of the selected text
            int curRtbStart = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int tempRtbStart = 0;
            Font font = richTextBox1.SelectionFont;
            if (len <= 1 && font != null) 
            {
                if (style == FontStyle.Bold && font.Bold ||
                    style == FontStyle.Italic && font.Italic ||
                    style == FontStyle.Underline && font.Underline)
                {
                    richTextBox1.SelectionFont = new Font(font, font.Style ^ style);
                }
                else if (style == FontStyle.Bold && !font.Bold ||
                         style == FontStyle.Italic && !font.Italic ||
                         style == FontStyle.Underline && !font.Underline)
                {
                    richTextBox1.SelectionFont = new Font(font, font.Style | style);
                }
                return;
            }
            tempRichTextBox.Rtf = richTextBox1.SelectedRtf;
            tempRichTextBox.Select(len - 1, 1); // the last character of the selected text
            // clone the selected text 
            // set the font style
            Font tempFont = (Font)tempRichTextBox.SelectionFont.Clone();

            for (int i = 0; i < len; i++)
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);  // set the style for each part 
                if (style == FontStyle.Bold && tempFont.Bold ||
                    style == FontStyle.Italic && tempFont.Italic ||
                    style == FontStyle.Underline && tempFont.Underline)
                {
                    tempRichTextBox.SelectionFont =
                        new Font(tempRichTextBox.SelectionFont,
                            tempRichTextBox.SelectionFont.Style ^ style);
                }
                else if (style == FontStyle.Bold && !tempFont.Bold ||
                         style == FontStyle.Italic && !tempFont.Italic ||
                         style == FontStyle.Underline && !tempFont.Underline)
                {
                    tempRichTextBox.SelectionFont =
                        new Font(tempRichTextBox.SelectionFont,
                            tempRichTextBox.SelectionFont.Style | style);
                }
            }
            tempRichTextBox.Select(tempRtbStart, len);
            richTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf; // return the copy back to the current text  
            richTextBox1.Select(curRtbStart, len);
        }

        /// <summary>  
        /// Set font, according to Font Combobox
        /// </summary>  
        private void ChangeFont(string fontName)
        {
            if (!usertype.Equals("Edit")) return;
            if (fontName == string.Empty)
                throw new System.Exception("The font name cannot be empty.");

            RichTextBox tempRichTextBox = new RichTextBox();  // tempRichTextBox is used save the copy of the selected text

            //curRichTextBox is the current text 
            int curRtbStart = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int tempRtbStart = 0;

            Font font = richTextBox1.SelectionFont;
            if (len <= 1 && null != font)
            {
                richTextBox1.SelectionFont = new Font(fontName, font.Size, font.Style);
                return;
            }

            tempRichTextBox.Rtf = richTextBox1.SelectedRtf;
            for (int i = 0; i < len; i++)  // set font for each part
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);
                tempRichTextBox.SelectionFont =
                    new Font(fontName, tempRichTextBox.SelectionFont.Size,
                        tempRichTextBox.SelectionFont.Style);
            }

            // return the copy back to the current text 
            tempRichTextBox.Select(tempRtbStart, len);
            richTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf;
            richTextBox1.Select(curRtbStart, len);
            richTextBox1.Focus();
        }

        /// <summary>  
        /// Set font size, according to Font Size Combobox  
        /// </summary>  
        private void ChangeFontSize(float fontSize)
        {
            if (!usertype.Equals("Edit")) return;
            if (fontSize <= 0.0)
                throw new InvalidProgramException("The number of font size should be bigger than 0.0.");

            RichTextBox tempRichTextBox = new RichTextBox();

            int curRtbStart = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int tempRtbStart = 0;

            Font font = richTextBox1.SelectionFont;
            if (len <= 1 && null != font)
            {
                richTextBox1.SelectionFont = new Font(font.Name, fontSize, font.Style);
                return;
            }

            tempRichTextBox.Rtf = richTextBox1.SelectedRtf;
            for (int i = 0; i < len; i++)
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);
                tempRichTextBox.SelectionFont =
                    new Font(tempRichTextBox.SelectionFont.Name,
                        fontSize, tempRichTextBox.SelectionFont.Style);
            }

            tempRichTextBox.Select(tempRtbStart, len);
            richTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf;
            richTextBox1.Select(curRtbStart, len);
            richTextBox1.Focus();
        }

        /// <summary>
        /// Cut text from RichTextBox to Clipboard
        /// </summary>
        private void DoCut()
        {
            if (!usertype.Equals("Edit")) return;
            Clipboard.SetData(DataFormats.Rtf, richTextBox1.SelectedRtf);
            richTextBox1.SelectedRtf = "";
        }

        /// <summary>
        /// Copy text from RichTextBox to Clipboard
        /// </summary>
        private void DoCopy()
        {
            if (!usertype.Equals("Edit")) return;
            Clipboard.SetData(DataFormats.Rtf, richTextBox1.SelectedRtf);
        }

        /// <summary>
        /// Paste text to RichTextBox from Clipboard
        /// </summary>
        private void DoPaste()
        {
            if (!usertype.Equals("Edit")) return;
            richTextBox1.Paste();
        }

        public EditorForm(string username, string password)
        {
            this.username = username;
            Users users = new Users();
            InitializeComponent();
            richTextBox1.ReadOnly = true;
            //richTextBox1.Enabled = false;
            if (users.isEditType(username, password))
            {
                usertype = "Edit";
                richTextBox1.ReadOnly = false;
                //richTextBox1.Enabled = true;
            }
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
            ChangeFontStyle(FontStyle.Bold);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Italic);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Underline);
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            ChangeFontSize((float) Int32.Parse(toolStripComboBox1.Text));
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Form aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void toolStripComboBox2_TextChanged(object sender, EventArgs e)
        {
            ChangeFont(toolStripComboBox2.Text);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            DoCut();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            DoCopy();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            DoPaste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCopy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPaste();
        }

        private void EditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)       // Ctrl-S Save
            {
                newFile();
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
            }
            if (e.Control && e.KeyCode == Keys.O)       // Ctrl-S Save
            {
                openFile();
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
            }
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                saveFile();
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
            }
        }
    }
}
