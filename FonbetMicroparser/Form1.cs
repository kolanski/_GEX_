using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FonbetMicroparser
{
    public partial class Form1 : Form
    {
        FonbetSimple ss;
        public Form1()
        {
            InitializeComponent();
            ss = new FonbetSimple();
            ss.rich = richTextBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ss.getgames();
        }
    }
}
