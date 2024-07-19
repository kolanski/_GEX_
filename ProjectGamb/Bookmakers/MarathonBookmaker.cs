using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGamb
{
    class MarathonBookmaker : BookmakerPattern
    {
        public bool automatic = false;
        List<string> CurrentGames;
        string MarathonBaseUrl = "https://www.betmarathon.com/en/live/view/";


        public void GetLinks()
        {
            Console.WriteLine("MarCurrLinks");
            GamesLinks = new List<string>();
            var list = this.ParentBrowser.Document.GetElementsByClassName("live-selection");
            for (int i = 0; i < list.Length; i++)
            {
                try
                {
                    var toAdd = list[i].ParentNode.ParentNode.ParentNode;
                    if (toAdd.SelectFirst(".//@data-category-sport").NodeValue == "Tennis")
                    {
                        string valueOfCurrGame = list[i].SelectFirst(".//@id").NodeValue;
                        Console.WriteLine(valueOfCurrGame);
                        GamesLinks.Add(valueOfCurrGame);
                    }
                }
                catch (NullReferenceException)
                {

                }
                catch (Exception e)
                {
                    Console.WriteLine("MarGetLinksErr:" + e);
                }
            }
        }

        public void OpenTabs()
        {
            for (int i = 0; i < GamesLinks.Count; i++)
            {
                this.CreateTab();
                this.BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate(MarathonBaseUrl + GamesLinks[i]);
            }
        }

        public void GetUrls()
        {
            CurrentGames = new List<string>();
            Console.WriteLine("CurrentGames");
            try
            {
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    Console.WriteLine(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().Remove(0, BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().IndexOf("view/") + 5));
                    CurrentGames.Add(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().Remove(0, BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString().IndexOf("view/") + 5));
                }
            }
            catch (Exception e)
            {

            }
        }
        private string CutPlayer(string tocut)
        {
            string ret = tocut;
            int i=0;
            while(i<ret.Length)
            {
                if (ret[i] == '.')
                {
                    ret = ret.Remove(i - 1, 2);
                    i = i - 1;
                }
                i++;
            }
            
            return ret;
        }
        public void Parse()
        {
            try
            {
                BookmakerTennisGames.CleanData();
                this.BookmakerTennisGames.CurrentBooker = TennisGames.Bookers.Marathon;
                for (int i = 0; i < BookmakerTabs.Count; i++)
                {
                    string Score = "";
                    string GamePoints = "";
                    string Player1 = "";
                    string Player2 = "";
                    
                    string[] splitted;
                    var CurrentBrowser = this.BookmakerWebBrowsers[i].geckoWebBrowser1.Document;
                    var PlayersData = CurrentBrowser.GetElementsByClassName("live-today-member-name nowrap");
                    var GamesData = CurrentBrowser.GetElementsByClassName("market-table-name");
                    if (PlayersData != null&&PlayersData.Length>0)
                    {
                        try
                        {
                            Player1 = PlayersData[0].TextContent.Replace(",", " ");
                            Player2 = PlayersData[1].TextContent.Replace(",", " ");
                            Score = CurrentBrowser.GetElementsByClassName("cl-left red")[0].TextContent.Replace("(", "").Replace(")", "");
                            splitted = Score.Split(' ');
                            GamePoints = splitted[splitted.Length - 1].Replace(":", " ");
                            if (Player1.Contains(".") || Player2.Contains("."))
                            {
                                Player1 = CutPlayer(Player1);
                                Player2 = CutPlayer(Player2);
                            }
                            if (splitted.Length == 3)
                            {
                                Score = splitted[0];
                            }
                            else
                            {
                                Score = "";
                                for (int s = 1; s < splitted.Length - 1; s++)
                                {
                                    Score += splitted[s].Replace(":", " ").Replace(",", " ");
                                }
                            }

                            BookmakerTennisGames.SetPlayers(Player1, Player2);
                            BookmakerTennisGames.SetGameData("", Score, GamePoints);

                            GamesData = CurrentBrowser.GetElementsByClassName("market-table-name");
                            //Console.WriteLine(Player1 + " " + Player2 + " " + Score +" "+ GamePoints);
                        }
                        catch(Exception e)
                        {

                        }
                        try
                        {


                            for (int k = 0; k < GamesData.Length; k++)
                            {
                                if (GamesData[k].TextContent.Contains("To Win Game"))
                                {
                                    var TableWithGames = GamesData[k].NextSibling.NextSibling;
                                    int CountOfTr = TableWithGames.EvaluateXPath(".//tr").GetNodes().Count();
                                    for (int h = 2; h <= CountOfTr; h++)
                                    {
                                        try
                                        {
                                            if ((TableWithGames.EvaluateXPath(".//tr[" + h + "]/td[1]/span") != null))
                                            {
                                                int index = GamesData[k].TextContent.IndexOfAny(new char[] { '1', '2', '3', '4' });
                                                char numset = GamesData[k].TextContent.ToString()[GamesData[k].TextContent.IndexOfAny(new char[] { '1', '2', '3', '4', '5' })];
                                                string numgame = TableWithGames.EvaluateXPath(".//tr[" + h + "]/td/span").GetNodes().FirstOrDefault().TextContent.Replace("Game", "").Replace(" ", "").Replace(" ", "");
                                                string coef1 = TableWithGames.EvaluateXPath(".//tr[" + h + "]/td[2]/div").GetNodes().FirstOrDefault().TextContent.Replace("\n", "").Replace(" ", "").Replace(" ", "");
                                                string coef2 = TableWithGames.EvaluateXPath(".//tr[" + h + "]/td[3]/div").GetNodes().FirstOrDefault().TextContent.Replace("\n", "").Replace(" ", "").Replace(" ", "");
                                                BookmakerTennisGames.AddGames(numset.ToString(), numgame, coef1, coef2);
                                                //  Console.WriteLine(numset+ " "+numgame + " " + coef1+ " "+ coef2);
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("MarathonParseerr" + e);
                                        }
                                    }
                                }
                            }

                        }



                        catch (Exception ex)
                        {
                            Console.WriteLine("MarPrserr:" + ex);
                        }
                        BookmakerTennisGames.AddData();
                    }
                }
            }
            catch(System.NullReferenceException)
            {
                Console.WriteLine("Marathon have NoGames or Not Loaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("MarPrserr:" + ex);
            }
        }

        public void CompareGames()
        {
            GetUrls();
            GetLinks();
            foreach (string GameNum in GamesLinks.Except(CurrentGames))
            {
                foreach (string link in GamesLinks)
                {
                    if (GameNum != null && link.Contains(GameNum))
                    {
                        Console.WriteLine("ToAdd");
                        Console.WriteLine(link);
                        CreateTab(MarathonBaseUrl + link);
                    }
                }
            }
        }

        public void CompareGamesSafe()
        {
            GetUrls();
            GetLinks();
            foreach (string GameNum in GamesLinks.Except(CurrentGames))
            {
                foreach (string link in GamesLinks)
                {
                    if (GameNum != null && link.Contains(GameNum))
                    {
                        Console.WriteLine("ToAdd");
                        Console.WriteLine(link);
                        CreateTabSafe(MarathonBaseUrl + link);
                    }
                }
            }
        }

        public void CompareToRemove()
        {
            GetLinks();
            GetUrls();
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {
                CloseTab(MarathonBaseUrl + GameToRemove);
                Console.WriteLine("ClosedMar" + GameToRemove);
            }
            foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
            {
                CloseTab(MarathonBaseUrl + GameToRemove);
                Console.WriteLine("Closedmar" + GameToRemove);
            }
        }

        public void CompareToRemoveSafe()
        {
            GetLinks();
            GetUrls();
            foreach (string GameToRemove in GamesLinks.Except(this.CurrentGames))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(MarathonBaseUrl + GameToRemove);
                    Console.WriteLine("ClosedMar" + GameToRemove);
                }
            }
            foreach (string GameToRemove in CurrentGames.Except(this.GamesLinks))
            {
                if (GameToRemove != "about:blank")
                {
                    CloseTabSafe(MarathonBaseUrl + GameToRemove);
                    Console.WriteLine("Closedmar" + GameToRemove);
                }
            }
        }

        public void setAutomatic()
        {
            automatic = !automatic;
            Console.WriteLine("MarAutois:" + automatic);
            AutoUpdateSafe();
        }
        private async Task AutoUpdate()
        {
            while (automatic)
            {
                try
                {
                    CompareGames();
                    await Task.Delay(5000);
                    CompareToRemove();
                    await Task.Delay(5000);
                }
                catch
                {

                }
            }
        }

        private async Task AutoUpdateSafe()
        {
            while (automatic)
            {
                try
                {
                    CompareGamesSafe();
                    await Task.Delay(5000);
                    CompareToRemoveSafe();
                    await Task.Delay(5000);
                }
                catch
                {

                }
            }
        }
    }
}
