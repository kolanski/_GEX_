using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Socket = Quobject.SocketIoClientDotNet.Client.Socket;
using Quobject.SocketIoClientDotNet.Client;
using System.Windows.Forms;

namespace MicroparserFramework
{
    public class GamesArr
    {
        public string Coefficent1 { get; set; }
        public string Coefficent2 { get; set; }
        public string GameNumber { get; set; }
        public string SetNumber { get; set; }
        public GamesArr()
        {
            
        }
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
        private string score;

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
        public void setScore(string score)
        {
            this.score = score;
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
        public Event(Datum data)
        {
            EventId = "0";
            CompetitionType = data.Event;
            this.ScoreAll = data.ScoreAll;
            try
            {
                Team1 = new Team(data.Player1, data.GamePoints.Substring(0, data.GamePoints.IndexOf(":")));
                Team2 = new Team(data.Player2, data.GamePoints.Substring(data.GamePoints.IndexOf(":")+1, data.GamePoints.Length - data.GamePoints.IndexOf(":")-1));
            }
            catch
            {
                Team1 = new Team(data.Player1, "0");
                Team2 = new Team(data.Player2, "0");
            }
            games = new List<Game>();
            foreach(var game in data.GamesArr)
            {
                games.Add(new Game(game.SetNumber, game.GameNumber, game.Coefficent1, game.Coefficent2));
            }
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
        public static double FractionToDouble(string fraction)
        {
            double result;

            if (double.TryParse(fraction, out result))
            {
                return result;
            }

            string[] split = fraction.Split(new char[] { ' ', '/' });

            if (split.Length == 2 || split.Length == 3)
            {
                int a, b;

                if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
                {
                    if (split.Length == 2)
                    {
                        return (double)a / b;
                    }

                    int c;

                    if (int.TryParse(split[2], out c))
                    {
                        return a + (double)b / c;
                    }
                }
            }

            throw new FormatException("Not a valid fraction.");
        }
        public static string renderJson(List<Event> listevents)
        {
            if (listevents != null)
            {
                RootObject torender = new RootObject();
                torender.data = new List<Datum>();
                lock (listevents)
                {
                    foreach (var _event in listevents)
                    {
                        Datum toadd = new Datum();
                        if (_event != null && _event.CompetitionType != null)
                        {
                            toadd.fill(_event.CompetitionType, _event.Team1.getName(), _event.Team2.getName(), _event.Team1.getScore() + " " + _event.Team2.getScore(), _event.ScoreAll);
                            foreach (var game in _event.games)
                            {
                                if (game != null)
                                {
                                    string numg = string.Join("", game.NumGame.ToCharArray().Where(c => Char.IsDigit(c)));
                                    if (game.Coefficent1 != "NaN" && game.Coefficent2 != "NaN")
                                        toadd.addgame(new GamesArr(game.NumSet, numg, game.Coefficent1, game.Coefficent2));
                                }
                            }
                            if (toadd.Event != null && toadd.Player1 != null && toadd.GamesArr != null)
                                torender.data.Add(toadd);
                        }
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string text = serializer.Serialize(torender);
                    return text;
                }
                
            }
            else
                return "";
        }
    }

    public class microserver
    {
        Socket socket;
        public RichTextBox richStatus;
        int counter = 0;
        public Action parsematches;
        public Action loadMatches;
        public List<Event> events= new List<Event>();
        public microserver(RichTextBox rich)
        {
            richStatus = rich;
        }
        public void parseJson(string json)
        {
            if (json != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Datum> tmpjson = serializer.Deserialize<List<Datum>>(json);
                List<Event> tmpevents = new List<Event>();
                if (tmpjson != null && tmpjson.Count > 0)
                {
                    foreach (var eve in tmpjson)
                    {
                        tmpevents.Add(new Event(eve));
                    }
                }
                if (tmpevents.Count > 0)
                    events = tmpevents;
            }
        }
        public void setActions(Action parse,Action reload)
        {
            parsematches = parse;
            loadMatches = reload;
        }

        public void init(string port)
        {
            //var options = new IO.Options() { ForceNew = false };
            socket = IO.Socket("http://127.0.0.1:"+port);
            socket.Connect();

            socket.On(Socket.EVENT_DISCONNECT, (data) =>
            {
                socket.Connect();
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evdisc:");
                    //  var m = JsonConvert.DeserializeObject<List<string>>(data);

                });
            });
            socket.On(Socket.EVENT_CONNECT, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                { richStatus.AppendText("Status evconn:"); });
            });
            socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                try
                {
                    richStatus.Invoke((MethodInvoker)delegate
                    {

                        richStatus.AppendText("Status evconnerr:");
                        counter++;
                        if (counter > 20)
                        {
                            counter = 0;
                            richStatus.Invoke((MethodInvoker)delegate
                            {
                                richStatus.Text = "";// "needparse:");
                        });
                        }
                        socket.Connect();
                    });
                }
                catch
                {

                }
            });
            socket.On(Socket.EVENT_CONNECT_TIMEOUT, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evconntimout:");
                });
            });

            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status everr:");
                });
            });
            socket.On(Socket.EVENT_RECONNECT, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evcreconn:");
                });
            });
            socket.On(Socket.EVENT_RECONNECT_ATTEMPT, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evcreconnatt:");
                });
            });
            socket.On(Socket.EVENT_RECONNECT_ERROR, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evrecerr:");
                });
            });
            socket.On(Socket.EVENT_RECONNECT_FAILED, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evrecfail:");
                });
            });
            socket.On(Socket.EVENT_RECONNECTING, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status ecreconnecting:");
                });
            });

            socket.On("parse", () =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("needparse:");
                });
                //richStatus.AppendText;
                socket.Emit("parseddata", ext.renderJson(events));
                parsematches();
                counter++;
                if (counter > 20)
                {
                    counter = 0;
                    richStatus.Invoke((MethodInvoker)delegate
                    {
                        richStatus.Text = "";// "needparse:");
                    });
                }
                //richStatus.AppendText(renderJson());
            });
            socket.On("reload", () =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Need reload");
                });
                //richStatus.AppendText("needreload:");
                loadMatches();
                //richStatus.AppendText(renderJson());
            });

        }
    }
}
