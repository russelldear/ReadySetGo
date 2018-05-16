using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadySetGo.Library;
using ReadySetGo.Library.DataContracts;
using ReadySetGo.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;

namespace ReadySetGo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<SetlistConfig> _config;
        private readonly ILogger<HomeController> _log;
        private readonly ISetlistBuilder _setlistBuilder;
        private readonly ISpotifyService _spotifyService;

        public HomeController(IOptions<SetlistConfig> config, ILogger<HomeController> log, ISetlistBuilder setlistBuilder, ISpotifyService spotifyService)
        {
            _config = config;
            _log = log;
            _setlistBuilder = setlistBuilder;
            _spotifyService = spotifyService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new HomeModel
            {
                ArtistName = "Taylor Swift",
                ConcertCount = 5
            });
        }

        [HttpPost]
        public ActionResult Post(HomeModel model, string returnUrl)
        {
            var playlistResult = _setlistBuilder.CreateSetlist(model.ArtistName, model.ConcertCount);

            HttpContext.Session.SetObjectAsJson("playlist", playlistResult);

            return View("Index", playlistResult.ToHomeModel());
        }

        [HttpPost]
        public ActionResult Spotify(HomeModel model, string returnUrl)
        {
            var host = "localhost";
            //var host = "russelldear.ddns.net";

            var port = "5001";
            //var port = "8999";

            var url = $"https://accounts.spotify.com/authorize/?" +
                $"client_id={_config.Value.ClientId}&" +
                $"response_type=code&" +
                $"redirect_uri=http%3A%2F%2F{host}%3A{port}%2Fhome%2Fcallback&" +
                $"scope=playlist-modify-private&" +
                $"state=34fFs29kd09";

            return Redirect(url);
        }

        public ActionResult Callback(string code, string state, string error)
        {
            TokenResponse token = null;

            var client = new HttpClient();

            var body = $"client_id={_config.Value.ClientId}&" +
                $"client_secret={_config.Value.ClientSecret}&" +
                $"grant_type=authorization_code&" +
                $"code={code}&" +
                $"redirect_uri=http%3A%2F%2Flocalhost%3A5001%2Fhome%2Fcallback";

            var response = client.PostAsync("https://accounts.spotify.com/api/token", new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

            using (var content = response.Content)
            {
                var result = content.ReadAsStringAsync().Result;

                token = JsonConvert.DeserializeObject<TokenResponse>(result);

                if (!string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    HttpContext.Session.SetObjectAsJson("tokenResponse", token);
                }
            }

            var playlist = HttpContext.Session.GetObjectFromJson<PlaylistResult>("playlist");

            playlist.Url = _spotifyService.CreatePlaylist(token, playlist);

            return View("Index", playlist.ToHomeModel());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
