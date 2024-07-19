using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGamb
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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm1 = new Form1();
            try
            {
                // Catch any exceptions leaking out of the program CallMainProgramLoop(); 
                Application.Run(myForm1);
            }
            catch (Exception e)
            // We could be catching anything here 
            {
                // The exception we caught could have been a program error
                // or something much more serious. Regardless, we know that
                // something is not right. We'll just output the exception 
                // and exit with an error. We won't try to do any work when
                // the program or process is in an unknown state!

                System.Console.WriteLine(e.Message);
            }
        }
    }
}
