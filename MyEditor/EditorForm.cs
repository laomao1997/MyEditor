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
        private string username = "";
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
        /// <param name="style">事件触发后传参：字体格式类型</param>  
        private void ChangeFontStyle(FontStyle style)
        {
            if (style != FontStyle.Bold && style != FontStyle.Italic &&
                style != FontStyle.Underline)
                throw new System.InvalidProgramException("字体格式错误");
            RichTextBox tempRichTextBox = new RichTextBox();  //将要存放被选中文本的副本  
            int curRtbStart = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int tempRtbStart = 0;
            Font font = richTextBox1.SelectionFont;
            if (len <= 1 && font != null) //与上边的那段代码类似，功能相同  
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
            tempRichTextBox.Select(len - 1, 1); //选中副本中的最后一个文字  
                                                //克隆被选中的文字Font，这个tempFont主要是用来判断  
                                                //最终被选中的文字是否要加粗、去粗、斜体、去斜、下划线、去下划线  
            Font tempFont = (Font)tempRichTextBox.SelectionFont.Clone();

            //清空2和3  
            for (int i = 0; i < len; i++)
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);  //每次选中一个，逐个进行加粗或去粗  
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
            richTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf; //将设置格式后的副本拷贝给原型  
            richTextBox1.Select(curRtbStart, len);
        }

        /// <summary>  
        /// Set font, according to Font Combobox
        /// </summary>  
        /// <param name="fontName">被选中的字体名</param>  
        private void ChangeFont(string fontName)
        {
            if (fontName == string.Empty)
                throw new System.Exception("字体名称参数错误，不能为空");

            RichTextBox tempRichTextBox = new RichTextBox();  //用于保存被选中文本的副本  

            //curRichTextBox是当前文本，即原型  
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
            for (int i = 0; i < len; i++)  //逐个设置字体种类  
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);
                tempRichTextBox.SelectionFont =
                    new Font(fontName, tempRichTextBox.SelectionFont.Size,
                        tempRichTextBox.SelectionFont.Style);
            }

            //将副本内容插入到到原型中  
            tempRichTextBox.Select(tempRtbStart, len);
            richTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf;
            richTextBox1.Select(curRtbStart, len);
            richTextBox1.Focus();
        }

        /// <summary>  
        /// Set font size, according to Font Size Combobox  
        /// </summary>  
        /// <param name="fontSize">被选中的字号</param>  
        private void ChangeFontSize(float fontSize)
        {
            if (fontSize <= 0.0)
                throw new InvalidProgramException("字号参数错误，不能小于等于0.0");

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
    }
}
