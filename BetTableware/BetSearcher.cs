using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTware_TestParsings;
using System.Globalization;
using WordsMatching;
namespace BetTableware
{
    public class BetSearcher
    {
        MatchsMaker match;
        MatchsMaker match1;
        NumberStyles style;
        CultureInfo culture;
        public logS Forklog=new logS();
        public string _1stname, _2ndname;
        public bool debug = false;
        public float minprof;
        bool tested = true;
        string banned="";
        public bool dtest = true;
        public void setban(string ban)
        {
            banned = ban;
        }
        public void setdebug(bool set)
        {
            debug = set;
        }
        public void setminprof(float set)
        {
            minprof = set;
        }
        public void setdtest(bool set)
        {
            dtest = set;
        }
        public void datas_to_compares(string _1st, string _2nd)
        {
            this._1stname = _1st;
            this._2ndname = _2nd;
        }
        public void compareTData(tennisData fonbet, tennisData betcity)
        {
            //Forklog = new logS();
           
            int idealcount = 0;
            if (_1stname == "")
            {
                _1stname = "fonbet";
                _2ndname = "betcity";
            }
            if(fonbet==null||betcity==null)
            {
                Forklog.Add("Error in parsing module");
                return;
            }
            else
            {
            for (int i = 0; i < fonbet.Count; i++)
            {

                for(int j=0;j< betcity.Count; j++)
                {

                    tested = true;

                    match = new MatchsMaker(fonbet.list[i].eventInfo._Player1, betcity.list[j].eventInfo._Player1, true);
                    match1 = new MatchsMaker(fonbet.list[i].eventInfo._Player2, betcity.list[j].eventInfo._Player2, true);
                    MatchsMaker matchn = new MatchsMaker(fonbet.list[i].eventInfo._Player1, betcity.list[j].eventInfo._Player2, true);
                    MatchsMaker matchn1 = new MatchsMaker(fonbet.list[i].eventInfo._Player2, betcity.list[j].eventInfo._Player1, true);

                    if ((match.Score + match1.Score) > 1.1)
                    {
                        if (_1stname == "fonbet")
                        {

                        }
                        if (dtest)
                            tested = false;
                        idealcount++;
                        //Forklog.Add(match.Score.ToString());
                        for (int u = 0; u < fonbet.list[i].gamesinEvent.list.Count; u++)
                        {
                            for (int h = 0; h < betcity.list[j].gamesinEvent.list.Count; h ++)
                            {

                                if (betcity.list[j].gamesinEvent.list[h].numgam == fonbet.list[i].gamesinEvent.list[u].numgam)
                                {

                                    float koef11;
                                    float koef21;
                                    float koef12;
                                    float koef22;
                                    culture = CultureInfo.CreateSpecificCulture("en-GB");
                                    style = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;
                                    float.TryParse(betcity.list[j].gamesinEvent.list[h].koef1, style, culture, out koef11);
                                    float.TryParse(fonbet.list[i].gamesinEvent.list[u].koef1, style, culture, out koef21);
                                    float.TryParse(betcity.list[j].gamesinEvent.list[h].koef2, style, culture, out koef12);
                                    float.TryParse(fonbet.list[i].gamesinEvent.list[u].koef2, style, culture, out koef22);
                                    /*if (_2ndname == " Maraphon ")
                                        Forklog.Add(koef11.ToString() + koef12.ToString() + koef21.ToString() + koef22.ToString());*/
                                    if (koef11 > koef21)
                                    {
                                        if ((1 / koef11 + 1 / koef22) < 1 - minprof)
                                        {
                                            Forklog.Add(_1stname + " Player1:" + fonbet.list[i].eventInfo._Player1 + _2ndname + "   Player2:" + betcity.list[j].eventInfo._Player2);
                                            Forklog.Add("Sum:" + (1 / koef11 + 1 / koef22).ToString());
                                            Forklog.Add("Numgam:" + betcity.list[j].gamesinEvent.list[h].numgam);
                                            Forklog.Add("Pl1:" + koef22 + " Pl2:" + koef11);
                                            if (koef11 > koef22)
                                            {
                                                if (debug)
                                                {
                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl1 +" + (1000 * koef22) / koef11 + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef22 - (1000 + (1000 * koef22) / koef11)));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl1 +" + (-1000 / (1 - koef11)) + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef22 - (1000 + (-1000 / (1 - koef11)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                            else
                                            {
                                                if (debug)
                                                {

                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl2 +" + (1000 * koef11) / koef22 + " on pl1");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef11 - (1000 + (1000 * koef11) / koef22)));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl2 +" + (-1000 / (1 - koef22)) + " on pl1");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef11 - (1000 + (-1000 / (1 - koef22)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                        }
                                        //  else
                                        //      Forklog.Add("fonbet Player1:" + fonbet.list[i].eventInfo._Player1 + "  betcity Player2:" + betcity.list[j].eventInfo._Player2 + " Sum:" + (1 / koef11 + 1 / koef22).ToString()); 
                                    }
                                    else
                                    {
                                        /*if (_2ndname == " Maraphon ")
                                            Forklog.Add(koef11.ToString() + koef12.ToString() + koef21.ToString() + koef22.ToString());*/
                                        if ((1 / koef21 + 1 / koef12) < 1 - minprof)
                                        {
                                            Forklog.Add(_1stname + " Player1:" + fonbet.list[i].eventInfo._Player1 + _2ndname + "   Player2:" + betcity.list[j].eventInfo._Player2);
                                            Forklog.Add("Sum:" + (1 / koef21 + 1 / koef12).ToString());
                                            Forklog.Add("Numgam:" + betcity.list[j].gamesinEvent.list[h].numgam);
                                            Forklog.Add("Pl1:" + koef21 + " Pl2:" + koef12);
                                            if (koef21 > koef12)
                                            {
                                                if (debug)
                                                {
                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl2 +" + (1000 * koef12) / koef21 + " on pl1");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef12 - (1000 + ((1000 * koef12) / koef21))));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl2 +" + (-1000 / (1 - koef21)) + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef12 - (1000 + (-1000 / (1 - koef21)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                            else
                                            {
                                                if (debug)
                                                {
                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl1 +" + (1000 * koef21) / koef12 + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef21 - (1000 + ((1000 * koef21) / koef12))));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl2 +" + (-1000 / (1 - koef12)) + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef21 - (1000 + (-1000 / (1 - koef12)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                        }
                                        // else
                                        //     Forklog.Add("fonbet Player1:" + fonbet.list[i].eventInfo._Player1 + "  betcity Player2:" + betcity.list[j].eventInfo._Player2 + " Sum:" + (1 / koef21 + 1 / koef12).ToString()); 
                                    }
                                }
                            }

                        }
                    }
                    /*
                     * Пример строки 
                     * Betcity
                     * Player1:Турсунов Д. Player2:Башич М.
                     * Numgam:18 Koef1:2 Koef2:1.72
                     * Fonbet
                     * Player1: Басич М Player2:  Турсунов Д
                     * Numgam:18 Koef1:1.45 Koef2:2.55
                     * Betcity.player1=fonbet.player2=>betcity.koef1=fonbet.koef2
                    */
                    //if((!fonbet.list[i].eventInfo._Player1.Contains(banned) && !betcity.list[j].eventInfo._Player1.Contains(banned) && !fonbet.list[i].eventInfo._Player2.Contains(banned) && !betcity.list[j].eventInfo._Player2.Contains(banned)))
                    //{

                    MatchsMaker old1, old2;
                    old1 = match;
                    old2 = match;
                    match = new MatchsMaker(fonbet.list[i].eventInfo._Player1, betcity.list[j].eventInfo._Player2, true);
                    match1 = new MatchsMaker(fonbet.list[i].eventInfo._Player2, betcity.list[j].eventInfo._Player1, true);
                    /*}
                    else
                    {
                        MatchsMaker old1 ,old2;
                    old1 = match;
                    old2 = match;
                    match = new MatchsMaker("aaa","bbb");
                    match1 = new MatchsMaker("aaa", "bbb");
                    }*/
                    /*Debug*/
                    //Forklog.Add(fonbet.list[i].eventInfo._Player1 + "   " + betcity.list[j].eventInfo._Player2+"   "+ match.Score.ToString());
                    //Forklog.Add(fonbet.list[i].eventInfo._Player2 + "   " + betcity.list[j].eventInfo._Player1 + "   " + match1.Score.ToString());
                    /*if (_2ndname == " Maraphon ")
                    {
                        if ((match.Score + match1.Score) > 1.1)
                        {

                            Forklog.Add(fonbet.list[i].eventInfo._Player1 + "   " + betcity.list[j].eventInfo._Player2 + "   " + match.Score.ToString());
                            Forklog.Add(fonbet.list[i].eventInfo._Player2 + "   " + betcity.list[j].eventInfo._Player1 + "   " + match1.Score.ToString());
                        }
                    }*/
                    if ((match.Score + match1.Score) > 1.1 && tested)
                    {

                        idealcount++;
                        // Forklog.Add(match.Score.ToString());
                        for (int u = 0; u < fonbet.list[i].gamesinEvent.list.Count; u++)//доделать
                        {
                            for (int h = 0; h < betcity.list[j].gamesinEvent.list.Count; h++)
                            {
                                if (betcity.list[j].gamesinEvent.list[h].numgam == fonbet.list[i].gamesinEvent.list[u].numgam)
                                {
                                    float koef11;
                                    float koef21;
                                    float koef12;
                                    float koef22;
                                    culture = CultureInfo.CreateSpecificCulture("en-GB");
                                    style = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;
                                    float.TryParse(betcity.list[j].gamesinEvent.list[h].koef1, style, culture, out koef11);
                                    float.TryParse(fonbet.list[i].gamesinEvent.list[u].koef2, style, culture, out koef21);
                                    float.TryParse(betcity.list[j].gamesinEvent.list[h].koef2, style, culture, out koef12);
                                    float.TryParse(fonbet.list[i].gamesinEvent.list[u].koef1, style, culture, out koef22);
                                    /*if (_2ndname == " Maraphon ")
                                        Forklog.Add(koef11.ToString() + koef12.ToString() + koef21.ToString() + koef22.ToString());*/
                                    if (koef11 > koef21)
                                    {
                                        if ((1 / koef11 + 1 / koef22) < 1 - minprof)
                                        {
                                            Forklog.Add(_1stname + " Player1:" + fonbet.list[i].eventInfo._Player1 + _2ndname + "   Player2:" + betcity.list[j].eventInfo._Player1);
                                            Forklog.Add("Sum:" + (1 / koef11 + 1 / koef22).ToString());
                                            Forklog.Add("Numgam:" + betcity.list[j].gamesinEvent.list[h].numgam);
                                            Forklog.Add("Pl1:" + koef22 + " Pl2:" + koef11);
                                            if (koef11 > koef22)
                                            {
                                                if (debug)
                                                {
                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl1 +" + (1000 * koef22) / koef11 + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef22 - (1000 + (1000 * koef22) / koef11)));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl1 +" + (-1000 / (1 - koef11)) + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef22 - (1000 + (-1000 / (1 - koef11)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                            else
                                            {
                                                if (debug)
                                                {
                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl2 +" + (1000 * koef11) / koef22 + " on pl1");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef11 - (1000 + (1000 * koef11) / koef22)));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl2 +" + (-1000 / (1 - koef22)) + " on pl1");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef11 - (1000 + (-1000 / (1 - koef22)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                        }
                                        //   else
                                        //      Forklog.Add("fonbet Player1:" + fonbet.list[i].eventInfo._Player1 + "  betcity Player2:" + betcity.list[j].eventInfo._Player1 + " Sum:" + (1 / koef11 + 1 / koef22).ToString());
                                    }
                                    else
                                    {
                                        if ((1 / koef21 + 1 / koef12) < 1 - minprof)
                                        {
                                            Forklog.Add(_1stname + " Player1:" + fonbet.list[i].eventInfo._Player1 + _2ndname + " Player2:" + betcity.list[j].eventInfo._Player1);
                                            Forklog.Add("Sum:" + (1 / koef21 + 1 / koef12).ToString());
                                            Forklog.Add("Numgam:" + betcity.list[j].gamesinEvent.list[h].numgam);
                                            Forklog.Add("Pl1:" + koef21 + " Pl2:" + koef12);
                                            if (koef21 > koef12)
                                            {
                                                if (debug)
                                                {
                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl2 +" + (1000 * koef12) / koef21 + " on pl1");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef12 - (1000 + ((1000 * koef12) / koef21))));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl2 +" + (-1000 / (1 - koef21)) + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef12 - (1000 + (-1000 / (1 - koef21)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                            else
                                            {
                                                if (debug)
                                                {
                                                    Forklog.Add("White bet.");
                                                    Forklog.Add("1000 on pl1 +" + (1000 * koef21) / koef12 + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef21 - (1000 + ((1000 * koef21) / koef12))));
                                                    Forklog.Add("Blue bet.");
                                                    Forklog.Add("1000 on pl2 +" + (-1000 / (1 - koef12)) + " on pl2");
                                                    Forklog.Add("Takeble profit:" + (1000 * koef21 - (1000 + (-1000 / (1 - koef12)))));
                                                    Forklog.Add(System.Environment.NewLine);
                                                }
                                            }
                                        }
                                        // else
                                        //     Forklog.Add("fonbet Player1:" + fonbet.list[i].eventInfo._Player1 + "  betcity Player2:" + betcity.list[j].eventInfo._Player1 + " Sum:" + (1 / koef21 + 1 / koef12).ToString());
                                    }
                                }
                            }
                        }
                    }

                }

            }
            //Forklog.Add("Count of matches:"+idealcount.ToString());

            }
        }
        private bool testsname(string fonbetsname, string betcitysname, string fonbetsname1)
        {
            
            return true;
        }
        private void testc() { }
    }
}