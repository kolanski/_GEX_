using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Text;

namespace ParimatchMicroparser
{
    public class GamesArr
    {
        public string Coefficent1 { get; set; }
        public string Coefficent2 { get; set; }
        public string GameNumber { get; set; }
        public string SetNumber { get; set; }
        public GamesArr(string Setnum, string gamenum, string cf1, string cf2)
        {
            SetNumber = Setnum;
            GameNumber = gamenum;
            Coefficent1 = cf1;
            Coefficent2 = cf2;
        }
    }

    public class Datum
    {
        public string Event { get; set; }
        public string GamePoints { get; set; }
        public List<GamesArr> GamesArr { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string ScoreAll { get; set; }
        public void fill(string _event, string pl1, string pl2, string gamepoints, string scoreall)
        {
            Event = _event;
            Player1 = pl1;
            Player2 = pl2;
            ScoreAll = scoreall;
            GamePoints = gamepoints;
        }
        public void addgame(GamesArr game)
        {
            if (GamesArr == null)
                GamesArr = new List<GamesArr>();
            GamesArr.Add(game);
        }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
    }
    public class Team
    {
        private readonly string name;
        private readonly string score;

        public Team(string name, string score)
        {
            this.name = name;
            this.score = score;
        }

        public Team()
        {
        }

        public string getName()
        {
            return name;
        }

        public string getScore()
        {
            return score;
        }
    }
    public class Game
    {
        public string NumGame;
        public string NumSet;
        public string Coefficent1;
        public string Coefficent2;
        public Game(string numset, string numgame, string coefficent1, string coefficent2)
        {
            NumGame = numgame;
            NumSet = numset;
            Coefficent1 = coefficent1;
            Coefficent2 = coefficent2;
        }
        public void calculatenumset(string score)
        {

        }
    }
    public class Event
    {
        public List<Game> games;

        public Event(string eventID, string competitionType, Team team1, Team team2)
        {
            EventId = eventID;
            CompetitionType = competitionType;
            Team1 = team1;
            Team2 = team2;
            games = new List<Game>();
        }
        public void addgame(Game game)
        {
            games.Add(game);
        }

        public Event(string eventID, string competitionType, Team team1, Team team2, string scoreAll) : this(eventID, competitionType, team1, team2)
        {
            this.ScoreAll = scoreAll;
        }

        public string ScoreAll { get; set; }
        public bool IsClose { get; set; }

        public string EventId { get; }

        public string CompetitionType { get; }

        public Team Team1 { get; }

        public Team Team2 { get; }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("======Event:" + EventId + "==========");
            result.AppendLine("Compitition name: " + CompetitionType);
            result.AppendLine("Player1: " + Team1.getName() + " Player 2" + Team2.getName());
            result.AppendLine("Scoreall:" + ScoreAll);
            result.AppendLine("Score: " + Team1.getScore() + ":" + Team2.getScore());
            foreach (Game game in games)
            {
                result.AppendLine("Numset:" + game.NumSet + " Numgame:" + game.NumGame + " Coef1:" + game.Coefficent1 + " Coef2:" + game.Coefficent2);
            }
            return result.ToString();
        }

        public Dictionary<object, object> toJSON()
        {
            var _event = new Dictionary<object, object>();
            _event.Add("eventID", EventId);
            _event.Add("competitionType", CompetitionType);
            _event.Add("competitionName", Team1.getName() + " vs " + Team2.getName());
            _event.Add("currentResult", Team1.getScore() + " : " + Team2.getScore());
            return _event;
        }
    }
    public static class ext
    {
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
        public static string renderJson(List<Event> listevents)
        {
            if (listevents != null)
            {
                RootObject torender = new RootObject();
                torender.data = new List<Datum>();
                foreach (var _event in listevents)
                {
                    Datum toadd = new Datum();
                    toadd.fill(_event.CompetitionType, _event.Team1.getName(), _event.Team2.getName(), _event.Team1.getScore() + " " + _event.Team2.getScore(), _event.ScoreAll);
                    foreach (var game in _event.games)
                    {

                        string numg = string.Join("", game.NumGame.ToCharArray().Where(c => Char.IsDigit(c)));
                        if (game.Coefficent1 != "NaN" && game.Coefficent2 != "NaN")
                            toadd.addgame(new GamesArr(game.NumSet, numg, game.Coefficent1, game.Coefficent2));
                    }
                    if (toadd.Event != null && toadd.Player1 != null && toadd.GamesArr != null)
                        torender.data.Add(toadd);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string text = serializer.Serialize(torender);
                return text;
            }
            else
                return "";
        }
    }

