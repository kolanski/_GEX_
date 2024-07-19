using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PinnacleWrapper;
using PinnacleWrapper.Data;
using MicroparserFramework;

namespace PinnacleMicroparser
{
    public partial class Form1 : Form
    {
        PinacleSimple ss;
        public Form1()
        {
            InitializeComponent();
            ss = new PinacleSimple(richTextBox1);
            //ss.LoadOdds2();
            //ss.rich = richTextBox1;
            //client = new PinnacleClient("MP10000", "asdasjdjw!", "USD",PinnacleWrapper.Enums.OddsFormat.DECIMAL);
              // 12 is the E-Sports Sport Id. This gets all Esports Events currently offered
            //var odds = client.GetOdds(new GetOddsRequest(33)); // this retrieves the odds that correspond to each fixture.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if(ss.leagueids.Count==0)
                ss.LoadOdds2();
            else
                ss.LoadOdds2();
            if (ss.games.Count > 0)
                richTextBox1.AppendText(ext.renderJson(ss.games));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ss.LoadMatches();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ss.LoadOdds2();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(string.Join(System.Environment.NewLine, ss.games));
        }
    }
    public class eventdata
    {
        public int mainid;
        public int selfid;
        public string data;
        public eventdata(int mainid, int selfid, string data)
        {
            this.mainid = mainid;
            this.selfid = selfid;
            this.data = data;
        }
    }
}
