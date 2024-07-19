using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProjectGambUniverse
{
    class SearchArbitrage
    {
        public static MySqlConnection conn;
        public static string connStr = "server=localhost;user=root;database=bets;port=3306;password=aksjdkaksdjka;";
        public SearchArbitrage()
        {

            conn = new MySqlConnection(connStr);
            try
            {
                Debug.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            Debug.WriteLine("Done.");
        }

        ~SearchArbitrage()
        {
            conn.Close();
        }

        public void ClearTable()
        {
            string Query = "TRUNCATE TABLE arbitrage;";
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                MySqlCommand MyCommand2 = new MySqlCommand(Query, connection);
                MyCommand2.ExecuteNonQuery();
            }

            //string Query = "TRUNCATE TABLE arbitrage;";
            //executequerry(Query);
            /* using (MySqlConnection c = conn)
             {

                 MySqlCommand MyCommand2 = new MySqlCommand(Query, c);
                 MySqlDataReader MyReader;
                 MyReader = MyCommand2.ExecuteReader();
                 while (MyReader.Read())
                 {
                 }
                 MyReader.Close();
             }*/
        }
        private static void WaitForEndOfRead()
        {
            while (conn.State == System.Data.ConnectionState.Fetching)
            {
            }
        }
        public static void ClearTable(string table)
        {
            try
            {
                string Query = "TRUNCATE TABLE " + table + ";";
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, connection);
                    MyCommand2.ExecuteNonQuery();
                }
            }
            catch (MySqlException ee)
            {
                var create = @"CREATE TABLE `" + table + @"` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
";
                if (ee.Message.Contains("exist"))
                {

                    using (var connection = new MySqlConnection(connStr))
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand MyCommand2 = new MySqlCommand("DROP TABLE `bets`.`" + table + "`;'", connection);
                            MyCommand2.ExecuteNonQuery();
                        }
                        catch
                        {

                        }
                    }

                    using (var connection = new MySqlConnection(connStr))
                    {
                        connection.Open();
                        MySqlCommand MyCommand2 = new MySqlCommand(create, connection);
                        MyCommand2.ExecuteNonQuery();
                    }
                }
            }
            // MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
            //   MySqlDataReader MyReader;
            //   MyReader = MyCommand2.ExecuteReader();
            /* while (MyReader.Read())
             {

             }*/
            //    MyReader.Close();
            //string Query = "TRUNCATE TABLE " + table + ";";
            //executequerry(Query);
            /*WaitForEndOfRead();
            if(conn.State==System.Data.ConnectionState.Open)
            using (MySqlCommand command = new MySqlCommand(Query, conn))
            {
                
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                    }
                    dr.Close();
                }
            }
            */
        }
        public static void executequerry(string querry)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;database=bets;port=3306;password=askjdkkajsdkjaksd;"))
            {
                MySqlCommand command = new MySqlCommand(querry, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

            }
        }

        public static void InsertInTable(string Player1, string Player2, double Coefficent1, double Coefficent2, string Bookmaker1, string Bookmaker2, string Numgame, string Score, string Scoreset)
        {
            string Query = "insert into Arbitrage(Player1,Player2,Coefficent1,Coefficent2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame) values('" + Player1.Replace("'", "") + "','" + Player2.Replace("'", "") + "','" + ((float)Coefficent1).ToString().Replace(",", ".") + "','" + ((float)Coefficent2).ToString().Replace(",", ".") + "','" + Bookmaker1 + "','" + Bookmaker2 + "','" + Numgame + "','" + Score + "','" + Scoreset + "');";
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                MySqlCommand MyCommand2 = new MySqlCommand(Query, connection);
                MyCommand2.ExecuteNonQuery();
            }
            // string Query = "insert into Arbitrage(Player1,Player2,Coefficent1,Coefficent2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame) values('" + Player1.Replace("'", "") + "','" + Player2.Replace("'", "") + "','" + ((float)Coefficent1).ToString().Replace(",", ".") + "','" + ((float)Coefficent2).ToString().Replace(",", ".") + "','" + Bookmaker1 + "','" + Bookmaker2 + "','" + Numgame + "','" + Score + "','" + Scoreset + "');";
            /*MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
            MySqlDataReader MyReader;
            MyReader = MyCommand2.ExecuteReader();
            while (MyReader.Read())
            {
            }
            MyReader.Close();*/
            // executequerry(Query);
        }

        public static void InsertInTable(string Player1, string Player2, double Coefficent1, double Coefficent2, string Bookmaker1, string Bookmaker2, string Numgame, string Score, string Scoreset, string Table, string NumSet)
        {
            try
            {
                if (Score == null)
                    Score = "";
                if (Scoreset == null)
                    Scoreset = "";
                string Query = "insert into " + Table + "(Player1,Player2,Coefficent1,Coefficent2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,NumSet) values('" + Player1.Replace("'", "") + "','" + Player2.Replace("'", "") + "','" + ((float)Coefficent1).ToString().Replace(",", ".") + "','" + ((float)Coefficent2).ToString().Replace(",", ".") + "','" + Bookmaker1 + "','" + Bookmaker2 + "','" + Numgame + "','" + Score + "','" + Scoreset + "','" + NumSet + "');";
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, connection);
                    MyCommand2.ExecuteNonQuery();
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message, "Системное сообщение");
                //Application.Restart();
            }
            //string Query = "insert into " + Table + "(Player1,Player2,Coefficent1,Coefficent2,Bookmaker1,Bookmaker2,NumGame,Score,ScoreGame,NumSet) values('" + Player1.Replace("'", "") + "','" + Player2.Replace("'", "") + "','" + ((float)Coefficent1).ToString().Replace(",", ".") + "','" + ((float)Coefficent2).ToString().Replace(",", ".") + "','" + Bookmaker1 + "','" + Bookmaker2 + "','" + Numgame + "','" + Score + "','" + Scoreset + "','" + NumSet + "');";
            /* MySqlCommand MyCommand2 = new MySqlCommand(Query, conn);
             MySqlDataReader MyReader;

             MyReader = MyCommand2.ExecuteReader();
             while (MyReader.Read())
             {
             }
             MyReader.Close();*/
            //executequerry(Query);
        }

        //public string Log;
        public void LoadTennisData(TennisGames ToLoad)
        {

        }
        private string CutMarathon(string player)
        {
            //Matviyenko, Lisa
            string tocut = player;
            tocut = tocut.Trim();
            if (tocut.Contains("/") || tocut.Length < 2)
            {
                return tocut;
            }
            else
            {
                if (tocut.IndexOf(", ") != -1)
                    tocut = tocut.Trim().Substring(0, tocut.LastIndexOf(",") + 3);
                else
                {
                    tocut = tocut.Trim().Substring(0, tocut.LastIndexOf(" ") + 2);

                }
            }
            return tocut.Replace(",", " ");
        }
        private string CutBet365Player(string player)
        {
            string tocut = player;
            if (tocut.Contains("/") || tocut.Length < 2)
            {
                return tocut;
            }
            else
            {
                tocut = tocut.Trim().Remove(1, tocut.Trim().IndexOf(" ") - 1);
            }
            return tocut;
        }
        private string CutWilliams(string player)
        {
            //Daria Gavrilova/Daria Kasatkina v Gabriela Dabrowski/Maria-Jose Martinez Sanchez
            string tocut = player;
            if (tocut.Contains("/")&&!tocut.Contains("-"))
            {
                /*
                var cutted = tocut.Remove(0, tocut.IndexOf(" ")+1);
                var cutted2 = cutted.Remove(cutted.IndexOf("/"),cutted.LastIndexOf(" "));
                */
                var cutted = tocut.Replace("/"," ").Trim().Split(' ');
                if(cutted.Length==4)
                {
                    tocut = cutted[1] + "/" + cutted[3];
                }
                return tocut;
            }
            if(tocut.Contains("-"))
            {
                var cutted = tocut.Replace("/", " ").Trim().Split(' ');
                bool name = true;
                bool endname = false;
                var tmp = "";
                for(int i=0;i<cutted.Length;i++)
                {
                    if(!cutted[i].Contains("-")&&!name)
                    {
                        tmp += cutted[i];
                        if(!endname)
                        name = !name;
                    }
                    else if(cutted[i].Contains("-"))
                    {
                        endname = true;
                        name = false;
                    }
                }
                return tocut;
            }
            else
                return tocut;
        }
        public bool IsNumber(char text)                                            //Проверка на то что это число
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text.ToString());
        }
        private int SumAllDigits(string InnString)
        {
            int ret = 0;

            for (int i = 0; i < InnString.Length; i++)
            {
                int n;
                bool isNumeric = int.TryParse(InnString[i].ToString(), out n);
                if (isNumeric)
                {
                    ret += int.Parse(InnString[i].ToString());
                }
            }
            return ret;
        }
        public static int SumAllDigits(string InnString, int count)
        {
            int ret = 0;
            int cnt = 0;
            for (int i = 0; i < InnString.Length; i++)
            {
                int n;
                bool isNumeric = int.TryParse(InnString[i].ToString(), out n);
                if (isNumeric)
                {
                    if (count > cnt)
                        ret += int.Parse(InnString[i].ToString());
                    cnt++;
                }
            }
            return ret;
        }
        public static int ConvertGames(TennisGames.Games game, TennisGames.Players Player)
        {
            int gameCounter = 0;
            if (Player.Score != null && Player.Score.ToString().Contains(","))
                Player.Score = Player.Score.Replace(",", "");
            if (Player.Score != null && game.SetNum != 0)
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

                                gameCounter = SumAllDigits(Player.Score, 2) + game.GameNum;
                                break;
                            }
                            catch
                            {
                                return game.GameNum;
                            }
                        }
                    case 3:
                        {
                            if (Player.Score.Length >= 6)
                                gameCounter = SumAllDigits(Player.Score, 4) + game.GameNum;
                            else
                                if (Player.Score.Length == 3 || Player.Score.Length == 4)
                                gameCounter = int.Parse(Player.Score[0].ToString()) + int.Parse(Player.Score[2].ToString());
                            break;
                        }
                    case 4:
                        {
                            if (Player.Score.Contains("-"))
                                Player.Score = Player.Score.Replace("-", " ");
                            if (Player.Score.Length >= 8)
                                gameCounter = SumAllDigits(Player.Score, 6) + game.GameNum;
                            else
                                if (Player.Score.Length == 5 || Player.Score.Length == 6)
                                gameCounter = int.Parse(Player.Score[0].ToString()) + int.Parse(Player.Score[2].ToString());
                            break;
                        }
                    case 5:
                        {
                            if (Player.Score.Contains("-"))
                                Player.Score = Player.Score.Replace("-", " ");
                            if (Player.Score.Length >= 10)
                                gameCounter = SumAllDigits(Player.Score, 8) + game.GameNum;
                            else
                                if (Player.Score.Length == 5 || Player.Score.Length == 6)
                                gameCounter = int.Parse(Player.Score[0].ToString()) + int.Parse(Player.Score[2].ToString());
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
            //Log += toadd + System.Environment.NewLine;
        }

        private string GetBookamerName(TennisGames.Bookers CurrentBooker)
        {
            return Enum.GetName(typeof(TennisGames.Bookers), CurrentBooker);
        }
        private string invertedScore(string toinvert)
        {
            if (toinvert != null)
            {
                string res = "";
                char todata = ' ';
                if (toinvert.Length > 3)
                {
                    for (var i = 0; i < toinvert.Length; i += 2)
                    {
                        if (Char.IsDigit(toinvert[i]))
                        {
                            if (todata == ' ')
                            {
                                todata = toinvert[i];
                            }
                            else
                            {
                                res += toinvert[i - 2];
                                res += toinvert[i - 1];
                                res += toinvert[i];

                                todata = ' ';
                            }
                        }
                        else
                        {
                            res += toinvert[i];
                        }
                    }
                }
                return res;
            }
            else
                return null;
        }
        private bool CompareScores(string score1, string score2)
        {

            bool equal = true;
            if ((score1 == null || score2 == null) || (score1.Length == 0 || score2.Length == 0))
                return true;
            else
            {
                if (score1.Length == score2.Length)
                {
                    for (var i = 0; i < score1.Length; i++)
                    {
                        if (Char.IsDigit(score1[i]))
                        {
                            if (Char.IsDigit(score2[i]))
                            {
                                if (score1[i] == score2[i])
                                    continue;
                                else
                                {
                                    equal = false; break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    equal = false;
                }
            }
            return equal;
        }
        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
        private bool hasNoEmptyPlayers(string Player1, string Player2)
        {
            return Player1 != "" && Player2 != "";
        }
        public string CutBooks(string Player, TennisGames.Bookers book)
        {
            if (book == TennisGames.Bookers.Bet365 || book == TennisGames.Bookers.Titanbet)
            {
                return CutBet365Player(Player);
            }
            if (book == TennisGames.Bookers.Marathon || book == TennisGames.Bookers.Sportingbet || book == TennisGames.Bookers.PariMatch)
            {
                return CutMarathon(Player);
            }
            if(book==TennisGames.Bookers.Williams)
            {
                return CutWilliams(Player);
            }
            else
                return Player;
            
        }
        public string CutPinnacle(string player)
        {
            try
            {
                string trys = player.Replace(player.Substring(1, player.IndexOf(" ") - 1), "");

                return trys;
            }
            catch
            {
                return player;
            }
        }
        public void CompareTennisGames(TennisGames Book1, TennisGames Book2)
        {

            int countcomp = 0;
            //Log = "";
            TennisGames BookHigh;
            TennisGames BookLow;

            WordsMatching.MatchsMaker match;
            WordsMatching.MatchsMaker match1;
            WordsMatching.MatchsMaker match2;
            WordsMatching.MatchsMaker match3;
            //  WordsMatching.MatchsMaker match4;
            if (Book1 != null && Book2 != null && (Book1.TennisData.Count > Book2.TennisData.Count))
            {
                lock (Book1)
                {
                    BookHigh = Book1;
                }
                lock (Book2)
                {
                    BookLow = Book2;
                }
            }
            else
            {
                lock (Book2)
                {
                    BookHigh = Book2;
                }
                lock (Book1)
                {
                    BookLow = Book1;
                }

            }
            if (Book1 != null && Book1.TennisData.Count != 0 && Book2 != null && Book2.TennisData.Count != 0)
                for (int i = 0; i < BookHigh.TennisData.Count; i++)// (int i = 0; i < BookHigh.TennisData.Count; i++)
                {
                    if (BookLow.TennisData.Count > 0)
                        for (int j = 0; j < BookLow.TennisData.Count; j++)
                        {
                            if (BookLow.TennisData.Count > 0 && BookHigh.TennisData.Count > 0)
                            {

                                if (hasNoEmptyPlayers(BookHigh.TennisData[i].TennisPlayers.Player1, BookHigh.TennisData[i].TennisPlayers.Player2) && hasNoEmptyPlayers(BookLow.TennisData[j].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2) && CompareScores(BookHigh.TennisData[i].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.Score) || CompareScores(BookHigh.TennisData[i].TennisPlayers.Score, invertedScore(BookLow.TennisData[j].TennisPlayers.Score)))
                                {

                                    //Вынести в функцию marathon
                                    //match = new WordsMatching.MatchsMaker(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player1);
                                    //match1 = new WordsMatching.MatchsMaker(BookHigh.TennisData[i].TennisPlayers.Player2, BookLow.TennisData[j].TennisPlayers.Player2);
                                    //match2 = new WordsMatching.MatchsMaker(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2);
                                    //match3 = new WordsMatching.MatchsMaker(BookHigh.TennisData[i].TennisPlayers.Player2, BookLow.TennisData[j].TennisPlayers.Player1);
                                    var Player1h = "";
                                    var Player2h = "";
                                    var Player1l = "";
                                    var Player2l = "";
                                    if (BookHigh.CurrentBooker == TennisGames.Bookers.Pinnacle || BookLow.CurrentBooker == TennisGames.Bookers.Pinnacle)
                                    {
                                        Player1h = BookHigh.TennisData[i].TennisPlayers.Player1;
                                        Player2h = BookHigh.TennisData[i].TennisPlayers.Player2;
                                        Player1l = BookLow.TennisData[j].TennisPlayers.Player1;
                                        Player2l = BookLow.TennisData[j].TennisPlayers.Player2;
                                    }
                                    if(BookHigh.CurrentBooker == TennisGames.Bookers.Pinnacle&& BookLow.CurrentBooker == TennisGames.Bookers.Winline)
                                    {
                                        Player1h = CutPinnacle(BookHigh.TennisData[i].TennisPlayers.Player1);
                                        Player2h = CutPinnacle(BookHigh.TennisData[i].TennisPlayers.Player2);
                                        Player1l = BookLow.TennisData[j].TennisPlayers.Player1;
                                        Player2l = BookLow.TennisData[j].TennisPlayers.Player2;
                                    }
                                    if (BookHigh.CurrentBooker == TennisGames.Bookers.Winline && BookLow.CurrentBooker == TennisGames.Bookers.Pinnacle)
                                    {
                                        Player1h = BookHigh.TennisData[i].TennisPlayers.Player1;
                                        Player2h = BookHigh.TennisData[i].TennisPlayers.Player2;
                                        Player1l = CutPinnacle(BookLow.TennisData[j].TennisPlayers.Player1);
                                        Player2l = CutPinnacle(BookLow.TennisData[j].TennisPlayers.Player2);
                                    }
                                    else
                                    {
                                        Player1h = CutBooks(BookHigh.TennisData[i].TennisPlayers.Player1, BookHigh.CurrentBooker);
                                        Player2h = CutBooks(BookHigh.TennisData[i].TennisPlayers.Player2, BookHigh.CurrentBooker);
                                        Player1l = CutBooks(BookLow.TennisData[j].TennisPlayers.Player1, BookLow.CurrentBooker);
                                        Player2l = CutBooks(BookLow.TennisData[j].TennisPlayers.Player2, BookLow.CurrentBooker);
                                    }
                                    match = new WordsMatching.MatchsMaker(Player1h, Player1l);
                                    match1 = new WordsMatching.MatchsMaker(Player2h, Player2l);
                                    match2 = new WordsMatching.MatchsMaker(Player1h, Player2l);
                                    match3 = new WordsMatching.MatchsMaker(Player2h, Player1l);



                                    //match4 = new WordsMatching.MatchsMaker(BookHigh.TennisData[i].TennisPlayers.Player2, BookHigh.TennisData[j].TennisPlayers.Player2);

                                    if ((match.Score + match1.Score) > 1.1)
                                    {
                                        countcomp++;
                                        //AddLog(BookHigh.TennisData[i].TennisPlayers.Player1 + " " + BookLow.TennisData[j].TennisPlayers.Player2);
                                        for (int u = 0; u < BookHigh.TennisData[i].TennisGameCoefs.Count; u++)
                                        {
                                            for (int h = 0; h < BookLow.TennisData[j].TennisGameCoefs.Count; h++)
                                            {
                                                double koef11 = BookHigh.TennisData[i].TennisGameCoefs[u].Coefficent1;
                                                double koef12 = BookHigh.TennisData[i].TennisGameCoefs[u].Coefficent2;
                                                double koef21 = BookLow.TennisData[j].TennisGameCoefs[h].Coefficent1;
                                                double koef22 = BookLow.TennisData[j].TennisGameCoefs[h].Coefficent2;
                                                if (BookHigh.TennisData[i].TennisPlayers.Score == "" && BookLow.TennisData[j].TennisPlayers.Score != "")
                                                {
                                                    var toscore = BookHigh.TennisData[i];
                                                    toscore.TennisPlayers.Score = BookLow.TennisData[j].TennisPlayers.Score;
                                                    toscore.TennisPlayers.ScoreSet = BookLow.TennisData[j].TennisPlayers.ScoreSet;
                                                    BookHigh.TennisData[i] = toscore;
                                                }
                                                else
                                                {
                                                    if (BookHigh.TennisData[i].TennisPlayers.Score != "" && BookLow.TennisData[j].TennisPlayers.Score == "")
                                                    {
                                                        var toscore = BookLow.TennisData[j];
                                                        toscore.TennisPlayers.Score = BookHigh.TennisData[i].TennisPlayers.Score;
                                                        toscore.TennisPlayers.ScoreSet = BookHigh.TennisData[i].TennisPlayers.ScoreSet;
                                                        BookLow.TennisData[j] = toscore;
                                                    }
                                                }
                                                if (BookHigh.TennisData[i].TennisPlayers.ScoreSet == "0 0" && BookLow.TennisData[j].TennisPlayers.ScoreSet != "0 0")
                                                {
                                                    var toscore = BookHigh.TennisData[i];
                                                    //toscore.TennisPlayers.Score = BookLow.TennisData[j].TennisPlayers.Score;
                                                    toscore.TennisPlayers.ScoreSet = BookLow.TennisData[j].TennisPlayers.ScoreSet;
                                                    BookHigh.TennisData[i] = toscore;
                                                }
                                                else
                                                {
                                                    if (BookHigh.TennisData[i].TennisPlayers.ScoreSet != "0 0" && BookLow.TennisData[j].TennisPlayers.ScoreSet == "0 0")
                                                    {
                                                        var toscore = BookLow.TennisData[j];
                                                        //toscore.TennisPlayers.Score = BookHigh.TennisData[i].TennisPlayers.Score;
                                                        toscore.TennisPlayers.ScoreSet = BookHigh.TennisData[i].TennisPlayers.ScoreSet;
                                                        BookLow.TennisData[j] = toscore;
                                                    }
                                                }
                                                if (ConvertGames(BookHigh.TennisData[i].TennisGameCoefs[u], BookHigh.TennisData[i].TennisPlayers) == ConvertGames(BookLow.TennisData[j].TennisGameCoefs[h], BookLow.TennisData[j].TennisPlayers))
                                                {
                                                    if (((koef11 * koef22) / (koef22 + koef11) - 1) > 0)
                                                    {
                                                        if (BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == BookLow.TennisData[j].TennisGameCoefs[h].SetNum || BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == 0 || BookLow.TennisData[j].TennisGameCoefs[h].SetNum == 0)
                                                            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1 + " " + BookHigh.TennisData[i].TennisPlayers.Player2, BookLow.TennisData[j].TennisPlayers.Player1 + " " + BookLow.TennisData[j].TennisPlayers.Player2, koef11, koef22, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    }
                                                    if (((koef12 * koef21) / (koef21 + koef12) - 1) > 0)
                                                    {
                                                        if (BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == BookLow.TennisData[j].TennisGameCoefs[h].SetNum || BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == 0 || BookLow.TennisData[j].TennisGameCoefs[h].SetNum == 0)
                                                            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1 + " " + BookHigh.TennisData[i].TennisPlayers.Player2, BookLow.TennisData[j].TennisPlayers.Player1 + " " + BookLow.TennisData[j].TennisPlayers.Player2, koef12, koef21, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    }
                                                    //if (koef11 > koef21)
                                                    //{
                                                    //    if ((1 / koef11 + 1 / koef22) < 1)
                                                    //    {
                                                    //        AddLog(Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker) + " Player1:" + BookHigh.TennisData[i].TennisPlayers.Player1 + Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker) + "Player2:" + BookLow.TennisData[j].TennisPlayers.Player2);
                                                    //        AddLog("Sum:" + (1 / koef11 + 1 / koef22).ToString());
                                                    //        AddLog("Numgam:" + BookLow.TennisData[j].TennisGameCoefs[h].GameNum);
                                                    //        AddLog("Pl1:" + koef22 + " Pl2:" + koef11);
                                                    //        if (koef11 > koef22)
                                                    //        {
                                                    //
                                                    //            AddLog("White bet.");
                                                    //            AddLog("1000 on pl1 +" + (1000 * koef22) / koef11 + " on pl2");
                                                    //            AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (1000 * koef22) / koef11)));
                                                    //            AddLog("Blue bet.");
                                                    //            AddLog("1000 on pl1 +" + (-1000 / (1 - koef11)) + " on pl2");
                                                    //            AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (-1000 / (1 - koef11)))));
                                                    //            AddLog(System.Environment.NewLine);
                                                    //            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef22, koef11, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //
                                                    //
                                                    //            AddLog("White bet.");
                                                    //            AddLog("1000 on pl2 +" + (1000 * koef11) / koef22 + " on pl1");
                                                    //            AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (1000 * koef11) / koef22)));
                                                    //            AddLog("Blue bet.");
                                                    //            AddLog("1000 on pl2 +" + (-1000 / (1 - koef22)) + " on pl1");
                                                    //            AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (-1000 / (1 - koef22)))));
                                                    //            AddLog(System.Environment.NewLine);
                                                    //            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef21, koef12, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    //
                                                    //        }
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    if ((1 / koef21 + 1 / koef12) < 1)
                                                    //    {
                                                    //        AddLog(Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker) + " Player1:" + BookLow.TennisData[i].TennisPlayers.Player1 + Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker) + "Player2:" + BookHigh.TennisData[j].TennisPlayers.Player2);
                                                    //        AddLog("Sum:" + (1 / koef21 + 1 / koef12).ToString());
                                                    //        AddLog("Numgam:" + BookLow.TennisData[j].TennisGameCoefs[h].GameNum);
                                                    //        AddLog("Pl1:" + koef12 + " Pl2:" + koef21);
                                                    //        if (koef21 > koef12)
                                                    //        {
                                                    //
                                                    //            AddLog("White bet.");
                                                    //            AddLog("1000 on pl1 +" + (1000 * koef22) / koef11 + " on pl2");
                                                    //            AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (1000 * koef22) / koef11)));
                                                    //            AddLog("Blue bet.");
                                                    //            AddLog("1000 on pl1 +" + (-1000 / (1 - koef11)) + " on pl2");
                                                    //            AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (-1000 / (1 - koef11)))));
                                                    //            AddLog(System.Environment.NewLine);
                                                    //            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef22, koef11, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //
                                                    //
                                                    //            AddLog("White bet.");
                                                    //            AddLog("1000 on pl2 +" + (1000 * koef11) / koef22 + " on pl1");
                                                    //            AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (1000 * koef11) / koef22)));
                                                    //            AddLog("Blue bet.");
                                                    //            AddLog("1000 on pl2 +" + (-1000 / (1 - koef22)) + " on pl1");
                                                    //            AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (-1000 / (1 - koef22)))));
                                                    //            AddLog(System.Environment.NewLine);
                                                    //            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef22, koef11, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    //
                                                    //        }
                                                    //    }
                                                    //}
                                                }

                                            }

                                        }

                                    }

                                    if ((match.Score + match1.Score)< (match2.Score + match3.Score)&&(match2.Score + match3.Score) > 1.1)
                                    {
                                        countcomp++;
                                        //AddLog(BookHigh.TennisData[i].TennisPlayers.Player1 + " " + BookLow.TennisData[j].TennisPlayers.Player2);
                                        for (int u = 0; u < BookHigh.TennisData[i].TennisGameCoefs.Count; u++)
                                        {
                                            for (int h = 0; h < BookLow.TennisData[j].TennisGameCoefs.Count; h++)
                                            {
                                                double koef11 = BookHigh.TennisData[i].TennisGameCoefs[u].Coefficent1;
                                                double koef12 = BookHigh.TennisData[i].TennisGameCoefs[u].Coefficent2;
                                                double koef21 = BookLow.TennisData[j].TennisGameCoefs[h].Coefficent1;
                                                double koef22 = BookLow.TennisData[j].TennisGameCoefs[h].Coefficent2;
                                                if (BookHigh.TennisData[i].TennisPlayers.Score == "" && BookLow.TennisData[j].TennisPlayers.Score != "")
                                                {
                                                    var toscore = BookHigh.TennisData[i];
                                                    toscore.TennisPlayers.Score = BookLow.TennisData[j].TennisPlayers.Score;
                                                    toscore.TennisPlayers.ScoreSet = BookLow.TennisData[j].TennisPlayers.ScoreSet;
                                                    BookHigh.TennisData[i] = toscore;
                                                }
                                                else
                                                {
                                                    if (BookHigh.TennisData[i].TennisPlayers.Score != "" && BookLow.TennisData[j].TennisPlayers.Score == "")
                                                    {
                                                        var toscore = BookLow.TennisData[j];
                                                        toscore.TennisPlayers.Score = BookHigh.TennisData[i].TennisPlayers.Score;
                                                        toscore.TennisPlayers.ScoreSet = BookHigh.TennisData[i].TennisPlayers.ScoreSet;
                                                        BookLow.TennisData[j] = toscore;
                                                    }
                                                }
                                                if (ConvertGames(BookHigh.TennisData[i].TennisGameCoefs[u], BookHigh.TennisData[i].TennisPlayers) == ConvertGames(BookLow.TennisData[j].TennisGameCoefs[h], BookLow.TennisData[j].TennisPlayers))
                                                {
                                                    if (((koef11 * koef21) / (koef21 + koef11) - 1) > 0)
                                                    {
                                                        if (BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == BookLow.TennisData[j].TennisGameCoefs[h].SetNum || BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == 0 || BookLow.TennisData[j].TennisGameCoefs[h].SetNum == 0)
                                                            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1 + " " + BookHigh.TennisData[i].TennisPlayers.Player2, BookLow.TennisData[j].TennisPlayers.Player1 + " " + BookLow.TennisData[j].TennisPlayers.Player2, koef11, koef21, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    }
                                                    if (((koef12 * koef22) / (koef22 + koef12) - 1) > 0)
                                                    {
                                                        if (BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == BookLow.TennisData[j].TennisGameCoefs[h].SetNum || BookHigh.TennisData[i].TennisGameCoefs[u].SetNum == 0 || BookLow.TennisData[j].TennisGameCoefs[h].SetNum == 0)
                                                            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1 + " " + BookHigh.TennisData[i].TennisPlayers.Player2, BookLow.TennisData[j].TennisPlayers.Player1 + " " + BookLow.TennisData[j].TennisPlayers.Player2, koef12, koef22, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    }
                                                    //if (koef11 > koef21)
                                                    //{
                                                    //    if ((1 / koef11 + 1 / koef22) < 1)
                                                    //    {
                                                    //        AddLog(Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker) + " Player1:" + BookHigh.TennisData[i].TennisPlayers.Player1 + Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker) + "Player2:" + BookLow.TennisData[j].TennisPlayers.Player2);
                                                    //        AddLog("Sum:" + (1 / koef11 + 1 / koef22).ToString());
                                                    //        AddLog("Numgam:" + BookLow.TennisData[j].TennisGameCoefs[h].GameNum);
                                                    //        AddLog("Pl1:" + koef22 + " Pl2:" + koef11);
                                                    //        if (koef11 > koef22)
                                                    //        {
                                                    //
                                                    //            AddLog("White bet.");
                                                    //            AddLog("1000 on pl1 +" + (1000 * koef22) / koef11 + " on pl2");
                                                    //            AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (1000 * koef22) / koef11)));
                                                    //            AddLog("Blue bet.");
                                                    //            AddLog("1000 on pl1 +" + (-1000 / (1 - koef11)) + " on pl2");
                                                    //            AddLog("Takeble profit:" + (1000 * koef22 - (1000 + (-1000 / (1 - koef11)))));
                                                    //            AddLog(System.Environment.NewLine);
                                                    //            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef22, koef11, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //
                                                    //
                                                    //            AddLog("White bet.");
                                                    //            AddLog("1000 on pl2 +" + (1000 * koef11) / koef22 + " on pl1");
                                                    //            AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (1000 * koef11) / koef22)));
                                                    //            AddLog("Blue bet.");
                                                    //            AddLog("1000 on pl2 +" + (-1000 / (1 - koef22)) + " on pl1");
                                                    //            AddLog("Takeble profit:" + (1000 * koef11 - (1000 + (-1000 / (1 - koef22)))));
                                                    //            AddLog(System.Environment.NewLine);
                                                    //            InsertInTable(BookHigh.TennisData[i].TennisPlayers.Player1, BookLow.TennisData[j].TennisPlayers.Player2, koef22, koef11, Enum.GetName(typeof(TennisGames.Bookers), BookHigh.CurrentBooker).ToString(), Enum.GetName(typeof(TennisGames.Bookers), BookLow.CurrentBooker).ToString(), BookLow.TennisData[j].TennisGameCoefs[h].GameNum.ToString(), BookLow.TennisData[j].TennisPlayers.Score, BookLow.TennisData[j].TennisPlayers.ScoreSet);
                                                    //
                                                    //        }
                                                    //    }
                                                    //}
                                                }

                                            }

                                        }

                                    }


                                }
                            }
                        }
                    //AddLog("Compares " + GetBookamerName(BookHigh.CurrentBooker) + " " + GetBookamerName(BookLow.CurrentBooker) + " " + countcomp + " games");
                }
        }
    }

    class PlayersDraw
    {
        /* string Player1;
         string Player2;
         string ScoreAll;
         String ScoreSet;
         List<int> ListOfBookmakers = new List<int>(10);
         */
    }
}

