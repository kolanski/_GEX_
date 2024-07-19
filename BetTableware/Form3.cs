using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetTableware
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(Program.MainForm.formsettings.getText())
                this.richTextBox1.Font = new Font("Arial Black", 10,FontStyle.Bold);
            else
                this.richTextBox1.Font = new Font("Arial", 10, FontStyle.Regular);
            if (richTextBox1.Text.Contains("ms"))
            {
                int textEnd = richTextBox1.TextLength;

                Font fnt = new Font("Tahoma", 14, FontStyle.Bold);
                int index = 0;
                int lastIndex = richTextBox1.Text.LastIndexOf("Fonbet"); ; //textbox1.text is the text to find
                while (index < lastIndex)
                {
                    richTextBox1.Find("Fonbet", index, textEnd, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Blue;  //set the color to blue if found
                    richTextBox1.SelectionFont = fnt; //bold the text if found
                    index = richTextBox1.Text.IndexOf("Fonbet", index) + 1;
                }
                index = 0;
                lastIndex = richTextBox1.Text.LastIndexOf("Betcity"); ; //textbox1.text is the text to find
                while (index < lastIndex)
                {
                    richTextBox1.Find("Betcity", index, textEnd, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Green;  //set the color to blue if found
                    richTextBox1.SelectionFont = fnt; //bold the text if found
                    index = richTextBox1.Text.IndexOf("Betcity", index) + 1;
                }
                index = 0;
                lastIndex = richTextBox1.Text.LastIndexOf("Maraphon"); ; //textbox1.text is the text to find
                while (index < lastIndex)
                {
                    richTextBox1.Find("Maraphon", index, textEnd, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.DarkOrange;  //set the color to blue if found
                    richTextBox1.SelectionFont = fnt; //bold the text if found
                    index = richTextBox1.Text.IndexOf("Maraphon", index) + 1;
                }
                index = 0;
                lastIndex = richTextBox1.Text.LastIndexOf("Zenit"); ; //textbox1.text is the text to find
                while (index < lastIndex)
                {
                    richTextBox1.Find("Zenit", index, textEnd, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.DarkRed;  //set the color to blue if found
                    richTextBox1.SelectionFont = fnt; //bold the text if found
                    index = richTextBox1.Text.IndexOf("Zenit", index) + 1;
                }
                index = 0;
                lastIndex = richTextBox1.Text.LastIndexOf("Olimp"); ; //textbox1.text is the text to find
                while (index < lastIndex)
                {
                    richTextBox1.Find("Olimp", index, textEnd, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.DarkOrchid;  //set the color to blue if found
                    richTextBox1.SelectionFont = fnt; //bold the text if found
                    index = richTextBox1.Text.IndexOf("Olimp", index) + 1;
                }
                richTextBox1.SelectionLength = 0;
            }
            }
        
        public void Hideo()
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Hide();
            });
        }
    }
}
