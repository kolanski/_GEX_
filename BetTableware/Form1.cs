using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using mshtml;
using Awesomium;
using Awesomium.Core;
using Gecko;
using System.Threading.Tasks;
namespace BetTableware
{
    public partial class Form1 : Form
    {
        public Form2 formsettings = new Form2();
        public Form3 formtesting = new Form3();
        public int i = 0;
        public bool prgstart = false;
        public System.Timers.Timer tmr;
        public bool refwb3i;
        public string script;
        public bool navigated365 = false;
        public Form1()
        {




            
            InitializeComponent();
            
            Gecko.Xpcom.Initialize(Application.StartupPath + "\\xulrunner");
            tmr = new System.Timers.Timer();
            tmr.Elapsed += new ElapsedEventHandler(elapsedtmr);

            tmr.Interval = Program.refinterval * 1000;
            tmr.Start();
            //webBrowser1.ScriptErrorsSuppressed = true;
            this.Text = Text + " Build:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            richTextBox3.Visible = false;
            //panel1.Controls.Add(richTextBox1);
            webControl1.Focus();
            refwb3i = false;
           geckoWebBrowser1.NSSError += (sender, eargs) =>
            {
                CertOverrideService.RememberRecentBadCert(eargs.Uri);
                Uri refUrl = geckoWebBrowser1.Url;
                geckoWebBrowser1.Navigate(eargs.Uri.AbsoluteUri, GeckoLoadFlags.FirstLoad, refUrl != null ? refUrl.AbsoluteUri : null, null);
                eargs.Handled = true;
            };
        }
        int count = 0;
        public bool gettested()
        {
            return this.checkBox1.Checked;
        }
        public void navigatewb1()
        {
            try
            {
                //string iss;
                var SRC = formsettings.textBoxBCSRC.Text;
                this.webControl1.Source = new Uri(SRC+"live/line.php");
                while (this.webControl1.IsLoading)
                    Application.DoEvents();
                //webControl1.ExecuteJavascript(String.Format("var list = document.getElementById('tab-now').getElementsByTagName('input');for(var i=0, il=list.length; i<il; i++) list[i].click();"));
                webControl1.ExecuteJavascript(String.Format("$('a.btn.f2').click();"));

                while (this.webControl1.IsLoading)
                    Application.DoEvents();

                // MessageBox.Show("lol");
            }
            catch
            {

            }
        }
        public void wb1refbut()
        {
            try
            {
                if (count == 1)
                {
                    if (webControl1.IsDocumentReady)
                        webControl1.ExecuteJavascript(String.Format("$('a.btn.refresh').click();"));
                    count = 0;
                }
            }
            catch
            {

            }

            while (this.webControl1.IsLoading)
                Application.DoEvents();
            count++;
        }
        public bool geterr1()
        {
            if (richTextBox1.Text.Contains("ParsingError"))
                return true;
            else
                return false;
        }
        public void waitwb1()
        {
            while (this.webControl1.IsLoading)
                Application.DoEvents();
        }
        public void navigatewb2()
        {
            var SRC = formsettings.textBoxFBSRC.Text;
            this.webControl2.Source = new Uri(SRC + "?locale=ru#4");

            while (webControl2.IsLoading)
                Application.DoEvents();
            this.geckoWebBrowser1.Navigate(SRC+"?locale=ru#4");

           /* using (AutoJSContext context = new AutoJSContext(geckoWebBrowser1.Window.JSContext))
            {
                string result;
                context.EvaluateScript("$('#sportsMenuContainer').find('div') .find(\'a:contains('Теннис')\').trigger('click');", out result);
            }*/
        }
        public async void navigatewb3()
        {
            var SRC = formsettings.textBoxMRSRC.Text;
            webControl3.Source = new Uri(SRC+"su/live/22723");
            // while (webControl3.IsLoading)
            //    Application.DoEvents();
            await Task.Delay(5000);
            //this.geckoWebBrowser1
            while (!webControl3.IsDocumentReady)
                Application.DoEvents();
            webControl3.ExecuteJavascript("$('#sportsMenuContainer').find('div') .find(\"a:contains('Теннис')\").trigger('click');");
            await Task.Delay(3000);
            webControl3.ExecuteJavascript("Markets.ONLY_ONE_EVENT_OPEN_AM=false;");
            //string xpath = "//*[@id='sportsMenuContainer']/div[contains(@data-sport-group,'Теннис')]/input";//*[@id="sportsMenuContainer"]/div[4] //*[@id="sportsMenuContainer"]/div[4]
            //JSObject shareBtn = webControl3.ExecuteJavascriptWithResult(String.Format(
            //    "document.evaluate(\"{0}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null ).singleNodeValue", xpath));

            // shareBtn.Invoke("click");
            // webControl3.ExecuteJavascript("showEvents('select_events_warning'); return false;");
            //webControl3.ExecuteJavascript("var list=document.getElementById('main_live_category_link_Теннис');if(list!=null&&!list.cheked)list.click();  showEvents('select_events_warning');list=null;");
            //   string xpath = ".//*[@id='container_AVAILABLE']/div[@data-category-sport='Футбол']/div[contains(@id, 'event')]/input";
            //webControl3.ExecuteJavascript("Markets.ONLY_ONE_EVENT_ADDITIONAL_MARKETS=false");
            // webControl3.ExecuteJavascript(String.Format("$('.event-more-view')[$('.event-more-view').length-2].click();"));
            webControl3.ExecuteJavascript(String.Format("$('.event-more-view').click();"));
        }
        public void navigatewb4()
        {
            webControl4.Invoke((MethodInvoker)delegate
            {
                var SRC = formsettings.textBoxZNSRC.Text;
                webControl4.Source = new Uri(SRC+"line/live/");
                while (webControl4.IsLoading)
                    Application.DoEvents();
                webControl4.ExecuteJavascript("gamesall.click();onlyview.click();document.getElementById('do').click();");
            });
        }
        public void navigatewb7()
        {
            webControl7.Invoke((MethodInvoker)delegate
            {
                //webControl7.Source = new Uri("http://new.winlinebet.com/stavki/sport/tennis/");
               // while (webControl7.IsLoading)
                //    Application.DoEvents();
                //webControl7.ExecuteJavascript("gamesall.click();onlyview.click();document.getElementById('do').click();");
            });

        }
        public void JsFireEvent(string getElementQuery, string eventName)
        {
            try
            {
                webControl5.Invoke((MethodInvoker)delegate
                {
                    webControl5.ExecuteJavascript(@"
                            function fireEvent(element,event) {
                                var evt = document.createEvent('HTMLEvents');
                                evt.initEvent(event, true, true ); // event type,bubbling,cancelable
                                element.dispatchEvent(evt);                                 
                            }
                            " + String.Format("fireEvent({0}, '{1}');", getElementQuery, eventName));
                });
            }
            catch
            {

            }
        }
        bool trynavigate = false;
        public void navigatewb6()
        {
            if (!trynavigate)
            {
                var SRC = formsettings.textBoxOLSRC.Text;
                trynavigate = true;
                this.webControl6.Source = new Uri(SRC+"index.php?action=setlang&id=0");
                while (webControl6.IsLoading)
                    Application.DoEvents();
                this.webControl6.Source = new Uri(SRC+"index.php?page=line&action=1&live=1");
                while (webControl6.IsLoading)
                    Application.DoEvents();
                addtext6("Olimp Loaded");
                addtext6("Setuping");
                webControl6.ExecuteJavascript(@" var cnt = 0;
                    var alldata = document.getElementsByClassName('smallwnd3')[0].getElementsByTagName('td');
                    for (var i = 0; i < alldata.length; i++) {
                        if (alldata[i].textContent.indexOf('Теннис') != -1) {
                            alldata[i].children[1].click();
                            cnt++;
                        }
                    }                    ;
                    
");
                webControl6.ExecuteJavascript("document.getElementsByClassName('msbtn1')[3].click();");
            }
            trynavigate = false;
        }
        public void navigatewb5()
        {
            try
            {
                this.webControl5.Source = new Uri("https://mobile.bet365.com/Preferences/");
                while (webControl5.IsLoading)
                    Application.DoEvents();
                //  addtext6("loaded");
                //  addtext6("setup");
                if (webControl5.IsDocumentReady)
                    webControl5.ExecuteJavascript("var e = document.getElementById('preflanguage');var e2 = document.getElementById('prefoddstype');if(e.options[e.selectedIndex].text=='Български')console.log('passLanguage');else e.selectedIndex=9;if(e2.options[e.selectedIndex]==2)return 'Passoddstype';else e2.selectedIndex=1;document.getElementById('prefsubmit').click();");
                while (webControl5.IsLoading)
                    webControl5.Update();
                //   webControl5.ExecuteJavascript("document.getElementById('bottomMenu').children[2].children[0].click();");

                //   webControl5.ExecuteJavascript("document.getElementById('bottomMenu').children[2].children[0].click();");
                //Thread.Sleep(500);
                while (webControl5.IsLoading)
                    Application.DoEvents();
                Thread t = new Thread(() =>
                {
                    Thread.Sleep(1000);
                    JsFireEvent("document.getElementById(\"bottomMenu\").children[1].children[0]", "click");
                    //  webControl5.ExecuteJavascript("var e = document.getElementById('preflanguage');var e2 = document.getElementById('prefoddstype');if(e.options[e.selectedIndex].text=='Български')console.log('passLanguage');else e.selectedIndex=9;if(e2.options[e.selectedIndex]==2)return 'Passoddstype';else e2.selectedIndex=1;document.getElementById('prefsubmit').click();");
                    // addtext6("DoneIter");
                });
                t.Start();

                //  Thread.Sleep(1000);
                Thread t1 = new Thread(() =>
                {
                    Thread.Sleep(1000);
                    JsFireEvent("document.getElementById(\"splash\").children[2].children[0]", "click");
                    //  addtext6("DoneIter");
                });
                while (t.IsAlive)
                    Application.DoEvents();
                t1.Start();
                while (t1.IsAlive)
                    Application.DoEvents();
                string sResult = webControl5.ExecuteJavascriptWithResult("document.getElementById('menuItem_3');");
                // addtext6(sResult);
                navigated365 = true;
            }
            catch
            {

            }
        }
        public void refwb4()
        {
            webControl4.Invoke((MethodInvoker)delegate
            {
                webControl4.ExecuteJavascript("$('#reloadLine').click();");
            });

        }
        public async void refwb3()
        {
            // webControl3.ExecuteJavascript("Markets.ONLY_ONE_EVENT_ADDITIONAL_MARKETS=false");
            //  webControl3.ExecuteJavascript("var list=$x('.//*[@id=\"container_AVAILABLE\"]/div[@data-category-sport=\"Теннис\"]/div[contains(@id, \"event\")]/input');if(list!=null){for(i=0;i<list.length;i++){if(!list[i].Checked)list[i].click();};}");
            await Task.Delay(3000);
            webControl3.ExecuteJavascript("Markets.ONLY_ONE_EVENT_OPEN_AM=false;");
            webControl3.ExecuteJavascript("Markets.applyView=function (c){var d=Markets.getLiveUpdatesHelper();if(d){d.fireAdditionalMarketsChanged();}var b=new EventView(c);if(this.isLoading(b)){return;}if(b.isVisible()){}else{b.show();}}");
            webControl3.ExecuteJavascript(String.Format("$('.event-more-view').click();"));
            //while (webControl3.IsLoading)
            //    Application.DoEvents();
            await Task.Delay(3000);
            refwb3i = false;
            //string lol = webControl3.ExecuteJavascriptWithResult("document.getElementsByAttribute('treeid');");

            //webControl3.ExecuteJavascript(String.Format("$('.event-more-view')[$('.event-more-view').length-1].click();"));

            // webControl3.ExecuteJavascript("var list=document.getElementById('main_live_category_link_Теннис');if(list!=null&&!list.cheked)list.click();  showEvents('select_events_warning');list=null;");
        }
        public void getlast()
        {

        }
        public string getdocwb4()
        {
            string lol = "";
            try
            {
                webControl4.Invoke((MethodInvoker)delegate
            {
                lol = this.webControl4.ExecuteJavascriptWithResult("document.getElementsByTagName('html')[0].innerHTML");
            });
            }
            catch
            {
                lol = "<html></html>";
            }
            return lol;
        }
        public string cookies1;
        public string current_page;
        public List<string> getdocwb5()
        {
            if (navigated365)
                try
                {


                    List<string> temp = new List<string>();
                    List<string> links = new List<string>();
                    var i = 3;
                    string sResult = "";
                    string cookies = "";
                    if (webControl5.IsDocumentReady)
                    {
                        webControl5.Invoke((MethodInvoker)delegate
                        {
                            try
                            {
                                string sresult = webControl5.ExecuteJavascriptWithResult(" document.getElementById('subMenu_2').childElementCount");
                                if (sresult != "undefined")
                                    for (i = 1; i < int.Parse(sresult) - 2; i++)
                                    {
                                        webControl5.ExecuteJavascript("var i=" + i);
                                        sResult = webControl5.ExecuteJavascriptWithResult("document.links[i].href");
                                    //  addtext6(sResult.ToString());
                                    links.Add(sResult.ToString());
                                    }
                            }
                            catch
                            {

                            }
                        });
                        webControl5.Invoke((MethodInvoker)delegate
                        {
                            cookies = webControl5.ExecuteJavascriptWithResult("document.cookie");

                        //addtext6(cookies);
                        cookies1 = cookies;
                            var url = sResult;
                            current_page = webControl5.ExecuteJavascriptWithResult("document.URL");
                            var html = "";
                            foreach (string url_t in links)
                            {
                                Uri myUri = new Uri(url_t);
                            // Create a 'HttpWebRequest' object for the specified url. 
                            // Thread.Sleep(1000);
                            CookieContainer cooks = new CookieContainer();
                                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                                myHttpWebRequest.Host = "mobile.bet365.com";
                            //     myHttpWebRequest.Connection = "keep-alive";
                            myHttpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                                myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1876.0 Safari/537.36";
                                myHttpWebRequest.Referer = current_page;
                                WebHeaderCollection myWebHeaderCollection = myHttpWebRequest.Headers;
                                myWebHeaderCollection.Add("Accept-Language:ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                                myWebHeaderCollection.Add("Accept-Encoding:gzip,deflate,sdch");
                            //   myHttpWebRequest.CookieContainer = cooks;
                            myHttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                                myHttpWebRequest.Headers["Cookie"] = cookies1;
                            // myHttpWebRequest.CookieContainer.Add(new C);
                            // Set the user agent as if we were a web browser
                            // myHttpWebRequest.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4";
                            try
                                {
                                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                                    var stream = myHttpWebResponse.GetResponseStream();
                                    var reader = new StreamReader(stream);
                                    html = reader.ReadToEnd();
                                // Release resources of response object.
                                myHttpWebResponse.Close();
                                // myHttpWebRequest.e
                            }
                                catch
                                {

                                }
                                temp.Add(html);
                                addtext6("Found Game");
                            }
                        });
                    }
                    return temp;
                }
                catch (Exception e)
                {
                    addtext6(e.ToString());
                    return new List<string>();
                }
            else
                return new List<string>();

        }
        public string use()
        {
            string lol = "";
            webControl3.Invoke((MethodInvoker)delegate
            {
                lol = this.webControl3.ExecuteJavascriptWithResult("document.getElementsByTagName('html')[0].innerHTML").ToString();
            });
            return lol;
        }
        public string getdocwb3()
        {
            this.webControl3.ExecuteJavascript("if(document.getElementsByClassName('details-description').length =< 1)$('.event-more-view').click();");
            string lol;
            string tes;
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            // webControl3.Refresh();
            // webControl3.Select(); 
            // webControl3.SelectAll();
            // lol = webControl3.Selection.HTML;

            tes = sw2.ElapsedMilliseconds.ToString();
            //  addtext3("test1:" + tes);
            sw2.Restart();

            //webControl3.SelectAll();

            // lol = webControl3.Selection.HTML;

            //webControl3.ExecuteJavascript("") ;


            lol = this.webControl3.ExecuteJavascriptWithResult("document.getElementsByTagName('html')[0].innerHTML").ToString();

            tes = sw2.ElapsedMilliseconds.ToString();
            // addtext3("test2:" + tes);
            //    webControl3.ResetText();
            //    webControl3.Select();
            sw2.Stop();
            sw2.Reset();
            bool done = false;

            if (lol != null)
            {
                if (lol.Length > 24)
                {
                    addtext4("good ");
                    done = true;
                    // richTextBox1.AppendText(webControl1.IsAccessible.ToString() + " " + webControl1.IsDocumentReady + " " + webControl1.IsLive + " " + webControl1.IsLoading + " " +webControl1.CanSelect+ webControl1.IsResponsive+System.Environment.NewLine);
                }
                else
                {
                    Thread t = new Thread(() =>
                    {
                        while (lol.Length < 24)
                        {
                            addtext4("bad  ");
                            //richTextBox1.AppendText(webControl1.IsAccessible.ToString() + " " + webControl1.IsDocumentReady + " " + webControl1.IsLive + " " + webControl1.IsLoading + " " + webControl1.CanSelect + webControl1.IsResponsive + System.Environment.NewLine);
                            Thread.Sleep(500);
                            lol = use();
                        }
                        done = true;
                        addtext4("good");
                    });
                    t.Start();
                    while (!done)
                    {
                        Application.DoEvents();
                    }
                    addtext4("done");
                }
            }
            else
            {
                richTextBox1.AppendText(webControl1.IsAccessible.ToString() + " " + webControl1.IsDocumentReady + " " + webControl1.IsLive + " " + webControl1.IsLoading + " " + webControl1.IsResponsive);
            }
            string doctext = lol;
            string classdata = "";
            bool found = false;
            try
            {
                HtmlAgilityPack.HtmlDocument ParDoc = new HtmlAgilityPack.HtmlDocument();
                ParDoc.LoadHtml(doctext);
                HtmlAgilityPack.HtmlNodeCollection list = ParDoc.DocumentNode.SelectNodes("//*[@id='container_AVAILABLE']/div");
                if (list != null)
                    for (int i = 0; i < list.Count; i++)
                    {
                        HtmlAgilityPack.HtmlNode hn = list[i];
                        try
                        {
                            if (hn.Attributes.Count >= 2 && hn.Attributes[1].Value == "Теннис")
                            {
                                classdata = hn.InnerText;
                            }
                            // break;
                        }
                        catch
                        {
                        }
                        if (classdata.Contains("Теннис.") && !classdata.Contains("Настольный"))
                        {

                            addtext4("rush");

                            found = true;
                            refwb3i = found;
                            break;
                        }
                    }
                if (!found)
                {
                    addtext4("Hyi");
                }
                else
                {
                    addtext("new games!");
                    navigatewb3();

                }
            }
            catch (Exception e)
            {
                addtext("autoupdater error" + e.ToString());
            }
            return lol;
        }
        public string getdocwb2()
        {
            string lol = "";
            if (!this.formsettings.isFonbet())
            {
                dynamic list = geckoWebBrowser1.Document.GetElementsByClassName("detailArrowClose");
                bool process = true;
                while (process)
                {
                    list = geckoWebBrowser1.Document.GetElementsByClassName("detailArrowClose");
                    var test = list.Length;
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i].Click();
                    }
                    if (test == list.Length)
                        process = false;
                    Console.Write("list:" + list.Length + System.Environment.NewLine);
                }
                label1.Text = "done";
            }
            // if (webControl2.IsDocumentReady)
            //  lol = this.webControl2.ExecuteJavascriptWithResult("document.getElementsByTagName('html')[0].innerHTML");
            return lol;

        }

        public string getdocwb1()
        {
            try
            {


                Awesomium.Core.JSValue lol;
                lol = this.webControl1.ExecuteJavascriptWithResult("document.getElementsByTagName('html')[0].innerHTML");
                return lol.ToString();
                //return lol.ToString();
            }
            catch
            {
                return "";
            }
        }
        public void setcolor1()
        {
            button1.BackColor = Color.Gray;
            button2.BackColor = Color.Blue;
        }
        public void openmf()
        {
            webControl3.Width = 360;
            webControl3.Height = 480;
        }
        private void elapsedtmr(object sender, EventArgs e)
        {
            if (button3.Enabled && prgstart)
            {
                addtext3(System.DateTime.Now.ToString() + "Refresh");
                Invoke((MethodInvoker)delegate
                {
                    refreshevnt();
                });

            }
            // Program.start();
        }
        public void setcolor2()
        {
            button2.BackColor = Color.Gray;
            button1.BackColor = Color.Blue;
        }
        public void clearall()
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            //richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
        }
        public void clear3()
        {
            richTextBox3.Invoke((MethodInvoker)delegate
            {
                richTextBox3.Clear();
            });
        }
        public void addtext(string text)
        {

            if (!Program.debugthread && !checkBox2.Checked)
                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText(System.DateTime.Now.ToString() + " " + "Added text." + text + "\n");
                });
        }

        public void addtext2(string text)
        {
            if (!Program.debugthread && !checkBox2.Checked)
                richTextBox2.Invoke((MethodInvoker)delegate
                {
                    richTextBox2.AppendText(System.DateTime.Now.ToString() + " " + "Added text." + text + "\n");
                });

        }
        public void addtext5(string text)
        {
            if (!Program.debugthread && !checkBox2.Checked)
                richTextBox5.Invoke((MethodInvoker)delegate
                {
                    richTextBox5.AppendText(System.DateTime.Now.ToString() + " " + "Added text." + text + "\n");
                });

        }
        public void select()
        {
            if (!Program.debugthread)
                richTextBox3.Invoke((MethodInvoker)delegate
                {
                    int textEnd = richTextBox3.TextLength;
                    Font fnt = new Font("Tahoma", 14, FontStyle.Bold);
                    int index = 0;
                    int lastIndex = richTextBox3.Text.LastIndexOf("Fonbet"); ; //textbox1.text is the text to find
                    while (index < lastIndex)
                    {
                        richTextBox3.Find("Fonbet", index, textEnd, RichTextBoxFinds.None);
                        richTextBox3.SelectionColor = Color.Blue;  //set the color to blue if found
                        richTextBox3.SelectionFont = fnt; //bold the text if found
                        index = richTextBox3.Text.IndexOf("Fonbet", index) + 1;
                    }
                    index = 0;
                    lastIndex = richTextBox3.Text.LastIndexOf("Betcity"); ; //textbox1.text is the text to find
                    while (index < lastIndex)
                    {
                        richTextBox3.Find("Betcity", index, textEnd, RichTextBoxFinds.None);
                        richTextBox3.SelectionColor = Color.Green;  //set the color to blue if found
                        richTextBox3.SelectionFont = fnt; //bold the text if found
                        index = richTextBox3.Text.IndexOf("Betcity", index) + 1;
                    }
                    index = 0;
                    lastIndex = richTextBox3.Text.LastIndexOf("Maraphon"); ; //textbox1.text is the text to find
                    while (index < lastIndex)
                    {
                        richTextBox3.Find("Maraphon", index, textEnd, RichTextBoxFinds.None);
                        richTextBox3.SelectionColor = Color.DarkOrange;  //set the color to blue if found
                        richTextBox3.SelectionFont = fnt; //bold the text if found
                        index = richTextBox3.Text.IndexOf("Maraphon", index) + 1;
                    }
                    index = 0;
                    lastIndex = richTextBox3.Text.LastIndexOf("Zenit"); ; //textbox1.text is the text to find
                    while (index < lastIndex)
                    {
                        richTextBox3.Find("Zenit", index, textEnd, RichTextBoxFinds.None);
                        richTextBox3.SelectionColor = Color.DarkRed;  //set the color to blue if found
                        richTextBox3.SelectionFont = fnt; //bold the text if found
                        index = richTextBox3.Text.IndexOf("Zenit", index) + 1;
                    }

                    index = 0;
                    lastIndex = richTextBox3.Text.LastIndexOf("Olimp"); ; //textbox1.text is the text to find
                    while (index < lastIndex)
                    {
                        richTextBox3.Find("Olimp ", index, textEnd, RichTextBoxFinds.None);
                        richTextBox3.SelectionColor = Color.Brown;  //set the color to blue if found
                        richTextBox3.SelectionFont = fnt; //bold the text if found
                        index = richTextBox3.Text.IndexOf(" Olimp ", index) + 1;
                    }
                    richTextBox3.SelectionLength = 0;
                    /* foreach (string text in richTextBox3.Lines)
                     {
                         int end = 0;
                         if (text.Contains(" Fonbet "))
                         {
                             // start = richTextBox3.Find(text);
                             end = text.Length;
                            // richTextBox3.SelectionStart = text.IndexOf("Fonbet");
                            // richTextBox3.SelectionLength = "Fonbet".Length;
                             richTextBox3.Select(text.IndexOf(" Fonbet "), " Fonbet ".Length);
                             richTextBox3.SelectionColor = Color.DarkCyan;
                             richTextBox3.SelectionFont = new Font("Tahoma", 14, FontStyle.Bold);

                         }
                         if (text.Contains(" Betcity "))
                         {
                             //start = richTextBox3.Find(text, 0, text.Length, RichTextBoxFinds.None);
                             end = text.Length;
                             richTextBox3.Select(text.IndexOf("Betcity"), "Betcity".Length);
                             richTextBox3.SelectionColor = Color.Aqua;
                             richTextBox3.SelectionFont = new Font("Tahoma", 14, FontStyle.Bold);
                         }

                         if (text.Contains(" Maraphon "))
                         {
                             //start = richTextBox3.Find(text);
                             end = text.Length;
                           //  richTextBox3.SelectionStart = text.IndexOf("Maraphon");
                           //  richTextBox3.SelectionLength = 200;
                             richTextBox3.Select(text.IndexOf("Maraphon"), "Maraphon".Length);
                             richTextBox3.SelectionColor = Color.Coral;
                             richTextBox3.SelectionFont = new Font("Tahoma", 14, FontStyle.Bold);
                         }
                     }*/
                });
        }
        public void addtext3(string text)
        {

            richTextBox3.Invoke((MethodInvoker)delegate
            {
                richTextBox3.AppendText(text + "\n");



            });

        }
        public void addtext4(string text)
        {
            if (!Program.debugthread && !checkBox2.Checked)
                richTextBox4.Invoke((MethodInvoker)delegate
                {
                    richTextBox4.AppendText(System.DateTime.Now.ToString() + " " + text + "\n");
                });

        }
        public void addtext6(string text)
        {
            if (!Program.debugthread)
                richTextBox6.Invoke((MethodInvoker)delegate
                {
                    richTextBox6.AppendText(System.DateTime.Now.ToString() + " " + text + "\n");
                });

        }
        public void show3()
        {
            richTextBox1.Invoke((MethodInvoker)delegate
            {
                richTextBox1.Visible = false;
            });
            richTextBox2.Invoke((MethodInvoker)delegate
            {
                richTextBox2.Visible = false;
            });
            richTextBox4.Invoke((MethodInvoker)delegate
            {
                richTextBox4.Visible = false;
            });
            richTextBox3.Invoke((MethodInvoker)delegate
            {
                richTextBox3.Visible = true;
            });
            panel1.Invoke((MethodInvoker)delegate
            {
                panel1.Visible = false;
            });
            setcolor2();
        }
        public void shownorm()
        {
            richTextBox1.Visible = true;
            richTextBox2.Visible = true;
            richTextBox4.Visible = true;
            richTextBox3.Visible = false;
            panel1.Visible = true;
            setcolor1();

        }
        public void savetext(string text)
        {
            StreamWriter writeSiteInfo = new StreamWriter("Bets.html");
            writeSiteInfo.WriteLine(text);
            writeSiteInfo.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //richTextBox1.AppendText(System.DateTime.Now.ToString() + " " + "Starting betcity."+"\n");
            // Program.start();
            setcolor1();
            button3.Enabled = true;
            // richTextBox1.AppendText(System.DateTime.Now.ToString() + " " + "End betcity." + "\n");
            //  richTextBox1.AppendText(System.DateTime.Now.ToString() + Program.PBC.ParWB.wb.DocumentText.ToString() + "End betcity." + "\n");
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            show3();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            shownorm();
        }
        public void setactive()
        {
            button3.Invoke((MethodInvoker)delegate
            {
                button3.Enabled = true;
            });
            /* richTextBox1.Invoke((MethodInvoker)delegate
             {
                 if (richTextBox1.Text.Contains("ParsingError"))
                 {
                     refreshevnt();
                 }
             });
            */
        }
        public void setnotactive()
        {
            button3.Invoke((MethodInvoker)delegate
            {
                button3.Enabled = false;
            });
        }
        private void refreshevnt()
        {
            if (!prgstart)
            {
                setnotactive();
                addtext("Init Betcity  ...Please Wait");
                addtext2("Init Fonbet  ...Please Wait");
                addtext4("Init Maraphon...Please Wait");
                addtext5("Init Zenit   ...Please Wait");
                addtext6("Init Olimp ...Please Wait");
                Program.init();
                Program.start();
                prgstart = true;
            }
            else
            {
                setnotactive();
                i++;
                if (i == 5)
                {

                    Program.reff = true;
                }
                Program.refresh();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

            //Program.hz();
            refreshevnt();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void настройкиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            formsettings.Show();
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromControl(this);
            Console.Write(this.Bounds.X.ToString() + this.Bounds.Y + ":" + System.Windows.Forms.Screen.FromControl(this).DeviceName);
            //formtesting.SetDesktopLocation(this.Bounds.X, this.Bounds.Y);
            int nTaskBarHeight = screen.Bounds.Bottom - screen.WorkingArea.Bottom;
            formtesting.Location = Program.MainForm.Location;
            formtesting.SetDesktopLocation(System.Windows.Forms.Screen.FromControl(this).Bounds.X + System.Windows.Forms.Screen.FromControl(this).Bounds.Width - formtesting.Size.Width, System.Windows.Forms.Screen.FromControl(Program.MainForm).Bounds.Height - formtesting.Size.Height - nTaskBarHeight);
            formtesting.Show();


        }
        public Rectangle GetScreen()
        {
            return System.Windows.Forms.Screen.FromControl(this).Bounds;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            formtesting.richTextBox1.Text = richTextBox3.Text;
        }

        private void Awesomium_Windows_Forms_WebControl_ShowCreatedWebView(object sender, ShowCreatedWebViewEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.saveData();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}