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
            this.Dispatcher.Invoke(() =>
            {
                StartBot.IsEnabled = false;
                StartBot.Content = "Starting...";
            });

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
                StartBot.IsEnabled = true;
                StartBot.Click -= StartBotClick;
            });
        }

        private void StartBotClick(object sender, RoutedEventArgs e)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(StartBotWorker);
            worker.RunWorkerAsync();
        }


    }
}
