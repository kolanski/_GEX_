using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroparserFramework;
using System.Windows.Forms;
using PinnacleWrapper;
using PinnacleWrapper.Data;
using System.Text.RegularExpressions;
using System.Net;

namespace PinnacleMicroparser
{
    class PinacleSimple
    {
        MicroparserFramework.microserver server;
        PinnacleWrapper.PinnacleClient client;
        
        //GetFixturesResponse response;
        //List<MicroparserFramework.Event> eventsList;
        public List<int> leagueids = new List<int>();
        List<string> torender = new List<string>();
        List<eventdata> todata = new List<eventdata>();
        public List<MicroparserFramework.Event> games = new List<MicroparserFramework.Event>();
        public RichTextBox rich=null;
        PinnacleApi.PinnacleApi pin;
        public PinacleSimple(RichTextBox rich)
        {
            this.rich = rich;
            server = new microserver(rich);
            server.setActions(LoadOdds2, LoadMatches);
            server.init("30085");
            //string proxyURI = "http://200.255.220.211:8080";
            //WebRequest.DefaultWebProxy = new System.Net.WebProxy(proxyURI);
            //client = new PinnacleClient("KK90000", "sssadsa!", "USD", PinnacleWrapper.Enums.OddsFormat.DECIMAL);
            //api test
            System.IO.StreamReader file =   new System.IO.StreamReader("login.txt");
            string name = file.ReadLine();
            string pass = file.ReadLine();

            file.Close();
            pin = new PinnacleApi.PinnacleApi(/*"MS991231", "asdasddd!",*/name,pass, "USD");
            //var ss= pin.GetClientBalance();
            //rich.AppendText(ss.Result.AvailableBalance.ToString());
        }

