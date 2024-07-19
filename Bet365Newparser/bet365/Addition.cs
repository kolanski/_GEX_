using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bet365Newparser.bet365
{
    public static class Connection
    {
        public static string PostRequest(string url, WebHeaderCollection headers)
        {
            var responseFromServer = "";
            ServicePointManager.Expect100Continue = false;
            WebProxy myProxy = new WebProxy();
            myProxy.IsBypassed(new Uri(url));
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = myProxy;
            request.Method = "POST";
            request.ContentType = "text/plain; charset=UTF-8";
            request.Referer = bet365data.BET365_HOME + "/";
            request.Headers.Add("Origin", bet365data.BET365_HOME);
            request.UserAgent = bet365data.USER_AGENT;
            request.Accept = "*/*";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.KeepAlive = true;
            request.ContentLength = 0;
            request.Headers.Add(headers);
            request.Timeout = 1500;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    Debug.Assert(dataStream != null, "dataStream != null");
                    using (BufferedStream buffer = new BufferedStream(dataStream))
                    {
                        using (StreamReader readerStream = new StreamReader(buffer))
                        {
                            responseFromServer = readerStream.ReadToEnd();
                            readerStream.Close();
                        }
                        buffer.Close();
                    }
                    response.Close();
                    dataStream.Close();

                }
            }
            return responseFromServer;
        }
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
        public Game(string numset,string numgame , string coefficent1, string coefficent2)
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
            foreach(Game game in games)
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
}
