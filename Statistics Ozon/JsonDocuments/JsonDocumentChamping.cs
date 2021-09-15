using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Statistics_Ozon.JsonDocuments
{
    public class JsonDocumentChamping
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("metrics")]
        public JsonDocumentChamping_Metrics Metrics { get; set; }
    }

    public class JsonDocumentChamping_Metrics
    {
        [JsonProperty("expense")]
        public string Expense { get; set; }
    }
}
