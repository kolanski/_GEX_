using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ProjectGambUniverse
{

    [JsonObject(MemberSerialization.OptIn)]
    public struct GamesArr
    {
        [JsonProperty("Coefficent1")]
        public string Coefficent1 { get; set; }
        [JsonProperty("Coefficent2")]
        public string Coefficent2 { get; set; }
        [JsonProperty("GameNumber")]
        public string GameNumber { get; set; }
        [JsonProperty("SetNumber")]
        public string SetNumber { get; set; }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public struct Datum
    {
        [JsonProperty("Event")]
        public string Event { get; set; }
        [JsonProperty("GamePoints")]
        public string GamePoints { get; set; }
        [JsonProperty("GamesArr")]
        public IList<GamesArr> GamesArr { get; set; }
        [JsonProperty("Player1")]
        public string Player1 { get; set; }
        [JsonProperty("Player2")]
        public string Player2 { get; set; }
        [JsonProperty("ScoreAll")]
        public string ScoreAll { get; set; }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public struct Games
    {
        [JsonProperty("data")]
        public IList<Datum> data { get; set; }
    }
    class OlimpController
    {
        Quobject.SocketIoClientDotNet.Client.Socket parentsocket;
        RichTextBox parentrich; 

        public TennisGames BookmakerTennisGames;

        public OlimpController(Quobject.SocketIoClientDotNet.Client.Socket Sock, RichTextBox rich)
        {
            BookmakerTennisGames = new TennisGames();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Olimp;

            parentsocket = Sock;
            parentrich = rich;
            parentsocket.On("olimpMessage", (data) =>
            {

                BookmakerTennisGames.CleanData();
                Newtonsoft.Json.Linq.JObject obj;
                if (data.ToString() != "")
                {
                    obj = Newtonsoft.Json.Linq.JObject.Parse(data.ToString());
                    Datum[] objArr = JsonConvert.DeserializeObject<Datum[]>(obj["data"].ToString());
                    foreach (Datum myJsonObj in objArr)
                    {
                        BookmakerTennisGames.SetPlayers(myJsonObj.Player1, myJsonObj.Player2);
                        BookmakerTennisGames.SetGameData(myJsonObj.Event, myJsonObj.ScoreAll, myJsonObj.GamePoints);
                        foreach (GamesArr towingames in myJsonObj.GamesArr)
                        {
                            BookmakerTennisGames.AddGames(towingames.SetNumber, towingames.GameNumber, towingames.Coefficent1, towingames.Coefficent2);
                        }
                        BookmakerTennisGames.AddData();
                    }
                }
                BookmakerTennisGames.PrintGames1();
                this.parentrich.Invoke((MethodInvoker)delegate
                {
                    //  var m = JsonConvert.DeserializeObject<List<string>>(data);
                    this.parentrich.Text = data.ToString();
                }); /*Console.WriteLine(data.ToString()); */
            });
        }
        public void SendParse()
        {
            parentsocket.Emit("parse1");
        }
    }
}
