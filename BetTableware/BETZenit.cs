using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using BTware_TestParsings;
using System.Text.RegularExpressions;
namespace BTware_TestParsings
{
    public class BETZenit : BETPatrn
    {
        bool parsecurr = false;
        public void setparsecurr(bool set)
        {
            parsecurr = set;
        }

        bool IsNumber(char text)                                            //Проверка на то что это число
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text.ToString());
        }
        public int _score(string inn)
        {
            int i = inn.IndexOf("(");
            int score = 0;
            while (inn[i] != ')')
            {
                if (IsNumber(inn[i]))
                    score += int.Parse(inn[i].ToString());
                i++;
            }
            return score;
        }
        public bool started(string inn)
        {
            //Score:0:0 (0:1) (15:15)
            int total = 0;
            for (int i = inn.Length - 1; i > 2; i--)
            {
                if (inn[i] != ')')
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
        public string delpl(string inn)
        {
            inn = inn.Replace("-", " ");
            int count = 0;
            try
            {
                foreach (char c in inn)
                    if (c == ' ') count++;
                if (!inn.Contains('/'.ToString()) && count > 1)
                {
                    int k = 0;
                    foreach (char c in inn)
                        if (c == ' ') count++;
                    for (int i = 0; i < inn.Length; i++)
                    {
                        if (inn[i] == ' ')
                            k++;
                        else
                            i = 666;
                    }
                    return inn.Substring(0, inn.IndexOf(" ", k) + 2);
                }
                else
                    return inn;
            }
            catch
            {
                return inn;
            }
        }

        public void ParseDoc()
        {

            TData = new tennisData();
            strTinfo tinfo = new strTinfo();
            tinfo.gamesinEvent = new listofTgames();
            string player = "";
            HtmlNodeCollection Tablenodes = ParDoc.DocumentNode.SelectNodes("//*[@class='b-sport']");
            if (Tablenodes != null)
            {
                for (int i = 0; i < Tablenodes.Count; i++)
                {
                    HtmlNodeCollection TableEvents = Tablenodes[i].SelectNodes(".//*[@class='b-league-name h-league']");
                    for (int j = 0; j < TableEvents.Count; j++)
                    {

                        if (TableEvents[j].InnerText.Contains("Теннис.") && !TableEvents[j].InnerText.Contains("Настольный"))
                        {


                            //log.Add(TableEvents[j].NextSibling.InnerText);
                            HtmlNodeCollection TableData = TableEvents[j].NextSibling.SelectNodes(".//table/tbody/tr/td/div/div");
                            if (TableData != null)
                                for (int ii = 0; ii < TableData.Count; ii++)
                                {
                                    if (TableData[ii].InnerText.Contains("выиграет:"))
                                    {
                                        if (!player.Equals(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText))
                                        {
                                            player = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText;
                                            if (tinfo.eventInfo._Player1 != null)
                                                TData.add(tinfo);
                                            tinfo = new strTinfo();
                                            tinfo.gamesinEvent = new listofTgames();
                                            //log.Add("Newplayer!" + System.Environment.NewLine);
                                        }
                                        //log.Add(TableData[ii].InnerText);
                                        HtmlNodeCollection tr = TableData[ii].SelectNodes(".//text()");
                                        if (TableData[ii].InnerText.Contains("сете"))
                                        {
                                            //log.Add(TableEvents[j].InnerText);//"Event"
                                            //В 5-м сете 14-м гейме  выиграет: Вердаско - 3,53;  Типсаревич - 1,24; 
                                            //Вердаско  - Типсаревич 2:2 (6:7, 1:6, 6:3, 7:5, 6:7) *15:40
                                            //                           123455678910
                                            //                                      11

                                            int st = tr[2].InnerText.IndexOf(":");
                                            string player1 = tr[2].InnerText.Substring(st).Replace(":", "");
                                            st = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText.IndexOf("(");
                                            int ed = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText.IndexOf(")");
                                            string score = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText.Substring(st + 1, ed - st - 1);
                                            string player2 = tr[4].InnerText;
                                            //getnumset
                                            st = TableData[ii].InnerText.IndexOf("В");
                                            ed = TableData[ii].InnerText.IndexOf("-м");
                                            string curset = TableData[ii].InnerText.Substring(st + 2, ed - st - 2);
                                            //getnumgame
                                            st = TableData[ii].InnerText.IndexOf("сете") + 4;
                                            ed = TableData[ii].InnerText.IndexOf("-м", st);
                                            string curgame = TableData[ii].InnerText.Substring(st, ed - st);
                                            int plus = 0;
                                            int startpos = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText.IndexOf("(") + 1;
                                            for (int h = 0; h < int.Parse(curset) - 1; h++)
                                            {
                                                plus += int.Parse(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText[startpos].ToString());
                                                plus += int.Parse(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText[startpos + 2].ToString());
                                                startpos += 5;
                                            }
                                            if (!player1.Equals(tinfo.eventInfo._Player1))
                                            {
                                                log.Add(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText);
                                                //log.Add(TableEvents[j].InnerText);//eventname
                                                log.Add(delpl(player1));//players
                                                log.Add(delpl(player2));
                                                log.Add(score);
                                                tinfo.eventInfo._event = TableEvents[j].InnerText;
                                                tinfo.eventInfo._Player1 = delpl(player1);
                                                tinfo.eventInfo._Player2 = delpl(player2);
                                                tinfo.eventInfo.score = score;
                                            }
                                            int totalcurgame = int.Parse(curgame) + plus;
                                            log.Add(totalcurgame.ToString());
                                            log.Add(tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                            log.Add(tr[5].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                            //int totalcurgame = int.Parse(curgame) + plus;
                                            //log.Add(totalcurgame.ToString());
                                            //log.Add(tr[3].InnerText.Replace(",", ".").Replace("-", ""));
                                            //log.Add(tr[5].InnerText.Replace(",", ".").Replace("-", ""));
                                            if (TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText.Contains("("))
                                            {
                                                //log.Add(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText);
                                                //log.Add((int.Parse(curgame) + plus).ToString());
                                                if (totalcurgame == (plus + 1) && started(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText))
                                                {
                                                    //log.Add("started:" + totalcurgame);
                                                    if (parsecurr)
                                                        tinfo.gamesinEvent.AddGame(totalcurgame.ToString(), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[5].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                                }
                                                else
                                                {
                                                    log.Add("Nostarted:" + totalcurgame);
                                                    tinfo.gamesinEvent.AddGame(totalcurgame.ToString(), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[5].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                                }
                                                // tinfo.gamesinEvent.AddGame(totalcurgame.ToString(), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[5].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                            }
                                        }
                                        else
                                        {
                                            //log.Add(TableEvents[j].InnerText);//"Event"
                                            //В 17-м гейме выиграет
                                            //Гомбос  - Херных 0:1 (4:6, 1:4) *0:30

                                            int st = tr[0].InnerText.IndexOf(":");
                                            string player1 = tr[0].InnerText.Substring(st).Replace(":", "");
                                            string player2 = tr[2].InnerText;
                                            st = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText.IndexOf("(");
                                            int ed = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText.IndexOf(")");
                                            string score = TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText.Substring(st + 1, ed - st - 1);
                                            st = TableData[ii].InnerText.IndexOf("В");
                                            ed = TableData[ii].InnerText.IndexOf("-м");
                                            string curgame = TableData[ii].InnerText.Substring(st + 2, ed - st - 2);
                                            if (!player1.Equals(tinfo.eventInfo._Player1))
                                            {
                                                log.Add(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]").InnerText);
                                                log.Add(delpl(player1));
                                                log.Add(delpl(player2));
                                                log.Add(score);
                                                tinfo.eventInfo._event = TableEvents[j].InnerText;
                                                tinfo.eventInfo._Player1 = delpl(player1);
                                                tinfo.eventInfo._Player2 = delpl(player2);
                                                tinfo.eventInfo.score = score;
                                            }
                                            log.Add(curgame);
                                            log.Add(tr[1].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                            log.Add(tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                            //tinfo.gamesinEvent.AddGame(curgame, tr[1].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                            if (TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText.Contains("("))
                                            {
                                                //log.Add(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText);
                                                if (int.Parse(curgame) == _score(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText) + 1 && started(TableData[ii].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText))
                                                {
                                                    //log.Add("started" + curgame);
                                                    if (parsecurr)
                                                        tinfo.gamesinEvent.AddGame(curgame, tr[1].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                                }
                                                else
                                                {
                                                    //log.Add("Nostarted" + curgame);
                                                    tinfo.gamesinEvent.AddGame(curgame, tr[1].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                                }
                                            }
                                            else
                                            {
                                                //log.Add(TableData[ii - 1].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText);
                                                if (int.Parse(curgame) == _score(TableData[ii - 1].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText) + 1 && started(TableData[ii - 1].ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode(".//td[2]/div").InnerText))
                                                {
                                                    //log.Add("started" + curgame);
                                                    if (parsecurr)
                                                        tinfo.gamesinEvent.AddGame(curgame, tr[1].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                                }
                                                else
                                                {
                                                    //log.Add("Nostarted" + curgame);
                                                    tinfo.gamesinEvent.AddGame(curgame, tr[1].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""), tr[3].InnerText.Replace(",", ".").Replace("-", "").Replace(";", "").Replace(" ", ""));
                                                }
                                            }
                                        }

                                    }
                                }

                        }
                    }


                }
            }
            {
                TData.add(tinfo);
                log.Add("Newplayer!" + System.Environment.NewLine);
            }


        }
    }
}

