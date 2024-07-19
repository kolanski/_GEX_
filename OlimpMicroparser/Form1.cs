using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OlimpMicroparser
{
    public partial class Form1 : Form
    {
        OlimpSimple olimp;
        Timer tm1 = new Timer();
        int cntr = 0;
        public Form1()
        {
            InitializeComponent();
            olimp = new OlimpSimple(richTextBox1);
            olimp.richTextBox1 = richTextBox1;
            olimp.GetMatches();
            tm1.Interval = 4000;
            tm1.Start();
            tm1.Tick += Tm1_Tick;
        }

        private void Tm1_Tick(object sender, EventArgs e)
        {
            cntr++;
            if(cntr>=10)
            {
                cntr = 0;
                olimp.GetMatches();
            }
            olimp.GetGames();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            olimp.GetMatches();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            olimp.GetGames();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                tm1.Start();
            }
            else
                tm1.Stop();
        }
    }
}
