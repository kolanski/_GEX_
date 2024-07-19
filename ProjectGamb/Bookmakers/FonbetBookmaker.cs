using Gecko;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectGamb
{
    class FonbetBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        //https://live.bkfonbet.com/?locale=en#4
        public static string FonbetSet = @"var list = document.getElementsByClassName('event');
                var process = true;
                while (process)
                {
                    list = document.getElementsByClassName('event');
                    var test = list.length;
                    for (var x = 0; x < list.length; x++)
                    {
if(list[x].getElementsByClassName('detailArrowClose').length==1&&list[x].getElementsByClassName('detailArrowClose')[0].parentNode.textContent.indexOf('set')!=-1)
                        {list[x].onclick();}
                    }
                    if (test == list.length)
                        process = false;
                    
                }";
        public void GetLinks()
        {
            try
            {
                string norm;
                using (Gecko.AutoJSContext java = new Gecko.AutoJSContext(this.ParentBrowser.Window.JSContext))
                {

                    java.EvaluateScript(FonbetSet, (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                }
                Console.WriteLine(norm);

                /*Console.WriteLine("CurrentLinks");
                GamesLinks = new List<string>();
                dynamic list = this.ParentBrowser.Document.GetElementsByClassName("detailArrowClose");
                bool process = true;
                while (process)
                {
                    list = this.ParentBrowser.Document.GetElementsByClassName("detailArrowClose");
                    var test = list.Length;
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i].Click();
                    }
                    if (test == list.Length)
                        process = false;
                    Console.Write("list:" + list.Length + System.Environment.NewLine);
                }*/
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("FnbHasNogames or no browserloaded");
            }
        }

        bool IsNumber(char text)                                            //Проверка на то что это число
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
        public static string Fonbet
        {
            get
            {
                return @"var tables=document.getElementById('lineTable');
var live = tables;
var trs = live.getElementsByTagName('tr');
var istennis = false;

var structarr=[];

var structGame={'Event':''
                ,'Player1':''
                ,'Player2':''
                ,'ScoreAll':''
                ,'GamePoints':''
				,'GamesArr':[]
               };
for (var i = 0; i < trs.length; i++)
    if (trs[i].getAttribute('class') != null)
    {
        if (trs[i].getAttribute('class') == 'trSegment')
        {

            if (trs[i].textContent.indexOf('Table tennis')==-1&&trs[i].textContent.indexOf('Ten')!=-1)
            {
                var currentevent = trs[i].textContent;
                istennis = true;
            }
            else
                istennis = false;
        }
        if (istennis)
        {
            if (trs[i].getElementsByClassName('eventCellName') != null)
            {
                //console.info(i);
                var el=trs[i].getElementsByClassName('eventCellName')[0] ;

                if (el!=null)
                {
                    

                    if (el.getElementsByClassName('event')[0]!=null)
                        playerinn = el.getElementsByClassName('event')[0].childNodes[1];
                    else
                        if (el.getElementsByClassName('eventBlocked')[0]!=null)
                            playerinn = el.getElementsByClassName('eventBlocked')[0].childNodes[1];
                    //console.info(playerinn);

                    if (playerinn.length>8)

                    {
						var scoregame = el.getElementsByClassName('eventScore ')[0];
                    if (scoregame.textContent.indexOf('(') != -1)
                        scoregame = scoregame.textContent.slice(scoregame.textContent.indexOf('(')+1,scoregame.textContent.length-1);
                        var player1=playerinn.textContent.split('—')[0];
                        var player2=playerinn.textContent.split('—')[1];
                        console.info(currentevent);
                        console.info(player1);
                        if (player2[0]==' ')
                            player2=player2.slice(1,player2.length);
                        console.info(player2);
                        console.info(scoregame);
						structarr.push({'Event':currentevent
						,'Player1':player1
						,'Player2':player2
						,'ScoreAll':scoregame
						,'GamePoints':''
						,'GamesArr':[]
						});
                    }

                    else

                    {

					currentpoints = el.getElementsByClassName('eventScore ')[0];
                    if (currentpoints.textContent.indexOf('(') != -1)
                        currentpoints = currentpoints.textContent.slice(currentpoints.textContent.indexOf('(')+1,currentpoints.textContent.length-1);
                        var setnum=playerinn.textContent;
                        console.info(setnum);
                        console.info(currentpoints);
						structarr[structarr.length-1].GamePoints=currentpoints
                        var grids= trs[i].nextSibling.getElementsByClassName('grid');
                        for (var h=0;h<grids.length;h++)
                        {
                            if (grids[h].textContent.indexOf('Games')!=-1 && grids[h].textContent.indexOf('special')==-1 && grids[h].getElementsByTagName('thead')[0].getElementsByTagName('tr') != null)
                            {
                                for (var g=0;g<grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr').length;g++)
                                {
                                    var numgame = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[0].textContent.replace('Game ','');
                                    var coef1 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[1].textContent;
                                    var coef2 = grids[h].getElementsByTagName('tbody')[0].getElementsByTagName('tr')[g].getElementsByTagName('td')[2].textContent;
                                    console.info(numgame.textContent);
                                    console.info(coef1.textContent);
                                    console.info(coef2.textContent);
									structarr[structarr.length-1].GamesArr.push({'SetNumber':setnum[0],'GameNumber':numgame,'Coefficent1':coef1,'Coefficent2':coef2});
                                }
                            }

                        }

                    }
                }
            }
        }

    }";
            }
        }
        Stopwatch sw = new Stopwatch();
        public void Parse()
        {
            
            sw.Start();
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
                    
                    java.EvaluateScript(Fonbet, (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                }
                Console.WriteLine(norm);
                using (Gecko.AutoJSContext java = new Gecko.AutoJSContext(this.ParentBrowser.Window.JSContext))
                {

                    java.EvaluateScript(@"structarr.length", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                    var len = int.Parse(norm);
                    for (int i = 0; i < len; i++)
                    {
                        java.EvaluateScript(@"structarr[" + i.ToString() + "]." + "Player1", (nsISupports)this.ParentBrowser.Window.DomWindow, out norm);
                        var Player1=norm;
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
                Console.WriteLine(norm);
                Console.WriteLine(sw.ElapsedMilliseconds);
                sw.Stop();
                sw.Reset();
            }
            catch (Exception e)
            {
                Console.WriteLine("Bet365PrsGetLinksErr" + e);
            }
            /*
            try
            {
                if (BookmakerTennisGames == null)
                    BookmakerTennisGames = new TennisGames();
                BookmakerTennisGames.CleanData();
                BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Fonbet;
                var Browser = this.ParentBrowser.Document;
                var Tables = Browser.EvaluateXPath("//table[@id='lineTable']");
                var live = Tables.GetNodes().ElementAt(0);
                var trs = live.EvaluateXPath(".//tr");
                var tolist = trs.GetNodes();
                var lst = tolist.ToList();
                Console.WriteLine(tolist.Count());

                bool istennis = false;
                string currentevent = "";
                string scoregame = "";
                string scoreset = "";
                string playerinn = "";
                string player1;
                string player2;
                string numgame;
                string coef1;
                string coef2;

                for (int i = 0; i < lst.Count; i++)
                {
                    try
                    {
                        if (lst[i].SelectSingle("./@class") != null)
                        {
                            if (lst[i].SelectSingle("./@class").NodeValue == "trSegment")
                            {
                                if (lst[i].TextContent.Contains("Ten"))
                                {
                                    currentevent = lst[i].TextContent;
                                    istennis = true;
                                }
                                else
                                    istennis = false;
                            }
                        }
                        if (istennis)
                        {
                            if (lst[i].SelectSingle(".//td[@class='eventCellName']") != null)
                            {
                                scoregame = lst[i].SelectSingle(".//td[@class='eventCellName']/div[2]").TextContent;
                                if (scoregame.IndexOf("(") != -1)
                                    scoregame = scoregame.Remove(0, scoregame.IndexOf("(") + 1).Replace(")", "").Replace("-", " ");
                                playerinn = lst[i].SelectSingle(".//td[@class='eventCellName']/div[1]/text()").TextContent;
                            }
                            if (lst[i].SelectSingle(".//td[@class='eventCellName eventCellNameSub']") != null)
                            {

                                scoreset = lst[i].SelectSingle(".//td[@class='eventCellName eventCellNameSub']/div[2]").TextContent;
                                var activescore = scoreset;
                                if (scoreset.IndexOf("(") != -1)
                                    scoreset = scoreset.Remove(0, scoreset.IndexOf("(") + 1).Replace(")", "").Replace("*", "").Replace("-", " ").Replace(")", "");

                                var numactivegame = numberinstring(activescore.Remove(activescore.IndexOf("(")));
                                var isactivegame = numberinstring(activescore.Remove(0, activescore.IndexOf("(")));

                                if (lst[i].NextSibling.EvaluateXPath(".//table[@class='grid']").GetNodes().ToList() != null)
                                {
                                    var bets = lst[i].NextSibling.EvaluateXPath(".//table[@class='grid']").GetNodes().ToList();
                                    foreach (var element in bets)
                                    {
                                        if (element.TextContent.Contains("Games") && !element.TextContent.Contains("special") && element.SelectSingle(" .//thead/tr/th[2]") != null)
                                        {
                                            player1 = element.SelectSingle(" .//thead/tr/th[2]").TextContent;
                                            player2 = element.SelectSingle(" .//thead/tr/th[3]").TextContent;
                                            numgame = element.SelectSingle(" .//tbody/tr[1]/td[1]").TextContent.Replace("Game", "");
                                            coef1 = element.SelectSingle(" .//tbody/tr[1]/td[2]").TextContent;
                                            coef2 = element.SelectSingle(" .//tbody/tr[1]/td[3]").TextContent;
                                            BookmakerTennisGames.SetPlayers(player1, player2);
                                            BookmakerTennisGames.SetGameData("", scoregame, scoreset);
                                            BookmakerTennisGames.AddGames((countnumberinstring(scoregame) / 2).ToString(), numgame, coef1, coef2);

                                            if (element.SelectSingle(" .//tbody/tr[2]/td[1]") != null)
                                            {
                                                numgame = element.SelectSingle(" .//tbody/tr[2]/td[1]").TextContent.Replace("Game", "");
                                                coef1 = element.SelectSingle(" .//tbody/tr[2]/td[2]").TextContent;
                                                coef2 = element.SelectSingle(" .//tbody/tr[2]/td[3]").TextContent;
                                                BookmakerTennisGames.AddGames((countnumberinstring(scoregame) / 2).ToString(), numgame, coef1, coef2);
                                            }
                                            BookmakerTennisGames.AddData();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }*/
        }

        internal void Automatic()
        {
            automatic = !automatic;
            Console.WriteLine("Mar Automatic is:" + automatic);
            AutoUpdate();
        }
        private async Task AutoUpdate()
        {
            while (automatic)
            {
                try
                {
                    
                    await Task.Delay(15000);
                    GetLinks();
                }
                catch
                {

                }
            }
        }
    }
}
