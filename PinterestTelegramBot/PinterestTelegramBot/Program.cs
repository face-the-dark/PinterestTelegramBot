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
            TelegramBot telegramBot = new TelegramBot(envConfig.TelegramBotToken);
            UrlBuilder urlBuilder = new UrlBuilder(bool.Parse(envConfig.IsSearch));
            PinterestImageScraper pinterestImageScraper = new PinterestImageScraper(urlBuilder, envConfig.PinterestSession);
            CacheService cacheService = new CacheService(pinterestImageScraper);
            Publisher publisher = new Publisher(cacheService, telegramBot, envConfig.ChannelId);

            async Task SendImageJob() => await publisher.SendImage();

            using BotScheduler scheduler = new BotScheduler(SendImageJob);
            scheduler.Start();

            await SendImageJob();

            await Task.Delay(-1);
        }
    }
}