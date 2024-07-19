using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGambAwesomium
{
    class BetCityBookmaker: BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        public async void NavigateEvents()
        {
            try
            {
                this.ParentBrowser.Load("http://www.betsbc.com/live/line.php");
                await Task.Delay(5000);
                this.ParentBrowser.ExecuteScriptAsync("$('a.uncheck_all').click();$('a[rel=2]').click();setTimeout($('a.btn.f2').click(), 1000);");
                   // this.ParentBrowser.webView.ExecuteJavascript(String.Format("$('a.btn.f2').click();"));
            }
            catch
            {

            }
        }
        public void Parse()
        {
            if (BookmakerTennisGames == null)
                BookmakerTennisGames = new TennisGames();
            BookmakerTennisGames.CleanData();
            BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.BetCity;

            try
            {
                var CurrentBrowser = this.ParentBrowser;
                CurrentBrowser.ExecuteScriptAsync(Scripts.BetCity);
                
               // CurrentBrowser.ExecuteScriptAsync(Scripts.Fonbet);
                var list = CurrentBrowser.EvaluateScriptAsync("structarr.length");
                //list.Wait();
                if (list.ToString() != "undefined" && int.Parse(list.ToString()) > 0)
                {

                    for (int i = 0; i < int.Parse(list.ToString()); i++)
                    {
                        var Player1 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "Player1").ToString();
                        var Player2 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "Player2").ToString();
                        var ScoreAll = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "ScoreAll").ToString();

                        var ScorePoints = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamePoints").ToString();

                        var LenArr = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr.length").ToString();

                        // ScoreAll += CurrentGames;
                        BookmakerTennisGames.SetPlayers(Player1.ToString(), Player2.ToString());
                        BookmakerTennisGames.SetGameData("", ScoreAll.ToString(), ScorePoints.ToString());

                        for (int h = 0; h < int.Parse(LenArr.ToString()); h++)
                        {
                            var SetNumber = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].SetNumber").ToString();

                            var GameNumber = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].GameNumber").ToString();

                            var Coefficent1 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent1").ToString();

                            var Coefficent2 = CurrentBrowser.EvaluateScriptAsync("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent2").ToString();

                            BookmakerTennisGames.AddGames(SetNumber.ToString(), GameNumber.ToString(), Coefficent1.ToString(), Coefficent2.ToString());
                        }
                        if (Player1.ToString() != "")
                            BookmakerTennisGames.AddData();
                    }
                }
               // ParentBrowser.ExecuteScriptAsync("$('a.btn.refresh').click();");
            }
            catch
            {

            }
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
                    NavigateEvents();
                }
                catch
                {

                }
            }

        }
    }
}
