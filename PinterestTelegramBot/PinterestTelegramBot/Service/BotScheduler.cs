using System;
using System.Threading.Tasks;
using System.Timers;
using PinterestTelegramBot.Utils;

namespace PinterestTelegramBot.Service
{
    public class BotScheduler : IDisposable
    {
        private const int MinIntervalMinutes = 7;
        private const int MaxIntervalMinutes = 15;
        private const int SecondsInMinute = 60;
        private const int MillisecondsInSecond = 1000;
        
        private readonly Timer _timer;
        private readonly Func<Task> _job;

        public BotScheduler(Func<Task> job)
        {
            _job = job;

            int intervalMinutes = RandomUtils.Next(MinIntervalMinutes, MaxIntervalMinutes);
            int interval = intervalMinutes * SecondsInMinute * MillisecondsInSecond;
            
            _timer = new Timer(interval);
            _timer.Elapsed += async (s, e) => await ExecuteJob();
            _timer.AutoReset = true;
        }

        public void Start() => 
            _timer.Start();

        public void Stop() => 
            _timer.Stop();

        public void Dispose() => 
            _timer?.Dispose();

        private async Task ExecuteJob() => 
            await _job();
    }
}