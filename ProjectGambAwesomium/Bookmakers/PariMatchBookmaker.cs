using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using Awesomium.Core;
using System.Diagnostics;

namespace ProjectGambAwesomium
{
    class PariMatchBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        private string PariMatchUrlBase = "http://www.parimatchru.com/en/bet.html?hl=";
        
        public void GetLinks()
        {
            GamesLinks = new List<string>();
            try
            {
                string norm;
                string len;


                    len =ParentBrowser.EvaluateScriptAsync(@"var list =document.getElementsByClassName('sport tennis')[0];var test=list.getElementsByClassName('dt processed');test.length");
                    for (int i = 0; i < int.Parse(len); i++)
                    {
                        if (int.Parse(len) != 0)
                        {
                            norm =ParentBrowser.EvaluateScriptAsync(@"var list =document.getElementsByClassName('sport tennis')[0];var test=list.getElementsByClassName('dt processed');test[" + i.ToString() + "].getAttribute('evno').toString();");
                            Debug.WriteLine(norm);
                            if (norm != "undefined")
                            GamesLinks.Add(norm);
                        }
                    }
                    len=ParentBrowser.EvaluateScriptAsync(@"var list =document.getElementsByClassName('sport tennis')[1];var test=list.getElementsByClassName('dt processed');test.length");
                    for (int i = 0; i < int.Parse(len); i++)
                    {
                        if (int.Parse(len) != 0)
                        {
                            norm=ParentBrowser.EvaluateScriptAsync(@"var list =document.getElementsByClassName('sport tennis')[1];var test=list.getElementsByClassName('dt processed');test[" + i.ToString() + "].getAttribute('evno').toString();");
                            if (norm.Length > 2)
                            {
                                Debug.WriteLine(norm);
                                if (norm != "undefined")
                                GamesLinks.Add(norm);
                            }
                        }
                    }
                
            }
            catch (Exception e)
            {
                Debug.WriteLine("Parimatcherr" + e);
            }
            /*
            Debug.WriteLine("CurrentLinks");
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
                            Debug.WriteLine(norm);
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
                                Debug.WriteLine(norm);
                                GamesLinks.Add(norm);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Parimatcherr" + e);
            }
            */
        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab(PariMatchUrlBase + GamesLinks[i]);
               // this.BookmakerWebBrowsers[i].Load();
            }
        }

        public void GetUrls()
        {
            CurrentGames = new List<string>();
            Debug.WriteLine("CurrentGames");
            try
            {
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    Debug.WriteLine(BookmakerWebBrowsers[i].Address.ToString().Remove(0, BookmakerWebBrowsers[i].Address.ToString().IndexOf("=") + 1));
                    CurrentGames.Add(BookmakerWebBrowsers[i].Address.ToString().Remove(0, BookmakerWebBrowsers[i].Address.ToString().IndexOf("=") + 1));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Bet365PrsGetUrlsErr" + e);
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
                        var Player1 = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('l')[0].childNodes[0].textContent");
                        if (Player1 != "undifined")
                        {
                            Debug.WriteLine(Player1.ToString());
                            var Player2 = BookmakerWebBrowsers[i].EvaluateScriptAsync(@"var players=document.getElementsByClassName('l')[0].childNodes;
if(players[1].textContent.length>3)players[1].textContent;
if(players[2].textContent.length>3)players[2].textContent;
if(players[3].textContent.length>3)players[3].textContent");
                            Debug.WriteLine(Player2.ToString());
                            string ScoreAll = "";
                            string ScoreGame = "0 0";
                            var playersdata = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('l')[1].textContent").ToString();
                            if (!playersdata.Contains(":"))
                            {
                                ScoreAll = playersdata.Remove(0, playersdata.IndexOf('(') + 1).Replace("-", " ").Replace(",", " ").Replace(")", "");
                            }
                            else
                            {
                                var toremove1 = playersdata.Remove(0, playersdata.IndexOf("(") + 1).Replace("-", " ").Replace(",", " ");
                                var toremove2 = toremove1.Remove(toremove1.IndexOf(")"), toremove1.Length - toremove1.IndexOf(")"));
                                ScoreAll = toremove2.Replace("(", "");
                                ScoreGame = toremove1.Remove(0, toremove1.IndexOf(")") + 2).Replace(":", " ");
                            }
                            BookmakerTennisGames.SetPlayers(Player1, Player2);
                            BookmakerTennisGames.SetGameData("", ScoreAll, ScoreGame);
                            var GamesList = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('dyn').length.toString()").ToString();
                            for (int j = 0; j < int.Parse(GamesList); j++)
                            {
                                var testTotest = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('dyn')[" + j.ToString() + "].textContent").ToString();
                                if (testTotest.Contains("Set ") && testTotest.Contains("game ") && !testTotest.Contains("point") && !testTotest.Contains("score") && !testTotest.Contains("Who will "))
                                {
                                    var SetGameText = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('dyn')[" + j.ToString() + "].childNodes[1].textContent").ToString();
                                    var set = SetGameText.Substring(SetGameText.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }), 1);
                                    var game = SetGameText.Substring(SetGameText.LastIndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }) - 1, 2).Replace(" ", "");
                                    var coef1 = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('dyn')[" + j.ToString() + "].childNodes[3].childNodes[1].textContent").ToString();
                                    var coef2 = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('dyn')[" + j.ToString() + "].childNodes[5].childNodes[1].textContent").ToString();
                                    if(coef1==Player1)
                                    {
                                        coef1 = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('dyn')[" + j.ToString() + "].childNodes[3].childNodes[3].textContent").ToString();
                                        coef2 = BookmakerWebBrowsers[i].EvaluateScriptAsync("document.getElementsByClassName('dyn')[" + j.ToString() + "].childNodes[5].childNodes[3].textContent").ToString();
                                    }
                                    if(coef1!="undifined"&&coef2!="undifined")
                                    BookmakerTennisGames.AddGames(set, game, coef1, coef2);
                                }

                            }
                            BookmakerTennisGames.AddData();
                        }
                    }
                    catch (Exception ep)
                    {
                        Debug.WriteLine("ParimatchprsErr:" + ep);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
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
                        Debug.WriteLine("ToAdd");
                        Debug.WriteLine(link);
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
                        Debug.WriteLine("ToAdd");
                        Debug.WriteLine(link);
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
                Debug.WriteLine("ClosedParimatch" + GameToRemove);
            }
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {

                CloseTab(PariMatchUrlBase + GameToRemove);
                Debug.WriteLine("ClosedParimatch" + GameToRemove);
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
                    Debug.WriteLine("ClosedParimatch" + GameToRemove);
                }
            }
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(PariMatchUrlBase + GameToRemove);
                    Debug.WriteLine("ClosedParimatch" + GameToRemove);
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
                    this.BookmakerWebBrowsers[i].Load(PariMatchUrlBase + GamesLinks[i]);
                }
        }

        public void Automatic()
        {
            automatic = !automatic;
            Debug.WriteLine("ParimatchAutois:" + automatic);
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
                    await Task.Delay(10000);
                    CompareToRemove();
                    await Task.Delay(10000);
                    cnt++;
                    if (cnt > 30)
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
