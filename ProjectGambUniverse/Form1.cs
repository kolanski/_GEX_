using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Quobject.Collections.Immutable;
using Quobject.EngineIoClientDotNet.Client.Transports;
using Quobject.EngineIoClientDotNet.Client;
using Quobject.EngineIoClientDotNet.ComponentEmitter;
using Quobject.EngineIoClientDotNet.Modules;
using Quobject.EngineIoClientDotNet.Parser;
using Quobject.SocketIoClientDotNet.Client;
using Socket = Quobject.SocketIoClientDotNet.Client.Socket;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace ProjectGambUniverse
{
    public partial class Form1 : Form
    {

        Socket socket;
        Socket socket1;
        SearchArbitrage search;
        BookMakerPattern Fonbet;
        BookMakerPattern Olimp;
        BookMakerPattern Marathon;
        BookMakerPattern Betcity;
        BookMakerPattern Parimatch;
        BookMakerPattern Williams;
        BookMakerPattern Bet365;
        BookMakerPattern Pinnacle;
        BookMakerPattern Titanbet;
        BookMakerPattern Sportingbet;
        BookMakerPattern WinLine;
        Timer tmrShow;
        bool automatic;
        bool reload;
        public Form1()
        {
            automatic = false;
            reload = false;
            InitializeComponent();
            search = new SearchArbitrage();
            socket = IO.Socket("http://127.0.0.1:3000");
            socket1 = IO.Socket("http://127.0.0.1:45000");
            socket.Connect();
            tmrShow = new Timer();
            tmrShow.Interval = 5000;
            tmrShow.Tick += tmrShow_Tick;
            tmrShow.Start();
            socket.On(Socket.EVENT_DISCONNECT, (data) =>
            {
                socket.Connect();
                Invoke((MethodInvoker)delegate
                {
                    richStatus.AppendText("Status evdisc:");
                    //  var m = JsonConvert.DeserializeObject<List<string>>(data);

                });
            });
            socket.On(Socket.EVENT_CONNECT, (data) =>
            {
                Invoke((MethodInvoker)delegate
                    { richStatus.AppendText("Status evconn:"); });
            });
            socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                Invoke((MethodInvoker)delegate
                    {
                        richStatus.AppendText("Status evconnerr:");
                    });
            });
            socket.On(Socket.EVENT_CONNECT_TIMEOUT, (data) =>
            {
                Invoke((MethodInvoker)delegate
{
    richStatus.AppendText("Status evconntimout:");
});
            });

            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                Invoke((MethodInvoker)delegate
{
    richStatus.AppendText("Status everr:");
});
            });
            socket.On(Socket.EVENT_RECONNECT, (data) =>
            {
                Invoke((MethodInvoker)delegate
{
    richStatus.AppendText("Status evcreconn:");
});
            });
            socket.On(Socket.EVENT_RECONNECT_ATTEMPT, (data) =>
            {
                Invoke((MethodInvoker)delegate
{
    richStatus.AppendText("Status evcreconnatt:");
});
            });
            socket.On(Socket.EVENT_RECONNECT_ERROR, (data) =>
            {
                Invoke((MethodInvoker)delegate
{
    richStatus.AppendText("Status evrecerr:");
});
            });
            socket.On(Socket.EVENT_RECONNECT_FAILED, (data) =>
            {
                Invoke((MethodInvoker)delegate
{
    richStatus.AppendText("Status evrecfail:");
});
            });
            socket.On(Socket.EVENT_RECONNECTING, (data) =>
            {
                Invoke((MethodInvoker)delegate
{
    richStatus.AppendText("Status ecreconnecting:");
});
            });
            socket1.On("reload1", () =>
            {
                richStatus.AppendText("SSUUUUCESS: want to reload");
                reloadall();
            });
            socket1.On("dumpdata", (data) => 
            {
                richStatus.AppendText("SSUUUUCESS: want to dump"+data);
                dumpto(data.ToString().Split('"')[5], TennisGames.Bookers.Pinnacle, TennisGames.Bookers.Bet365, TennisGames.Bookers.Marathon, TennisGames.Bookers.Williams, TennisGames.Bookers.Marathon,TennisGames.Bookers.Winline);
            });
            //dump
            //socket.On(Socket., (data) => { richStatus.AppendText("Status evconn:" + data.ToString()); });




            //socket.On(Socket.EVENT_RECONNECTING)
            Fonbet = new BookMakerPattern(socket, this.richTextBox1, "fonbetMessage", "parse", TennisGames.Bookers.Fonbet, "reload");
            Olimp = new BookMakerPattern(socket, this.richTextBox2, "olimpMessage", "parse1", TennisGames.Bookers.Olimp, "reload1");
            Marathon = new BookMakerPattern(socket, this.richTextBox3, "marathonMessage", "parse2", TennisGames.Bookers.Marathon, "reload2");
            Betcity = new BookMakerPattern(socket, this.richTextBox4, "betcityMessage", "parse3", TennisGames.Bookers.BetCity, "reload3");
            Parimatch = new BookMakerPattern(socket, this.richTextBox5, "parimatchMessage", "parse4", TennisGames.Bookers.PariMatch, "reload4");
            Williams = new BookMakerPattern(socket, this.richTextBox11, "williamsMessage", "parse5", TennisGames.Bookers.Williams, "reload5");
            Bet365 = new BookMakerPattern(socket, this.richTextBox12, "bet365Message", "parse6", TennisGames.Bookers.Bet365, "reload6");
            Pinnacle = new BookMakerPattern(socket, this.richTextBox13, "pinnacleMessage", "parse7", TennisGames.Bookers.Pinnacle, "reload7");
            Titanbet = new BookMakerPattern(socket, this.richTextBox14, "titanbetMessage", "parse8", TennisGames.Bookers.Titanbet, "reload8");
            Sportingbet = new BookMakerPattern(socket, this.richTextBox15, "sportingbetMessage", "parse9", TennisGames.Bookers.Sportingbet, "reload9");
            WinLine = new BookMakerPattern(socket, this.richTextBox16, "winlineMessage", "parse10", TennisGames.Bookers.Winline, "reload10");

        }
        public void dumpto(string username,params TennisGames.Bookers[] values)
        {
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var data=System.IO.Directory.CreateDirectory(desktopFolder+"\\dumps"+"\\"+username + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").Replace(":","").Replace(" ",""));
            foreach (var book in values)
            {
                createjsonwithpath(data.FullName, book, dumptext(book));
            }
        }
        public void createjsonwithpath(string path,TennisGames.Bookers book, string text)
        {
            if (text != null)
            {
                
                System.IO.File.WriteAllText(path + "/" + book.ToString() + ".json", text);
            }
        }
        public void createjson(TennisGames.Bookers book,string text)
        {
            if(text!=null)
            {
                var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                System.IO.File.WriteAllText(desktopFolder + "/" + book.ToString() + ".json", text);
            }
        }
        public string dumptext(TennisGames.Bookers book)
        {
            //fon rich1
            //ol rich2
            //mar rich3
            //betcity rich4
            //parimatch rich5
            //williams rich11
            //bet365 rich12
            //pinnacle rich13
            switch (book)
            {
                case TennisGames.Bookers.Pinnacle:
                    {
                        return richTextBox13.Text;
                        //break;
                    }

                case TennisGames.Bookers.Williams:
                    {
                        return richTextBox11.Text;
                        
                    }
                case TennisGames.Bookers.Bet365:
                    {
                        return richTextBox12.Text;
                        
                    }
                case TennisGames.Bookers.Marathon:
                    {
                        return richTextBox3.Text;
                        
                    }
                case TennisGames.Bookers.Winline:
                    {
                        return richTextBox16.Text;
                    }
                default:
                    return "";
            }

        }
        public void dump(TennisGames.Bookers book)
        {
            //fon rich1
            //ol rich2
            //mar rich3
            //betcity rich4
            //parimatch rich5
            //williams rich11
            //bet365 rich12
            //pinnacle rich13
            switch (book)
            {
                case TennisGames.Bookers.Pinnacle:
                    {
                        createjson(book, richTextBox13.Text);
                        break;
                    }
                case TennisGames.Bookers.Williams:
                    {
                        createjson(book, richTextBox11.Text);
                        break;
                    }
                case TennisGames.Bookers.Bet365:
                    {
                        createjson(book, richTextBox12.Text);
                        break;
                    }
                case TennisGames.Bookers.Marathon:
                    {
                        createjson(book, richTextBox3.Text);
                        break;
                    }
                case TennisGames.Bookers.Winline:
                    {
                        createjson(book, richTextBox16.Text);
                        break;
                    }
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            
        }
        private void tmrShow_Tick(object sender, EventArgs e)
        {
            richStatus.Text =
            "Fonbet.G " + Fonbet.GetTimeOut().ToString() + System.Environment.NewLine +
            "Olimp.Ge " + Olimp.GetTimeOut().ToString() + System.Environment.NewLine +
            "Marathon " + Marathon.GetTimeOut().ToString() + System.Environment.NewLine +
            "Parimatc " + Parimatch.GetTimeOut().ToString() + System.Environment.NewLine +
            "Betcity. " + Betcity.GetTimeOut().ToString() + System.Environment.NewLine +
            "Williams " + Williams.GetTimeOut().ToString() + System.Environment.NewLine +
            "Bet365 " + Bet365.GetTimeOut().ToString() + System.Environment.NewLine
            ;
            if (Fonbet.GetTimeOut() > 100 || Olimp.GetTimeOut() > 100 || Parimatch.GetTimeOut() > 100)
            {
                socket.Open();
                socket = new Socket(new Manager(), "");
                socket = IO.Socket("http://127.0.0.1:3000");
                socket.Connect();
                Fonbet.updateSocket(socket);
                Olimp.updateSocket(socket);
                Marathon.updateSocket(socket);
                Parimatch.updateSocket(socket);
                Betcity.updateSocket(socket);
                Williams.updateSocket(socket);
                Bet365.updateSocket(socket);
                Titanbet.updateSocket(socket);
                WinLine.updateSocket(socket);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Fonbet.SendParse();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Olimp.SendParse();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Marathon.SendParse();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Betcity.SendParse();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Parimatch.SendParse();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!checkBoxAnalyse.Checked)
                sendparseall();
            else
                analyseparse();
        }
        private string readfile(string name)
        {
            string filedata;
            using (StreamReader sr = new StreamReader(name + ".json"))
            {
                // Read the stream to a string, and write the string to the console.
                filedata = sr.ReadToEnd();
            }
            return filedata;
        }
        public List<string> getbooks()
        {
            var data = Directory.GetFiles(Directory.GetCurrentDirectory(), "*json");
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = data[i].Replace(Directory.GetCurrentDirectory() + "\\", "").Replace(".json", "");
            }
            return data.ToList();
        }

        private void analyseparse()
        {
            var books = getbooks();
            
            foreach (string book in books)
            {
                switch(book)
                {
                    case "Pinnacle":
                        {
                            Pinnacle.loadjson(readfile("Pinnacle"));
                            break;
                        }
                    case "Bet365":
                        {
                            Bet365.loadjson(readfile("Bet365"));
                            break;
                        }
                    case "Williams":
                        {
                            Williams.loadjson(readfile("Williams"));
                            break;
                        }
                    case "Marathon":
                        {
                            Marathon.loadjson(readfile("Marathon"));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        public void sendparseall()
        {
            Fonbet.SendParse();
            Marathon.SendParse();
            if (!reload)
            {
                Olimp.SendParse();
                Betcity.SendParse();
                Parimatch.SendParse();
                Williams.SendParse();
                Bet365.SendParse();
                Pinnacle.SendParse();
                Titanbet.SendParse();
                Sportingbet.SendParse();
                WinLine.SendParse();
            }
        }
        Stopwatch sw = new Stopwatch();
        Stopwatch sw1 = new Stopwatch();
        BooksData myd = new BooksData();
        GamesCache cach = new GamesCache();
        public void compareall()
        {
            //emit search start
            sw1.Restart();
            search.ClearTable();
            if (!oldProcess.Checked)
            {
                if (!isFonbet.Checked)
                    myd.Update(Fonbet.BookmakerTennisGames);
                if (!isOlimp.Checked)
                    myd.Update(Olimp.BookmakerTennisGames);
                if (!isMarathon.Checked)
                    myd.Update(Marathon.BookmakerTennisGames);
                if (!isBetcity.Checked)
                    myd.Update(Betcity.BookmakerTennisGames);
                if (!isParimatch.Checked)
                    myd.Update(Parimatch.BookmakerTennisGames);
                if (!isWilliams.Checked)
                    myd.Update(Williams.BookmakerTennisGames);
                if (!isBet365.Checked)
                    myd.Update(Bet365.BookmakerTennisGames);
                if (!isPinnacle.Checked)
                    myd.Update(Pinnacle.BookmakerTennisGames);

            }
            if (cach.getDataStatus())
            {
                cach.LoadBooks(myd);
            }
            if (!oldProcess.Checked)
            {
                cach.UpdateFound();
                cach.SearchAll();
            }
            this.label2.Text = sw1.ElapsedMilliseconds.ToString();
            sw.Restart();

            if (oldProcess.Checked)
            {
                search.CompareTennisGames(WinLine.BookmakerTennisGames, Pinnacle.BookmakerTennisGames);
                search.CompareTennisGames(WinLine.BookmakerTennisGames, Olimp.BookmakerTennisGames);
                search.CompareTennisGames(WinLine.BookmakerTennisGames, Marathon.BookmakerTennisGames);
                search.CompareTennisGames(WinLine.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                search.CompareTennisGames(WinLine.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                search.CompareTennisGames(WinLine.BookmakerTennisGames, Fonbet.BookmakerTennisGames);
                search.CompareTennisGames(Bet365.BookmakerTennisGames, WinLine.BookmakerTennisGames);
                search.CompareTennisGames(Titanbet.BookmakerTennisGames, WinLine.BookmakerTennisGames);
                search.CompareTennisGames(Sportingbet.BookmakerTennisGames, WinLine.BookmakerTennisGames);
                search.CompareTennisGames(Williams.BookmakerTennisGames, WinLine.BookmakerTennisGames);


                search.CompareTennisGames(Pinnacle.BookmakerTennisGames, Olimp.BookmakerTennisGames);
                search.CompareTennisGames(Pinnacle.BookmakerTennisGames, Marathon.BookmakerTennisGames);
                search.CompareTennisGames(Pinnacle.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                search.CompareTennisGames(Pinnacle.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                search.CompareTennisGames(Pinnacle.BookmakerTennisGames, Fonbet.BookmakerTennisGames);

                search.CompareTennisGames(Bet365.BookmakerTennisGames, Pinnacle.BookmakerTennisGames);

                search.CompareTennisGames(Titanbet.BookmakerTennisGames, Olimp.BookmakerTennisGames);
                search.CompareTennisGames(Titanbet.BookmakerTennisGames, Marathon.BookmakerTennisGames);
                search.CompareTennisGames(Titanbet.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                search.CompareTennisGames(Titanbet.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                search.CompareTennisGames(Williams.BookmakerTennisGames, Titanbet.BookmakerTennisGames);
                search.CompareTennisGames(Bet365.BookmakerTennisGames, Titanbet.BookmakerTennisGames);
                search.CompareTennisGames(Titanbet.BookmakerTennisGames, Pinnacle.BookmakerTennisGames);

                search.CompareTennisGames(Sportingbet.BookmakerTennisGames, Olimp.BookmakerTennisGames);
                search.CompareTennisGames(Sportingbet.BookmakerTennisGames, Marathon.BookmakerTennisGames);
                search.CompareTennisGames(Sportingbet.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                search.CompareTennisGames(Sportingbet.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                search.CompareTennisGames(Williams.BookmakerTennisGames, Sportingbet.BookmakerTennisGames);
                search.CompareTennisGames(Bet365.BookmakerTennisGames, Sportingbet.BookmakerTennisGames);
                search.CompareTennisGames(Sportingbet.BookmakerTennisGames, Pinnacle.BookmakerTennisGames);
                search.CompareTennisGames(Titanbet.BookmakerTennisGames, Sportingbet.BookmakerTennisGames);

                search.CompareTennisGames(Olimp.BookmakerTennisGames, WinLine.BookmakerTennisGames);
                search.CompareTennisGames(Fonbet.BookmakerTennisGames, WinLine.BookmakerTennisGames);
                if (!isFonbet.Checked && !isOlimp.Checked)
                    search.CompareTennisGames(Fonbet.BookmakerTennisGames, Olimp.BookmakerTennisGames);
                if (!isFonbet.Checked && !isMarathon.Checked)
                    search.CompareTennisGames(Fonbet.BookmakerTennisGames, Marathon.BookmakerTennisGames);
                if (!isFonbet.Checked && !isBetcity.Checked)
                    search.CompareTennisGames(Fonbet.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                if (!isFonbet.Checked && !isParimatch.Checked)
                    search.CompareTennisGames(Fonbet.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                if (!isWilliams.Checked && !isFonbet.Checked)
                    search.CompareTennisGames(Williams.BookmakerTennisGames, Fonbet.BookmakerTennisGames);
                if (!isBet365.Checked && !isFonbet.Checked)
                    search.CompareTennisGames(Bet365.BookmakerTennisGames, Fonbet.BookmakerTennisGames);

                if (!isOlimp.Checked && !isMarathon.Checked)
                    search.CompareTennisGames(Olimp.BookmakerTennisGames, Marathon.BookmakerTennisGames);
                if (!isOlimp.Checked && !isBetcity.Checked)
                    search.CompareTennisGames(Olimp.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                if (!isOlimp.Checked && !isParimatch.Checked)
                    search.CompareTennisGames(Olimp.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                if (!isWilliams.Checked && !isOlimp.Checked)
                    search.CompareTennisGames(Williams.BookmakerTennisGames, Olimp.BookmakerTennisGames);
                if (!isBet365.Checked && !isOlimp.Checked)
                    search.CompareTennisGames(Bet365.BookmakerTennisGames, Olimp.BookmakerTennisGames);

                if (!isMarathon.Checked && !isBetcity.Checked)
                    search.CompareTennisGames(Marathon.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                if (!isMarathon.Checked && !isParimatch.Checked)
                    search.CompareTennisGames(Marathon.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                if (!isWilliams.Checked && !isMarathon.Checked)
                    search.CompareTennisGames(Williams.BookmakerTennisGames, Marathon.BookmakerTennisGames);
                if (!isBet365.Checked && !isMarathon.Checked)
                    search.CompareTennisGames(Bet365.BookmakerTennisGames, Marathon.BookmakerTennisGames);

                if (!isWilliams.Checked && !isParimatch.Checked)
                    search.CompareTennisGames(Williams.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                if (!isBet365.Checked && !isParimatch.Checked)
                    search.CompareTennisGames(Bet365.BookmakerTennisGames, Parimatch.BookmakerTennisGames);
                if (!isWilliams.Checked && !isBetcity.Checked)
                    search.CompareTennisGames(Williams.BookmakerTennisGames, Betcity.BookmakerTennisGames);



                if (!isBet365.Checked && !isBetcity.Checked)
                    search.CompareTennisGames(Bet365.BookmakerTennisGames, Betcity.BookmakerTennisGames);
                if (!isBet365.Checked && !isWilliams.Checked)
                    search.CompareTennisGames(Bet365.BookmakerTennisGames, Williams.BookmakerTennisGames);
                search.CompareTennisGames(Williams.BookmakerTennisGames, Pinnacle.BookmakerTennisGames);
            }
            this.label1.Text = sw.ElapsedMilliseconds.ToString();
            //emit search ok

        }
        public void reloadall()
        {
            Olimp.SendReload();
            Betcity.SendReload();
            Parimatch.SendReload();
            Marathon.SendReload();
            Williams.SendReload();
            Bet365.SendReload();
            Pinnacle.SendReload();
            Titanbet.SendReload();
            Sportingbet.SendReload();
            WinLine.SendReload();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            compareall();
        }
        private void clearCache()
        {
            cach = new GamesCache();
            myd = new BooksData();
        }
        private async Task AutoUpdate()
        {
            int cnt = 0;
            while (automatic)
            {

                try
                {

                    sendparseall();
                    await Task.Delay(5000);
                    compareall();
                    cnt++;
                    if (cnt == 110)
                    {
                        reloadall();
                        cnt = 0;
                        await Task.Delay(20000);
                    }
                }
                catch
                {

                }
            }
        }
        public async Task setAutoupdate()
        {
            await Task.Delay(2000);
            checkBox1.Checked = true;
        }
        public void setcheck1()
        {
            checkBox1.Checked = true;
        }
        private async void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            automatic = checkBox1.Checked;
            await AutoUpdate();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Williams.SendParse();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            socket.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            socket.Open();
            socket = new Socket(new Manager(), "");
            socket = IO.Socket("http://127.0.0.1:3000");
            socket.Connect();
            Fonbet.updateSocket(socket);
            Olimp.updateSocket(socket);
            Marathon.updateSocket(socket);
            Parimatch.updateSocket(socket);
            Betcity.updateSocket(socket);
            Williams.updateSocket(socket);
            Bet365.updateSocket(socket);
            Pinnacle.updateSocket(socket);
            Sportingbet.updateSocket(socket);
            WinLine.updateSocket(socket);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Bet365.SendParse();
        }

        private void CacheButton_Click(object sender, EventArgs e)
        {
            cach = new GamesCache();
            myd = new BooksData();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == -1)
                richTextBox6.Text = cach.Print();
            else
                richTextBox6.Text = cach.Print((int)numericUpDown1.Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDown1.Value != -1)
            {
                label3.Text = Enum.GetName(typeof(TennisGames.Bookers), (int)numericUpDown1.Value).ToString();
            }
            else
            {
                label3.Text = "None";
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Pinnacle.SendParse();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Titanbet.SendParse();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Sportingbet.SendParse();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            WinLine.SendParse();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            dump(TennisGames.Bookers.Pinnacle);
        }
    }
}
