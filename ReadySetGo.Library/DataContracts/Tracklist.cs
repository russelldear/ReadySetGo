using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReadySetGo.Library.DataContracts
{
    public partial class Tracklist
    {
        [JsonProperty("uris")]
        public List<String> Uris { get; set; }
    }
}
