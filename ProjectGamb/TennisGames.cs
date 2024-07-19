using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGamb
{
    public class TennisGames
    {
        //Properties test

        public struct Games
        {
            public int SetNum;
            public int GameNum;
            public double Coefficent1;
            public double Coefficent2;
        }
        public struct Players
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
            WilliamsHill=0,
            UniBet=1,
            Bet365=2,
            Marathon=3,
            PariMatch=4,
            Fonbet=5
        }
        public struct TennisDataStruct
        {
            public Players TennisPlayers;
            public List<Games> TennisGameCoefs;
        }
        public Players tempPlayers;
        public List<Games> tempGames;
        public bool BookmakerCategory;
        public List<TennisDataStruct> TennisData= new List<TennisDataStruct>();
        public Bookers CurrentBooker;

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

        public void AddGames(string SetNumber, string GameNumber, string Coefficent1, string Coefficent2)
        {
            Games tmpGames = new Games();
            tmpGames.GameNum = int.Parse(GameNumber);
            tmpGames.SetNum = int.Parse(SetNumber);
            tmpGames.Coefficent1 = double.Parse(Coefficent1, CultureInfo.CreateSpecificCulture("en-US"));
            tmpGames.Coefficent2 = double.Parse(Coefficent2, CultureInfo.CreateSpecificCulture("en-US"));
            if(!tempGames.Contains(tmpGames))
            tempGames.Add(tmpGames);
        }

        public void AddData()
        {
            TennisDataStruct tmpTennisDataStruct= new TennisDataStruct();
            tmpTennisDataStruct.TennisPlayers=tempPlayers;
            tmpTennisDataStruct.TennisGameCoefs =tempGames;
            TennisData.Add(tmpTennisDataStruct);
        }

        public void CleanData()
        {
            TennisData = new List<TennisDataStruct>();
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
                    if (el.SetNum != null)
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
            return returnValue;
        }

    }
}
