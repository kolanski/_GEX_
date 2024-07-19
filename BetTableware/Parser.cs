using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using mshtml;

namespace BetTableware
{
    class ParserBC
    {
        //string status;
        public WBP.bnag ParWB = new WBP.bnag("Betcity");
        private void press_submit()
        {
            foreach (HtmlElement hh in ParWB.wb.Document.GetElementsByTagName("input"))
            {
                if (hh.GetAttribute("value") == "Show")
                    hh.InvokeMember("click");
            }
        }
        private void press_all()
        {
            foreach (HtmlElement hh in ParWB.wb.Document.GetElementsByTagName("a"))
            {
                //richTextBox1.AppendText(hh.InnerText);
                if (hh.InnerText == "Показать")
                    hh.InvokeMember("click");
            }
        }
        private void refpage()
        {
           if( ParWB.wb.Document!=null)
            foreach (HtmlElement hh in ParWB.wb.Document.GetElementsByTagName("a"))
            {
                if (hh!=null&&hh.InnerText.Contains("Обновить"))
                {
                    //  richTextBox2.AppendText(hh.InnerText+hp);
                    //  richTextBox2.AppendText(hh.GetAttribute("className"));
                    //MethodInfo clickMethod = hh.GetType().GetMethod("click");
                    hh.InvokeMember("click");
                    Program.MainForm.addtext3(System.DateTime.Now.ToString()+"Refreshing Data...");
                    break;
                }
                // hp++;
                // if (hh.InnerText == "All events")
                //    hh.InvokeMember("click");
            }
        }
        private void check_all()
        {
            foreach(HtmlElement hh in ParWB.wb.Document.GetElementsByTagName("input"))
            {
                if(hh.GetAttribute("name").Contains("livebets"))
                    hh.SetAttribute("checked", "true");
            }
        }
        private void navigate()
        {
            
           // object[] args = { "bets"};
            ParWB.loadpage("http://betcityru.com/live/line.php");
            waitstatus("WBRdy"); Application.DoEvents();
            check_all();
            press_all();
            waitstatus("WBRdy"); Application.DoEvents();
            //ParWB.wb.Document.InvokeScript("subm_all", args); Application.DoEvents();
            //waitstatus("WBRdy"); Application.DoEvents();
            //waitstatus("WBCmp");
            //ParWB.wb.Document.InvokeScript("check_all_l"); Application.DoEvents();
            //press_submit();
            //waitstatus("WBRdy"); Application.DoEvents();
        }
        public void refresh()
        {
            ParWB.refpage("http://betcityru.com/live/line.php");
            waitstatus("WBRdy"); 
            Application.DoEvents();
            check_all();
            press_all();
            waitstatus("WBRdy"); Application.DoEvents();
        }
        public void refresh2()
        {
            refpage();
            //waitstatus("Navigated","WBRdy"); 

        }
        private void waitstatus(string status1)
        {
            while (ParWB.wbst.status != status1)
            {
                Application.DoEvents();
            }
        }
        private void waitstatus(string status1,string status2)
        {
            while (ParWB.wbst.status != status1 || ParWB.wbst.status != status2)
            {
                Application.DoEvents();
            }
        }
    }
    class ParcerFB : ParserBC
    {
        public void navigate()
        {
            ParWB.loadpage("https://live.fonbet.com/livebets/?locale=ru");
            //ParWB.wait_doneloading(6);
            ParWB.waitcontains(1, 6);
            //ParWB.waitcontains("Готово");
            //ParWB.waitcontains("Готово!");
            //waitstatus("WBRdy");
        }
        private void waitstatus(string status1)
        {
            while (ParWB.wbst.status != status1)
            {
                Application.DoEvents();
            }
        }
    }
    class ParserMF : ParserBC
    {
        public void navigate()
        {
            ParWB.loadpage("");
        }
        private void waitstatus(int secs)
        {

        }
    }
    class Parser2BC
    {
        private Awesomium.Windows.Forms.WebControl webBr=new Awesomium.Windows.Forms.WebControl();
        public string gm;
        public void navigate()
        {
           
           this.webBr.Source=new Uri("http://betcityru.com/live/line.php");
            while (this.webBr.IsLoading)
                Application.DoEvents();
         this.webBr.ExecuteJavascript(String.Format("var list = document.getElementById('tab-now').getElementsByTagName('input');for(var i=0, il=list.length; i<il; i++) list[i].click();"));
           this.webBr.ExecuteJavascript(String.Format("$('a.btn.f1').click();"));
        }
        public void refresh2()
        {
            webBr = new Awesomium.Windows.Forms.WebControl();
            webBr.Source = new Uri("http://betcityru.com/live/line.php");
            while (webBr.IsLoading)
                Application.DoEvents();
            webBr.ExecuteJavascript(String.Format("var list = document.getElementById('tab-now').getElementsByTagName('input');for(var i=0, il=list.length; i<il; i++) list[i].click();"));
            webBr.ExecuteJavascript(String.Format("$('a.btn.f1').click();"));
        }
        public void getdoc()
        {
            Awesomium.Core.JSValue lol;
            lol = this.webBr.ExecuteJavascriptWithResult("document.getElementsByTagName('html')[0].innerHTML");
            gm = lol.ToString();
            //return lol.ToString();
        }

    }
    class Parser2FB : Parser2BC
    {
        private Awesomium.Windows.Forms.WebControl webBr = new Awesomium.Windows.Forms.WebControl();
      /*  public void navigate()
        {
            this.webBr.Source = new Uri("https://live.fonbet.com/livebets/?locale=ru");
            while (webBr.IsLoading)
                Application.DoEvents();
        }*/
    }
    class Parser2MF : Parser2BC
    {

    }

}
