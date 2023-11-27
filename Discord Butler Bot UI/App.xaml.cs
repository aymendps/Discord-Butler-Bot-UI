using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Process? _botProcess = null;
        private bool _botProcessIsRunning = false;

        /// <summary>
        /// Event that is invoked when a bot event occurs
        /// </summary>
        public event Action<BotEvent> OnBotEvent;

        private void InvokeOnBotEvent(BotEvent botEvent)
        {
            OnBotEvent?.Invoke(botEvent);
        }

        // Background worker that runs the bot process and listens for its events
        private void BotProcessBackgroundWorker(object? sender, DoWorkEventArgs e)
        {
            Trace.WriteLine("Starting Bot Process");

            // Create process
            _botProcess = new Process();
            _botProcess.StartInfo.FileName = ".\\Assets\\start_bot.bat";
            _botProcess.StartInfo.RedirectStandardInput = true;
            _botProcess.StartInfo.RedirectStandardOutput = true;
            _botProcess.StartInfo.RedirectStandardError = true;
            _botProcess.StartInfo.UseShellExecute = false;
            _botProcess.StartInfo.CreateNoWindow = true;

            _botProcess.Start();

            // When this is set back to false, the background worker will stop
            _botProcessIsRunning = true;

            // Listen for bot events
            while (_botProcessIsRunning && !_botProcess.StandardOutput.EndOfStream)
            {
                var line = _botProcess.StandardOutput.ReadLine();
                var currentBotEvent = BotEventManager.GetBotEvent(line);

                if(currentBotEvent != BotEvent.None)
                {
                    InvokeOnBotEvent(currentBotEvent);
                }
            }

            Trace.WriteLine("Stopped Bot Process Background Worker");
        }

        /// <summary>
        /// Starts the bot process in the background
        /// </summary>
        public void StartBotProcess()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(BotProcessBackgroundWorker);
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Kills the bot process
        /// </summary>
        public void StopBotProcess()
        {
            // Setting this to false will stop the bot process background worker
            _botProcessIsRunning = false;

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
            StopBotProcess();
        }
    }
}
