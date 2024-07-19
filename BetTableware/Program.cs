using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using BTware_TestParsings;
using System.Media;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;

namespace BetTableware
{
    public delegate void testevent1();
    public class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// 
        /// public static MainForm MainForm
        /// 
        public static int loopcount = 0;
        public static int soundcount = 0;
        public static int cnt = 0;
        public static bool reff = false;
        public static bool debugthread = false;
        public static Stopwatch sw = new Stopwatch();
        public static bool startbc = true;
        public static bool soundplay = true;
        public static int refinterval=5;
        public static Form1 MainForm;
        public static HTMLParcBC PARSBETCITY=new HTMLParcBC();
        public static Fonbet PARSEFONBET=new Fonbet();
        public static BETMarafon PARSEMARAPHON = new BETMarafon();
        public static BETZenit PARSEZENIT = new BETZenit();
        public static BET365 PARSEBET365 = new BET365();
        public static Olimp PARSEOLIMP = new Olimp();
        public static SoundPlayer player = new SoundPlayer(Properties.Resources.Triangle);
        public static SoundPlayer player1 = new SoundPlayer(Properties.Resources.bubble);
        //public static ParserBC PBC=new ParserBC();
        //public static Parser2BC PBC = new Parser2BC();
       // public static ParcerFB PFB=new ParcerFB();
       // public static ParcerMF PMF = new ParcerMF();
        public static BetSearcher betsearch=new BetSearcher();
        public static BetSearcher betsearch1 = new BetSearcher();
        public static BetSearcher betsearch2 = new BetSearcher();
        public static BetSearcher betsearch3 = new BetSearcher();
        public static BetSearcher betsearch4 = new BetSearcher();
        public static BetSearcher betsearch5 = new BetSearcher();
        public static BetSearcher betsearch6 = new BetSearcher();
        public static BetSearcher betsearch7 = new BetSearcher();
        public static int test=0;
        public static int test2 = 0;
        public static int counter = 0;
        public static event testevent1 eventadd;
        public static event testevent1 eventsearch;
        public static int evcnt = 0;
        public static string sa ="";
       [STAThread]

       public  static void Main()
        {
            Console.WriteLine("Datetime:" + GetNistTime().Month+" "+GetNistTime().Day);
            Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(true);
            // betsearch = new BetSearcher();
            MainForm = new Form1();
            //  MainForm.Show();
            eventadd += new testevent1(handlerevadd);
            eventsearch += new testevent1(handlerevsearch);
            SetProcessDPIAware();
            if (GetNistTime().Month <10)
                Application.Run(Program.MainForm);
            else
                MessageBox.Show("Программа нуждается в обновлении, запросите новую версию");
        }
       [System.Runtime.InteropServices.DllImport("user32.dll")]
       private static extern bool SetProcessDPIAware();
       public static DateTime GetNistTime()
       {
           DateTime dateTime = DateTime.MinValue;

           HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
           request.Method = "GET";
           request.Accept = "text/html, application/xhtml+xml, */*";
           request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
           request.ContentType = "application/x-www-form-urlencoded";
           request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore); //No caching
           HttpWebResponse response = (HttpWebResponse)request.GetResponse();
           if (response.StatusCode == HttpStatusCode.OK)
           {
               StreamReader stream = new StreamReader(response.GetResponseStream());
               string html = stream.ReadToEnd();//<timestamp time=\"1395772696469995\" delay=\"1395772696469995\"/>
               string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
               double milliseconds = Convert.ToInt64(time) / 1000.0;
               dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
           }

