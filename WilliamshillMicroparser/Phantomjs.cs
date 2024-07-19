using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WilliamshillMicroparser
{
    class Phantomjs
    {
        System.Diagnostics.Process process;

        public Phantomjs()
        {
            Config.PhantomJSPath = Directory.GetCurrentDirectory() + "\\phantomjs.exe";
        }
        public static class Config
        {
            public static string PhantomJSPath { get; internal set; }
        }
        public string Title()
        {
            process.StandardInput.WriteLine("get title");
            process.StandardInput.Flush();
            
            return process.StandardOutput.ReadToEnd();
        }
        public string Grab(string url)
        {
            process = new System.Diagnostics.Process();

            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = Config.PhantomJSPath,
                Arguments = string.Format("\"{0}\\{1}\" {2}", Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "index.js", url)
            };
            process.Exited += Process_Exited;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            //process.WaitForExit(1000);

            return output;
        }

        private void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine(e);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("Process exited");
        }
    }
}
