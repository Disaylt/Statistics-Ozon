using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Statistics_Ozon.JsonDocuments
{
    public class JsonDocumentBodyOrdersFBO
    {
        [JsonProperty("dir")]
        public string Dir { get; set; }

        [JsonProperty("filter")]
        public JsonDocumentBodyOrdersFBO_Filter Filter { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("translit")]
        public bool Translit { get; set; }

        [JsonProperty("with")]
        public JsonDocumentBodyOrdersFBO_With With { get; set; }
    }
    public class JsonDocumentBodyOrdersFBO_Filter
    {
        [JsonProperty("since")]
        public string Since { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
    public class JsonDocumentBodyOrdersFBO_With
    {
        [JsonProperty("analytics_data")]
        public bool Analytics_data { get; set; }

        [JsonProperty("financial_data")]
        public bool Financial_data { get; set; }
    }
}
