using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using BTware_TestParsings;
using System.Threading.Tasks;

namespace BTware_TestParsings
{
    public class BETPatrn
    {
        public HtmlDocument ParDoc;
        public logS log;
        public tennisData TData;
        ///<summary>
        ///Загрузка из файла. 
        ///</summary>
        public void Load(string toload)
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            string Doctext;
            using (StreamReader reader = new StreamReader(toload, System.Text.Encoding.GetEncoding(1251)))
            {
                Doctext = reader.ReadToEnd();
                reader.Close();
            }
            ParDoc.LoadHtml(Doctext);
            log.Add("Created from file:"+toload);
        }
        ///<summary>
        ///Загрузка из текста. 
        ///</summary>
        public void LoadS(string toload)
        {
            ParDoc = new HtmlDocument();
            log = new logS();
            ParDoc.LoadHtml(toload);
            log.Add("Created from text length:" + toload.Length);
        }
        
    }
}
