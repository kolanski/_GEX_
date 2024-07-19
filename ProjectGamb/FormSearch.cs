using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGamb
{
    public partial class FormSearch : Form
    {
        SearchArbitrage search;
        public Stopwatch uptime = new Stopwatch();
        public FormSearch()
        {
            InitializeComponent();
            search = new SearchArbitrage();
        }

        private void SearchParseAll_Click(object sender, EventArgs e)
        {
            Program.myForm1.SendToSearchForm();
        }

        public void requestupdate()
        {
            Program.myForm1.SendToSearchForm();
        }

        public void addText(string text, int numOfRichBox)
        {
            switch (numOfRichBox)
            {
                case 1:
                    {
                        richTextBox1.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.Clear();
                    if(!BlockCheck.Checked)
                    richTextBox1.AppendText(text);

                });
                        break;
                    }
                case 2:
                    {
                        richTextBox2.Invoke((MethodInvoker)delegate
                {
                    richTextBox2.Clear();
                    if (!BlockCheck.Checked)
                    richTextBox2.AppendText(text);

                });
                        break;
                    }

                case 3:
                    {
                        richTextBox3.Invoke((MethodInvoker)delegate
                {
                    richTextBox3.Clear();
                    if (!BlockCheck.Checked)
                    richTextBox3.AppendText(text);

                });
                        break;
                    }
                case 4:
                    {
                        richTextBox4.Invoke((MethodInvoker)delegate
                {
                    richTextBox4.Clear();
                    richTextBox4.AppendText(text);

                });
                        break;
                    }
                case 5:
                    {
                        richTextBox5.Invoke((MethodInvoker)delegate
                        {
                            //  richTextBox5.Clear();
                            //  richTextBox5.AppendText(text);
                            if (!BlockCheck.Checked)
                            richTextBox5.Text = text;
                        });
                        break;
                    }
                case 6:
                    {
                        richTextBox6.Invoke((MethodInvoker)delegate
                {
                    richTextBox6.Clear();
                    richTextBox6.AppendText(text);

                });
                        break;
                    }
                case 7:
                    {
                        richTextBox7.Invoke((MethodInvoker)delegate
                        {
                            richTextBox7.Clear();
                            richTextBox7.AppendText(text);

                        });
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server serv = new Server();
            Task n = new Task(() => serv.start());
            n.Start();
            this.button1.Enabled = false;
            uptime.Start();
        }

        private void AutoButton_Click(object sender, EventArgs e)
        {
            try
            {
                AutoIsOn.Checked = !AutoIsOn.Checked;
                auto();
            }
            catch (Exception k)
            {
                Console.WriteLine("ErrinFormsearch:" + k);
            }
        }

        private string formattime(TimeSpan toformat)
        {
            TimeSpan t = toformat;

            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                            t.Hours,
                            t.Minutes,
                            t.Seconds,
                            t.Milliseconds);
            return answer;
        }

        public string getuptime()
        {
            return formattime(uptime.Elapsed);
        }
        private async Task auto()
        {
            string fulltext = "" ;
             Action<object> action = (object obj) =>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            fulltext = "";

            search.ClearTable();

            search.CompareTennisGames(Program.myForm1.GetWilliamsGames(), Program.myForm1.GetBet365());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetMarathon(), Program.myForm1.GetBet365());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetMarathon(), Program.myForm1.GetWilliamsGames());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetUniBet(), Program.myForm1.GetWilliamsGames());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetUniBet(), Program.myForm1.GetBet365());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetUniBet(), Program.myForm1.GetMarathon());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetPariMatchGames(), Program.myForm1.GetMarathon());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetPariMatchGames(), Program.myForm1.GetBet365());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetPariMatchGames(), Program.myForm1.GetWilliamsGames());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetPariMatchGames(), Program.myForm1.GetUniBet());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetFonbetGames(), Program.myForm1.GetUniBet());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetFonbetGames(), Program.myForm1.GetWilliamsGames());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetFonbetGames(), Program.myForm1.GetBet365());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetFonbetGames(), Program.myForm1.GetMarathon());
            fulltext += search.Log;

            search.CompareTennisGames(Program.myForm1.GetFonbetGames(), Program.myForm1.GetPariMatchGames());
            fulltext += search.Log;

            addText(fulltext, 4);
            sw.Stop();

            label1.Invoke((MethodInvoker)delegate
            {
                label1.Text = sw.ElapsedMilliseconds.ToString();
            });
            

        };
            while (AutoIsOn.Checked)
            {
                try
                {
                    await Task.Delay(3000);
                    requestupdate();
                    Task t1 = new Task(action,"alpha");
                    t1.Start();
                    await t1;
                    
                }
                catch
                {

                }
            }
        }

        public void Seat()
        {

        }

        private void ParseBooks_Click(object sender, EventArgs e)
        {
            search.CompareTennisGames(Program.myForm1.GetWilliamsGames(), Program.myForm1.GetBet365());
            addText(search.Log, 4);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormSearch_Load(object sender, EventArgs e)
        {


        }

        private void BlockCheck_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
    class Client
    {

        // Отправка страницы с ошибкой
        private void SendError(TcpClient Client, int Code)
        {
            // Получаем строку вида "200 OK"
            // HttpStatusCode хранит в себе все статус-коды HTTP/1.1
            string CodeStr = Code.ToString() + " " + ((HttpStatusCode)Code).ToString();
            // Код простой HTML-странички
            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            // Необходимые заголовки: ответ сервера, тип и длина содержимого. После двух пустых строк - само содержимое
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            // Приведем строку к виду массива байт
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            // Отправим его клиенту
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            // Закроем соединение
            Client.Close();
        }

        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient Client)
        {
            // Объявим строку, в которой будет хранится запрос клиента
            string Request = "";
            // Буфер для хранения принятых от клиента данных
            byte[] Buffer = new byte[1024];
            // Переменная для хранения количества байт, принятых от клиента
            int Count;
            // Читаем из потока клиента до тех пор, пока от него поступают данные
            try
            {
                while ((Count = Client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    // Преобразуем эти данные в строку и добавим ее к переменной Request
                    Request += Encoding.ASCII.GetString(Buffer, 0, Count);
                    // Запрос должен обрываться последовательностью \r\n\r\n
                    // Либо обрываем прием данных сами, если длина строки Request превышает 4 килобайта
                    // Нам не нужно получать данные из POST-запроса (и т. п.), а обычный запрос
                    // по идее не должен быть больше 4 килобайт
                    if (Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                    {
                        break;
                    }
                }

                // Парсим строку запроса с использованием регулярных выражений
                // При этом отсекаем все переменные GET-запроса
                Match ReqMatch = Regex.Match(Request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");

                // Если запрос не удался
                if (ReqMatch == Match.Empty)
                {
                    // Передаем клиенту ошибку 400 - неверный запрос
                    SendError(Client, 400);
                    return;
                }

                // Получаем строку запроса
                string RequestUri = ReqMatch.Groups[1].Value;

                // Приводим ее к изначальному виду, преобразуя экранированные символы
                // Например, "%20" -> " "
                RequestUri = Uri.UnescapeDataString(RequestUri);

                // Если в строке содержится двоеточие, передадим ошибку 400
                // Это нужно для защиты от URL типа http://example.com/../../file.txt
                if (RequestUri.IndexOf("..") >= 0)
                {
                    SendError(Client, 400);
                    return;
                }

                // Если строка запроса оканчивается на "/", то добавим к ней index.html
                if (RequestUri.EndsWith("/"))
                {
                    RequestUri += "index.html";
                }

                string FilePath = "www/" + RequestUri;

                // Если в папке www не существует данного файла, посылаем ошибку 404
                if (!File.Exists(FilePath))
                {
                    SendError(Client, 404);
                    return;
                }

                // Получаем расширение файла из строки запроса
                string Extension = RequestUri.Substring(RequestUri.LastIndexOf('.'));

                // Тип содержимого
                string ContentType = "";

                // Пытаемся определить тип содержимого по расширению файла
                switch (Extension)
                {
                    case ".htm":
                    case ".html":
                        ContentType = "text/html";
                        break;
                    case ".css":
                        ContentType = "text/stylesheet";
                        break;
                    case ".js":
                        ContentType = "text/javascript";
                        break;
                    case ".jpg":
                        ContentType = "image/jpeg";
                        break;
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                        ContentType = "image/" + Extension.Substring(1);
                        break;
                    default:
                        if (Extension.Length > 1)
                        {
                            ContentType = "application/" + Extension.Substring(1);
                        }
                        else
                        {
                            ContentType = "application/unknown";
                        }
                        break;
                }
                // запись текста в файл
                string textToWrite1 = "";
                string textToWrite2 = "";
                string textToWrite3 = "";
                string textToWrite4 = "";
                string textToWrite5 = "";
                Program.myForm1.SearchForm.richTextBox1.Invoke((MethodInvoker)delegate
                    {
                        foreach (string line in Program.myForm1.SearchForm.richTextBox1.Lines)
                        {
                            if (line == "")
                                textToWrite1 += "<p>" + "&nbsp;" + "</p>";
                            else
                                textToWrite1 += "<p>" + line + "</p>";
                        }
                    });
                Program.myForm1.SearchForm.richTextBox2.Invoke((MethodInvoker)delegate
                {
                    foreach (string line in Program.myForm1.SearchForm.richTextBox2.Lines)
                    {
                        if (line == "")
                            textToWrite2 += "<p>" + "&nbsp;" + "</p>";
                        else
                            textToWrite2 += "<p>" + line + "</p>";
                    }
                });
                Program.myForm1.SearchForm.richTextBox3.Invoke((MethodInvoker)delegate
                {
                    foreach (string line in Program.myForm1.SearchForm.richTextBox3.Lines)
                    {
                        if (line == "")
                            textToWrite3 += "<p>" + "&nbsp;" + "</p>";
                        else
                            textToWrite3 += "<p>" + line + "</p>";
                    }
                });
                Program.myForm1.SearchForm.richTextBox4.Invoke((MethodInvoker)delegate
                {
                    foreach (string line in Program.myForm1.SearchForm.richTextBox4.Lines)
                    {
                        if (line == "")
                            textToWrite4 += "<p>" + "&nbsp;" + "</p>";
                        else
                            textToWrite4 += "<p>" + line + "</p>";
                    }
                });
                Program.myForm1.SearchForm.richTextBox5.Invoke((MethodInvoker)delegate
                {
                    foreach (string line in Program.myForm1.SearchForm.richTextBox5.Lines)
                    {
                        if (line == "")
                            textToWrite5 += "<p>" + "&nbsp;" + "</p>";
                        else
                            textToWrite5 += "<p>" + line + "</p>";
                    }
                });
                string test = @"<html>
<head>
<script>
       setTimeout(function(){
   window.location.reload(1);
}, 5000);
</script>
  <style>
   p {
    margin-top: 0em; /* Отступ сверху */
    margin-bottom: 0em; /* Отступ снизу */
   }
  </style>
</head>
<body>
	<h1>It works!</h1>
    <form>
        <button>test</button>
    </form>" + "<div style=\"float: left;width: 370px;height: 960px;overflow-x:scroll;overflow-y:scroll;\">" + textToWrite1 + "</div>" +
       "<div style=\"float: left;width: 370px;height: 960px;overflow-x:scroll;overflow-y:scroll;\">" + textToWrite2 + "</div>" +
       "<div style=\"float: left;width: 370px;height: 960px;overflow-x:scroll;overflow-y:scroll;\">" + textToWrite3 + "</div>" +
       "<div style=\"float: right;width: 500px;height: 300px;overflow-x:scroll;overflow-y:scroll;\">" + textToWrite4 + "</div>" +
       "<div style=\"float: left;width: 370px;height: 960px;overflow-x:scroll;overflow-y:scroll;\">" + textToWrite5 + "</div>" +
       "<div style=\"float: left;\">" + Program.myForm1.SearchForm.getuptime() + "</div>" +
    @"
</body>
</html>";

                // Посылаем заголовки
                string Headers = "HTTP/1.1 200 OK\nContent-Type: " + ContentType + "\nContent-Length: " + test.Length + "\n\n";
                byte[] HeadersBuffer = Encoding.ASCII.GetBytes(Headers);
                Client.GetStream().Write(HeadersBuffer, 0, HeadersBuffer.Length);

                byte[] bytes = Encoding.ASCII.GetBytes(test);
                Client.GetStream().Write(bytes, 0, bytes.Length);
                Client.Close();
            }
            catch (Exception lol)
            {
                Console.WriteLine("Clnterr:" + lol);
            }
        }
    }

    public class Server
    {
        TcpListener Listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public Server()
        {

        }

        public Server(int Port)
        {
            try
            {
                //Listener = new TcpListener(IPAddress.Parse("192.168.0.105"), Port); // Создаем "слушателя" для указанного порта
                Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), Port); // Создаем "слушателя" для указанного порта
                Listener.Start(); // Запускаем его

                // В бесконечном цикле
                while (true)
                {
                    // Принимаем новых клиентов. После того, как клиент был принят, он передается в новый поток (ClientThread)
                    // с использованием пула потоков.
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Servererr:" + e);
            }
        }

        static void ClientThread(Object StateInfo)
        {
            // Просто создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
            new Client((TcpClient)StateInfo);
        }

        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }

        public void start()
        {
            // Определим нужное максимальное количество потоков
            // Пусть будет по 4 на каждый процессор
            int MaxThreadsCount = Environment.ProcessorCount * 4;
            // Установим максимальное количество рабочих потоков
            ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
            // Установим минимальное количество рабочих потоков
            ThreadPool.SetMinThreads(2, 2);
            // Создадим новый сервер на порту 80
            new Server(80);
        }
    }
}
