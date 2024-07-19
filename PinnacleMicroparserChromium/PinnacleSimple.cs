using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.WinForms;
using System.Windows.Forms;
using MicroparserFramework;
using CefSharp;

namespace PinnacleMicroparserChromium
{
    class PinnacleSimple
    {
        private ChromiumWebBrowser chromium;
        private object richTextBox1;
        public string path;
        bool init = false;
        RichTextBox rich;
        microserver server;
        public List<Event> games = new List<Event>();

        string PinnacleStartParse = "";

        public PinnacleSimple(ChromiumWebBrowser chromium, RichTextBox richTextBox1)
        {
            try
            {
                PinnacleStartParse = System.IO.File.ReadAllText(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("PinnacleMicroparserChromium.exe", "") + "\\" + "pinnacle.js");
                Console.WriteLine(PinnacleStartParse.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.chromium = chromium;
            this.chromium.FrameLoadEnd += Chromium_FrameLoadEnd;
            
            this.rich = richTextBox1;
            server = new microserver(rich);
            server.setActions(loadmatches, getEvents);
            server.init("30085");
        }

        private void getEvents()
        {
            //reload
            counter = 0;
            init = false;
            started = false;
            chromium.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync("window.location.href='https://www.pinnacle.com/ru/login'");
        }
        public void goreload()
        {
            getEvents();
        }
        public void loadmatches()
        {
            //parse
            if (started && init)
            {
                var task = chromium.GetBrowser().FocusedFrame.EvaluateScriptAsync("parse();JSON.stringify(structarr);", new TimeSpan(0, 0, 1).ToString());
                task.Wait();
                var response = task.Result;
                if (response.Success)
                    server.parseJson(response.Result.ToString());
            }
        }
        internal void rendermatches()
        {
            string torender = "";
            foreach (var text in server.events)
            {
                torender += text.ToString();
            }
            rich.Text = torender;
        }
        int counter = 0;
        bool started = false;
        private void Chromium_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            counter++;
            rich.Invoke(new Action(() => { rich.AppendText("Current count:" + counter.ToString() + System.Environment.NewLine); }));

            //state controlling
            //initiate state
            if (!init && counter >= 5)
            {
                    chromium.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(PinnacleStartParse);
                counter = 0;
                init = true;
            }
            if (init && counter >= 4)
            {
                chromium.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(PinnacleStartParse);
                counter = 0;
                started = true;
            }
            if(started&&counter==4)
            {
                chromium.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(PinnacleStartParse);
                counter = 0;
            }
        }
    }
}