    public class ParimatchSimple
    {
        public static string PARIMATCH_HOME = "https://www.pari-match4.com/en/";
        public List<string> matchesid = new List<string>();
        public static string USER_AGENT = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
        private readonly CookieContainer _cookie = new CookieContainer();
        private readonly Uri _cookieHostname = new Uri(PARIMATCH_HOME);
        public RichTextBox rich;
        private Extreme.Net.HttpRequest request=null;
        public List<Event> listevents;
        // Create the clearance handler.
        //private ClearanceHandler handler = new ClearanceHandler();

        private AngleSharp.Parser.Html.HtmlParser parser = new AngleSharp.Parser.Html.HtmlParser();

        private void addtexttorich(string text)
        {
            rich.Invoke((MethodInvoker)delegate
            {
                rich.AppendText(text);
            });
        }
        private void getlistofmatches(AngleSharp.Dom.Html.IHtmlDocument document)
        {
            try
            {
                matchesid = new List<string>();

                var inpl = document.GetElementById("inplay");
                var counttennis = inpl.GetElementsByClassName("sport tennis");
                foreach (var ten in counttennis)
                {
                    var listofmatches = ten.GetElementsByClassName("dt");
                    foreach (var el in listofmatches)
                    {
                        matchesid.Add(el.GetAttribute("evno").ToString());
                    }
                }
            }
            catch
            {

            }
        }
        private void getgames(AngleSharp.Dom.Html.IHtmlDocument document)
        {
            listevents = new List<Event>();
            string Player1 = "";
            string Player2 = "";
            var tables = document.GetElementsByClassName("props");
            for (var i = 0; i < tables.Length; i++)
            {
                if (tables[i].PreviousElementSibling.GetElementsByClassName("l")[0] != null)
                {
                    Player1 = tables[i].PreviousElementSibling.GetElementsByClassName("l")[0].ChildNodes[0].TextContent;
                    if (Player1 != null)
                    {
                        Player2 = "";
                        var players = tables[i].PreviousElementSibling.GetElementsByClassName("l")[0].ChildNodes;
                        if (players[1].TextContent.Length > 3) Player2 = players[1].TextContent;
                        if (players[2].TextContent.Length > 3) Player2 = players[2].TextContent;
                        if (players[3].TextContent.Length > 3) Player2 = players[3].TextContent;
                    }
                    var ScoreAll = "";
                    var ScoreGame = "0 0";
                    try
                    {
                        var playersdata = tables[i].PreviousElementSibling.GetElementsByClassName("l")[1].TextContent;
                        if (playersdata.IndexOf(':') != -1)
                        {
                            ScoreAll = playersdata.Substring(playersdata.IndexOf('(') + 1).Replace("-", " ").Replace(",", " ").Replace(")", "");
                            var toremove1 = playersdata.Substring(playersdata.IndexOf("(") + 1).Replace("-", " ").Replace(",", " ");
                            var toremove2 = toremove1.Substring(toremove1.IndexOf(')'));
                            ScoreGame = toremove1.Substring(toremove1.IndexOf(')') + 2).Replace(":", " ");
                            ScoreAll = toremove1.Substring(0, toremove1.IndexOf(')'));
                            if (ScoreAll.IndexOf(")") != -1) ScoreAll = ScoreAll.Substring(0, ScoreAll.IndexOf(")"));
                        }
                    }
                    catch
                    {
                        ScoreAll = "0 0";
                    }
                    //push to structarr
                    Event ev = new Event(matchesid[i], "", new Team(Player1,ScoreGame.Split(' ')[0]), new Team(Player2, ScoreGame.Split(' ')[1]), ScoreAll);
                    var GamesList = tables[i].GetElementsByClassName("dyn").Length;
                    for (var j = 0; j < GamesList; j++)
                    {
                        var markets = tables[i].GetElementsByClassName("dyn")[j].TextContent;
                        if (markets.Contains("Set ")&&!markets.Contains("Deuce") && markets.Contains("game ") && !markets.Contains("point") && !markets.Contains("score")&& !markets.Contains("Who will "))
                        {
                            var SetGameText = tables[i].GetElementsByClassName("dyn")[j].ChildNodes[1].TextContent;//.match(/\d{2}|\d/ig);
                            Regex reg = new Regex(@"\d{2}|\d",RegexOptions.IgnoreCase);
                            var Set = reg.Matches(SetGameText)[0].Value;
                            var Game = reg.Matches(SetGameText)[1].Value;
                            if (tables[i].GetElementsByClassName("dyn")[j].ChildNodes[3].ChildNodes[1] != null)
                            {
                                var coef1 = tables[i].GetElementsByClassName("dyn")[j].ChildNodes[3].ChildNodes[1].TextContent;
                                var coef2 = tables[i].GetElementsByClassName("dyn")[j].ChildNodes[5].ChildNodes[1].TextContent;
                                if (coef2 == Player2 || coef2.Length > 5) coef2 = tables[i].GetElementsByClassName("dyn")[j].ChildNodes[5].ChildNodes[3].TextContent;
                                if (coef1 == Player1)
                                {
                                    coef1 = tables[i].GetElementsByClassName("dyn")[j].ChildNodes[3].ChildNodes[3].TextContent;
                                }
                                if (coef1 != "undefined" && coef2 != "undefined")
                                {
                                    //pushgames
                                    ev.addgame(new ParimatchMicroparser.Game(Set, Game, coef1, coef2));
                                    //structarr[structarr.length - 1].GamesArr.push({ 'SetNumber': Set, 'GameNumber': Game, 'Coefficent1': coef1, 'Coefficent2': coef2 });
                                }
                            }
                        }
                    }
                    if(ev.Team1.getName()!=null&&ev.games.Count>0)
                    {
                        listevents.Add(ev);
                    }
                }
            }
        }
        public async void loadmatches()
        {
            if (request == null)
            {
                request = await CloudFlareNet.CloudFlareNet.GetCloudflareClientAsync(PARIMATCH_HOME, USER_AGENT);
                request.Get(PARIMATCH_HOME);
            }
            else
            {
                var data = await request.GetAsync(PARIMATCH_HOME + "live_as.html?curs=0&curName=$");
                var document=parser.Parse(data.ToString());
                getlistofmatches(document);
                addtexttorich(string.Join("\n", matchesid));
            }
        }
        public async void loadgames()
        {
            if(matchesid!=null&&matchesid.Count>0&&request!=null)
            {
                //https://www.parimatch.com/en/live_ar.html?ARDisabled=on&hl=17139810%2C17139698%2C17142931%2C17139717%2C17139814%2C17139735%2C17139740%2C17139788%2C17139793&he=17139698,17139717,17139735,17139740,17139788,17139793,17139810,17139814,17142931,&curs=0&curName=$
                var data = await request.GetAsync(PARIMATCH_HOME + "live_ar.html?ARDisabled=on&hl="+string.Join("%2C", matchesid)+ "&he="+string.Join(",",matchesid)+ ",&curs=0&curName=$");
                var document = parser.Parse(data.ToString());
                getgames(document);
            }
            addtexttorich(ext.renderJson(listevents)+"\n");
        }
    }
}
