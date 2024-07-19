using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bet365Newparser.bet365;
using System.Net;

namespace Bet365Newparser
{
    
    public partial class Form1 : Form
    {
        
        Bet365Simple ss = new Bet365Simple();
        WebBrowser wb = new WebBrowser() { ScriptErrorsSuppressed = true };
        public Form1()
        {
            InitializeComponent();
            initcookese();
            ss.rich = richTextBox1;
            ss.initsock();
            ss.loadMatches();
            ss.wb = wb;
            //wb.Headers.Add(HttpRequestHeader.Cookie, "somecookie");
        }
        public CookieContainer GetCookieContainer()
        {
            CookieContainer container = new CookieContainer();
            try
            {
                
                Uri target = new Uri("https://mobile.bet365.com");
                foreach (string cookie in wb.Document.Cookie.Split(';'))
                {
                    string name = cookie.Split('=')[0];
                    string value = cookie.Substring(name.Length + 1);

                    container.Add(new Cookie(name.Trim(), value.Trim()) { Domain = target.Host });
                }
            }
            catch(Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }

            return container;
        }
        bool inited = false;
        void initcookese()
        {
            wb.Navigate("https://mobile.bet365.com");
            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            richTextBox1.AppendText("Navigated");
            inited = true;
            ss.passcookies(GetCookieContainer());
            wb.Dispose();
            wb = null;
        }
        private void button1_Click(object sender, EventArgs e)
        {


            if (!inited)
                initcookese();
            //richTextBox1.Text = string.Join("\n", wb.co);

            else
               ss.loadMatches();
            //ss.ParseAll();
            //Task tsk = new Task(()=> { ss.ParseAll(); });
            //tsk.Start();
            richTextBox1.Text = string.Join("\n",ss.matchesid);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ss.parsematches();
            richTextBox1.Text = ss.renderJson();
        }
        Timer timer1 = new Timer();
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 5000; // in miliseconds
            //timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ss.parsematches();
            //richTextBox1.Text = ss.renderJson();
            if (ss.listevents != null)
                richTextBox1.Text += "\n" + ss.matchesid.Count + " " + ss.listevents.Count;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                InitTimer();
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }
    }
}
