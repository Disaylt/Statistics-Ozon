using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Statistics_Ozon.JsonDocuments
{
    public class JsonDocumeentBodyMediaPoducts
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("sort")]
        public List<JsonDocumeentBodyMediaPoducts_Sort> Sort { get; set; }

        [JsonProperty("status")]
        public List<string> Status { get; set; }

        [JsonProperty("statsFrom")]
        public string StatsFrom { get; set; }

        [JsonProperty("statsTo")]
        public string StatsTo { get; set; }

        [JsonProperty("objectType")]
        public List<object> ObjectType { get; set; }

        [JsonProperty("displayFields")]
        public List<string> DisplayFields { get; set; }
    }

    public class JsonDocumeentBodyMediaPoducts_Sort
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("desc")]
        public bool Desc { get; set; }
    }
}
