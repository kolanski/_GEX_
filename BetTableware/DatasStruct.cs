using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTware_TestParsings
{
    public struct strTgame              //Структура для данных гейма.
    {
        public string numgam;           //Номер гейма.
        public string koef1;            //Коэффицент для первого игрока.
        public string koef2;            //Коэффицент для второго игрока.
    }
    public struct strTevent             //Структура для данных события.
    {
        public string _event;           //Название события.
        public string _Player1;         //Первый игрок.
        public string _Player2;         //Второй игрок.
        public string score;            //Счёт.
    }

    public class listofTgames           //Класс для игр.
    {
        public int elsinlist;           //Количество элементов в классе.
        strTgame tmp = new strTgame();  //Временная структура для передачи.
        public List<strTgame> list= new List<strTgame>();//Список для геймов.
        public void AddGame(string numgame, string koef1, string koef2)
        {
            tmp.numgam = numgame;
            tmp.koef1 = koef1;
            tmp.koef2 = koef2;
            list.Add(tmp);
            elsinlist++;
        }
        public strTgame getList(int getnum)
        {
            if (getnum > 0 && getnum < elsinlist)
            {
                return list[getnum];
            }
            else
            {
                tmp.koef1 = "ER";
                tmp.koef2 = "ER";
                tmp.numgam = "ER";
                return tmp;
            }
        }
        public string getListitem(int intcnt)
        {
            return ("Numgam:" + list[intcnt].numgam + "Kf1: " + list[intcnt].koef1 + "Kf2: " + list[intcnt].koef1);
        }
    }

    public struct strTinfo
    {
        public strTevent eventInfo;
        public listofTgames gamesinEvent;
    }

    public class tennisData
    {
        public int Count;
        strTinfo tmp = new strTinfo();
        public List<strTinfo> list = new List<strTinfo>();
        public void add(strTinfo toadd)
        {
            tmp.eventInfo = toadd.eventInfo;
            tmp.gamesinEvent = toadd.gamesinEvent;
            list.Add(tmp);
            Count++;
        }
        public void add(strTevent toadde, listofTgames toaddg)
        {
            tmp.eventInfo = toadde;
            tmp.gamesinEvent = toaddg;
            list.Add(tmp);
            Count++;
        }
        
        public string printall()
        {
            string toprint = System.Environment.NewLine;
            for (int i = 0; i < Count; i++)
            {
                toprint += ("Event:" + list[i].eventInfo._event +System.Environment.NewLine+ "Player1:" + list[i].eventInfo._Player1 + " Player2:" + list[i].eventInfo._Player2);
                toprint+=System.Environment.NewLine;
                toprint += (list[i].eventInfo.score); toprint += System.Environment.NewLine;
                if(list[i].gamesinEvent.list!=null)
                for (int j = 0; j < list[i].gamesinEvent.list.Count; j++)
                {
                    toprint += "Numgam:" + list[i].gamesinEvent.list[j].numgam + " Koef1:" + list[i].gamesinEvent.list[j].koef1 + " Koef2:" +list[i].gamesinEvent.list[j].koef2;
                    toprint += System.Environment.NewLine;
                }
            }
            return toprint;
        }
        public void clear()
        {
                     tmp = new strTinfo();
                     list = new List<strTinfo>();
                     Count = 0;
        }
    }

    class DatasStruct
    {

    }
}
