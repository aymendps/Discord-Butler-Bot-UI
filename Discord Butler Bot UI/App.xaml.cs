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
        // Singleton
        private static Process? _botProcess = null;
        /// <summary>
        /// Singleton instance of the bot process
        /// </summary>
        public static Process BotProcessInstance
        {
            get
            {
                // If the process is already running, return it
                if (_botProcess != null)
                {
                    Trace.WriteLine("Bot Process already started");
                    return _botProcess;
                }

                // Otherwise, create then start the process
                Trace.WriteLine("Starting Bot Process");
                _botProcess = new Process();
                _botProcess.StartInfo.FileName = ".\\Assets\\start_bot.bat";
                _botProcess.StartInfo.RedirectStandardInput = true;
                _botProcess.StartInfo.RedirectStandardOutput = true;
                _botProcess.StartInfo.RedirectStandardError = true;
                _botProcess.StartInfo.UseShellExecute = false;
                _botProcess.StartInfo.CreateNoWindow = true;

                _botProcess.Start();

                return _botProcess;
            }
        }

        /// <summary>
        /// Kills the bot process
        /// </summary>
        public static void ExitBotProcess()
        {
            if (_botProcess != null && !_botProcess.HasExited)
            {
                Trace.WriteLine("Killing Bot Process");

                // Bot process runs using Node.js, but is run through cmd.exe, so we don't have a direct reference to it
                // Killing process instance even with children included won't kill the node processes
                // Instead, we kill all node processes, which will kill the bot process
                foreach (var node in Process.GetProcessesByName("node"))
                {
                    node.Kill();
                }

                _botProcess.Kill();
                _botProcess = null;
            }
        }

        // Called when the application exits
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Trace.WriteLine("Application Exit");
            ExitBotProcess();
        }
    }
}
