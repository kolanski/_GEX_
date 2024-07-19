using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParimatchMicroparser
{
    public partial class Form1 : Form
    {
        ParimatchSimple parimatch;
        public Form1()
        {
            InitializeComponent();
            parimatch = new ParimatchSimple();
            parimatch.rich = richTextBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            parimatch.loadmatches();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(parimatch.matchesid!=null&&parimatch.matchesid.Count>0)
            parimatch.loadgames();
        }
    }
}
