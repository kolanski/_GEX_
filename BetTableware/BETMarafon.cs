using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BTware_TestParsings;
namespace BTware_TestParsings
{
    public class BETMarafon : BETPatrn
    {
        public new logS log;
        new HtmlDocument ParDoc;
        bool parsecurr = false;
        string Doctext;
        List<bool> active = new List<bool>();
        public tennisData mfndata;
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
            log.Add("Created data from file:" + toload);
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
        public bool IsNumber(char text)                                            //Проверка на то что это число
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text.ToString());
        }
        public string cut(string to)
        {
            string ret = "";
            bool ok = true;
            int i = 0;
            while (ok)
            {
                ret = ret + to[i];
                if (i < to.Length - 1)
                    i++;
                else
                    ok = false;

                if (to[i].Equals("/"))
                {
                    ok = false;

                }
                if (i > 4 && to[i - 3].Equals(','))
                    ok = false;



            }
            return ret;
        }
        private int totalparse(string inn)
        {
            //Score:1:1 (6-7  6-4  0-0)
            //      0123456789
            //0:0  (0:3)  (40:30)
            int sets;
            //inn = " " + inn;
            inn = inn.Replace(" ", "");
            int set1;
            int set2;
            int tmp1, tmp2, tmp3, tmp4, tmp5, tmp6, tmp7, tmp8, tmp9, tmp10;
            int.TryParse(inn[0].ToString(), out set1);
            int.TryParse(inn[2].ToString(), out set2);
            sets = set1 + set2 + 1;
            if (!inn.Contains("*") && inn.Length > 12)
                switch (sets)
                {
                    case 0:
                        {
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            return tmp1 + tmp2;
                        };
                    case 1:
                        {
                            int.TryParse(inn[4].ToString(), out tmp1);
                            int.TryParse(inn[6].ToString(), out tmp2);
                            return tmp1 + tmp2;
                        };
                    case 2:
                        {
                            int.TryParse(inn[4].ToString(), out tmp1);
                            int.TryParse(inn[6].ToString(), out tmp2);
                            int.TryParse(inn[8].ToString(), out tmp3);
                            int.TryParse(inn[10].ToString(), out tmp4);
                            return tmp1 + tmp2 + tmp3 + tmp4;
                        }
                    case 3:
                        {
                            int.TryParse(inn[4].ToString(), out tmp1);
                            int.TryParse(inn[6].ToString(), out tmp2);
                            int.TryParse(inn[8].ToString(), out tmp3);
                            int.TryParse(inn[10].ToString(), out tmp4);
                            int.TryParse(inn[12].ToString(), out tmp5);
                            int.TryParse(inn[14].ToString(), out tmp6);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6;
                        }
                    case 4:
                        {
                            if (inn.Length > 11)
                            {
                                int.TryParse(inn[4].ToString(), out tmp1);
                                int.TryParse(inn[6].ToString(), out tmp2);
                                int.TryParse(inn[8].ToString(), out tmp3);
                                int.TryParse(inn[10].ToString(), out tmp4);
                                int.TryParse(inn[12].ToString(), out tmp5);
                                int.TryParse(inn[14].ToString(), out tmp6);
                                int.TryParse(inn[16].ToString(), out tmp7);
                                int.TryParse(inn[18].ToString(), out tmp8);
                                return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6 + tmp7 + tmp8;
                            }
                            else
                                return 0;
                        }
                    case 5:
                        {
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[10].ToString(), out tmp3);
                            int.TryParse(inn[12].ToString(), out tmp4);
                            int.TryParse(inn[15].ToString(), out tmp5);
                            int.TryParse(inn[17].ToString(), out tmp6);
                            int.TryParse(inn[20].ToString(), out tmp7);
                            int.TryParse(inn[22].ToString(), out tmp8);
                            int.TryParse(inn[25].ToString(), out tmp9);
                            int.TryParse(inn[27].ToString(), out tmp10);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6 + tmp7 + tmp8 + tmp9 + tmp10;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            else
                if (inn.Length <= 12)
                {
                    int.TryParse(inn[0].ToString(), out tmp1);
                    int.TryParse(inn[2].ToString(), out tmp2);
                    return tmp1 + tmp2;
                }
                else
                    return 0;

        }
        public void setparsecur(bool set)
        {
            parsecurr = set;
        }
        private int parsescore(string inn)
        {
            //Score:1:1 (6-7  6-4  0-0)
            //      0123456789
            //
            //inn = inn.Replace(" ", "");
            //inn = " " + inn;
            //1:0 (40:0) 
            //2:0 (40:15)

            inn = inn.Replace(" ", "");
            int sets;
            int set1;
            int set2;
            int tmp1, tmp2, tmp3, tmp4, tmp5, tmp6, tmp7, tmp8, tmp9, tmp10;
            int.TryParse(inn[0].ToString(), out set1);
            int.TryParse(inn[2].ToString(), out set2);
            sets = set1 + set2;
            if (!inn.Contains("*") && inn.Length > 12)
                switch (sets)
                {
                    case 0:
                        {
                            return 0;
                        };
                    case 1:
                        {
                            int.TryParse(inn[4].ToString(), out tmp1);
                            int.TryParse(inn[6].ToString(), out tmp2);
                            return tmp1 + tmp2;
                        };
                    case 2:
                        {
                            int.TryParse(inn[4].ToString(), out tmp1);
                            int.TryParse(inn[6].ToString(), out tmp2);
                            int.TryParse(inn[8].ToString(), out tmp3);
                            int.TryParse(inn[10].ToString(), out tmp4);
                            return tmp1 + tmp2 + tmp3 + tmp4;
                        }
                    case 3:
                        {
                            int.TryParse(inn[4].ToString(), out tmp1);
                            int.TryParse(inn[6].ToString(), out tmp2);
                            int.TryParse(inn[8].ToString(), out tmp3);
                            int.TryParse(inn[10].ToString(), out tmp4);
                            int.TryParse(inn[12].ToString(), out tmp5);
                            int.TryParse(inn[14].ToString(), out tmp6);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6;
                        }
                    case 4:
                        {
                            if (inn.Length > 11)
                            {
                                int.TryParse(inn[4].ToString(), out tmp1);
                                int.TryParse(inn[6].ToString(), out tmp2);
                                int.TryParse(inn[8].ToString(), out tmp3);
                                int.TryParse(inn[10].ToString(), out tmp4);
                                int.TryParse(inn[12].ToString(), out tmp5);
                                int.TryParse(inn[14].ToString(), out tmp6);
                                int.TryParse(inn[16].ToString(), out tmp7);
                                int.TryParse(inn[18].ToString(), out tmp8);
                                return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6 + tmp7 + tmp8;
                            }
                            else
                                return 0;
                        }
                    case 5:
                        {
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[10].ToString(), out tmp3);
                            int.TryParse(inn[12].ToString(), out tmp4);
                            int.TryParse(inn[15].ToString(), out tmp5);
                            int.TryParse(inn[17].ToString(), out tmp6);
                            int.TryParse(inn[20].ToString(), out tmp7);
                            int.TryParse(inn[22].ToString(), out tmp8);
                            int.TryParse(inn[25].ToString(), out tmp9);
                            int.TryParse(inn[27].ToString(), out tmp10);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6 + tmp7 + tmp8 + tmp9 + tmp10;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            else
                if (inn.Length <= 12)
                {
                    int.TryParse(inn[0].ToString(), out tmp1);
                    int.TryParse(inn[2].ToString(), out tmp2);
                    return 0;
                }
                else
                    return 0;
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
        private string delnbs(string text)                                  //Удаление из строки слова &nbsp;
        {
            string tmp = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i <= text.Length - 6)
                {
                    if (text[i] == '&' && text[i + 1] == 'n' && text[i + 2] == 'b' && text[i + 3] == 's' && text[i + 4] == 'p' && text[i + 5] == ';')
                    {
                        tmp += " ";
                        i += 5;
                    }
                }
                if (text[i] != ';')
                    tmp += text[i];
            }
            return tmp;
        }
        private string hup(string inn)
        {
            bool kop = true;
            int i = 0;
            string hol = inn;
            int l = 0;
            while (kop)
            {
                if (hol[i] == ' ')
                {
                    l++;
                }
                else
                    kop = false;
                i++;
            }
            hol.Substring(0, l);
            return hol;
        }
        public int TableCount;
        public int DatasCount;
        public void Pardoc()
        {
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            HtmlNodeCollection tbls = ParDoc.DocumentNode.SelectNodes(".//table[@class='foot-market']/tbody[contains(@id,'event')]");
            HtmlNodeCollection testPar = ParDoc.DocumentNode.SelectNodes("//table");
            if(testPar!=null)
            log.Add(testPar.Count.ToString());
            HtmlNodeCollection curr = new HtmlNodeCollection(null);
            HtmlNodeCollection gamtabls = new HtmlNodeCollection(null);
            mfndata = new tennisData();
            strTevent mfntev = new strTevent();
            listofTgames mfnlist = new listofTgames();
            List<string> scr = new List<string>();
            List<string> plrs = new List<string>();
            if (tbls != null)
            {
                log.Add("Tables count:" + tbls.Count.ToString());
                TableCount = tbls.Count;
                for (int j = 0; j < tbls.Count; j++)
                {
                    bool gamesfound = false;
                    if (tbls[j].SelectSingleNode(".//table[@class='td-border table-layout-fixed ']") != null)
                    {


                        char t = 'm';
                        int s = 0;

                        // log.Add(score.InnerText);

                        HtmlNodeCollection searchgams = tbls[j].SelectNodes(".//div[@class='block-market-table-wrapper']");
                        for (int k = 0; k < searchgams.Count; k++)
                        {
                            HtmlNodeCollection lolka = searchgams[k].SelectNodes(".//div[@class='market-table-name']");
                            if (lolka != null)
                                for (int l = 0; l < lolka.Count; l++)
                                {
                                    //if (lolka != null)
                                    //    log.Add(lolka[l].InnerText);
                                    if (lolka[l] != null && lolka[l].InnerText.Contains("Победа в гейме") || lolka[l].InnerText.Contains("To Win Game"))
                                    {
                                        HtmlNodeCollection lolka5 = searchgams[k].SelectNodes(".//table[@class='td-border table-layout-fixed ']");
                                        string jop = lolka5[0].SelectSingleNode(".//tr[1]/th[2]/div").InnerText.Replace("\t", "");
                                        if (jop.IndexOf('\n', 3)!=-1)
                                        jop = jop.Remove(jop.IndexOf('\n', 3), jop.Length - jop.IndexOf('\n', 4)).Replace("\n", "");

                                        if (!cut(jop).Contains("/"))
                                        {
                                            plrs.Add(cut(jop).Replace(" ", "").Replace(",", " "));
                                            log.Add("Player1:" + cut(jop).Replace(" ", "").Replace(",", " "));
                                        }
                                        else
                                        {
                                            string test = cut(jop).Substring(0, cut(jop).IndexOf("/"));
                                            string testn = "";
                                            int b = 0;
                                            if (test[b + 1] == ' ')
                                                b = 4;
                                            else
                                            {
                                                b = 0;
                                                if (test[b] == ' ')
                                                {
                                                    b += 1;
                                                }
                                            }
                                            if (test[b + 1] == ' ')
                                                b += 4;
                                            for (; b < test.Length; b++)
                                            {

                                                testn += test[b];
                                            }
                                            test = testn;
                                            int off = cut(jop).IndexOf("/");
                                            int len = cut(jop).Length;
                                            int v = off + 1;
                                            string test1 = "";
                                            while (v < len)
                                            {
                                                test1 += jop[v];
                                                v++;
                                            }

                                            log.Add("Player1:" + test + " / " + test1);

                                            plrs.Add(test + " / " + test1);
                                        }
                                        if(l>0)
                                            gamtabls.Add(lolka5[l-1]);
                                        else
                                            gamtabls.Add(lolka5[l]);
                                        gamesfound = true;
                                        //log.Add(lolka.InnerText);
                                    }
                                }
                        }

                        if (gamesfound)
                        {
                            HtmlNode score = tbls[j].SelectSingleNode("./tr[1]/td[1]/table/tbody/tr[1]/td[2]/div"); scr.Add(score.InnerText);
                            curr.Add(tbls[j].SelectSingleNode("./tr[1]/td[1]/table/tbody/tr[1]/td[2]"));
                            for (int i = score.InnerText.Length - 1; i > 0 && t != '('; i--)
                            {
                                t = score.InnerText[i];
                                if (IsNumber(t))
                                {
                                    s += int.Parse(t.ToString());
                                }

                            }
                            if (s != 0)
                            {
                                active.Add(true);
                                t = '(';
                            }
                            else
                                active.Add(false);
                        }

                    }
                }
                // log.Add(tbls.Count.ToString());
                for (int j = 0; j < curr.Count; j++)
                {
                    HtmlNodeCollection lols = curr[j].SelectNodes(".//span/div[1]");
                    HtmlNodeCollection lols2 = curr[j].SelectNodes(".//span/div[2]");
                    // HtmlNodeCollection score = curr[j].SelectNodes(".////*[@id="category895927"]/div/table/tbody[2]/tr[1]/td[1]/table/tbody/tr[1]/td[2]/div"
                    HtmlNodeCollection gams = gamtabls[j].SelectNodes(".//span");
                    log.Add("▼");
                    log.Add("Player1:" + cut(lols[0].InnerText.ToString()).Replace(",", "") + " Player2:" + cut(lols2[0].InnerText.ToString()).Replace(",", ""));
                    log.Add("Score:" + scr[j].ToString().Replace("\t", ""));
                    //    log.Add(gams[0].InnerText.Replace("Гейм", "") + " " + gams[2].InnerText.Replace("Гейм", ""));
                    mfntev._Player1 = cut(lols[0].InnerText.ToString()).Replace(",", "");
                    mfntev._Player2 = cut(lols2[0].InnerText.ToString()).Replace(",", "");
                    mfntev.score = scr[j].ToString();
                    
                    //Deleting node with ▼ and ▲
                    for(int h=0;h<gams.Count;h++)
                    {
                        if (gams[h].InnerText.Contains("▼") || gams[h].InnerText.Contains("▲"))
                        {
                            gams.Remove(gams[h]);
                        }
                    }

                    if (gams.Count > 4)
                    {
                        //string gams1 = gams[4].InnerText.Replace("\n", "");
                        //gams1 = gams1.Replace("\t", "");
                        if (gams.Count != 9)
                        {
                            if (gams.Count ==5 )
                            {
                                int gam1;
                                int lok = parsescore(scr[j].ToString().Replace(" ", ""));
                                gam1 = int.Parse(delnbs(gams[1].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий)", "")) + lok;
                                // int gam2 = int.Parse(delnbs(gams[4].InnerText).Replace("Гейм", "")) + lok;

                                //  mfnlist.AddGame(gam2.ToString(), gams[7].InnerText.Replace("\n", ""), gams[9].InnerText.Replace("\n", ""));
                                log.Add("Game:" + gam1.ToString() + " " + gams[3].InnerText.Replace("\n", "") + " " + gams[4].InnerText.Replace("\n", ""));
                                if ((gam1 == totalparse(scr[j].ToString().Replace(" ", "")) + 1) && started(scr[j].ToString()))
                                {
                                    log.Add("Started" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));
                                    if (parsecurr)
                                        mfnlist.AddGame(gam1.ToString(), gams[3].InnerText.Replace("\n", ""), gams[4].InnerText.Replace("\n", ""));
                                }
                                else
                                {
                                    log.Add("Nostarted" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));

                                    mfnlist.AddGame(gam1.ToString(), gams[3].InnerText.Replace("\n", ""), gams[4].InnerText.Replace("\n", ""));
                                }
                                //log.Add("Game:" + gam2.ToString() + " " + gams[7].InnerText.Replace("\n", "") + " " + gams[9].InnerText.Replace("\n", ""));
                                if (j < plrs.Count - 1 && plrs[j + 1] == mfntev._Player2)
                                {
                                    gams = gamtabls[j + 1].SelectNodes(".//span");
                                    //gams1 = gams[0].InnerText.Replace("\n", "");
                                    //gams1 = gams1.Replace("\t", "");

                                    lok = totalparse(scr[j].ToString().Replace(" ", ""));
                                    try
                                    {
                                        gam1 = int.Parse(delnbs(gams[0].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok + 1;
                                    }
                                    catch
                                    {
                                        log.Add("zaplatka1");
                                        gam1 = 0;
                                    }
                                    //gam2 = int.Parse(delnbs(gams[2].InnerText).Replace("Гейм", "")) + lok;
                                    log.Add("Game:" + gam1.ToString() + " " + gams[2].InnerText.Replace("\n", "") + " " + gams[3].InnerText.Replace("\n", ""));
                                    mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                    plrs.Remove(plrs[j + 1]);
                                    gamtabls.Remove(gamtabls[j + 1]);
                                }
                            }
                            if (gams.Count == 6)
                            {
                                int gam1;
                                int lok = parsescore(scr[j].ToString().Replace(" ", ""));
                                gam1 = int.Parse(delnbs(gams[1].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий)", "")) + lok;
                                // int gam2 = int.Parse(delnbs(gams[4].InnerText).Replace("Гейм", "")) + lok;

                                //  mfnlist.AddGame(gam2.ToString(), gams[7].InnerText.Replace("\n", ""), gams[9].InnerText.Replace("\n", ""));
                                log.Add("Game:" + gam1.ToString() + " " + gams[4].InnerText.Replace("\n", "") + " " + gams[5].InnerText.Replace("\n", ""));
                                if ((gam1 == totalparse(scr[j].ToString().Replace(" ", "")) + 1) && started(scr[j].ToString()))
                                {
                                    log.Add("Started" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));
                                    if (parsecurr)
                                        mfnlist.AddGame(gam1.ToString(), gams[4].InnerText.Replace("\n", ""), gams[5].InnerText.Replace("\n", ""));
                                }
                                else
                                {
                                    log.Add("Nostarted" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));

                                    mfnlist.AddGame(gam1.ToString(), gams[4].InnerText.Replace("\n", ""), gams[5].InnerText.Replace("\n", ""));
                                }
                                //log.Add("Game:" + gam2.ToString() + " " + gams[7].InnerText.Replace("\n", "") + " " + gams[9].InnerText.Replace("\n", ""));
                                if (j < plrs.Count - 1 && plrs[j + 1] == mfntev._Player2)
                                {
                                    gams = gamtabls[j + 1].SelectNodes(".//span");
                                    //gams1 = gams[0].InnerText.Replace("\n", "");
                                    //gams1 = gams1.Replace("\t", "");

                                    lok = totalparse(scr[j].ToString().Replace(" ", ""));
                                    try
                                    {
                                        gam1 = int.Parse(delnbs(gams[0].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok + 1;
                                    }
                                    catch
                                    {
                                        log.Add("zaplatka1");
                                        gam1 = 0;
                                    }
                                    //gam2 = int.Parse(delnbs(gams[2].InnerText).Replace("Гейм", "")) + lok;
                                    log.Add("Game:" + gam1.ToString() + " " + gams[2].InnerText.Replace("\n", "") + " " + gams[3].InnerText.Replace("\n", ""));
                                    mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                    plrs.Remove(plrs[j + 1]);
                                    gamtabls.Remove(gamtabls[j + 1]);
                                }
                            }
                            if (gams.Count == 7)
                            {
                                int gam1;
                                int lok = parsescore(scr[j].ToString().Replace(" ", ""));
                                Regex rx = new Regex(@"/\d{2}|\d/");
                                //gam1 = int.Parse(rx.Match(gams[1].InnerText).Value);
                               // gam1 = int.Parse(rx.Match(gams[1].InnerText).Value);
                                
                                gam1 = int.Parse(delnbs(gams[1].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий)", "")) + lok;
                                // int gam2 = int.Parse(delnbs(gams[4].InnerText).Replace("Гейм", "")) + lok;

                                //  mfnlist.AddGame(gam2.ToString(), gams[7].InnerText.Replace("\n", ""), gams[9].InnerText.Replace("\n", ""));
                                log.Add("Game:" + gam1.ToString() + " " + gams[5].InnerText.Replace("\n", "") + " " + gams[6].InnerText.Replace("\n", ""));
                                if ((gam1 == totalparse(scr[j].ToString().Replace(" ", "")) + 1) && started(scr[j].ToString()))
                                {
                                    log.Add("Started" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));
                                    if (parsecurr)
                                        mfnlist.AddGame(gam1.ToString(), gams[5].InnerText.Replace("\n", ""), gams[6].InnerText.Replace("\n", ""));
                                }
                                else
                                {
                                    log.Add("Nostarted" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));

                                    mfnlist.AddGame(gam1.ToString(), gams[5].InnerText.Replace("\n", ""), gams[6].InnerText.Replace("\n", ""));
                                }
                                //log.Add("Game:" + gam2.ToString() + " " + gams[7].InnerText.Replace("\n", "") + " " + gams[9].InnerText.Replace("\n", ""));
                                if (j < plrs.Count - 1 && plrs[j + 1] == mfntev._Player2)
                                {
                                    gams = gamtabls[j + 1].SelectNodes(".//span");
                                    //gams1 = gams[0].InnerText.Replace("\n", "");
                                    //gams1 = gams1.Replace("\t", "");

                                    lok = totalparse(scr[j].ToString().Replace(" ", ""));
                                    try
                                    {
                                        gam1 = int.Parse(delnbs(gams[0].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok + 1;
                                    }
                                    catch
                                    {
                                        log.Add("zaplatka1");
                                        gam1 = 0;
                                    }
                                    //gam2 = int.Parse(delnbs(gams[2].InnerText).Replace("Гейм", "")) + lok;
                                    log.Add("Game:" + gam1.ToString() + " " + gams[2].InnerText.Replace("\n", "") + " " + gams[3].InnerText.Replace("\n", ""));
                                    mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                    plrs.Remove(plrs[j + 1]);
                                    gamtabls.Remove(gamtabls[j + 1]);
                                }
                            }
                            else
                            {
                                if (gams.Count == 11)
                                {
                                    int gam1;
                                    int gam2;
                                    int lok = parsescore(scr[j].ToString().Replace(" ", ""));
                                    int.TryParse(delnbs(gams[1].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", ""), out gam1);
                                    gam1 += lok;
                                    
                                    if (gams[7].InnerText.Contains("Game"))
                                        gam2 = int.Parse(delnbs(gams[7].InnerText).Replace("Game", "")) + lok;
                                    else
                                        gam2 = int.Parse(delnbs(gams[7].InnerText).Replace("Гейм", "")) + lok;
                                        mfnlist.AddGame(gam2.ToString(), gams[9].InnerText.Replace("\n", ""), gams[10].InnerText.Replace("\n", ""));
                                        log.Add("Game:" + gam1.ToString() + " " + gams[5].InnerText.Replace("\n", "") + " " + gams[6].InnerText.Replace("\n", ""));
                                        if ((gam1 == totalparse(scr[j].ToString().Replace(" ", "")) + 1) && started(scr[j].ToString()))
                                        {
                                            log.Add("Started" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));
                                            if (parsecurr)
                                                mfnlist.AddGame(gam1.ToString(), gams[5].InnerText.Replace("\n", ""), gams[6].InnerText.Replace("\n", ""));
                                        }
                                        else
                                        {
                                            log.Add("Nostarted" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));

                                            mfnlist.AddGame(gam1.ToString(), gams[5].InnerText.Replace("\n", ""), gams[6].InnerText.Replace("\n", ""));
                                        }
                                        log.Add("Game:" + gam2.ToString() + " " + gams[9].InnerText.Replace("\n", "") + " " + gams[10].InnerText.Replace("\n", ""));
                                        if (j < plrs.Count - 1 && plrs[j + 1] == mfntev._Player2)
                                        {
                                            gams = gamtabls[j + 1].SelectNodes(".//span");
                                            //gams1 = gams[0].InnerText.Replace("\n", "");
                                            //gams1 = gams1.Replace("\t", "");

                                            lok = totalparse(scr[j].ToString().Replace(" ", ""));
                                            gam1 = int.Parse(delnbs(gams[0].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok + 1;
                                            //gam2 = int.Parse(delnbs(gams[2].InnerText).Replace("Гейм", "")) + lok;
                                            log.Add("Game:" + gam1.ToString() + " " + gams[2].InnerText.Replace("\n", "") + " " + gams[3].InnerText.Replace("\n", ""));
                                            mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                            plrs.Remove(plrs[j + 1]);
                                            gamtabls.Remove(gamtabls[j + 1]);
                                        }
                                    }
                                
                                else
                                {
                                    if (gams.Count != 11 && gams.Count != 5 && gams.Count != 6 && gams.Count != 7)
                                    {
                                        int gam1;
                                        int lok = parsescore(scr[j].ToString().Replace(" ", ""));
                                        int.TryParse(delnbs(gams[1].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", ""), out gam1);
                                        gam1 += lok;
                                        int gam2 = int.Parse(delnbs(gams[5].InnerText).Replace("Гейм", "")) + lok;

                                        mfnlist.AddGame(gam2.ToString(), gams[6].InnerText.Replace("\n", ""), gams[7].InnerText.Replace("\n", ""));
                                        log.Add("Game:" + gam1.ToString() + " " + gams[3].InnerText.Replace("\n", "") + " " + gams[4].InnerText.Replace("\n", ""));
                                        if ((gam1 == totalparse(scr[j].ToString().Replace(" ", "")) + 1) && started(scr[j].ToString()))
                                        {
                                            log.Add("Started" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));
                                            if (parsecurr)
                                                mfnlist.AddGame(gam1.ToString(), gams[3].InnerText.Replace("\n", ""), gams[4].InnerText.Replace("\n", ""));
                                        }
                                        else
                                        {
                                            log.Add("Nostarted" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));

                                            mfnlist.AddGame(gam1.ToString(), gams[3].InnerText.Replace("\n", ""), gams[4].InnerText.Replace("\n", ""));
                                        }
                                        log.Add("Game:" + gam2.ToString() + " " + gams[6].InnerText.Replace("\n", "") + " " + gams[7].InnerText.Replace("\n", ""));
                                        if (j < plrs.Count - 1 && plrs[j + 1] == mfntev._Player2)
                                        {
                                            gams = gamtabls[j + 1].SelectNodes(".//span");
                                            //gams1 = gams[0].InnerText.Replace("\n", "");
                                            //gams1 = gams1.Replace("\t", "");

                                            lok = totalparse(scr[j].ToString().Replace(" ", ""));
                                            gam1 = int.Parse(delnbs(gams[0].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok + 1;
                                            //gam2 = int.Parse(delnbs(gams[2].InnerText).Replace("Гейм", "")) + lok;
                                            log.Add("Game:" + gam1.ToString() + " " + gams[2].InnerText.Replace("\n", "") + " " + gams[3].InnerText.Replace("\n", ""));
                                            mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                            plrs.Remove(plrs[j + 1]);
                                            gamtabls.Remove(gamtabls[j + 1]);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                int gam1;
                                int lok = parsescore(scr[j].ToString().Replace(" ", ""));
                                gam1 = int.Parse(delnbs(gams[1].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok;
                                int gam2 = int.Parse(delnbs(gams[5].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok;

                                mfnlist.AddGame(gam2.ToString(), gams[6].InnerText.Replace("\n", ""), gams[8].InnerText.Replace("\n", ""));
                                log.Add("Game:" + gam1.ToString() + " " + gams[5].InnerText.Replace("\n", "") + " " + gams[7].InnerText.Replace("\n", ""));
                                if ((gam1 == totalparse(scr[j].ToString().Replace(" ", "")) + 1) && started(scr[j].ToString()))
                                {
                                    log.Add("Started" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));
                                    if (parsecurr)
                                        mfnlist.AddGame(gam1.ToString(), gams[5].InnerText.Replace("\n", ""), gams[7].InnerText.Replace("\n", ""));
                                }
                                else
                                {
                                    log.Add("Nostarted" + (totalparse(scr[j].ToString().Replace(" ", "")) + 1));

                                    mfnlist.AddGame(gam1.ToString(), gams[5].InnerText.Replace("\n", ""), gams[7].InnerText.Replace("\n", ""));
                                }
                                log.Add("Game:" + gam2.ToString() + " " + gams[6].InnerText.Replace("\n", "") + " " + gams[8].InnerText.Replace("\n", ""));
                                if (j < plrs.Count - 1 && plrs[j + 1] == mfntev._Player2)
                                {
                                    gams = gamtabls[j + 1].SelectNodes(".//span");
                                    //gams1 = gams[0].InnerText.Replace("\n", "");
                                    //gams1 = gams1.Replace("\t", "");

                                    lok = totalparse(scr[j].ToString().Replace(" ", ""));
                                    gam1 = int.Parse(delnbs(gams[1].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok + 1;
                                    //gam2 = int.Parse(delnbs(gams[2].InnerText).Replace("Гейм", "")) + lok;
                                    log.Add("Game:" + gam1.ToString() + " " + gams[5].InnerText.Replace("\n", "") + " " + gams[7].InnerText.Replace("\n", ""));
                                    mfnlist.AddGame(gam1.ToString(), gams[5].InnerText.Replace("\n", ""), gams[7].InnerText.Replace("\n", ""));
                                    plrs.Remove(plrs[j + 1]);
                                    gamtabls.Remove(gamtabls[j + 1]);
                                }
                            }
                            catch
                            {

                            }
                        }
                        //log.Add("Games total:"+gams1);
                    }
                    else
                    {
                        try
                        {
                            if (gams.Count == 4)
                            {
                                // string gams1 = gams[1].InnerText.Replace("\n", "");
                                //gams1 = gams1.Replace("\t", "");

                                int lok = parsescore(scr[j].ToString().Replace(" ", ""));
                                int gam1 = int.Parse(delnbs(gams[0].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok;
                                // int gam2 = int.Parse(gams[2].InnerText.Replace("Гейм", "")) + lok;


                                //   mfnlist.AddGame(gam2.ToString(), gams[5].InnerText.Replace("\n", ""), gams[7].InnerText.Replace("\n", ""));
                                log.Add("Game:" + gam1.ToString() + " " + gams[2].InnerText.Replace("\n", "") + " " + gams[3].InnerText.Replace("\n", ""));
                                if ((gam1 == totalparse(scr[j].ToString()) + 1) && started(scr[j].ToString()))
                                {
                                    log.Add("Started" + (totalparse(scr[j].ToString()) + 1));
                                    if (parsecurr)
                                        mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                }
                                else
                                {
                                    log.Add("Nostarted" + (totalparse(scr[j].ToString()) + 1));
                                    mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                }
                                //  log.Add("Game:" + gam2.ToString() + " " + gams[5].InnerText.Replace("\n", "") + " " + gams[7].InnerText.Replace("\n", ""));
                            }
                            if (j + 1 < plrs.Count && j < plrs.Count && plrs[j + 1] == mfntev._Player2)
                            {
                                gams = gamtabls[j + 1].SelectNodes(".//span");
                                //  string gams1 = gams[0].InnerText.Replace("\n", "");
                                // gams1 = gams1.Replace("\t", "");

                                int lok = totalparse(scr[j].ToString());
                                int gam1 = int.Parse(delnbs(gams[0].InnerText).Replace("Гейм", "").Replace("(текущий)", "").Replace(" (следующий) ", "")) + lok + 1;
                                //gam2 = int.Parse(delnbs(gams[2].InnerText).Replace("Гейм", "")) + lok;
                                log.Add("Game:" + gam1.ToString() + " " + gams[2].InnerText.Replace("\n", "") + " " + gams[3].InnerText.Replace("\n", ""));
                                mfnlist.AddGame(gam1.ToString(), gams[2].InnerText.Replace("\n", ""), gams[3].InnerText.Replace("\n", ""));
                                plrs.Remove(plrs[j + 1]);
                                gamtabls.Remove(gamtabls[j + 1]);
                            }
                        }
                        catch
                        {
                            log.Add("We have trouble with solo game;");
                        }
                    }
                    mfndata.add(mfntev, mfnlist);
                    mfnlist = new listofTgames();
                    //    log.Add(active[j].ToString());
                }
                // log.Add("lolka");
            }
            else
                log.Add("tables null");
            log.Add("TimeForSearch:" + sw.ElapsedMilliseconds.ToString());
            sw.Stop();
            DatasCount = mfndata.Count;
            TData = mfndata;
        }
    }
}