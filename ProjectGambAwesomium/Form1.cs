using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGambAwesomium
{
    public partial class Form1 : Form
    {
        WilliamsBookmaker WilliamsController = new WilliamsBookmaker();
        UniBetBookmaker UniBetController = new UniBetBookmaker();
        Bet365Bookmaker Bet365Controller = new Bet365Bookmaker();
        MarathonBookmaker MarathonController = new MarathonBookmaker();
        PariMatchBookmaker PariMatchController = new PariMatchBookmaker();
        FonbetBookmaker FonbetController = new FonbetBookmaker();
        BetCityBookmaker BetCityController = new BetCityBookmaker();
        BetInternetBookmaker BetInternetController = new BetInternetBookmaker();

        WebForm WilliamsWebBrowser;
        WebForm UniBetWebbrowser;
        WebForm Bet365WebBrowser;
        WebForm MarathonWebBrowser;
        WebForm PariMatchWebBrowser;
        //WebForm FonbetWebBrowser;
        WebForm BetCityWebBrowser;
        WebForm BetInternetWebBrowser;

        public FormSearch SearchForm = new FormSearch();
        public void SetUpMyBrowsers()
        {
            WilliamsWebBrowser = new WebForm(new Uri("http://sports.williamhill.com/bet/en-gb/betlive/24"));
            WilliamsWebBrowser.TopLevel = false;
            WilliamsWebBrowser.Dock = DockStyle.None;
            WilliamsWebBrowser.Location = new System.Drawing.Point(3, 3);
            WilliamsWebBrowser.Show();
            TabControl.TabPages[0].Controls.Add(WilliamsWebBrowser);

            UniBetWebbrowser = new WebForm(new Uri("https://touch.unibet.com/client-start/sportsbook#events/live"));
            UniBetWebbrowser.TopLevel = false;
            UniBetWebbrowser.Dock = DockStyle.None;
            UniBetWebbrowser.Location = new System.Drawing.Point(3, 3);
            UniBetWebbrowser.Show();
            TabControl.TabPages[1].Controls.Add(UniBetWebbrowser);

            Bet365WebBrowser = new WebForm(new Uri("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=13;ip=1;lng=1"));
            Bet365WebBrowser.TopLevel = false;
            Bet365WebBrowser.Dock = DockStyle.None;
            Bet365WebBrowser.Location = new System.Drawing.Point(3, 3);
            Bet365WebBrowser.Show();
            TabControl.TabPages[2].Controls.Add(Bet365WebBrowser);

            MarathonWebBrowser = new WebForm(new Uri("https://www.betmarathon.com/en/live.htm"));
            MarathonWebBrowser.TopLevel = false;
            MarathonWebBrowser.Dock = DockStyle.None;
            MarathonWebBrowser.Location = new System.Drawing.Point(3, 3);
            MarathonWebBrowser.Show();
            TabControl.TabPages[3].Controls.Add(MarathonWebBrowser);

            PariMatchWebBrowser = new WebForm(new Uri("http://www.parimatchru.com/en/live.html"));
            PariMatchWebBrowser.TopLevel = false;
            PariMatchWebBrowser.Dock = DockStyle.None;
            PariMatchWebBrowser.Location = new System.Drawing.Point(3, 3);
            PariMatchWebBrowser.Show();
            TabControl.TabPages[4].Controls.Add(PariMatchWebBrowser);

            FonbetGeckoWebBrowser.Navigate("https://live.bkfonbet.com/?locale=en#4");
            //FonbetWebBrowser = new WebForm("https://live.bkfonbet.com/?locale=en#4");
            //FonbetWebBrowser.TopLevel = false;
            //FonbetWebBrowser.Dock = DockStyle.None;
            //FonbetWebBrowser.Location = new System.Drawing.Point(3, 3);
            //FonbetWebBrowser.Show();
            //TabControl.TabPages[5].Controls.Add(FonbetWebBrowser);

            BetCityWebBrowser = new WebForm("http://www.betsbc.com/en/");
            BetCityWebBrowser.TopLevel = false;
            BetCityWebBrowser.Dock = DockStyle.None;
            BetCityWebBrowser.Location = new System.Drawing.Point(3, 3);
            BetCityWebBrowser.Show();
            TabControl.TabPages[6].Controls.Add(BetCityWebBrowser);

            BetInternetWebBrowser = new WebForm("http://mobile.betinternet.com/en/Sports.bet");
            BetInternetWebBrowser.TopLevel = false;
            BetInternetWebBrowser.Dock = DockStyle.None;
            BetInternetWebBrowser.Location = new System.Drawing.Point(3, 3);
            BetInternetWebBrowser.Show();
            TabControl.TabPages[7].Controls.Add(BetInternetWebBrowser);
            
        }
        public bool boolAuto=false;
        public Form1()
        {
            InitializeComponent();
            this.Text = Text + " Build:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Gecko.Xpcom.Initialize(Application.StartupPath + "\\xulrunner");
            SetUpMyBrowsers();
            SetUp();
        }

        public void Form1Auto()
        {

            boolAuto = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           SearchForm.Show();
           Task tsk = new Task(async () =>
           {
              // await Task.Delay(10000);
              // Invoke((MethodInvoker)delegate { WilliamsController.Navigate("http://sports.williamhill.com/bet/en-gb/betlive/24"); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { TabControl.SelectedIndex = 1; });
               await Task.Delay(1000);
             //  Invoke((MethodInvoker)delegate { UniBetController.Navigate("https://touch.unibet.com/client-start/sportsbook#events/live"); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { TabControl.SelectedIndex = 2; });
               await Task.Delay(1000);
              // Invoke((MethodInvoker)delegate { Bet365Controller.Navigate("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=13;ip=1;lng=1"); });
             //  await Task.Delay(20000);
               Invoke((MethodInvoker)delegate { TabControl.SelectedIndex = 3; });
               await Task.Delay(10000);
             //  Invoke((MethodInvoker)delegate {   MarathonController.Navigate("https://www.betmarathon.com/en/live.htm");});
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { TabControl.SelectedIndex = 5; });
               await Task.Delay(1000);
          //     Invoke((MethodInvoker)delegate { FonbetController.Navigate("https://live.bkfonbet.com/?locale=en#4"); });
               await Task.Delay(1000);
               
               Invoke((MethodInvoker)delegate
               {
                   MarathonController.ParentBrowser.ExecuteScriptAsync(@"var test= document.getElementsByClassName('sport-menu-item');for(var el=0;el< test.length;el++)
if(test[el].getAttribute('data-sport-group')=='Football') test[el].childNodes[3].click();");
              });
               await Task.Delay(1000);
              Invoke((MethodInvoker)delegate {  WilliamsController.GetLinks();});
              await Task.Delay(1000);
             Invoke((MethodInvoker)delegate {   WilliamsController.OpenTabs();});
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { Bet365Controller.GetLinks(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { Bet365Controller.OpenTabs(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { MarathonController.GetLinks(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { BetInternetController.GetLinks(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { FonbetController.GetUrls(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { MarathonController.OpenTabs(); });
               await Task.Delay(30000);
               Invoke((MethodInvoker)delegate { BetInternetController.OpenTabs(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { WilliamsController.setAutomatic(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { Bet365Controller.setAutomatic(); });
               await Task.Delay(1000);
            //   Invoke((MethodInvoker)delegate { BetInternetController.setAutomatic(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { MarathonController.setAutomatic(); });
               await Task.Delay(30000);
               Invoke((MethodInvoker)delegate { FonbetController.Automatic(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { PariMatchController.GetLinks(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { PariMatchController.OpenTabs(); });
               await Task.Delay(3000);
               Invoke((MethodInvoker)delegate { PariMatchController.Automatic(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { BetCityController.NavigateEvents(); });
               await Task.Delay(1000);
               Invoke((MethodInvoker)delegate { BetCityController.Automatic(); });
               await Task.Delay(1000);
           });
            if(boolAuto)
                tsk.Start();
        }
    
        private void searchshow()
        {
            this.BeginInvoke((Action)delegate
            {
                SearchForm = new FormSearch();
                SearchForm.Show();
            }
            );
        }


    
        void SetUp()
        {
            //searchshow();
            WilliamsController.SetUpBrowser(WilliamsWebBrowser);
            WilliamsController.SetUpTabPage(WilliamsTabControl);
            UniBetController.SetUpBrowser(UniBetWebbrowser);
            UniBetController.SetUpTabPage(UniBetTabControl);
            Bet365Controller.SetUpBrowser(Bet365WebBrowser);
            Bet365Controller.SetUpTabPage(Bet365TabControl);
            MarathonController.SetUpBrowser(MarathonWebBrowser);
            MarathonController.SetUpTabPage(MarathonTabControl);
            PariMatchController.SetUpBrowser(PariMatchWebBrowser);
            PariMatchController.SetUpTabPage(PariMatchTabControl);
            FonbetController.SetUpBrowser(FonbetGeckoWebBrowser);
            BetCityController.SetUpBrowser(BetCityWebBrowser);
            BetInternetController.SetUpBrowser(BetInternetWebBrowser);
            BetInternetController.SetUpTabPage(BetInternetTabControl);
        }

        private void WilliamsNavButton_Click(object sender, EventArgs e)
        {
            WilliamsController.Navigate("http://sports.williamhill.com/bet/en-gb/betlive/24");
        }

        private void WilliamsGetLinksButton_Click(object sender, EventArgs e)
        {
            WilliamsController.GetLinks();
        }

        private void WilliamsOpenTabsButton_Click(object sender, EventArgs e)
        {
            WilliamsController.OpenTabs();
        }

        private void WilliamsCloseTabsButton_Click(object sender, EventArgs e)
        {
            WilliamsController.CloseAllTabs();
        }

        private void WilliamsGetCurrentUrls_Click(object sender, EventArgs e)
        {
            WilliamsController.GetCurrentUrls();
        }

        private void WilliamsCompareGames_Click(object sender, EventArgs e)
        {
            WilliamsController.CompareGames();
        }

        private void WilliamsCloseSelectedTab_Click(object sender, EventArgs e)
        {
            WilliamsController.CloseTab(WilliamsController.ParentTab.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WilliamsController.TestTab();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WilliamsController.CompareGamesToRemove();
        }

        private void WilliamsAutomaticButton_Click(object sender, EventArgs e)
        {
            WilliamsController.setAutomatic();
            WilliamsAutoIsOnCheckBox.Checked = WilliamsController.automatic;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WilliamsController.automatic)
                WilliamsController.setAutomatic();
        }

        private void WilliamsParseButton_Click(object sender, EventArgs e)
        {
            WilliamsController.Parse();
        }

        private void WilliamsTenderOpen_Click(object sender, EventArgs e)
        {
            WilliamsController.TenderOpenTabs();
        }
        private void UniBetNavigateButton_Click(object sender, EventArgs e)
        {
            UniBetController.Navigate("https://touch.unibet.com/client-start/sportsbook#events/live");
        }

        private void UniBetGetLinksButton_Click(object sender, EventArgs e)
        {
            UniBetController.GetLinks();
        }

        private void UniBetOpenTabsButton_Click(object sender, EventArgs e)
        {
            UniBetController.OpenTabs();
        }

        private void UniBetCloseTabsButton_Click(object sender, EventArgs e)
        {
            UniBetController.CloseAllTabs();
        }

        private void UniBetGetUrlsButton_Click(object sender, EventArgs e)
        {
            UniBetController.GetCurrentUrls();
        }

        private void UniBetCompGamesButton_Click(object sender, EventArgs e)
        {
            UniBetController.CompareGames();
        }

        private void UniBetParseButton_Click(object sender, EventArgs e)
        {
            UniBetController.Parse();
        }

        private void UniBetCloseSelectButton_Click(object sender, EventArgs e)
        {
            UniBetController.CloseTab(UniBetController.ParentTab.SelectedIndex);
        }
        private void UnibetTenderOpen_Click(object sender, EventArgs e)
        {
            UniBetController.TenderOpen();
        }
        private void UniBetAutomaticButton_Click(object sender, EventArgs e)
        {
            UniBetController.setAutomatic();
            UniBetAutoIsOnCheckBox.Checked = UniBetController.automatic;
        }

        private void Bet365NavigateButton_Click(object sender, EventArgs e)
        {
            //GeckoPreferences.User["general.useragent.override"] = "Mozilla/5.0 (Linux; Android 4.1.1; Nexus 7 Build/JRO03D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166  Safari/535.19";
            Bet365Controller.Navigate("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=13;ip=1;lng=1");
        }

        private void Bet365GetlinksButton_Click(object sender, EventArgs e)
        {
            Bet365Controller.GetLinks();
        }

        private void Bet365OpenTabsButton_Click(object sender, EventArgs e)
        {
            Bet365Controller.OpenTabs();
        }

        private void Bet365CloseTabsButton_Click(object sender, EventArgs e)
        {
            Bet365Controller.CloseAllTabs();
        }

        private void Bet365GetUrlsButton_Click(object sender, EventArgs e)
        {
            Bet365Controller.GetUrls();
        }

        private void Bet365ParseButton_Click(object sender, EventArgs e)
        {
            Bet365Controller.Parse();
        }

        private async void Bet365CompGamesButton_Click(object sender, EventArgs e)
        {
           Bet365Controller.CompareGames();
        }

        private void Bet365CloseSelectButton_Click(object sender, EventArgs e)
        {
            Bet365Controller.CloseTab(Bet365Controller.ParentTab.SelectedIndex);
        }

        private void Bet365TenderOpen_Click(object sender, EventArgs e)
        {
            Bet365Controller.TenderOpen();
        }

        private void Bet365AutomaticButton_Click(object sender, EventArgs e)
        {
            Bet365Controller.setAutomatic();
            Bet365AutoCheckBox.Checked = !Bet365AutoCheckBox.Checked;
        }

        public long WilliamsAction()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Reset();
            sw.Start();
            WilliamsController.Parse();
            sw.Stop();
            //SearchForm.label3.Text = sw.ElapsedMilliseconds.ToString();
            return sw.ElapsedMilliseconds;
        }
        public long ControllerAction(Action functionToExec)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Reset();
            sw.Start();
            functionToExec();
            //WilliamsController.Parse();
            sw.Stop();
            //SearchForm.label3.Text = sw.ElapsedMilliseconds.ToString();
            return sw.ElapsedMilliseconds;
        }
        public Task<long> SomeWilliams(Action functionToExec)
        {
            var tcs = new TaskCompletionSource<long>();
            Task.Factory.StartNew(() =>
            {
                var result = this.ControllerAction(functionToExec);
                tcs.SetResult(result);
            });
            return tcs.Task;
        }

        private async Task ParseAll()
        {
            //Synced
            long result = ControllerAction(new Action(() => WilliamsController.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label3.Text = result.ToString(); });

          /*  result =  ControllerAction(new Action(() => UniBetController.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label4.Text = result.ToString(); });*/

            result = ControllerAction(new Action(() => Bet365Controller.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label5.Text = result.ToString(); });

            result = ControllerAction(new Action(() => MarathonController.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label6.Text = result.ToString(); });

            result = ControllerAction(new Action(() => PariMatchController.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label7.Text = result.ToString(); });

            result = ControllerAction(new Action(() => FonbetController.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label8.Text = result.ToString(); });

            result = ControllerAction(new Action(() => BetCityController.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label9.Text = result.ToString(); });

          /*  result = ControllerAction(new Action(() => BetInternetController.Parse()));
            Invoke((MethodInvoker)delegate
            { SearchForm.label10.Text = result.ToString(); });*/
        }

        private async Task ParseAllP()
        {
            //Parallel
            if (!WilliamsController.toremove && !UniBetController.toremove && !Bet365Controller.toremove && !MarathonController.toremove)
            {
                var list = new List<Action>{
                new Action(()=>{long result = SomeWilliams(new Action(() => WilliamsController.Parse())).Result;
                Invoke((MethodInvoker)delegate
                { SearchForm.label3.Text = result.ToString(); });}),
                new Action(()=>{long result = SomeWilliams(new Action(() => UniBetController.Parse())).Result;
                Invoke((MethodInvoker)delegate
                { SearchForm.label4.Text = result.ToString(); });}),
                 new Action(()=>{long result = SomeWilliams(new Action(() => Bet365Controller.Parse())).Result;
                Invoke((MethodInvoker)delegate
                { SearchForm.label5.Text = result.ToString(); });}),
                 new Action(()=>{long result = SomeWilliams(new Action(() => MarathonController.Parse())).Result;
                Invoke((MethodInvoker)delegate
                { SearchForm.label6.Text = result.ToString(); });}),
                 new Action(()=>{long result = SomeWilliams(new Action(() => PariMatchController.Parse())).Result;
                Invoke((MethodInvoker)delegate
                { SearchForm.label7.Text = result.ToString(); });}),
                 new Action(()=>{long result = SomeWilliams(new Action(() => FonbetController.Parse())).Result;
                Invoke((MethodInvoker)delegate
                { SearchForm.label8.Text = result.ToString(); });}),
                 new Action(()=>{long result = SomeWilliams(new Action(() => BetCityController.Parse())).Result;
                Invoke((MethodInvoker)delegate
                { SearchForm.label9.Text = result.ToString(); });})
          };
                await Task.Run(() => Parallel.ForEach(list, new ParallelOptions { MaxDegreeOfParallelism = 7 }, action => action()));
            }
        }

        public async Task SendToSearchForm()
        {
            if(!isParallel.Checked)
                await ParseAll();
            else
                await ParseAllP();
            if (WilliamsController.BookmakerTennisGames!=null)
            SearchForm.addText(WilliamsController.BookmakerTennisGames.PrintGames(),1);
            if (UniBetController.BookmakerTennisGames!=null)
            SearchForm.addText(UniBetController.BookmakerTennisGames.PrintGames(),  2);
            if (Bet365Controller.BookmakerTennisGames!=null)
            SearchForm.addText(Bet365Controller.BookmakerTennisGames.PrintGames(),  3);
            if (MarathonController.BookmakerTennisGames!=null)
            SearchForm.addText(MarathonController.BookmakerTennisGames.PrintGames(), 5);
            if (PariMatchController.BookmakerTennisGames!=null)
            SearchForm.addText(PariMatchController.BookmakerTennisGames.PrintGames(), 6);
            if (FonbetController.BookmakerTennisGames!=null)
            SearchForm.addText(FonbetController.BookmakerTennisGames.PrintGames(), 7);
            if (BetCityController.BookmakerTennisGames!=null)
            SearchForm.addText(BetCityController.BookmakerTennisGames.PrintGames(), 8);
            if (BetInternetController.BookmakerTennisGames != null)
                SearchForm.addText(BetInternetController.BookmakerTennisGames.PrintGames(), 9);
        }
        
        public ProjectGambAwesomium.TennisGames GetWilliamsGames()
        {
           return WilliamsController.BookmakerTennisGames;
        }

        public ProjectGambAwesomium.TennisGames GetUniBet()
        {
            return UniBetController.BookmakerTennisGames;
        }

        public ProjectGambAwesomium.TennisGames GetBet365()
        {
            return Bet365Controller.BookmakerTennisGames;
        }

        public ProjectGambAwesomium.TennisGames GetMarathon()
        {
            return MarathonController.BookmakerTennisGames;
        }

        public ProjectGambAwesomium.TennisGames GetPariMatchGames()
        {
            return PariMatchController.BookmakerTennisGames;
        }

        public ProjectGambAwesomium.TennisGames GetFonbetGames()
        {
            return FonbetController.BookmakerTennisGames;
        }

        public ProjectGambAwesomium.TennisGames GetBetCityGames()
        {
            return BetCityController.BookmakerTennisGames;
        }

        public ProjectGambAwesomium.TennisGames GetBetInternetGames()
        {
            return BetInternetController.BookmakerTennisGames;
        }

        public void  MarathonNavigateButton_Click(object sender, EventArgs e)
        {
           MarathonController.Navigate("https://www.betmarathon.com/en/live.htm");
        }

        private void MarathonGetLinksButton_Click(object sender, EventArgs e)
        {
            MarathonController.GetLinks();
        }

        private void MarathonOpenTabsButton_Click(object sender, EventArgs e)
        {
            MarathonController.OpenTabs();
        }

        private void MarathonCloseTabsButton_Click(object sender, EventArgs e)
        {
            MarathonController.CloseAllTabs();
        }

        private void MarathonGetUrlsButton_Click(object sender, EventArgs e)
        {
            MarathonController.GetUrls();
        }

        private void MarathonParseButton_Click(object sender, EventArgs e)
        {
            MarathonController.Parse();
        }

        private void MarathonCompGamesButton_Click(object sender, EventArgs e)
        {
            MarathonController.CompareGames();
        }

        private void MarathonTenderOpenButton_Click(object sender, EventArgs e)
        {
            MarathonController.TenderOpen();
        }

        private void MarathonAutomaticButton_Click(object sender, EventArgs e)
        {
            MarathonController.setAutomatic();
            MarathonAutoIsOnCheckbox.Checked = !MarathonAutoIsOnCheckbox.Checked;
        }

        private void PariMatchNavigateButton_Click(object sender, EventArgs e)
        {
            PariMatchController.Navigate("http://www.parimatchru.com/en/live.html");
        }

        private void PariMatchGetLinksButton_Click(object sender, EventArgs e)
        {
            PariMatchController.GetLinks();
        }

        private void PariMatchOpenTabsButton_Click(object sender, EventArgs e)
        {
            PariMatchController.OpenTabs();
        }

        private void PariMatchCloseTabsButton_Click(object sender, EventArgs e)
        {
            PariMatchController.CloseAllTabs();
        }

        private void PariMatchGetUrlsButton_Click(object sender, EventArgs e)
        {
            PariMatchController.GetUrls();
        }

        private void PariMatchParseButton_Click(object sender, EventArgs e)
        {
            Task lol = new Task(() => { Invoke((MethodInvoker)delegate { PariMatchController.Parse(); }); });
            lol.Start();
            
        }

        private void PariMatchCompGamesButton_Click(object sender, EventArgs e)
        {
            PariMatchController.CompareGames();
        }

        private void PariMatchCloseSelectButton_Click(object sender, EventArgs e)
        {
            PariMatchController.CloseTab(PariMatchController.ParentTab.SelectedIndex);
        }

        private void PariMatchTenderOpenButton_Click(object sender, EventArgs e)
        {
            PariMatchController.TenderOpen();
        }

        private void PariMatchAutomaticButton_Click(object sender, EventArgs e)
        {
            PariMatchController.Automatic();
            PariMatchAutoIsOnCheckbox.Checked = !PariMatchAutoIsOnCheckbox.Checked;
        }

        private void FonbetNavigateButton_Click(object sender, EventArgs e)
        {
            FonbetController.Navigate("https://live.bkfonbet.com/?locale=en#4");
        }

        private void FonbetGetlinksButton_Click(object sender, EventArgs e)
        {
            FonbetController.GetLinks();
        }

        private void FonbeParseButton_Click(object sender, EventArgs e)
        {
            FonbetController.Parse();
        }

        private void FonbetAutomaticButton_Click(object sender, EventArgs e)
        {
            FonbetAutoIsOnCheckbox.Checked = !FonbetAutoIsOnCheckbox.Checked;
            FonbetController.Automatic();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            WilliamsController.ParentBrowser.ExecuteScriptAsync(@"document.getElementById('oddsSelect').selectedIndex =1;
document.site.set_pref('price_display','DECIMAL');");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Bet365Controller.ParentBrowser.ExecuteScriptAsync(@"document.getElementById('ctl00_Main_ddlOddsType').selectedIndex=1;
HelpDropdownChange(document.getElementById('ctl00_Main_ddlOddsType'), 3);");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void FonbetGetUrlsButton_Click(object sender, EventArgs e)
        {
            FonbetController.GetUrls();
        }

        private void BetCityNavigationButton_Click(object sender, EventArgs e)
        {
            BetCityController.Navigate("http://www.betsbc.com/en/");
        }

        private void BetCityGetLinksButton_Click(object sender, EventArgs e)
        {
            BetCityController.NavigateEvents();
        }

        private void BetCityParseButton_Click(object sender, EventArgs e)
        {
            BetCityController.Parse();
        }

        private void BetCityAutomaticButton_Click(object sender, EventArgs e)
        {
            BetCityController.Automatic();
        }

        private void BetInternetNavigateButton_Click(object sender, EventArgs e)
        {
            BetInternetController.Navigate("http://mobile.betinternet.com/en/Sports.bet");
        }

        private void BetInternetGetLinksButton_Click(object sender, EventArgs e)
        {
            BetInternetController.GetLinks();
        }

        private void BetInternetOpenTabsButton_Click(object sender, EventArgs e)
        {
            BetInternetController.OpenTabs();
        }

        private void BetInternetParseButton_Click(object sender, EventArgs e)
        {
            BetInternetController.Parse();
        }

        private void BetInternetCompGamesButton_Click(object sender, EventArgs e)
        {
            BetInternetController.CompareGames();
        }

        private void BetInternetCloseTabsButton_Click(object sender, EventArgs e)
        {
            BetInternetController.CloseAllTabs();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }



    }
}
