using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace ProjectGamb
{
    class SearchArbitrage
    {
        MySqlConnection conn;
        public SearchArbitrage()
        {
            string connStr = "server=localhost;user=root;database=bets;port=3306;password=ashjdjasjdkamksdk;";
            conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Done.");
        }
        ~SearchArbitrage()
        {
            conn.Close();
        }
        public void ClearTable()
        {
            string Query = "TRUNCATE TABLE arbitrage;";
            MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
            MySqlDataReader MyReader;
            MyReader = MyCommand2.ExecuteReader();
            while (MyReader.Read())
            {
            }
            MyReader.Close();
        }
        public void InsertInTable(string Player1,string Player2,double Coefficent1,double Coefficent2,string Bookmaker1,string Bookmaker2,string Numgame,string Score,string Scoreset)
        {
            string Query = "insert into Arbitrage(Player1,Player2,Coefficent1,Coefficent2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame) values('" + Player1 + "','" + Player2 + "','" +((float) Coefficent1).ToString().Replace(",",".") + "','" + ((float) Coefficent2).ToString().Replace(",",".") + "','" + Bookmaker1 + "','" + Bookmaker2 + "','" + Numgame + "','" + Score + "','" + Scoreset + "');";
            MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
            MySqlDataReader MyReader;
            MyReader = MyCommand2.ExecuteReader();
            while (MyReader.Read())
            {
            }
            MyReader.Close();
        }

        public string Log;
        public void LoadTennisData(TennisGames ToLoad)
        {

        }

        private int ConvertGames(TennisGames.Games game, TennisGames.Players Player)
        {
            int gameCounter = 0;
            if (game.SetNum != 0)
            {
                switch (game.SetNum)
                {
                    case 1:
                        {
                            gameCounter = game.GameNum;
                            break;
                        }
                    case 2:
                        {
                            try
                            {
                                gameCounter = (int.Parse(Player.Score[0].ToString()) + int.Parse(Player.Score[2].ToString())) + game.GameNum;
                                break;
                            }
                            catch
                            {
                                return game.GameNum;
                            }
                        }
                    case 3:
                        {
                            gameCounter = int.Parse(Player.Score[0].ToString()) + int.Parse(Player.Score[2].ToString()) + int.Parse(Player.Score[4].ToString()) + int.Parse(Player.Score[6].ToString()) + game.GameNum;
                            break;
                        }
                }
            }
            else
            {
                if (game.SetNum == 0)
                {
                    gameCounter = game.GameNum;
                }
            }
            return gameCounter;
        }

        private void AddLog(string toadd)
        {
            Log += toadd + System.Environment.NewLine;
        }

        private string GetBookamerName(TennisGames.Bookers CurrentBooker)
        {
           return Enum.GetName(typeof(TennisGames.Bookers), CurrentBooker);
        }

        public void CompareTennisGames(TennisGames Book1, TennisGames Book2)
        {
            int countcomp = 0;
            Log = "";
            TennisGames BookHigh;
            TennisGames BookLow;

            WordsMatching.MatchsMaker match;
            WordsMatching.MatchsMaker match1;

            if (Book1.TennisData.Count > Book2.TennisData.Count)
            {
                BookHigh = Book1;
                BookLow = Book2;
            }
            else
            {
                BookHigh = Book2;
                BookLow = Book1;
            }
            try
            {

                for (int i = 0; i < BookHigh.TennisData.Count; i++)
                {
                    for (int j = 0; j < BookLow.TennisData.Count; j++)
                    {
                        match = new WordsMatching.MatchsMaker(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player1);
                        match1 = new WordsMatching.MatchsMaker(BookHigh.TennisData[i].TennisPlayers.Player2, BookLow.TennisData[j].TennisPlayers.Player2);
                        if ((match.Score + match1.Score) > 1.1)
                        {
                            countcomp++;
                            //AddLog(BookHigh.TennisData[i].TennisPlayers.Player1 + " " + BookLow.TennisData[j].TennisPlayers.Player2);
                            for (int u = 0; u < BookHigh.TennisData[i].TennisGameCoefs.Count; u++)
                            {
                                for (int h = 0; h < BookLow.TennisData[j].TennisGameCoefs.Count; h++)
                                {
                                    double koef11 = BookHigh.TennisData[i].TennisGameCoefs[u].Coefficent1;
                                    double koef21 = BookLow.TennisData[j].TennisGameCoefs[h].Coefficent1;
                                    double koef12 = BookHigh.TennisData[i].TennisGameCoefs[u].Coefficent2;
                                    double koef22 = BookLow.TennisData[j].TennisGameCoefs[h].Coefficent2;
                                    if (BookHigh.TennisData[i].TennisPlayers.Score == "" && BookLow.TennisData[j].TennisPlayers.Score != "")
                                    {
                                        var toscore = BookHigh.TennisData[i];
                                        toscore.TennisPlayers.Score = BookLow.TennisData[j].TennisPlayers.Score;
                                        BookHigh.TennisData[i] = toscore;
                                    }
                                    else
                                    {
                                        if (BookHigh.TennisData[i].TennisPlayers.Score != "" && BookLow.TennisData[j].TennisPlayers.Score == "")
                                        {
                                            var toscore = BookLow.TennisData[j];
                                            toscore.TennisPlayers.Score = BookHigh.TennisData[i].TennisPlayers.Score;
                                            BookLow.TennisData[j] = toscore;
                                        }
                                    }
                                    if (ConvertGames(BookHigh.TennisData[i].TennisGameCoefs[u], BookHigh.TennisData[i].TennisPlayers) == ConvertGames(BookLow.TennisData[j].TennisGameCoefs[h], BookLow.TennisData[j].TennisPlayers))
                                    {
                                        if (koef11 > koef21)
                                        {
                                            if ((1 / koef11 + 1 / koef22) < 1)
                                            {
                                                AddLog(Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker) + " Player1:" + BookHigh.TennisData[i].TennisPlayers.Player1 + Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker) + "Player2:" + BookLow.TennisData[j].TennisPlayers.Player2);
                                                AddLog("Sum:" + (1 / koef11 + 1 / koef22).ToString());
                                                AddLog("Numgam:" + BookLow.TennisData[j].TennisGameCoefs[h].GameNum);
                                                AddLog("Pl1:" + koef22 + " Pl2:" + koef11);
                                                if (koef11 > koef22)
                                                {

                                                    AddLog("White bet.");
                                                    AddLog("1000 on pl1 +" + (1000 * koef22) / koef11 + " on pl2");
                                                    AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (1000 * koef22) / koef11)));
                                                    AddLog("Blue bet.");
                                                    AddLog("1000 on pl1 +" + (-1000 / (1 - koef11)) + " on pl2");
                                                    AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (-1000 / (1 - koef11)))));
                                                    AddLog(System.Environment.NewLine);
                                                    InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef22, koef11, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                }
                                                else
                                                {


                                                    AddLog("White bet.");
                                                    AddLog("1000 on pl2 +" + (1000 * koef11) / koef22 + " on pl1");
                                                    AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (1000 * koef11) / koef22)));
                                                    AddLog("Blue bet.");
                                                    AddLog("1000 on pl2 +" + (-1000 / (1 - koef22)) + " on pl1");
                                                    AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (-1000 / (1 - koef22)))));
                                                    AddLog(System.Environment.NewLine);
                                                    InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef22, koef11, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);

                                                }
                                            }
                                        }
                                    }

                                }

                            }

                        }

                    }
                }
                AddLog("Compares " + GetBookamerName(BookHigh.CurrentBooker) + " " + GetBookamerName(BookLow.CurrentBooker)+" " + countcomp + " games");
            }
            catch (Exception e)
            {
                AddLog(e.ToString());
            }
        }
    }


    class PlayersDraw
    {
        string Player1;
        string Player2;
        string ScoreAll;
        String ScoreSet;
        List<int> ListOfBookmakers = new List<int>(10);

    }
}

