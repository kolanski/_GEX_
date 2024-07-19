using Gecko;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGamb
{
    class PariMatchBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        private string PariMatchUrlBase = "http://www.parimatchru.com/en/bet.html?hl=";
        
        public void GetLinks()
        {
            Console.WriteLine("CurrentLinks");
            GamesLinks = new List<string>();
            try
            {
                string norm;
                string len;

                using (Gecko.AutoJSContext java = new Gecko.AutoJSContext(this.ParentBrowser.Window.JSContext))
                {
                    java.EvaluateScript(@"var list =document.getElementsByClassName('sport tennis')[0];var test=list.getElementsByClassName('dt processed');test.length", (nsISupports)this.ParentBrowser.Window.DomWindow, out len);
                    for (int i = 0; i < int.Parse(len); i++)
                    {
                        if (int.Parse(len) != 0)
                        {
                            java.EvaluateScript(@"var list =document.getElementsByClassName('sport tennis')[0];var test=list.getElementsByClassName('dt processed');test[" + i.ToString() + "].getAttribute('evno').toString();", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                            Console.WriteLine(norm);
                            GamesLinks.Add(norm);
                        }
                    }
                    java.EvaluateScript(@"var list =document.getElementsByClassName('sport tennis')[1];var test=list.getElementsByClassName('dt processed');test.length", (nsISupports)this.ParentBrowser.Window.DomWindow, out len);
                    for (int i = 0; i < int.Parse(len); i++)
                    {
                        if (int.Parse(len) != 0)
                        {
                            java.EvaluateScript(@"var list =document.getElementsByClassName('sport tennis')[1];var test=list.getElementsByClassName('dt processed');test[" + i.ToString() + "].getAttribute('evno').toString();", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                            if (norm.Length > 2)
                            {
                                Console.WriteLine(norm);
                                GamesLinks.Add(norm);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Parimatcherr" + e);
            }

        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate(PariMatchUrlBase + GamesLinks[i]);
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
                    Console.WriteLine(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().Remove(0, BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().IndexOf("=") + 1));
                    CurrentGames.Add(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().Remove(0, BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().IndexOf("=") +1));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bet365PrsGetUrlsErr" + e);
            }
        }

        public void Parse()
        {
            if (BookmakerTennisGames == null)
                BookmakerTennisGames = new TennisGames();
            BookmakerTennisGames.CleanData();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.PariMatch;
            if(BookmakerWebBrowsers!=null)
            try
            {
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    try
                    {
                        var playersdata = BookmakerWebBrowsers[i].geckoWebBrowser1.Document.GetElementsByClassName("l");
                        string Player1 = playersdata[0].ChildNodes[0].TextContent;
                        string Player2 = playersdata[0].ChildNodes[2].TextContent;
                        if (Player2 == "")
                            if (playersdata[0].ChildNodes[3].TextContent != "")
                                Player2 = playersdata[0].ChildNodes[3].TextContent;
                            else
                                Player2 = playersdata[0].ChildNodes[4].TextContent;
                        string ScoreAll = "";
                        string ScoreGame = "0 0";
                        if (!playersdata[1].TextContent.Contains(":"))
                        {
                            ScoreAll = playersdata[1].TextContent.Remove(0, playersdata[1].TextContent.IndexOf('(') + 1).Replace("-", " ").Replace(",", " ").Replace(")", "");
                        }
                        else
                        {
                            var toremove1 = playersdata[1].TextContent.Remove(0, playersdata[1].TextContent.IndexOf("(") + 1).Replace("-", " ").Replace(",", " ");
                            var toremove2 = toremove1.Remove(toremove1.IndexOf(")"), toremove1.Length - toremove1.IndexOf(")"));
                            ScoreAll = toremove2.Replace("(", "");
                            ScoreGame = toremove1.Remove(0, toremove1.IndexOf(")") + 2).Replace(":", " ");
                        }
                        BookmakerTennisGames.SetPlayers(Player1, Player2);
                        BookmakerTennisGames.SetGameData("", ScoreAll, ScoreGame);
                        var GamesList = BookmakerWebBrowsers[i].geckoWebBrowser1.Document.GetElementsByClassName("dyn");
                        for (int j = 0; j < GamesList.Length; j++)
                        {
                            var testTotest = GamesList[j].TextContent;
                            if (testTotest.Contains("Set ") && testTotest.Contains("game ") && !testTotest.Contains("point") && !testTotest.Contains("score") && !testTotest.Contains("Who will "))
                            {
                                var SetGameText = GamesList[j].ChildNodes[1].TextContent;
                                var set = SetGameText.Substring(SetGameText.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }), 1);
                                var game = SetGameText.Substring(SetGameText.LastIndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }) - 1, 2).Replace(" ", "");
                                BookmakerTennisGames.AddGames(set, game, GamesList[j].ChildNodes[3].ChildNodes[1].TextContent, GamesList[j].ChildNodes[5].ChildNodes[1].TextContent);
                            }

                        }
                        BookmakerTennisGames.AddData();
                    }
                    catch(Exception ep)
                    {
                        Console.WriteLine("ParimatchprsErr:"+ep);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
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
                        CreateTab(PariMatchUrlBase + link);
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
                        CreateTabSafe(PariMatchUrlBase + link);
                    }
                }
            }
        }

        public void CompareToRemove()
        {
            GetLinks();
            GetUrls();
            foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
            {
                CloseTab(PariMatchUrlBase + GameToRemove);
                Console.WriteLine("ClosedParimatch" + GameToRemove);
            }
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {

                CloseTab(PariMatchUrlBase + GameToRemove);
                Console.WriteLine("ClosedParimatch" + GameToRemove);
            }
        }

        public void CompareToRemoveSafe()
        {
            GetLinks();
            GetUrls();
            foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(PariMatchUrlBase + GameToRemove);
                    Console.WriteLine("ClosedParimatch" + GameToRemove);
                }
            }
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(PariMatchUrlBase + GameToRemove);
                    Console.WriteLine("ClosedParimatch" + GameToRemove);
                }
            }
        }

        public async void TenderOpen()
        {
            if (GamesLinks != null)
                for (int i = 0; i < GamesLinks.Count; i++)
                {
                    this.CreateTab();
                    await Task.Delay(2000);
                    this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate(PariMatchUrlBase + GamesLinks[i]);
                }
        }

        public void Automatic()
        {
            automatic = !automatic;
            Console.WriteLine("ParimatchAutois:" + automatic);
            AutoUpdateSafe();
        }


        private async Task AutoUpdate()
        {
            while (automatic)
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
    }
}
