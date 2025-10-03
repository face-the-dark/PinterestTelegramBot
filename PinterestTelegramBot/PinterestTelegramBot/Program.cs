using System;
using System.IO;
using System.Threading.Tasks;
using DotNetEnv;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PinterestTelegramBot
{
    internal class Program
    {
        private const string EnvPath = "../../../.env";
        private const string TelegramBotToken = "TELEGRAM_BOT_TOKEN";

        public static async Task Main(string[] args)
        {
            Env.Load(EnvPath);
            
            string token = Environment.GetEnvironmentVariable(TelegramBotToken);

            if (token == null)
            {
                Console.WriteLine($"Environment variable [{TelegramBotToken}] not found.");
                
                return;
            }
            
            TelegramBotClient bot = new TelegramBotClient(token);
            User user = await bot.GetMe();
            
            Console.WriteLine($"Hello, World! I am user {user.Id} and my name is {user.FirstName}.");
        }
    }
}