using Gecko;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGamb
{
    class Bet365Bookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        private string Bet365UrlBase = "https://mobile.365sb.com/premium/?lng=1#type=Coupon;key=";
        public void GetLinks()
        {
            Console.WriteLine("CurrentLinks");
            GamesLinks = new List<string>();
            for (int i = 0; i < this.ParentBrowser.Document.GetElementsByClassName("Fixture").Length - 1; i++)
            {
                try
                {
                    string norm;
                    using (Gecko.AutoJSContext java = new Gecko.AutoJSContext(this.ParentBrowser.Window.JSContext))
                    {
                        java.EvaluateScript(@"document.getElementsByClassName('Fixture')[" + i + "].wrapper.stem.data.ID;", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                    }
                    string scriptContent = @"document.getElementsByClassName('Fixture')[" + i + "].wrapper.stem.data.ID;";
                    AutoJSContext jsContext = new AutoJSContext(this.ParentBrowser.Window.JSContext);
                    string result;
                    jsContext.EvaluateScript(scriptContent, (nsISupports)ParentBrowser.Window.DomWindow, out result);
                    Console.WriteLine(result);
                    Console.WriteLine(norm);
                    GamesLinks.Add(norm);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Bet365PrsGetLinksErr" + e);
                }
            }
        }



        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate("https://mobile.bet365.com/#type=Coupon;key=" + GamesLinks[i]);
            }
        }

        public void GetUrls()
        {
            CurrentGames = new List<string>();
            Console.WriteLine("CurrentGames");
            try
            {
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    Console.WriteLine(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().Remove(0, BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().IndexOf("key=") + 4));
                    CurrentGames.Add(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().Remove(0, BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().IndexOf("key=") + 4));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bet365PrsGetUrlsErr" + e);
            }
        }

        public void Parse()
        {
            //WordsMatching.MatchsMaker scoreChange;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                BookmakerTennisGames.CleanData();
                this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Bet365;
                for (int i = 0; i < BookmakerTabs.Count; i++)
                {
                    string Score = "";
                    string GamePoints = "";
                    string Player1 = "";
                    string Player2 = "";
                    var CurrentBrowser = this.BookmakerWebBrowsers[i].geckoWebBrowser1;
                    var ScoreBoard = CurrentBrowser.Document.GetElementById("previousSets");
                    try
                    {
                        for (int h = 0; h < ScoreBoard.ChildNodes[0].ChildNodes.Length; h++)
                        {
                            Score += ScoreBoard.ChildNodes[0].ChildNodes[h].TextContent.Replace("-", " ") + " ";
                        }
                        GamePoints = CurrentBrowser.Document.GetElementById("team1ArenaPoint").TextContent + " " + CurrentBrowser.Document.GetElementById("team2ArenaPoint").TextContent;
                        //  Console.WriteLine("Score " + Score);
                        //  Console.WriteLine("GamePoints " + GamePoints);

                    }
                    catch (System.NullReferenceException)
                    {
                        Console.WriteLine("Bet365 have score error");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bet365PrsScoreErr:" + e);
                    }

                    try
                    {
                        var tmpPl1 = CurrentBrowser.Document.GetElementById("team1Name");
                        var tmpPl2 = CurrentBrowser.Document.GetElementById("team2Name");
                        if (tmpPl1 != null)
                        {
                            Player1 = CurrentBrowser.Document.GetElementById("team1Name").TextContent;
                            Player2 = CurrentBrowser.Document.GetElementById("team2Name").TextContent;
                            BookmakerTennisGames.SetPlayers(Player1, Player2);
                            BookmakerTennisGames.SetGameData("", Score, GamePoints);
                        }
                        // Console.WriteLine("Player1 " + Player1);
                        //  Console.WriteLine("Player2 " + Player2);
                    }
                    catch (System.NullReferenceException)
                    {
                        Console.WriteLine("Bet365 have no players error");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Bet365PrsPlayersErr" + e);
                    }

                    var MatchBets = CurrentBrowser.Document.GetElementsByClassName("Market");

                    for (int h = 0; h < MatchBets.Length; h++)
                    {
                        if (MatchBets[h].TextContent.Contains("Game Winner"))
                        {
                            string GameNumber = MatchBets[h].ChildNodes[0].ChildNodes[0].TextContent.Replace("Game Winner", "").Replace("th", "").Replace("nd", "").Replace("st", "");
                            //Console.WriteLine(MatchBets[h].ChildNodes[0].ChildNodes[0].TextContent.Replace("Game Winner", "").Replace("th", "").Replace("nd", "").Replace("st", ""));

                            try
                            {
                                // Console.WriteLine(MatchBets[h].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[1].TextContent);
                                // Console.WriteLine(MatchBets[h].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[1].TextContent);
                                string PlayerEqual = MatchBets[h].ChildNodes[1].ChildNodes[0].ChildNodes[0].TextContent;
                                string Coefficent1 = MatchBets[h].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[1].TextContent;
                                string Coefficent2 = MatchBets[h].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[1].TextContent;
                                //if (GameNumber != null && Coefficent1)
                                PlayerEqual= PlayerEqual.Remove(PlayerEqual.IndexOfAny("0123456789".ToCharArray())).Replace("(Svr)", "");
                                if(Player1.Last().ToString()!=" ")
                                    PlayerEqual=PlayerEqual.Remove(PlayerEqual.Length-1);
                                if (Player1==PlayerEqual)
                                    BookmakerTennisGames.AddGames("0", GameNumber.Replace("rd", ""), Coefficent1, Coefficent2);
                                else
                                    BookmakerTennisGames.AddGames("0", GameNumber.Replace("rd", ""), Coefficent2, Coefficent1);
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                Console.WriteLine("Bet365 Game have no Games");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Bet365PrsGameErr:" + e);
                            }
                        }

                    }
                    if (Player1 != "")
                        BookmakerTennisGames.AddData();
                    //  Console.WriteLine();
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Bet365 have NoGames or Not Loaded");
            }
            catch (Exception e)
            {
                Console.WriteLine("Bet365Err:" + e);
            }
        }

        public void CompareGames()
        {
            GetUrls();
            GetLinks();
            foreach (string GameNum in GamesLinks.Except(CurrentGames))
            {
                foreach (string link in GamesLinks)
                {
                    if (GameNum != null && link.Contains(GameNum))
                    {
                        Console.WriteLine("ToAdd");
                        Console.WriteLine(link);
                        CreateTab(Bet365UrlBase + link);
                    }
                }
            }
        }

        public void CompareGamesSafe()
        {
            GetUrls();
            GetLinks();
            foreach (string GameNum in GamesLinks.Except(CurrentGames))
            {
                foreach (string link in GamesLinks)
                {
                    if (GameNum != null && link.Contains(GameNum))
                    {
                        Console.WriteLine("ToAdd");
                        Console.WriteLine(link);
                        CreateTabSafe(Bet365UrlBase + link);
                    }
                }
            }
        }
        public void setAutomatic()
        {
            automatic = !automatic;
            Console.WriteLine("Bet365Autois:"+automatic);
            AutoUpdateSafe();
        }

        public void CompareToRemove()
        {
            GetLinks();
            GetUrls();
            foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
            {
                CloseTab(Bet365UrlBase + GameToRemove);
                Console.WriteLine("ClosedBet365" + GameToRemove);
            }
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {

                CloseTab(Bet365UrlBase + GameToRemove);
                Console.WriteLine("ClosedBet365" + GameToRemove);
            }
            CloseTab("https://mobile.365sb.com/premium/?lng=1#type=InPlay;");
            CloseTab("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=;ip=1;lng=1");
        }

        public void CompareToRemoveSafe()
        {
            GetLinks();
            GetUrls();
            foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(Bet365UrlBase + GameToRemove);
                    Console.WriteLine("ClosedBet365" + GameToRemove);
                }
            }
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(Bet365UrlBase + GameToRemove);
                    Console.WriteLine("ClosedBet365" + GameToRemove);
                }
            }
            CloseTabSafe("https://mobile.365sb.com/premium/?lng=1#type=InPlay;");
            CloseTabSafe("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=;ip=1;lng=1");
        }
        

        private async Task AutoUpdate()
        {
            while(automatic)
            {
                try
                {
                    CompareGames();
                    await Task.Delay(10000);
                    CompareToRemove();
                    await Task.Delay(10000);
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
                    await Task.Delay(10000);
                    CompareToRemoveSafe();
                    await Task.Delay(10000);
                }
                catch
                {

                }
            }
        }

        public async void TenderOpen()
        {
            if(GamesLinks!=null)
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                await Task.Delay(2000);
                this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate(Bet365UrlBase + GamesLinks[i]);
            }
        }
    }
}
