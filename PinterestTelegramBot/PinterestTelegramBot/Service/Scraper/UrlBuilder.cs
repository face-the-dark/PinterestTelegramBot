using System;

namespace PinterestTelegramBot.Service.Scraper
{
    public class UrlBuilder
    {
        private const string HomeUrl = "https://ru.pinterest.com/";
        private const string SearchUrl = "https://www.pinterest.com/search/pins/?q=";
        private const string SearchKeyword = "tyan";

        private bool _isSearch;

        public UrlBuilder(bool isSearch) =>
            _isSearch = isSearch;

        public string Build()
        {
            string url = HomeUrl;

            if (_isSearch)
                url = $"{SearchUrl}{Uri.EscapeDataString(SearchKeyword)}";

            return url;
        }
    }
}