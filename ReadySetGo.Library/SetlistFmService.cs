using System;
using System.Net.Http;
using ReadySetGo.Library.DataContracts;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace ReadySetGo.Library
{
    public class SetlistFmService : ISetlistFmService
    {
        private const string SetlistFmApiKey = "0e72db40-e178-4cf7-afbc-6ba732e2a23e";

        private static readonly HttpClient Client = new HttpClient();

        public SetlistFmService()
        {
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("x-api-key", SetlistFmApiKey);
        }

        public Artist SearchArtist(string artistName)
        {
            var response = Client.GetStringAsync("https://api.setlist.fm/rest/1.0/search/artists?sort=relevance&artistName=" + artistName).Result;

            var searchArtistResponse = JsonConvert.DeserializeObject<SearchArtistResponse>(response);

            return searchArtistResponse.Artists.First();
        }

        public List<Setlist> GetSetlists(string mbid)
        {
            var response = Client.GetStringAsync($"https://api.setlist.fm/rest/1.0/artist/{mbid}/setlists").Result;

            var setlistResponse = JsonConvert.DeserializeObject<SetlistResponse>(response);

            return setlistResponse.Setlists;
        }
    }
}
