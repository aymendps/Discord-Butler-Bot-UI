using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Discord_Butler_Bot_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Process? BotProcess = null;

        public static Process GetBotProcess()
        {
            if (BotProcess != null)
            {
                Trace.WriteLine("Bot Process already started");
                return BotProcess;
            }

            Trace.WriteLine("Starting Bot Process");
            BotProcess = new Process();
            BotProcess.StartInfo.FileName = ".\\assets\\start_bot.bat";
            BotProcess.StartInfo.RedirectStandardInput = true;
            BotProcess.StartInfo.RedirectStandardOutput = true;
            BotProcess.StartInfo.RedirectStandardError = true;
            BotProcess.StartInfo.UseShellExecute = false;
            BotProcess.StartInfo.CreateNoWindow = true;
         
            BotProcess.Start();

            return BotProcess;
        }

        public static void BotProcessExit()
        {
            if (BotProcess != null && !BotProcess.HasExited)
            {
                Trace.WriteLine("Killing Bot Process");

                foreach (var node in Process.GetProcessesByName("node"))
                {
                    node.Kill();
                }

                BotProcess.Kill();
                BotProcess = null;
            }
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            BotProcessExit();
        }
    }
}
