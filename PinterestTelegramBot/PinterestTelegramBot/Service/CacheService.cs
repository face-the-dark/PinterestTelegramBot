using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PinterestTelegramBot.Service.Scraper;

namespace PinterestTelegramBot.Service;

public class CacheService
{
    private readonly PinterestImageScraper _pinterestImageScraper;

    private HashSet<string> _allImageUrls;
    
    public CacheService(PinterestImageScraper pinterestImageScraper)
    {
        _pinterestImageScraper = pinterestImageScraper;
        _allImageUrls = new HashSet<string>();
    }

    public async Task<string> GetRandomImageUrl()
    {
        if (_allImageUrls.Count <= 0)
            await Update();
        
        string randomImageUrl = null;

        if (_allImageUrls.Count > 0)
        {
            randomImageUrl = _allImageUrls.First();
            _allImageUrls.Remove(randomImageUrl);
        }
        
        return randomImageUrl;
    }

    private async Task Update() => 
        _allImageUrls = await _pinterestImageScraper.GetAllImageUrls();
}