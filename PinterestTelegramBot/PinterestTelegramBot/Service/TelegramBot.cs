using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PinterestTelegramBot.Service
{
    public class TelegramBot
    {
        private const string Caption = "сосал?";

        private readonly TelegramBotClient _botClient;

        public TelegramBot(string token) => 
            _botClient = new TelegramBotClient(token);

        public async Task SendImage(string channelId, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            await _botClient.SendPhoto(
                chatId: channelId,
                photo: InputFile.FromUri(imageUrl),
                caption: Caption
            );
        }
    }
}