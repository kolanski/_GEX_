using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using System.Diagnostics;

namespace ProjectGambAwesomium
{
    class MarathonBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        string MarathonBaseUrl = "https://www.betmarathon.com/en/live/view/";
        public bool toremove = false;
        public bool toparse = false;

        public void GetLinks()
        {
            GamesLinks = new List<string>();
            ParentBrowser.ExecuteScriptAsync(Scripts.MarathonGetLinksSet);
            var len = ParentBrowser.EvaluateScriptAsync("arr.length");
            
            var ilen = int.Parse(len.ToString());
            for (int i = 0; i < ilen; i++)
            {
                var link = ParentBrowser.EvaluateScriptAsync("arr[" + i.ToString() + "]");
                
                if (link != null)
                {
                    GamesLinks.Add(link.ToString());
                    Debug.WriteLine(link.ToString());
                }
            }
            /*
            Debug.WriteLine("MarCurrLinks");
            GamesLinks = new List<string>();
            var list = this.ParentBrowser.Document.GetElementsByClassName("live-selection");
            for (int i = 0; i < list.Length; i++)
            {
                try
                {
                    var toAdd = list[i].ParentNode.ParentNode.ParentNode;
                    if (toAdd.SelectFirst(".//@data-category-sport")!=null)
                    if (toAdd.SelectFirst(".//@data-category-sport").NodeValue == "Tennis")
                    {
                        string valueOfCurrGame = list[i].SelectFirst(".//@id").NodeValue;
                        Debug.WriteLine(valueOfCurrGame);
                        GamesLinks.Add(valueOfCurrGame);
                    }
                }
                catch (NullReferenceException)
                {

                }
                catch (Exception e)
                {
                    Debug.WriteLine("MarGetLinksErr:" + e);
                }
            }*/
        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab(MarathonBaseUrl + GamesLinks[i]);
                //this.BookmakerWebBrowsers[i].Load(MarathonBaseUrl + GamesLinks[i]);
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
                    Debug.WriteLine(BookmakerWebBrowsers[i].Address.ToString().Remove(0, BookmakerWebBrowsers[i].Address.ToString().IndexOf("view/") + 5));
                    CurrentGames.Add(BookmakerWebBrowsers[i].Address.ToString().Remove(0, BookmakerWebBrowsers[i].Address.ToString().IndexOf("view/") + 5));
                }
            }
            catch (Exception e)
            {

            }
        }
        private string CutPlayer(string tocut)
        {
            string ret = tocut;
            int i=0;
            while(i<ret.Length)
            {
                if (ret[i] == '.')
                {
                    ret = ret.Remove(i - 1, 2);
                    i = i - 1;
                }
                i++;
            }
            
            return ret;
        }
        public void Parse()
        {
            if(!toremove)
            try
            {
                toparse = true;
                BookmakerTennisGames.CleanData();
                this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Marathon;
                if (BookmakerWebBrowsers!=null)
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    try
                    {
                        var ParentBrowser = this.BookmakerWebBrowsers[i];
                        ParentBrowser.ExecuteScriptAsync(Scripts.Marathon);
                        ParentBrowser.ExecuteScriptAsync(@"Markets.applyView=function (c){var d=Markets.getLiveUpdatesHelper();if(d){d.fireAdditionalMarketsChanged();}var b=new EventView(c);if(this.isLoading(b)){return;}if(b.isVisible()){}else{b.show();}}");
                        ParentBrowser.ExecuteScriptAsync(@"$('.event-more-view').click();");
                        var Player1 = ParentBrowser.EvaluateScriptAsync("Player1");
                        var Player2 = ParentBrowser.EvaluateScriptAsync("Player2");
                        var ScoreAll = ParentBrowser.EvaluateScriptAsync("Score");
                        
                        var ScorePoints = ParentBrowser.EvaluateScriptAsync("CurrentPoints");
                        
                        var LenArr = ParentBrowser.EvaluateScriptAsync("GamesArray.length");
                        

                        BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
                        BookmakerTennisGames.SetGameData("", ScoreAll.ToString(), ScorePoints.ToString());

                        for (int h = 0; h < int.Parse(LenArr.ToString()); h++)
                        {
                            var SetNumber = ParentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].SetNum[0]");
                            
                            var GameNumber = ParentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].GameNum");
                            
                            var Coefficent1 = ParentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].Coef1");
                            
                            var Coefficent2 = ParentBrowser.EvaluateScriptAsync("GamesArray[" + h.ToString() + "].Coef2");
                            
                            BookmakerTennisGames.AddGames(SetNumber.ToString(), GameNumber.ToString(), Coefficent1.ToString(), Coefficent2.ToString());
                        }
                        if (Player1.ToString() != "")
                            BookmakerTennisGames.AddData();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("WilliamsPrsee:" + e);
                        toparse = false;
                    }
                    toparse = false;
                }
            }
            catch(System.NullReferenceException)
            {
                Debug.WriteLine("Marathon have NoGames or Not Loaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MarPrserr:" + ex);
            }
            toparse = false;
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
                        CreateTab(MarathonBaseUrl + link);
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
                        CreateTabSafe(MarathonBaseUrl + link);
                    }
                }
            }
        }

        public async Task CompareToRemove()
        {
            GetLinks();
            GetUrls();
            toremove = true;
            if (!toparse)
            {
                foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
                {
                    await CloseTab(MarathonBaseUrl + GameToRemove);
                    Debug.WriteLine("ClosedMar" + GameToRemove);
                }
                foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
                {
                    await CloseTab(MarathonBaseUrl + GameToRemove);
                    Debug.WriteLine("Closedmar" + GameToRemove);
                }
            }
            toremove = false;
        }

        public void CompareToRemoveSafe()
        {
            GetLinks();
            GetUrls();
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(MarathonBaseUrl + GameToRemove);
                    Debug.WriteLine("ClosedMar" + GameToRemove);
                }
            }
            foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(MarathonBaseUrl + GameToRemove);
                    Debug.WriteLine("Closedmar" + GameToRemove);
                }
            }
        }

        public void setAutomatic()
        {
            automatic = !automatic;
            Debug.WriteLine("MarAutois:" + automatic);
            AutoUpdate();
        }

        public async void TenderOpen()
        {
            GetLinks();
            if (GamesLinks != null)
                for (int i = 0; i < GamesLinks.Count; i++)
                {
                    this.CreateTab();
                    await Task.Delay(2000);
                    this.BookmakerWebBrowsers[i].Load(MarathonBaseUrl + GamesLinks[i]);
                }
        }

        private async Task AutoUpdate()
        {
            while (automatic)
            {
                int cnt = 0;
                try
                {
                    CompareGames();
                    await Task.Delay(5000);
                    CompareToRemove();
                    await Task.Delay(5000);
                    cnt++;
                    if (cnt > 60)
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
                    await Task.Delay(5000);
                    CompareToRemoveSafe();
                    await Task.Delay(5000);
                }
                catch
                {

                }
            }
        }
    }
}
