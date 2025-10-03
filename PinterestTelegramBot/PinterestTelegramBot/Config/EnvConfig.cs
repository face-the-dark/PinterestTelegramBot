using System;
using DotNetEnv;

namespace PinterestTelegramBot.Config
{
    public class EnvConfig
    {
        private const string EnvPath = "../../../.env";
        
        private const string TelegramBotToken = "TELEGRAM_BOT_TOKEN";
        private const string ChannelId = "CHANNEL_ID";
        private const string PinterestSession = "PINTEREST_SESSION";
    
        public void InitializeVariables
        (
            out string telegramBotToken,
            out string channelId,
            out string pinterestSession
        )
        {
            Env.Load(EnvPath);

            telegramBotToken = GetEnvOrThrowException(TelegramBotToken);
            channelId = GetEnvOrThrowException(ChannelId);
            pinterestSession = GetEnvOrThrowException(PinterestSession);
        }
    
        private string GetEnvOrThrowException(string key)
        {
            string value = Environment.GetEnvironmentVariable(key);
        
            if (string.IsNullOrEmpty(value))
                throw new Exception($"Environment variable [{key}] not found");
        
            return value;
        }
    }
}