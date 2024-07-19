using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGambUniverse
{
    public class GamesCache
    {
        int BooksCount = Enum.GetNames(typeof(TennisGames.Bookers)).Length;
        public List<PlayersList> Cached = new List<PlayersList>();
        private BooksData Data;
        private int idcount=0;
        public void update(TennisGames games)
        {

        }
        public bool getDataStatus()
        {
            return Data == null;
        }
        public void LoadBooks(BooksData bookstest)
        {
            Data = bookstest;
        }
        WordsMatching.MatchsMaker Match;
        WordsMatching.MatchsMaker Match1;
        WordsMatching.MatchsMaker Match2;
        WordsMatching.MatchsMaker Match3;
        public int getDataCount(int bookindex)
        {
            if (bookindex >= 0 && bookindex <= BooksCount)
                return Data.BookGames[bookindex].Count;
            else
                return 0;
        }
        private void UpdateCacheIds(int line)
        {
            
            int trueid=-1;
            //search norm id
            for(var i=0;i<BooksCount;i++)
            {
                if(Cached[line].Players[i]!=null)
                {
                    if (Cached[line].Players[i].id != -1)
                    {
                        trueid = Cached[line].Players[i].id;
                        break;
                    }
                }
            }
            for (var i = 0; i < BooksCount; i++)
            {
                if (Cached[line].Players[i] != null)
                {
                    if (Cached[line].Players[i].id == -1)
                    {
                        Cached[line].Players[i].id=trueid;
                        
                    }
                }
            }
        }
        private void SearchAndAdd(GameData toSearch, int bookIndex, int gameIndex)
        {
            if (Cached.Count == 0)
            {
                PlayersList list = new PlayersList();
                list.Players[bookIndex] = toSearch;
                list.id = idcount;
                toSearch.id = 0;
                Cached.Add(list);
                idcount++;
            }
            bool found = false;
            if (toSearch.game.TennisPlayers.Player1 != "" && toSearch.game.TennisPlayers.Player2 != "")
                for (var i = 0; i < Cached.Count; i++)
                {
                    //comparing
                    if (Cached[i].Players[bookIndex] == null)
                    {
                        for (var f = 0; f < BooksCount; f++)
                        {
                            if (Cached[i].Players[f] != null)
                            {
                                Match = new WordsMatching.MatchsMaker(Cached[i].Players[f].game.TennisPlayers.Player1, toSearch.game.TennisPlayers.Player1);
                                Match1 = new WordsMatching.MatchsMaker(Cached[i].Players[f].game.TennisPlayers.Player2, toSearch.game.TennisPlayers.Player2);
                                Match2 = new WordsMatching.MatchsMaker(Cached[i].Players[f].game.TennisPlayers.Player1, toSearch.game.TennisPlayers.Player2);
                                Match3 = new WordsMatching.MatchsMaker(Cached[i].Players[f].game.TennisPlayers.Player2, toSearch.game.TennisPlayers.Player1);
                                if ((Match.Score + Match1.Score) > 1.1)
                                {
                                    found = true;
                                    Cached[i].Players[bookIndex] = toSearch;
                                    if (Cached[i].Players[f].id != -1)
                                    {
                                        Cached[i].Players[bookIndex].id = Cached[i].Players[f].id;
                                        toSearch.id = Cached[i].Players[f].id;
                                        break;
                                    }
                                    else
                                    {
                                        Cached[i].Players[bookIndex].id = Cached[i].Players[f].id;
                                        toSearch.id = Cached[i].Players[f].id;
                                        break;
                                    }
                                }
                                if ((Match2.Score + Match3.Score) > 1.1)
                                {
                                    found = true;
                                    Cached[i].Players[bookIndex] = toSearch;
                                    Cached[i].Players[bookIndex].id = Cached[i].Players[f].id;
                                    Cached[i].Players[bookIndex].inverted = true;
                                    toSearch.id = Cached[i].Players[f].id;
                                    toSearch.inverted = true;
                                    break;
                                }
                                if(Match.Score + Match1.Score>0.9||Match2.Score + Match3.Score>0.9)
                                {

                                }
                                
                            }
                        }

                    }
                    else
                    {
                        if (Cached[i].Players[bookIndex].game.TennisPlayers.Player2.Length == toSearch.game.TennisPlayers.Player2.Length)
                        {
                            if (Cached[i].Players[bookIndex].game.TennisPlayers.Player2 == toSearch.game.TennisPlayers.Player2)
                            {
                                found = true;
                               // if (toSearch)
                                Cached[i].Players[bookIndex] = toSearch;
                                //Cached[i].Players[bookIndex].id = Cached[i].Players[f].id;
                                if (Cached[i].Players[bookIndex].id==-1)
                                {
                                    UpdateCacheIds(i);
                                }
                                toSearch.id = Cached[i].Players[bookIndex].id;
                            }
                        }
                        //continue;
                    }


                }
            if (!found)
            {
                if (toSearch.game.TennisPlayers.Player1.Length > 3)
                {
                    PlayersList list = new PlayersList();
                    list.Players[bookIndex] = toSearch;
                    list.id = idcount;
                    toSearch.id=list.id;
                    Cached.Add(list);
                    idcount++;
                }
            }
        }

        public void UpdateFound()
        {
           // int updateblecount = 0;
            for (int i = 0; i < BooksCount; i++)
            {
                if (Data.BookGames[i] != null)
                    for (int j = 0; j < Data.BookGames[i].Count; j++)
                    {
                        if (Data.BookGames[i].ElementAt(j).id == -1)
                        {
                            //Search or add to cache
                            SearchAndAdd(Data.BookGames[i].ElementAt(j), i, j);
                        }
                    }
            }
        }
        private void SearchForks(int line)
        {
            int tmpa = 0;
            int tmpb = 0;
            int tmpa1 = 0;
            int tmpb1 = 0;
            for (int i = 0; i < BooksCount; i++)
            {
                if (Cached[line].Players[i] != null)
                {
                    if (Cached[line].Players[i].game.TennisGameCoefs.Count > 0)
                    {
                        for (var f = 0; f < BooksCount; f++)
                        {
                            if (f != i && Cached[line].Players[i] != null)
                                for (int u = 0; u < Cached[line].Players[i].game.TennisGameCoefs.Count; u++)
                                {
                                    if (Cached[line].Players[f] != null && Cached[line].Players[f].game != null)
                                        for (int h = 0; h < Cached[line].Players[f].game.TennisGameCoefs.Count; h++)
                                        {
                                            double koef11 = 1;
                                            double koef12 = 1;
                                            double koef21 = 1;
                                            double koef22 = 1;
                                            if (Cached[line].Players[i].game.TennisGameCoefs[u] != null)
                                            {
                                                koef11 = Cached[line].Players[i].game.TennisGameCoefs[u].Coefficent1;
                                                koef12 = Cached[line].Players[i].game.TennisGameCoefs[u].Coefficent2;
                                            }
                                            if (Cached[line].Players[f].game.TennisGameCoefs[h] != null)
                                            {
                                                koef21 = Cached[line].Players[f].game.TennisGameCoefs[h].Coefficent1;
                                                koef22 = Cached[line].Players[f].game.TennisGameCoefs[h].Coefficent2;
                                            }
                                            if (Cached[line].Players[i].game.TennisPlayers.Score == "")
                                            {
                                                Cached[line].Players[i].game.TennisPlayers.Score = Cached[line].Players[f].game.TennisPlayers.Score;
                                            }
                                            if (Cached[line].Players[f].game.TennisPlayers.Score == "")
                                            {
                                                Cached[line].Players[f].game.TennisPlayers.Score = Cached[line].Players[i].game.TennisPlayers.Score;
                                            }
                                            if (SearchArbitrage.ConvertGames(Cached[line].Players[i].game.TennisGameCoefs[u], Cached[line].Players[i].game.TennisPlayers) == SearchArbitrage.ConvertGames(Cached[line].Players[f].game.TennisGameCoefs[h], Cached[line].Players[f].game.TennisPlayers))
                                            {
                                                if (!Cached[line].Players[i].inverted && !Cached[line].Players[f].inverted)
                                                {
                                                    if (((koef11 * koef22) / (koef22 + koef11) - 1) > 0)
                                                    {
                                                        if (tmpa != i && tmpb != f && tmpa != tmpa1 && tmpb != tmpb1)
                                                          //  SearchArbitrage.InsertInTable(Cached[line].Players[i].game.TennisPlayers.Player1 + " " + Cached[line].Players[i].game.TennisPlayers.Player2, Cached[line].Players[f].game.TennisPlayers.Player1 + " " + Cached[line].Players[f].game.TennisPlayers.Player2, koef11, koef22, Enum.GetName(typeof(TennisGames.Bookers), i).ToString(), Enum.GetName(typeof(TennisGames.Bookers), f).ToString(), Cached[line].Players[f].game.TennisGameCoefs[h].GameNum.ToString(), Cached[line].Players[f].game.TennisPlayers.Score, Cached[line].Players[f].game.TennisPlayers.ScoreSet);
                                                        tmpa = f;
                                                        tmpb = i;

                                                    }
                                                    if (((koef12 * koef21) / (koef21 + koef12) - 1) > 0)
                                                    {
                                                        if (tmpa1 != f && tmpb1 != i && tmpa1 != tmpa && tmpb1 != tmpb)
                                                           // SearchArbitrage.InsertInTable(Cached[line].Players[i].game.TennisPlayers.Player1 + " " + Cached[line].Players[i].game.TennisPlayers.Player2, Cached[line].Players[f].game.TennisPlayers.Player1 + " " + Cached[line].Players[f].game.TennisPlayers.Player2, koef12, koef21, Enum.GetName(typeof(TennisGames.Bookers), i).ToString(), Enum.GetName(typeof(TennisGames.Bookers), f).ToString(), Cached[line].Players[f].game.TennisGameCoefs[h].GameNum.ToString(), Cached[line].Players[f].game.TennisPlayers.Score, Cached[line].Players[f].game.TennisPlayers.ScoreSet);
                                                        tmpa1 = i;
                                                        tmpb1 = f;
                                                    }
                                                }
                                                else
                                                {
                                                    if (((koef11 * koef21) / (koef21 + koef11) - 1) > 0)
                                                    {
                                                        if (tmpa != i && tmpb != f && tmpa != tmpa1 && tmpb != tmpb1)
                                                            //SearchArbitrage.InsertInTable(Cached[line].Players[i].game.TennisPlayers.Player1 + " " + Cached[line].Players[i].game.TennisPlayers.Player2, Cached[line].Players[f].game.TennisPlayers.Player1 + " " + Cached[line].Players[f].game.TennisPlayers.Player2, koef11, koef22, Enum.GetName(typeof(TennisGames.Bookers), i).ToString(), Enum.GetName(typeof(TennisGames.Bookers), f).ToString(), Cached[line].Players[f].game.TennisGameCoefs[h].GameNum.ToString(), Cached[line].Players[f].game.TennisPlayers.Score, Cached[line].Players[f].game.TennisPlayers.ScoreSet);
                                                        tmpa = f;
                                                        tmpb = i;

                                                    }
                                                    if (((koef12 * koef22) / (koef22 + koef12) - 1) > 0)
                                                    {
                                                        if (tmpa1 != f && tmpb1 != i && tmpa1 != tmpa && tmpb1 != tmpb)
                                                           // SearchArbitrage.InsertInTable(Cached[line].Players[i].game.TennisPlayers.Player1 + " " + Cached[line].Players[i].game.TennisPlayers.Player2, Cached[line].Players[f].game.TennisPlayers.Player1 + " " + Cached[line].Players[f].game.TennisPlayers.Player2, koef12, koef21, Enum.GetName(typeof(TennisGames.Bookers), i).ToString(), Enum.GetName(typeof(TennisGames.Bookers), f).ToString(), Cached[line].Players[f].game.TennisGameCoefs[h].GameNum.ToString(), Cached[line].Players[f].game.TennisPlayers.Score, Cached[line].Players[f].game.TennisPlayers.ScoreSet);
                                                        tmpa1 = i;
                                                        tmpb1 = f;
                                                    }
                                                }
                                            }
                                        }
                                }
                        }
                    }
                }
            }
        }
        private bool LineSearch(int line)
        {
            int countdata = 0;
            for (int i = 0; i < BooksCount; i++)
            {
                if (Cached[line].Players[i] != null)
                {
                    countdata++;
                    if (countdata >= 2)
                    {
                        break;
                    }
                }
            }
            if (countdata > 1)
            {

                return true;
            }
            else
                return false;
        }
        public string Print()
        {
            string ret = "";
            for (var k = 0; k < Cached.Count; k++)
                for (int i = 0; i < BooksCount; i++)
                {
                    if (Cached[k].Players[i] != null)
                    {
                        ret += "id: " + Cached[k].Players[i].id + " numbook: " + i + " Pl1: " + Cached[k].Players[i].game.TennisPlayers.Player1 + " Pl2: " + Cached[k].Players[i].game.TennisPlayers.Player2 + " Score: " + Cached[k].Players[i].game.TennisPlayers.Score + System.Environment.NewLine;
                    }
                }
            return ret;
        }
        public string Print(int set)
        {
            string ret = "";
            string info = "";
            int count = 0;
            info += Enum.GetName(typeof(TennisGames.Bookers),set).ToString()+Environment.NewLine;
            for (var k = 0; k < Cached.Count; k++)
                for (int i = 0; i < BooksCount; i++)
                {
                    if (Cached[k].Players[i] != null&&i==set)
                    {
                        ret += "id: " + Cached[k].id + " numbook: " + i + " Pl1: " + Cached[k].Players[i].game.TennisPlayers.Player1 + " Pl2: " + Cached[k].Players[i].game.TennisPlayers.Player2 + " Score: " + Cached[k].Players[i].game.TennisPlayers.Score + System.Environment.NewLine;
                        count++;
                    }
                }
            info += Cached.Count + Environment.NewLine;
            info += count+Environment.NewLine;
            return info+ ret;
        }
        private bool LineDelete(int line)
        {
            int countdata = 0;
            for (int i = 0; i < BooksCount; i++)
            {
                if (Cached[line].Players[i] != null)
                {
                    countdata++;
                }
            }
            if (countdata <= 1)
            {
                //for (var g = 0; g < BooksCount; g++)
                //{
                //    if (Cached[line].Players[g] != null)
                //    {
                //        Cached[line].Players[g].id=-1;
                //        Cached[line].id = -1;
                //    }
                //}
                return true;
            }
            else
                return false;
        }
        private bool UpdateIds(int line)
        {
            int countdata = 0;
            for (int i = 0; i < BooksCount; i++)
            {
                if (Cached[line].Players[i] != null)
                {
                    if (Cached[line].id != line)
                    {
                        Cached[line].id = line;
                        countdata++;

                    }
                }
            }
            if (countdata > 1)
            {
                return true;
            }
            else
                return false;
        }
        public void SearchAll()
        {
            int searched = 0;
            int updated = 0;
            for (var i = 0; i < Cached.Count; i++)
            {
                if (LineSearch(i))
                {
                    searched++;
                    SearchForks(i);
                }
            }
           //for (var i = 0; i < Cached.Count; i++)
           //{
           //    if (LineDelete(i))
           //        Cached.RemoveAt(i);
           //}
           //for (var i = 0; i < Cached.Count; i++)
           //{
           //    if (UpdateIds(i))
           //        updated++;
           //}
            Console.WriteLine("Count of similar games:" + searched);
            Console.WriteLine("Count of cached games:" + Cached.Count);
            Console.WriteLine("Count of updated indexes:" + updated);
        }
    }
    public class BooksData
    {
        static int len = Enum.GetNames(typeof(TennisGames.Bookers)).Length;
        public List<GameData>[] BookGames = new List<GameData>[len];
        public void Update(TennisGames games)
        {
            int currentList = (int)games.CurrentBooker;
            if (BookGames[currentList] == null)
                BookGames[currentList] = new List<GameData>();

            for (var i = 0; i < games.TennisData.Count; i++)
            {
                //try to foun this string
                    if (BookGames[currentList].ElementAtOrDefault(i) != null && BookGames[currentList].ElementAtOrDefault(i).game.TennisPlayers.Player1.Trim() == games.TennisData[i].TennisPlayers.Player1.Trim())
                    {
                        BookGames[currentList].ElementAt(i).UpdateGames(games.TennisData[i]);
                    }
                    else
                    {
                        if (BookGames[currentList].ElementAtOrDefault(i) == null)
                            BookGames[currentList].Add(new GameData(games.TennisData[i], -1));
                        if (BookGames[currentList].ElementAtOrDefault(i).game.TennisPlayers.Player1 != games.TennisData[i].TennisPlayers.Player1)
                        {
                            BookGames[currentList].Insert(i, new GameData(games.TennisData[i], -1));
                        }
                    }
                
            }
            if (BookGames[currentList].Count > games.TennisData.Count)
            {
                for (var i = BookGames[currentList].Count; i > 0; i--)
                {
                    if (i > games.TennisData.Count)
                    {
                        BookGames[currentList].Remove(BookGames[currentList].ElementAt(i - 1));
                    }
                }
            }
        }
    }
    public class GameData
    {
        public TennisGames.TennisDataStruct game;
        public int id;
        public bool inverted = false;
        public GameData(TennisGames.TennisDataStruct tennisDataStruct, int p)
        {
            
            game = tennisDataStruct;
            id = p;
        }
        public GameData(TennisGames.TennisDataStruct tennisDataStruct)
        {
            
            game = tennisDataStruct;
            id = -1;
        }
        public void UpdateGames(TennisGames.TennisDataStruct tennisDataStruct)
        {
            game.TennisPlayers = tennisDataStruct.TennisPlayers;
            game.TennisGameCoefs = tennisDataStruct.TennisGameCoefs;
        }
        public void setId(int myid)
        {
            id = myid;
        }
    }
    public class PlayersList
    {
        static int len = Enum.GetNames(typeof(TennisGames.Bookers)).Length;
        public GameData[] Players = new GameData[len];
        public int id = -1;
       // int lastIndex = -1;

    }
}
