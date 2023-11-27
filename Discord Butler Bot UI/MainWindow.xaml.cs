using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord_Butler_Bot_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BotRunningTimer _botRunningTimer;
        public MainWindow()
        {
            InitializeComponent();
            _botRunningTimer = new BotRunningTimer(RunningTimeText);
        }

        // Starts the bot process and waits for it to become online
        private void StartBotWorker(object? sender, DoWorkEventArgs e)
        {
            var process = App.BotProcessInstance;

            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();

                if(BotEventManager.IsBotEvent(line, BotEvent.Online))
                {
                    Trace.WriteLine(line);
                    break;
                }
            }

            // Update the UI to reflect the bot being online
            this.Dispatcher.Invoke(() =>
            {
                StatusLoading.Visibility = Visibility.Hidden;
                StatusOnline.Visibility = Visibility.Visible;

                _botRunningTimer.Start();

                StartBotButton.Content = "Stop";
                StartBotButton.Click -= StartBotClick;
                StartBotButton.Click += StopBotClick;
                StartBotButton.IsEnabled = true;
            });
        }

        // Handles clicking the start bot button
        private void StartBotClick(object sender, RoutedEventArgs e)
        {
            // Update the UI to reflect the bot starting
            StartBotButton.IsEnabled = false;
            StartBotButton.Content = "Starting...";
            StatusOffline.Visibility = Visibility.Hidden;
            StatusLoading.Visibility = Visibility.Visible;

            // Start the bot process
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(StartBotWorker);
            worker.RunWorkerAsync();
        }

        // Handles clicking the stop bot button
        private async void StopBotClick(object sender, RoutedEventArgs e)
        {
            // Update the UI to reflect the bot stopping
            StartBotButton.IsEnabled = false;
            StartBotButton.Content = "Stopping...";
            StatusOnline.Visibility = Visibility.Hidden;
            StatusLoading.Visibility = Visibility.Visible;

            // Stop the bot process
            App.ExitBotProcess();

            // Reset the UI after a few seconds
            await Task.Delay(5000);

            StatusLoading.Visibility = Visibility.Hidden;
            StatusOffline.Visibility = Visibility.Visible;

            _botRunningTimer.Stop();

            StartBotButton.Content = "Start";
            StartBotButton.Click -= StopBotClick;
            StartBotButton.Click += StartBotClick;
            StartBotButton.IsEnabled = true;
        }
    }
}