           return dateTime;
       }
       public static int getref()
       {
           return refinterval;
       }
       public static void setref(int reffs)
       {
           refinterval = reffs;
           if (MainForm != null)
           {
               MainForm.tmr.Stop();
               MainForm.tmr.Interval = refinterval * 1000;
               MainForm.tmr.Start();
           }
       }
       static void handlerevadd()
       {
           evcnt++;
           if (evcnt == 4||evcnt>8)
               eventsearch();
       }
       static void handlerevsearch()
       {
          // MainForm.show3();
           loop();
           evcnt = 0;
       }
        public static void threadtest()
        {
           // MainForm.addtext("Complete");
            
           // PBC.navigate();
            //PBC.ParWB.ShowAll();
            MainForm.addtext("Starting betcity");
            MainForm.addtext("Complete");
            PARSBETCITY.setparsecurr(MainForm.formsettings.parsecurr());
                // MainForm.addtext("Complete");
                
                //PBC.ParWB.ShowAll();

                //File.WriteAllText("livebetcity.html", PBC.ParWB.getdocument(), Encoding.GetEncoding(PBC.ParWB.wb.Document.Encoding));
                //if (PBC.ParWB.getdocument() != null)
               // {
                    //PARSBETCITY.Loadpage("livebetcity.html");
                    //PARSBETCITY.Loadpage1(PBC.ParWB.getdocument(), Encoding.GetEncoding(PBC.ParWB.wb.Document.Encoding));
                    lock (PARSBETCITY)
                    {
                        PARSBETCITY.ParceDoc();
                    }
                    for (int i = 0; i < PARSBETCITY.log.els; i++)
                    {
                        MainForm.addtext(PARSBETCITY.log.ToPrint(i));
                        Application.DoEvents();
                    }
                    test++;
                    test2++;
                    if (eventadd != null)
                        eventadd();
                    //MainForm.savetext("Complete");
                    reff = false;
              //  }
                startbc = false;
        }

        public static void init()
        {
            //PFB.navigate();
            MainForm.navigatewb3();
            MainForm.navigatewb2();
            MainForm.navigatewb4();
            PARSEFONBET.loadpage1(MainForm.getdocwb2());
            PARSEFONBET.Parcedoc();
            MainForm.navigatewb1();
            PARSBETCITY.Loadpage2(MainForm.getdocwb1());
            PARSEMARAPHON.loadpage1(MainForm.getdocwb3());
            //PARSEZENIT.LoadS(MainForm.getdocwb4());
            //PARSEZENIT.ParseDoc();
            MainForm.refwb3();
            MainForm.navigatewb5();
            MainForm.navigatewb6();
            PARSEOLIMP.loadpage(MainForm.webControl6);
            //PARSEMARAPHON.loadpage1(MainForm.getdocwb3());
           // PARSBETCITY.ParceDoc();
        }
        public static void refbet()
        {
            PARSBETCITY.setparsecurr(MainForm.formsettings.parsecurr());
            if (!MainForm.formsettings.isBetcity())
            {
                PARSBETCITY.Loadpage2(MainForm.getdocwb1());
                PARSBETCITY.ParceDoc();
            }
            else
            {
                PARSBETCITY.Loadpage2("");
                PARSBETCITY.ParceDoc();
            }
            for (int i = 0; i < PARSBETCITY.log.els; i++)
            {
                MainForm.addtext(PARSBETCITY.log.ToPrint(i));
                Application.DoEvents();
            }
        }
        public static void refthread()
        {
            MainForm.addtext("Starting betcity");
            MainForm.addtext("Complete");
            PARSBETCITY.setparsecurr(MainForm.formsettings.parsecurr());
            // MainForm.addtext("Complete");

            //PBC.ParWB.ShowAll();

            //File.WriteAllText("livebetcity.html", PBC.ParWB.getdocument(), Encoding.GetEncoding(PBC.ParWB.wb.Document.Encoding));

                //PARSBETCITY.Loadpage("livebetcity.html");
            if (test > 20)
            {
                MainForm.navigatewb1();
                Thread.Sleep(500);
                test = 0;
                MainForm.addtext("test" + test.ToString());
                MainForm.waitwb1();
            }
            if (!MainForm.formsettings.isBetcity())
            {
                PARSBETCITY.Loadpage2(MainForm.getdocwb1());
                lock (PARSBETCITY)
                {
                    
                    PARSBETCITY.ParceDoc();
                }
            }
            else
            {
                PARSBETCITY.Loadpage2("");
                lock (PARSBETCITY)
                {
                    PARSBETCITY.ParceDoc();
                }
            }
                for (int i = 0; i < PARSBETCITY.log.els; i++)
                {
                    MainForm.addtext(PARSBETCITY.log.ToPrint(i));
                    Application.DoEvents();
                }
                test++;
                test2++;
                if (eventadd != null)
                    eventadd();
                //MainForm.savetext("Complete");
                reff = false;
                MainForm.wb1refbut();
        }
               

        public static void threadfontest()
        {
            MainForm.addtext2("Starting fonbet");
            //PFB.navigate();
            MainForm.addtext2("Complete");
            //File.WriteAllText("fonbet.htm", PFB.ParWB.wb.Document.Body.Parent.OuterHtml, Encoding.GetEncoding(PFB.ParWB.wb.Document.Encoding));
            //PARSEFONBET.loadpage("fonbet.htm");
            PARSEFONBET.parsecurr = MainForm.formsettings.parsecurr();
                lock (PARSEFONBET)
                {
                   
                }
                for (int i = 0; i < PARSEFONBET.log.els; i++)
                {
                    MainForm.addtext2(PARSEFONBET.log.ToPrint(i));
                    Application.DoEvents();
                }
                test++;
                if (eventadd != null)
                    eventadd();
        }
        public static void threadmarafon()
        {
            MainForm.addtext4("Starting maraphon");
            MainForm.addtext("Complete");
            PARSEMARAPHON.setparsecur(MainForm.formsettings.parsecurr());
            lock (PARSEMARAPHON)
            {
                //PARSEMARAPHON.loadpage1(MainForm.getdocwb3());
                PARSEMARAPHON.Pardoc();
            }
         //   for (int i = 0; i < PARSEMARAPHON.log.els; i++)
         //   {
                MainForm.addtext4(PARSEMARAPHON.log.ToString());
                Application.DoEvents();
         //   }
            eventadd();

        }
        public static void threadzenit()
        {
            MainForm.addtext5("Starting zenit");
            MainForm.refwb4();
            MainForm.addtext5("Complete");
            PARSEZENIT.setparsecurr(MainForm.formsettings.parsecurr());
            try
            {
                if (!MainForm.formsettings.isZenit())
                lock (PARSEZENIT)
                {
                    //PARSEMARAPHON.loadpage1(MainForm.getdocwb3());
                    PARSEZENIT.LoadS(MainForm.getdocwb4());
                    PARSEZENIT.ParseDoc();
                }
                else
                {
                    PARSEZENIT.LoadS("");
                    PARSEZENIT.ParseDoc();
                }
                for (int i = 0; i < PARSEZENIT.log.els; i++)
                {
                    MainForm.addtext5(PARSEZENIT.log.ToPrint(i));
                    Application.DoEvents();
                }
            }
            catch
            {
                MainForm.addtext5("Error:(");
            }
            eventadd();
        }
        public static bool needrefolimp = false;
        public static void threadBet365()
        {
            MainForm.addtext6("Starting Olimp");
            lock (PARSEOLIMP)
            {
                if (PARSEOLIMP.myweb != null)
                    PARSEOLIMP.Parsedoc();
                else
                    PARSEOLIMP.loadpage(MainForm.webControl6);
            }
            MainForm.addtext6(PARSEOLIMP.olimpTdata.printall());
            if (PARSEOLIMP.olimpTdata.Count < 1)
                needrefolimp = true;
        }
        public static void refmarafon()
        {
            MainForm.addtext4("Starting maraphon");
            MainForm.addtext("Complete");
            PARSEMARAPHON.setparsecur(MainForm.formsettings.parsecurr());
            lock (PARSEMARAPHON)
            {
                if (!MainForm.formsettings.isMarafon())
                {
                    PARSEMARAPHON.loadpage1(MainForm.getdocwb3());
                    PARSEMARAPHON.Pardoc();
                }
                else
                {
                    PARSEMARAPHON.loadpage1("");
                    PARSEMARAPHON.Pardoc();
                }
            }
            for (int i = 0; i < PARSEMARAPHON.log.els; i++)
            {
                MainForm.addtext4(PARSEMARAPHON.log.ToPrint(i));
                Application.DoEvents();
            }
            if (PARSEMARAPHON.TableCount > 3 && PARSEMARAPHON.DatasCount < (PARSEMARAPHON.TableCount - 2))
            {
                MainForm.addtext4("Updating action in progress....."+cnt);

                cnt++;
            }
            eventadd();
           /* if (MainForm.geterr1() && !MainForm.formsettings.isBetcity())
            {
                MainForm.navigatewb1();
                MainForm.clear3();
                betsearch.Forklog = new logS();
                betsearch1.Forklog = new logS();
                betsearch2.Forklog = new logS();
                loop();
            }*/
        }
        public static void refreshfontest()
        {
            MainForm.addtext2("Refreshing fonbet");
            //File.WriteAllText("fonbet.htm", PFB.ParWB.wb.Document.Body.Parent.OuterHtml, Encoding.GetEncoding(PFB.ParWB.wb.Document.Encoding));
            PARSEFONBET.parsecurr = MainForm.formsettings.parsecurr(); 
            if (!MainForm.formsettings.isFonbet())
            {
                PARSEFONBET.loadpage1(MainForm.getdocwb2());
                PARSEFONBET.loadpage(MainForm.geckoWebBrowser1);
                PARSEFONBET.Parcedoc();
                PARSEFONBET.Parcedoc1();
            }
            else
            {
                PARSEFONBET.loadpage1("");
                PARSEFONBET.Parcedoc();
            }
            for (int i = 0; i < PARSEFONBET.log.els; i++)
            {
                MainForm.addtext2(PARSEFONBET.log.ToPrint(i));
                Application.DoEvents();
            }
            test++;
           // if (eventadd != null)
                eventadd();
        }
        public static int cntr = 0;
        public static void start()
        {
           // PBC = new ParserBC();
           // PFB = new ParcerFB();
           // betsearch = new BetSearcher();
            Thread oTh = new Thread(threadtest);
            Thread o2 = new Thread(threadfontest);
            Thread o3 = new Thread(threadmarafon);
            Thread o4 = new Thread(threadzenit);
            Thread o5 = new Thread(threadBet365);
            //o2.SetApartmentState(ApartmentState.STA);
            //oTh.SetApartmentState(ApartmentState.STA);
            o2.Start();
            oTh.Start();
            o3.Start();
            o4.Start();
            o5.Start();
        }
        public static void setplaysound(bool set)
        {
            soundplay = set;
        }
        
        public static void refresh()
        {
            evcnt = 0;
            MainForm.clearall();
            PARSBETCITY.TData1= new tennisData();
            PARSEFONBET.fonbetTdata = new tennisData();
            PARSEMARAPHON.mfndata = new tennisData();
            PARSEZENIT.TData = new tennisData();
            PARSBETCITY.log = new logS();
            PARSEFONBET.log = new logS();
            PARSEMARAPHON.log = new logS();
            PARSEZENIT.log = new logS();
            //PBC.refresh2();
            betsearch.Forklog = new logS();
            betsearch1.Forklog = new logS();
            betsearch2.Forklog = new logS();
            betsearch3.Forklog = new logS();
            betsearch4.Forklog = new logS();
            betsearch5.Forklog = new logS();
            betsearch6.Forklog = new logS();
            betsearch7.Forklog = new logS();
            try
            {
                try
                {

                    refthread();
                }
                catch
                {
                    eventadd();
                }
                refreshfontest();
                
                try
                {
                    refmarafon();
                }
                catch(Exception e)
                {
                    for (int i = 0; i < PARSEMARAPHON.log.els; i++)
                    {
                        MainForm.addtext4(PARSEMARAPHON.log.ToPrint(i));
                        Application.DoEvents();
                    }
                    MainForm.addtext4("Error:" + e);
                }
                MainForm.refwb4();
                threadBet365();
                threadzenit();
                if (cnt >= 30)
                {
                    MainForm.refwb3();
                    MainForm.navigatewb6();
                    cnt = 0;
                }
                if (MainForm.refwb3i)
                {
                    cnt++;

                }
                if(needrefolimp)
                {
                    MainForm.navigatewb6();
                    needrefolimp = false;

                }
            }
            catch
            {
                evcnt = 3;
                eventadd();
            }
           
            //loop();
            MainForm.setactive();
        }
        public static void threadsearch1()
        {
            betsearch.setdebug(MainForm.formsettings.debug());
            betsearch.setminprof(MainForm.formsettings.minproc());
            betsearch.setdtest(MainForm.formsettings.hardtest());
            betsearch.datas_to_compares(" Fonbet ", " Betcity ");
            betsearch.compareTData(PARSEFONBET.fonbetTdata, PARSBETCITY.TData1);
        }
        public static void threadsearch2()
        {
            betsearch1.setminprof(MainForm.formsettings.minproc());
            betsearch1.setdebug(MainForm.formsettings.debug());
            betsearch1.datas_to_compares(" Fonbet ", " Maraphon ");
            betsearch1.compareTData(PARSEFONBET.fonbetTdata, PARSEMARAPHON.mfndata);
        }
        public static void threadsearch3()
        {
            betsearch2.setminprof(MainForm.formsettings.minproc());
            betsearch2.setdebug(MainForm.formsettings.debug());
            betsearch2.datas_to_compares(" Betcity ", " Maraphon ");
            betsearch2.compareTData(PARSBETCITY.TData1, PARSEMARAPHON.mfndata);
        }
        public static XElement draw(BETPatrn data) 
        {
            XElement xEmp;
            if (data != null)
                xEmp =
                            new XElement("Bookmaker",
                               data.TData.list.Select(p =>
                                    new XElement("Game",
                                        new XElement("Player1", p.eventInfo._Player1),
                                        new XElement("Player2", p.eventInfo._Player2),
                                        new XElement("Score", p.eventInfo.score),
                                            new XElement("Games",
                                                 p.gamesinEvent.list.Select(h =>
                                                     new XElement("Numgame",
                                                         new XElement ("Num",h.numgam),
                                                         new XElement("Coef1", h.koef1),
                                                         new XElement("Coef2", h.koef2)))))));
            else
                xEmp = new XElement("null");
            // Console.Write(xEmp);
            return xEmp;
           
        }
        public static void saveData()
        {
            draw(PARSEMARAPHON).Save("Marathon.xml");
            draw(PARSEFONBET).Save("Fonbet.xml");
            draw(PARSBETCITY).Save("Betcity.xml");
            draw(PARSEZENIT).Save("Zenit.xml");
        }
        
        public static void loop()
        {
            Task[] tsk = new Task[8];
            sw.Start();
            MainForm.clear3();
            //MainForm.addtext3("ReadyForSearch");
            if (!debugthread)
            {
                if (!MainForm.formsettings.isBetcity() && !MainForm.formsettings.isFonbet())
                {
                    betsearch.setdebug(MainForm.formsettings.debug());
                    betsearch.setdtest(MainForm.formsettings.hardtest());
                    betsearch.setminprof(MainForm.formsettings.minproc());
                    betsearch.datas_to_compares(" Fonbet ", " Betcity ");
                    betsearch.setban(MainForm.formsettings.banned());
                    tsk[0]= new Task(()=>
                    betsearch.compareTData(PARSEFONBET.fonbetTdata, PARSBETCITY.TData1));
                    tsk[0].Start();
                }
                // MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
                if (!MainForm.formsettings.isFonbet() && !MainForm.formsettings.isMarafon())
                {
                    betsearch1.setdtest(MainForm.formsettings.hardtest());
                    betsearch1.setminprof(MainForm.formsettings.minproc());
                    betsearch1.setdebug(MainForm.formsettings.debug());
                    betsearch1.datas_to_compares(" Fonbet ", " Maraphon ");
                    betsearch1.setban(MainForm.formsettings.banned());
                    tsk[1]= new Task(()=>
                    betsearch1.compareTData(PARSEFONBET.fonbetTdata, PARSEMARAPHON.mfndata));
                    if (tsk[1] != null)
                    tsk[1].Start();
                }
                //MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
                if (!MainForm.formsettings.isBetcity() && !MainForm.formsettings.isMarafon())
                {
                    betsearch2.setdtest(MainForm.formsettings.hardtest());
                    betsearch2.setminprof(MainForm.formsettings.minproc());
                    betsearch2.setdebug(MainForm.formsettings.debug());
                    betsearch2.datas_to_compares(" Betcity ", " Maraphon ");
                    betsearch2.setban(MainForm.formsettings.banned());
                    tsk[2]= new Task(()=>
                    betsearch2.compareTData(PARSBETCITY.TData1, PARSEMARAPHON.mfndata));
                    if (tsk[2] != null)
                    tsk[2].Start();
                }

                if (!MainForm.formsettings.isZenit() && !MainForm.formsettings.isBetcity())
                {
                    betsearch3.setdtest(MainForm.formsettings.hardtest());
                    betsearch3.setminprof(MainForm.formsettings.minproc());
                    betsearch3.setdebug(MainForm.formsettings.debug());
                    betsearch3.datas_to_compares(" Betcity ", " Zenit ");
                    betsearch3.setban(MainForm.formsettings.banned());
                    tsk[3]= new Task(()=>
                    betsearch3.compareTData(PARSBETCITY.TData1, PARSEZENIT.TData));
                    if (tsk[3] != null)
                    tsk[3].Start();
                }
                if (!MainForm.formsettings.isZenit() && !MainForm.formsettings.isFonbet())
                {
                    betsearch4.setdtest(MainForm.formsettings.hardtest());
                    betsearch4.setminprof(MainForm.formsettings.minproc());
                    betsearch4.setdebug(MainForm.formsettings.debug());
                    betsearch4.datas_to_compares(" Fonbet ", " Zenit ");
                    betsearch4.setban(MainForm.formsettings.banned());
                    tsk[4]= new Task(()=>
                    betsearch4.compareTData(PARSEFONBET.fonbetTdata, PARSEZENIT.TData));
                    if (tsk[4] != null)
                    tsk[4].Start();
                }
                if (!MainForm.formsettings.isZenit() && !MainForm.formsettings.isMarafon())
                {
                    betsearch5.setdtest(MainForm.formsettings.hardtest());
                    betsearch5.setminprof(MainForm.formsettings.minproc());
                    betsearch5.setdebug(MainForm.formsettings.debug());
                    betsearch5.datas_to_compares(" Maraphon ", " Zenit ");
                    betsearch5.setban(MainForm.formsettings.banned());
                    tsk[5]= new Task(()=>
                    betsearch5.compareTData(PARSEMARAPHON.mfndata, PARSEZENIT.TData));
                    if (tsk[5]!=null)
                    tsk[5].Start();
                }
                if (!MainForm.formsettings.OlimpChecked.Checked && !MainForm.formsettings.isFonbet())
                {
                    betsearch6.setdebug(MainForm.formsettings.debug());
                    betsearch6.setdtest(MainForm.formsettings.hardtest());
                    betsearch6.setminprof(MainForm.formsettings.minproc());
                    betsearch6.datas_to_compares(" Fonbet ", " Olimp ");
                    betsearch6.setban(MainForm.formsettings.banned());
                    tsk[6] = new Task(() =>
                    betsearch6.compareTData(PARSEFONBET.fonbetTdata, PARSEOLIMP.olimpTdata));
                    if (tsk[6] != null)
                        tsk[6].Start();
                    
                }
                if (!MainForm.formsettings.OlimpChecked.Checked && !MainForm.formsettings.isMarafon())
                {
                    betsearch7.setdebug(MainForm.formsettings.debug());
                    betsearch7.setdtest(MainForm.formsettings.hardtest());
                    betsearch7.setminprof(MainForm.formsettings.minproc());
                    betsearch7.datas_to_compares(" Maraphon ", " Olimp ");
                    betsearch7.setban(MainForm.formsettings.banned());
                    tsk[7] = new Task(() =>
                    betsearch7.compareTData(PARSEMARAPHON.mfndata, PARSEOLIMP.olimpTdata));
                    if (tsk[7] != null)
                        tsk[7].Start();

                }
                for (int i = 0; i < tsk.Length; i++)
                {
                    if(tsk[i]!=null)
                    Task.WaitAll(tsk[i]);
                }
                
            }
            else
            {
                if (true)
                {
                    betsearch6.setdebug(MainForm.formsettings.debug());
                    betsearch6.setdtest(MainForm.formsettings.hardtest());
                    betsearch6.setminprof(MainForm.formsettings.minproc());
                    betsearch6.datas_to_compares(" Fonbet ", " Olimp");
                    betsearch6.setban(MainForm.formsettings.banned());
                    betsearch6.compareTData(PARSEFONBET.fonbetTdata, PARSEOLIMP.olimpTdata);
                }

                if (!MainForm.formsettings.isBetcity() && !MainForm.formsettings.isFonbet())
                {
                    betsearch.setdebug(MainForm.formsettings.debug());
                    betsearch.setdtest(MainForm.formsettings.hardtest());
                    betsearch.setminprof(MainForm.formsettings.minproc());
                    betsearch.datas_to_compares(" Fonbet ", " Betcity ");
                    betsearch.setban(MainForm.formsettings.banned());
                    betsearch.compareTData(PARSEFONBET.fonbetTdata, PARSBETCITY.TData1);
                }
                // MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
                if (!MainForm.formsettings.isFonbet() && !MainForm.formsettings.isMarafon())
                {
                    betsearch1.setdtest(MainForm.formsettings.hardtest());
                    betsearch1.setminprof(MainForm.formsettings.minproc());
                    betsearch1.setdebug(MainForm.formsettings.debug());
                    betsearch1.datas_to_compares(" Fonbet ", " Maraphon ");
                    betsearch1.setban(MainForm.formsettings.banned());
                    betsearch1.compareTData(PARSEFONBET.fonbetTdata, PARSEMARAPHON.mfndata);
                }
                //MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
                if (!MainForm.formsettings.isBetcity() && !MainForm.formsettings.isMarafon())
                {
                    betsearch2.setdtest(MainForm.formsettings.hardtest());
                    betsearch2.setminprof(MainForm.formsettings.minproc());
                    betsearch2.setdebug(MainForm.formsettings.debug());
                    betsearch2.datas_to_compares(" Betcity ", " Maraphon ");
                    betsearch2.setban(MainForm.formsettings.banned());
                    betsearch2.compareTData(PARSBETCITY.TData1, PARSEMARAPHON.mfndata);
                }

                if (!MainForm.formsettings.isZenit() && !MainForm.formsettings.isBetcity())
                {
                    betsearch3.setdtest(MainForm.formsettings.hardtest());
                    betsearch3.setminprof(MainForm.formsettings.minproc());
                    betsearch3.setdebug(MainForm.formsettings.debug());
                    betsearch3.datas_to_compares(" Betcity ", " Zenit ");
                    betsearch3.setban(MainForm.formsettings.banned());
                    betsearch3.compareTData(PARSBETCITY.TData1, PARSEZENIT.TData);
                }
                if (!MainForm.formsettings.isZenit() && !MainForm.formsettings.isFonbet())
                {
                    betsearch4.setdtest(MainForm.formsettings.hardtest());
                    betsearch4.setminprof(MainForm.formsettings.minproc());
                    betsearch4.setdebug(MainForm.formsettings.debug());
                    betsearch4.datas_to_compares(" Fonbet ", " Zenit ");
                    betsearch4.setban(MainForm.formsettings.banned());
                    betsearch4.compareTData(PARSEFONBET.fonbetTdata, PARSEZENIT.TData);

                }
                if (!MainForm.formsettings.isZenit() && !MainForm.formsettings.isMarafon())
                {
                    betsearch5.setdtest(MainForm.formsettings.hardtest());
                    betsearch5.setminprof(MainForm.formsettings.minproc());
                    betsearch5.setdebug(MainForm.formsettings.debug());
                    betsearch5.datas_to_compares(" Maraphon ", " Zenit ");
                    betsearch5.setban(MainForm.formsettings.banned());
                    betsearch5.compareTData(PARSEMARAPHON.mfndata, PARSEZENIT.TData);
                }
            }
            //MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
            for (int i = 0; betsearch.Forklog != null && i < betsearch.Forklog.els; i++)
                MainForm.addtext3(betsearch.Forklog.ToPrint(i));
            //MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
            for (int i = 0; betsearch1.Forklog != null && i < betsearch1.Forklog.els; i++)
                MainForm.addtext3(betsearch1.Forklog.ToPrint(i));
            //MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
            for (int i = 0; betsearch2.Forklog != null && i < betsearch2.Forklog.els; i++)
                MainForm.addtext3(betsearch2.Forklog.ToPrint(i));
            for (int i = 0; betsearch3.Forklog != null && i < betsearch3.Forklog.els; i++)
                MainForm.addtext3(betsearch3.Forklog.ToPrint(i));
            for (int i = 0; betsearch4.Forklog != null && i < betsearch4.Forklog.els; i++)
                MainForm.addtext3(betsearch4.Forklog.ToPrint(i));
            for (int i = 0; betsearch5.Forklog != null && i < betsearch5.Forklog.els; i++)
                MainForm.addtext3(betsearch5.Forklog.ToPrint(i));
            for (int i = 0; betsearch6.Forklog != null && i < betsearch6.Forklog.els; i++)
                MainForm.addtext3(betsearch6.Forklog.ToPrint(i));
            for (int i = 0; betsearch7.Forklog != null && i < betsearch7.Forklog.els; i++)
                MainForm.addtext3(betsearch7.Forklog.ToPrint(i));
           // MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
            //MainForm.select();
            if (betsearch.Forklog.els < 2 && betsearch1.Forklog.els < 2 && betsearch2.Forklog.els < 2 && betsearch3.Forklog.els < 2 && betsearch4.Forklog.els < 2 && betsearch5.Forklog.els < 2&&betsearch6.Forklog.els < 2)
            {
                MainForm.addtext3("No vilkas...:(");
                if (MainForm.gettested())
                    MainForm.formtesting.Hideo();
                soundcount = 0;
            }
            else
            {
                if (soundplay)
                {
                    if (MainForm.formsettings.getactivegaming())
                    {
                        
                        if(soundcount>=1)
                        if (MainForm.formsettings.getSound() == "Стандартный")
                        {
                            player.Play();
                            
                        }
                        else
                        {
                            player1.Play();
                            
                        }
                        
                        soundcount++;
                    }
                    else
                    {
                        if (MainForm.formsettings.getSound() == "Стандартный")
                        {
                            player.Play();
                        }
                        else
                        {
                            player1.Play();
                        }
                    }
                    if (MainForm.gettested())
                        MainForm.formtesting.Show();
                }
            }
            MainForm.addtext3(sw.ElapsedMilliseconds.ToString() + " ms");
            
            sw.Stop();
            sw.Reset();
            
            MainForm.setactive();
            loopcount++;

        }
    }
}
