using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ExtensionMethods;

namespace ProjectGambAwesomium
{
    class WilliamsBookmaker : BookmakerPattern
    {
        List<string> CurrentGames;
        public bool automatic = false;
        public bool toremove = false;
        public bool toparse = false;
        public bool TestCurrentParentBrowser()
        {
            ParentBrowser.ExecuteScriptAsync(Scripts.WilliamsTestBrowser);
            var TestBrowser = ParentBrowser.EvaluateScriptAsync("testBrowser");
            
            if (TestBrowser != null)
            {
                if (TestBrowser.ToString() == "Tennis")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public void GetLinks()
        {
           GamesLinks = new List<string>();
           if(TestCurrentParentBrowser())
           {
               ParentBrowser.ExecuteScriptAsync(Scripts.WilliamsGetLinksSet);
               var len = ParentBrowser.EvaluateScriptAsync("len");
               
               var ilen=int.Parse(len.ToString());
               for(int i=0;i<ilen;i++)
               {
                   var link = ParentBrowser.EvaluateScriptAsync(Scripts.WilliamsGetLinksArr(i));
                   
                   if(link!=null)
                   {
                       GamesLinks.Add(link.ToString());
                       //Debug.WriteLine(link.ToString());
                   }
               }
           }

            /*
            GamesLinks = new List<string>();
            var list = this.ParentBrowser.Document.GetElementsByClassName("rowLive");
            foreach (Gecko.GeckoNode node in list)
            {
                dynamic link = node.EvaluateXPath(".//td[5]/a").GetNodes().FirstOrDefault();
                if (link != null)
                    GamesLinks.Add(link.Href);
                
            }*/
        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab(GamesLinks[i]);
             //   this.BookmakerWebBrowsers[i].Load();
            }
        }

        public async void TenderOpenTabs()
        {
            try
            {
                for (int i = 0; i < GamesLinks.Count; i++)
                {
                    this.CreateTab();
                    await Task.Delay(2000);
                    this.BookmakerWebBrowsers[i].Load(GamesLinks[i]);
                }
            }
            catch(ObjectDisposedException e)
            {
                 
            }

        }
        public void TestTab()
        {
            this.CreateTab();
        }

        public void GetCurrentUrls()
        {
            CurrentGames = new List<string>();
            try
            {
                if (BookmakerWebBrowsers!=null)
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

        public void CompareGames()
        {
            GetCurrentUrls();
            GetLinks();
            List<string> newCurrentGames = new List<string>();
            List<string> newGamesLinks = new List<string>();

            //toadd
            string pattern = @"[0-9]{7}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            for(int i=0;i<GamesLinks.Count;i++)
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

        public void CompareGamesSafe()
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
                        CreateTabSafe(link);
                    }
                }
            }
        }

        public async Task CompareGamesToRemove()
        {
            toremove = true;
            
            GetCurrentUrls();
            GetLinks();
            List<string> newGamesLinks = new List<string>();
            List<string> newCurrentGames = new List<string>();
            string pattern = @"[0-9]{7}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);



            Debug.WriteLine("ToRemove");
            if (!toparse)
            foreach (string GameNum in newCurrentGames.Except(newGamesLinks))
            {
                foreach(string curr in CurrentGames)
                {
                    if(curr.Contains(GameNum))
                        CloseTab(curr);
                }
                    
                
                Debug.WriteLine(GameNum);
            }
            toremove = false;
        }

        public void CompareGamesToRemoveSafe()
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

            for (int i = 0; i < CurrentGames.Count; i++)
            {
                if (rgx.Match(CurrentGames[i]).Success)
                    newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value);
                else
                    newCurrentGames.Add(CurrentGames[i]);
            }

            Debug.WriteLine("ToRemove");
            foreach (string GameNum in newCurrentGames.Except(newGamesLinks))
            {
                if (GameNum != "about:blank")
                {
                    CloseTabSafe(GameNum);
                    Debug.WriteLine(GameNum);
                }
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
            int cnt = 0;
            while (automatic)
            {
                try
                {
                    CompareGames();
                    await Task.Delay(3000);
                    CompareGamesToRemove();
                    await Task.Delay(3000);
                        ParentBrowser.Load("http://sports.williamhill.com/bet/en-gb/betlive/24");
                        await Task.Delay(120000);
                        cnt++;
                        if (cnt > 15)
                        {
                            cnt = 0;
                            this.CloseAllTabs();
                            await Task.Delay(120000);
                        }
                  //  Debug.WriteLine(Task.CurrentId);
                }
                catch
                {

                }
            }
        }

        private async Task AutoUpdateSafe()
        {
            while (automatic)
            {
                try
                {
                    CompareGamesSafe();
                    await Task.Delay(3000);
                    CompareGamesToRemoveSafe();
                    await Task.Delay(3000);
                    ParentBrowser.Load("http://sports.williamhill.com/bet/en-gb/betlive/24");
                    await Task.Delay(120000);
                    //  Debug.WriteLine(Task.CurrentId);
                }
                catch
                {

                }
            }
        }

        public void Parse()
        {
            BookmakerTennisGames.CleanData();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Williams;
            toparse = true;
            if(!toremove)
            try                                                                                                                                                                                                                                                                                                                      
            {
                if (BookmakerWebBrowsers!=null)
                for(int i=0;i<this.BookmakerWebBrowsers.Count;i++)
                {
                    try
                    {
                        var CurrentBrowser = this.BookmakerWebBrowsers[i];
                            CurrentBrowser.ExecuteScriptAsync(Scripts.Williams);

                            var Player1 = CurrentBrowser.EvaluateScriptAsync("Player1");
                            var Player2 = CurrentBrowser.EvaluateScriptAsync("Player2");
                            var ScoreAll = CurrentBrowser.EvaluateScriptAsync("Score");

                            var ScorePoints = CurrentBrowser.EvaluateScriptAsync("CurrentPoints");

                            var LenArr = CurrentBrowser.EvaluateScriptAsync("GamesArray.length");

                            var CurrentGames = CurrentBrowser.EvaluateScriptAsync("CurrentGames");

                            var Totscore = ScoreAll.ToString() + CurrentGames.ToString();
                            // ScoreAll += CurrentGames;
                            BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
                            BookmakerTennisGames.SetGameData("", Totscore, ScorePoints.ToString());

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
                    catch(Exception e)
                    {
                        Debug.WriteLine("WilliamsPrsee:" + e);
                    }
                }
            }
            catch (System.NullReferenceException)
            {
                Debug.WriteLine("Williams have NoGames or Not Loaded");
            }
            catch (Exception e)
            {
                Debug.WriteLine("WilliamsPrsErr:" + e);
            }
            toparse = false;
        }
    }
}