using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadySetGo.Library;
using ReadySetGo.Models;

namespace ReadySetGo.Controllers
{
    public class HomeController : Controller
    {
        readonly ISetlistBuilder _setlistBuilder;

        public HomeController(ISetlistBuilder setlistBuilder)
        {
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
            var playlistResult = _setlistBuilder.CreateSetlist(model.ArtistName, model.ConcertCount);

            return View("Index", new HomeModel
            {
                ArtistName = WebUtility.UrlDecode(playlistResult.ArtistName),
                ConcertCount = model.ConcertCount,
                ActualCount = playlistResult.ActualCount,
                Songs = playlistResult.Songs
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
