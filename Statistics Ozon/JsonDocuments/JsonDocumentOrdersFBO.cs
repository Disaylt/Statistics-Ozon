using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Statistics_Ozon.JsonDocuments
{
    public class JsonDocumentOrdersFBO
    {
        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("products")]
        public List<JsonDocumentOrdersFBO_Products> Products { get; set; }
    }
    public class JsonDocumentOrdersFBO_Products
    {
        [JsonProperty("sku")]
        public long SKU { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
