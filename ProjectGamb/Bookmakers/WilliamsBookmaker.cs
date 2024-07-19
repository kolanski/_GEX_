using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectGamb
{
    class WilliamsBookmaker : BookmakerPattern
    {
        List<string> CurrentGames;
        public bool automatic = false;

        public void GetLinks()
        {
            GamesLinks = new List<string>();
            Gecko.GeckoNodeCollection list = this.ParentBrowser.Document.GetElementsByClassName("rowLive");
            foreach (Gecko.GeckoNode node in list)
            {
                dynamic link = node.EvaluateXPath(".//td[5]/a").GetNodes().FirstOrDefault();
                if (link != null)
                    GamesLinks.Add(link.Href);
                //Console.Write(link.Href + System.Environment.NewLine);
            }
        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate(GamesLinks[i]);
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
                    this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate(GamesLinks[i]);
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
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    Trace.WriteLine(BookmakerWebBrowsers[i].geckoWebBrowser1.Url);
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

        public void CompareGamesToRemove()
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

            for ( int i = 0; i < CurrentGames.Count; i++)
            {
                if (rgx.Match(CurrentGames[i]).Success)
                    newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); 
                else 
                    newCurrentGames.Add(CurrentGames[i]);
            }

            Console.WriteLine("ToRemove");
            foreach (string GameNum in newCurrentGames.Except(newGamesLinks))
            {
                CloseTab(GameNum);
                Console.WriteLine(GameNum);
            }

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

        public void setAutomatic()
        {
            automatic = !automatic;
            Console.WriteLine("Automatic is:" + automatic);
            AutoUpdateSafe();
        }

        private async Task AutoUpdate()
        {
            while (automatic)
            {
                try
                {
                    CompareGames();
                    await Task.Delay(3000);
                    CompareGamesToRemove();
                    await Task.Delay(3000);
                  //  Console.WriteLine(Task.CurrentId);
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
                    //  Console.WriteLine(Task.CurrentId);
                }
                catch
                {

                }
            }
        }

        public void Parse()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Reset();
            sw.Start();
            BookmakerTennisGames.CleanData();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.WilliamsHill;
            try
            {
                string Compare = "";
                for (int i = 0; i < this.BookmakerTabs.Count; i++)
                {
                    var list = this.BookmakerWebBrowsers[i].geckoWebBrowser1.Document.GetElementsByClassName("leftPad title");
                    var scoreBoard = this.BookmakerWebBrowsers[i].geckoWebBrowser1.Document.GetElementById("scoreboard_frame") as Gecko.DOM.GeckoIFrameElement;

                    for (int l = 0; l < list.Count(); l++)
                    {
                        try
                        {
                            if (list[l].TextContent.Contains("Set - Game") && !list[l].TextContent.Contains("Point") && !list[l].TextContent.Contains("Score") && !list[l].TextContent.Contains("Point") && !list[l].TextContent.Contains("Total") && !list[l].TextContent.Contains("Win ") && !list[l].TextContent.Contains("Games"))
                            {
                                Regex rgs = new Regex("^[0-9]");
                                string SetNumber = rgs.Match(list[l].ChildNodes[5].TextContent).Value;
                                string GameNumber = list[l].ChildNodes[5].TextContent.Remove(0, list[l].ChildNodes[5].TextContent.Length - 2).Trim();
                                string Player2 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.ChildNodes[1].EvaluateXPath(".//div[@class='eventselection']/text()").GetNodes().LastOrDefault().NodeValue.Replace("\n", "").Replace("\t", "");
                                string Player1 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.ChildNodes[1].EvaluateXPath(".//div[@class='eventselection']/text()").GetNodes().FirstOrDefault().NodeValue.Replace("\n", "").Replace("\t", "");
                                string Coefficent2 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.ChildNodes[1].EvaluateXPath(".//div[@class='eventprice']/text()").GetNodes().LastOrDefault().NodeValue.Replace("\n", "").Replace("\t", "");
                                string Coefficent1 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.ChildNodes[1].EvaluateXPath(".//div[@class='eventprice']/text()").GetNodes().FirstOrDefault().NodeValue.Replace("\n", "").Replace("\t", "");
                                string Score = "";
                                string CurrentGames = "";
                                string CurrentPoints = "";
                                try
                                {
                                    for (int s = 1; s <= 4; s++)
                                    {
                                        if (scoreBoard.ContentDocument.GetElementById("set_" + s.ToString() + "_A").TextContent.ToString() != "")
                                        {
                                            if (scoreBoard.ContentDocument.GetElementById("set_" + s.ToString() + "_A").TextContent.Length > 1)
                                            {
                                                Score += scoreBoard.ContentDocument.GetElementById("set_" + s.ToString() + "_A").TextContent[0].ToString() + " ";
                                                Score += scoreBoard.ContentDocument.GetElementById("set_" + s.ToString() + "_B").TextContent[0].ToString() + " ";
                                            }
                                            else
                                            {
                                                Score += scoreBoard.ContentDocument.GetElementById("set_" + s.ToString() + "_A").TextContent.ToString() + " ";
                                                Score += scoreBoard.ContentDocument.GetElementById("set_" + s.ToString() + "_B").TextContent.ToString() + " ";
                                            }
                                        }
                                    }

                                    CurrentGames = scoreBoard.ContentDocument.GetElementById("games_A").TextContent.ToString() + " " + scoreBoard.ContentDocument.GetElementById("games_B").TextContent.ToString();
                                    CurrentPoints = scoreBoard.ContentDocument.GetElementById("points_A").TextContent.ToString() + " " + scoreBoard.ContentDocument.GetElementById("points_B").TextContent.ToString();
                                }
                                catch (Exception e)
                                {
                                  //Console.WriteLine(e);
                                }
                                if (Compare.Equals(Player1)||" "+Compare==Player1)
                                {
                                  //Console.WriteLine("Set" + SetNumber + " Game" + GameNumber);
                                  //Console.WriteLine(Coefficent1 + " " + Coefficent2);
                                    BookmakerTennisGames.AddGames(SetNumber, GameNumber, Coefficent1, Coefficent2);
                                }
                                else
                                {
                                    if (Compare != "" && (Compare != Player1 && " " + Compare != Player1 && " " + Compare.Remove(Compare.Length-1) != Player1))
                                        this.BookmakerTennisGames.AddData();
                                  //Console.WriteLine(Player1);
                                  //Console.WriteLine(Player2);
                                    Compare = Player1;
                                 //   if (Compare[0].ToString() == " ")
                                  //      Compare = Compare.Remove(0);
                                  //Console.WriteLine("Set" + SetNumber + " Game" + GameNumber);
                                  //Console.WriteLine(Coefficent1 + " " + Coefficent2);
                                  //Console.WriteLine("Score:" + Score);
                                  //Console.WriteLine("Games:" + CurrentGames);
                                  //Console.WriteLine("Points:" + CurrentPoints);
                                    BookmakerTennisGames.SetPlayers(Player1, Player2);
                                    BookmakerTennisGames.SetGameData("",Score+CurrentGames,CurrentPoints);
                                    BookmakerTennisGames.AddGames(SetNumber, GameNumber, Coefficent1, Coefficent2);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("WilliamsPrsGamesErr:"+e);
                        }
                    }
                }
                if (BookmakerTennisGames.tempGames != null && BookmakerTennisGames.tempGames.Count != 0)
                    this.BookmakerTennisGames.AddData();

             //   Console.WriteLine("Count" + BookmakerTennisGames.TennisData.Count);
                Console.WriteLine(sw.ElapsedMilliseconds);
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Williams have NoGames or Not Loaded");
            }
            catch (Exception e)
            {
                Console.WriteLine("WilliamsPrsErr:" + e);
            }
        }
    }
}