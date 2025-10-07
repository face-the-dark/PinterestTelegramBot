using System;
using System.Threading.Tasks;

namespace PinterestTelegramBot.Service;

public class Publisher
{
    private readonly CacheService _cacheService;
    private readonly TelegramBot _telegramBot;
    private readonly string _channelId;

    public Publisher(CacheService cacheService, TelegramBot telegramBot, string channelId)
    {
        _cacheService = cacheService;
        _telegramBot = telegramBot;
        _channelId = channelId;
    }

    public async Task SendImage()
    {
        string imageUrl = await _cacheService.GetRandomImageUrl();

        if (string.IsNullOrEmpty(imageUrl))
            throw new Exception("Image not found");

        await _telegramBot.SendImage(_channelId, imageUrl);
    }
}