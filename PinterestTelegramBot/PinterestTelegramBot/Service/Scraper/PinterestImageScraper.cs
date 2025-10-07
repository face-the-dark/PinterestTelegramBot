using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PinterestTelegramBot.Service.Scraper
{
    public class PinterestImageScraper
    {
        private const string UserAgent = "Mozilla/5.0";
        private const string CookieName = "Cookie";
        private const string PinterestSessionCookie = "_pinterest_sess=";

        private const string IdName = "id";
        private const string ContentId = "__PWS_INITIAL_PROPS__";

        private readonly JsonParser _jsonParser;
        private readonly HttpClient _httpClient;
        private readonly UrlBuilder _urlBuilder;

        public PinterestImageScraper(UrlBuilder urlBuilder, string pinterestSession)
        {
            _jsonParser = new JsonParser();
            _httpClient = new HttpClient();
            _urlBuilder = urlBuilder;
            
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            _httpClient.DefaultRequestHeaders.Add(CookieName, $"{PinterestSessionCookie}{pinterestSession}");
        }

        public async Task<HashSet<string>> GetAllImageUrls()
        {
            string html = await GetHtml();
            
            return ScrapAllImageUrls(html);
        }

        private async Task<string> GetHtml()
        {
            string url = _urlBuilder.Build();

            return await _httpClient.GetStringAsync(url);
        }

        private HashSet<string> ScrapAllImageUrls(string html)
        {
            HashSet<string> allImageUrls = new HashSet<string>();
            
            string jsonContentText = GetJsonContentText(html);

            if (string.IsNullOrEmpty(jsonContentText) == false)
            {
                List<string> imageUrls = _jsonParser.GetAllImageUrls(jsonContentText);
                allImageUrls.UnionWith(imageUrls);
            }
            
            return allImageUrls;
        }

        private string GetJsonContentText(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            HtmlNode contentNode = htmlDocument.DocumentNode.Descendants()
                .FirstOrDefault(n => n.GetAttributeValue(IdName, string.Empty) == ContentId);

            return contentNode?.InnerText;
        }
    }
}