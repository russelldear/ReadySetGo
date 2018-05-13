using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReadySetGo.Library.DataContracts
{
    public class SetlistResponse
    {
        [JsonProperty("setlist")]
        public List<Setlist> Setlists { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("itemsPerPage")]
        public long ItemsPerPage { get; set; }
    }
}
