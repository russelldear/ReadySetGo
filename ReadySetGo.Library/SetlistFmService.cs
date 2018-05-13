using System.Net.Http;
using ReadySetGo.Library.DataContracts;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ReadySetGo.Library
{
    public class SetlistFmService : ISetlistFmService
    {
        private const string SetlistFmApiKey = "0e72db40-e178-4cf7-afbc-6ba732e2a23e";

        private static readonly HttpClient Client = new HttpClient();

        public SetlistFmService()
        {
            if (!Client.DefaultRequestHeaders.Contains("x-api-key"))
            {
                Client.DefaultRequestHeaders.Add("Accept", "application/json");
                Client.DefaultRequestHeaders.Add("x-api-key", SetlistFmApiKey);
            }
        }

        public Artist SearchArtist(string artistName)
        {
            var response = Client.GetStringAsync("https://api.setlist.fm/rest/1.0/search/artists?sort=relevance&artistName=" + artistName).Result;

            var searchArtistResponse = JsonConvert.DeserializeObject<SearchArtistResponse>(response);

            return searchArtistResponse.Artists.First();
        }

        public List<Setlist> GetSetlists(string mbid, int concertCount, out int actualCount)
        {
            var result = new List<Setlist>();
                
            var response = Client.GetStringAsync($"https://api.setlist.fm/rest/1.0/artist/{mbid}/setlists").Result;

            var setlistResponse = JsonConvert.DeserializeObject<SetlistResponse>(response);

            result.AddRange(setlistResponse.Setlists);

            var totalConcerts = setlistResponse.Total;

            var pageCounter = 1;

            while (result.Count < concertCount && (pageCounter * 20) < totalConcerts)
            {
                pageCounter++;

                response = Client.GetStringAsync($"https://api.setlist.fm/rest/1.0/artist/{mbid}/setlists?p={pageCounter}").Result;

                setlistResponse = JsonConvert.DeserializeObject<SetlistResponse>(response);

                result.AddRange(setlistResponse.Setlists);
            }

            actualCount = Math.Min(result.Count, concertCount);

            return result.Take(concertCount).ToList();
        }
    }
}
