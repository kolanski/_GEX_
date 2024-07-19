using BTware_TestParsings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetTableware
{
    public class Olimp
    {
        public tennisData olimpTdata;
        public logS log;
        //Stopwatch sw;
        public Awesomium.Windows.Forms.WebControl myweb;
        public void loadpage(Awesomium.Windows.Forms.WebControl toload)
        {
            myweb = toload;
        }
        private int getbase(string inn,string setnum)
        {
            int sets;
            inn = inn.Replace(",", "");
            int set1;
            int set2;
            int tmp1, tmp2, tmp3, tmp4;
            int.TryParse(inn[0].ToString(), out set1);
            int.TryParse(inn[2].ToString(), out set2);
            sets = int.Parse(setnum) - 1;
            if (inn.Length > 5)
            {
                switch (sets)
                {
                    case 0:
                        {
                            return 0;
                        };
                    case 1:
                        {
                            int.TryParse(inn[0].ToString(), out tmp1);
                            int.TryParse(inn[2].ToString(), out tmp2);
                            return tmp1 + tmp2;
                        };
                    case 2:
                        {
                            int.TryParse(inn[0].ToString(), out tmp1);
                            int.TryParse(inn[2].ToString(), out tmp2);
                            int.TryParse(inn[4].ToString(), out tmp3);
                            int.TryParse(inn[6].ToString(), out tmp4);
                            return tmp1 + tmp2 + tmp3 + tmp4;
                        }
                    case 3:
                        {
                            int tmp5, tmp6;
                            int.TryParse(inn[0].ToString(), out tmp1);
                            int.TryParse(inn[2].ToString(), out tmp2);
                            int.TryParse(inn[4].ToString(), out tmp3);
                            int.TryParse(inn[6].ToString(), out tmp4);
                            int.TryParse(inn[8].ToString(), out tmp5);
                            int.TryParse(inn[10].ToString(), out tmp6);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6;
                        }
                    case 4:
                        {
                            int tmp5, tmp6, tmp7, tmp8;
                            int.TryParse(inn[0].ToString(), out tmp1);
                            int.TryParse(inn[2].ToString(), out tmp2);
                            int.TryParse(inn[4].ToString(), out tmp3);
                            int.TryParse(inn[6].ToString(), out tmp4);
                            int.TryParse(inn[8].ToString(), out tmp5);
                            int.TryParse(inn[10].ToString(), out tmp6);
                            int.TryParse(inn[12].ToString(), out tmp7);
                            int.TryParse(inn[14].ToString(), out tmp8);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6 + tmp7 + tmp8;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }
            else
                return 0;
        

        }
        public void Parsedoc()
        {

            if (olimpTdata==null)
                olimpTdata = new tennisData();
            olimpTdata.clear();
            try
            {
                var CurrentBrowser = this.myweb;
                if (CurrentBrowser.IsDocumentReady)
                CurrentBrowser.Invoke((MethodInvoker)delegate
                {
                    CurrentBrowser.ExecuteJavascript(@"if (document.getElementsByClassName('msbtn1')[3] != undefined)
            document.getElementsByClassName('msbtn1')[3].click();
        //Olimpbet js
        structarr = [];
        if (document.getElementById('betline') != undefined && document.getElementById('betline').getElementsByClassName('smallwnd2')[0] != undefined) {
            
            var table = document.getElementById('betline').getElementsByClassName('smallwnd2')[0];
            var games = table.getElementsByClassName('hi');
            //cycle
            for (var s = 0; s < games.length; s++) {
                var bets = games[s].nextSibling.nextSibling;
                var players = '';
                var len = games[s].children[1].children[0].children[0].childNodes.length;
                for (var g = 0; g < len; g++) {
                    if (games[s].children[1].children[0].children[0].childNodes[g].nodeName == '#text') {
                        
                        players = games[s].children[1].children[0].children[0].childNodes[g];
            //console.log(players.textContent.replace(/\d/g, '').slice(1));
                    }
                }                ;
                players = players.textContent.replace(/\d/g, '').slice(1).split(' - ');
                var score = ['', '', ''];
                if (games[s].getElementsByClassName('txtmed')[0] != undefined)
                    score = games[s].getElementsByClassName('txtmed')[0].textContent.split(/[()]/);
                //console.log(score);
                structarr.push(
                    {
                        'Event': '',
                        'Player1': players[0],
                        'Player2': players[1],
                        'ScoreAll': score.slice(1, 2).toString(),
                        'GamePoints': score[2],
                        'GamesArr': []
                    });
                var betgame = 0;
                if (bets.getElementsByTagName('div')[1] != undefined)
                    betgame = bets.getElementsByTagName('div')[1].children;
                if (betgame != 0)
                    for (var i = 0; i < betgame.length; i++) {
                        if ((betgame[i].textContent.indexOf('сет') != -1 && betgame[i].textContent.indexOf('гейм') != -1)) {
                            //console.log(betgame[i].textContent + ' ' + betgame[i].textContent.length);
                            if (betgame[i].textContent.length == 15 || betgame[i].textContent.length == 16) {
                                //console.log(betgame[i].textContent);
                                if (betgame[i + 3].textContent!=undefined&& betgame[i + 3].textContent.match(/\d+.\d+/g)!=undefined) {
                                    var coef1 = betgame[i + 2].textContent.match(/\d+.\d+/g)[0];
                                    var coef2 = betgame[i + 3].textContent.match(/\d+.\d+/g)[0];
                                    var setnum = betgame[i].textContent.match(/\d{2}|\d/ig);
                                    //console.log(coef1);
                                    //console.log(coef2);
                                    
                                    structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': setnum[0], 'GameNumber': setnum[1], 'Coefficent1': coef1, 'Coefficent2': coef2 });
                                }
                            }
                        }
                    }                ;
            }            ;
        }
        else {

        }
        ;");

                    // CurrentBrowser.ExecuteScriptAsync(Scripts.Fonbet);
                    var list = CurrentBrowser.ExecuteJavascriptWithResult("structarr.length");
                    strTevent tmpTevent = new strTevent();
                    listofTgames tmpTgames = new listofTgames();
                    //list.Wait();
                    if (list.ToString() != "undefined" && int.Parse(list.ToString()) > 0&&CurrentBrowser.IsDocumentReady)
                    {

                        for (int i = 0; i < int.Parse(list.ToString()); i++)
                        {
                            var Player1 = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "Player1").ToString();
                            var Player2 = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "Player2").ToString();
                            var ScoreAll = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "ScoreAll").ToString();

                            var ScorePoints = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "GamePoints").ToString();
                            Console.WriteLine(ScorePoints);
                            var LenArr = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "GamesArr.length").ToString();

                            // ScoreAll += CurrentGames;
                            tmpTevent = new strTevent();
                            tmpTgames = new listofTgames();
                            tmpTevent._Player1 = Player1.ToString();
                            tmpTevent._Player2 = Player2.ToString();
                            tmpTevent.score = ScoreAll;
                            if(LenArr!="undefined")
                            for (int h = 0; h < int.Parse(LenArr); h++)
                            {
                                var SetNumber = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].SetNumber").ToString();

                                var GameNumber = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].GameNumber").ToString();

                                var Coefficent1 = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent1").ToString();

                                var Coefficent2 = CurrentBrowser.ExecuteJavascriptWithResult("structarr[" + i.ToString() + "]." + "GamesArr[" + h.ToString() + "].Coefficent2").ToString();

                                if (getbase(ScoreAll, (int.Parse(SetNumber) + 1).ToString()) !=(getbase(ScoreAll, SetNumber) + int.Parse(GameNumber)-1))
                                tmpTgames.AddGame((getbase(ScoreAll, SetNumber) + int.Parse(GameNumber)).ToString(), Coefficent1, Coefficent2);
                                // BookmakerTennisGames.AddGames(SetNumber.ToString(), GameNumber.ToString(), Coefficent1.ToString(), Coefficent2.ToString());
                            }
                            if (Player1.ToString() != "")
                                olimpTdata.add(tmpTevent, tmpTgames);
                        }
                    }
                    CurrentBrowser.ExecuteJavascript(@"if(document.getElementById('refrate')!=undefined&& document.getElementById('refrate').previousSibling.previousSibling!=undefined)
        document.getElementById('refrate').previousSibling.previousSibling.click();");
                    // ParentBrowser.ExecuteScriptAsync("$('a.btn.refresh').click();");
                });
            }
            catch
            {

            }
        }
    }
}
