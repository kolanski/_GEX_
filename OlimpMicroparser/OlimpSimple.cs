using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroparserFramework;
using System.Net.Http;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace OlimpMicroparser
{
    class OlimpSimple
    {
        public List<string> matchesid = new List<string>();
        string pagematches = "http://olimp.com/betting";
        string pagegames = "http://olimp.com/ajax_index.php?page=line&line_nums=1&action=2&mid=0&id=0";
        public RichTextBox richTextBox1;
        CookieContainer cookieContainer;

        microserver server;
        AngleSharp.Parser.Html.HtmlParser parser = new AngleSharp.Parser.Html.HtmlParser();


        public OlimpSimple(RichTextBox rich)
        {
            cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri("http://olimp.com/"), new Cookie("curr_lang", "2"));
            //cookieContainer.Add(new Uri("http://olimp.com/"), );
            richTextBox1 = rich;
            server = new microserver(rich);
            server.setActions(GetGames, GetMatches);
            server.init("30300");
        }
        public List<Event> games = new List<Event>();
        public async void GetMatches()
        {
            try
            {

                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (HttpClient client = new HttpClient(handler))
                using (HttpResponseMessage response = await client.GetAsync(pagematches))
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    string[] devided = result.Split(new string[] { "<a href=\"javascript:set_lcheck2" }, StringSplitOptions.None);
                    string tennisgames = "";
                    if (devided.Length > 0)
                        devided = devided.RemoveAt(0);
                    foreach (string games in devided)
                    {
                        if (games.Contains("Tennis")&&!games.Contains("<b>Soccer</b>"))
                        {
                            tennisgames = games;
                            break;
                        }
                    }
                    if (tennisgames != "")
                    {
                        var splited = tennisgames;
                        var matches = splited.Split(new string[] { "index.php?page=line&action=2&live[]=" }, StringSplitOptions.None);
                        matches = matches.RemoveAt(0);
                        List<string> tmpmatches = new List<string>();
                        foreach (string match in matches)
                        {
                            tmpmatches.Add(match.Substring(0, match.IndexOf("\"")));
                        }
                        if (tmpmatches.Count > 0)
                        {
                            matchesid = tmpmatches;
                        }
                    }
                    richTextBox1.Text = string.Join("\n", matchesid);
                    //Console.WriteLine(result);
                }
            }
            catch
            { }
        }

        public async void GetGames()
        {
            //http://olimp.com/ajax_index.php?page=line&line_nums=1&action=2&mid=0&id=0&live[]=26601033&live[]=26720841&live[]=26736092&live[]=26726983

            /*try
            {*/
            List<Event> tmpgames = new List<Event>();
            if (matchesid.Count > 0)
            {
                string gamesstring = "";
                foreach (var str in matchesid)
                {
                    gamesstring += "&live[]=" + str;
                }


                HttpContent hcontent = new StringContent("");
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                using (HttpClient client = new HttpClient(handler))
                {
                    using (HttpResponseMessage response = await client.PostAsync("http://olimp.com/index.php?page=line&line_nums=1&action=2&mid=0&id=0" + gamesstring, hcontent))
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();
                        var document = parser.Parse(result);
                        Event myev = null;
                        if(document.GetElementById("betline")!=null&& document.GetElementById("betline").GetElementsByClassName("smallwnd2")[0]!=null)
                        { 
                        var table = document.GetElementById("betline").GetElementsByClassName("smallwnd2")[0];
                        var games = table.GetElementsByClassName("hi");
                            for (int s = 0; s < games.Length; s++)
                            {

                                var bets = games[s].NextElementSibling;
                                var players = "";
                                var len = games[s].Children[1].Children[0].Children[0].Children[0].ChildNodes.Length;
                                for (int g = 0; g < len; g++)
                                {
                                    if (games[s].Children[1].Children[0].Children[0].Children[0].ChildNodes[g].NodeName == "SPAN")
                                    {

                                        players = games[s].Children[1].Children[0].Children[0].Children[0].ChildNodes[g].TextContent;
                                        break;
                                    }

                                }
                                players = Regex.Replace(players.Substring(4), @"\d", "");
                                string[] playerstmp = new string[3];
                                playerstmp = players.Split(new string[] { " - " }, StringSplitOptions.None);
                                var score = new string[3];
                                if (games[s].GetElementsByClassName("txtmed").Length > 0)
                                {
                                    var tmpscore = games[s].GetElementsByClassName("txtmed")[0].TextContent;
                                    score = Regex.Split(tmpscore, "[()]");
                                }
                                if (score[0] != null && playerstmp.Length > 1 && score.Length > 1)
                                {
                                    myev = new Event("", "", new Team(playerstmp[0].Trim(), Regex.Replace(score[2], "/:/", " ").Split(' ')[0]), new Team(playerstmp[1].Trim(), Regex.Replace(score[2], "/:/", " ").Split(' ')[1]), score[1].Replace(",", "").Replace(":", " "));
                                    tmpgames.Add(myev);
                                    var divs = bets.GetElementsByTagName("div");
                                    if (divs.Length > 1)
                                    {
                                        var betgame = divs[1].Children;
                                        if (betgame != null)
                                        {
                                            for (var i = 0; i < betgame.Length; i++)
                                            {
                                                if ((betgame[i].TextContent.Contains("set") && betgame[i].TextContent.Contains("game")))
                                                {
                                                    if (betgame[i].TextContent.Length == 17 || betgame[i].TextContent.Length == 18)
                                                    {
                                                        var coef1 = betgame[i + 2].TextContent; coef1 = Regex.Match(coef1, @"\d+.\d+").Value;
                                                        var coef2 = betgame[i + 3].TextContent; coef2 = Regex.Match(coef2, @"\d+.\d+").Value;
                                                        var setnum = betgame[i].TextContent; var tmpsetnum = Regex.Matches(setnum, @"\d{2}|\d");
                                                        var toadd = new Game(tmpsetnum[0].Value, tmpsetnum[1].Value, coef1, coef2);
                                                        tmpgames[tmpgames.Count - 1].addgame(toadd);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            /*}
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex);
                            }*/
                        }
                        if (tmpgames.Count > 0)
                        {
                            this.games = tmpgames;
                        }
                        server.events = this.games;
                    }
                }
            }
            richTextBox1.Text = games.Count.ToString();

        }
    }
}