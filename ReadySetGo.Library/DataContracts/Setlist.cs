using System;
using Newtonsoft.Json;

namespace ReadySetGo.Library.DataContracts
{
    public partial class Setlist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        [JsonProperty("eventDate")]
        public string EventDate { get; set; }

        [JsonProperty("lastUpdated")]
        public string LastUpdated { get; set; }

        [JsonProperty("artist")]
        public Artist Artist { get; set; }

        [JsonProperty("venue")]
        public Venue Venue { get; set; }

        [JsonProperty("tour")]
        public Tour Tour { get; set; }

        [JsonProperty("sets")]
        public SetlistSets SetlistSets { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class SetlistSets
    {
        [JsonProperty("set")]
        public Set[] Sets { get; set; }
    }

    public partial class Set
    {
        [JsonProperty("song")]
        public Song[] Songs { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class Song
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tape", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tape { get; set; }

        [JsonProperty("cover", NullValueHandling = NullValueHandling.Ignore)]
        public Artist Cover { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public string Info { get; set; }

        [JsonProperty("with", NullValueHandling = NullValueHandling.Ignore)]
        public Artist With { get; set; }

        public int DuplicateCount { get; set; }
    }

    public partial class Tour
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Venue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class City
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("stateCode")]
        public string StateCode { get; set; }

        [JsonProperty("coords")]
        public Coords Coords { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }
    }

    public partial class Coords
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("long")]
        public double Long { get; set; }
    }

    public partial class Country
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
