using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Statistics_Ozon.JsonDocuments
{
    public class JsonDocumentTokens
    {
        [JsonProperty("googleApplicationName")]
        public string GoogleeApplicationName { get; set; }

        [JsonProperty("googltTableIdOfStatistics")]
        public string GoogltTableIdOfStatistics { get; set; }

        [JsonProperty("googleTableIdOfExpenses")]
        public string GoogleTableIdOfExpenses { get; set; }

        [JsonProperty("ozonGreyToken")]
        public string OzonGreyToken { get; set; }

        [JsonProperty("ozonApiKey")]
        public string OzonApiKey { get; set; }

        [JsonProperty("ozonClientId")]
        public string OzonClientId { get; set; }
    }
}
