using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

namespace ProjectGambAwesomium
{
    
    class Bet365Bookmaker : BookmakerPattern
    {
        public bool toremove = false;
        public bool toparse = false;
        public bool automatic = false;
        List<string> CurrentGames;
        private string Bet365UrlBase = "https://mobile.365sb.com/premium/?lng=1#type=Coupon;key=";
        
        public bool TestCurrentParentBrowser()
        {
            var TestBrowser=ParentBrowser.EvaluateScriptAsync("document.getElementsByClassName('on')[0].textContent");
           // TestBrowser.Wait();
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
                ParentBrowser.ExecuteScriptAsync(Scripts.Bet365GetLinksSet);
                var len = ParentBrowser.EvaluateScriptAsync("arr.length");
                var ilen = int.Parse(len.ToString());
                for (int i = 0; i < ilen; i++)
                {
                    var link = ParentBrowser.EvaluateScriptAsync("arr["+i.ToString()+"]");
                    
                    if (link != null)
                    {
                        Debug.WriteLine("getlinks");
                        GamesLinks.Add(link.ToString());
                        Debug.WriteLine(link.ToString());
                    }
                }
            }
            /*Debug.WriteLine("CurrentLinks");
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
                    Debug.WriteLine(norm);
                    GamesLinks.Add(norm);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Bet365PrsGetLinksErr" + e);
                }
            }*/
        }



        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab("https://mobile.365sb.com/premium/?lng=1#type=Coupon;key=" + GamesLinks[i], GamesLinks[i]);
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
                    Debug.WriteLine(BookmakerWebBrowsers[i].Address.ToString().Remove(0, BookmakerWebBrowsers[i].Address.ToString().IndexOf("key=") + 4));
                    if (BookmakerWebBrowsers[i].Address.ToString().IndexOf("key=") != -1)
                        CurrentGames.Add(BookmakerWebBrowsers[i].Address.ToString().Remove(0, BookmakerWebBrowsers[i].Address.ToString().IndexOf("key=") + 4));
                    else
                        CurrentGames.Add(BookmakerWebBrowsers[i].Address.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Bet365PrsGetUrlsErr" + e);
            }
        }

        public void Parse()
        {
            toparse = true;
            try
            {
                BookmakerTennisGames.CleanData();
                this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Bet365;
                if(!toremove&&BookmakerTabs!=null)
                for (int i = 0; i < BookmakerTabs.Count; i++)
                {
                    try
                    {
                        var CurrentBrowser = this.BookmakerWebBrowsers[i];
                        CurrentBrowser.ExecuteScriptAsync(Scripts.Bet365);

                        var Player1 = CurrentBrowser.EvaluateScriptAsync("Player1");
                        if (Player1 != "")
                        {


                            var Player2 = CurrentBrowser.EvaluateScriptAsync("Player2");
                            var ScoreAll = CurrentBrowser.EvaluateScriptAsync("Score");

                            var ScorePoints = CurrentBrowser.EvaluateScriptAsync("CurrentPoints");

                            var LenArr = CurrentBrowser.EvaluateScriptAsync("GamesArray.length");


                            BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
                            BookmakerTennisGames.SetGameData("", ScoreAll.ToString(), ScorePoints.ToString());

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
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine("Bet365Prsee:" + e);
                    }
                }
                
            }
            catch (System.NullReferenceException)
            {
                Debug.WriteLine("Bet365 have NoGames or Not Loaded");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Bet365Err:" + e);
            }
            toparse = false; 
        }

        public async Task CompareGames()
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
                        CreateTab(Bet365UrlBase + link,link);
                        //await Task.Delay(1000);
                    }
                }
            }
        }
        public async Task CompareGamesDes()
        {
            GetUrls();
            GetLinks();
            var GameNum = GamesLinks.Except(CurrentGames);
            TenderOpenArr(GameNum.ToList());
        }
        public async void CompareGamesSafe()
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
                        CreateTabSafe(Bet365UrlBase + link);
                      //  await Task.Delay(3000);
                    }
                }
            }
        }
        public void setAutomatic()
        {
            automatic = !automatic;
            Debug.WriteLine("Bet365Autois:"+automatic);
            AutoUpdate();
        }

        public void CompareToRemove()
        {
            GetLinks();
            GetUrls();
            toremove = true;
            if(!toparse)
            if (GamesLinks.Count > 0)
            {
                foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
                {
                    if (!GameToRemove.Contains("about:blank"))
                        CloseTab(Bet365UrlBase + GameToRemove);
                    else
                        CloseTab( GameToRemove);
                    Debug.WriteLine("ClosedBet365" + GameToRemove);
                }
                foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
                {

                    if (!GameToRemove.Contains("about:blank"))
                        CloseTab(Bet365UrlBase + GameToRemove);
                    else
                        CloseTab(GameToRemove);
                    Debug.WriteLine("ClosedBet365" + GameToRemove);
                }
                //CloseTab("https://mobile.365sb.com/premium/?lng=1#type=InPlay;");
                //CloseTab("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=;ip=1;lng=1");
                
            }
            toremove = false;
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
                    Debug.WriteLine("ClosedBet365" + GameToRemove);
                }
            }
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(Bet365UrlBase + GameToRemove);
                    Debug.WriteLine("ClosedBet365" + GameToRemove);
                }
            }
            CloseTabSafe("https://mobile.365sb.com/premium/?lng=1#type=InPlay;");
            CloseTabSafe("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=;ip=1;lng=1");
        }
        

        private async Task AutoUpdate()
        {
            int cnt = 0;
            while(automatic)
            {
                try
                {
                    CompareGames();
                    await Task.Delay(120000);
                    CompareToRemove();
                    await Task.Delay(30000);
                    await Task.Delay(60000);
                    if(!TestCurrentParentBrowser())
                    {
                        this.ParentBrowser.Load("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=13;ip=1;lng=1");
                        await Task.Delay(120000);
                    }
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
                    this.ParentBrowser.Load("https://mobile.365sb.com/premium/?lng=1#type=InPlay;key=13;ip=1;lng=1");
                    await Task.Delay(120000);
                }
                catch
                {

                }
            }
        }

        public async void TenderOpen()
        {
            int del = 0;
            if(GamesLinks!=null)
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTabDelay(Bet365UrlBase + GamesLinks[i],del+=5000);
                await Task.Delay(2000);
                //this.BookmakerWebBrowsers[i].Navigate(Bet365UrlBase + GamesLinks[i]);
            }
        }

        public async void TenderOpenArr(List<string> data)
        {
            int del = 0;
            if (data != null)
                for (int i = 0; i < data.Count; i++)
                {
                    this.CreateTabDelay(Bet365UrlBase + data[i], del += 20000);
                    await Task.Delay(2000);
                    //this.BookmakerWebBrowsers[i].Navigate(Bet365UrlBase + GamesLinks[i]);
                }
        }
    }
}
