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
        private const string IsSearch = "IS_SEARCH";
    
        public void InitializeVariables
        (
            out string telegramBotToken,
            out string channelId,
            out string pinterestSession,
            out string isSearch
        )
        {
            Env.Load(EnvPath);

            telegramBotToken = GetEnvOrThrowException(TelegramBotToken);
            channelId = GetEnvOrThrowException(ChannelId);
            pinterestSession = GetEnvOrThrowException(PinterestSession);
            isSearch = GetEnvOrThrowException(IsSearch);
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