        public void getTennisFixtures()
        {
            PinnacleApi.Odds odds;
            odds = pin.GetOddsForTennis().Result;
            if (odds != null)
                foreach (var sp in odds.leagues)
                {
                    string joinedevents = string.Join(System.Environment.NewLine, sp.events);
                    rich.AppendText(sp.id + " " + joinedevents + System.Environment.NewLine);
                }
            else
                rich.AppendText("No live tennis events");
        }
        public async void LoadMatches()
        {
            try
            {
                leagueids = new List<int>();
                todata = new List<eventdata>();
                //var x = await client.GetSports();
                var response = await pin.GetFixturesForTennis();
                foreach (var result in response.League)
                {
                    foreach (var _event in result.Events)
                    {
                        if (_event.LiveStatus == 1 &&_event.Status !="H"&& _event.Home.Contains("Game")&&_event.ParlayRestriction==1)
                        { //richTextBox1.AppendText(_event.Home+" "+_event.LiveStatus + "\n");
                            //torender.Add(_event.Home + " " + _event.Away + " " + _event.LiveStatus);
                            todata.Add(new eventdata(result.Id, _event.Id, _event.Home + "|" + _event.Away + "|" + _event.LiveStatus + "|" + _event.ParlayRestriction));
                            if (!leagueids.Contains(result.Id))
                                leagueids.Add(result.Id);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if(rich!=null)
                rich.Text = ex.Message;
            }
        }
        public void getSports()
        {
            PinnacleApi.Sports sports;
            sports = pin.GetSports().Result;

            foreach(var sp in sports.sports)
            {
                rich.AppendText(sp.id + " " + sp.name+System.Environment.NewLine);
            }
        }
        public void getTennisOdds()
        {
            PinnacleApi.Odds odds;
            odds = pin.GetOddsForTennis().Result;
            if (odds != null)
                foreach (var sp in odds.leagues)
                {
                    string joinedevents = string.Join(System.Environment.NewLine, sp.events);
                    rich.AppendText(sp.id + " " + joinedevents + System.Environment.NewLine);
                }
            else
                rich.AppendText("No live tennis events");
        }
        //33 is tennis
        int counter = 0;
        int delayer = 0;
        public async void LoadOdds2()
        {
            counter++;
            //ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
            var response1 = await pin.GetOddsForTennis();

            if (response1 != null && counter < 4)
            {
                games.Clear();
                foreach (var data in response1.leagues)
                {
                    foreach (var _event in data.events)
                    {
                        //richTextBox1.AppendText(_event.Periods[0].MoneyLine.Away+" "+ _event.Periods[0].MoneyLine.Home+"\n");
                        if (_event.periods[0].moneyline != null)
                        {
                            //torender[i++] += " " + _event.Periods[0].MoneyLine.Away + " " + _event.Periods[0].MoneyLine.Home;

                            if (todata.Find((s) => s.selfid == _event.id) != null)
                            {
                                todata.First(s => s.selfid == _event.id).data += " " + _event.periods[0].moneyline.away + " " + _event.periods[0].moneyline.home;
                                if (games.Count == 0)
                                {
                                    var players = todata.Find(s => s.selfid == _event.id).data.Split('|');
                                    var player1 = players[0].Substring(0, players[0].IndexOf("Game"));
                                    var player2 = players[1].Substring(0, players[1].IndexOf("Game"));

                                    MicroparserFramework.Event ev = new MicroparserFramework.Event("", "", new MicroparserFramework.Team(player1, "0"), new MicroparserFramework.Team(player2, "0"), "");
                                    if (_event.periods[0].moneyline.away != 0)
                                    {
                                        Regex reg = new Regex(@"\d{2}|\d", RegexOptions.IgnoreCase);
                                        var parsedgame = reg.Matches(players[0]);
                                        ev.addgame(new Game(parsedgame[1].Value, parsedgame[0].Value, _event.periods[0].moneyline.home.ToString().Replace(",", "."), _event.periods[0].moneyline.away.ToString().Replace(",", ".")));
                                    }
                                    games.Add(ev);
                                }
                                else
                                if (games.Count > 0)
                                {
                                    var players1 = todata.Find(s => s.selfid == _event.id).data.Split('|');
                                    var player11 = players1[0].Substring(0, players1[0].IndexOf("Game"));
                                    var player21 = players1[1].Substring(0, players1[1].IndexOf("Game"));
                                    if (games[games.Count - 1].Team1.getName() != player11)
                                    {
                                        MicroparserFramework.Event ev = new MicroparserFramework.Event("", "", new MicroparserFramework.Team(player11, "0"), new MicroparserFramework.Team(player21, "0"), "");
                                        Regex reg = new Regex(@"\d{2}|\d", RegexOptions.IgnoreCase);
                                        var parsedgame = reg.Matches(players1[0]);
                                        ev.addgame(new Game(parsedgame[1].Value, parsedgame[0].Value, _event.periods[0].moneyline.home.ToString().Replace(",", "."), _event.periods[0].moneyline.away.ToString().Replace(",", ".")));
                                        games.Add(ev);
                                    }
                                    else
                                    {
                                        Regex reg = new Regex(@"\d{2}|\d", RegexOptions.IgnoreCase);
                                        var parsedgame = reg.Matches(players1[0]);
                                        games[games.Count - 1].addgame(new Game(parsedgame[1].Value, parsedgame[0].Value, _event.periods[0].moneyline.home.ToString().Replace(",", "."), _event.periods[0].moneyline.away.ToString().Replace(",", ".")));
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            if (counter == 4)
            {
                LoadMatches();
                counter = 0;
            }
            if (games.Count > 0)
                server.events = games;
        }
        public async void LoadOdds()
        {

            counter++;
            delayer++;
            if (counter > 4)
            {
                LoadMatches();
                counter = 0;
                //return;
            }
            if (delayer == 2)
            {
                delayer = 0;
                GetOddsResponse response1 = await client.GetOdds(new GetOddsRequest(33, leagueids, 1, true));
                int i = 0;
                games.Clear();
                foreach (var data in response1.Leagues)
                {
                    foreach (var _event in data.Events)
                    {
                        //richTextBox1.AppendText(_event.Periods[0].MoneyLine.Away+" "+ _event.Periods[0].MoneyLine.Home+"\n");
                        if (_event.Periods[0].MoneyLine != null)
                        {
                            //torender[i++] += " " + _event.Periods[0].MoneyLine.Away + " " + _event.Periods[0].MoneyLine.Home;

                            if (todata.Find((s) => s.selfid == _event.Id) != null)
                            {
                                todata.First(s => s.selfid == _event.Id).data += " " + _event.Periods[0].MoneyLine.Away + " " + _event.Periods[0].MoneyLine.Home;
                                if (games.Count == 0)
                                {
                                    var players = todata.Find(s => s.selfid == _event.Id).data.Split('|');
                                    var player1 = players[0].Substring(0, players[0].IndexOf("Game"));
                                    var player2 = players[1].Substring(0, players[1].IndexOf("Game"));

                                    MicroparserFramework.Event ev = new MicroparserFramework.Event("", "", new MicroparserFramework.Team(player1, "0"), new MicroparserFramework.Team(player2, "0"), "");
                                    if (_event.Periods[0].MoneyLine.Away != 0)
                                    {
                                        Regex reg = new Regex(@"\d{2}|\d", RegexOptions.IgnoreCase);
                                        var parsedgame = reg.Matches(players[0]);
                                        ev.addgame(new Game(parsedgame[1].Value, parsedgame[0].Value, _event.Periods[0].MoneyLine.Home.ToString().Replace(",", "."), _event.Periods[0].MoneyLine.Away.ToString().Replace(",", ".")));
                                    }
                                    games.Add(ev);
                                }
                                else
                                if (games.Count > 0)
                                {
                                    var players1 = todata.Find(s => s.selfid == _event.Id).data.Split('|');
                                    var player11 = players1[0].Substring(0, players1[0].IndexOf("Game"));
                                    var player21 = players1[1].Substring(0, players1[1].IndexOf("Game"));
                                    if (games[games.Count - 1].Team1.getName() != player11)
                                    {
                                        MicroparserFramework.Event ev = new MicroparserFramework.Event("", "", new MicroparserFramework.Team(player11, "0"), new MicroparserFramework.Team(player21, "0"), "");
                                        Regex reg = new Regex(@"\d{2}|\d", RegexOptions.IgnoreCase);
                                        var parsedgame = reg.Matches(players1[0]);
                                        ev.addgame(new Game(parsedgame[1].Value, parsedgame[0].Value, _event.Periods[0].MoneyLine.Home.ToString().Replace(",", "."), _event.Periods[0].MoneyLine.Away.ToString().Replace(",", ".")));
                                        games.Add(ev);
                                    }
                                    else
                                    {
                                        Regex reg = new Regex(@"\d{2}|\d", RegexOptions.IgnoreCase);
                                        var parsedgame = reg.Matches(players1[0]);
                                        games[games.Count - 1].addgame(new Game(parsedgame[1].Value, parsedgame[0].Value, _event.Periods[0].MoneyLine.Home.ToString().Replace(",", "."), _event.Periods[0].MoneyLine.Away.ToString().Replace(",", ".")));
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                if(games.Count>0)
                server.events = games;
            }
        }

    }
}