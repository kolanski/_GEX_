using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExtensionMethods;

namespace ProjectGambAwesomium
{
    class UniBetBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        public bool toremove = false;
        public bool toparse = false;
        bool TestCurrentParentBrowser()
        {
            if (ParentBrowser.Address == "https://touch.unibet.com/client-start/sportsbook#events/live")
                return true;
            else
                return false;
        }
        public void GetLinks()
        {
            GamesLinks = new List<string>();
            if (TestCurrentParentBrowser())
            {
                ParentBrowser.ExecuteScriptAsync(Scripts.UnibetGetLinksSet);
                var len = ParentBrowser.EvaluateScriptAsync("arr.length");
                
                var ilen = int.Parse(len.ToString());
                for (int i = 0; i < ilen; i++)
                {
                    var link = ParentBrowser.EvaluateScriptAsync("arr["+i.ToString()+"]");
                    
                    if (link != null)
                    {
                        GamesLinks.Add(link.ToString());
                        //Debug.WriteLine(link.ToString());
                    }
                }
            }
            /*
            GamesLinks = new List<string>();
            var list = this.ParentBrowser.Document.GetElementsByClassName("starred ir");
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].PreviousSibling.TextContent.Contains("Tennis"))
                {
                    Console.Write(list[i].EvaluateXPath(".//@data-id").GetNodes().FirstOrDefault().NodeValue + System.Environment.NewLine);
                    GamesLinks.Add(list[i].EvaluateXPath(".//@data-id").GetNodes().FirstOrDefault().NodeValue);
                }
            }*/
        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab("https://touch.unibet.com/client-start/sportsbook#event/live/" + GamesLinks[i]);
                //this.BookmakerWebBrowsers[i].Load();
            }
        }
        public async void TenderOpen()
        {
            GetLinks();
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                await Task.Delay(2000);
                this.BookmakerWebBrowsers[i].Load("https://touch.unibet.com/client-start/sportsbook#event/live/" + GamesLinks[i]);
            }
        }
        public void GetCurrentUrls()
        {
            CurrentGames = new List<string>();
            try
            {
                if (BookmakerWebBrowsers!=null)
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    Debug.WriteLine(BookmakerWebBrowsers[i].Address);
                    CurrentGames.Add(BookmakerWebBrowsers[i].Address.ToString());
                }
            }
            catch
            {

            }
        }

        public void CompareGames()
        {
            try
            {
                GetCurrentUrls();
                GetLinks();
                List<string> newCurrentGames = new List<string>();
                List<string> newGamesLinks = new List<string>();

                //toadd
                string pattern = @"[0-9]{10}";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                for (int i = 0; i < GamesLinks.Count; i++)
                { newGamesLinks.Add(rgx.Match(GamesLinks[i]).Value); }
                if (CurrentGames.Count > 0)
                    for (int i = 0; i < CurrentGames.Count; i++)
                    { newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); }
                Trace.WriteLine("Эти фамилии есть в списке A, но их нет в списке B");
                foreach (string GameNum in newGamesLinks.Except(newCurrentGames))
                {
                    foreach (string link in GamesLinks)
                    {
                        if (GameNum != null && link.Contains(GameNum))
                        {
                            Trace.WriteLine(link);
                            bool contains = false;
                            foreach (string game in CurrentGames)
                            { if (game == "https://touch.unibet.com/client-start/sportsbook#event/live/" + link) { contains = true; break; } }
                            if (!contains)
                            CreateTab("https://touch.unibet.com/client-start/sportsbook#event/live/" + link);
                        }
                    }
                }
               // bool contains = false;
               // foreach(string var in GamesLinks)
            }
            catch
            {

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
                for (int i = 0; i < CurrentGames.Count; i++) { if (rgx.Match(CurrentGames[i]).Success)newCurrentGames.Add(rgx.Match(CurrentGames[i]).Value); else newCurrentGames.Add(CurrentGames[i]); }
                Debug.WriteLine("ToRemove");
                foreach (string GameNum in newCurrentGames.Except(newGamesLinks))
                {
                    CloseTab(GameNum);
                    Debug.WriteLine(GameNum);
                    
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("Unierr:" + e);
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
            catch (Exception e)
            {
                Debug.WriteLine("Unierr:" + e);
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
                    Debug.WriteLine(Task.CurrentId);
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
                    Debug.WriteLine(Task.CurrentId);
                }
                catch
                {

                }
            }
        }

        public void setAutomatic()
        {
            automatic = !automatic;
            Debug.WriteLine("Automatic is:" + automatic);
            AutoUpdate();
        }


        public void Parse()
        {
            BookmakerTennisGames.CleanData();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.UniBet;
            try
            {
                if (BookmakerWebBrowsers!=null)
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    try
                    {
                        var CurrentBrowser = this.BookmakerWebBrowsers[i];
                        CurrentBrowser.ExecuteScriptAsync(Scripts.Unibet);

                        var Player1 = CurrentBrowser.EvaluateScriptAsync("Player1");
                        var Player2 = CurrentBrowser.EvaluateScriptAsync("Player2");
                        var ScoreAll = CurrentBrowser.EvaluateScriptAsync("Score");

                        var ScorePoints = CurrentBrowser.EvaluateScriptAsync("CurrentPoints");

                        var LenArr = CurrentBrowser.EvaluateScriptAsync("GamesArray.length");
                        

                        BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
                        BookmakerTennisGames.SetGameData("", ScoreAll.ToString(), ScorePoints.ToString());
                        if (LenArr.ToString() != "undefined")
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
                        Debug.WriteLine("UnibetPrsee:" + e);
                    }
                }
            }
            catch(System.NullReferenceException)
            {
                Debug.WriteLine("Unibet have NoGames or Not Loaded");
            }
            catch(Exception e)
            {
                Debug.WriteLine("UnibetPrsErr:" + e);
            }
        }

        public void TryClick(int index)
        {
            /*
            dynamic val = BookmakerWebBrowsers[index].Document.GetElementsByClassName("category-container collapsed");
            for (int i = 0; i < val.Length; i++)
            {
                if(val[i].TextContent.Contains("GAME"))
                {
                    val[i].ChildNodes[0].Click();
                }
            }

            val = BookmakerWebBrowsers[index].Document.GetElementsByClassName("category-container");
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i].TextContent.Contains("SELECTED MARKETS"))
                {
                    val[i].ChildNodes[0].Click();
                }
            }*/
        }
    }
}
