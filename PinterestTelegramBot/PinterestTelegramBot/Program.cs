using System;
using System.Threading.Tasks;
using PinterestTelegramBot.Config;
using PinterestTelegramBot.Service;
using PinterestTelegramBot.Service.Scraper;

namespace PinterestTelegramBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            EnvConfig envConfig = new EnvConfig();
            envConfig.InitializeVariables(out string token, out string channelId, out string pinterestSession,
                out string isSearch);

            TelegramBot telegramBot = new TelegramBot(token);
            PinterestImageScraper pinterestImageScraper =
                new PinterestImageScraper(bool.Parse(isSearch), pinterestSession);
            CacheService cacheService = new CacheService(pinterestImageScraper);

            async Task SendImageJob() => await GetAndSendImage(cacheService, telegramBot, channelId);

            using BotScheduler scheduler = new BotScheduler(SendImageJob);
            scheduler.Start();

            await SendImageJob();

            await Task.Delay(-1);
        }

        private static async Task GetAndSendImage
        (
            CacheService cacheService,
            TelegramBot telegramBot,
            string channelId
        )
        {
            string imageUrl = await cacheService.GetRandomImageUrl();

            if (string.IsNullOrEmpty(imageUrl))
                throw new Exception("Image not found");

            await telegramBot.SendImage(channelId, imageUrl);
        }
    }
}