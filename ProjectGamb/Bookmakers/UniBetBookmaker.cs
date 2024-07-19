using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectGamb
{
    class UniBetBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;

        public void GetLinks()
        {
            GamesLinks = new List<string>();
            var list = this.ParentBrowser.Document.GetElementsByClassName("starred ir");
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].PreviousSibling.TextContent.Contains("Tennis"))
                {
                    Console.Write(list[i].EvaluateXPath(".//@data-id").GetNodes().FirstOrDefault().NodeValue + System.Environment.NewLine);
                    GamesLinks.Add(list[i].EvaluateXPath(".//@data-id").GetNodes().FirstOrDefault().NodeValue);
                }
            }
        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate("https://touch.unibet.com/client-start/sportsbook#event/live/" + GamesLinks[i]);
            }
        }
        public async void TenderOpen()
        {
            GetLinks();
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                await Task.Delay(2000);
                this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate("https://touch.unibet.com/client-start/sportsbook#event/live/" + GamesLinks[i]);
            }
        }
        public void GetCurrentUrls()
        {
            CurrentGames = new List<string>();
            try
            {
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    Console.WriteLine(BookmakerWebBrowsers[i].geckoWebBrowser1.Url);
                    CurrentGames.Add(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString());
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
            string pattern = @"[0-9]{10}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            Parallel.For(0, GamesLinks.Count, i => { newGamesLinks.Add(rgx.Match(GamesLinks[i]).Value); });
            Parallel.For(0, CurrentGames.Count, i => { newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); });
            Trace.WriteLine("Эти фамилии есть в списке A, но их нет в списке B");
            foreach (string GameNum in newGamesLinks.Except(newCurrentGames))
            {
                foreach (string link in GamesLinks)
                {
                    if (GameNum != null && link.Contains(GameNum))
                    {
                        Trace.WriteLine(link);
                        CreateTab("https://touch.unibet.com/client-start/sportsbook#event/live/"+link);
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
            string pattern = @"[0-9]{10}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            Parallel.For(0, GamesLinks.Count, i => { newGamesLinks.Add(rgx.Match(GamesLinks[i]).Value); });
            Parallel.For(0, CurrentGames.Count, i => { newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); });
            Trace.WriteLine("Эти фамилии есть в списке A, но их нет в списке B");
            foreach (string GameNum in newGamesLinks.Except(newCurrentGames))
            {
                foreach (string link in GamesLinks)
                {
                    if (GameNum != null && link.Contains(GameNum))
                    {
                        Trace.WriteLine(link);
                        CreateTabSafe("https://touch.unibet.com/client-start/sportsbook#event/live/" + link);
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
                string pattern = @"[0-9]{10}";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                for (int i = 0; i < GamesLinks.Count; i++)
                {
                    newGamesLinks.Add(rgx.Match(GamesLinks[i]).Value);
                }
                Parallel.For(0, CurrentGames.Count, i => { if (rgx.Match(CurrentGames[i]).Success)newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); else newCurrentGames.Add(CurrentGames[i]); });
                Console.WriteLine("ToRemove");
                foreach (string GameNum in newCurrentGames.Except(newGamesLinks))
                {
                    CloseTab(GameNum);
                    Console.WriteLine(GameNum);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Unierr:" + e);
            }
        }

        public void CompareGamesToRemoveSafe()
        {
            try
            {
                GetCurrentUrls();
                GetLinks();
                List<string> newGamesLinks = new List<string>();
                List<string> newCurrentGames = new List<string>();
                string pattern = @"[0-9]{10}";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                for (int i = 0; i < GamesLinks.Count; i++)
                {
                    newGamesLinks.Add(rgx.Match(GamesLinks[i]).Value);
                }
                Parallel.For(0, CurrentGames.Count, i => { if (rgx.Match(CurrentGames[i]).Success)newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); else newCurrentGames.Add(CurrentGames[i]); });
                Console.WriteLine("ToRemove");
                foreach (string GameNum in newCurrentGames.Except(newGamesLinks))
                {
                    if (GameNum != "about:blank")
                    {
                        CloseTabSafe(GameNum);
                        Console.WriteLine(GameNum);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unierr:" + e);
            }
        }

        private async Task AutoUpdate()
        {
            while (automatic)
            {
                try
                {
                    CompareGames();
                    await Task.Delay(15000);
                    CompareGamesToRemove();
                    await Task.Delay(3000);
                    Console.WriteLine(Task.CurrentId);
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
                    await Task.Delay(15000);
                    CompareGamesToRemoveSafe();
                    await Task.Delay(3000);
                    Console.WriteLine(Task.CurrentId);
                }
                catch
                {

                }
            }
        }

        public void setAutomatic()
        {
            automatic = !automatic;
            Console.WriteLine("Automatic is:" + automatic);
            AutoUpdateSafe();
        }


        public void Parse()
        {
            BookmakerTennisGames.CleanData();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.UniBet;
            try
            {
                for (int j = 0; j < BookmakerWebBrowsers.Count; j++)
                {
                    TryClick(j);
                    try
                    {
                        var Player1 = BookmakerWebBrowsers[j].geckoWebBrowser1.Document.GetElementsByClassName("event-name-home eventName")[0].TextContent;
                        var Player2 = BookmakerWebBrowsers[j].geckoWebBrowser1.Document.GetElementsByClassName("event-name-away eventName")[0].TextContent;
                        var EventName = BookmakerWebBrowsers[j].geckoWebBrowser1.Document.GetElementsByClassName("clearfix event-path-and-description")[0].TextContent;
                        var Row1 = BookmakerWebBrowsers[j].geckoWebBrowser1.Document.GetElementsByClassName("set-row2")[0];
                        var Row2 = BookmakerWebBrowsers[j].geckoWebBrowser1.Document.GetElementsByClassName("set-row3")[0];

                        string Score = "";
                        string ScoreSet = "";
                        for (int i = 2; i < Row1.ChildNodes.Length - 1; i++)
                        {
                            Score += Row1.ChildNodes[i].TextContent + " " + Row2.ChildNodes[i].TextContent + " ";
                        }
                        var list = BookmakerWebBrowsers[j].geckoWebBrowser1.Document.GetElementsByClassName("criteria");

                        ScoreSet = Row1.ChildNodes[Row1.ChildNodes.Length - 1].TextContent + " " + Row2.ChildNodes[Row2.ChildNodes.Length - 1].TextContent;
                        BookmakerTennisGames.SetPlayers(Player1, Player2);
                        BookmakerTennisGames.SetGameData(EventName, Score, ScoreSet);

                        for (int i = 0; i < list.Length; i++)
                        {
                            if (list[i].ChildNodes[0].TextContent.Length == 15 || list[i].ChildNodes[0].TextContent.Length == 14)
                            {
                                string SetGame = list[i].ChildNodes[0].TextContent.Replace("SET", "").Replace("GAME", "").Replace("-", "");
                                string Coefficent1 = list[i].NextSibling.ChildNodes[1].ChildNodes[0].ChildNodes[1].TextContent;
                                string Coefficent2 = list[i].NextSibling.ChildNodes[2].ChildNodes[0].ChildNodes[1].TextContent;
                                BookmakerTennisGames.AddGames(SetGame[1].ToString(), (SetGame[SetGame.Length - 2].ToString() + SetGame[SetGame.Length - 1].ToString()).ToString(), Coefficent1, Coefficent2);
                            }
                        }

                        BookmakerTennisGames.AddData();
                    }
                    catch
                    {

                    }
                }
            }
            catch(System.NullReferenceException)
            {
                Console.WriteLine("Unibet have NoGames or Not Loaded");
            }
            catch(Exception e)
            {
                Console.WriteLine("UnibetPrsErr:" + e);
            }
        }

        public void TryClick(int index)
        {
            
            dynamic val = BookmakerWebBrowsers[index].geckoWebBrowser1.Document.GetElementsByClassName("category-container collapsed");
            for (int i = 0; i < val.Length; i++)
            {
                if(val[i].TextContent.Contains("GAME"))
                {
                    val[i].ChildNodes[0].Click();
                }
            }

            val = BookmakerWebBrowsers[index].geckoWebBrowser1.Document.GetElementsByClassName("category-container");
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i].TextContent.Contains("SELECTED MARKETS"))
                {
                    val[i].ChildNodes[0].Click();
                }
            }
        }
    }
}
