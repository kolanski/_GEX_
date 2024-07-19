using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BTware_TestParsings
{
    public delegate void EvOnChange();              //Объявление делегата для класса logs

    /**********************************************************************************************************************************************************/
    public struct gams
    {
        public string numgam;
        public string koef1;
        public string koef2;
    }
    /**********************************************************************************************************************************************************/
    public struct _Tevents                          //Структура для событий
    {
        public string _event;         //Название события
        public string _date;          //Дата
        public string _time;          //Время  
        public string _1stplayer;     //Имя первого игрока
        public string _2ndplayer;     //Имя второго игрока 
        public gams koef1;
        public gams koef2;
        public void empty()           //Метод для задания полей по умолчанию
        {
            
            this._1stplayer = "null";
           
            this._2ndplayer = "null";
            this._date = "null";
            this._event = "null";
            this._time = "null";
        }
    };
    /**********************************************************************************************************************************************************/
    public class logS                               //Класс logs для записи лога действий
    {

        public int els;                             //Количество событий в логе
        //public event EvOnChange AddEvent;           //Событие добавления
        public List<string> list = new List<string>();     //Контейнер списка логов
        public void Add(string toadd)               //Метод для добавления записей в лог
        {
            list.Add(toadd);
           // if (AddEvent != null)
           //     AddEvent();
            els++;
        }
        public string ToPrint(int elsnum)           //Метод для вывода записей из лога
        {
            try
            {
                if (els > 0 && elsnum < els)
                {
                    string[] array = list.ToArray();

                    return array[elsnum];
                }
                else
                    return ("Empty!" + '\n');
            }
            catch
            {
                return ("Empty!" + '\n');
            }
        }

    };
    /**********************************************************************************************************************************************************/
    public class events                             //Класс events для хранения событий
    {
        List<_Tevents> list = new List<_Tevents>();
        public int els;
        public _Tevents empty;
        public void Add(_Tevents toadd)
        {
            list.Add(toadd);
            els++;
        }
        public _Tevents ToPrint(int elsnum)
        {
            if (els > 0 && elsnum < els)
            {
                _Tevents[] array = list.ToArray();

                return array[elsnum];
            }
            else
            {
                empty = new _Tevents();
                empty.empty();
                return empty;
            }
        }

    }
    /**********************************************************************************************************************************************************/
    public class HTMLParcBC:BETPatrn                         //Класс для парсинга BETCITY
    {
        new HtmlDocument ParDoc;
        public new logS log;
        public logS events;
        public tennisData TData1;
        string Doctext;
        bool parsecurr = false;
        public void setparsecurr(bool set)
        {
            parsecurr = set;
        }
        public void parsegams(string _in,ref string outgam,ref string outkoef1,ref string outkoef2)  //Извлечение данных из строки с данными о гейме
        {
            //String example :"Победа в гейме: 10-й гейм П1: 3.56; 10-й гейм П2: 1.25;
            string stpars="";
            char tmp;
            stpars=_in.Replace("Победа в гейме: ","");
            stpars.Replace("-й", "");
            tmp=stpars[0];
            int pos = 0;
            string tooutgam = "";
           // string tooutkoef1 = "";
           // string tooutkoef2 = "";
            while (tmp != 'П')
            {
                tmp = stpars[pos];
                if (IsNumber(tmp))
                    tooutgam += tmp;

                pos++;
            }
            pos += 2;
            outgam = tooutgam;

            //while (tmp != ';')
            //{
            //    tmp = stpars[pos];
            //    if (IsNumber(tmp) || tmp == '.')
            //        tooutkoef1 += tmp;

            //    pos++;
            //} 
            //outkoef1 = tooutkoef1;
            //while (tmp != 'П')
            //{
            //    tmp = stpars[pos];
            //    pos++;
            //}
            //pos += 2;
            //while (tmp != ';')
            //{
            //    tmp = stpars[pos];
            //    if (IsNumber(tmp) || tmp == '.')
            //        tooutkoef2 += tmp;

            //    pos++;
            //}
            //outkoef2 = tooutkoef2;
        }
        public string delpl(string inn)
        {
            if(!inn.Contains('/'.ToString()))
                return inn.Substring(0,inn.IndexOf(" ")+2);
            else
                return inn;
        }
        public string getgam(string test)                                   //Проверка на то что строка содержит данные о гейме
        {
            if (delnbs(test.ToString()) != delnbs(test.ToString()).Replace("Победа в гейме:", ""))
                return test;
            else return "";
        }
        /*public HTMLParc()                                                   //Конструктор по умолчанию
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            using (StreamReader reader = new StreamReader("livebets.html", System.Text.Encoding.GetEncoding(1251)))
            {
                Doctext = reader.ReadToEnd();
                reader.Close();
            }
            ParDoc.LoadHtml(Doctext);
            log.Add("Create");
        }
        public HTMLParc(string toload)                                      //Конструктор с параметром
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            events = new logS();
            using (StreamReader reader = new StreamReader(toload, System.Text.Encoding.GetEncoding(1251)))
            {
                Doctext = reader.ReadToEnd();
                reader.Close();
            }
            ParDoc.LoadHtml(Doctext);
            log.Add("Create");
        }*/
        public void Loadpage(string toload)                                      //Конструктор с параметром
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            events = new logS();
            using (StreamReader reader = new StreamReader(toload, System.Text.Encoding.GetEncoding(1251)))
            {
                Doctext = reader.ReadToEnd();
                reader.Close();
            }
            ParDoc.LoadHtml(Doctext);
            log.Add("Created data from file:" + toload);
        }
        public void Loadpage2(string toload)
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            events = new logS();
            ParDoc.LoadHtml("<html>"+toload+"</html>");
            log.Add("Create");
        }
        public void Loadpage1(string toload,Encoding wb)
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            events = new logS();
            //string newload = "";
            string str = toload;
            Encoding srcEncodingFormat = wb;
            Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
            byte[] originalByteString = srcEncodingFormat.GetBytes(str);
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
            dstEncodingFormat, originalByteString);
            string finalString = dstEncodingFormat.GetString(convertedByteString);
            Doctext = finalString;
            ParDoc.LoadHtml(Doctext);
            log.Add("Create");
        }
        Stopwatch sw;
        public string printdoc()                                            //Вывод текущего загруженного документа
        {
            return Doctext;
        }
        private string delnbs(string text)                                  //Удаление из строки слова &nbsp;
        {
            string tmp = "";
            for (int i = 0; i < text.Length; i++)
            {
                if(i<=text.Length-6)
                {
                    if (text[i] == '&' && text[i + 1] == 'n' && text[i + 2] == 'b' && text[i + 3] == 's' && text[i + 4] == 'p' && text[i + 5] == ';')
                    {
                        tmp += " ";
                        i += 5;
                    }
                }
                if(text[i]!=';')
                tmp+=text[i];
            }
            return tmp;
        }
        bool IsNumber(char text)                                            //Проверка на то что это число
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text.ToString());
        }
        private bool istime(string test)                                    //Проверка на то что это время
        {
            if (test.Length >= 5)
                if (IsNumber(test[0]) && IsNumber(test[1]) && test[2] == ':' && IsNumber(test[3]) && IsNumber(test[4]))
                    return true;
                else
                    return false;
            else
                return false;
        }

        private int CountWords(string s, string s0)                        //Считает количество вхождений слова в строку
        {
            int count = (s.Length - s.Replace(s0, "").Length) / s0.Length;
            return count;
        }                         
        private string extevent(string inn)                                 //Извлекает название события из строки
        {
            string buf;
            string ext="";
            buf = inn.ToString().Replace("Теннис. ", "");
            buf = buf.Replace("Турнир ", "");
            buf = buf.Replace("Мужчины. ", "");
            buf = buf.Replace("Женщины. ", "");
            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i] != '.')
                    ext += buf[i];
                else
                    break;
            }
            ext=ext.Replace(" ", "");
            ext = ext.Replace("\r", "").Replace("\n", "");
            ext=ext.Replace(System.Environment.NewLine, "");
            return ext;
        
        }


        public int parsescore(string inn1)
        {
            //Счёт: 1:1(6:1,4:6,2:2) 0:15
            string inn = inn1.Replace("Счёт: ","");
            inn.Remove(0, 2);
            int sets;
            int set1;
            int set2;
            int tmp1, tmp2, tmp3, tmp4, tmp5, tmp6, tmp7, tmp8, tmp9, tmp10;
            int.TryParse(inn[1].ToString(), out set1);
            int.TryParse(inn[3].ToString(), out set2);
            sets = set1 + set2+1;
            
            if (!inn.Contains("*"))
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
                            if (inn.Length > 11)
                            {
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
                            else
                                return 0;
                        }
                    case 5:
                        {
                            int.TryParse(inn[5].ToString(), out tmp1);
                            int.TryParse(inn[7].ToString(), out tmp2);
                            int.TryParse(inn[9].ToString(), out tmp3);
                            int.TryParse(inn[11].ToString(), out tmp4);
                            int.TryParse(inn[13].ToString(), out tmp5);
                            int.TryParse(inn[15].ToString(), out tmp6);
                            int.TryParse(inn[17].ToString(), out tmp7);
                            int.TryParse(inn[19].ToString(), out tmp8);
                            int.TryParse(inn[21].ToString(), out tmp9);
                            int.TryParse(inn[23].ToString(), out tmp10);
                            return tmp1 + tmp2 + tmp3 + tmp4 + tmp5 + tmp6 + tmp7 + tmp8 + tmp9 + tmp10;
                        }
                    default:
                        {
                            return 0;
                        }
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
        public void ParceDoc()                                              //Главная функция парсинга документа
        {

            sw = new Stopwatch();
            TData1 = new tennisData();
            sw.Start();
            strTevent tmpTev = new strTevent();
            listofTgames tmpGames = new listofTgames(); ;
            _Tevents stmp = new _Tevents();
            HtmlNodeCollection TableNodes = ParDoc.DocumentNode.SelectNodes("/html/body/div[3]/table");
            HtmlNodeCollection brText = ParDoc.DocumentNode.SelectNodes("/html/body/div[3]/text()");
            //log.Add("Text:" + brText.Count);
            if (TableNodes == null || brText == null)
            {
                log.Add("ParsingError");
            }
            else
            {
                log.Add("TableCount:" + TableNodes.Count);
                for (int i = 0; i < brText.Count; i++)
                {
                    if (brText[i].InnerText.Length > 10)
                    {
                        // log.Add("Text:" + i + " " + brText[i].InnerText);
                        //if(brText[i]!=null)
                        events.Add(brText[i].InnerText);
                    }
                }

                //int brtextindex = 0;
                for (int t = 0; t < TableNodes.Count; t++)
                {
                    string test;
                    int currentgamenum = 0;
                    test = events.ToPrint(t).ToString().Replace("Теннис.", " ");
                    //log.Add(events.ToPrint(t));
                    //log.Add(System.Environment.NewLine);
                    //   log.Add("New table start.");
                    if (events.ToPrint(t) != test && events.ToPrint(t) == events.ToPrint(t).ToString().Replace("Настольный ", " "))
                    {
                        // stmp._event = extevent(events.ToPrint(t));
                        // log.Add(stmp._event);
                        HtmlNodeCollection rows = TableNodes[t].SelectNodes(".//tr");
                        HtmlNodeCollection koefsn = TableNodes[t].SelectNodes(".//*[@id='dt2']");
                        for (int i = 0; i < rows.Count; i++)
                        {
                            //log.Add(delnbs(rows[i].InnerText));
                            HtmlNodeCollection cols = rows[i].SelectNodes(".//td");
                            if (istime(cols[0].InnerText))
                            {
                                //log.Add(rows[i].InnerText);


                                    stmp._1stplayer = delpl(cols[1].InnerText);
                                    stmp._2ndplayer = delpl(cols[2].InnerText);
                                    tmpTev._Player1 = stmp._1stplayer;
                                    tmpTev._Player2 = stmp._2ndplayer;
                                int p=i;
                                   while(!rows[p].InnerText.Contains("Счёт")&&i<rows.Count)
                                {
                                    p++;
                                }
                                   tmpTev.score = rows[p].InnerText;
                                   //log.Add("Score:" + rows[p].InnerText);

                                //  log.Add("Player 1: "+stmp._1stplayer+" Player 2: "+stmp._2ndplayer);
                            }
                            if (CountWords(rows[i].InnerText, "Победа в гейме:") > 0)
                            {
                                int gamscount = CountWords(rows[i].InnerText, "Победа в гейме:");
                                HtmlNodeCollection gams = TableNodes[t].SelectNodes(".//*[@id='dt2']");
                                int match = 0;
                                int ink = 0;
                                for (int j = currentgamenum; match != gamscount; j++)
                                {
                                    if (gams[j].InnerText.Contains("Победа в гейме: "))
                                    {
                                        //  log.Add(delnbs(gams[j].InnerText));
                                        HtmlNodeCollection hrefs = gams[j].SelectNodes(".//a");

                                        stmp.koef1.numgam = "";
                                        parsegams(delnbs(gams[j].InnerText), ref stmp.koef1.numgam, ref stmp.koef1.koef1, ref stmp.koef1.koef2);
                                        if ((int.Parse(stmp.koef1.numgam) == parsescore(tmpTev.score) + 1) && started(tmpTev.score))
                                        {
                                            int pr = parsescore(tmpTev.score);
                                            bool sc = started(tmpTev.score);
                                            log.Add(pr.ToString() + sc);
                                            if(parsecurr)
                                                tmpGames.AddGame(stmp.koef1.numgam, hrefs[0].InnerText, hrefs[1].InnerText);
                                        }
                                        else
                                        {

                                            int pr = parsescore(tmpTev.score);
                                            bool sc = started(tmpTev.score);
                                            log.Add(pr.ToString() + sc + int.Parse(stmp.koef1.numgam));
                                            tmpGames.AddGame(stmp.koef1.numgam, hrefs[0].InnerText, hrefs[1].InnerText);
                                        }
                                        match++;
                                    }
                                    ink++;
                                }
                                currentgamenum += ink;
                                int curr = currentgamenum;
                                while (rows[i].InnerText.Contains("Счёт:"))
                                {
                                    curr++;
                                }
                               // log.Add("Score:" + TableNodes[t].InnerText);

                            }
                            if (rows[i].InnerText.Contains("online"))
                            {
                                if (tmpGames.list != null && tmpGames.list.Count != 0)
                                    TData1.add(tmpTev, tmpGames);
                                tmpGames = new listofTgames();
                                //log.Add("Toadd");
                            }

                            /*
                            tmp = "";
                            if (CountWords(rows[i].InnerText, "Победа в гейме:") != 0)
                            {
                                log.Add(CountWords(rows[i].InnerText,"Победа в гейме:").ToString());
                                gamecountintable = CountWords(rows[i].InnerText, "Победа в гейме:");
                                totalgameintablecount += gamecountintable;
                            }

                            HtmlNodeCollection cols = rows[i].SelectNodes(".//td");

                            for (int j = 0; j < cols.Count; j++)
                            {
                                if (getgam(cols[j].InnerText.ToString()) != "")
                                    koefcount++;
                                string value = cols[j].InnerText;
                                if (delnbs(value) == "Время") counter = true;

                                if (counter && istime(delnbs(value)))
                                {
                                    stmp._time = delnbs(cols[j].InnerText);
                                    stmp._1stplayer = delnbs(cols[j + 1].InnerText);
                                    stmp._2ndplayer = delnbs(cols[j + 2].InnerText);
                                    counter = false;
                                }


                                if (delnbs(value).ToString() != delnbs(value).ToString().Replace("online", ""))
                                {
                                   // log.Add("Total gamecount:" + gamecountintable);
                                    oncount++;
                                    //  log.Add("Total rows" + i + "This row" + j);

                                    if (koefsn != null)
                                    {
                                        hop = "";

                                          log.Add(koefsn.Count.ToString());
                                        stmp.koef1.numgam = "";
                                        stmp.koef2.numgam = "";
                                        int koefarrcount = 0;
                                        for (int r = k; r <= totalgameintablecount; r++)
                                        {

                                            //if(koefcount<koefsn.Count-1)
                                            //if (r < koefsn.Count)
                                                if (getgam(koefsn[r].InnerText.ToString()) != "")
                                                {
                                                    //  stmp.koef1.koef1
                                                    if (stmp.koef1.numgam == null)
                                                        parsegams(koefsn[r].InnerText.ToString(), ref stmp.koef1.numgam, ref stmp.koef1.koef1, ref stmp.koef1.koef2);
                                                    else
                                                        if (stmp.koef1.numgam != null)
                                                        {
                                                            parsegams(koefsn[r].InnerText.ToString(), ref stmp.koef2.numgam, ref stmp.koef2.koef1, ref stmp.koef2.koef2);
                                                        }
                                               

                                                    k++;
                                                    rcount++;
                                                } 
                                          //  log.Add("Stringtoparse:" + koefsn[r].InnerText.ToString());
                                        }
                                        //log.Add("Rcount:" + koefcount);
                                    }

                                    //if (stmp.koef1.numgam != null)
                                    //{
                                    tmpTev = new strTevent();
                                    tmpTev._event = stmp._event;
                                    tmpTev._Player1 = stmp._1stplayer;
                                    tmpTev._Player2 = stmp._2ndplayer;
                                
                                    //log.Add("Event:" + stmp._event);
                                    log.Add("Time:" + stmp._time + " Player 1:" + stmp._1stplayer + " Player 2:" + stmp._2ndplayer);
                                    //log.Add("Eventgamcount:" + gamecountintable);
                                    //}
                                    tmpGames = new listofTgames();
                                    if (stmp.koef1.numgam != "")
                                    {
                                        log.Add("Numgam:" + stmp.koef1.numgam + " Player 1:" + stmp.koef1.koef1 + " Player 2:" + stmp.koef1.koef2);
                                   
                                        tmpGames.AddGame(stmp.koef1.numgam, stmp.koef1.koef1, stmp.koef1.koef2);
                                    }
                                    if (stmp.koef2.numgam != "")
                                    {
                                        log.Add("Numgam:" + stmp.koef2.numgam + " Player 1:" + stmp.koef2.koef1 + " Player 2:" + stmp.koef2.koef2);
                                        tmpGames.AddGame(stmp.koef2.numgam, stmp.koef2.koef1, stmp.koef2.koef2);
                                    }
                                    if(tmpGames.elsinlist!=0)
                                    TData.add(tmpTev, tmpGames);
                                    // log.Add("1st gam:" + stmp.koef1.numgam);
                                    // log.Add("2nd gam:" + stmp.koef2.numgam);
                                }
                                tmp += value + " ";
                                //TESTCODEBLOCK*********************************************************************************************************************
                                //log.Add("Total gamecount:" + gamecountintable);
                                //TESTCODEBLOCK*********************************************************************************************************************
                            }

                            count++;
                            //STARTTESTCODEBLOCK*********************************************************************************************************************
                            //tmp2 = delnbs(tmp);
                            //log.Add(tmp2);
                            //ENDTESTCODEBLOCK*********************************************************************************************************************
                            //log.Add
                        */
                        }
                        //TESTCODEBLOCK*********************************************************************************************************************
                        //if (koefsn != null)
                        //{
                        //    hop="";
                        //    log.Add(koefsn.Count.ToString());
                        //    stmp.koef1.numgam = null;
                        //    stmp.koef2.numgam = null;
                        //    for (int r = 0; r < koefsn.Count; r++)
                        //    {
                        //        //hop += koefsn[r].InnerText;
                        //        if (stmp.koef1.numgam != null&&koefsn[r].InnerText.ToString() != koefsn[r].InnerText.ToString().Replace("Победа в гейме:", ""))
                        //        {
                        //            stmp.koef2.numgam = koefsn[r].InnerText;
                        //        }
                        //        if (stmp.koef2.numgam == null && (koefsn[r].InnerText.ToString() != koefsn[r].InnerText.ToString().Replace("Победа в гейме:", "")))
                        //        {
                        //            stmp.koef1.numgam = koefsn[r].InnerText;
                        //        }
                        //    }
                        //}
                        //   log.Add("Time:" + stmp._time + " Player 1:" + stmp._1stplayer + " Player 2:" + stmp._2ndplayer);
                        //   log.Add("1st gam:" + stmp.koef1.numgam);
                        //  log.Add("2nd gam:" + stmp.koef2.numgam);
                        //  log.Add(hop);
                        //   log.Add("New table end.");
                        //  log.Add(System.Environment.NewLine);
                        //TESTCODEBLOCK*********************************************************************************************************************
                        //log.Add("Totalgameeventsintable:" + totalgameintablecount);
                    }
                    //else
                    //    log.Add(brText[brtextindex].InnerText + "Not tennis");

                }

                //log.Add("Total count:" + count);
                log.Add("Time processing ms:" + sw.ElapsedMilliseconds.ToString());
                sw.Stop();
                log.Add("Total in tableinlist:" + TData1.list.Count);
                log.Add(TData1.printall());
                TData = TData1;
            }
        }
    }
}