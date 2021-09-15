using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Statistics_Ozon.JsonDocuments;
using System.IO;

namespace Statistics_Ozon
{
    public static class JsonHandler
    {
        private const string _ozonFormatDate = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
        private const string _ozonFullFormaDate = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ";
        private static JsonDocumentTokens _jsonDocumentTokens;
        private static List<List<object>> _jsonDocumentCostPrices;

        public static JsonDocumentTokens JsonDocumentTokens
        {
            get
            {
                if(_jsonDocumentTokens == null)
                {
                    _jsonDocumentTokens = GetJsonDocumentTokens();
                }
                return _jsonDocumentTokens;
            }
        }
        public static List<List<object>> JsonDocumentCostPrices
        {
            get
            {
                if(_jsonDocumentCostPrices == null)
                {
                    _jsonDocumentCostPrices = GetCostPrices();
                }
                return _jsonDocumentCostPrices;
            }
        }


        public static List<JsonDocumentOrdersFBO> ConvertToListJsonDocumentOrdersFBO(string ordersFBO)
        {
            JToken jToken = JToken.Parse(ordersFBO)["result"];
            List<JsonDocumentOrdersFBO> jsonDocumentOrdersFBO = jToken.ToObject<List<JsonDocumentOrdersFBO>>();
            return jsonDocumentOrdersFBO;
        }

        public static string GetBodyRequestOrdersFBO(DateTime ordersDate)
        {
            JsonDocumentBodyOrdersFBO jsonBodyListFBO = new JsonDocumentBodyOrdersFBO
            {
                Dir = "asc",
                Limit = 999,
                Offset = 0,
                Translit = true,
                Filter = new JsonDocumentBodyOrdersFBO_Filter()
                {
                    Since = ordersDate.Date.ToString(_ozonFormatDate),
                    To = ordersDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(_ozonFormatDate)
                },
                With = new JsonDocumentBodyOrdersFBO_With()
                {
                    Analytics_data = true,
                    Financial_data = true
                }
            };
            string result = JsonConvert.SerializeObject(jsonBodyListFBO);
            return result;
        }

        public static string GetBodyMediaCompay(DateTime dateTime)
        {
            JsonDcumentBodyMediaCompany jsonMediaProductsData = new JsonDcumentBodyMediaCompany
            {
                Page = 1,
                PageSize = 100,
                PaymentMethod = new List<object>(),
                Placement = new List<object>(),
                Search = "",
                Sort = new List<JsonDcumentBodyMediaCompany_Sort>()
                {
                    new JsonDcumentBodyMediaCompany_Sort
                    {
                        Desc = false,
                        Field = "SORT_FIELD_STATUS"
                    }
                },
                StatsFrom = dateTime.Date.ToString(_ozonFullFormaDate),
                StatsTo = dateTime.Date.ToString(_ozonFullFormaDate),
                Status = new List<object>()
            };
            string result = JsonConvert.SerializeObject(jsonMediaProductsData);
            return result;
        }

        public static string GetBpdyMediaProducts(DateTime dateTime)
        {
            JsonDocumeentBodyMediaPoducts jsonMediaData = new JsonDocumeentBodyMediaPoducts
            {
                Page = 1,
                PageSize = 100,
                Search = "",
                Sort = new List<JsonDocumeentBodyMediaPoducts_Sort>()
                {
                    new JsonDocumeentBodyMediaPoducts_Sort
                    {
                        Field = "SORT_FIELD_STATUS",
                        Desc = false
                    }
                },
                Status = new List<string>
                {
                    "SERVING_STATE_RUNNING",
                    "SERVING_STATE_STOPPED",
                    "SERVING_STATE_MODERATION_IN_PROGRESS",
                    "SERVING_STATE_PLANNED",
                    "SERVING_STATE_INACTIVE"
                },
                StatsFrom = dateTime.Date.ToString(_ozonFullFormaDate),
                StatsTo = dateTime.Date.ToString(_ozonFullFormaDate),
                ObjectType = new List<object>(),
                DisplayFields = new List<string>
                {
                    "LIST_FIELD_TITLE",
                    "LIST_FIELD_OBJECT_TYPE",
                    "LIST_FIELD_STATUS",
                    "LIST_FIELD_PRIORITY",
                    "LIST_FIELD_EXPENSE",
                    "LIST_FIELD_VIEWS",
                    "LIST_FIELD_CLICKS",
                    "LIST_FIELD_CTR",
                    "LIST_FIELD_AVG_BID",
                    "LIST_FIELD_AVG_VIEW_PRICE",
                    "LIST_FIELD_AVG_CLICK_PRICE",
                    "LIST_FIELD_ORDERS",
                    "LIST_FIELD_ORDERS_MONEY",
                    "LIST_FIELD_DRR",
                    "LIST_FIELD_DAILY_BUDGET",
                    "LIST_FIELD_BUDGET"
                }

            };
            string result = JsonConvert.SerializeObject(jsonMediaData);
            return result;
        }

        public static void UpdateCostPrices(List<object> costPriceFotDay)
        {
            JsonDocumentCostPrices.Add(costPriceFotDay);
            string costPricesData = JsonConvert.SerializeObject(JsonDocumentCostPrices);
            FileManager.SaveCostPricesFile(costPricesData);
        }

        private static JsonDocumentTokens GetJsonDocumentTokens()
        {
            JToken jToken = JToken.Parse(FileManager.TokensDocument);
            JsonDocumentTokens tokens = jToken.ToObject<JsonDocumentTokens>();
            return tokens;
        }
        public static List<JsonDocumentChamping> ConverToJsonDocumentChamping(string data)
        {
            JToken jToken = JToken.Parse(data)["campaigns"];
            List<JsonDocumentChamping> ordersListFBO = jToken.ToObject<List<JsonDocumentChamping>>();
            return ordersListFBO;
        }

        private static List<List<object>> GetCostPrices()
        {
            List<List<object>> costPrices;
            if(FileManager.CostPricesDocument != string.Empty)
            {
                JToken jToken = JToken.Parse(FileManager.CostPricesDocument);
                costPrices = jToken.ToObject<List<List<object>>>();
            }
            else
            {
                costPrices = new List<List<object>>();
            }
            return costPrices;
        }

    }
}
