using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Socket = Quobject.SocketIoClientDotNet.Client.Socket;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading;

namespace Bet365Newparser.bet365
{
    public class GamesArr
    {
        public string Coefficent1 { get; set; }
        public string Coefficent2 { get; set; }
        public string GameNumber { get; set; }
        public string SetNumber { get; set; }
        public GamesArr(string Setnum,string gamenum,string cf1,string cf2)
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
                GamesArr = new List<bet365.GamesArr>();
            GamesArr.Add(game);
        }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
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
    }


    class Bet365Simple
    {
        Socket socket;
        public List<string> matchesid = new List<string>();
        public static string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36 Edge/14.14393";
        public static string BET365_HOME = "https://mobile.bet365.com";
        private CookieContainer _cookie = new CookieContainer();
        private readonly Uri _cookieHostname = new Uri(BET365_HOME);
        public RichTextBox rich;
        public List<Event> listevents;
        public WebBrowser wb;
        bool inited=false;
        public void initsock()
        {
            int counter = 0;
            var richStatus = rich;
            Inited = false;
            //var options = new IO.Options() { ForceNew = false };
            socket = IO.Socket("http://127.0.0.1:30365");
            
            socket.Connect();
            
            socket.On(Socket.EVENT_DISCONNECT, (data) =>
            {
                socket.Connect();
                richStatus.Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evdisc:");
                });
            });
            socket.On(Socket.EVENT_CONNECT, (data) =>
            {
                richStatus.Invoke((MethodInvoker)delegate
                { richStatus.AppendText("Status evconn:"); });
                
            });
            socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
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
                parsematches();
                socket.Emit("parseddata", renderJson());
                counter++;
                if(counter>20)
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
                    richStatus.AppendText("needreload:");
                });
                //richStatus.AppendText("needreload:");
                loadMatches();
                //richStatus.AppendText(renderJson());
            });

        }
        static double FractionToDouble(string fraction)
        {
            double result;
            if (fraction != "")
            {


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
            }
            else return 1;
            throw new FormatException("Not a valid fraction.");
        }
        public CookieContainer GetCookieContainer()
        {
            CookieContainer container = new CookieContainer();
            Uri target = new Uri("https://mobile.bet365.com");
            foreach (string cookie in wb.Document.Cookie.Split(';'))
            {
                string name = cookie.Split('=')[0];
                string value = cookie.Substring(name.Length + 1);

                container.Add(new Cookie(name.Trim(), value.Trim()) { Domain = target.Host });
            }

            return container;
        }
        
        [STAThread]
        void initcookese()
        {

            if (wb == null)
            {
                var th = new Thread(() => {
                    wb = new WebBrowser() { ScriptErrorsSuppressed = true };

                    wb.Navigate("https://mobile.bet365.com");
                    Application.Run();
                });
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            while (wb==null||wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            rich.AppendText("Navigated");
            this.Inited = true;
            passcookies(GetCookieContainer());
        }
        private void runBrowserThread(Uri url)
        {
            
        }


        void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction)
        {
                Action wrapperAction = () =>
                {
                    request.BeginGetResponse(new AsyncCallback((iar) =>
                    {
                        try
                        {
                            var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                            responseAction(response);
                        }
                        catch (System.Net.WebException ex)
                        {
                            addtexttorich(string.Join(" ", new { ex.HResult, ex.Message, ex.Status }));
                            if(ex.Status==WebExceptionStatus.ConnectFailure)
                            {
                                
                            }
                            if(ex.Status==WebExceptionStatus.ProtocolError&&ex.Message.Contains("400"))
                            {
                                //cookies refresh
                                Inited = false;
                                Application.Restart();
                                initcookese();
                            }
                            BET365_HOME = "https://mobile.bet365.com";
                            //changehost 
                        }
                    }), request);
                };
                wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
                {
                    var action = (Action)iar.AsyncState;
                    action.EndInvoke(iar);
                }), wrapperAction);
            
            
        }
        private Dictionary<string, Dictionary<string, string>> parameterizeLine(string line)
        {
            var chunk = line.Split(';');

            if (chunk.Length == 0)
                return null;

            var cmd = chunk[0];

            var map
                = new Dictionary<string, Dictionary<string, string>>();

            var paramDictionarys = chunk.Select(pstr => pstr.Split('=')).Where(pdata
                => pdata.Length == 2).ToDictionary(pdata => pdata[0], pdata => pdata[1]);

            map.Add(cmd, paramDictionarys);

            return map;
        }

        private void addtexttorich(string text)
        {
            rich.Invoke((MethodInvoker)delegate
            {
                rich.AppendText(text);
            });
        }
        public void loadMatches()
        {
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(BET365_HOME + "/mobilepreload/?pt=13&tl=OV_13_1_3&;CONFIG_1_3;OVInPlay_1_3;__time&ci=158");
            request.UserAgent = USER_AGENT;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            WebHeaderCollection myWebHeaderCollection = request.Headers;
            //Add the Accept-Language header (for Danish) in the request.
            myWebHeaderCollection.Add("Accept-Language:da");
            //Include English in the Accept-Langauge header. 
            myWebHeaderCollection.Add("Accept-Language", "en-US,en;q=0.8");
            //_cookie.Add(new Cookie("Cookie","aps03=lng=1&tzi=21&ct=181&cst=0&cg=0&oty=2; session=processform=0; pstk=76AFF31925024AE1BB65A44F95EE4890000003"));
            request.CookieContainer = _cookie;
            
            request.Proxy = null;
            request.Method = "GET";
            // init your request...then:


            DoWithResponse(request, (response) =>
            {
                var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var gameData = body.Split((char)0x01);

                var newParse = body.Split('|');
                var oldParse = gameData[gameData.Length - 1].Split((char)0x7c);
                var gameDateList = new List<string>(100);

                if (newParse.Length > oldParse.Length)
                {
                    gameDateList = newParse.ToList();
                    var countDeleteElems = gameDateList.TakeWhile(str => !str.Contains("CL")).Count();
                    gameDateList.RemoveRange(0, countDeleteElems);
                }
                else
                {
                    gameDateList = oldParse.ToList();
                    var countDeleteElems = gameDateList.TakeWhile(str => !str.Contains("CL")).Count();
                    gameDateList.RemoveRange(0, countDeleteElems);
                }
                if (gameDateList.Count == 0)
                {
                    addtexttorich("[Bet365]gameDateListNull: " + body);
                }
                var initialCL = parameterizeLine(gameDateList[0]);
                var paramsDic = new Dictionary<string, string>();
                if (initialCL != null)
                {
                    initialCL.TryGetValue("CL", out paramsDic);
                }
                else
                {
                    //addtexttorich("[Bet365] InitialCl is null. Data from server: " + body);
                }
                /*if (paramsDic == null)
                {
                    rich.addtexttorich("[Bet365] InitialCl is null. Data from server: " + gameDataRequest);
                    return null;
                }*/
                //rich.addtexttorich("[Bet365] InitialCl is good. Data from server: " + gameDataRequest);
                //addtexttorich("[Bet365] InitialCl is good." + "\n");
                var events = new List<string>(5);
                var isTennis = false;
                var isFirst = true;
                foreach (var data in gameDateList)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }

                    var lineData = parameterizeLine(data);
                    if (lineData == null)
                        continue;
                    if (!isTennis)
                    {
                        isTennis = true;
                        continue;
                    }
                    if (lineData.ContainsKey("CL"))
                    {
                        break;
                    }
                    if (lineData.ContainsKey("EV"))
                    {
                        var Id = "";
                        Dictionary<string, string> tmp;
                        lineData.TryGetValue("EV", out tmp);

                        if (tmp != null) tmp.TryGetValue("ID", out Id);
                        else addtexttorich("[Bet365]No id in parseline. Continue");

                        if (Id == null) addtexttorich("[Bet365] ID IS NULL");
                        if ((Id != null) && (Id.Trim().Length == 18))
                        {
                            events.Add(Id.Trim());
                        }
                    }
                }
                matchesid = events;
            });

        }

        public string renderJson()
        {
            if (listevents != null)
            {
                RootObject torender = new RootObject();
                torender.data = new List<Datum>();
                foreach (var _event in listevents)
                {
                    Datum toadd = new Datum();
                    if (_event != null && _event.CompetitionType != null)
                    {
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
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string text = serializer.Serialize(torender);
                return text;
            }
            else
                return "";
        }
        int delayer = 0;

        public bool Inited
        {
            get
            {
                return inited;
            }

            set
            {
                inited = value;
            }
        }

        public void parsematches()
        {
            delayer++;
            if (matchesid.Count > 0 && delayer == 4)
            {
                delayer = 0;
                List<Event> parsedevents = new List<Event>();
                
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(BET365_HOME + "/mobilepreload/?pt=13&tl=" + string.Join(";", matchesid) + "&CONFIG_1_3;OVInPlay_1_3;__time&ci=158");
                request.UserAgent = USER_AGENT;
                request.CookieContainer = _cookie;
                request.Proxy = null;
                request.Method = "GET";
                // init your request...then:

                try
                {
                    DoWithResponse(request, (response) =>
                    {
                        var body = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        var eventExpandedData = body.Split((char)0x01);
                        eventExpandedData = eventExpandedData.RemoveAt(0);
                        //eventExpandedData = eventExpandedData[eventExpandedData.Length - 1].Split((char)0x7c);
                        foreach (var element in eventExpandedData)
                        {
                            var localeventExpandedData = element.Split((char)0x7c);
                            var resultList = new List<Dictionary<string, string>>();
                            var currentRoot = new Dictionary<string, string>();

                            resultList.Add(new Dictionary<string, string>());
                            resultList.Add(new Dictionary<string, string>());

                            var firstItem = true;

                            string currentKey = null;
                            string competitionType = null;
                            string eventName = null;

                            string team1Score = null;
                            string team2Score = null;

                            string name1Player = null;
                            string name2Player = null;
                            string scoreAll = null;
                            List<string> list = new List<string>();
                            List<string> coefs = new List<string>();
                            int awaitstate = 0;
                            string id = "";
                            //int coefscount = 0;
                            //rich.AppendText( eventExpandedData.ToString());
                            foreach (var anEventExpandedData in localeventExpandedData)
                            {
                                var parsedLine = parameterizeLine(anEventExpandedData);

                                if (parsedLine == null)
                                    continue;
                                if (parsedLine.ContainsKey("FS"))
                                {
                                    parsedLine.TryGetValue("ID", out currentRoot);
                                }
                                if (parsedLine.ContainsKey("CO") && awaitstate == 1)
                                {
                                    awaitstate = 2;
                                }
                                if (parsedLine.ContainsKey("PA") && awaitstate == 2)
                                {
                                    awaitstate = 3;
                                    string value = null;
                                    string nameval = null;
                                    parsedLine.TryGetValue("PA", out currentRoot);
                                    Debug.Assert(currentRoot != null, "currentRoot != null");
                                    currentRoot?.TryGetValue("NA", out nameval);
                                    currentRoot?.TryGetValue("OD", out value);
                                    coefs.Add(Math.Round(FractionToDouble(value) + 1, 2).ToString());

                                    continue;
                                }
                                if (parsedLine.ContainsKey("PA") && awaitstate == 3)
                                {
                                    awaitstate = 3;
                                    string value = null;
                                    string nameval = null;
                                    parsedLine.TryGetValue("PA", out currentRoot);
                                    Debug.Assert(currentRoot != null, "currentRoot != null");
                                    currentRoot?.TryGetValue("NA", out nameval);
                                    currentRoot?.TryGetValue("OD", out value);
                                    coefs.Add(Math.Round(FractionToDouble(value) + 1, 2).ToString());
                                    //swap cfs if player 2 dont contains svr
                                    if (!nameval.Contains(name2Player))
                                    {
                                        string coef1 = coefs[coefs.Count - 1];
                                        string coef2 = coefs[coefs.Count - 2];
                                        coefs[coefs.Count - 1] = coef2;
                                        coefs[coefs.Count - 2] = coef1;
                                    }
                                    awaitstate = 0;
                                }
                                if (parsedLine.ContainsKey("EV"))
                                {
                                    //Event
                                    parsedLine.TryGetValue("EV", out currentRoot);
                                    Debug.Assert(currentRoot != null, "currentRoot != null");
                                    currentRoot?.TryGetValue("CT", out competitionType);
                                    currentRoot?.TryGetValue("NA", out eventName);
                                    currentRoot?.TryGetValue("SS", out scoreAll);
                                    string[] split = new string[1];
                                    split[0] = " v ";
                                    name1Player = eventName.Split(split, StringSplitOptions.RemoveEmptyEntries)[0].TrimEnd(' ');
                                    name2Player = eventName.Split(split, StringSplitOptions.RemoveEmptyEntries)[1].TrimStart(' ');
                                }
                                else if (parsedLine.ContainsKey("MA"))
                                {
                                    string value = null;
                                    parsedLine.TryGetValue("MA", out currentRoot);
                                    Debug.Assert(currentRoot != null, "currentRoot != null");
                                    currentRoot?.TryGetValue("NA", out value);
                                    if (value.Contains("Game Winner"))
                                    {
                                        //    [15]: "CO;CN=2;ID=1;IT=55919341C13-1764-1_1_9;NA=;OR=0;SY=0;"
                                        //[16]: "PA;FI=55961355;HA=;HD=;ID=991462536;IT=P991462536_1_9;NA=Timea Babos (Svr);OD=4/7;OR=0;PX=;SU=0;"
                                        //[17]: "PA;FI=55961355;HA=;HD=;ID=991462537;IT=P991462537_1_9;NA=Carla Suarez Navarro;OD=5/4;OR=1;PX=;SU=0;"
                                        //rich.AppendText(name1Player+" "+ name2Player+ " " + value + Environment.NewLine);
                                        awaitstate = 1;
                                        if (value.Length >= 16)
                                            value = value.Remove(2, value.Length - 2);
                                        else
                                            value = value.Remove(1, value.Length - 1);
                                        list.Add(value);
                                    }
                                }
                                else if (parsedLine.ContainsKey("SC"))
                                {
                                    parsedLine.TryGetValue("SC", out currentRoot);
                                    if (firstItem)
                                    {
                                        currentKey = "name";
                                        firstItem = false;
                                    }
                                    else
                                    {
                                        Debug.Assert(currentRoot != null, "currentRoot != null");
                                        currentRoot.TryGetValue("NA", out currentKey);
                                    }
                                }
                                else if (parsedLine.ContainsKey("TE"))
                                {
                                    parsedLine.TryGetValue("TE", out currentRoot);
                                    var equelsValue = "";
                                    currentRoot.TryGetValue("OR", out equelsValue);
                                    if (string.Equals(equelsValue, "0"))
                                    {
                                        currentRoot.TryGetValue("PO", out team1Score);
                                        currentRoot.TryGetValue("NA", out name1Player);
                                    }
                                    else
                                    {
                                        currentRoot.TryGetValue("NA", out name2Player);
                                        currentRoot.TryGetValue("PO", out team2Score);
                                    }
                                }
                            }

                            if (competitionType == null)
                            {
                                //addtexttorich("[Bet365] Competition Type null");
                                //addtexttorich("Repsonse from server length" + body.Length);

                            }
                            var team1 = new Team(name1Player, team1Score);
                            var team2 = new Team(name2Player, team2Score);
                            var eEvent = (competitionType != null) ? new Event(id, competitionType, team1, team2, scoreAll.Replace(",", " ").Replace("-", " ")) : null;
                            for (var i = 0; i < list.Count; i++)
                            {
                                try
                                {
                                    eEvent.addgame(new Game("0", list[i], coefs[i * 2].Replace(",", "."), coefs[i * 2 + 1].Replace(",", ".")));
                                }
                                catch
                                {

                                }
                            }


                            try
                            {
                                parsedevents.Add(eEvent);
                                //addtexttorich(name1Player + " " + name2Player + " " + list.Count + " " + list[0] + Environment.NewLine + coefs[0] + " " + coefs[1] + Environment.NewLine);
                                coefs = new List<string>();

                            }
                            catch
                            {

                            }

                        }
                        if(parsedevents.Count>0&&parsedevents[0]!=null)
                        listevents = parsedevents;
                        foreach (var _event in listevents)
                        {
                            //addtexttorich(_event.ToString());
                        }
                    });
                }
                catch//(Exception ex)
                {
                    //ex;
                    rich.AppendText("ex");
                }
            }
        }

        public void passcookies(CookieContainer cookieContainer)
        {
            _cookie = cookieContainer;
        }
    }
}