using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PinterestTelegramBot.Utils;

namespace PinterestTelegramBot.Service.Scraper
{
    public class PinterestImageScraper
    {
        private const string UserAgent = "Mozilla/5.0";
        private const string CookieName = "Cookie";
        private const string PinterestSessionCookie = "_pinterest_sess=";

        private const string SearchUrl = "https://www.pinterest.com/search/pins/?q=";
        private const string SearchKeyword = "tyan";

        private const string IdName = "id";
        private const string ContentId = "__PWS_INITIAL_PROPS__";

        private readonly JsonParser _jsonParser;
        private readonly HttpClient _httpClient;

        public PinterestImageScraper(string pinterestSession)
        {
            _jsonParser = new JsonParser();
            _httpClient = new HttpClient();
            
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            _httpClient.DefaultRequestHeaders.Add(CookieName, $"{PinterestSessionCookie}{pinterestSession}");
        }

        public async Task<string> GetImageUrl()
        {
            string html = await GetHtml();
            List<string> allImageUrls = GetAllImageUrls(html);

            if (allImageUrls.Count > 0)
                return allImageUrls[RandomUtils.Next(allImageUrls.Count)];

            return null;
        }

        private async Task<string> GetHtml()
        {
            string url = $"{SearchUrl}{Uri.EscapeDataString(SearchKeyword)}";
            
            return await _httpClient.GetStringAsync(url);
        }

        private List<string> GetAllImageUrls(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            HtmlNode contentNode = htmlDocument.DocumentNode.Descendants()
                .FirstOrDefault(n => n.GetAttributeValue(IdName, string.Empty) == ContentId);

            string jsonContentText = contentNode?.InnerText;

            List<string> allImageUrls = new List<string>();
            
            if (string.IsNullOrEmpty(jsonContentText) == false)
            {
                List<string> imageUrls = _jsonParser.GetAllImageUrls(jsonContentText);
                allImageUrls.AddRange(imageUrls);
            }
            
            return allImageUrls;
        }
    }
}