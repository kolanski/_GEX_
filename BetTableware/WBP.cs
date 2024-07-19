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
    public class WBP
    {
        public delegate void EvOnChange();
        public class logS
        {

            public int els;
            public event EvOnChange AddEvent;
            List<string> list = new List<string>();
            public void Add(string toadd)
            {
                list.Add(toadd);
                if (AddEvent != null)
                    AddEvent();
                els++;
            }
            public string ToPrint(int elsnum)
            {
                if (els > 0 && elsnum < els)
                {
                    string[] array = list.ToArray();

                    return array[elsnum] + '\n';
                }
                else
                    return ("Empty!" + '\n');
            }

        };
        public struct stats
        {
            public string status;
            public int cnt;
        };

        public class bnag
        {
            public WebBrowser wb;

            public int readycount=0;
            public stats wbst=new stats();
            public string currentstat = "";
          //  public logS wblog = new logS();
            //RichTextBox RCB = new RichTextBox();
            public bool contains = false;
            int rcbcount = 0;
            Random rnd= new Random();
            string name = "Default";
            int rdycnt = 0;
            int ojcnt = 0;
            int ncount = 0;
            int sizex, sizey;
            //string contssss = "";
            //bool trytoconts = false;
            public bnag()
            {
                int tmp;
                
                tmp=rnd.Next();
                name = name + tmp.ToString();
              //  wblog.Add(System.DateTime.Now.ToString() + " Created instance of Bware named:" + name);
            }
            public bnag(string name1)
            {
                
           //     wblog.Add(System.DateTime.Now.ToString() + " Created instance of Bware named:" + name1);
                name = name1;
            }
            public void setdef(int szx, int szy)
            {
                sizex = szx;
                sizey = szy;
            }

            public void loadpage(string url)
            {
                
                
                wb = new WebBrowser();
                wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
                wb.ScriptErrorsSuppressed = true;
              //  wblog.AddEvent += new EvOnChange(wb_onadd);
                wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
                wb.Navigated += new WebBrowserNavigatedEventHandler(wb_Navigated);
                wb.StatusTextChanged += new EventHandler(wb_StatusChange);
                if (url != null)
                {
                    wb.Navigate(url);
                  //  wblog.Add(System.DateTime.Now.ToString() + " " + "User navigate on url:" + url);
                }
              //  else
              //      wblog.Add(System.DateTime.Now.ToString() + " " + "User navigate on empty url:");
            }
            public void refpage(string url)
            {
                wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
                wb.ScriptErrorsSuppressed = true;
                //  wblog.AddEvent += new EvOnChange(wb_onadd);
                wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
                wb.Navigated += new WebBrowserNavigatedEventHandler(wb_Navigated);
                wb.StatusTextChanged += new EventHandler(wb_StatusChange);
                if (url != null)
                {
                    wb.Navigate(url);
                    //  wblog.Add(System.DateTime.Now.ToString() + " " + "User navigate on url:" + url);
                }
            }
            private void wb_onadd()
            {
                //RCB.AppendText(wblog.ToPrint(rcbcount));
                rcbcount++;
            }
            public string getdoc()
            {
                return wb.Document.Body.Parent.OuterHtml;
            }
            private void wb_StatusChange(object sender,EventArgs e)
            {
                this.currentstat = wb.StatusText;
                if (wb.StatusText.Contains("Готово") || wb.StatusText.Contains("Done"))
                    rdycnt++;
                if (wb.StatusText.Contains("Ожидание") || wb.StatusText.Contains("Waiting "))
                    ojcnt++;
                    //contains = true;
            }
            private void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
            {
                wbst.status = "Navigated";
                wbst.cnt = ncount;
             //   wblog.Add(System.DateTime.Now.ToString() + " " + "User navigated event.");
                ncount--;

            }
            public void waitcontains(int cnt1,int cnt2)
            {
                
                //trytoconts = true;
                while (!(cnt1<=ojcnt&&cnt2<=rdycnt))
                {

                    Application.DoEvents();
                    continue;
                }
            }
            private void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
            {
                wbst.status = "Navigating";
                wbst.cnt = ncount;
               // wblog.Add(System.DateTime.Now.ToString() + " " + "User navigating event.");
                ncount++;
            }

            private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                
                if (this.wb.ReadyState != WebBrowserReadyState.Complete) return;
                
                   if (e.Url.AbsolutePath.ToString() != "blank" && ncount == 0)
                   {
                       wbst.status = "WBCmp";
                       wbst.cnt = ncount;
                    //   wblog.Add(System.DateTime.Now.ToString() + " " + "CompletelyDone!");
                       
                       
                   }
                   if (wb.StatusText == "Готово" || wb.StatusText=="Done")
                   {
                       wbst.status = "WBRdy";
                       wbst.cnt = ncount;
                       readycount++;
                   //    wblog.Add(System.DateTime.Now.ToString() + " " + "Ready!");
                   }
            }
            public void ShowAll()
            {


            }
            public WebBrowser getcontrol()
            {
                return wb;
            }
            public void wait_doneloading(int readycount)
            {
                while (readycount != this.readycount)
                {
                    Application.DoEvents();
                    continue;
                }
            }
            public string getdocument()
            {
                
                if (wb.Document != null && wb.Document.Body != null)
                      return  wb.Document.Body.Parent.OuterHtml;
                else
                    return "";
            }

        }
    }
}
