using CefSharp.WinForms;
using MicroparserFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;

namespace Bet365MicroparserChromium
{
    class Bet365Simple
    {
        private ChromiumWebBrowser chromium;
        public string path;
        bool init = false;
        RichTextBox rich;
        microserver server;
        public List<Event> games = new List<Event>();

        string Bet365StartParse = "";
        public Bet365Simple(ChromiumWebBrowser chromium, RichTextBox richTextBox1)
        {
            try
            {
                Bet365StartParse = System.IO.File.ReadAllText(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("Bet365MicroparserChromium.exe", "") + "\\" + "bet365.js");
                Console.WriteLine(Bet365StartParse.Length);
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
            server.init("30365");
        }

        private void Chromium_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            counter++;
            rich.Invoke(new Action(() => { rich.AppendText("Current count:" + counter.ToString() + System.Environment.NewLine); }));
            if (!init && counter >= 2)
            {
                chromium.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(Bet365StartParse);
                counter = 0;
                init = true;
            }
        }
        public void setinit()
        {
            chromium.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(Bet365StartParse);
            counter = 0;
            init = true;
        }
        private void getEvents()
        {
            //reload
            counter = 0;
            init = false;
            started = false;
            chromium.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync("window.location.href='https://www.bet365.com/#/IP/'");
        }
        public void goreload()
        {
            getEvents();
        }
        public void loadmatches()
        {
            //parse
            if (init)
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
        
    }
}
