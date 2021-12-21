using System.Configuration;
using System.Diagnostics;

namespace EasySave_3.Models
{
    public class ProcessManagement
    {

        public static Process Init_CryptoSoft(string Source, string Destination)
        {
            string CryptoSoft = ConfigurationManager.AppSettings.Get("CryptoSoft");

            Process Process = new Process();
            Process.StartInfo.FileName = CryptoSoft;
            Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.ArgumentList.Add("Crypt");
            Process.StartInfo.ArgumentList.Add(Source);
            Process.StartInfo.ArgumentList.Add(Destination);

            return Process;
        }


        public static void Launch_CryptoSoft(Process Process)
        {
            Process.Start();
        }

        public static void OpenLogFile()
        {
            Process P = new Process();

            P.StartInfo = new ProcessStartInfo(ConfigurationManager.AppSettings.Get("LogFile"))
            {
                UseShellExecute = true
            };

            P.Start();
        }

        public static void OpenStateFile()
        {
            Process P = new Process();

            P.StartInfo = new ProcessStartInfo(ConfigurationManager.AppSettings.Get("StateFile"))
            {
                UseShellExecute = true
            };

            P.Start();
        }

        public static void OpenSettigsFile()
        {
            Process P = new Process();

            P.StartInfo = new ProcessStartInfo(ConfigurationManager.AppSettings.Get("Settings"))
            {
                UseShellExecute = true
            };

            P.Start();
        }


    }
}
