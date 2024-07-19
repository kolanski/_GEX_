using CefSharp;
using CefSharp.OffScreen;
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

namespace WinlineMicroparser
{
    public partial class Form1 : Form
    {
        WinlineSimple winline;
        public ChromiumWebBrowser chromium;
        string path = "https://www.winlinebet.ru/stavki/sport/tennis";
        public Form1()
        {
            InitializeComponent();
            InitBrowser();
            winline = new WinlineSimple(chromium,this.richTextBox1);
            winline.path = path;
        }

        public void InitBrowser()
        {
            
            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                
            };
            //Perform dependency check to make sure all relevant resources are in our output directory.
            // Create the offscreen Chromium browser.
            Cef.Initialize(new CefSettings() { UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1" ,
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            }, performDependencyCheck: true, browserProcessHandler: null);
            chromium = new ChromiumWebBrowser(path);
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            winline.loadmatches();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            winline.rendermatches();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
