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
        private BotRunningTimer _botRunningTimer;
        public MainWindow()
        {
            InitializeComponent();
            _botRunningTimer = new BotRunningTimer(RunningTimeText);
        }

        private void StartBotWorker(object? sender, DoWorkEventArgs e)
        {
            var process = App.GetBotProcess();

            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();

                if(BotEventManager.IsBotEvent(line, BotEvent.Started))
                {
                    Trace.WriteLine(line);
                    break;
                }
            }

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

        private void StartBotClick(object sender, RoutedEventArgs e)
        {
            StartBotButton.IsEnabled = false;
            StartBotButton.Content = "Starting...";
            StatusOffline.Visibility = Visibility.Hidden;
            StatusLoading.Visibility = Visibility.Visible;

            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(StartBotWorker);
            worker.RunWorkerAsync();
        }

        private async void StopBotClick(object sender, RoutedEventArgs e)
        {
            StartBotButton.IsEnabled = false;
            StartBotButton.Content = "Stopping...";
            StatusOnline.Visibility = Visibility.Hidden;
            StatusLoading.Visibility = Visibility.Visible;

            App.BotProcessExit();

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
