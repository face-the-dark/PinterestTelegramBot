using System;
using System.Threading.Tasks;
using System.Timers;

namespace PinterestTelegramBot.Service
{
    public class BotScheduler : IDisposable
    {
        private const int IntervalMinutes = 10;
        private const int SecondsInMinute = 60;
        private const int MillisecondsInSecond = 1000;
        
        private readonly Timer _timer;
        private readonly Func<Task> _job;

        public BotScheduler(Func<Task> job)
        {
            _job = job;

            int interval = IntervalMinutes * SecondsInMinute * MillisecondsInSecond;
            
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