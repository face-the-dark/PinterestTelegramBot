using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PinterestTelegramBot.Service.Scraper
{
    public class JsonParser
    {
        private const string ImagesJsonKey = "images";
        private const string OriginalSizePropertyName = "orig";
        private const string Url = "url";

        public List<JToken> FindAllImages(JToken jsonToken)
        {
            List<JToken> imagesTokens = new List<JToken>();

            if (jsonToken == null)
                return imagesTokens;

            if (jsonToken.Type == JTokenType.Object)
            {
                foreach (JProperty jsonProperty in jsonToken.Children<JProperty>())
                {
                    if (jsonProperty.Name == ImagesJsonKey) 
                        imagesTokens.Add(jsonProperty.Value);

                    imagesTokens.AddRange(FindAllImages(jsonProperty.Value));
                }
            }
            else if (jsonToken.Type == JTokenType.Array)
            {
                foreach (JToken childJsonToken in jsonToken.Children())
                    imagesTokens.AddRange(FindAllImages(childJsonToken));
            }

            return imagesTokens;
        }

        public List<string> FindAllImageUrls(JToken imagesToken)
        {
            List<string> urls = new List<string>();

            if (imagesToken == null)
                return urls;

            foreach (JProperty sizeProperty in imagesToken.Children<JProperty>())
            {
                if (sizeProperty.Name == OriginalSizePropertyName && sizeProperty.Value is JObject jsonObject)
                {
                    string url = jsonObject[Url]?.ToString();

                    if (string.IsNullOrEmpty(url) == false)
                        urls.Add(url);
                }
            }

            return urls;
        }
    }
}