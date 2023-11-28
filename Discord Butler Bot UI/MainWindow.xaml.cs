using Discord_Butler_Bot_UI.BotEvents;
using Discord_Butler_Bot_UI.UserControls;
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

            // Subscribe to bot events
            var instance = (App)Application.Current;
            instance.OnBotEvent += HandleBotEvent;
        }

        // Handles bot events
        private void HandleBotEvent(BotEvent botEvent)
        {
            switch(botEvent)
            {
                case BotEvent.Online:
                    // Update the UI to reflect the bot being online
                    this.Dispatcher.Invoke(() =>
                    {
                        StatusLoading.Visibility = Visibility.Hidden;
                        StatusOnline.Visibility = Visibility.Visible;

                        RunningTimer.Start();

                        StartBotButton.Content = "Stop";
                        StartBotButton.Click -= StartBotClick;
                        StartBotButton.Click += StopBotClick;
                        StartBotButton.IsEnabled = true;

                        LogPanelPlaceholder.Text = "Listening for bot logs...";
                    });
                    break;

                case BotEvent.JoinedChannel:
                case BotEvent.LeftChannel:
                    this.Dispatcher.Invoke(() =>
                    {
                        LogPanelPlaceholder.Visibility = Visibility.Collapsed;

                        // Maybe builder pattern in the future?
                        var log = new BotLog() { StatusColor = BotEventManager.BotEventToBrush(botEvent) };
                        log.LogContent.Text = botEvent == BotEvent.JoinedChannel ? "Joined a voice channel" : "Left a voice channel";

                        LogPanel.Children.Add(log);
                        LogPanelScrollViewer.ScrollToBottom();
                    });
                    break;

                case BotEvent.AddedSong:
                    this.Dispatcher.Invoke(() => 
                    {
                        LogPanelPlaceholder.Visibility = Visibility.Collapsed;

                        // Maybe builder pattern in the future?
                        var log = new BotLog() { StatusColor = BotEventManager.BotEventToBrush(botEvent) };
                        log.LogContent.Inlines.Add(new Run("Added "));
                        log.LogContent.Inlines.Add(new Run("Some Song") { Foreground = FindResource("PrimaryColor") as SolidColorBrush});
                        log.LogContent.Inlines.Add(new Run(" to the queue"));
                        
                        LogPanel.Children.Add(log);
                        LogPanelScrollViewer.ScrollToBottom();
                    });
                    break;

                case BotEvent.PlayingSong:
                    this.Dispatcher.Invoke(() =>
                    {
                        LogPanelPlaceholder.Visibility = Visibility.Collapsed;

                        // Maybe builder pattern in the future?
                        var log = new BotLog() { StatusColor = BotEventManager.BotEventToBrush(botEvent) };
                        log.LogContent.Inlines.Add(new Run("Played "));
                        log.LogContent.Inlines.Add(new Run("Some Song") { Foreground = FindResource("PrimaryColor") as SolidColorBrush });
                        log.LogContent.Inlines.Add(new Run(" from the queue"));

                        LogPanel.Children.Add(log);
                        LogPanelScrollViewer.ScrollToBottom();
                    });
                    break;

                case BotEvent.SkippedSong:
                    this.Dispatcher.Invoke(() =>
                    {
                        LogPanelPlaceholder.Visibility = Visibility.Collapsed;

                        // Maybe builder pattern in the future?
                        var log = new BotLog() { StatusColor = BotEventManager.BotEventToBrush(botEvent) };
                        log.LogContent.Inlines.Add(new Run("Skipped "));
                        log.LogContent.Inlines.Add(new Run("Some Song") { Foreground = FindResource("PrimaryColor") as SolidColorBrush });

                        LogPanel.Children.Add(log);
                        LogPanelScrollViewer.ScrollToBottom();
                    });
                    break;
            }
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
            var instance = (App)Application.Current;
            instance.StartBotProcess();
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
            var instance = (App)Application.Current;
            instance.StopBotProcess();

            // Reset the UI after a few seconds
            await Task.Delay(5000);

            StatusLoading.Visibility = Visibility.Hidden;
            StatusOffline.Visibility = Visibility.Visible;

            RunningTimer.Stop();

            StartBotButton.Content = "Start";
            StartBotButton.Click -= StopBotClick;
            StartBotButton.Click += StartBotClick;
            StartBotButton.IsEnabled = true;

            LogPanelPlaceholder.Text = "Logs will appear here when the bot is running.";
        }
    }
}
