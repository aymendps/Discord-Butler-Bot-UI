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
        public MainWindow()
        {
            InitializeComponent();
        }

        void StartBotWorker(object? sender, DoWorkEventArgs e)
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
                StartBot.Content = "Stop";
                StartBot.Click -= StartBotClick;
                StartBot.Click += StopBotClick;
                StartBot.IsEnabled = true;
            });
        }

        private void StartBotClick(object sender, RoutedEventArgs e)
        {
            StartBot.IsEnabled = false;
            StartBot.Content = "Starting...";

            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(StartBotWorker);
            worker.RunWorkerAsync();
        }

        private async void StopBotClick(object sender, RoutedEventArgs e)
        {
            StartBot.IsEnabled = false;
            StartBot.Content = "Stopping...";

            App.BotProcessExit();

            await Task.Delay(5000);

            StartBot.Content = "Start";
            StartBot.Click -= StopBotClick;
            StartBot.Click += StartBotClick;
            StartBot.IsEnabled = true;
        }
    }
}
