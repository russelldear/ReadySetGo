using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReadySetGo.Library.DataContracts
{
    public class SearchArtistResponse
    {
        [JsonProperty("artist")]
        public List<Artist> Artists { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("itemsPerPage")]
        public long ItemsPerPage { get; set; }
    }
}
