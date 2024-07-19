using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGambUniverse
{
    class MarathonController
    {
        Quobject.SocketIoClientDotNet.Client.Socket parentsocket;
        RichTextBox parentrich;
        public TennisGames BookmakerTennisGames;
        public MarathonController(Quobject.SocketIoClientDotNet.Client.Socket Sock, RichTextBox rich)
        {
            BookmakerTennisGames = new TennisGames();
            this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Marathon;
            parentsocket = Sock;
            parentrich = rich;
            parentsocket.On("marathonMessage", (data) =>
            {
                BookmakerTennisGames.CleanData();
                Newtonsoft.Json.Linq.JObject obj;
                if (data.ToString() != "")
                {
                    obj = Newtonsoft.Json.Linq.JObject.Parse(data.ToString());
                    Datum[] objArr = JsonConvert.DeserializeObject<Datum[]>(obj["data"].ToString());
                    if (objArr!=null)
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
            parentsocket.Emit("parse2");
        }
    }
}
