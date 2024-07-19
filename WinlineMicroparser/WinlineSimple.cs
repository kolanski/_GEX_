using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroparserFramework;

namespace WinlineMicroparser
{
    class WinlineSimple
    {
        string WinlineStartParse = "";
        ChromiumWebBrowser wb;
        bool init = false;
        RichTextBox rich;
        microserver server;
        public List<Event> games = new List<Event>();
        internal string path;

        public WinlineSimple(ChromiumWebBrowser browser,RichTextBox rich)
        {
            try
            {
                WinlineStartParse = System.IO.File.ReadAllText(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("WinlineMicroparser.exe","") +"\\"+"winline.js");
                Console.WriteLine(WinlineStartParse.Length);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            wb = browser;
            wb.FrameLoadEnd += Wb_FrameLoadEnd;
            this.rich = rich;
            server = new microserver(rich);
            server.setActions(loadmatches, getEvents);
            server.init("30287");
        }

        private void Wb_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (!init)
            {
                wb.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(WinlineStartParse);
                init = true;
            }
        }

        public void getEvents()
        {
            //loadmatches();
            init = false;
            var task = wb.GetBrowser().FocusedFrame.EvaluateScriptAsync("window.location.href='"+path+"'", new TimeSpan(0, 0, 1).ToString());
            task.Wait();
            
        }
        public void loadmatches()
        {
            if (init)
            {

                var task = wb.GetBrowser().FocusedFrame.EvaluateScriptAsync("parse();JSON.stringify(structarr);", new TimeSpan(0, 0, 1).ToString());
                task.Wait();
                var response = task.Result;
                if(response.Success)
                server.parseJson(response.Result.ToString());
                
            }
            else
            {
                Console.WriteLine("Is Not Init");
            }
        }

        internal void rendermatches()
        {
            string torender = "";
            foreach(var text in server.events)
            {
                torender+= text.ToString();
            }
            rich.Text = torender;
            
        }
    }
}
