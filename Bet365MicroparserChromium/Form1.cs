using CefSharp;
using CefSharp.WinForms;
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

namespace Bet365MicroparserChromium
{
    public partial class Form1 : Form
    {
        Bet365Simple bet365;
        public ChromiumWebBrowser chromium;
        string path = "https://www.bet365.com/#/IP/";
        public Form1()
        {
            InitializeComponent();
            InitBrowser();
            bet365 = new Bet365Simple(chromium, this.richTextBox1);
            bet365.path = path;
        }
        public void InitBrowser()
        {

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data

            };
            //Perform dependency check to make sure all relevant resources are in our output directory.
            // Create the offscreen Chromium browser.
            Cef.Initialize(new CefSettings()
            {
                //UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246",
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache1")
            }, performDependencyCheck: true, browserProcessHandler: null);
            chromium = new ChromiumWebBrowser(path);
            this.Controls.Add(chromium);
            chromium.Dock = DockStyle.Fill;
            //chromium = new ChromiumWebBrowser(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bet365.loadmatches();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bet365.rendermatches();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bet365.goreload();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cef.Shutdown();
            System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache1"));

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bet365.setinit();
        }
    }
}