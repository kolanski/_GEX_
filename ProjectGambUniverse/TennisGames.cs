using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGambUniverse
{
    public class TennisGames
    {
        //Properties test

        public class Games
        {
            public int SetNum;
            public int GameNum;
            public double Coefficent1;
            public double Coefficent2;
        }

        public class Players
        {
            public bool isInverted;
            public string Player1;
            public string Player2;
            public string Event;
            public string Score;
            public string ScoreSet;
        }

        public enum Bookers
        {
            Williams = 0,
            UniBet = 1,
            Bet365 = 2,
            Marathon = 3,
            PariMatch = 4,
            Fonbet = 5,
            BetCity = 6,
            Olimp = 7,
            InternetBet = 8,
            Xbet = 9,
            Titanbet = 10,
            BetFair = 11,
            Pinnacle = 12,
            Sportingbet=13,
            Winline = 14
        }

        [Serializable]
        public class TennisDataStruct
        {
            public Players TennisPlayers;
            public List<Games> TennisGameCoefs;
        }

        public IEnumerator<List<TennisDataStruct>> Example()
        {
            yield return this.TennisData;

        }
        public Players tempPlayers;
        public List<Games> tempGames;
        public bool BookmakerCategory;
        public List<TennisDataStruct> TennisData= new List<TennisDataStruct>();
        public Bookers CurrentBooker;
        public IEnumerator<TennisDataStruct> GetEnumerator()
        {
            return TennisData.GetEnumerator();
        }
        public void SetPlayers(string Player1,string Player2)
        {
            tempPlayers = new Players();
            tempGames = new List<Games>();
            tempPlayers.Player1 = Player1;
            tempPlayers.Player2 = Player2;
        }

        public void SetGameData(string EventName,string Score,string ScoreSet)
        {
            if(tempPlayers.Player1!=null)
            {
                tempPlayers.Event = EventName;
                tempPlayers.Score = Score;
                tempPlayers.ScoreSet = ScoreSet;
            }
        }

        public void AddGames(int SetNumber,int GameNumber,double Coefficent1, double Coefficent2)
        {
            Games tmpGames= new Games();
            tmpGames.GameNum= GameNumber;
            tmpGames.SetNum = SetNumber;
            tmpGames.Coefficent1=Coefficent1;
            tmpGames.Coefficent2=Coefficent2;
            if (!tempGames.Contains(tmpGames))
            tempGames.Add(tmpGames);
        }
        private string GetBookamerName(TennisGames.Bookers CurrentBooker)
        {
            return Enum.GetName(typeof(TennisGames.Bookers), CurrentBooker);
        }
        public void AddGames(string SetNumber, string GameNumber, string Coefficent1, string Coefficent2)
        {
            if(!Coefficent1.Contains("/"))
            try
            {
                Games tmpGames = new Games();
                tmpGames.GameNum = int.Parse(GameNumber);
                tmpGames.SetNum = int.Parse(SetNumber);
                if (Coefficent1 != "SP")
                {
                    tmpGames.Coefficent1 = double.Parse(Coefficent1.Replace("▼","").Replace("▲",""), CultureInfo.CreateSpecificCulture("en-US"));
                    tmpGames.Coefficent2 = double.Parse(Coefficent2.Replace("▼", "").Replace("▲", ""), CultureInfo.CreateSpecificCulture("en-US"));
                    
                    if (!tempGames.Contains(tmpGames))
                        tempGames.Add(tmpGames);
                }
            }
            catch
            {

            }
        }

        public void AddData()
        {
            TennisDataStruct tmpTennisDataStruct= new TennisDataStruct();
            tmpTennisDataStruct.TennisPlayers=tempPlayers;
            tmpTennisDataStruct.TennisGameCoefs =tempGames;
            lock(TennisData)
            {
            TennisData.Add(tmpTennisDataStruct);
            }
        }
        public void modPlayer1(string mod)
        {

        }
        public void CleanData()
        {
            TennisData.Clear();
        }

        public void SetInvertedPlayer(string Player1,bool Inverted)
        {
            var ToInvert = TennisData.Find(i => i.TennisPlayers.Player1 == Player1);
            if(ToInvert.TennisPlayers.Player1!=null)
            ToInvert.TennisPlayers.isInverted = Inverted;
        }

        public string PrintGames()
        {
            string Sp = System.Environment.NewLine;
            string returnValue = "";
            TennisData.ForEach(i =>
            {
                returnValue += "Event"+i.TennisPlayers.Event+Sp;
                returnValue += "Player1:" + i.TennisPlayers.Player1 + Sp;
                returnValue += "Player2:" + i.TennisPlayers.Player2 + Sp;
                returnValue += "Score:" + i.TennisPlayers.Score + Sp;
                returnValue += "ScoreSet:" + i.TennisPlayers.ScoreSet + Sp;

                foreach (Games el in i.TennisGameCoefs)
                {
                    if (el.SetNum != 0)
                    {
                        returnValue += "Set:" + el.SetNum + " Game:" + el.GameNum + Sp;
                        returnValue += el.Coefficent1 + " " + el.Coefficent2 + Sp;
                    }
                    else
                    {
                        returnValue += " Game:" + el.GameNum + Sp;
                        returnValue += el.Coefficent1 + " " + el.Coefficent2 + Sp;
                    }

                }
                returnValue += Sp;
            });
            
                SearchArbitrage.ClearTable(GetBookamerName(this.CurrentBooker));
                TennisData.ForEach(i =>
                {
                    foreach (Games el in i.TennisGameCoefs)
                    {
                        SearchArbitrage.InsertInTable(i.TennisPlayers.Player1, i.TennisPlayers.Player2, el.Coefficent1, el.Coefficent2, GetBookamerName(this.CurrentBooker), GetBookamerName(this.CurrentBooker), el.GameNum.ToString(), i.TennisPlayers.Score, i.TennisPlayers.ScoreSet, GetBookamerName(this.CurrentBooker), el.SetNum.ToString());
                    }
                });
            
            return returnValue;
        }
        List<T> Clone<T>(IEnumerable<T> oldList)
        {
            return new List<T>(oldList);
        }
        public void PrintGames1()
        {
            string Sp = System.Environment.NewLine;

            SearchArbitrage.ClearTable(GetBookamerName(this.CurrentBooker));
            try
            {
                //change foreach to for
                List<TennisDataStruct> TennisDataTmp = Clone(TennisData);
                    for (int i = 0; i < TennisDataTmp.Count; i++)
                    {
                        if (TennisDataTmp[i].TennisGameCoefs != null && TennisDataTmp[i].TennisGameCoefs.Count > 0)
                        {
                            for (int j = 0; j < TennisDataTmp[i].TennisGameCoefs.Count; j++)
                            {
                                if (j < TennisDataTmp[i].TennisGameCoefs.Count&& TennisDataTmp[i].TennisGameCoefs[j]!=null)
                                    SearchArbitrage.InsertInTable(
                                        TennisDataTmp[i].TennisPlayers.Player1,
                                        TennisDataTmp[i].TennisPlayers.Player2,
                                        TennisDataTmp[i].TennisGameCoefs[j].Coefficent1,
                                        TennisDataTmp[i].TennisGameCoefs[j].Coefficent2,
                                        GetBookamerName(this.CurrentBooker),
                                        GetBookamerName(this.CurrentBooker),
                                        TennisDataTmp[i].TennisGameCoefs[j].GameNum.ToString(),
                                        TennisDataTmp[i].TennisPlayers.Score,
                                        TennisDataTmp[i].TennisPlayers.ScoreSet,
                                        GetBookamerName(this.CurrentBooker),
                                        TennisDataTmp[i].TennisGameCoefs[j].SetNum.ToString());
                                //TennisData[i].TennisGameCoefs[j].
                            }
                        }
                    
                }
                    //TennisData.ForEach(i =>
                    //{
                    //    foreach (Games el in i.TennisGameCoefs)
                    //    {
                    //        if (el != null)
                    //            SearchArbitrage.InsertInTable(i.TennisPlayers.Player1, i.TennisPlayers.Player2, el.Coefficent1, el.Coefficent2, GetBookamerName(this.CurrentBooker), GetBookamerName(this.CurrentBooker), el.GameNum.ToString(), i.TennisPlayers.Score, i.TennisPlayers.ScoreSet, GetBookamerName(this.CurrentBooker), el.SetNum.ToString());
                    //    }
                    //});
            }
            catch
            {

            }
        }

    }
}
