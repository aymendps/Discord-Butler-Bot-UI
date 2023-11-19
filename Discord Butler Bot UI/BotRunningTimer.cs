using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Discord_Butler_Bot_UI
{
    internal class BotRunningTimer
    {
        private DispatcherTimer _timer;
        private int _secondsRunning = 0;
        private int _minutesRunning = 0;
        private int _hoursRunning = 0;
        private TextBlock _timerText;

        public BotRunningTimer(TextBlock timerText)
        {
            _timerText = timerText;
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(UpdateBotRunningTime);
            _timer.Interval = new TimeSpan(0, 0, 1);
        }

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
                _hoursRunning++;
            }
            _timerText.Text = $"{_hoursRunning.ToString("00")}:{_minutesRunning.ToString("00")}:{_secondsRunning.ToString("00")}";
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _secondsRunning = 0;
            _minutesRunning = 0;
            _hoursRunning = 0;
            _timerText.Text = "00:00:00";
        }
    }
}
