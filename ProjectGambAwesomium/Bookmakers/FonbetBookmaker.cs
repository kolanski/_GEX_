using Gecko;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGambAwesomium
{
    class FonbetBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        //https://live.bkfonbet.com/?locale=en#4
        Gecko.GeckoWebBrowser ParentBrowser;
        public void SetUpBrowser(Gecko.GeckoWebBrowser Browser)
        {
            ParentBrowser = Browser;
        }
        public void Navigate(string Url)
        {
            ParentBrowser.Navigate(Url);
        }
        public bool TestCurrentParentBrowser()
        {
            return true;
        }
        public async void GetUrls()
        {  //
           // try
           // {
           //
           //     this.ParentBrowser.ExecuteScriptAsync(Scripts.FonbetSettings);
           //     await Task.Delay(1000);
           //     this.ParentBrowser.ExecuteScriptAsync(@"document.getElementById('langRus').onclick()");
           //     await Task.Delay(1000);
           //     this.ParentBrowser.ExecuteScriptAsync(@"document.getElementById('langEng').onclick()");
           // }
           // catch (System.NullReferenceException)
           // {
           //     Debug.WriteLine("FnbHasNogames or no browserloaded");
           // }
        }
        public void GetLinks()
        {
            try
            {
                string norm;
                using (Gecko.AutoJSContext java = new Gecko.AutoJSContext(this.ParentBrowser.Window.JSContext))
                {

                    java.EvaluateScript(Scripts.FonbetSet, (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                }
                Console.WriteLine(norm);
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("FnbHasNogames or no browserloaded");
            }

        }

        bool IsNumber(char text)                                            //Проверка на то что это число263556     
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text.ToString());
        }

        private int numberinstring(string inn)
        {
            int count = 0;
            for (int i = 0; i < inn.Length; i++)
            {
                if (IsNumber(inn[i]))
                    count += int.Parse(inn[i].ToString());
            }
            return count;
        }

        private int countnumberinstring(string inn)
        {
            int count = 0;
            for (int i = 0; i < inn.Length; i++)
            {
                if (IsNumber(inn[i]))
                    count++;
            }
            return count;
        }

        public void Parse()
        {
           //  sw.Start();
             if (BookmakerTennisGames == null)
                    BookmakerTennisGames = new TennisGames();
                BookmakerTennisGames.CleanData();
                BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Fonbet;
                try
                {
                    string norm;
                    string sss;
                    using (Gecko.AutoJSContext java = new Gecko.AutoJSContext(this.ParentBrowser.Window.JSContext))
                    {

                        java.EvaluateScript(Scripts.Fonbet, (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                    }
                    //   Console.WriteLine(norm);
                    using (Gecko.AutoJSContext java = new Gecko.AutoJSContext(this.ParentBrowser.Window.JSContext))
                    {

                        java.EvaluateScript(@"structarr.length", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                        var len = int.Parse(norm);
                        for (int i = 0; i < len; i++)
                        {
                            java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "Player1", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                            var Player1 = norm;
                            java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "Player2", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                            var Player2 = norm;
                            java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "ScoreAll", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                            var ScoreAll = norm;
                            java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "GamePoints", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                            var ScorePoints = norm;
                            java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "GamesArr.length", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                            var LenArr = norm;
                            BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
                            BookmakerTennisGames.SetGameData("", ScoreAll.ToString(), ScorePoints.ToString());

                            for (int h = 0; h < int.Parse(LenArr.ToString()); h++)
                            {
                                java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].SetNumber", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                                var SetNumber = norm;
                                java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].GameNumber", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                                var GameNumber = norm;
                                java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent1", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                                var Coefficent1 = norm;
                                java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent2", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                                var Coefficent2 = norm;

                                BookmakerTennisGames.AddGames(SetNumber.ToString(), GameNumber.ToString(), Coefficent1.ToString(), Coefficent2.ToString());
                            }
                            if (Player1.ToString() != "")
                                BookmakerTennisGames.AddData();

                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Bet365PrsGetLinksErr" + e);
                }
               // Console.WriteLine(norm);
                //Console.WriteLine(sw.ElapsedMilliseconds);
                //sw.Stop();
                //sw.Reset();

            //if (BookmakerTennisGames == null)
            //    BookmakerTennisGames = new TennisGames();
            //BookmakerTennisGames.CleanData();
            //BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Fonbet;
            ////GetLinks();
            //
            //try
            //{
            //    var CurrentBrowser = this.ParentBrowser;
            //    ParentBrowser.ExecuteScriptAsync(Scripts.FonbetSet);
            //    CurrentBrowser.ExecuteScriptAsync(Scripts.Fonbet);
            //    var list = CurrentBrowser.EvaluateScriptAsync("structarr.length");
            //    //list.Wait();
            //    if (list.ToString()!="undefined"&&int.Parse(list.ToString()) > 0)
            //    {
            //
            //        for (int i = 0; i < int.Parse(list.ToString()); i++)
            //        {
            //            var Player1 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "Player1").ToString();
            //            var Player2 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "Player2").ToString();
            //            var ScoreAll = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "ScoreAll").ToString();
            //            
            //            var ScorePoints = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamePoints").ToString();
            //            
            //            var LenArr = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr.length").ToString();
            //            
            //            // ScoreAll += CurrentGames;
            //            BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
            //            BookmakerTennisGames.SetGameData("", ScoreAll.ToString(), ScorePoints.ToString());
            //
            //            for (int h = 0; h < int.Parse(LenArr.ToString()); h++)
            //            {
            //                var SetNumber = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].SetNumber").ToString();
            //
            //                var GameNumber = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].GameNumber").ToString();
            //
            //                var Coefficent1 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent1").ToString();
            //
            //                var Coefficent2 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent2").ToString();
            //                
            //                BookmakerTennisGames.AddGames(SetNumber.ToString(), GameNumber.ToString(), Coefficent1.ToString(), Coefficent2.ToString());
            //            }
            //            if (Player1.ToString() != "")
            //                BookmakerTennisGames.AddData();
            //        }
            //    }
            //}
            //catch
            //{
            //
            //}
        }

        public void Automatic()
        {
            automatic = !automatic;
            Debug.WriteLine("Fon Automatic is:" + automatic);
            AutoUpdate();
        }
        private async Task AutoUpdate()
        {
            while (automatic)
            {
                try
                {
                    
                   await Task.Delay(60000);
                   GetLinks();
                }
                catch
                {

                }
            }
           
        }
    }
}
