using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bet365Newparser.bet365
{
    public class NewBet365data
    {
        bet365data dataparaser = new bet365data();
        public static string BET365_HOME = "https://mobile.365sb.com";
        public static string USER_AGENT = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
        private static readonly string[] CHANNELS = { "OVInPlay_1_9", "OVInPlay_1_3", "OVInPlay_1_6" };
        public RichTextBox rich;
        /*New*/
        private readonly CookieContainer _cookie = new CookieContainer();
        private readonly Uri _cookieHostname = new Uri(BET365_HOME);
        private readonly List<string> _ports = new List<string>();
        private readonly List<string> hosts = new List<string>();
        private string _clientId;
        private string _clientRn;
        private string _homePage;
        private int _serverNum;
        public List<string> listofMatches = new List<string>();
        
        private int GenerateRandom(int min, int max)
        {
            var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
            return new Random(seed).Next(min, max);
        }
        //load from site https://mobile.365sb.com/mobilepreload/?pt=13&tl=15299657325C13_1_3&;CONFIG_1_3;LIInPlay_1_3;__time&ci=158
        //parse scoreall is SS=4-6,5-7,5-3;
        //parse current points is XP=40-30;
        //parse players is NA=Fabio Fognini v Federico Delbonis;
        //parse eventname CT=2016 Davis Cup;
        //parse gamewinner NA=32nd Game Winner;OR=3;OT=0;PI=;PT=;SU=0;TO=GD;UC=;|CO;CN=2;ID=1;IT=57379181C13-1764-1_1_3;NA=;OR=0;SY=0;|PA;FI=57384587;HA=;HD=;ID=1049432372;IT=P1049432372_1_3;NA=Fabio Fognini (Svr);OD=2/7;OR=0;PX=;SU=0;|PA;FI=57384587;HA=;HD=;ID=1049432373;IT=P1049432373_1_3;NA=Federico Delbonis;OD=5/2;

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
        private void GetCookies()
        {
            var html = new StringBuilder();
            var req = (HttpWebRequest)WebRequest.Create(BET365_HOME);
            //req.Timeout = 5000;
            req.UserAgent = USER_AGENT;
            req.CookieContainer = _cookie;
            req.Proxy = null;
            var encode = Encoding.GetEncoding("utf-8");

            using (var response = (HttpWebResponse)req.GetResponse())
            {
                var cookies = response.Cookies;
                _cookie.Add(_cookieHostname, cookies);
                var readStream = new StreamReader(response?.GetResponseStream(), encode);
                html.Append(readStream.ReadToEnd());
                readStream.Close();
                response.Close();
                var stringBuilder = new StringBuilder();
                /* for (int i = 0; i < cookies.Count; i++)
                {
                    stringBuilder.AppendLine("[" + i + "] " + cookies[i]);
                }
                rich.AppendText("Cookie: " + stringBuilder.ToString());*/
            }
            _homePage = html.ToString();
        }
        private string GetProperty(string property)
        {
            string pattern;

            switch (property)
            {
                case "sessionId":
                    pattern = property + "\":\"(.*?)\"";
                    break;
                case "ConnectionDetails":
                    pattern = property + "\":\\[(.*?)\\]";
                    break;
                default:
                    return null;
            }

            var reg = new Regex(pattern);
            var match = reg.Match(_homePage);
            if (match.Success)
            {
                return match.Value;
            }
            rich.AppendText("[Bet365] Success == false");
            return null;
        }
        private void SetConnectionDetails(string connectionDetails)
        {
            var pattern = "Host\":\"(.*?)\"";
            var reg = new Regex(pattern);
            var match = reg.Match(connectionDetails);
            var oneMatch = match?.Value.Remove(0, 7);
            var twoMatch = match.NextMatch()?.Value.Remove(0, 7);
            hosts.Add(oneMatch.Remove(oneMatch.Length - 1, 1));
            hosts.Add(twoMatch.Remove(twoMatch.Length - 1, 1));

            pattern = "Port\":(.*?),";
            var regPort = new Regex(pattern);
            var matherPort = regPort.Match(connectionDetails);
            var oneMatchPort = matherPort?.Value.Remove(0, 6);
            var twoMatchPort = matherPort.NextMatch()?.Value.Remove(0, 6);
            _ports.Add(oneMatchPort.Remove(oneMatchPort.Length - 1, 1));
            _ports.Add(twoMatchPort.Remove(twoMatchPort.Length - 1, 1));
        }
        private void SetConnection()
        {
            GetCookies();
            if (_cookie.Count == 0) return;
            var sessionId = GetProperty("sessionId");
            if (sessionId == null) return;
            var connectionDetails = GetProperty("ConnectionDetails");
            if (connectionDetails == null) return;

            sessionId = sessionId.Remove(0, 12);
            sessionId = sessionId.Remove(sessionId.Length - 1, 1);

            SetConnectionDetails(connectionDetails);
            if (hosts.Count != 2 || _ports.Count != 2) return;

            var buffer = new StringBuilder();

            for (var i = 0; i < 16; i++)
            {
                buffer.Append(GenerateRandom(0, 10));
            }
            _clientRn = buffer.ToString();

            var headesCollection = new WebHeaderCollection
            {
                {"method", "0"},
                {"topic", "__time,S_" + sessionId},
                {"transporttimeout", "20"},
                {"type", "F"}
            };


            var postResponce = powRequest(0, headesCollection);
            var temp = postResponce.Split((char)0x02);
            _clientId = temp[1];
            rich.AppendText("=========");
            rich.AppendText("Tmp count: " + temp.Length);
            rich.AppendText("ClientID: " + _clientId);
            rich.AppendText("ClientRN: " + _clientRn);
            rich.AppendText("SessionID: " + sessionId);
            rich.AppendText("=========");
        }

        public void Setup()
        {
            try
            {
                rich.AppendText("[Bet365]Start parsing Bet365");

                SetConnection();
            }
            catch (Exception e)
            {
                rich.AppendText("[Bet365]Ex: " + e.Message + "stack: " + e.StackTrace);
            }

        }
        public List<Event> ParseAll()
        {
            var eventsList = new List<Event>();
            try
            {
                rich.AppendText("[Bet365]Start parsing Bet365");

                SetConnection();
            }
            catch (Exception e)
            {
                rich.AppendText("[Bet365]Ex: " + e.Message + "stack: " + e.StackTrace);
                return default(List<Event>);
            }
            var matches = new List<string>();
            var success = false;
            try
            {
                foreach (var channel in CHANNELS)
                {
                    rich.AppendText("[Bet365]Trying to get events list from " + channel + " channel.");
                    var result = GetAvailableMatches(channel);
                    if (result == null)
                    {
                        rich.AppendText("[Bet365] Result is null");
                        success = false;
                        continue;
                    }
                    if (matches.Count == 0)
                    {
                        if (result.Count != 0)
                        {
                            matches = result;
                            rich.AppendText("[BET365] ALL OK WE FOUND ELEMS \n");
                            success = true;
                            break;
                        }
                    }
                    if (matches.Count < result.Count)
                    {
                        matches = result;
                        rich.AppendText("[Bet365] matches is more elements than previus: " + matches.Count + " : " +
                                        result.Count);
                        success = true;
                        break;
                    }
                    success = true;
                }
            }
            catch (Exception e)
            {
                rich.AppendText("[Bet365]Ex in Chanel Get Mathes: " + e.Message + "stack: " + e.StackTrace);
                return default(List<Event>);
            }
            if (!success) return null;
            try
            {
                if (matches.Count > 0)
                    listofMatches = matches;
                foreach (var match in matches)
                {
                    rich.AppendText(match + Environment.NewLine);
                }
            }
            catch (Exception e)
            {
                rich.AppendText("[Bet365]Ex in getTennisEvent Score: " + e.Message + "stack: " + e.StackTrace);
                return default(List<Event>);
            }
            return eventsList;
        }
        public string powRequest(int sid, WebHeaderCollection specialHeaders)
        {
            var defaultHeaders = new WebHeaderCollection();

            if (_clientId != null)
            {
                defaultHeaders.Add("clientid", _clientId);
            }

            if (sid != 0)
            {
                defaultHeaders.Add("s", _serverNum.ToString());
                _serverNum++;
            }
            var totalHeaders = new WebHeaderCollection { specialHeaders, defaultHeaders };
            var url = hosts.ElementAt(1) + @"/pow/?sid=" + sid + "&rn=" + _clientRn;
            return Connection.PostRequest(url, totalHeaders);
        }
        private void Subscribe(string channel)
        {
            var headers = new WebHeaderCollection { { "method", "22" }, { "topic", channel } };
            powRequest(2, headers);
        }

        private void Unsubscribe(string channel)
        {
            var headers = new WebHeaderCollection { { "method", "23" }, { "topic", channel } };
            powRequest(2, headers);
        }
        private List<string> GetAvailableMatches(string channel)
        {
            var matches = new List<string>();
            try
            {
                matches = GetEvents(channel);
            }
            catch (IOException e)
            {
                rich.AppendText(e.StackTrace);
            }

            return matches;
        }
        private List<string> GetEvents(string channel)
        {
            Subscribe(channel);
            var headers = new WebHeaderCollection { { "method", "1" } };
            var gameDataRequest = powRequest(2, headers);
            if (string.IsNullOrWhiteSpace(gameDataRequest))
            {
                rich.AppendText("[Bet365] Null string");
                return null;
            }
            var gameData = gameDataRequest.Split((char)0x01);

            var newParse = gameDataRequest.Split('|');
            var oldParse = gameData[gameData.Length - 1].Split((char)0x7c);
            var gameDateList = new List<string>(100);
            if (newParse.Length == oldParse.Length)
                return default(List<string>);
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
                rich.AppendText("[Bet365]gameDateListNull: " + gameDataRequest);
                return null;
            }
            var initialCL = parameterizeLine(gameDateList[0]);
            var paramsDic = new Dictionary<string, string>();
            if (initialCL != null)
            {
                initialCL.TryGetValue("CL", out paramsDic);
            }
            else
            {
                rich.AppendText("[Bet365] InitialCl is null. Data from server: " + gameDataRequest);
            }
            /*if (paramsDic == null)
            {
                rich.AppendText("[Bet365] InitialCl is null. Data from server: " + gameDataRequest);
                return null;
            }*/
            //rich.AppendText("[Bet365] InitialCl is good. Data from server: " + gameDataRequest);
            rich.AppendText("[Bet365] InitialCl is good.");
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
                    if (!lineData.ContainsKey("CL"))
                        continue;
                    isTennis = true;
                    continue;
                }
                if (lineData.ContainsKey("CL"))
                {
                    break;
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
                    else rich.AppendText("[Bet365]No id in parseline. Continue");

                    if (Id == null) rich.AppendText("[Bet365] ID IS NULL");
                    if ((Id != null) && (Id.Trim().Length == 18))
                    {
                        events.Add(Id.Trim());
                    }
                }
            }
            Unsubscribe(channel);
            return events;
        }

    }
}
