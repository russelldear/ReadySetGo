using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReadySetGo.Library;
using ReadySetGo.Library.DataContracts;
using ReadySetGo.Models;

namespace ReadySetGo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _log;

        private readonly ISetlistBuilder _setlistBuilder;

        public HomeController(ILogger<HomeController> log, ISetlistBuilder setlistBuilder)
        {
            _log = log;
            _setlistBuilder = setlistBuilder;
        }

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
            _log.LogInformation("Got here.");

            var playlistResult = _setlistBuilder.CreateSetlist(model.ArtistName, model.ConcertCount);

            if (playlistResult.ArtistFound.HasValue && playlistResult.ArtistFound.Value == false)
            {
                return View("Index", new HomeModel
                {
                    ArtistName = WebUtility.UrlDecode(playlistResult.ArtistName),
                    ConcertCount = model.ConcertCount,
                    ArtistFound = false
                });
            }

            if (playlistResult.SongsFound.HasValue && playlistResult.SongsFound.Value == false)
            {
                return View("Index", new HomeModel
                {
                    ArtistName = WebUtility.UrlDecode(playlistResult.ArtistName),
                    ConcertCount = model.ConcertCount,
                    SongsFound = false
                });
            }

            return View("Index", new HomeModel
            {
                ArtistName = WebUtility.UrlDecode(playlistResult.ArtistName),
                ConcertCount = model.ConcertCount,
                ActualCount = playlistResult.ActualCount,
                Songs = playlistResult.Songs,
                ArtistFound = true,
                SongsFound = true
            });
        }

        [HttpPost]
        public ActionResult Spotify(HomeModel model, string returnUrl)
        {
            var url = $"https://accounts.spotify.com/authorize/?" +
                "client_id=4b0fcb4ba28842529cc2cf6a001d26ab&" +
                "response_type=code&" +
                "redirect_uri=http%3A%2F%2Flocalhost%3A5001%2Fhome%2Fcallback&" +
                "scope=user-read-private%20user-read-email&" +
                "state=34fFs29kd09";

            return Redirect(url);
        }

        public ActionResult Callback(string code, string state, string error)
        {
            var client = new HttpClient();

            var body = $"client_id={"4b0fcb4ba28842529cc2cf6a001d26ab"}&client_secret={""}&grant_type=authorization_code&code={code}&redirect_uri=http%3A%2F%2Flocalhost%3A5001%2Fhome%2Fcallback";

            var response = client.PostAsync("https://accounts.spotify.com/api/token", new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

            using (var content = response.Content)
            {
                var result = content.ReadAsStringAsync().Result;

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
            }

            return Ok();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
