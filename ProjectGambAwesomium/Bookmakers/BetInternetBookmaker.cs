using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectGambAwesomium
{
    class BetInternetBookmaker : BookmakerPattern
    {
        List<string> CurrentGames;
        public bool automatic = false;
        string BetInternetBaseUrl = "http://mobile.betinternet.com/en/InPlayConsole.bet?EventID=";
        public void GetLinks()
        {
            try
            {
                GamesLinks = new List<string>();
                ParentBrowser.ExecuteScriptAsync(Scripts.BetInternetGet);
                var len = ParentBrowser.EvaluateScriptAsync("len.length");

                var ilen = int.Parse(len.ToString());
                for (int i = 0; i < ilen - 1; i++)
                {
                    var link = ParentBrowser.EvaluateScriptAsync("arr[" + i.ToString() + "];");

                    if (link != null)
                    {
                        GamesLinks.Add(link.ToString());
                        //Debug.WriteLine(link.ToString());
                    }
                }
            }
            catch
            {

            }
        }
        public void OpenTabs()
        {
            if (GamesLinks != null)
                for (int i = 0; i < GamesLinks.Count; i++)
                {
                    this.CreateTab(GamesLinks[i]);
                    //   this.BookmakerWebBrowsers[i].Load();
                }
        }


        public void CompareGames()
        {
            GetCurrentUrls();
            GetLinks();
            List<string> newCurrentGames = new List<string>();
            List<string> newGamesLinks = new List<string>();

            //toadd
            string pattern = @"[0-9]{7}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                newGamesLinks.Add(rgx.Match(GamesLinks[i]).Value);
            }
            for (int i = 0; i < CurrentGames.Count; i++)
            {
                newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value);
            }
            Trace.WriteLine("Эти фамилии есть в списке A, но их нет в списке B");
            foreach (string GameNum in newGamesLinks.Except(newCurrentGames))
            {
                foreach (string link in GamesLinks)
                {
                    if (GameNum != null && link.Contains(GameNum))
                    {
                        Trace.WriteLine(link);
                        CreateTab(link);
                    }
                }
            }
        }

        public void CompareGamesToRemove()
        {
            try
            {
                GetCurrentUrls();
                GetLinks();
                List<string> newGamesLinks = new List<string>();
                List<string> newCurrentGames = new List<string>();
                string pattern = @"[0-9]{7}";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                for (int i = 0; i < GamesLinks.Count; i++)
                {
                    newGamesLinks.Add(rgx.Match(GamesLinks[i]).Value);
                }
                for (int i = 0; i < CurrentGames.Count; i++) { if (rgx.Match(CurrentGames[i]).Success)newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); else newCurrentGames.Add(CurrentGames[i]); }
                Debug.WriteLine("ToRemove");
                foreach (string GameNum in newCurrentGames.Except(newGamesLinks))
                {
                    CloseTab(BetInternetBaseUrl+ GameNum);
                    Debug.WriteLine(GameNum);

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("InternetBetErrerr:" + e);
            }
        }

        public void GetCurrentUrls()
        {
            CurrentGames = new List<string>();
            try
            {
                if (BookmakerWebBrowsers != null)
                    for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                    {
                        Trace.WriteLine(BookmakerWebBrowsers[i].Address);
                        CurrentGames.Add(BookmakerWebBrowsers[i].Address.ToString());
                    }
            }
            catch
            {

            }
        }

        public void Parse()
        {
            BookmakerTennisGames.CleanData();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.InternetBet;
            try
            {
                if (BookmakerWebBrowsers != null)
                    for (int i = 0; i < this.BookmakerWebBrowsers.Count; i++)
                    {
                        try
                        {
                            var CurrentBrowser = this.BookmakerWebBrowsers[i];
                            CurrentBrowser.ExecuteScriptAsync(Scripts.BetInternet);

                            var Player1 = CurrentBrowser.EvaluateScriptAsync("Player1");
                            var Player2 = CurrentBrowser.EvaluateScriptAsync("Player2");
                           // var ScoreAll = CurrentBrowser.EvaluateScriptAsync("Score");

                          //  var ScorePoints = CurrentBrowser.EvaluateScriptAsync("CurrentPoints");

                            var LenArr = CurrentBrowser.EvaluateScriptAsync("GamesArray.length");

                            //var CurrentGames = CurrentBrowser.EvaluateScriptAsync("CurrentGames");

                        //    var Totscore = ScoreAll.ToString() + CurrentGames.ToString();
                            // ScoreAll += CurrentGames;
                            BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
                            BookmakerTennisGames.SetGameData("", "", "");

                            for (int h = 0; h < int.Parse(LenArr.ToString()); h++)
                            {
                                var SetNumber = CurrentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].SetNum[0]");

                                var GameNumber = CurrentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].GameNum");

                                var Coefficent1 = CurrentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].Coef1");

                                var Coefficent2 = CurrentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].Coef2");

                                BookmakerTennisGames.AddGames(SetNumber.ToString(), GameNumber.ToString(), Coefficent1.ToString(), Coefficent2.ToString());
                            }
                            if (Player1.ToString() != "")
                                BookmakerTennisGames.AddData();

                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("BetInternetPrsee:" + e);
                        }
                    }
            }
            catch (Exception e)
            {
                Debug.WriteLine("BetInternetPrsee:" + e);
            }
        }
        public void setAutomatic()
        {
            automatic = !automatic;
            Debug.WriteLine("Automatic is:" + automatic);
            AutoUpdate();
        }
        private async Task AutoUpdate()
        {
            while (automatic)
            {
                int cnt = 0;
                try
                {
                    CompareGames();
                    await Task.Delay(3000);
                    CompareGamesToRemove();
                    await Task.Delay(3000);
                    ParentBrowser.Load("http://mobile.betinternet.com/en/Sports.bet");
                    await Task.Delay(120000);
                    cnt++;
                    if (cnt > 15)
                    {
                        cnt = 0;
                        this.CloseAllTabs();
                        await Task.Delay(120000);
                    }
                }
                catch
                {

                }
            }
        }
    }
}

