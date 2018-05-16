using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public class SpotifyService : ISpotifyService
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        private readonly HttpClient _client;

        public SpotifyService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _client = new HttpClient();

            _client.BaseAddress = new Uri("https://api.spotify.com/v1");
        }

        public string CreatePlaylist(TokenResponse token, PlaylistResult playlist)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");

            var user = GetUser();

            var createdPlaylist = CreateNew(user);

            return createdPlaylist.Href;
        }

        private SpotifyUser GetUser()
        {
            var response = _client.GetAsync("/v1/me").Result;

            var user = JsonConvert.DeserializeObject<SpotifyUser>(response.Content.ReadAsStringAsync().Result);
            return user;
        }

        private SpotifyPlaylist CreateNew(SpotifyUser user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"/v1/users/{user.Id}/playlists")
            {
                Content = new StringContent("{\"name\":\"A New Playlist\", \"public\":false}",
                                                Encoding.UTF8,
                                                "application/json")
            };

            var response = _client.SendAsync(request).Result;

            var createdPlaylist = JsonConvert.DeserializeObject<SpotifyPlaylist>(response.Content.ReadAsStringAsync().Result);

            return createdPlaylist;
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
