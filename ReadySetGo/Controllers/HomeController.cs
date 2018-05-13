using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadySetGo.Library;
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
