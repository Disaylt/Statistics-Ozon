using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace Statistics_Ozon
{
    public static class OzonApiClient
    {
        public static string GetOrdersFBO(DateTime ordersDate)
        {
            string bodyJson = JsonHandler.GetBodyRequestOrdersFBO(ordersDate);
            string url = "https://api-seller.ozon.ru/v2/posting/fbo/list";
            var webClient = new WebClient();
            webClient.Headers["Client-Id"] = JsonHandler.JsonDocumentTokens.OzonClientId;
            webClient.Headers["Api-Key"] = JsonHandler.JsonDocumentTokens.OzonApiKey;
            webClient.Encoding = Encoding.UTF8;
            string response = webClient.UploadString(url, "POST", bodyJson);
            return response;
        }

        public static string GetMedia(DateTime dateTime, string media)
        {
            (string url, string body) = GetMediaDataForRequest(dateTime, media);
            WebClient webClient = GetOzonMediaWebClient();
            string response = webClient.UploadString(url, "POST", body);
            Thread.Sleep(1 * 1000);
            return response;
        }

        private static (string url, string body) GetMediaDataForRequest(DateTime dateTime, string media)
        {
            switch(media)
            {
                case "company":
                    return ("https://performance.ozon.ru/api/adv/media_campaign", JsonHandler.GetBodyMediaCompay(dateTime));
                case "products":
                    return ("https://performance.ozon.ru/api/adv/product_campaign", JsonHandler.GetBpdyMediaProducts(dateTime));
                case "promo":
                    return ("https://performance.ozon.ru/api/adv/search_promo_campaign", JsonHandler.GetBpdyMediaProducts(dateTime));
                default:
                    throw new Exception($"{nameof(GetMediaDataForRequest)} - неизвестное медиа.");
            }
        }

        private static WebClient GetOzonMediaWebClient()
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers["Accept"] = "application/json, text/plain, */*";
            webClient.Headers["Content-Type"] = "application/json;charset=utf-8";
            webClient.Headers["Host"] = "performance.ozon.ru";
            webClient.Headers["Referer"] = "https://seller.ozon.ru/";
            webClient.Headers["Origin"] = "https://seller.ozon.ru";
            webClient.Headers["Sec-Fetch-Dest"] = "empty";
            webClient.Headers["Sec-Fetch-Mode"] = "cors";
            webClient.Headers["Sec-Fetch-Site"] = "same-site";
            webClient.Headers["TE"] = "trailers";
            webClient.Headers["x-o3-company-id"] = "96202";
            webClient.Headers["token"] = JsonHandler.JsonDocumentTokens.OzonGreyToken;
            webClient.Headers["x-o3-app-name"] = "performance";
            webClient.Headers["x-o3-adv-current-organisation"] = "53563";
            webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0";
            return webClient;
        }
    }
}
