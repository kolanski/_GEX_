using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace ProjectGambAwesomium
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        public static Form1 myForm1;
        [STAThread]
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm1 = new Form1();
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            //sett.UserAgent = "Mozilla/5.0 (Linux; Android 4.1.1; Nexus 7 Build/JRO03D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166  Safari/535.19";
            //sett.CachePath = appPath+"\\Cache";

            try
            {
                
                // Catch any exceptions leaking out of the program CallMainProgramLoop(); 
                if (args.Length>0&&args[0] == "auto")
                {
                    Debug.WriteLine("AUTOREADY");
                    myForm1.Form1Auto();
                }
                Application.Run(myForm1);
                
            }
            catch (System.Exception e)
            // We could be catching anything here 
            {
                // The exception we caught could have been a program error
                // or something much more serious. Regardless, we know that
                // something is not right. We'll just output the exception 
                // and exit with an error. We won't try to do any work when
                // the program or process is in an unknown state!

                Debug.WriteLine(e.Message);
            }
        }
    }
}
