using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BTware_TestParsings;
using Gecko;
namespace BTware_TestParsings
{


    public struct evnts
    {
        public string numgam;
        public string koef1;
        public string koef2;
    }
    public struct tableevnt
    {
        public string _event;
        public string _Player1;
        public string _Player2;
        public string score;
    }
    public class evntslst
    {
        public int els;
        evnts tmp = new evnts();
        List<evnts> list = new List<evnts>();     //Контейнер списка логов
        public void Add(string numgam, string kf1, string kf2)               //Метод для добавления записей в лог
        {
            tmp.numgam = numgam;
            tmp.koef1 = kf1;
            tmp.koef2 = kf2;
            list.Add(tmp);

            els++;
        }
        public evnts D = new evnts();

        public evnts ToPrint(int elsnum)           //Метод для вывода записей из лога
        {
            if (els > 0 && elsnum < els)
            {
                evnts array = list[elsnum];

                return array;
            }
            else
            {
                D.numgam = "";
                D.koef1 = "";
                D.koef2 = "";
                return D;
            };
        }
    }
    public struct ttl
    {
        public tableevnt cur;
        public evntslst games;
    }

    public class Fonbet : BETPatrn
    {
        public Gecko.GeckoWebBrowser geckoWebBrowser1;
        public bool parsecurr;
        public tennisData fonbetTdata;
        public new logS log;
        Stopwatch sw;
        new HtmlDocument ParDoc;
        string Doctext;
        public void loadpage(string toload)
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            using (StreamReader reader = new StreamReader(toload, System.Text.Encoding.UTF8))
            {
                Doctext = reader.ReadToEnd();
                reader.Close();
            }
            ParDoc.LoadHtml(Doctext);
            log.Add("Create");
        }
        public void loadpage(Gecko.GeckoWebBrowser gecko)
        {
            geckoWebBrowser1 = gecko;
        }
        public void loadpage1(string toload)
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            Doctext = toload;
            //ParDoc.LoadHtml("<html>"+Doctext+"</html>");
            ParDoc.LoadHtml(Doctext);
            log.Add("Create");
        }
        bool IsNumber(char text)                                            //Проверка на то что это число
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text.ToString());
        }
        public void parcePlayers(string inn, ref string player1, ref string player2)
        {
            bool toplayer2 = false;
            string tmp = "";
            for (int i = 0; i < inn.Length; i++)
            {
                if (IsNumber(inn[i]))
                    continue;
                else
                    if (toplayer2)
                    {
                        tmp += inn[i];
                    }
                    else
                        if (inn[i].Equals(' ') && inn[i + 1].Equals('—'))
                        {
                            toplayer2 = true;
                            i++;
                            player1 = tmp;
                            tmp = "";

                        }
                        else
                        {
                            if (!toplayer2)
                                tmp += inn[i];
                        }

            }
            player2 = tmp.Replace("-", " ");
        }

        private void parsegames(string toparse, ref string numgam, ref string koef1, ref string koef2)
        {
            //Кто выиграет 4-й гейм 1.35 2.95 
            string tmp = toparse;
            bool game = true;
            bool kf1 = false;
            bool kf2 = false;
            int k = 0;
            tmp = tmp.Replace("Кто ", "");
            tmp = tmp.Replace("выиграет ", "");
            tmp = tmp.Replace("гейм", " ");
            tmp = tmp.Replace("-й", "");
            for (int i = 0; i < tmp.Length; i++)
            {
                if (game && IsNumber(tmp[i]))
                {
                    if (IsNumber(tmp[i + 1]))
                    {
                        numgam += tmp[i].ToString() + tmp[i + 1].ToString();
                        game = false;
                        kf1 = true;
                    }
                    else
                    {
                        numgam += tmp[i];
                        game = false;
                        kf1 = true;
                    }
                    i += 3;
                }
                if (kf1 && IsNumber(tmp[i]))
                {

                    while (tmp[i] != 32)
                    {
                        k = tmp[i];
                        koef1 += tmp[i];
                        i++;
                    }
                    kf1 = false;
                    kf2 = true;
                }
                if (kf2 && IsNumber(tmp[i]))
                {
                    while (tmp[i] != 32)
                    {
                        koef2 += tmp[i];
                        i++;
                    }
                    kf2 = false;
                }
            }
        }
        private int parsescore(string inn)
        {
            //Score:1:1 (6-7 6-4 0-0)
            //      0123456789
            int sets;
            int set1;
            int set2;
            int tmp1, tmp2, tmp3, tmp4;
            int.TryParse(inn[0].ToString(), out set1);
            int.TryParse(inn[2].ToString(), out set2);
            sets = set1 + set2;
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
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            return tmp1 + tmp2;
                        };
                    case 2:
                        {
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[9].ToString(), out tmp3);
                            int.TryParse(inn[11].ToString(), out tmp4);
                            return tmp1 + tmp2 + tmp3 + tmp4;
                        }
                    case 3:
                        {
                            int tmp5, tmp6;
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[9].ToString(), out tmp3);
                            int.TryParse(inn[11].ToString(), out tmp4);
                            int.TryParse(inn[13].ToString(), out tmp5);
                            int.TryParse(inn[15].ToString(), out tmp6);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6;
                        }
                    case 4:
                        {
                            int tmp5, tmp6, tmp7, tmp8;
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[9].ToString(), out tmp3);
                            int.TryParse(inn[11].ToString(), out tmp4);
                            int.TryParse(inn[13].ToString(), out tmp5);
                            int.TryParse(inn[15].ToString(), out tmp6);
                            int.TryParse(inn[17].ToString(), out tmp7);
                            int.TryParse(inn[19].ToString(), out tmp8);
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

        public string printdoc()                                            //Вывод текущего загруженного документа
        {
            return Doctext;
        }
        public bool started(string inn)
        {
            //Score:0:0 (0:1) (15:15)
            int total = 0;
            for (int i = inn.Length - 1; i > 2; i--)
            {
                if (inn[i] != '(')
                {
                    if (IsNumber(inn[i]))
                        total += int.Parse(inn[i].ToString());
                }
                else
                    i = 2;
            }
            if (total != 0)
                return true;
            else
                return false;
        }
        private int parsescoreall(string inn)
        {
            //Score:1:1 (6-7 6-4 0-0)
            //      0123456789
            int sets;
            int set1;
            int set2;
            int tmp1, tmp2, tmp3, tmp4;
            int.TryParse(inn[0].ToString(), out set1);
            int.TryParse(inn[2].ToString(), out set2);
            sets = set1 + set2 + 1;
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
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            return tmp1 + tmp2;
                        };
                    case 2:
                        {
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[9].ToString(), out tmp3);
                            int.TryParse(inn[11].ToString(), out tmp4);
                            return tmp1 + tmp2 + tmp3 + tmp4;
                        }
                    case 3:
                        {
                            int tmp5, tmp6;
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[9].ToString(), out tmp3);
                            int.TryParse(inn[11].ToString(), out tmp4);
                            int.TryParse(inn[13].ToString(), out tmp5);
                            int.TryParse(inn[15].ToString(), out tmp6);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6;
                        }
                    case 4:
                        {
                            int tmp5, tmp6, tmp7, tmp8;
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[9].ToString(), out tmp3);
                            int.TryParse(inn[11].ToString(), out tmp4);
                            int.TryParse(inn[13].ToString(), out tmp5);
                            int.TryParse(inn[15].ToString(), out tmp6);
                            int.TryParse(inn[17].ToString(), out tmp7);
                            int.TryParse(inn[19].ToString(), out tmp8);
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
        public void Parcedoc()
        {
            try
            {
                sw = new Stopwatch();
                sw.Start();
                string tmp = "";
                int toad = 0;
                // bool parserows = false;
                bool starttable = false;
                HtmlNodeCollection Table = ParDoc.DocumentNode.SelectNodes("//table");
                //log.Add("Count" + Table.Count);
                if (Table != null)
                {
                    HtmlNodeCollection rows = Table[0].SelectNodes(".//tr");
                    //log.Add("count" + rows.Count);
                    strTevent tmpTevent = new strTevent();
                    listofTgames tmpTgames = new listofTgames();
                    fonbetTdata = new tennisData();
                    for (int i = 0; i < rows.Count; i++)
                    {

                        HtmlNode node = rows[i];
                        HtmlAttribute atr = node.Attributes["class"];
                        tmp = "";
                        if (atr != null && atr.Value.ToString() == "sportHead")
                        {
                            HtmlNodeCollection cols = rows[i].SelectNodes(".//td");
                            if (cols != null)
                                for (int j = 0; j < cols.Count; j++)
                                {
                                    tmp += cols[j].InnerText;
                                }

                            if (tmp != "" && tmp.Contains("Теннис") && !tmp.Contains("Настольный"))
                            {

                                if (starttable)
                                {
                                    //         log.Add("TOADD*******************************************************");
                                    //          log.Add("Player1:" + tmpTevent._Player1 + " Player 2:" + tmpTevent._Player2);
                                    //         log.Add("Score:" + tmpTevent.score);
                                    //tmpTevent = new strTevent();
                                    //        for (int c = 0; c < tmpTgames.list.Count; c++)
                                    //            log.Add(tmpTgames.getListitem(c));
                                    fonbetTdata.add(tmpTevent, tmpTgames);
                                    tmpTgames = new listofTgames();
                                    //        log.Add("ENDTOADD*******************************************************");

                                    //         log.Add("Table end");
                                }
                                //   log.Add("Start new table");
                                tmpTevent._event = tmp;
                                //      log.Add(tmpTevent._event);
                                starttable = true;
                                //parserows = true;
                                i++;
                                tmp = "";
                                HtmlNodeCollection cols1 = rows[i].SelectNodes(".//td");
                                if (cols1 != null)
                                    for (int j = 0; j < cols1.Count; j++)
                                    {
                                        tmp += cols1[j].InnerText + " ";

                                    }
                                tmpTevent.score = cols1[0].InnerText;
                                string pl1 = "";
                                string pl2 = "";
                                parcePlayers(cols1[1].InnerText, ref pl1, ref pl2);
                                tmpTevent._Player1 = pl1;
                                tmpTevent._Player2 = pl2;
                                //log.Add("Player1:" + tmpTevent._Player1 + " Player 2:" + tmpTevent._Player2);
                                //   log.Add("Score:" + tmpTevent.score);
                                toad = parsescore(tmpTevent.score);
                                //      log.Add(toad.ToString());
                                //log.Add(tmp);
                            }
                            else
                            {
                                if (tmpTevent._event != null)
                                {
                                    //       log.Add("TOADD*******************************************************");
                                    //       log.Add("Player1:" + tmpTevent._Player1 + " Player 2:" + tmpTevent._Player2);
                                    //       log.Add("Score:" + tmpTevent.score);
                                    //tmpTevent = new strTevent();
                                    //      for (int c = 0; c < tmpTgames.list.Count; c++)
                                    //          log.Add(tmpTgames.getListitem(c));
                                    fonbetTdata.add(tmpTevent, tmpTgames);
                                    tmpTgames = new listofTgames();
                                    //       log.Add("ENDTOADD*******************************************************");

                                    //      log.Add("Table end");
                                }
                                //parserows = false;
                                starttable = false;
                            }

                        }
                        else
                        {
                            if (starttable)
                            {

                                HtmlNodeCollection cols1 = rows[i].SelectNodes(".//td");
                                if (cols1 != null)
                                    for (int j = 0; j < cols1.Count; j++)
                                    {
                                        tmp += cols1[j].InnerText + " ";

                                    }
                                //log.Add(tmp);
                                if (tmp != "" && IsNumber(tmp[0]) && tmp[1] == ':' && !tmp.Contains("сет"))
                                {
                                    //       log.Add("Tableintableend");
                                    //      log.Add("TOADD*******************************************************");
                                    //      log.Add("Player1:" + tmpTevent._Player1 + " Player 2:" + tmpTevent._Player2);
                                    //      log.Add("Score:" + tmpTevent.score);
                                    //tmpTevent = new strTevent();
                                    //    for (int c = 0; c < tmpTgames.list.Count; c++)
                                    //       log.Add(tmpTgames.getListitem(c));
                                    fonbetTdata.add(tmpTevent, tmpTgames);
                                    tmpTgames = new listofTgames();
                                    //      log.Add("ENDTOADD*******************************************************");

                                    //     log.Add("Start new table");
                                    tmpTevent.score = cols1[0].InnerText;
                                    string pl1 = "";
                                    string pl2 = "";
                                    parcePlayers(cols1[1].InnerText, ref pl1, ref pl2);
                                    tmpTevent._Player1 = pl1;
                                    tmpTevent._Player2 = pl2;
                                    // log.Add("Player1:" + tmpTevent._Player1 + " Player 2:" + tmpTevent._Player2);
                                    //     log.Add("Score:" + tmpTevent.score);
                                    toad = parsescore(tmpTevent.score);
                                    //log.Add(toad.ToString());
                                    //log.Add(tmp);
                                }
                                if (tmp.Contains("гейм") && !tmp.Contains("первым") && !tmp.Contains("Исход") && !tmp.Contains("тотал") && !tmp.Contains("Тотал") && Regex.Matches(tmp, "гейм").Count == 1)
                                {
                                    try
                                    {
                                        //   log.Add(tmp);
                                        string numgam = "";
                                        string kf1 = "";
                                        string kf2 = "";
                                        int tmp1;
                                        parsegames(tmp, ref numgam, ref kf1, ref kf2);
                                        //log.Add("ParsegamesTest+++ Numgam:" + numgam + "+toadd:" + toad + " kf1:" + kf1 + " kf2:" + kf2);
                                        int cn1 = i;
                                        while (!rows[cn1].InnerText.Contains("*") && cn1 > 0)
                                        {
                                            cn1--;
                                        }
                                        // log.Add(rows[cn1].InnerText);
                                        HtmlNodeCollection cols2 = rows[cn1].SelectNodes(".//span");
                                        string currentgame = "";
                                        if (cols2 != null)
                                        {
                                            currentgame = cols2[0].InnerText;
                                            // tmpTevent.score = cols2[0].InnerText;
                                            cols2 = rows[cn1 - 2].SelectNodes(".//td");

                                            if (cols2[0].InnerText.Contains("("))
                                            {
                                                // log.Add(cols2[0].InnerText);
                                            }
                                            else
                                            {
                                                while (!rows[cn1 - 1].InnerText.Contains("(") && cn1 > 0)
                                                {
                                                    cn1--;
                                                }
                                                cols2 = rows[cn1 - 1].SelectNodes(".//td");
                                                //  log.Add(cols2[0].InnerText);
                                            }
                                        }
                                        // log.Add(cols2[0].InnerText + currentgame + " " + (parsescoreall(cols2[0].InnerText) + 1) + started(currentgame));
                                        int.TryParse(numgam, out tmp1);
                                        if (cols2 != null)
                                        {
                                            int lol = parsescore(cols2[0].InnerText);
                                            tmpTevent.score = cols2[0].InnerText + " " + currentgame;
                                        }
                                        //  log.Add((lol + tmp1).ToString());
                                        numgam = (tmp1 + toad).ToString();
                                        if ((cols2 != null) && int.Parse(numgam) == parsescoreall(cols2[0].InnerText) + 1 && started(currentgame))
                                        {
                                            if (parsecurr)
                                                tmpTgames.AddGame(numgam, kf1, kf2);
                                        }
                                        else
                                        {
                                            tmpTgames.AddGame(numgam, kf1, kf2);
                                        }
                                    }
                                    catch
                                    {
                                        string numgam = "";
                                        string kf1 = "";
                                        string kf2 = "";
                                        int tmp1;
                                        parsegames(tmp, ref numgam, ref kf1, ref kf2);
                                        int.TryParse(numgam, out tmp1);
                                        numgam = (tmp1 + toad).ToString();
                                        tmpTgames.AddGame(numgam, kf1, kf2);
                                    }
                                }
                            }
                        }
                    }

                    if (fonbetTdata.list.Count > 0 && fonbetTdata.list[fonbetTdata.list.Count - 1].eventInfo._event != tmpTevent._event)
                    {
                        // log.Add("TOADD*******************************************************");
                        //  log.Add("Player1:" + tmpTevent._Player1 + " Player 2:" + tmpTevent._Player2);
                        //  log.Add("Score:" + tmpTevent.score);
                        //tmpTevent = new strTevent();
                        //    for (int c = 0; c < tmpTgames.list.Count; c++)
                        //        log.Add(tmpTgames.getListitem(c));
                        fonbetTdata.add(tmpTevent, tmpTgames);
                        tmpTgames = new listofTgames();
                        // log.Add("ENDTOADD*******************************************************");
                    }
                    else
                    {
                        if (fonbetTdata.list.Count == 0)
                        {
                            fonbetTdata.add(tmpTevent, tmpTgames);
                            tmpTgames = new listofTgames();
                        }
                        else
                        {
                            if (tmpTevent._Player1 != fonbetTdata.list[fonbetTdata.Count - 1].eventInfo._Player1)
                            {
                                fonbetTdata.add(tmpTevent, tmpTgames);
                                tmpTgames = new listofTgames();
                            }
                        }
                    }
                }
                if (fonbetTdata != null)
                {
                    log.Add(fonbetTdata.printall());
                    sw.Stop();
                    log.Add("Elapsed in ms:" + sw.ElapsedMilliseconds);
                }
            }
            catch
            {
                log.Add("Error");
            }
        }
        struct evid
        {
            //public string _event;
            //public int id;
        }
        public int numberinstring(string inn)
        {
            int count = 0;
            for (int i = 0; i < inn.Length; i++)
            {
                if (IsNumber(inn[i]))
                    count += int.Parse(inn[i].ToString());
            }
            return count;
        }
        public void Parcedoc1()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            strTevent tmpTevent = new strTevent();
            listofTgames tmpTgames = new listofTgames();
            fonbetTdata = new tennisData();
            string currentevent = "";
            string scoregame = "";
            string scoreset = "";
            string playerinn = "";
            string pardoc = geckoWebBrowser1.Document.Body.Parent.OuterHtml;
            string player1;
            string player2;
            string numgame;
            string coef1;
            string coef2;
            bool istennis = false;
            HtmlAgilityPack.HtmlDocument Doc = new HtmlAgilityPack.HtmlDocument();

            sw.Start();
            Doc.LoadHtml(pardoc);
            HtmlNodeCollection Table = Doc.DocumentNode.SelectNodes("//table[@id='lineTable']");
            if(Table!=null&&Table[0]!=null)
            { 
            HtmlNode liveTable = Table[0];
            HtmlNodeCollection events = liveTable.SelectNodes(".//tr");

            for (int i = 0; i < events.Count; i++)
            {
                try
                {
                    if (events[i].HasAttributes && events[i].Attributes["class"] != null)
                    {
                        if (events[i].Attributes["class"].Value == "trSegment")
                        {
                            if (events[i].InnerText.Contains("Тен"))
                            {
                                currentevent = events[i].InnerText;
                                istennis = true;
                            }
                            else
                                istennis = false;
                        }
                        if (istennis)
                        {
                            int numactivegame, isactivegame;
                            if (events[i].SelectSingleNode(".//td[@class='eventCellName']") != null)
                            {
                                //addtext("Счёт игры:" + events[i].SelectSingleNode(".//td[@class='eventCellName']/div[5]").InnerText + space);
                                scoregame = events[i].SelectSingleNode(".//td[@class='eventCellName']/div[2]").InnerText;
                                playerinn = events[i].SelectSingleNode(".//td[@class='eventCellName']/div[1]/text()").InnerText;
                            }
                            if (events[i].SelectSingleNode(".//td[@class='eventCellName eventCellNameSub']") != null)
                            {
                                //Console.WriteLine("Счёт гейма и сета:" + events[i].SelectSingleNode(".//td[@class='eventCellName eventCellNameSub']/div[5]").InnerText);
                                scoreset = events[i].SelectSingleNode(".//td[@class='eventCellName eventCellNameSub']/div[2]").InnerText;
                                var activescore = scoreset;
                                //Console.WriteLine(activescore);
                                if(activescore.IndexOf("(")!=-1)
                                {
                                     numactivegame = numberinstring(activescore.Remove(activescore.IndexOf("(")));
                                     isactivegame = numberinstring(activescore.Remove(0, activescore.IndexOf("(")));
                                }
                               else
                                {
                                     numactivegame = numberinstring(activescore);
                                     isactivegame = numberinstring(activescore);
                                }
                                HtmlNodeCollection bets;
                                if (events[i].NextSibling.SelectNodes(".//table[@class='grid']") != null)
                                {
                                    bets = events[i].NextSibling.SelectNodes(".//table[@class='grid']");
                                    foreach (var element in bets)
                                    {
                                        if (element.InnerText.Contains("Гейм") && !element.InnerText.Contains("поведет") && !element.InnerText.Contains("очко") && !element.InnerText.Contains("эйс")&&!element.InnerText.Contains("счет") && element.SelectSingleNode(" .//thead/tr/th[2]") != null)
                                        {
                                            player1 = element.SelectSingleNode(" .//thead/tr/th[2]").InnerText;
                                            player2 = element.SelectSingleNode(" .//thead/tr/th[3]").InnerText;
                                            numgame = element.SelectSingleNode(" .//tbody/tr[1]/td[1]").InnerText.Replace("Кто выиграет гейм", "");
                                            coef1 = element.SelectSingleNode(" .//tbody/tr[1]/td[2]").InnerText;
                                            coef2 = element.SelectSingleNode(" .//tbody/tr[1]/td[3]").InnerText;
                                            if (tmpTevent._Player1 != player1)
                                            {
                                                fonbetTdata.add(tmpTevent, tmpTgames);
                                                tmpTgames = new listofTgames();
                                                parcePlayers(playerinn, ref player1, ref player2); ;
                                                tmpTevent._Player1 = player1;
                                                tmpTevent._Player2 = player2;
                                                tmpTevent._event = currentevent;
                                                tmpTevent.score = scoregame;
                                                int totalgame = parsescore(scoregame) + int.Parse(numgame);

                                                if (numberinstring(scoreset) > 0 && parsescoreall(scoregame) == totalgame)
                                                {
                                                    if (int.Parse(numgame) == numactivegame + 1 && isactivegame > 0)
                                                    { }
                                                    else
                                                        tmpTgames.AddGame(totalgame.ToString(), coef1, coef2);
                                                }
                                                else
                                                {
                                                    if (int.Parse(numgame) == numactivegame + 1 && isactivegame > 0)
                                                    { }
                                                    else
                                                    { tmpTgames.AddGame(totalgame.ToString(), coef1, coef2); }

                                                }
                                            }


                                            //addtext("player1:" + player1 + space);
                                            //addtext("player2:" + player2 + space);
                                            //addtext("numgame:" + numgame + space);
                                            //addtext("coef1:  " + coef1 + space);
                                            //addtext("coef2:  " + coef2 + space);
                                            if (element.SelectSingleNode(" .//tbody/tr[2]/td[1]") != null)
                                            {
                                                numgame = element.SelectSingleNode(" .//tbody/tr[2]/td[1]").InnerText.Replace("Кто выиграет гейм", "");
                                                coef1 = element.SelectSingleNode(" .//tbody/tr[2]/td[2]").InnerText;
                                                coef2 = element.SelectSingleNode(" .//tbody/tr[2]/td[3]").InnerText;
                                                int totalgame = parsescore(scoregame) + int.Parse(numgame);
                                                if ((int.Parse(numgame) == numactivegame + 1 && isactivegame > 0))
                                                { }
                                                else
                                                { tmpTgames.AddGame(totalgame.ToString(), coef1, coef2); }


                                                //addtext("numgame:" + numgame + space);
                                                //addtext("coef1:  " + coef1 + space);
                                                //addtext("coef2:  " + coef2 + space);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                catch //(Exception de)
                {
                    //  Console.Write(de.ToString());
                }
            }
            }
            if (fonbetTdata.Count > 0 && fonbetTdata.list[fonbetTdata.Count - 1].eventInfo._Player1 != tmpTevent._Player1)
            {
                fonbetTdata.add(tmpTevent, tmpTgames);
            }
            sw.Stop();
            //addtext("es:" + sw.ElapsedMilliseconds);
            TData = fonbetTdata;
            log.Add(fonbetTdata.printall());

        }
    }
}
