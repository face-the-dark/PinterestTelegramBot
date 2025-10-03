using System.Threading.Tasks;
using PinterestTelegramBot.Config;
using PinterestTelegramBot.Service;
using PinterestTelegramBot.Service.Scraper;

namespace PinterestTelegramBot
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            EnvConfig envConfig = new EnvConfig();
            envConfig.InitializeVariables(out var token, out var channelId, out var pinterestSession);

            TelegramBot telegramBot = new TelegramBot(token);
            PinterestImageScraper pinterestImageScraper = new PinterestImageScraper(pinterestSession);
            
            async Task SendImageJob() => await GetAndSendImage(pinterestImageScraper, telegramBot, channelId);

            using BotScheduler scheduler = new BotScheduler(SendImageJob);
            scheduler.Start();

            await SendImageJob();

            await Task.Delay(-1);
        }

        private static async Task GetAndSendImage
        (
            PinterestImageScraper pinterestImageScraper,
            TelegramBot telegramBot,
            string channelId
        )
        {
            string imageUrl = await pinterestImageScraper.GetImageUrl();
            await telegramBot.SendImage(channelId, imageUrl);
        }
    }
}