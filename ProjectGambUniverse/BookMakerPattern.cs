using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGambUniverse
{
    class BookMakerPattern
    {
        Stopwatch sw = new Stopwatch();
        Quobject.SocketIoClientDotNet.Client.Socket parentsocket;
        RichTextBox parentrich;
        //Datum[] objArr;
        public TennisGames BookmakerTennisGames;
        private string parsemessage,sendparsemessage,reloadmessage;
        public BookMakerPattern(Quobject.SocketIoClientDotNet.Client.Socket Socket, RichTextBox Rich, string parsemes, string sendparsemes,TennisGames.Bookers book,string rel)
        {
            sw.Start();
            BookmakerTennisGames = new TennisGames();
            this.BookmakerTennisGames.CurrentBooker = book;
            parentsocket = Socket;
            parentrich = Rich;
            parsemessage = parsemes;
            sendparsemessage = sendparsemes;
            reloadmessage = rel;
            parentsocket.On(parsemessage, (data) =>
            {
                sw.Restart();
                BookmakerTennisGames.CleanData();
                Newtonsoft.Json.Linq.JObject obj;
                if (data.ToString() != "")
                {
                    obj = Newtonsoft.Json.Linq.JObject.Parse(data.ToString());
                    Datum[] objArr=null;
                    if(!obj["data"].ToString().Contains("null"))
                      objArr=  JsonConvert.DeserializeObject<Datum[]>(obj["data"].ToString());
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
            //lerngth exeed limit  message
            parentsocket.Emit(sendparsemessage);
        }
        public void SendReload()
        {
            parentsocket.Emit(reloadmessage);
        }
        public void updateSocket(Quobject.SocketIoClientDotNet.Client.Socket Socket)
        {
            try
            {
                parentsocket = Socket;
                parentsocket.On(parsemessage, (data) =>
                {
                    sw.Restart();
                    BookmakerTennisGames.CleanData();
                    Newtonsoft.Json.Linq.JObject obj;
                    if (data.ToString() != ""&&data.ToString().Contains("["))
                    {
                        obj = Newtonsoft.Json.Linq.JObject.Parse(data.ToString());
                        Datum[] objArr = null;
                        if (!obj["data"].ToString().Contains("null"))
                            objArr = JsonConvert.DeserializeObject<Datum[]>(obj["data"].ToString());
                        if (objArr != null)
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
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void loadjson(string data)
        {
            Newtonsoft.Json.Linq.JObject obj;
            if (data.ToString() != "")
            {
                obj = Newtonsoft.Json.Linq.JObject.Parse(data.ToString());
                Datum[] objArr = null;
                if (!obj["data"].ToString().Contains("null"))
                    objArr = JsonConvert.DeserializeObject<Datum[]>(obj["data"].ToString());
                if (objArr != null)
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
        }
        public float GetTimeOut()
        {
            return (float)sw.ElapsedMilliseconds / 1000f;
        }
    }
}
