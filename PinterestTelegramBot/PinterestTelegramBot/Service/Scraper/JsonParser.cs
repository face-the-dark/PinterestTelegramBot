using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PinterestTelegramBot.Service.Scraper
{
    public class JsonParser
    {
        private const string RootKey = "initialReduxState";
        private const string ImagesJsonKey = "images";
        private const string OriginalSizePropertyName = "orig";
        private const string Url = "url";

        public List<string> GetAllImageUrls(string jsonContentText)
        {
            List<string> allImageUrls = new List<string>();
            
            JObject jsonObject = JObject.Parse(jsonContentText);
            JToken rootJsonToken = jsonObject[RootKey];
            List<JToken> imagesList = ParseImageTokens(rootJsonToken);

            foreach (JToken imageJsonToken in imagesList)
                allImageUrls.AddRange(GetAllImageUrls(imageJsonToken));
            
            return allImageUrls;
        }

        private List<JToken> ParseImageTokens(JToken jsonToken)
        {
            List<JToken> imageTokens = new List<JToken>();

            if (jsonToken == null)
                return imageTokens;

            if (jsonToken.Type == JTokenType.Object)
            {
                foreach (JProperty jsonProperty in jsonToken.Children<JProperty>())
                {
                    if (jsonProperty.Name == ImagesJsonKey)
                        imageTokens.Add(jsonProperty.Value);

                    imageTokens.AddRange(ParseImageTokens(jsonProperty.Value));
                }
            }
            else if (jsonToken.Type == JTokenType.Array)
            {
                foreach (JToken childJsonToken in jsonToken.Children())
                    imageTokens.AddRange(ParseImageTokens(childJsonToken));
            }

            return imageTokens;
        }

        private List<string> GetAllImageUrls(JToken imagesToken)
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