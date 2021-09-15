using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Statistics_Ozon.JsonDocuments
{
    public class JsonDcumentBodyMediaCompany
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("sort")]
        public List<JsonDcumentBodyMediaCompany_Sort> Sort { get; set; }

        [JsonProperty("status")]
        public List<object> Status { get; set; }

        [JsonProperty("statsFrom")]
        public string StatsFrom { get; set; }

        [JsonProperty("statsTo")]
        public string StatsTo { get; set; }

        [JsonProperty("placement")]
        public List<object> Placement { get; set; }

        [JsonProperty("paymentMethod")]
        public List<object> PaymentMethod { get; set; }
    }
    public class JsonDcumentBodyMediaCompany_Sort
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("desc")]
        public bool Desc { get; set; }
    }
}
