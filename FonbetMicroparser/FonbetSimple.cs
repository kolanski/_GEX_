using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroparserFramework;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Windows.Forms;
using System.Net;

namespace FonbetMicroparser
{
    class FonbetSimple
    {
        //https://line.fbwebdn.com/line/mobile/showEvents?lang=eng&lineType=live&skId=4
        string baseurl= "https://line.fbwebdn.com/line/mobile/showEvents?lang=eng&lineType=live&skId=4";
        HttpClient client = new HttpClient(
        new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip
                                     | DecompressionMethods.Deflate
        });

        public RichTextBox rich;
        public List<MicroparserFramework.Event> games = new List<MicroparserFramework.Event>();
        public async void getgames()
        {
            string data = await client.GetStringAsync(baseurl);
            JavaScriptSerializer serial = new JavaScriptSerializer();
            var listevents=serial.Deserialize<RootObject>(data);
            games.Clear();
            foreach(var ev in listevents.events)
            {
                if(ev.parentId==0&&ev.blocked==null)
                {
                    MicroparserFramework.Event eve = new MicroparserFramework.Event(ev.id.ToString(),ev.sportName.Replace("Tennis. ",""),new Team(ev.team1,"0"),new Team(ev.team2,"0"),ev.scoreComment.Substring(1, ev.scoreComment.Length-1).Replace("-"," "));
                    games.Add(eve);
                }
                else
                {
                    if(games.Count>0&&games[games.Count-1].EventId==ev.parentId.ToString())
                    {
                        if (ev.scoreComment != null&&!ev.scoreComment.Contains("(tiebreak"))
                        {
                            games[games.Count - 1].Team1.setScore(ev.scoreComment.Replace("(", "").Replace(")", "").Replace("*", "").Split('-')[0].Replace("00","0"));
                            games[games.Count - 1].Team2.setScore(ev.scoreComment.Replace("(", "").Replace(")", "").Replace("*", "").Split('-')[1].Replace("00","0"));
                        }
                        foreach(var field in ev.subcategories)
                        {
                            if(field.name=="Games"&&field.quotes.Count>0)
                            {
                                for(int i=0;i<field.quotes.Count;i+=3)
                                {
                                    games[games.Count - 1].addgame(new Game(ev.name[0].ToString(),field.quotes[i].name.Replace("Game ",""),field.quotes[i+1].quote, field.quotes[i + 2].quote));
                                }
                            }
                        }
                        //games[games.Count - 1].addgame(new Game());
                    }
                }
            }
            rich.Text = ext.renderJson(games)+"\n"+games.Count;
        }
    }
    public class Quote
    {
        public int factorId { get; set; }
        public string cartEventName { get; set; }
        public string cartQuoteName { get; set; }
        public string name { get; set; }
        public double value { get; set; }
        public string quote { get; set; }
        public int pValue { get; set; }
        public string p { get; set; }
        public bool? subtitle { get; set; }
        public bool? blocked { get; set; }
    }

    public class Subcategory
    {
        public int num { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public List<Quote> quotes { get; set; }
    }

    public class Event
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public int number { get; set; }
        public string startTime { get; set; }
        public int startTimeTimestamp { get; set; }
        public int sportId { get; set; }
        public string sportName { get; set; }
        public int skId { get; set; }
        public string skName { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string name { get; set; }
        public int priority { get; set; }
        public string score { get; set; }
        public string scoreComment { get; set; }
        public List<int> translationProviders { get; set; }
        public List<Subcategory> subcategories { get; set; }
        public bool? blocked { get; set; }
    }

    public class RootObject
    {
        public string request { get; set; }
        public string lineType { get; set; }
        public List<Event> events { get; set; }
    }
}
