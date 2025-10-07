using System;
using DotNetEnv;

namespace PinterestTelegramBot.Config
{
    public class EnvConfig
    {
        private const string EnvPath = "../../../.env";

        private const string TelegramBotTokenKey = "TELEGRAM_BOT_TOKEN";
        private const string ChannelIdKey = "CHANNEL_ID";
        private const string PinterestSessionKey = "PINTEREST_SESSION";
        private const string IsSearchKey = "IS_SEARCH";

        public EnvConfig()
        {
            Env.Load(EnvPath);
            
            TelegramBotToken = GetEnvOrThrowException(TelegramBotTokenKey);
            ChannelId = GetEnvOrThrowException(ChannelIdKey);
            PinterestSession = GetEnvOrThrowException(PinterestSessionKey);
            IsSearch = GetEnvOrThrowException(IsSearchKey);
        }

        public string TelegramBotToken { get; }
        public string ChannelId { get; }
        public string PinterestSession { get; }
        public string IsSearch { get; }
        
        private string GetEnvOrThrowException(string key)
        {
            string value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(value))
                throw new Exception($"Environment variable [{key}] not found");

            return value;
        }
    }
}