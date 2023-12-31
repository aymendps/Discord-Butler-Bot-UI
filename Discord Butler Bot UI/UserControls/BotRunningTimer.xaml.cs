﻿using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Discord_Butler_Bot_UI.UserControls
{
    /// <summary>
    /// Interaction logic for BotRunningTimer.xaml
    /// </summary>
    public partial class BotRunningTimer : UserControl
    {
        private readonly DispatcherTimer _timer;
        private int _secondsRunning = 0;
        private int _minutesRunning = 0;
        private int _hoursRunning = 0;

        public BotRunningTimer()
        {
            InitializeComponent();

            // Create a timer with a one second interval.
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(UpdateBotRunningTime);
            _timer.Interval = new TimeSpan(0, 0, 1);
        }

        // Increases the running time by one second then updates the timer text
        private void UpdateBotRunningTime(object? sender, EventArgs e)
        {
            _secondsRunning++;
            if (_secondsRunning >= 60)
            {
                _secondsRunning = 0;
                _minutesRunning++;
            }
            if (_minutesRunning >= 60)
            {
                _minutesRunning = 0;
                if (_hoursRunning < 99) _hoursRunning++;
            }
            RunningTimeText.Text = $"{_hoursRunning:00}:{_minutesRunning:00}:{_secondsRunning:00}";
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// Stops the timer and resets the running time
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            _secondsRunning = 0;
            _minutesRunning = 0;
            _hoursRunning = 0;
            RunningTimeText.Text = "00:00:00";
        }
    }
}
