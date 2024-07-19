using Gecko;
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

namespace ProjectGamb
{
    public partial class Form1 : Form
    {
        WilliamsBookmaker WilliamsController = new WilliamsBookmaker();
        UniBetBookmaker UniBetController = new UniBetBookmaker();
        Bet365Bookmaker Bet365Controller = new Bet365Bookmaker();
        MarathonBookmaker MarathonController = new MarathonBookmaker();
        PariMatchBookmaker PariMatchController = new PariMatchBookmaker();
        FonbetBookmaker FonbetController = new FonbetBookmaker();

        public FormSearch SearchForm ;
        
        public Form1()
        {
            InitializeComponent();
            Gecko.Xpcom.Initialize(Application.StartupPath + "\\xulrunner");
            SetUp();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // SearchForm.Show();
            System.Threading.Thread t = new System.Threading.Thread(new
           System.Threading.ThreadStart(this.searchshow));
            t.Start();
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
            FonbetController.SetUpBrowser(FonbetWebBrowser);
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
            GeckoPreferences.User["general.useragent.override"] = "Mozilla/5.0 (Linux; Android 4.1.1; Nexus 7 Build/JRO03D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166  Safari/535.19";
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

        private void Bet365CompGamesButton_Click(object sender, EventArgs e)
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

        private void ParseAll()
        {
            WilliamsController.Parse();
            UniBetController.Parse();
            Bet365Controller.Parse();
            MarathonController.Parse();
            PariMatchController.Parse();
            FonbetController.Parse();
        }

        public void SendToSearchForm()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Reset();
            sw.Start();
            ParseAll();
            SearchForm.addText(WilliamsController.BookmakerTennisGames.PrintGames(),1);
            SearchForm.addText(UniBetController.BookmakerTennisGames.PrintGames(),  2);
            SearchForm.addText(Bet365Controller.BookmakerTennisGames.PrintGames(),  3);
            SearchForm.addText(MarathonController.BookmakerTennisGames.PrintGames(), 5);
            SearchForm.addText(PariMatchController.BookmakerTennisGames.PrintGames(), 6);
            SearchForm.addText(FonbetController.BookmakerTennisGames.PrintGames(), 7);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
        
        public ProjectGamb.TennisGames GetWilliamsGames()
        {
           return WilliamsController.BookmakerTennisGames;
        }

        public ProjectGamb.TennisGames GetUniBet()
        {
            return UniBetController.BookmakerTennisGames;
        }

        public ProjectGamb.TennisGames GetBet365()
        {
            return Bet365Controller.BookmakerTennisGames;
        }

        public ProjectGamb.TennisGames GetMarathon()
        {
            return MarathonController.BookmakerTennisGames;
        }

        public ProjectGamb.TennisGames GetPariMatchGames()
        {
            return PariMatchController.BookmakerTennisGames;
        }

        public ProjectGamb.TennisGames GetFonbetGames()
        {
            return FonbetController.BookmakerTennisGames;
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
            PariMatchController.Parse();
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

    }
}
