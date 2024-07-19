using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGambUniverse
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        public static Form1 myForm1;
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm1 = new Form1();
            try
            {

                // Catch any exceptions leaking out of the program CallMainProgramLoop(); 
                if (args.Length > 0 && args[0] == "auto")
                {
                    //myForm1.setAutoupdate();
                    myForm1.setcheck1();
                   // myForm1.Form1Auto();
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
                //Application.Restart();
            }
        }
    }
}
