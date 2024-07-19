using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BTware_TestParsings
{
    public class BET365: BETPatrn
    {
        public void ParseDoc()
        {
            HtmlNode player1 = ParDoc.DocumentNode.SelectSingleNode("//*[@id=\"page\"]/div[3]/div[1]/div[2]/div");
            log.Add(player1.InnerText.Substring(4, player1.InnerText.Length - 4));
            HtmlNode player2 = ParDoc.DocumentNode.SelectSingleNode("//*[@id=\"page\"]/div[3]/div[1]/div[2]/div[2]");
            log.Add(player2.InnerText.Substring(4, player2.InnerText.Length-4));
            HtmlNode score = ParDoc.DocumentNode.SelectSingleNode("//*[@id=\"ScreenTitle\"]/span[2]");
            log.Add(score.InnerText);
            HtmlNodeCollection games = ParDoc.DocumentNode.SelectNodes("//*[@id=\"page\"]/div[3]/div");
            foreach(HtmlNode el in games)
            {
                if (el.InnerText.Contains("Гейм - Победител"))
                {
                    log.Add(el.InnerText);
                  //  el.SelectSingleNode("");
                }
            }
        }
    }
}
