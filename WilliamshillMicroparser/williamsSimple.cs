using MicroparserFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace WilliamshillMicroparser
{
    /*public static class WebSocketExtensions
    {
        public static async Task OpenAsync(
            this WebSocket webSocket,
            int retryCount = 5,
            CancellationToken cancelToken = default(CancellationToken))
        {
            var failCount = 0;
            var exceptions = new List<Exception>(retryCount);

            var openCompletionSource = new TaskCompletionSource<bool>();
            cancelToken.Register(() => openCompletionSource.TrySetCanceled());

            EventHandler openHandler = (s, e) => openCompletionSource.TrySetResult(true);

            EventHandler<ErrorEventArgs> errorHandler = (s, e) =>
            {
                if (exceptions.All(ex => ex.Message != e.Exception.Message))
                {
                    exceptions.Add(e.Exception);
                }
            };

            EventHandler closeHandler = (s, e) =>
            {
                if (cancelToken.IsCancellationRequested)
                {
                    openCompletionSource.TrySetCanceled();
                }
                else if (++failCount < retryCount)
                {
                    webSocket.Open();
                }
                else
                {
                    var exception = exceptions.Count == 1
                        ? exceptions.Single()
                        : new AggregateException(exceptions);

                    openCompletionSource.TrySetException(new Exception("Unable to connect", exception));
                }
            };

            try
            {
                webSocket.Opened += openHandler;
                webSocket.Error += errorHandler;
                webSocket.Closed += closeHandler;

                webSocket.Open();
                await openCompletionSource.Task.ConfigureAwait(false);
            }
            finally
            {
                webSocket.Opened -= openHandler;
                webSocket.Error -= errorHandler;
                webSocket.Closed -= closeHandler;
            }
        }
    }*/
    public class williamsSimple
    {
        //http://www.wincomparator.com/en-gb/live-scores/tennis/
        bool needproxy = false;
        List<RootObject> jsons = new List<RootObject>();
        public List<string> matchesid = new List<string>();
        public List<Event> games = new List<Event>();
        public Dictionary<Result, Teams> scores = new Dictionary<Result, Teams>();

        private AngleSharp.Parser.Html.HtmlParser parser = new AngleSharp.Parser.Html.HtmlParser();
        public RichTextBox richTextBox1;
        microserver server;
        //PhantomJS phantomJS = new PhantomJS();

        public williamsSimple(RichTextBox rich)
        {
            server = new microserver(rich);
            server.setActions(loadgames, loadmatches);
            server.init("30385");
            /*phantomJS.OutputReceived += (sender, e) =>
            {
                Console.WriteLine("PhantomJS output: {0}", e.Data);
            };
            phantomJS.RunAsync("./ScoresParser.js",null);*/
        }
        public async void loadmatches()
        {
            richTextBox1.Clear();
            try
            {
                matchesid.Clear();
                string page = "http://sports.williamhill.com/bet/en-gb/betlive/24";

                // ... Use HttpClient.

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = new WebProxy("http://185.5.64.70:8080"),
                    UseProxy = true
                };

                using (HttpClient client = !needproxy ? new HttpClient() : new HttpClient(httpClientHandler) { })
                using (HttpResponseMessage response = await client.GetAsync(page))
                using (HttpContent content = response.Content)
                {
                    // ... Read the string.
                    jsons = new List<RootObject>();
                    string result = await content.ReadAsStringAsync();
                    string[] sep = new string[2];
                    sep[0] = "document.aip_list.create_prebuilt_event(";
                    sep[1] = ");";
                    // ... Display the result.
                    if (result != null &&
                    result.Length >= 50)
                    {
                        var findalldata = result.Split(new string[] { "document.aip_list.create_prebuilt_event(" }, StringSplitOptions.None);
                        findalldata = findalldata.RemoveAt(0);
                        for (int i = 0; i < findalldata.Length; i++)
                        {
                            findalldata[i] = findalldata[i].Substring(4, findalldata[i].IndexOf(");") - 4).Trim().TrimStart(System.Environment.NewLine.ToCharArray());
                            JavaScriptSerializer serial = new JavaScriptSerializer();
                            var jsondata = serial.Deserialize<RootObject>(findalldata[i]);
                            if (jsondata.is_running == "1")
                                jsons.Add(jsondata);

                        }
                        //string go = "";
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        //string text = serializer.Serialize(findalldata);
                        foreach (var json in jsons)
                        {
                            if (json.is_running == "1")
                                matchesid.Add(json.event_link);
                        }
                        richTextBox1.AppendText(string.Join("\n", jsons.Select((s) => s.event_link)));
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                richTextBox1.AppendText(ex.Message);
                needproxy = true;
            }
        }
        async Task<int> ProcessURLAsync(string url, HttpClient client)
        {
            try
            {
                var byteArray = await client.GetStringAsync(url);
                DisplayResults(url, byteArray);
                return byteArray.Length;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("410") || ex.Message.Contains("404"))
                {
                    matchesid.Remove(url);
                }
                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText(ex + "\n");
                        //  var m = JsonConvert.DeserializeObject<List<string>>(data);
                    });

                return 0;
            }

        }

        public void loadscores()
        {
            scores.Clear();

            string[] args = new string[1];
            args[0] = "http://www.google.com/";
            //phantomJS.RunScript(@"page.evaluate()", args);
            /*using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("https://ls.sportradar.com/ls/feeds/?/betradar/en/Europe:Berlin/gismo/event_get"))
            using (HttpContent content = response.Content)
            {
                JavaScriptSerializer serial = new JavaScriptSerializer();
                var json = await content.ReadAsStringAsync();
                var data =serial.Deserialize<ScoreClass>(json);
                foreach(var l1 in data.doc[0].data)
                {
                    if(l1._sid==5)
                    {
                        //needparse
                        scores.Add(l1.match.result, l1.match.teams);
                        //scores.Add(l1.matchid,);
                    }
                }
            }
            /*for(int i=0;i<matchesid.Count;i++)
            {
                var el = matchesid[i];
                var data= await getscore(el);
                if (scores.ContainsKey(el)&&data!="")
                    scores[el] = data;
                else if(data!="")
                    scores.Add(el, data);
            }*/
            richTextBox1.AppendText(string.Join("\n", scores.Values));
        }

        public void messageparser(string message)
        {

        }
        /* private void sendalldata(string ev, WebSocket socket)
         {
             string evdata = ev.Split('/')[7];
             socket.Send("tennis/matches/" + evdata + "/eventName");
             //socket.Send("\u0019"+"tennis/matches/"+evdata+"totalSets");
             //socket.Send("\u0019"+"tennis/matches/"+evdata+"currentSet");
             /*socket.Send("\u0019"+"tennis/matches/"+evdata+"competitors" +"/A/teamName" );
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/competitors" +"/B/teamName" );
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/1/gamesWon/A");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/1/gamesWon/B");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/2/gamesWon/A");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/2/gamesWon/B");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/3/gamesWon/A");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/3/gamesWon/B");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/4/gamesWon/A");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/4/gamesWon/B");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/5/gamesWon/A");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"/previousSets"+"/5/gamesWon/B");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"currentSet"  +"/gamesWon/A");
             socket.Send("\u0019"+"tennis/matches/"+evdata+"currentSet"  +"/gamesWon/B");
         }*/
        /*private async Task<string> getscore(string url)
        {
            return await Task<string>.Run(() =>
            {
                WebSocket ws = new WebSocket("ws://scoreboards.williamhill.com/diffusion?t=null&v=4&ty=WB") { NoDelay = true };
                string score = "";

                string ret = "";
                int count = 0;
                bool state = true;

                ws.MessageReceived += (sender, e) =>
                {
                    Console.WriteLine("Server says: " + e.Message);
                    Int64 value = 0;
                    if (long.TryParse(e.Message.Replace("\u0019", "").Replace("\u0001", ""), out value) && value > 1470000000000)
                    {
                        //ws.Send(@"\u0019"+e.Message+ @"\u0001");
                        //Console.WriteLine("recived mess" + e.Message);
                        count--;
                    }
                    count++;
                    if (count == 1)
                    {

                    }
                    if (count == 2)
                    {

                        ret = e.Message.Split("\u0001".ToCharArray())[1];
                        if (ret == "inPlay")
                        {
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/competitors" + "/A/teamName");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/competitors" + "/B/teamName");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/1/gamesWon/A");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/1/gamesWon/B");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/2/gamesWon/A");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/2/gamesWon/B");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/3/gamesWon/A");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/3/gamesWon/B");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/4/gamesWon/A");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/4/gamesWon/B");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/5/gamesWon/A");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/previousSets" + "/5/gamesWon/B");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/currentSet" + "/gamesWon/A");
                            ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/currentSet" + "/gamesWon/B");
                        }
                        else
                            ws.Close();
                        //ws.Close();

                    }
                    if (count > 4)
                    {
                        if (e.Message.Length > 15)
                        {
                            if (e.Message.Split('\u0001').Last().ToString() != "")
                                score += e.Message.Split('\u0001').Last().ToString() + " ";
                        }



                    }
                    if (e.Message.Contains("currentSet/gamesWon/B"))
                    {
                        state = !state;
                        ws.Close();
                    }
                    //ws.Close();
                };
                ws.Opened += (sender, e) =>
                {
                    //Console.WriteLine("Connected " + url);
                    ws.Send("\u0016tennis/matches/" + url.Split('/')[7] + "/phase");
                };
                ws.Error += (sender, e) => Console.WriteLine("Error " + e.Exception);
                ws.Closed += (sender, e) =>
                {
                    Console.WriteLine("Closed " + url);
                    state = !state;
                };
                if (ws.State != WebSocketState.Open)
                    ws.Open();

                while (state && ws.State != WebSocketState.Closed)
                {

                }
                return score;
            });
        }*/
        private void DisplayResults(string url, string content)
        {
            // Display the length of each website. The string format 
            // is designed to be used with a monospaced font, such as
            // Lucida Console or Global Monospace.
            try
            {
                var bytes = content.Length;
                // Strip off the "http://".
                //var displayURL = url.Replace("http://", "");


                var document = parser.Parse(content);
                var list = document.Children[0].GetElementsByClassName("leftPad title");
                var scoreBoard = document.GetElementsByClassName("livePushContent");
                var SetNumber = "";
                var GameNumber = "";
                var Player2 = "";
                var Player1 = "";
                var Coefficent2 = "";
                var Coefficent1 = "";
                var Score = "";
                //var CurrentGames = "";
                //var CurrentPoints = "";
                var GamesArray = new string[10];
                //var LenArr = "";
                var data = new string[10];
                Event myev = null;
                if (list != null)
                {
                    for (var l = 0; l < list.Length; l++)
                    {

                        if (list[l].TextContent.Contains("Set - Game") && !list[l].TextContent.Contains("Point") && !list[l].TextContent.Contains("Score") && !list[l].TextContent.Contains("Total") && !list[l].TextContent.Contains("Win ") && !list[l].TextContent.Contains("Games"))
                        {
                            var childnodes = list[l].ParentElement.ParentElement.ParentElement.ParentElement.Children;

                            if (childnodes.Length >= 1 && childnodes[0] != null && childnodes[0].GetElementsByClassName("eventselection")[0] != null && childnodes[0].GetElementsByClassName("eventselection")[0].TextContent != "")
                            {

                                SetNumber = list[l].ChildNodes[7].ChildNodes[0].TextContent;
                                string re = "^[0-9]";
                                GameNumber = SetNumber.Substring(SetNumber.Length - 2);
                                SetNumber = Regex.Match(SetNumber, re, RegexOptions.IgnoreCase).Value;
                                Player1 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.Children[0].GetElementsByClassName("eventselection")[0].TextContent.Trim();

                                Player2 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.Children[0].GetElementsByClassName("eventselection")[1].TextContent.Trim();
                                try
                                {
                                    Coefficent1 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.Children[0].GetElementsByClassName("eventprice")[0].TextContent.Trim();
                                    Coefficent2 = list[l].ParentElement.ParentElement.ParentElement.ParentElement.Children[0].GetElementsByClassName("eventprice")[1].TextContent.Trim();
                                    if (Coefficent1 == "EVS")
                                    {
                                        Coefficent1 = "1";
                                    }
                                    if (Coefficent2 == "EVS")
                                    {
                                        Coefficent2 = "1";
                                    }
                                }
                                catch
                                {
                                    Coefficent1 = "1";
                                    Coefficent2 = "1";
                                }
                                if (games.Count >= 0)
                                {
                                    if (games.Count == 0 || games[games.Count - 1].Team1.getName() != Player1)
                                    {
                                        myev = new Event(url, "", new Team(Player1, "0"), new Team(Player2, "0"), Score);
                                        games.Add(myev);
                                    }
                                }
                                var toadd = new Game(SetNumber, GameNumber, Math.Round(1 + ext.FractionToDouble(Coefficent1), 3).ToString().Replace(",", "."), Math.Round(1 + ext.FractionToDouble(Coefficent2), 3).ToString().Replace(",", "."));
                                if (toadd != null)
                                    games[games.Count - 1].addgame(toadd);
                                Score = "";
                            }
                        };
                    }

                }
            }
            catch(Exception ex)
            {
                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText(ex.Message);
                });
            }
            //if (myev != null)
            //games.Add(myev);
            richTextBox1.Invoke((MethodInvoker)delegate
                        {
                                //richTextBox1.AppendText(url.Split('/')[7] + "\n" /*+Score + "\n" */);
                            });
        }

        private async Task CreateMultipleTasksAsync()
        {

            // Declare an HttpClient object, and increase the buffer size. The
            // default buffer size is 65,536.

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = new WebProxy("http://185.99.13.70:8080"),
                UseProxy = true
            };

            using (HttpClient client = !needproxy ? new HttpClient() : new HttpClient(httpClientHandler))
            {

                // Create and start the tasks. As each task finishes, DisplayResults 
                // displays its length.
                List<Task<int>> tasks = new List<Task<int>>();
                foreach(var link in matchesid)
                {
                    tasks.Add(ProcessURLAsync(link, client));
                }
              
                int total = 0;
                /*
                Parallel.ForEach(tasks, async (task) => {
                    if(task!=null)
                    total += await task; });
                    */
                Parallel.ForEach(tasks, async (task) =>
                {
                    if (task != null)
                    {
                        await task; total++;
                    }
                });
                try
                {
                    games.Clear();
                    var taskkkil = await Task.WhenAll<int>(tasks.Where(t => t != null).ToArray());
                }
                catch (AggregateException ae)
                {
                    ae.Handle((x) =>
                    {
                        if (x is UnauthorizedAccessException) // This we know how to handle.
                            {
                            Console.WriteLine("You do not have permission to access all folders in this path.");
                            Console.WriteLine("See your network administrator or try another path.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Error " + x.Message);
                        }
                        return false; // Let anything else stop the application.
                        });
                }
                // Display the total count for the downloaded websites.

                richTextBox1.Text += total + " " + jsons.Count + " " + GC.GetTotalMemory(true);
                richTextBox1.AppendText("Totalgames in games" + games.Count.ToString());
            }
        }
        Task tsk;
        public async void loadgames()
        {

            if (tsk == null || tsk.IsCompleted)
            {
                richTextBox1.Clear();
                
                tsk = CreateMultipleTasksAsync();
                await tsk;
                //loadscores();
                foreach (var el in games)
                {
                    /*if (scores.ContainsKey(el.EventId))
                        el.ScoreAll = scores[el.EventId];
                    else
                        el.ScoreAll = "0 0";
                        */
                }
                server.events = games;
            }
            //richTextBox1.AppendText( ext.renderJson(games));
        }
        private int scoretrig = 0;
        private void updatescore()
        {
            scoretrig++;
            if (scoretrig > 2)
                scoretrig = 0;
            loadscores();

        }
    }
    public class Sport
    {
        public string sport_id { get; set; }
        public string sport_name { get; set; }
        public string prematch_disporder { get; set; }
        public string inplay_disporder { get; set; }
    }

    public class Type
    {
        public string type_id { get; set; }
        public string type_name { get; set; }
        public string disporder { get; set; }
    }

    public class ComClock
    {
        public string state { get; set; }
        public string period { get; set; }
        public string time { get; set; }
    }

    public class ComScore
    {
        public string score_1 { get; set; }
        public string score_2 { get; set; }
        public string comp1_score { get; set; }
        public string comp2_score { get; set; }
    }

    public class Collection
    {
        public string collection_id { get; set; }
        public string expanded { get; set; }
        public string name { get; set; }
        public string num_disp_mkt { get; set; }
        public string primary { get; set; }
    }

    public class Market
    {
        public string ev_mkt_id { get; set; }
        public string ev_oc_grp_id { get; set; }
        public string mkt_sort { get; set; }
        public string status { get; set; }
        public string mkt_name { get; set; }
        public string master_mkt_name { get; set; }
        public string grp_id { get; set; }
        public string displayed { get; set; }
        public string preloaded { get; set; }
        public string disporder { get; set; }
        public string expanded { get; set; }
        public string bir_index { get; set; }
        public string ew_avail { get; set; }
        public string ew_places { get; set; }
        public string ew_fac_num { get; set; }
        public string ew_fac_den { get; set; }
        public string hcap_value { get; set; }
        public string blurb { get; set; }
        public string col_id { get; set; }
        public string col_expanded { get; set; }
        public string col_disporder { get; set; }
        public string col_name { get; set; }
        public string bet_in_run { get; set; }
        public string last_msg_id { get; set; }
        public string template_name { get; set; }
        public string template_grp_id { get; set; }
        public string template_grp_col { get; set; }
        public string template_num_col { get; set; }
        public string template_col_header { get; set; }
        public string disp_sort { get; set; }
        public string tooltip_visible { get; set; }
        public string hovertext { get; set; }
    }

    public class Selection
    {
        public string ev_oc_id { get; set; }
        public string ev_mkt_id { get; set; }
        public string lp_num { get; set; }
        public string lp_den { get; set; }
        public string ilp_avail { get; set; }
        public string mkt_bir_index { get; set; }
        public string mkt_ew_avail { get; set; }
        public string mkt_ew_places { get; set; }
        public string mkt_ew_fac_num { get; set; }
        public string mkt_ew_fac_den { get; set; }
        public string mkt_hcap_value { get; set; }
        public string fb_result { get; set; }
        public string status { get; set; }
        public string result { get; set; }
        public string hcap_spread { get; set; }
        public string cs_home { get; set; }
        public string cs_away { get; set; }
        public string displayed { get; set; }
        public string raw_desc { get; set; }
        public string disporder { get; set; }
        public string name { get; set; }
    }

    public class RootObject
    {
        public Sport sport { get; set; }
        public Type type { get; set; }
        public string disporder { get; set; }
        public string @event { get; set; }
        public string event_link { get; set; }
        public string status { get; set; }
        public string raw_primary { get; set; }
        public string is_us_format { get; set; }
        public string start_time { get; set; }
        public string offset { get; set; }
        public string secs_to_start_time { get; set; }
        public string suspend_at { get; set; }
        public string has_video { get; set; }
        public string has_stats { get; set; }
        public string is_off { get; set; }
        public string is_running { get; set; }
        public string mkt_display_count { get; set; }
        public string name { get; set; }
        public string lang { get; set; }
        public string last_msg_id { get; set; }
        public string com_last_msg_id { get; set; }
        public string disp_perd_code { get; set; }
        public ComClock com_clock { get; set; }
        public ComScore com_score { get; set; }
        public List<Collection> collections { get; set; }
        public List<Market> markets { get; set; }
        public List<object> cast_markets { get; set; }
        public List<Selection> selections { get; set; }
    }
}
